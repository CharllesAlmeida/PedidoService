using System;

namespace PedidoService.Application.Events
{
    public class PedidoCriadoIntegrationEvent
    {
        public Guid PedidoId { get; }
        public Guid ClienteId { get; }
        public DateTime Data { get; }
        public decimal Total { get; }

        public PedidoCriadoIntegrationEvent(Guid pedidoId, Guid clienteId, DateTime data, decimal total)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Data = data;
            Total = total;
        }
    }
}