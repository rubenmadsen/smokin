using System.Configuration;

namespace API.Models
{
    public class User : BaseModel
    {
        public string userName { get; set; }

        public string consumableName { get; set; }
        public DateTime date {  get; set; }
        public int amount {  get; set; }
    }
}
