using appui.connectors.Utils;
using appui.models;
using appui.models.MsSql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace appui.connectors.Extensions
{
    public static class ServiceCollectionEx
    {
        private static MsSqlConnection Create(string server, string database, string userid, string password)
        {
            return new MsSqlConnection(server, database, userid, password);
        }

        public static IServiceCollection AddConnectorsServices(this IServiceCollection service)
        {
            return
                service
                .AddSingleton<Func<string, SqlConnectionWrapper>>((connectionString) => new SqlConnectionWrapper(connectionString))
                .AddTransient<MsSqlQueryConnector>()
                .AddTransient<MsSqlQueryConnector>()
                .AddTransient<MsSqlDbServer>()
                .AddTransient<MsSqlDbDatabase>()
                .AddTransient<MsSqlDbUserName>()
                .AddTransient<MsSqlDbPassword>()
                .AddTransient<Func<string, string, string, string, MsSqlConnection>>(p => Create);
        }
    }
}