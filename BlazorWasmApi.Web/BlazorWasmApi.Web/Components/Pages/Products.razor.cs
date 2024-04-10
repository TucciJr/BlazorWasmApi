using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorWasmApi.Web.Components.Pages;

public partial class Products : ComponentBase
{
    [Inject]
    public IProductService ProductService { get; set; }

    public IEnumerable<ProductDto> ProductDtos { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ProductDtos = await ProductService.GetItems();
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
}
