namespace HiveServer.SDK
{
    public interface IHiveServerPlugin
    {
        string Name { get; }
        string Description { get; }
        string Version { get; }
        void Initialize();
        void OnWorkToDo(object sender, EventArgs e);
    }
}