using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using PedidoService.Domain.Interfaces;
using PedidoService.Infrastructure.Repository;

namespace PedidoService.API.Configurations
{
    public static class MongoConfiguration
    {
        public static void ConfigureMongo(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb") ?? "";
            var databaseName = configuration.GetValue<string>("MongoDatabaseName");

            Console.WriteLine($"MongoDB Connection String: {connectionString}");
            Console.WriteLine($"MongoDB Database Name: {databaseName}");

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
            services.AddScoped<IPedidoItemRepository>(sp =>
                new PedidoItemRepository(sp.GetRequiredService<IMongoClient>(), databaseName));
        }
    }

}
