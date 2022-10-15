using appui.shared.Interfaces;

namespace appui.shared
{
    public class EmptyConnector : IConnector
    {
        public IPageReader GetReader()
        {
            return null;
        }

        public Task<IList<IConnectionStringInfo>> LoadConnectionStrings(Dictionary<string, object> args)
        {
            throw new NotImplementedException();
        }
    }
}
