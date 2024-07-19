using Node.SDK;

namespace ProofOfAvailability
{
    public class ProofOfAvailabilityRunner : IPlugin
    {
        public string Name => "Proof Of Availability";

        public string Description => "Pings a time series database to proof availability";

        public string Version => "1.0.0";

        private void TimerCallback(object state)
        {
            Log($"{this.Name} triggered");
        }

        public void OnWorkToDo(WorkEventArgs e)
        {
            try
            {
                Log($"{this.Name} Plugin received work to complete!");
                if (e.NodeAction != null)
                {
                    Log($"Task to complete is {e.NodeAction.Name} received work to complete!");
                } else
                {
                    Log($"Action to complete was null!");
                }
            } catch (Exception ex)
            {
                Log($"Error handling work request: {ex.Message}");
            }

        }

        public void Initialize()
        {

            try
            {
                Log($"{this.Name} Plugin starting...");

                Timer timer = new Timer(TimerCallback, null, 0, 10000);
            } catch (Exception ex)
            {
                Log($"Error running plugin: {ex.Message}");
            }


            //Do NOT BLOCK THIS THREAD.
            //Thread.Sleep(Timeout.Infinite);
        }

        private void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public void OnWorkComplete(WorkEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
