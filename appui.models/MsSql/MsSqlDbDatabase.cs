using System.Diagnostics.CodeAnalysis;

namespace appui.models.MsSql
{
    [ExcludeFromCodeCoverage]
    public class MsSqlDbDatabase
    {
        public string DatabaseName { get; }

        public MsSqlDbDatabase(string databasename)
        {
            DatabaseName = databasename;
        }
    }
}