namespace API.Models
{
    public class Consumable : BaseModel
    {
        public string Name { get; set; }
        public Dictionary<Toxin, float> Toxins { get; set; }
    }
}
