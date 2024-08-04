using Node.SDK;
using System.Runtime.CompilerServices;

namespace GreenPowerCheck
{
    public class PowerMonitor : IPlugin
    {
        public string Name => "Green Power Monitor";

        public string Description => "Monitors the home power output and reports it back to the main server.";

        public string Version => "1.0.0";

        public string Fake => "asdfdsf";

        private void TimerCallback(object state)
        {
          Log($"{this.Name} triggered");
        }

        public void OnWorkToDo(WorkEventArgs e)
        {
            try
            {
               Log($"GreenPowerCheck Plugin received work to complete!");
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
            Log("Green Power Monitor Plugin starting...");

           // Timer timer = new Timer(TimerCallback, null, 0, 10000);

            //Do NOT BLOCK THIS THREAD.
            Thread.Sleep(Timeout.Infinite);
        }

        private void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public void OnWorkComplete(WorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnShutdown(ShutdownEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}