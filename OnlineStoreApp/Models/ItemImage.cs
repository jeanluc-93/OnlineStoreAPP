using System.Text.Json.Serialization;

namespace OnlineStoreApp.Models
{
    public class ItemImage
    {
        [JsonPropertyName("imageId")]
        public int ImageId { get; set; }

        [JsonPropertyName("itemId")]
        public int ItemID { get; set; }

        [JsonPropertyName("data")]
        public byte[] Data { get; set; }
    }
}
