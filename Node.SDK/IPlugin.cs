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

        //Add OnStopping - an event that will allow devs to clean up and shutdown the Plugin
    }
}