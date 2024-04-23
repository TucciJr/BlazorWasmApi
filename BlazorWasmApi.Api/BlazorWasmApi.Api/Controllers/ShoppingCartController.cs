using BlazorWasmApi.Api.Entities;
using BlazorWasmApi.Api.Extensions;
using BlazorWasmApi.Api.Repositories.Contracts;
using BlazorWasmApi.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWasmApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        [Route("{userId}/GetItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
        {
            try
            {
                var cartItems = await shoppingCartRepository.GetItems(userId);

                if (cartItems == null)
                {
                    return NoContent();
                }

                var cartItemDto = cartItems.ConvertToDto();

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {
            try
            {
                var cartItem = await shoppingCartRepository.GetItem(id);

                if (cartItem == null)
                {
                    return NoContent();
                }

                var cartItemDto = cartItem.ConvertToDto();

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var newCartItem = await shoppingCartRepository.AddItem(cartItemToAddDto);

                if (newCartItem == null)
                {
                    return NoContent();
                }

                var product = await productRepository.GetItem(newCartItem.ProductId);

                if (product == null)
                {
                    throw new Exception($"Somethig went wrong when attempting to retrieve product #{newCartItem.ProductId}");
                }

                var newCartItemDto = newCartItem.ConvertToDto();

                return CreatedAtAction(
                    nameof(GetItem),
                    new { id = newCartItemDto.Id },
                    newCartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
        {
            try
            {
                var cartItem = await shoppingCartRepository.DeleteItem(id);

                if (cartItem == null)
                {
                    return NotFound();
                }

                var cartItemDto = cartItem.ConvertToDto();

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);

                if (cartItem == null)
                {
                    return NotFound();
                }

                var cartItemDto = cartItem.ConvertToDto();
                return base.Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
