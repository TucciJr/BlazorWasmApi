using BlazorWasmApi.Models.Dtos;

namespace BlazorWasmApi.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetItems();
    Task<ProductDto> GetItem(int id);
}
