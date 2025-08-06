using Microsoft.Extensions.Caching.StackExchangeRedis;
namespace PedidoService.API.Configurations
{
    public static class RedisConfiguration
    {
        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Redis");

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = connectionString;
                });
            }
        }
    }
}