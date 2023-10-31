using OnlineStoreApp.Models;

namespace OnlineStoreApp.Interfaces
{
    public interface IShoppingCartService
    {
        /// <summary>
        /// Get the users cart.
        /// </summary>
        /// <param name="userId">User id to retrive cart for.</param>
        /// <returns>A list of all the items in the users shopping cart.</returns>
        Task<List<ShoppingCartItem>> GetShoppingCartAsync(string userId);

        /// <summary>
        /// Adds an item to the users cart.
        /// </summary>
        /// <param name="userId">Users cart to add too.</param>
        /// <param name="newItem">Item to be added</param>
        /// <returns>True is item has been added.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<bool> AddToCartAsync(string userId, CartRequest newItem);

        /// <summary>
        /// Removes items from an users cart.
        /// </summary>
        /// <param name="userId">Users cart to be removed from.</param>
        /// <param name="removeItem">Details to update too.</param>
        /// <returns>True is item has been added; False if item is not found.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<bool> RemoveFromCartAsync(string userId, CartRequest removeItem);
    }
}
