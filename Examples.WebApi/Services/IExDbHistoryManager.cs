using Examples.Models.DTO;
using Examples.Models.Entities;

namespace Examples.WebApi.Services
{
    public interface IExDbHistoryManager
    {
        Task<bool> CreateHistoryAsync(ExDbHistoryInsertDto dto);
        Task<ExDbHistory?> GetHistoryByIdAsync(int historyId);
        Task UpdateHistoryByIdAsync(int historyId, ExDbHistoryUpdateDto dto);
        Task DeleteHistoryByIdAsync(int historyId);
        Task RestoreHistoryByIdAsync(int historyId);
        ExDbHistory? GetMostRecentHistoryByParticipantId(int participantId);
    }
}
