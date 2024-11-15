using Examples.Models.Entities;
using System.Text.Json.Serialization;

namespace Examples.Models.DTO
{
    public class ExDbHistoryUpdateDto
    {
        public int? ParticipantId { get; set; }
        public string? Details { get; set; }
        public DateTime? EntryTimestamp { get; set; }
        [JsonIgnore]
        public ExDbParticipant? ExDbParticipant { get; set; }
    }
}
