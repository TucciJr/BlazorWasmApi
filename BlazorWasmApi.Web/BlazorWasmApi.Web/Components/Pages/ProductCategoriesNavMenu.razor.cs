using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorWasmApi.Web.Components.Pages
{
    public partial class ProductCategoriesNavMenu
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<ProductCategoryDto> ProductCategoriesDtos { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ProductCategoriesDtos = await ProductService.GetProductCategoryDtos();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
