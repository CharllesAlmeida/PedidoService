using MediatR;
using PedidoService.Application.Commands;
using PedidoService.Application.Interfaces;
using PedidoService.Domain.Entities;
using PedidoService.Application.Events;
using PedidoService.Domain.Interfaces;

namespace PedidoService.Application.Handlers
{
    public class CriarPedidoCommandHandler : IRequestHandler<CriarPedidoCommand, Guid>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoItemRepository _itemRepository;
        private readonly IPublisherService _publisherService;

        public CriarPedidoCommandHandler(
            IPedidoRepository pedidoRepository,
            IPedidoItemRepository itemRepository,
            IPublisherService publisherService)
        {
            _pedidoRepository = pedidoRepository;
            _itemRepository = itemRepository;
            _publisherService = publisherService;
        }

        public async Task<Guid> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Pedido;

            if (dto.ClienteId == Guid.Empty)
                throw new ArgumentException("ClienteId deve ser informado.");

            if (dto.Itens == null || !dto.Itens.Any())
                throw new ArgumentException("O pedido deve conter pelo menos um item.");

            foreach (var item in dto.Itens)
            {
                if (string.IsNullOrWhiteSpace(item.Produto))
                    throw new ArgumentException("O produto é obrigatório.");
                if (item.Quantidade <= 0)
                    throw new ArgumentException("A quantidade deve ser maior que zero.");
                if (item.PrecoUnitario <= 0)
                    throw new ArgumentException("O preço unitário deve ser maior que zero.");
            }

            var pedido = new Pedido(dto.ClienteId);
            await _pedidoRepository.AdicionarAsync(pedido);

            var itens = dto.Itens.Select(item =>
                new PedidoItem(
                    pedido.Id,
                    item.Produto,
                    item.Quantidade,
                    item.PrecoUnitario
                )
            ).ToList();

            await _itemRepository.SalvarItensAsync(itens);

            var total = itens.Sum(i => i.CalcularSubtotal());

            var integrationEvent = new PedidoCriadoIntegrationEvent(
                pedido.Id,
                pedido.ClienteId,
                pedido.Data,
                total
            );

            try
            {
                await _publisherService.PublicarPedidoCriadoAsync(integrationEvent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao publicar o evento PedidoCriado.", ex);
            }

            return pedido.Id;
        }
    }
}
