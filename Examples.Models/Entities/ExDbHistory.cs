using System.Text.Json.Serialization;

namespace Examples.Models.Entities
{
    public class ExDbHistory
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public string Details { get; set; } = string.Empty;
        public DateTime EntryTimestamp { get; set; }
        public bool Deleted { get; set; } = false;
        [JsonIgnore]
        public ExDbParticipant? ExDbParticipant { get; set; }

        public override string ToString()
        {
            string timeStamp = EntryTimestamp.ToString("dd/MM/yyyy @ hh:mm:ss tt");

            return $"\t[{timeStamp}] : {Details}\n";
        }
    }
}
