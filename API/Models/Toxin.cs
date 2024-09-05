using System.Text.Json.Serialization;

namespace API.Models
{
    public class Toxin : BaseModel
    {
        [JsonPropertyName("toxin")]
        public string Name { get; set; }
        public Category Category { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
