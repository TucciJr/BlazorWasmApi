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
        try
        {
            var products = await productRepository.GetItems();

            if (products == null)
            {
                return NotFound();
            }

            var productsDto = products.ConvertToDto();

            return Ok(productsDto);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetItem(int id)
    {
        try
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
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
        }
    }

    [HttpGet]
    [Route(nameof(GetProductCategories))]
    public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
    {
        try
        {
            var productCategories = await productRepository.GetCategories();

            var productCategoriesDtos = productCategories.ConvertToDto();

            return Ok(productCategoriesDtos);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
        }
    }

    [HttpGet]
    [Route("{categoryId}/GetItemsByCategory")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
    {
        try
        {
            var products = await productRepository.GetItemsByCategory(categoryId);

            if (products == null)
            {
                return NotFound();
            }

            var productsDto = products.ConvertToDto();

            return Ok(productsDto);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
        }
    }
}
