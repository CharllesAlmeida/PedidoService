
using Newtonsoft.Json;
using PedidoService.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace PedidoService.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IRabbitMqPublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqPublisher(string rabbitMqHostName)
        {
            var factory = new ConnectionFactory { HostName = rabbitMqHostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Task PublicarAsync<T>(T mensagem) where T : class
        {
            var queueName = typeof(T).Name;
            var json = JsonConvert.SerializeObject(mensagem);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
