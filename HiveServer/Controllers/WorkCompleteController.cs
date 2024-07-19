using Microsoft.AspNetCore.Mvc;
using HiveServer.SDK;

namespace HiveServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkCompleteController : ControllerBase
    {
        private readonly ILogger<WorkCompleteController> _logger;

        public WorkCompleteController(ILogger<WorkCompleteController> logger)
        {
            _logger = logger;
        }

        //Event called by the node to remove the work from the queue after it has been finished. 
        [HttpGet(Name = "CompeteWork")]
        public IActionResult Post(NodeActionResult actionResult)
        {
            try
            {
                if (actionResult == null || actionResult.NodeAction == null)
                    return StatusCode(500, "The Node Action to complete was not found on the request");

                var action = ActionQueue.RemoveActionFromQueue(actionResult.NodeAction.ActionID);
                                
                return Ok(action);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing GetWorkToDo.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}