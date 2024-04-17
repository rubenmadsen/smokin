using System.Collections.Generic;

namespace API.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string LogLevel { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
