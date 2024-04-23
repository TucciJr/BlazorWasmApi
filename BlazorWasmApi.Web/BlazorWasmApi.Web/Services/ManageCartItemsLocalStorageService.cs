using Blazored.LocalStorage;
using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;

namespace BlazorWasmApi.Web.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IShoppingCartService shoppingCartService;

        private const string key = "CartItemCollection";

        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
        }
        public async Task<IEnumerable<CartItemDto>> GetCollection()
        {
            return await localStorageService.GetItemAsync<IEnumerable<CartItemDto>>(key)
                ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await localStorageService.RemoveItemAsync(key);
        }

        public async Task SaveCollection(IEnumerable<CartItemDto> cartItemDtos)
        {
            await localStorageService.SetItemAsync(key, cartItemDtos);
        }

        private async Task<IEnumerable<CartItemDto>> AddCollection()
        {
            var productCollection = await shoppingCartService.GetItems(HardCoded.UserId);

            if (productCollection != null)
            {
                await localStorageService.SetItemAsync(key, productCollection);
            }

            return productCollection;
        }
    }
}
