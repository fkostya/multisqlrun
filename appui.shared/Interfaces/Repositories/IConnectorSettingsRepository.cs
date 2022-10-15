using appui.models;

namespace appui.shared.Interfaces.Repositories
{
    public interface IConnectorSettingsRepository
    {
        Task<IEnumerable<ConnectorSetting>> GetConnectorSettings();
    }
}