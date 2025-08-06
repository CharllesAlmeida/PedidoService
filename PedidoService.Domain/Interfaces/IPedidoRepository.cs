using PedidoService.Domain.Entities;

namespace PedidoService.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task AdicionarAsync(Pedido pedido);
        Task<Pedido> ObterPorIdAsync(Guid pedidoId);
    }
}
