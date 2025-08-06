using MediatR;
using PedidoService.Application.DTOs;
using PedidoService.Application.Queries;
using PedidoService.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System;

namespace PedidoService.Application.Handlers
{
    public class ObterPedidoQueryHandler : IRequestHandler<ObterPedidoQuery, PedidoDto?>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoItemRepository _pedidoItemRepository;
        private readonly IDistributedCache _cache;

        public ObterPedidoQueryHandler(
            IPedidoRepository pedidoRepository,
            IPedidoItemRepository pedidoItemRepository,
            IDistributedCache cache)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoItemRepository = pedidoItemRepository;
            _cache = cache;
        }

        public async Task<PedidoDto?> Handle(ObterPedidoQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"pedido:{request.PedidoId}";

            var cachedBytes = await _cache.GetAsync(cacheKey, cancellationToken);
            if (cachedBytes != null && cachedBytes.Length > 0)
            {
                try
                {
                    var cachedPedido = JsonSerializer.Deserialize<PedidoDto>(cachedBytes);
                    if (cachedPedido != null)
                    {
                        return cachedPedido;
                    }
                }
                catch
                {
                }
            }

            var pedido = await _pedidoRepository.ObterPorIdAsync(request.PedidoId);
            if (pedido == null)
                return null;

            var itens = await _pedidoItemRepository.ObterPorPedidoIdAsync(request.PedidoId);

            var pedidoDto = new PedidoDto
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                Data = pedido.Data,
                Status = pedido.Status.ToString(), 
                Itens = itens.Select(i => new PedidoItemDto
                {
                    Produto = i.Produto,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            };

            try
            {
                var bytes = JsonSerializer.SerializeToUtf8Bytes(pedidoDto);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                };
                await _cache.SetAsync(cacheKey, bytes, options, cancellationToken);
            }
            catch
            {
            }

            return pedidoDto;
        }
    }
}
