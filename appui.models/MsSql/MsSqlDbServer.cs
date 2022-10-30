using System.Diagnostics.CodeAnalysis;

namespace appui.models.MsSql
{
    [ExcludeFromCodeCoverage]
    public class MsSqlDbServer
    {
        public string ServerName { get; }

        public MsSqlDbServer(string servername)
        {
            ServerName = servername;
        }
    }
}