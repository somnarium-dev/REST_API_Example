using Examples.Data;
using Examples.Models.DTO;
using Examples.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Examples.WebApi.Services
{
    public class ExDbHistoryManager(ExampleDbContext dbContext) : IExDbHistoryManager
    {
        private readonly ExampleDbContext _dbContext = dbContext;

        public async Task<bool> CreateHistoryAsync(ExDbHistoryInsertDto dto)
        {
            await _dbContext.Histories.AddAsync(new ExDbHistory()
            {
                ParticipantId = dto.ParticipantId,
                Details = dto.Details,
                EntryTimestamp = dto.EntryTimestamp,
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ExDbHistory?> GetHistoryByIdAsync(int HistoryId)
        {
            return await _dbContext
                        .Histories
                        .FirstOrDefaultAsync(h => h.Id == HistoryId);
        }

        public async Task UpdateHistoryByIdAsync(int historyId, ExDbHistoryUpdateDto dto)
        {
            ExDbHistory? thisHistory = _dbContext
                       .Histories
                       .FirstOrDefault(h => h.Id == historyId) ?? throw new Exception();

            // Take new data from the dto, defaulting to original data when nulls are encountered.
            thisHistory.ParticipantId   = dto.ParticipantId     ?? thisHistory.ParticipantId;
            thisHistory.Details         = dto.Details           ?? thisHistory.Details;
            thisHistory.EntryTimestamp  = dto.EntryTimestamp    ?? thisHistory.EntryTimestamp;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteHistoryByIdAsync(int historyId)
        {
            ExDbHistory? thisHistory = _dbContext
                       .Histories
                       .FirstOrDefault(h => h.Id == historyId) ?? throw new Exception();

            // If the participant wasn't previously deleted, bail.
            if (!thisHistory.Deleted)
            { throw new Exception("History has already been deleted."); }

            thisHistory.Deleted = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task RestoreHistoryByIdAsync(int historyId)
        {
            ExDbHistory? thisHistory = _dbContext
                       .Histories
                       .FirstOrDefault(h => h.Id == historyId) ?? throw new Exception();

            // If the participant wasn't previously deleted, bail.
            if (!thisHistory.Deleted)
            { throw new Exception("History has not been deleted, and cannot be restored."); }

            thisHistory.Deleted = false;

            await _dbContext.SaveChangesAsync();
        }

        public ExDbHistory? GetMostRecentHistoryByParticipantId(int participantId)
        {
            ExDbHistory ? thisHistory = _dbContext
                       .Histories
                       .LastOrDefault(h => h.ParticipantId == participantId) ?? throw new Exception();

            return thisHistory;
        }
    }
}
