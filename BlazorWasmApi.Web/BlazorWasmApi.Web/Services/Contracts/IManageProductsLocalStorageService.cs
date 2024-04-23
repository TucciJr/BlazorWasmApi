using BlazorWasmApi.Models.Dtos;

namespace BlazorWasmApi.Web.Services.Contracts
{
    public interface IManageProductsLocalStorageService
    {
        Task<IEnumerable<ProductDto>> GetCollection();
        Task RemoveCollection();
    }
}
