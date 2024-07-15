using Node.SDK;
using System.Runtime.CompilerServices;

namespace GreenPowerCheck
{
    public class PowerMonitor : IPlugin
    {
        public string Name => "Green Power Monitor";

        private void TimerCallback(object state)
        {
            Console.WriteLine($"{this.Name} triggered at: {DateTime.Now}");
        }

        public void OnWorkToDo(object sender, WorkEventArgs e)
        {
            try
            {
                Console.WriteLine($"GreenPowerCheck Plugin received work to complete!");
                if (e.NodeAction != null)
                {
                    Console.WriteLine($"Task to complete is {e.NodeAction.Name} received work to complete!");
                }
                else
                {
                    Console.WriteLine($"Action to complete was null!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling work request: {ex.Message}");
            }

        }

        public void Initialize()
        {
            Console.WriteLine("Green Power Monitor.");

           // Timer timer = new Timer(TimerCallback, null, 0, 10000);
        }
    }
}