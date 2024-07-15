namespace Node.SDK
{
    public interface IPlugin
    {
        string Name { get; }
        void Initialize();
        void OnWorkToDo(object sender, WorkEventArgs e);
    }
}