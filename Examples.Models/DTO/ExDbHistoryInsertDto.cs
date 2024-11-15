using Examples.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Examples.Models.DTO
{
    public class ExDbHistoryInsertDto
    {
        [Required(ErrorMessage = $"{nameof(ParticipantId)} is required.")]
        public int ParticipantId { get; set; }
        [Required(ErrorMessage = $"{nameof(Details)} are required.")]
        public string Details { get; set; } = string.Empty;
        [Required(ErrorMessage = $"{nameof(EntryTimestamp)} is required.")]
        public DateTime EntryTimestamp { get; set; }
        [JsonIgnore]
        public bool Deleted { get; set; } = false;
        [JsonIgnore]
        public ExDbParticipant? ExDbParticipant { get; set; }
    }

    /*
     * Please note: If the timestamp is entered in a format other than ISO 8601, the API will throw.
     * Resolving this issue would be an easy target for future improvements. As is, asking end users
     * to utilize ISO 8601 is a cumbersome ask.
     * 
     * - NEH 11/14/2024
     */
}
