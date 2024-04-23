using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace BlazorWasmApi.Web.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly HttpClient httpClient;

    public event Action<int> OnShoppingCartChanged;

    public ShoppingCartService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(CartItemDto);
                }

                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"HTTP Status: {response.StatusCode} - Message: {message}");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<CartItemDto>> GetItems(int userId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<CartItemDto>();
                }

                var cartItemDtos = await response.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>();

                return cartItemDtos;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"HTTP Status: {response.StatusCode} - Message: {message}");
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<CartItemDto> DeleteItem(int id)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/ShoppingCart/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }

            return default(CartItemDto);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        try
        {
            var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response = await httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }

            return default(CartItemDto);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void RaiseEventoOnShoppingCartChanged(int totalQty)
    {
        if (OnShoppingCartChanged != null)
        {
            OnShoppingCartChanged.Invoke(totalQty);
        }
    }
}
