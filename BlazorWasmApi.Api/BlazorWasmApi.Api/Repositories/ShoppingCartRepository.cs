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
            var cartItem = await (from product in shopOnlineDbContext.Products
                              where product.Id == cartItemToAddDto.ProductId
                              select new CartItem
                              {
                                  CartId = cartItemToAddDto.CartId,
                                  ProductId = product.Id,
                                  Qty = cartItemToAddDto.Qty
                              }).SingleOrDefaultAsync();

            if (cartItem !=  null)
            {
                var result = await shopOnlineDbContext.CartItems.AddAsync(cartItem);
                await shopOnlineDbContext.SaveChangesAsync();

                return result.Entity;
            }
        }
        else
        {
            var cartItem = await shopOnlineDbContext.CartItems.SingleOrDefaultAsync(x => x.CartId == cartItemToAddDto.CartId && x.ProductId == cartItemToAddDto.ProductId);

            if (cartItem != null)
            {
                cartItem.Qty++;
                await shopOnlineDbContext.SaveChangesAsync();

                return cartItem;
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
        return await shopOnlineDbContext
            .CartItems
            .Include(x => x.Cart)
            .Include(x => x.Product)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<CartItem>> GetItems(int userId)
    {
        var cartItems = await shopOnlineDbContext.CartItems
            .Include(x => x.Product)
            .Where(x => x.Cart.UserId == userId)
            .ToListAsync();

        return cartItems;
    }

    public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        var item = await GetItem(id);

        if (item != null)
        {
            item.Qty = cartItemQtyUpdateDto.Qty;
            await shopOnlineDbContext.SaveChangesAsync();

            return item;
        }

        return null;
    }
}
