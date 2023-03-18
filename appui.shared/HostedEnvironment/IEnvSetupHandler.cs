namespace appui.shared.HostedEnvironment
{
    public interface IEnvSetupHandler
    {
        public Task Execute(IHostedEnvironment hosted, IServiceProvider args);
    }
}