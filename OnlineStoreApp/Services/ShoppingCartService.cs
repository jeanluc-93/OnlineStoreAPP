using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Data;
using OnlineStoreApp.Interfaces;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        ApplicationDbContext _dbContext;

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework database context</param>
        public ShoppingCartService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get the users cart.
        /// </summary>
        /// <param name="userId">User id to retrive cart for.</param>
        /// <returns>A list of all the items in the users shopping cart.</returns>
        public async Task<List<ShoppingCartItem>> GetShoppingCartAsync(string userId)
        {
            try
            {
                var shoppingCart = await _dbContext.ShoppingCartItems.Where(u => u.UserId.Equals(userId)).ToListAsync();
                return shoppingCart;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds an item to the users cart.
        /// </summary>
        /// <param name="userId">Users cart to add too.</param>
        /// <param name="newItem">Item to be added</param>
        /// <returns>True is item has been added.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<bool> AddToCartAsync(string userId, CartRequest newItem)
        {
            try
            {
                var existingItem = await _dbContext.ShoppingCartItems.Where(i => i.ItemId == newItem.ItemId).FirstOrDefaultAsync();

                // Create if new, else update quantity.
                if (existingItem == null)
                {
                    var cartItem = new ShoppingCartItem
                    {
                        ItemId = newItem.ItemId,
                        UserId = userId,
                        Quantity = newItem.Quantity
                    };

                    _dbContext.ShoppingCartItems.Add(cartItem);
                }
                else
                {
                    existingItem.Quantity = newItem.Quantity;                    
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Removes items from an users cart.
        /// </summary>
        /// <param name="userId">Users cart to be removed from.</param>
        /// <param name="removeItem">Details to update too.</param>
        /// <returns>True is item has been added; False if item is not found.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<bool> RemoveFromCartAsync(string userId, CartRequest removeItem)
        {
            try
            {
                var itemToRemove = await _dbContext.ShoppingCartItems.Where(u => u.UserId == userId).FirstOrDefaultAsync();
                if (itemToRemove == null)
                    return false;

                if (removeItem.Quantity == 0)
                    _dbContext.ShoppingCartItems.Remove(itemToRemove);
                else
                {
                    itemToRemove.Quantity = removeItem.Quantity;
                    _dbContext.Entry(itemToRemove).State = EntityState.Modified;
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
