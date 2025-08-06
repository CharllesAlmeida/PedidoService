

namespace PedidoService.Infrastructure.Messaging.Events
{
    public class PedidoCriadoIntegrationEvent
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime Data { get; set; }
        public decimal Total { get; set; }

        public PedidoCriadoIntegrationEvent(Guid pedidoId, Guid clienteId, DateTime data, decimal total)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Data = data;
            Total = total;
        }
    }
}

