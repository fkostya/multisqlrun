namespace appui.shared.HostedEnvironment
{
    public sealed class EnvSetupFactory
    {
        private static Dictionary<string, IEnvSetupHandler> handlers = new();

        public static void RegisterFactory(string name, IEnvSetupHandler handler)
        {
            handlers[name] = handler;
        }

        public static Dictionary<string, IEnvSetupHandler> GetFactories()
        {
            return handlers;
        }
    }
}