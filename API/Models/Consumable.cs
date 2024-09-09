namespace API.Models
{
    public class Consumable : BaseModel
    {
        public string Name { get; set; }

        public string toxinName { get; set; }
        public string amount { get; set; }
    }
}
