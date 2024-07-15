using Node.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace NodeShell
{

    /// <summary>
    /// Checks NiveServer for work to consume on the Action Queue
    /// </summary>
    public class WorkMonitor
    {
        private int interval = 15000;

        private static HiveServerClient hiveServerClient = new HiveServerClient("https://localhost:32775"); // Replace with your HiveServer URL

        // Define the workToComplete Event
        public EventHandler<WorkEventArgs> WorkToDo;
        
        public void StartWorkMonitor()
        {
            Console.WriteLine($"Work Monitor started: checking every {interval / 1000} seconds...");

            // Initial timer setup
           var timer = new Timer(async _ => await TimerCallback(null), null, 0, interval);

            //Keep the worker running
            Thread.Sleep(Timeout.Infinite);
        }

        private async Task TimerCallback(object state)
        {
            Console.WriteLine($"Checking Hive Server for Node Work: {DateTime.Now}");

            //Check in with HiveServer
            NodeAction action = await hiveServerClient.CheckForWorkAsync();

            if (action != null)
            {
                Console.WriteLine("Found work to do, sending to Plugins.");
                OnWorkToDo(new WorkEventArgs() { NodeAction = action });
                return;
            }
        }

        protected virtual void OnWorkToDo(WorkEventArgs e)
        {
            WorkToDo?.Invoke(this, e);
        }
    }
}
