using Examples.Data;
using Examples.Models.DTO;
using Examples.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Examples.WebApi.Services
{
    public class ExDbParticipantManager(ExampleDbContext dbContext) : IExDbParticipantManager
    {
        private readonly ExampleDbContext _dbContext = dbContext;
        public async Task<bool> CreateParticipantAsync(ExDbParticipantInsertDto dto)
        {
            await _dbContext.Participants.AddAsync(new ExDbParticipant()
            {
                Active = dto.Active,
                Name = dto.Name,
                Age = dto.Age,
                Deleted = dto.Deleted
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ExDbParticipant?> GetParticipantByIdAsync(int participantId)
        {
            return await _dbContext
                        .Participants
                        .FirstOrDefaultAsync(p => p.Id == participantId);
        }

        public async Task UpdateParticipantByIdAsync(int participantId, ExDbParticipantUpdateDto dto)
        {
            ExDbParticipant? thisParticipant = _dbContext
                       .Participants
                       .FirstOrDefault(p => p.Id == participantId) ?? throw new Exception();

            // Take new data from the dto, defaulting to original data when nulls are encountered.
            thisParticipant.Active  = dto.Active;
            thisParticipant.Name    = dto.Name      ?? thisParticipant.Name;
            thisParticipant.Age     = dto.Age       ?? thisParticipant.Age;
            thisParticipant.Phase   = dto.Phase;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteParticipantByIdAsync(int participantId)
        {
            ExDbParticipant? thisParticipant = _dbContext
                       .Participants
                       .FirstOrDefault(p => p.Id == participantId) ?? throw new Exception();

            // If the participant wasn't previously deleted, bail.
            if (!thisParticipant.Deleted)
            { throw new Exception("Participant has already been deleted."); }

            thisParticipant.Deleted = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task RestoreParticipantByIdAsync(int participantId)
        {
            ExDbParticipant? thisParticipant = _dbContext
                       .Participants
                       .FirstOrDefault(p => p.Id == participantId) ?? throw new Exception();

            // If the participant wasn't previously deleted, bail.
            if (!thisParticipant.Deleted)
            { throw new Exception("Participant has not been deleted, and cannot be restored."); }

            thisParticipant.Deleted = false;

            await _dbContext.SaveChangesAsync();
        }
    }
}
