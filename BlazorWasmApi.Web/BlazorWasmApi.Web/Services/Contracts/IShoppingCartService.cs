﻿using BlazorWasmApi.Models.Dtos;

namespace BlazorWasmApi.Web.Services.Contracts;

public interface IShoppingCartService
{
    Task<IEnumerable<CartItemDto>> GetItems(int userId);
    Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
    Task<CartItemDto> DeleteItem(int id);
    Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);

    event Action<int> OnShoppingCartChanged;
    void RaiseEventoOnShoppingCartChanged(int totalQty);
}
