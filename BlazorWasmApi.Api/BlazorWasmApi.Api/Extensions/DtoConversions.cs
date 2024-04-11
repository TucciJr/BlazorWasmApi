using BlazorWasmApi.Api.Entities;
using BlazorWasmApi.Models.Dtos;

namespace BlazorWasmApi.Api.Extensions;

public static class DtoConversions
{
    public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products, IEnumerable<ProductCategory> productCategories)
    {
        return (from product in products
                join productCategory in productCategories
                on product.CategoryId equals productCategory.Id
                select new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageURL = product.ImageURL,
                    Price = product.Price,
                    Qty = product.Qty,
                    CategoryId = product.CategoryId,
                    CategoryName = productCategory.Name
                }
            ).ToList();
    }

    public static ProductDto ConvertToDto(this Product product, ProductCategory productCategory)
    {
        return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageURL = product.ImageURL,
                    Price = product.Price,
                    Qty = product.Qty,
                    CategoryId = product.CategoryId,
                    CategoryName = productCategory.Name
                };
    }

    public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems)
    {
        return (from cartItem in cartItems
                select new CartItemDto
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.Product.Name,
                    ProductDescription = cartItem.Product.Description,
                    ProductImageURL = cartItem.Product.ImageURL,
                    Price = cartItem.Product.Price,
                    CartId = cartItem.CartId,
                    Qty = cartItem.Qty,
                    TotalPrice = cartItem.Qty * cartItem.Product.Price
                }).ToList();
    }

    public static CartItemDto ConvertToDto(this CartItem cartItem)
    {
        var cartItemDto = new CartItemDto
        {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            CartId = cartItem.CartId,
            Qty = cartItem.Qty,
            TotalPrice = cartItem.Qty * (cartItem.Product?.Price ?? 0) // Se Product for nulo, assume preço zero
        };

        if (cartItem.Product != null)
        {
            cartItemDto.ProductName = cartItem.Product.Name;
            cartItemDto.ProductDescription = cartItem.Product.Description;
            cartItemDto.ProductImageURL = cartItem.Product.ImageURL;
            cartItemDto.Price = cartItem.Product.Price;
        }

        return cartItemDto;
    }
}
