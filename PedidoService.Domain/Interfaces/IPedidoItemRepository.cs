using PedidoService.Domain.Entities;

namespace PedidoService.Domain.Interfaces
{
    public interface IPedidoItemRepository
    {
        Task SalvarItensAsync(IEnumerable<PedidoItem> itens);

        Task<List<PedidoItem>> ObterPorPedidoIdAsync(Guid pedidoId);
    }
}
