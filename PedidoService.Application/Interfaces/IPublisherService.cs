using PedidoService.Application.Events;

namespace PedidoService.Application.Interfaces
{
    public interface IPublisherService
    {
        Task PublicarPedidoCriadoAsync(PedidoCriadoIntegrationEvent evento);
    }
}
