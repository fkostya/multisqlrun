namespace appui.shared.HostedEnvironment
{
    public class EnvSetupFactory : IEnvSetupFactory
    {
        private static Dictionary<string, IEnvSetupHandler> handlers = new();

        public void RegisterHandler(string name, IEnvSetupHandler handler)
        {
            handlers[name] = handler;
        }

        public Dictionary<string, IEnvSetupHandler> GetHandlers()
        {
            return handlers ?? new Dictionary<string, IEnvSetupHandler>();
        }
    }
}