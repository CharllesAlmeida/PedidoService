using PedidoService.Domain.Entities;
using PedidoService.Domain.Interfaces;
using MongoDB.Driver;

namespace PedidoService.Infrastructure.Repository
{
    public class PedidoItemRepository : IPedidoItemRepository
    {
        private readonly IMongoCollection<PedidoItem> _pedidoItensCollection;

        public PedidoItemRepository(IMongoClient mongoClient, string databaseName)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _pedidoItensCollection = database.GetCollection<PedidoItem>("PedidoItens");
        }

        public async Task SalvarItensAsync(IEnumerable<PedidoItem> itens)
        {
            await _pedidoItensCollection.InsertManyAsync(itens);
        }

        public async Task<List<PedidoItem>> ObterPorPedidoIdAsync(Guid pedidoId)
        {
            var filter = Builders<PedidoItem>.Filter.Eq(item => item.PedidoId, pedidoId);
            return await _pedidoItensCollection.Find(filter).ToListAsync();
        }
    }
}
