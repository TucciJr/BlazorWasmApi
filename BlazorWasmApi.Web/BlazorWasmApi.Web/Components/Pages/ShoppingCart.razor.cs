using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWasmApi.Web.Components.Pages;

public partial class ShoppingCart
{
    [Inject]
    public IJSRuntime Js { get; set; }
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }
    [Inject]
    public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
    [Inject]
    public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

    public IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
    public string ErrorMessage { get; set; }
    public string TotalPrice { get; set; }
    public int TotalQuantity { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();// ShoppingCartService.GetItems(HardCoded.UserId);

            CartChanged();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    protected async Task DeleteCartItem_Click(int id)
    {
        var cartItemDto = await ShoppingCartService.DeleteItem(id);

        ShoppingCartItems = GetCartItems(id);

        await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);

        CartChanged();
    }

    protected async Task UpdateQtyCartItem_Click(int id, int qty)
    {
        try
        {
            if (qty > 0)
            {
                var updateItemDto = new CartItemQtyUpdateDto
                {
                    CartItemId = id,
                    Qty = qty
                };

                var cartItemDto = await ShoppingCartService.UpdateQty(updateItemDto);

                await UpdateItemTotalPrice(cartItemDto);

                CartChanged();

                await MakeUpdateQtyButtonVisible(id, false);
            }
            else
            {
                var item = ShoppingCartItems.FirstOrDefault(x => x.Id == id);

                if (item != null)
                {
                    item.Qty = 1;
                    item.TotalPrice = item.Price;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected async Task UpdateQty_Input(int id)
    {
        await MakeUpdateQtyButtonVisible(id, true);
    }

    private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
    {
        await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
    }

    private IEnumerable<CartItemDto> GetCartItems(int id)
    {
        return ShoppingCartItems.Where(x => x.Id != id);
    }

    private CartItemDto GetCartItem(int id)
    {
        return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
    }

    private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
    {
        var item = GetCartItem(cartItemDto.Id);

        if (item != null)
        {
            item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
        }

        await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
    }

    private void CalculateCartSummartyTotals()
    {
        SetTotalPrice();
        SetTotalQuantity();
    }

    private void SetTotalPrice()
    {
        TotalPrice = ShoppingCartItems.Sum(x => x.TotalPrice).ToString("C");
    }

    private void SetTotalQuantity()
    {
        TotalQuantity = ShoppingCartItems.Sum(x => x.Qty);
    }

    private void CartChanged()
    {
        CalculateCartSummartyTotals();

        ShoppingCartService.RaiseEventoOnShoppingCartChanged(TotalQuantity);
    }
}
