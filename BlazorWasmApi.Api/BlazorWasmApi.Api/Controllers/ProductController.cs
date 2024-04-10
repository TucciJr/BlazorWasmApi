using BlazorWasmApi.Api.Extensions;
using BlazorWasmApi.Api.Repositories.Contracts;
using BlazorWasmApi.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWasmApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository productRepository;

    public ProductController(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
    {
        var products = await productRepository.GetItems();
        var productCategories = await productRepository.GetCategories();

        if (products == null || productCategories == null)
        {
            return NotFound();
        }

        var productsDto = products.ConvertToDto(productCategories);

        return Ok(productsDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetItem(int id)
    {
        var product = await productRepository.GetItem(id);

        if (product == null)
        {
            return BadRequest();
        }
        var productCategory = await productRepository.GetCategory(product.CategoryId);
        var productDto = product.ConvertToDto(productCategory);

        return Ok(productDto);
    }
}
