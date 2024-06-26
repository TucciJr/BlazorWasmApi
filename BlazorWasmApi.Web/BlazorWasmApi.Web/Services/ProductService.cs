﻿using BlazorWasmApi.Models.Dtos;
using BlazorWasmApi.Web.Services.Contracts;

namespace BlazorWasmApi.Web.Services
{
    public class ProductService(HttpClient httpClient) : IProductService
    {
        private readonly HttpClient httpClient = httpClient;

        public async Task<ProductDto> GetItem(int id)
        {
            var response = await httpClient.GetAsync($"api/Product/{id}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(ProductDto);
                }

                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            var response = await httpClient.GetAsync($"api/Product/");

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return Enumerable.Empty<ProductDto>();
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategoryDtos()
        {
            try
            {
                var response = await httpClient.GetAsync("api/Product/GetProductCategories");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductCategoryDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status Code: {response.StatusCode} - Message: {message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Product/{categoryId}/GetItemsByCategory");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status Code: {response.StatusCode} - Message: {message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
