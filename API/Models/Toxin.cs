using System.Text.Json.Serialization;

namespace API.Models
{
    public class Toxin : BaseModel
    {

        
        public string toxin { get; set; }
        public Category Category { get; set; }


        public string Description { get; set; }
    }
}
