using BlazorWasmApi.Api.Data;
using BlazorWasmApi.Api.Entities;
using BlazorWasmApi.Api.Repositories.Contracts;
using BlazorWasmApi.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BlazorWasmApi.Api.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ShopOnlineDbContext shopOnlineDbContext;

    public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
    {
        this.shopOnlineDbContext = shopOnlineDbContext;
    }

    private async Task<bool> CartItemExists(int cartId, int product)
    {
        return await shopOnlineDbContext.CartItems.AnyAsync(x => x.CartId == cartId && x.ProductId == product);
    }

    public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
    {
        if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
        {
            var item = await (from product in shopOnlineDbContext.Products
                              where product.Id == cartItemToAddDto.ProductId
                              select new CartItem
                              {
                                  CartId = cartItemToAddDto.CartId,
                                  ProductId = product.Id,
                                  Qty = cartItemToAddDto.Qty
                              }).SingleOrDefaultAsync();

            if (item !=  null)
            {
                var result = await shopOnlineDbContext.CartItems.AddAsync(item);
                await shopOnlineDbContext.SaveChangesAsync();

                return result.Entity;
            }
        }

        return null;
    }

    public async Task<CartItem> DeleteItem(int id)
    {
        var cartItem = await shopOnlineDbContext.CartItems
            .FindAsync(id);

        if (cartItem != null)
        {
            shopOnlineDbContext.Remove(cartItem);
            await shopOnlineDbContext.SaveChangesAsync();
        }

        return cartItem;
    }

    public async Task<CartItem> GetItem(int id)
    {
        return await (from cart in shopOnlineDbContext.Carts
                      join cartItem in shopOnlineDbContext.CartItems
                      on cart.Id equals cartItem.CartId
                      select new CartItem
                      {
                          Id = cartItem.Id,
                          ProductId = cartItem.ProductId,
                          Qty = cartItem.Qty,
                          CartId = cartItem.CartId
                      }).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<CartItem>> GetItems(int userId)
    {
        var cartItems = await shopOnlineDbContext.CartItems
            .Include(x => x.Product)
            .Where(x => x.Cart.UserId == userId)
            .ToListAsync();

        return cartItems;
            
        return await (from cart in shopOnlineDbContext.Carts
                      join cartItem in shopOnlineDbContext.CartItems
                      on cart.Id equals cartItem.CartId
                      where cart.UserId == userId
                      select new CartItem
                      {
                          Id = cartItem.Id,
                          ProductId = cartItem.ProductId,
                          Qty = cartItem.Qty,
                          CartId = cartItem.CartId
                      }).ToListAsync();
    }

    public Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        throw new NotImplementedException();
    }
}
