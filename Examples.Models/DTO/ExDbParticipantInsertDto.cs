using Examples.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Examples.Models.DTO
{
    public class ExDbParticipantInsertDto
    {
        public int Id { get; set; }
        public ParticipantActive Active { get; set; } = ParticipantActive.Inactive;
        [Required(ErrorMessage = $"{nameof(Name)} is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = $"{nameof(Age)} is required.")]
        public string Age { get; set; } = string.Empty;
        [Required(ErrorMessage = $"{nameof(ParticipantPhase)} is required.")]
        public ParticipantPhase Phase { get; set; }
        public bool Deleted { get; set; } = false;
        [JsonIgnore]
        public ICollection<ExDbHistory>? History { get; set; }
    }
}
