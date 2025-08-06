using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using PedidoService.Application.DTOs;
using PedidoService.Application.Handlers;
using PedidoService.Application.Queries;
using PedidoService.Domain.Entities;
using PedidoService.Domain.Interfaces;
using Xunit;

namespace PedidoService.Tests
{
    public class ObterPedidoQueryHandlerCacheTests
    {
        [Fact]
        public async Task Handle_DeveRetornarPedidoDoCache_QuandoExistirCache()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                Id = Guid.NewGuid(),
                ClienteId = Guid.NewGuid(),
                Data = DateTime.UtcNow,
                Status = "Confirmado",
                Itens = new List<PedidoItemDto>
                {
                    new PedidoItemDto { Produto = "Produto A", Quantidade = 2, PrecoUnitario = 10m }
                }
            };

            var cacheKey = $"pedido:{pedidoDto.Id}";
            var cachedBytes = JsonSerializer.SerializeToUtf8Bytes(pedidoDto);

            var mockCache = new Mock<IDistributedCache>();
            mockCache
                .Setup(c => c.GetAsync(cacheKey, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cachedBytes);
            mockCache
                .Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var mockPedidoRepo = new Mock<IPedidoRepository>();
            var mockPedidoItemRepo = new Mock<IPedidoItemRepository>();

            var handler = new ObterPedidoQueryHandler(
                mockPedidoRepo.Object,
                mockPedidoItemRepo.Object,
                mockCache.Object);

            // Act
            var result = await handler.Handle(new ObterPedidoQuery(pedidoDto.Id), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pedidoDto.Id, result!.Id);
            Assert.Equal(pedidoDto.ClienteId, result.ClienteId);
            Assert.Equal(pedidoDto.Status, result.Status);
            Assert.Equal(pedidoDto.Itens.Count, result.Itens.Count);

            mockPedidoRepo.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
            mockPedidoItemRepo.Verify(r => r.ObterPorPedidoIdAsync(It.IsAny<Guid>()), Times.Never);
            mockCache.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveBuscarDoRepositorioEAdicionarAoCache_QuandoCacheNaoExistir()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var pedido = new Pedido(clienteId);
            pedido.MarcarComoConfirmado();
            var pedidoId = pedido.Id;

            var itens = new List<PedidoItem>
            {
                new PedidoItem(pedidoId, "Produto A", 2, 10m)
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

            byte[]? savedBytes = null;
            mockCache
                .Setup(c => c.SetAsync($"pedido:{pedidoId}", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
                .Callback<string, byte[], DistributedCacheEntryOptions, CancellationToken>((key, bytes, options, token) => savedBytes = bytes)
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
            Assert.Equal(pedido.Status.ToString(), result.Status);
            Assert.NotNull(result.Itens);
            Assert.Single(result.Itens);
            mockPedidoRepo.Verify(r => r.ObterPorIdAsync(pedidoId), Times.Once);
            mockPedidoItemRepo.Verify(r => r.ObterPorPedidoIdAsync(pedidoId), Times.Once);
            mockCache.Verify(c => c.SetAsync($"pedido:{pedidoId}", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(savedBytes);
            var cachedDto = JsonSerializer.Deserialize<PedidoDto>(savedBytes!);
            Assert.NotNull(cachedDto);
            Assert.Equal(result.Id, cachedDto!.Id);
        }
    }
}