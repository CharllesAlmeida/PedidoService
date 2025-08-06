using MediatR;

namespace PedidoService.Domain.Events.PedidoService.Domain.Events
{
    public class PedidoCriadoEvent : INotification
    {
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public DateTime Data { get; }
        public decimal Total { get; }

        public PedidoCriadoEvent(Guid pedidoId, Guid clienteId, DateTime data, decimal total)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Data = data;
            Total = total;
        }
    }
}
