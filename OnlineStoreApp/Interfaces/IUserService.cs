namespace OnlineStoreApp.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Check whether the user exists in the database.
        /// </summary>
        /// <param name="id">Id if the user that needs to be checked.</param>
        /// <returns>True if user is found ekse False.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        Task<bool> CheckIfUserExists(string id);
    }
}
