using appui.models;
using appui.shared.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace appui.shared.Repositories
{
    public class ConnectorSettingsAppSettingRepository : IConnectorSettingsRepository
    {
        private readonly IEnumerable<ConnectorSetting> _settings;

        public ConnectorSettingsAppSettingRepository(IOptions<List<ConnectorSetting>> settings)
        {
            this._settings = settings.Value.AsEnumerable() ?? new List<ConnectorSetting>();
        }

        public async Task<IEnumerable<ConnectorSetting>> GetConnectorSettings()
        {
            return await Task.FromResult(_settings);
        }
    }
}