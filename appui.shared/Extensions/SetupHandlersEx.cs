﻿using appui.shared.HostedEnvironment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace appui.shared.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SetupHandlersEx
    {
        public static IServiceCollection AddSetupHandlers<T>(this IServiceCollection service, Action<IServiceProvider> func)
            where T : IHostedEnvironment
        {
            var provider = service.BuildServiceProvider();
            service.TryAddSingleton(typeof(IHostedEnvironment), typeof(T));

            func(provider);

            return service;
        }
    }
}