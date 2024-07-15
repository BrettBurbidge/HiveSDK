namespace HiveServer.SDK
{
    public interface IHiveServerPlugin
    {
        string Name { get; }
        void Initialize();
        void OnWorkToDo(object sender, EventArgs e);
    }
}