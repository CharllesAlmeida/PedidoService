using PedidoService.Domain.Entities;
using PedidoService.Domain.Interfaces;
using PedidoService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PedidoService.Infrastructure.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidoDbContext _context;

        public PedidoRepository(PedidoDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<Pedido> ObterPorIdAsync(Guid pedidoId)
        {
            return await _context.Pedidos
                .AsNoTracking() 
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }
    }
}
