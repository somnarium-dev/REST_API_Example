using Examples.Models.DTO;
using Examples.Models.Entities;

namespace Examples.WebApi.Services
{
    public interface IExDbParticipantManager
    {
        Task<bool> CreateParticipantAsync(ExDbParticipantInsertDto dto);
        Task<ExDbParticipant?> GetParticipantByIdAsync(int participantId);
        Task UpdateParticipantByIdAsync(int participantId, ExDbParticipantUpdateDto dto);
        Task DeleteParticipantByIdAsync(int participantId);
        Task RestoreParticipantByIdAsync(int participantId);
    }
}
