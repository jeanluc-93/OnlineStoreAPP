using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Data;
using OnlineStoreApp.Models;
using OnlineStoreApp.Services;

namespace OnlineStoreAppTests
{
    public class ShoppingCartServiceTests
    {
        [Fact]
        public async Task GetShoppingCartAsync_ReturnsShoppingCart()
        {
            // Arrange
            var userId = "user123";
            var shoppingCartItems = new List<ShoppingCartItem>
            {
                new ShoppingCartItem { UserId = userId, ItemId = 1, Quantity = 5 },
                new ShoppingCartItem { UserId = userId, ItemId = 2, Quantity = 2 },
                new ShoppingCartItem { UserId = "user987", ItemId = 1, Quantity = 1 }
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.ShoppingCartItems.AddRange(shoppingCartItems);
                dbContext.SaveChanges();

                var shoppingCartService = new ShoppingCartService(dbContext); 

                // Act
                var userShoppingCart = await shoppingCartService.GetShoppingCartAsync(userId);

                // Assert
                Assert.NotNull(userShoppingCart);
                Assert.Equal(2, userShoppingCart.Count);

                foreach (var item in userShoppingCart)
                {
                    Assert.Equal(userId, item.UserId);
                }
            }
        }

        [Fact]
        public async Task GetShoppingCartAsync_ReturnsEmptyCart()
        {
            // Arrange
            var userId = "user123";
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var shoppingCartService = new ShoppingCartService(dbContext);

                // Act
                var userShoppingCart = await shoppingCartService.GetShoppingCartAsync(userId);

                // Assert
                Assert.NotNull(userShoppingCart);
                Assert.Empty(userShoppingCart);
            }
        }

        [Fact]
        public async Task AddToCartAsync_AddsNewItem()
        {
            // Arrange
            var userId = "user123";
            var newItem = new CartRequest { ItemId = 1, Quantity = 2 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var shoppingCartService = new ShoppingCartService(dbContext);

                // Act
                var result = await shoppingCartService.AddToCartAsync(userId, newItem);

                // Assert
                Assert.True(result);

                // Verify that the item has been added to the shopping cart
                var itemInDatabase = await dbContext.ShoppingCartItems.FirstOrDefaultAsync(item => item.UserId == userId && item.ItemId == newItem.ItemId);
                Assert.NotNull(itemInDatabase);
                Assert.Equal(userId, itemInDatabase.UserId);
                Assert.Equal(newItem.ItemId, itemInDatabase.ItemId);
                Assert.Equal(newItem.Quantity, itemInDatabase.Quantity);
            }
        }

        [Fact]
        public async Task AddToCartAsync_UpdatesExistingItem()
        {
            // Arrange
            var userId = "user123";
            var existingItem = new ShoppingCartItem { ItemId = 1, UserId = userId, Quantity = 2 };
            var newItem = new CartRequest { ItemId = 1, Quantity = 5 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.ShoppingCartItems.Add(existingItem);
                dbContext.SaveChanges();

                var shoppingCartService = new ShoppingCartService(dbContext);

                // Act
                var result = await shoppingCartService.AddToCartAsync(userId, newItem);

                // Assert
                Assert.True(result);

                // Verify that the existing item has been updated
                var updatedInDatabase = await dbContext.ShoppingCartItems.FirstOrDefaultAsync(item => item.UserId == userId && item.ItemId == newItem.ItemId);
                Assert.NotNull(updatedInDatabase);
                Assert.Equal(userId, updatedInDatabase.UserId);
                Assert.Equal(newItem.ItemId, updatedInDatabase.ItemId);
                Assert.Equal(newItem.Quantity, updatedInDatabase.Quantity);
            }
        }

        [Fact]
        public async Task RemoveFromCartAsync_RemovesItem()
        {
            // Arrange
            var userId = "user123";
            var existingItem = new ShoppingCartItem { ItemId = 1, UserId = userId, Quantity = 2 };
            var removeItem = new CartRequest { ItemId = 1, Quantity = 0 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.ShoppingCartItems.Add(existingItem);
                dbContext.SaveChanges();

                var shoppingCartService = new ShoppingCartService(dbContext);

                // Act
                var result = await shoppingCartService.RemoveFromCartAsync(userId, removeItem);

                // Assert
                Assert.True(result);

                // Verify that the existing item has been removed from the shopping cart
                var removedFromDatabase = await dbContext.ShoppingCartItems.FirstOrDefaultAsync(item => item.UserId == userId && item.ItemId == removeItem.ItemId);
                Assert.Null(removedFromDatabase);
            }
        }

        [Fact]
        public async Task RemoveFromCartAsync_UpdatesItemQuantity()
        {
            // Arrange
            var userId = "user123";
            var existingItem = new ShoppingCartItem { ItemId = 1, UserId = userId, Quantity = 2 };
            var removeItem = new CartRequest { ItemId = 1, Quantity = 1 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.ShoppingCartItems.Add(existingItem);
                dbContext.SaveChanges();

                var shoppingCartService = new ShoppingCartService(dbContext);

                // Act
                var result = await shoppingCartService.RemoveFromCartAsync(userId, removeItem);

                // Assert
                Assert.True(result);

                // Verify that the existing item's quantity has been updated in the shopping cart
                var updatedInDatabase = await dbContext.ShoppingCartItems.FirstOrDefaultAsync(item => item.UserId == userId && item.ItemId == removeItem.ItemId);
                Assert.NotNull(updatedInDatabase);
                Assert.Equal(userId, updatedInDatabase.UserId);
                Assert.Equal(removeItem.ItemId, updatedInDatabase.ItemId);
                Assert.Equal(1, updatedInDatabase.Quantity);
            }
        }

        [Fact]
        public async Task RemoveFromCartAsync_ItemNotFound()
        {
            // Arrange
            var userId = "user123";
            var removeItem = new CartRequest { ItemId = 1, Quantity = 0 };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var shoppingCartService = new ShoppingCartService(dbContext);

                // Act
                var result = await shoppingCartService.RemoveFromCartAsync(userId, removeItem);

                // Assert
                Assert.False(result);
            }
        }
    }
}
