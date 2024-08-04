namespace Node.SDK
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        string Version { get; }
        void Initialize();

        void OnWorkToDo(WorkEventArgs e);

        void OnWorkComplete(WorkEventArgs e);

        void OnShutdown(ShutdownEventArgs e);
    }
}