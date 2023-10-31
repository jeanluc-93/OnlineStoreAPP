using System.Text.Json.Serialization;

namespace OnlineStoreApp.Models
{
    public class CartRequest
    {
        [JsonPropertyName("itemId")]
        public int ItemId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
