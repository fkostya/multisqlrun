namespace appui.shared.HostedEnvironment
{
    public interface IEnvSetupFactory
    {
        void RegisterHandler(string name, IEnvSetupHandler handler);

        Dictionary<string, IEnvSetupHandler> GetHandlers();
    }
}