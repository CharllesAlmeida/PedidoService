using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PedidoService.Application.Commands;
using PedidoService.Application.DTOs;
using PedidoService.Application.Handlers;
using PedidoService.Domain.Entities;
using PedidoService.Domain.Interfaces;
using PedidoService.Application.Interfaces;
using PedidoService.Application.Events;

namespace PedidoService.Tests
{
    public class CriarPedidoCommandHandlerTests
    {
        [Fact]
        public async Task Deve_Criar_Pedido_Quando_Comando_For_Valido()
        {
            // Arrange
            var mockPedidoRepo = new Mock<IPedidoRepository>();
            var mockItemRepo = new Mock<IPedidoItemRepository>();
            var mockPublisher = new Mock<IPublisherService>();

            var handler = new CriarPedidoCommandHandler(
                mockPedidoRepo.Object,
                mockItemRepo.Object,
                mockPublisher.Object
            );

            var dto = new CriarPedidoDto
            {
                ClienteId = Guid.NewGuid(),
                Itens = new List<CriarPedidoDto.ItemDto>
                {
                    new CriarPedidoDto.ItemDto
                    {
                        Produto = "Produto A",
                        Quantidade = 2,
                        PrecoUnitario = 50m
                    }
                }
            };

            var command = new CriarPedidoCommand(dto);

            // Act
            var pedidoId = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, pedidoId);
            mockPedidoRepo.Verify(r => r.AdicionarAsync(It.IsAny<Pedido>()), Times.Once);
            mockItemRepo.Verify(r => r.SalvarItensAsync(It.IsAny<List<PedidoItem>>()), Times.Once);
            mockPublisher.Verify(p => p.PublicarPedidoCriadoAsync(It.IsAny<PedidoCriadoIntegrationEvent>()), Times.Once);
        }
    }
}
