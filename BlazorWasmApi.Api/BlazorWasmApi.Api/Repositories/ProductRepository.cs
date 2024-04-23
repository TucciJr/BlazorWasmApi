using BlazorWasmApi.Api.Data;
using BlazorWasmApi.Api.Entities;
using BlazorWasmApi.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BlazorWasmApi.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopOnlineDbContext shopOnlineDbContext;

    public ProductRepository(ShopOnlineDbContext ctx)
    {
        this.shopOnlineDbContext = ctx;
    }

    public async Task<IEnumerable<ProductCategory>> GetCategories()
    {
        var productCategories = await shopOnlineDbContext.ProductCategories.ToListAsync();

        return productCategories;
    }

    public async Task<ProductCategory> GetCategory(int id)
    {
        var category = await shopOnlineDbContext.ProductCategories
            .FindAsync(id);

        return category;
    }

    public async Task<Product> GetItem(int id)
    {
        var product = await shopOnlineDbContext.Products
            .FindAsync(id);

        return product;
    }

    public async Task<IEnumerable<Product>> GetItems()
    {
        var products = await shopOnlineDbContext.Products
            .Include(x => x.Category)
            .ToListAsync();

        return products;
    }


    public async Task<IEnumerable<Product>> GetItemsByCategory(int categoryId)
    {
        var products = await shopOnlineDbContext.Products
            .Include(x => x.Category)
            .Where(x => x.CategoryId == categoryId)
            .ToListAsync();

        return products;
    }
}
