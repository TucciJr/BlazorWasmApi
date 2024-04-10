using BlazorWasmApi.Models.Dtos;
using Microsoft.AspNetCore.Components;

namespace BlazorWasmApi.Web.Components.Pages;

public class DisplayProductsBase : ComponentBase
{
    [Parameter]
    public IEnumerable<ProductDto> Products { get; set; }
}
