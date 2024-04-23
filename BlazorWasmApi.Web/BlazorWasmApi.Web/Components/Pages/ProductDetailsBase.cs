using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorWasmApi.Web.Components.Pages;

public class ProductDetailsBase : ComponentBase
{
    [Parameter]
    public int Id { get; set; }
    [Inject]
    public IProductService ProductService { get; set; }
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
    [Inject]
    public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

    public ProductDto Product { get; set; }

    public string ErrorMessage { get; set; }

    public List<CartItemDto> ShoppingCartItems { get; set; }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            ShoppingCartItems = (await ManageCartItemsLocalStorageService.GetCollection()).ToList();

            Product = await GetProductById(Id);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
    {
        try
        {
            var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

            if (cartItemDto != null)
            {
                ShoppingCartItems.Add(cartItemDto);

                await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
            }

            NavigationManager.NavigateTo("/ShoppingCart");

        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    private async Task<ProductDto> GetProductById(int id)
    {
        var productDtos = await ManageProductsLocalStorageService.GetCollection();

        if (productDtos != null)
        {
            var productDto = productDtos.SingleOrDefault(x => x.Id == id);

            return productDto; 
        }

        return null;
    }
}
