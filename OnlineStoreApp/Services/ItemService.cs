using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Data;
using OnlineStoreApp.Interfaces;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework database context</param>
        public ItemService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new item in the database.
        /// </summary>
        /// <param name="newItem">The item to be created.</param>
        /// <returns>The newly created item.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<Item> CreateItemAsync(Item newItem)
        {
            try
            {
                _dbContext.Items.Add(newItem);
                await _dbContext.SaveChangesAsync();
                return newItem;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves the item from the database.
        /// </summary>
        /// <param name="id">The item to be looked-up.</param>
        /// <returns>The found item.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<Item> GetItemAsync(int itemId)
        {
            try
            {
                var item = await _dbContext.Items.FindAsync(itemId);
                return item;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves all the items from the database.
        /// </summary>
        /// <returns>The list items.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<List<Item>> GetAllItemsAsync()
        {
            try
            {
                var allItems = await _dbContext.Items.ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates an item in the database.
        /// </summary>
        /// <param name="id">The item to be updated.</param>
        /// <param name="item">Updated <see cref="Item"/> details.</param>
        /// <returns>The updated item.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<Item> UpdateItemAsync(int id, Item updatedItem)
        {
            try
            {
                var existingItem = await _dbContext.Items.FindAsync(id);

                if (existingItem == null)
                    throw new InvalidOperationException($"Item with ID {id} not found.");

                _dbContext.Entry(existingItem).CurrentValues.SetValues(updatedItem);
                await _dbContext.SaveChangesAsync();

                return existingItem;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="id">The id of the item to be deleted.</param>
        /// <returns>True if item is removed; False if the item does not exist.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var itemToDelete = await _dbContext.Items.FindAsync(id);

                if (itemToDelete == null)
                    return false;
                else
                {
                    _dbContext.Items.Remove(itemToDelete);
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Uploads an image to the database.
        /// </summary>
        /// <param name="id">The id of the linked item.</param>
        /// <param name="image">The file to be stored away.</param>
        /// <returns>True is uploaded; False is the item does not exist.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<bool> UploadImageAsync(int id, [FromForm] IFormFile image)
        {
            try
            {
                if (image != null && image.Length > 0)
                {
                    // Convert image to a byte[] to be stored.
                    var newImage = new ItemImage();

                    var foundItem = await _dbContext.Items.FindAsync(id);
                    if (foundItem == null)
                        return false;

                    newImage.ItemID = id;
                    using (var memoryStream = new MemoryStream())
                    {
                        image.CopyTo(memoryStream);
                        newImage.Data = memoryStream.ToArray();
                    }

                    _dbContext.Images.Add(newImage);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves the item's iamge from the database.
        /// </summary>
        /// <param name="id">The item id's image to look-up.</param>
        /// <returns>The found image.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<ItemImage> GetImageAsync(int id)
        {
            try
            {
                var item = await _dbContext.Images.Where(i => i.ItemID == id).FirstOrDefaultAsync();
                return item;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
