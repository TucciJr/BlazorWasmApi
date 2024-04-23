using BlazorWasmApi.Models.Dtos;

namespace BlazorWasmApi.Web.Services.Contracts;

public interface IManageCartItemsLocalStorageService
{
    Task<IEnumerable<CartItemDto>> GetCollection();
    Task SaveCollection(IEnumerable<CartItemDto> cartItemDtos);
    Task RemoveCollection();
}
