using System.Text.Json.Serialization;

namespace OnlineStoreApp.Models
{
    public class Item
    {
        [JsonPropertyName("itemId")]
        public int ItemId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public int Price { get; set; }
    }
}
