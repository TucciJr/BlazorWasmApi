using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services;
using BlazorWasmApi.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorWasmApi.Web.Components.Pages;

public partial class Products
{
    [Inject]
    public IProductService ProductService { get; set; }
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }
    [Inject]
    public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
    [Inject]
    public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

    public IEnumerable<ProductDto> ProductDtos { get; set; }
    public NavigationManager NavigationManager { get; set; }
    public string ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await ClearLocalStorage();

            ProductDtos = await ManageProductsLocalStorageService.GetCollection();//ProductService.GetItems()

            var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();//ShoppingCartService.GetItems(HardCoded.UserId);

            var totalQty = shoppingCartItems.Sum(x => x.Qty);

            ShoppingCartService.RaiseEventoOnShoppingCartChanged(totalQty);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
    {
        return from product in ProductDtos
               group product by product.CategoryId into productByCategoryGroup
               orderby productByCategoryGroup.Key
               select productByCategoryGroup;
    }

    protected string GetCategoryName(IGrouping<int, ProductDto> productByCategoryGroup)
    {
        return productByCategoryGroup.FirstOrDefault(p => p.CategoryId == productByCategoryGroup.Key).CategoryName;
    }

    private async Task ClearLocalStorage()
    {
        await ManageProductsLocalStorageService.RemoveCollection();
        await ManageCartItemsLocalStorageService.RemoveCollection();
    }
}
