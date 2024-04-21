using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorWasmApi.Web.Components.Pages;

public partial class ProductsByCategory : ComponentBase
{
    [Parameter]
    public int CategoryId { get; set; }

    [Inject]
    public IProductService ProductService { get; set; }

    public IEnumerable<ProductDto> Products { get; set; }

    public string ErrorMessage { get; set; }

    public string CategoryName { get; private set; }

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            Products = await ProductService.GetItemsByCategory(CategoryId);

            if (Products != null && Products.Count() > 0)
            {
                var productDto = Products.FirstOrDefault(x => x.CategoryId == CategoryId);

                if (productDto != null)
                {
                    CategoryName = productDto.CategoryName;
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
}
