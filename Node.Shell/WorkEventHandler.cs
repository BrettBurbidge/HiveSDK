using Node.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Node.Shell.LocalPluginManager;

namespace Node.Shell
{
    /// <summary>
    /// Handles events to the Pluggins when there is work to be done.
    /// </summary>
    public class WorkEventHandler
    {
        private readonly ActionEventHandler _eventHandler;
        private List<IPlugin> _plugins;
        private readonly ILogger _logger;

        public WorkEventHandler(ILogger logger, ActionEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
            _logger = logger;
        }

        public void SetPlugins(List<IPlugin> plugins)
        {
            _plugins = plugins;
        }

        public async Task HandleEventsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await foreach (var eventData in _eventHandler.ReadAllAsync().WithCancellation(cancellationToken))
                {
                    if (eventData == null) return;

                    if (_plugins == null) return;

                    _logger.LogInformation($"Node Action Event received - Send Event to Plugins. {eventData}");
                    // Process the event here
                    foreach (var plugin in _plugins)
                    {
                        try
                        {

                            plugin.OnWorkToDo(new WorkEventArgs() { NodeAction = eventData });
                        } catch (Exception ex)
                        {
                            _logger.LogError($"Notify Plugins failed...{ex.Message}");
                        }

                    }
                }
            } catch (OperationCanceledException)
            {
                // Handle cancellation
            }
        }
    }

}