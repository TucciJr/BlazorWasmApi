using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;

namespace BlazorWasmApi.Web.Components.Pages;

public partial class Checkout
{
    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

    protected int TotalQty { get; set; }

    protected decimal PaymentAmount { get; set; }

    protected string PaymentDescription { get; set; }

    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);

            if (ShoppingCartItems != null)
            {
                Guid orderGuid = Guid.NewGuid();

                PaymentAmount = ShoppingCartItems.Sum(x => x.TotalPrice);
                TotalQty = ShoppingCartItems.Sum(x => x.Qty);
                PaymentDescription = $"O_{HardCoded.UserId}_{orderGuid}";
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("initPayPalButton");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
