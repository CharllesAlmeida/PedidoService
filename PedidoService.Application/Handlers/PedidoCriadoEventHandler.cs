using MediatR;
using PedidoService.Domain.Events.PedidoService.Domain.Events;
using PedidoService.Domain.Interfaces;

namespace PedidoService.Application.Handlers
{
    public class PedidoCriadoEventHandler : INotificationHandler<PedidoCriadoEvent>
    {
        private readonly IRabbitMqPublisher _publisher;

        public PedidoCriadoEventHandler(IRabbitMqPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Handle(PedidoCriadoEvent notification, CancellationToken cancellationToken)
        {
            await _publisher.PublicarAsync(notification);
        }
    }
}
