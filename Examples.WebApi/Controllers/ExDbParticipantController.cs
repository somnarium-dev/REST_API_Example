using Examples.Models.DTO;
using Examples.Models.Entities;
using Examples.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace Examples.WebApi.Controllers
{
    [Route("/api/participants")]
    public class ExDbParticipantController(IExDbParticipantManager participantManager, Serilog.ILogger logger) : Controller
    {
        private readonly IExDbParticipantManager _participantManager = participantManager;
        private readonly Serilog.ILogger _logger = logger;

        [HttpPost]
        public async Task<ActionResult> InsertNewParticipant([FromBody] ExDbParticipantInsertDto dto)
        {
            using (LogContext.PushProperty("Examples.Rest_API.API", "Create Participant"))
            {
                try
                {
                    var participantCreationResult = await _participantManager.CreateParticipantAsync(dto);

                    if (participantCreationResult)
                    {
                        _logger.Information("Created participant.");
                        return Ok();
                    }
                    else
                    {
                        throw new Exception("Failed to create new participant in example database. Please verify your input parameters and try again.");
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("{errorMessage}", e.Message);

                    return new ContentResult()
                    {
                        Content = e.Message,
                        ContentType = "text/plain",
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
        }

        [HttpGet("{participantId}")]
        public async Task<ActionResult<ExDbParticipant>> GetParticipant(int participantId)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Get Participant"))
            {
                try
                {
                    var participantToReturn = await _participantManager.GetParticipantByIdAsync(participantId);
                    if (participantToReturn == null) { return NotFound(); }

                    return Ok(participantToReturn);
                }
                catch (Exception e)
                {
                    _logger.Error("{errorMessage}", e.Message);

                    return new ContentResult()
                    {
                        Content = e.Message,
                        ContentType = "text/plain",
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
        }

        [HttpPut("{participantId}")]
        public async Task<ActionResult> UpdateParticipant(int participantId, [FromBody] ExDbParticipantUpdateDto dto)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Update Participant"))
            {
                try
                {
                    // Check for participant.
                    var participantToUpdate = await _participantManager.GetParticipantByIdAsync(participantId);
                    if (participantToUpdate == null) { return NotFound(); }

                    // Update.
                    await _participantManager.UpdateParticipantByIdAsync(participantId, dto);

                    _logger.Information("Updated participant #{id}", participantId);

                    return Ok();
                }
                catch (Exception e)
                {
                    _logger.Error("{errorMessage}", e.Message);

                    return new ContentResult()
                    {
                        Content = e.Message,
                        ContentType = "text/plain",
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
        }

        [HttpPut("{participantId}/delete")]
        public async Task<ActionResult> DeleteParticipant(int participantId)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Delete Participant"))
            {
                try
                {
                    // Check for participant.
                    var participantToDelete = await _participantManager.GetParticipantByIdAsync(participantId);
                    if (participantToDelete == null) { return NotFound(); }

                    // Delete.
                    await _participantManager.DeleteParticipantByIdAsync(participantId);

                    _logger.Information("Deleted participant #{id}", participantId);

                    return Ok();
                }
                catch (Exception e)
                {
                    _logger.Error("{errorMessage}", e.Message);

                    return new ContentResult()
                    {
                        Content = e.Message,
                        ContentType = "text/plain",
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
        }

        [HttpPut("{participantId}/restore")]
        public async Task<ActionResult> RestoreParticipant(int participantId)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Restore Participant"))
            {
                try
                {
                    // Check for participant.
                    var participantToRestore = await _participantManager.GetParticipantByIdAsync(participantId);
                    if (participantToRestore == null) { return NotFound(); }

                    // Restore.
                    await _participantManager.RestoreParticipantByIdAsync(participantId);

                    _logger.Information("Restored participant #{id}", participantId);

                    return Ok();
                }
                catch (Exception e)
                {
                    _logger.Error("{errorMessage}", e.Message);

                    return new ContentResult()
                    {
                        Content = e.Message,
                        ContentType = "text/plain",
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
        }
    }
}
