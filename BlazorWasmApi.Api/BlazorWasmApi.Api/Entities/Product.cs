using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorWasmApi.Api.Entities;

public class Product
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImageURL { get; set; } = null!;

    public decimal Price { get; set; }

    public int Qty { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = [];

    public virtual ProductCategory Category { get; set; } = null!;
}
