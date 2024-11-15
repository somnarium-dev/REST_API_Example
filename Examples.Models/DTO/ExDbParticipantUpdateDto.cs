using System.ComponentModel.DataAnnotations;

namespace Examples.Models.DTO
{
    public class ExDbParticipantUpdateDto
    {
        public ParticipantActive Active { get; set; }
        public string? Name { get; set; }
        public string? Age { get; set; }
        [Required(ErrorMessage = $"{nameof(ParticipantPhase)} is required.")]
        public ParticipantPhase Phase { get; set; }
    }
}
