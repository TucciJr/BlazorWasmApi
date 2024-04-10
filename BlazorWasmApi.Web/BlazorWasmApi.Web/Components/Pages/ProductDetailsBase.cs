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

    public ProductDto Product { get; set; }

    public string ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Product = await ProductService.GetItem(Id);
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

            NavigationManager.NavigateTo("/ShoppingCart");

        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}
