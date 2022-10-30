using appui.models.Interfaces;
using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace appui.models
{
    [ExcludeFromCodeCoverage]

    public class MsSqlConnection : Connection
    {
        public virtual string DbServer { get; }
        public virtual string DbDatabase { get; }
        public string DbUserName { get; }
        public string DbPassword { get; }

        public MsSqlConnection()
            :this(string.Empty, string.Empty, string.Empty, string.Empty)
        {

        }

        public MsSqlConnection(string dbServer, string dbDatabase, string dbUserName, string dbPassword)
        {
            DbServer = dbServer;
            DbDatabase = dbDatabase;
            DbUserName = dbUserName;
            DbPassword = dbPassword;
        }

        public override T GetConnectionString<T>()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = DbServer,
                InitialCatalog = DbDatabase,
                UserID = DbUserName,
                Password = DbPassword
            };
            return (T)(object)builder.ConnectionString;
        }

        public override bool IsValid()
        {
            return !(string.IsNullOrEmpty(DbServer) ||
                    string.IsNullOrEmpty(DbDatabase) ||
                    string.IsNullOrEmpty(DbUserName) ||
                    string.IsNullOrEmpty(DbPassword));
        }
    }
}