namespace appui.shared.Interfaces
{
    public interface ILoadSource
    {
        IEnumerable<IConnectionStringInfo> GetConnections();
    }
}
