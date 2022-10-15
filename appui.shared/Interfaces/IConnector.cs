namespace appui.shared.Interfaces
{
    /// <summary>
    /// Connector to database connection string provider like DF infra or html page with all data.
    /// </summary>
    public interface IConnector
    {
        /// <summary>
        /// Load connection strings from connection string provider
        /// </summary>
        /// <returns></returns>
        Task<IList<IConnectionStringInfo>> LoadConnectionStrings(Dictionary<string, object> args);
    }
}
