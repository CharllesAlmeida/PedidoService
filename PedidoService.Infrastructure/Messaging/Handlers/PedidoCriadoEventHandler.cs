
using MediatR;
using PedidoService.Domain.Events.PedidoService.Domain.Events;
using PedidoService.Domain.Interfaces;
using PedidoService.Infrastructure.Messaging.Events;

namespace PedidoService.Infrastructure.Messaging.Handlers
{
    public class PedidoCriadoEventHandler : INotificationHandler<PedidoCriadoEvent>
    {
        private readonly IRabbitMqPublisher _publisher;

        public PedidoCriadoEventHandler(IRabbitMqPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Handle(PedidoCriadoEvent domainEvent, CancellationToken cancellationToken)
        {
            var integrationEvent = new PedidoCriadoIntegrationEvent(
                domainEvent.PedidoId,
                domainEvent.ClienteId,
                domainEvent.Data,
                domainEvent.Total
            );

            await _publisher.PublicarAsync(integrationEvent);
        }
    }
}
