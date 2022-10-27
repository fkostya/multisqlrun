using appui.shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace appui.shared
{
    [ExcludeFromCodeCoverage]
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
