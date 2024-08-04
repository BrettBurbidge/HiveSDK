using Node.SDK;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Node.Shell
{

    /// <summary>
    /// Handles multi threaded events. This is used to hold and raise events when nodes have work to do. 
    /// </summary>
    public class ActionEventHandler
    {
        private readonly Channel<NodeAction> _channel;

        public ActionEventHandler()
        {
            _channel = Channel.CreateUnbounded<NodeAction>();
            _ = ProcessEventsAsync();
        }

        public async Task EmitEventAsync(NodeAction nodeAction)
        {
            await _channel.Writer.WriteAsync(nodeAction);
        }

        private async Task ProcessEventsAsync()
        {
            await foreach (var eventData in _channel.Reader.ReadAllAsync())
            {
                await HandleEventAsync(eventData);
            }
        }

        public IAsyncEnumerable<NodeAction> ReadAllAsync()
        {
            return _channel.Reader.ReadAllAsync();
        }

        private Task HandleEventAsync(NodeAction eventData)
        {
            Console.WriteLine($"Handling event: {eventData}");
            // Add your event handling logic here
            return Task.CompletedTask;
        }
    }
}