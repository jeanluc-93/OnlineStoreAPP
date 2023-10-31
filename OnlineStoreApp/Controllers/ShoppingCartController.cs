using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreApp.Interfaces;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Controllers
{
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUserService _userService;

        public ShoppingCartController(IValidator<Item> validator, IShoppingCartService shoppingCartService, IUserService userService)
        {
            _shoppingCartService = shoppingCartService;
            _userService = userService;
        }

        [Route("getShoppingCart")]
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetShoppingCart()
        {
            try
            {
                var userId = User.Identity.Name;
                var userExists = await _userService.CheckIfUserExists(userId);
                if (!userExists)
                    return NotFound("User not found.");

                var cart = await _shoppingCartService.GetShoppingCartAsync(userId);

                if (cart.Count > 0)
                    return Ok(cart);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("addToCart")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddItemToShoppingCart(CartRequest newItem)
        {
            try
            {
                if (newItem == null)
                    return BadRequest("Provide an item to add.");

                var userId = User.Identity.Name;
                var userExists = await _userService.CheckIfUserExists(userId);
                if (!userExists)
                    return NotFound("User not found.");

                var isAdded = await _shoppingCartService.AddToCartAsync(userId, newItem);

                if (isAdded)
                    return Ok(isAdded);
                else
                    return BadRequest("Failed to add item to the cart.");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("removeFromCart")]
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveItemFromShoppingCart(CartRequest removeItem)
        {
            try
            {
                if (removeItem == null)
                    return BadRequest("Provide an item to remove.");

                var userId = User.Identity.Name;
                var userExists = await _userService.CheckIfUserExists(userId);
                if (!userExists)
                    return NotFound("User not found.");

                var isRemoved = await _shoppingCartService.RemoveFromCartAsync(userId, removeItem);

                if (isRemoved)
                    return Ok(isRemoved);
                else
                    return BadRequest("Failed to add item to the cart.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
