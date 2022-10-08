using appui.models.Interfaces;
using Microsoft.Data.SqlClient;
using System.Threading;

namespace appui.models
{
    public class MsSqlConnection : Connection
    {
        public string? DbServer { get; set; }
        public string? DbUserName { get; set; }
        public string? DbDatabase { get; set; }
        public string? DbPassword { get; set; }

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
    }
}