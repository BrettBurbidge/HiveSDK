using Node.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Node.Shell
{

    /// <summary>
    /// Checks NiveServer for work to consume on the Action Queue
    /// </summary>
    public class WorkMonitor
    {

        private readonly ActionEventHandler _actionEventHandler;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;
        private readonly TimeSpan _interval;
        private readonly JsonSerializerOptions _jsonOptions;

        public WorkMonitor(ILogger logger, string endpoint, ActionEventHandler actionHandler, TimeSpan? interval = null)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _endpoint = endpoint;
            _interval = interval ?? TimeSpan.FromSeconds(15);
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _actionEventHandler = actionHandler;
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await MakeHttpRequestAsync();
                } catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while making HTTP request.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task MakeHttpRequestAsync()
        {
            _logger.LogInformation("Making HTTP request to {Endpoint}", _endpoint);
            HttpResponseMessage response = await _httpClient.GetAsync($"{_endpoint}/WorkMonitor");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            //SDK & BRAND NODE IMPLEMENTATION VERSION SPECIFIC CODE... HOW TO MANAGE THIS?

            if (!string.IsNullOrEmpty(responseBody))
            {
                NodeAction action = JsonSerializer.Deserialize<NodeAction>(responseBody, _jsonOptions);
                if (action != null)
                {
                    _logger.LogInformation($"Work response for action: {action?.Name}");
                    await _actionEventHandler.EmitEventAsync(action);
                } else
                {
                    _logger.LogInformation($"Work check completed successfully but no actions to perform.");
                }

            } else
            {
                _logger.LogWarning($"No action found to perform.");
            }

            _logger.LogInformation("Received response from WorkMonitor: {ResponseBody}", responseBody);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

    }
}
