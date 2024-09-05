namespace API.Models
{
    public class Toxin : BaseModel
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
    }
}
