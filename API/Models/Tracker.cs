namespace API.Models
{
    public class Tracker :BaseModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Consumable Consumable { get; set; }
        public int Amount { get; set; }
    }
}
