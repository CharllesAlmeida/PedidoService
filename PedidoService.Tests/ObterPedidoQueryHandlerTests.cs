using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PedidoService.Application.DTOs;
using PedidoService.Application.Handlers;
using PedidoService.Application.Queries;
using PedidoService.Domain.Entities;
using PedidoService.Domain.Enums;
using PedidoService.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System;
using Xunit;

namespace PedidoService.Tests
{
    public class ObterPedidoQueryHandlerTests
    {
        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoPedidoNaoExiste()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();

            var mockPedidoRepo = new Mock<IPedidoRepository>();

            mockPedidoRepo
                .Setup(r => r.ObterPorIdAsync(pedidoId))
                .ReturnsAsync((Pedido)null!);

            var mockPedidoItemRepo = new Mock<IPedidoItemRepository>();

            mockPedidoItemRepo
                .Setup(r => r.ObterPorPedidoIdAsync(pedidoId))
                .ReturnsAsync(new List<PedidoItem>());

            var mockCache = new Mock<IDistributedCache>();

            mockCache.Setup(c => c.GetAsync($"pedido:{pedidoId}", It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]?)null);
            mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new ObterPedidoQueryHandler(
                mockPedidoRepo.Object,
                mockPedidoItemRepo.Object,
                mockCache.Object);

            var result = await handler.Handle(new ObterPedidoQuery(pedidoId), CancellationToken.None);

            // Assert
            Assert.Null(result);

            mockPedidoItemRepo.Verify(r => r.ObterPorPedidoIdAsync(It.IsAny<Guid>()), Times.Never);
            mockCache.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRetornarPedidoDto_ComItensQuandoPedidoExiste()
        {
            // Arrange
            var clienteId = Guid.NewGuid();

            var pedido = new Pedido(clienteId);

            pedido.MarcarComoConfirmado();

            var pedidoId = pedido.Id;
            var data = pedido.Data;

            var itens = new List<PedidoItem>
            {
                new PedidoItem(pedidoId, "Produto A", 2, 10m),
                new PedidoItem(pedidoId, "Produto B", 1, 5m)
            };

            var mockPedidoRepo = new Mock<IPedidoRepository>();
            mockPedidoRepo
                .Setup(r => r.ObterPorIdAsync(pedidoId))
                .ReturnsAsync(pedido);

            var mockPedidoItemRepo = new Mock<IPedidoItemRepository>();
            mockPedidoItemRepo
                .Setup(r => r.ObterPorPedidoIdAsync(pedidoId))
                .ReturnsAsync(itens);

            var mockCache = new Mock<IDistributedCache>();

            mockCache
                .Setup(c => c.GetAsync($"pedido:{pedidoId}", It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]?)null);

            mockCache
                .Setup(c => c.SetAsync($"pedido:{pedidoId}", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var handler = new ObterPedidoQueryHandler(
                mockPedidoRepo.Object,
                mockPedidoItemRepo.Object,
                mockCache.Object);

            // Act
            var result = await handler.Handle(new ObterPedidoQuery(pedidoId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pedidoId, result!.Id);
            Assert.Equal(clienteId, result.ClienteId);
            Assert.Equal(data, result.Data);
            Assert.Equal(PedidoStatus.Confirmado.ToString(), result.Status);
            Assert.NotNull(result.Itens);
            Assert.Equal(itens.Count, result.Itens.Count);
            Assert.Contains(result.Itens, i => i.Produto == "Produto A" && i.Quantidade == 2 && i.PrecoUnitario == 10m);
            Assert.Contains(result.Itens, i => i.Produto == "Produto B" && i.Quantidade == 1 && i.PrecoUnitario == 5m);

            mockPedidoRepo.Verify(r => r.ObterPorIdAsync(pedidoId), Times.Once);
            mockPedidoItemRepo.Verify(r => r.ObterPorPedidoIdAsync(pedidoId), Times.Once);
            mockCache.Verify(c => c.SetAsync($"pedido:{pedidoId}", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}