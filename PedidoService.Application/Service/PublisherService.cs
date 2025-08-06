using PedidoService.Application.Interfaces;
using PedidoService.Application.Events;
using PedidoService.Domain.Interfaces;

namespace PedidoService.Application.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IRabbitMqPublisher _rabbitMqPublisher;

        public PublisherService(IRabbitMqPublisher rabbitMqPublisher)
        {
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        public async Task PublicarPedidoCriadoAsync(PedidoCriadoIntegrationEvent evento)
        {
            await _rabbitMqPublisher.PublicarAsync(evento);
        }
    }
}
