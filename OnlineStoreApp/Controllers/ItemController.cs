using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreApp.Interfaces;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IValidator<Item> _validator;

        public ItemController(IItemService itemService, IValidator<Item> validator)
        {
            _itemService = itemService;
            _validator = validator;
        }

        [Route("createItem")]
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateItem(Item newItem)
        {
            try
            {
                if (newItem == null)
                    return BadRequest("Supply item to be created.");

                ValidationResult result = _validator.Validate(newItem);
                if (!result.IsValid)
                    return BadRequest(result);

                var createdItem = await _itemService.CreateItemAsync(newItem);

                return Created("GetItem", createdItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("{id}")]
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItem(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Please provide valid id.");

                var foundItem = await _itemService.GetItemAsync(id);

                if (foundItem != null)
                    return Ok(foundItem);
                else
                    return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("getAllItems")]
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var listOfItems = await _itemService.GetAllItemsAsync();

                if (listOfItems != null)
                    return Ok(listOfItems);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("update/{id}")]
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItem(int id, Item updatedItem)
        {
            try
            {
                if (id != updatedItem.ItemId)
                {
                    // _logger.LogWarning("API operation failed: UpdateItem (ID in URL does not match ID in request body)");
                    return BadRequest("Item ID in the URL does not match the ID in the request body.");
                }

                if (id == 0 || updatedItem == null)
                    return BadRequest();

                ValidationResult result = _validator.Validate(updatedItem);
                if (!result.IsValid)
                    return BadRequest(result);

                var updateResult = await _itemService.UpdateItemAsync(id, updatedItem);
                return Ok(updateResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Provide valid item Id.");

                var isDeleted = await _itemService.DeleteItemAsync(id);

                if (isDeleted)
                    return NoContent();
                else
                    return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("uploadImage/{id}")]
        [HttpPost]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadImage(int id, [FromForm] IFormFile image)
        {
            try
            {
                var isUploaded = await _itemService.UploadImageAsync(id, image);

                if (isUploaded)
                    return NoContent();
                else
                    return NotFound();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [Route("image/{id}")]
        [HttpGet]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemImage(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Please provide a valid item id.");

                var item = await _itemService.GetImageAsync(id);

                if (item == null || item.Data == null || item.Data.Length == 0)
                    return NotFound($"ItemId {id} image not found.");

                return File(item.Data, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
