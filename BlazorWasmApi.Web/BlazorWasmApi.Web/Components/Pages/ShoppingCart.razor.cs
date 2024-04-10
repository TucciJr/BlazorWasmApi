using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorWasmApi.Web.Components.Pages;

public partial class ShoppingCart : ComponentBase
{
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }
    public IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
    public string ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    protected async Task DeleteCartItem_Click(int id)
    {
        var cartItemDto = await ShoppingCartService.DeleteItem(id);

        ShoppingCartItems = ShoppingCartItems.Where(x => x.Id != id);
    }

    private CartItemDto GetCartItem(int id)
    {
        return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
    }

    private void RemoveCartItem(int id)
    {
        var cartItemDto = GetCartItem(id);
    }
}
