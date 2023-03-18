using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace appui.shared.HostedEnvironment
{
    public static class RunSetupEx
    {
        public static async Task<IServiceCollection> RunSetup(this IServiceCollection service)
        {
            var provider = service.BuildServiceProvider();
            var factories = EnvSetupFactory.GetFactories();
            var hosted = provider.GetService<IHostedEnvironment>();

            foreach (var (_, handler) in factories)
            {
                try
                {
                    await handler.Execute(hosted, provider);
                }
                catch
                {
                }
            }

            return service;
        }
    }
}
