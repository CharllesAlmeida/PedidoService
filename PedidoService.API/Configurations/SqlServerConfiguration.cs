using Microsoft.EntityFrameworkCore;
using PedidoService.Infrastructure.Persistence;

namespace PedidoService.API.Configurations
{
    public static class SqlServerConfiguration
    {
        public static void ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServer") ?? "";

            if (!connectionString.Contains("Encrypt=", StringComparison.OrdinalIgnoreCase))
                connectionString += ";Encrypt=False";

            if (!connectionString.Contains("TrustServerCertificate=", StringComparison.OrdinalIgnoreCase))
                connectionString += ";TrustServerCertificate=True";

            if (!connectionString.Contains("Integrated Security=", StringComparison.OrdinalIgnoreCase))
                connectionString += ";Integrated Security=False";

            Console.WriteLine($"SQL Server Connection String: {connectionString}");

            services.AddDbContext<PedidoDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()));
        }
    }
}
