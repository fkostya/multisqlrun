using System.Diagnostics.CodeAnalysis;

namespace appui.models.MsSql
{
    [ExcludeFromCodeCoverage]
    public class MsSqlDbUserName
    {
        public string UserName { get; }

        public MsSqlDbUserName(string username)
        {
            UserName = username;
        }
    }
}