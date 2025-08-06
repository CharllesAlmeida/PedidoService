using PedidoService.Application.Interfaces;
using PedidoService.Application.Services;
using PedidoService.Domain.Interfaces;
using PedidoService.Infrastructure.Messaging;
using Polly;

namespace PedidoService.API.Configurations
{
    public static class RabbitMqConfiguration
    {
        public static void ConfigureRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var hostname = configuration.GetValue<string>("RabbitMq:HostName");
            Console.WriteLine($"RabbitMQ Hostname: {hostname}");

            services.AddSingleton<IRabbitMqPublisher>(serviceProvider =>
            {
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (exception, timeSpan, retryCount, _) =>
                        {
                            Console.WriteLine($"Tentativa {retryCount} de conexão ao RabbitMQ falhou. Esperando {timeSpan} antes da próxima tentativa.");
                        });

                return retryPolicy.Execute(() => new RabbitMqPublisher(hostname));
            });

            services.AddScoped<IPublisherService, PublisherService>();
        }
    }
}
