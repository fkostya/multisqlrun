using appui.shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace appui.shared.HostedEnvironment
{
    public class EnvSetupFactory : IEnvSetupFactory
    {
        private readonly Dictionary<string, IEnvSetupHandler> handlers = new();
        private readonly Dictionary<Type, string> registeredHandlers = new();
        private readonly int MaxHandlers = 50;
        private readonly ILogger<EnvSetupFactory> logger;

        public EnvSetupFactory(ILogger<EnvSetupFactory> logger, IOptions<AppSettings> options)
        {
            this.logger = logger;
            this.MaxHandlers = options?.Value?.MaxHandlers ?? 50;
        }

        public bool RegisterHandler(string handlerName, IEnvSetupHandler handler)
        {
            if (handler == null || string.IsNullOrWhiteSpace(handlerName)) return false;
            
            if(handlerName.Length >= 29)
            {
                throw new ArgumentOutOfRangeException("handler name is too long, max lengh is 29");

            }
            if (handlers.Count >= MaxHandlers)
            {
                logger?.LogInformation("reached MAX limit of handlers");
                return false;
            }

            if (registeredHandlers.ContainsKey(handler.GetType()))
            {
                logger?.LogInformation("handler of type {handler} already registered by handlerName {registeredHandlers}", handler.GetType(), registeredHandlers[handler.GetType()]);
                return false;
            }

            if (handlers.ContainsKey(handlerName))
            {
                logger?.LogInformation("handler with the same handlerName {handlerName} already registered", handlerName);
                return false;
            }

            handlers[handlerName] = handler;
            registeredHandlers[handler.GetType()] = handlerName;
            return true;
        }

        public Dictionary<string, IEnvSetupHandler> GetHandlers()
        {
            return handlers ?? new Dictionary<string, IEnvSetupHandler>();
        }

        public bool UnregisterHandler(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !handlers.ContainsKey(name)) return false;

            registeredHandlers.Remove(handlers[name].GetType());
            handlers.Remove(name);

            return true;
        }
    }
}