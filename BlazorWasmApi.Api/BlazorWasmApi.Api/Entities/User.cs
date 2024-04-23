namespace BlazorWasmApi.Api.Entities;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
