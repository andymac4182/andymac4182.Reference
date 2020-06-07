using andymac4182.Reference.Data;
using andymac4182.Reference.Service.Infrastructure.Configuration;
using Insight.Database.Reliable;
using Insight.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace andymac4182.Reference.Service.Configure
{
    public static class Persistence
    {
        public static IServiceCollection AddCustomPersistence(this IServiceCollection serviceCollection)
        {
            SqlInsightDbProvider.RegisterProvider();

            serviceCollection.AddSingleton<IExampleDbRepository>(context =>
            {
                var connectionString = context.GetRequiredService<DatabaseConnectionStringSetting>().Value;

                var innerConnection = new SqlConnection(connectionString);

                return new ReliableConnection(innerConnection).AsParallel<IExampleDbRepository>();
            });

            return serviceCollection;
        }
    }
}