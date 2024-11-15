using Examples.Models.DTO;
using Examples.Models.Entities;
using Examples.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace Examples.WebApi.Controllers
{
    [Route("/api/histories")]
    public class ExDbHistoryController(IExDbHistoryManager historyManager, Serilog.ILogger logger) : Controller
    {
        private readonly IExDbHistoryManager _historyManager = historyManager;
        private readonly Serilog.ILogger _logger = logger;

        [HttpPost]
        public async Task<ActionResult<ExDbHistory>> InsertNewHistory([FromBody] ExDbHistoryInsertDto dto)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Create History"))
            {
                try
                {
                    var historyCreationResult = await _historyManager.CreateHistoryAsync(dto);

                    if (historyCreationResult)
                    {
                        _logger.Information("Created history entry.");
                        return Ok();
                    }
                    else
                    {
                        throw new Exception("Failed to create new history entry in example database. Please verify your input parameters and try again.");
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

        [HttpGet("{historyId}")]
        public async Task<ActionResult> GetHistory(int historyId)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Get History"))
            {
                try
                {
                    var historyToReturn = await _historyManager.GetHistoryByIdAsync(historyId);
                    if (historyToReturn == null) { return NotFound(); }

                    return Ok(historyToReturn);
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

        [HttpPut("{historyId}")]
        public async Task<ActionResult> UpdateHistory(int historyId, ExDbHistoryUpdateDto dto)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Update History"))
            {
                try
                {
                    // Check for history.
                    var historyToUpdate = await _historyManager.GetHistoryByIdAsync(historyId);
                    if (historyToUpdate == null) { return NotFound(); }

                    // Update.
                    await _historyManager.UpdateHistoryByIdAsync(historyId, dto);

                    _logger.Information("Updated history #{id}", historyId);

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

        [HttpDelete("{historyId}/delete")]
        public async Task<ActionResult> DeleteHistory(int historyId)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Delete History"))
            {
                try
                {
                    // Check for history.
                    var historyToDelete = await _historyManager.GetHistoryByIdAsync(historyId);
                    if (historyToDelete == null) { return NotFound(); }

                    // Delete.
                    await _historyManager.DeleteHistoryByIdAsync(historyId);

                    _logger.Information("Deleted history #{id}", historyId);

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

        [HttpPut("{historyId}/restore")]
        public async Task<ActionResult> RestoreHistory(int historyId)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Restore History"))
            {
                try
                {
                    // Check for history.
                    var participantToRestore = await _historyManager.GetHistoryByIdAsync(historyId);
                    if (participantToRestore == null) { return NotFound(); }

                    // Restore.
                    await _historyManager.RestoreHistoryByIdAsync(historyId);

                    _logger.Information("Restored history #{id}", historyId);

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

        [HttpGet("{clientId}/mostrecent")]
        public async Task<ActionResult> GetMostRecentHistory(int clientId)
        {
            using (LogContext.PushProperty("Examples.REST_API.API", "Get Most Recent History"))
            {
                try
                {
                    var historyToReturn = _historyManager.GetMostRecentHistoryByParticipantId(clientId);
                    if (historyToReturn == null) { return NotFound(); }

                    return Ok(historyToReturn);
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
