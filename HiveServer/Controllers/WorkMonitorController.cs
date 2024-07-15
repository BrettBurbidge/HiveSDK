using Microsoft.AspNetCore.Mvc;
using HiveServer.SDK;

namespace HiveServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkMonitorController : ControllerBase
    {
        private readonly ILogger<WorkMonitorController> _logger;

        public WorkMonitorController(ILogger<WorkMonitorController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWorkToDo")]
        public IActionResult Get()
        {
            try
            {
                var action = ActionQueue.GetActionToConsume();

                if (action == null)
                {
                    return NotFound(); // Or return appropriate HTTP status code
                }

                return Ok(action);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing GetWorkToDo.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}