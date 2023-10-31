using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Data;
using OnlineStoreApp.Interfaces;

namespace OnlineStoreApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="dbContext">Entity Framework database context</param>
        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Check whether the user exists in the database.
        /// </summary>
        /// <param name="id">Id if the user that needs to be checked.</param>
        /// <returns>True if user is found ekse False.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the database operation.</exception>
        public async Task<bool> CheckIfUserExists(string id)
        {
            try
            {
                var user = await _dbContext.Users.Where(u => u.UserName.Equals(id)).FirstOrDefaultAsync();
                if (user == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
