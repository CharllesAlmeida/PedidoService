using PedidoService.Domain.Enums;

namespace PedidoService.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public DateTime Data { get; private set; }
        public PedidoStatus Status { get; private set; }

        private Pedido() { }

        public Pedido(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            Data = DateTime.UtcNow;
            Status = PedidoStatus.Pendente;
        }

        public void MarcarComoConfirmado() => Status = PedidoStatus.Confirmado;
        public void Cancelar() => Status = PedidoStatus.Cancelado;
    }
}
