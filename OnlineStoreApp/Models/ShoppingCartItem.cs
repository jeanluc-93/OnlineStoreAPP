using System.Text.Json.Serialization;

namespace OnlineStoreApp.Models
{
    public class ShoppingCartItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("itemId")]
        public int ItemId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        // Reference to the user who owns the item
        [JsonPropertyName("userID")]
        public string UserId { get; set; }
    }
}
