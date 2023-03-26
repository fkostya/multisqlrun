using Microsoft.Extensions.DependencyInjection;

namespace appui.shared.HostedEnvironment
{
    public class MsWindowsRunSetup
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnvSetupFactory _factory;
        private readonly IHostedEnvironment _hosted;

        public MsWindowsRunSetup(IServiceProvider serviceProvider, IEnvSetupFactory factory, IHostedEnvironment hosted)
        {
            _serviceProvider = serviceProvider;
            _factory = factory;
            _hosted = hosted;
        }

        public async Task<IServiceCollection> RunSetup(IServiceCollection services)
        {
            var handlers = _factory?.GetHandlers() ?? new Dictionary<string, IEnvSetupHandler>();

            foreach (var (_, handler) in handlers)
            {
                try
                {
                    await handler.Execute(_hosted);
                }
                catch
                {
                }
            }

            return services;
        }
    }
}