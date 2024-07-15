namespace PluginTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GreenPowerCheck.PowerMonitor monitor = new GreenPowerCheck.PowerMonitor();
            Console.WriteLine($"testing {monitor.Name}");
            monitor.Initialize();
            monitor.OnWorkToDo(null, new Node.SDK.WorkEventArgs() { NodeAction = new Node.SDK.NodeAction() { Name = "test action" } });
        }
    }
}