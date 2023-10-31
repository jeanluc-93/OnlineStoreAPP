using Microsoft.AspNetCore.Mvc;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Interfaces
{
    public interface IItemService
    {
        /// <summary>
        /// Creates a new item in the database.
        /// </summary>
        /// <param name="newItem">The item to be created.</param>
        /// <returns>The newly created item.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<Item> CreateItemAsync(Item newItem);

        /// <summary>
        /// Retrieves the item from the database.
        /// </summary>
        /// <param name="id">The item to be looked-up.</param>
        /// <returns>The found item.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<Item> GetItemAsync(int id);

        /// <summary>
        /// Retrieves all the items from the database.
        /// </summary>
        /// <returns>The list items. <see cref="Item"/></returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<List<Item>> GetAllItemsAsync();

        /// <summary>
        /// Updates an item in the database.
        /// </summary>
        /// <param name="id">The item to be updated.</param>
        /// <param name="item">Updated <see cref="Item"/> details.</param>
        /// <returns>The updated item.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<Item> UpdateItemAsync(int id, Item item);

        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="id">The id of the item to be deleted.</param>
        /// <returns>True if item is removed; False if the item does not exist.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<bool> DeleteItemAsync(int id);

        /// <summary>
        /// Uploads an image to the database.
        /// </summary>
        /// <param name="id">The id of the linked item.</param>
        /// <param name="image">The file to be stored away.</param>
        /// <returns>True is uploaded; False is the item does not exist.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<bool> UploadImageAsync(int id, [FromForm] IFormFile image);

        /// <summary>
        /// Retrieves the item's iamge from the database.
        /// </summary>
        /// <param name="id">The item id's image to look-up.</param>
        /// <returns>The found image.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<ItemImage> GetImageAsync(int id);
    }
}
