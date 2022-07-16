namespace appui.shared.Interfaces
{
    public interface ILoadSource
    {
        IEnumerable<IConnectionRecord> GetConnections();
    }
}
