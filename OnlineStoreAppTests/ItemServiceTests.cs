using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Data;
using OnlineStoreApp.Models;
using OnlineStoreApp.Services;

namespace OnlineStoreAppTests
{
    public class ItemServiceTests
    {
        [Fact]
        public async Task CreateItemAsync_ReturnsNewItem()
        {
            // Arrange
            var newItem = new Item { Name = "Item 1", Price = 999 };

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var itemService = new ItemService(dbContext);

                // Act
                var createdItem = await itemService.CreateItemAsync(newItem);

                // Assert
                Assert.NotNull(createdItem);
                Assert.Equal(newItem.Name, createdItem.Name);
            }
        }

        [Fact]
        public async Task GetItemAsync_ReturnsItem()
        {
            // Arrange
            var itemId = 1;
            var itemToRetrieve = new Item { ItemId = 1, Name = "Item 1", Price = 999 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Items.Add(itemToRetrieve);
                dbContext.SaveChanges();

                var itemService = new ItemService(dbContext);

                // Act
                var retrievedItem = await itemService.GetItemAsync(itemId);

                // Assert
                Assert.NotNull(retrievedItem);
                Assert.Equal(itemToRetrieve.ItemId, retrievedItem.ItemId);
                Assert.Equal(itemToRetrieve.Name, retrievedItem.Name);
                Assert.Equal(itemToRetrieve.Price, retrievedItem.Price);
            }
        }

        [Fact]
        public async Task GetItemAsync_ReturnsNull()
        {
            // Arrange
            var itemId = 1;

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var itemService = new ItemService(dbContext); // Replace with your actual dependencies

                // Act
                var retrievedItem = await itemService.GetItemAsync(itemId);

                // Assert
                Assert.Null(retrievedItem);
            }
        }

        [Fact]
        public async Task GetAllItemsAsync_ReturnsItems()
        {
            // Arrange
            var itemsToRetrieve = new List<Item>
            {
                new Item { ItemId = 1, Name = "Item 1" },
                new Item { ItemId = 2, Name = "Item 2" },
                new Item { ItemId = 3, Name = "Item 3" },
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Items.AddRange(itemsToRetrieve);
                dbContext.SaveChanges();

                var itemService = new ItemService(dbContext);

                // Act
                var retrievedItems = await itemService.GetAllItemsAsync();

                // Assert
                Assert.NotNull(retrievedItems);
                Assert.Equal(itemsToRetrieve.Count, retrievedItems.Count);

                foreach (var item in itemsToRetrieve)
                {
                    Assert.Contains(retrievedItems, i => i.ItemId == item.ItemId && i.Name == item.Name && i.Price == item.Price);
                }
            }
        }

        [Fact]
        public async Task GetAllItemsAsync_ReturnsEmptyList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var itemService = new ItemService(dbContext);

                // Act
                var retrievedItems = await itemService.GetAllItemsAsync();

                // Assert
                Assert.NotNull(retrievedItems);
                Assert.Empty(retrievedItems);
            }
        }

        [Fact]
        public async Task UpdateItemAsync_UpdatesItem()
        {
            // Arrange
            var itemId = 1;
            var initialItem = new Item { ItemId = itemId, Name = "Item 1", Price = 1 };
            var updatedItem = new Item { ItemId = itemId, Name = "Item 2", Price = 999 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Items.Add(initialItem);
                dbContext.SaveChanges();

                var itemService = new ItemService(dbContext);

                // Act
                var result = await itemService.UpdateItemAsync(itemId, updatedItem);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedItem.ItemId, result.ItemId);
                Assert.Equal(updatedItem.Name, result.Name);
                Assert.Equal(updatedItem.Price, result.Price);

                // Verify that the item has been updated in the database
                var updatedDatabaseItem = await dbContext.Items.FindAsync(itemId);
                Assert.NotNull(updatedDatabaseItem);
                Assert.Equal(updatedItem.ItemId, updatedDatabaseItem.ItemId);
                Assert.Equal(updatedItem.Name, updatedDatabaseItem.Name);
                Assert.Equal(updatedItem.Price, updatedDatabaseItem.Price);
            }
        }

        [Fact]
        public async Task UpdateItemAsync_ThrowsException_ItemNotFound()
        {
            // Arrange
            var itemId = 1;
            var updatedItem = new Item { ItemId = itemId, Name = "Item 1" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var itemService = new ItemService(dbContext);

                // Act and Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() => itemService.UpdateItemAsync(itemId, updatedItem));
            }
        }

        [Fact]
        public async Task DeleteItemAsync_DeletesItem()
        {
            // Arrange
            var itemId = 1;
            var itemToDelete = new Item { ItemId = itemId, Name = "Item 1", Price = 999 };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Items.Add(itemToDelete);
                dbContext.SaveChanges();

                var itemService = new ItemService(dbContext);

                // Act
                var result = await itemService.DeleteItemAsync(itemId);

                // Assert
                Assert.True(result);

                // Verify that the item has been deleted from the database
                var deletedItem = await dbContext.Items.FindAsync(itemId);
                Assert.Null(deletedItem);
            }
        }

        [Fact]
        public async Task DeleteItemAsync_ItemNotFound()
        {
            // Arrange
            var itemId = 1;

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var itemService = new ItemService(dbContext);

                // Act
                var result = await itemService.DeleteItemAsync(itemId);

                // Assert
                Assert.False(result); // Item not found, should return false
            }
        }

        [Fact]
        public async Task UploadImageAsync_UploadsImage()
        {
            // Arrange
            var itemId = 1;
            var imageFile = new FormFile(new MemoryStream(new byte[1024]), 0, 1024, "Data", "test.jpeg");
            var itemWithoutImage = new Item { ItemId = itemId, Name = "Item 1" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Items.Add(itemWithoutImage);
                await dbContext.SaveChangesAsync();

                var itemService = new ItemService(dbContext);

                // Act
                var result = await itemService.UploadImageAsync(itemId, imageFile);

                // Assert
                Assert.True(result);

                // Verify that the image has been uploaded and associated with the item
                var imageFromDatabase = await dbContext.Images.FirstOrDefaultAsync(image => image.ItemID == itemId);
                Assert.NotNull(imageFromDatabase);
                Assert.Equal(itemId, imageFromDatabase.ItemID);
                Assert.NotNull(imageFromDatabase.Data);
                Assert.Equal(imageFile.Length, imageFromDatabase.Data.Length);
            }
        }

        [Fact]
        public async Task UploadImageAsync_ItemNotFound()
        {
            // Arrange
            var itemId = 1;
            var imageFile = new FormFile(new MemoryStream(new byte[1024]), 0, 1024, "Data", "test.jpg");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                var itemService = new ItemService(dbContext);

                // Act
                var result = await itemService.UploadImageAsync(itemId, imageFile);

                // Assert
                Assert.False(result); // Item not found, should return false
            }
        }

        [Fact]
        public async Task UploadImageAsync_EmptyImage()
        {
            // Arrange
            var itemId = 1;
            var itemWithImage = new Item { ItemId = itemId, Name = "Item with Image" };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Items.Add(itemWithImage);
                dbContext.SaveChanges();

                var itemService = new ItemService(dbContext);

                // Act
                var result = await itemService.UploadImageAsync(itemId, null);

                // Assert
                Assert.False(result);
            }
        }
    }
}