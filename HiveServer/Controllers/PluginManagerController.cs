using Microsoft.AspNetCore.Mvc;
using HiveServer.SDK;

namespace HiveServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PluginManagerController : ControllerBase
    {
        private readonly ILogger<PluginManagerController> _logger;

        public PluginManagerController(ILogger<PluginManagerController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "CheckForNewPlugins")]
        public IActionResult CheckForNewPlugins()
        {
            try
            {
                return Ok("Check for new plugins");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing GetWorkToDo.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}