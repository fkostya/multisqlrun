namespace appui.shared.HostedEnvironment
{
    public interface IEnvSetupFactory
    {
        bool RegisterHandler(string name, IEnvSetupHandler handler);

        bool UnregisterHandler(string name);

        Dictionary<string, IEnvSetupHandler> GetHandlers();
    }
}