using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;
using PedidoService.Application.Configuration;
using PedidoService.Application.Queries;
using PedidoService.Domain.Interfaces;
using Xunit;

namespace PedidoService.Tests
{
    public class PedidoQueryServiceTests
    {
        [Fact]
        public async Task Deve_Retornar_Nulo_Se_Pedido_Nao_Encontrado()
        {
            var mockCache = new Mock<IDistributedCache>();
            var mockRepo = new Mock<IPedidoRepository>();
            var mockItemRepo = new Mock<IPedidoItemRepository>();
            var mockOptions = Options.Create(new CacheSettings { PedidoPorIdSegundos = 120 });

            mockRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Domain.Entities.Pedido)null!);

            var service = new PedidoQueryService(mockCache.Object, mockRepo.Object, mockItemRepo.Object, mockOptions);
            var result = await service.ObterPorIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }
    }
}