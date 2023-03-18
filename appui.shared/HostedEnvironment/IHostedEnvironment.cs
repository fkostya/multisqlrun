using appui.models.HostedEnvironment;

namespace appui.shared.HostedEnvironment
{
    public interface IHostedEnvironment
    {
        Task Execute(object args);

        public Task<ConfigSettingsJson> GetConfigFromSecureStorage(string filename);

        Task<string> GetConfigFullPath(string fileName);
    }
}
