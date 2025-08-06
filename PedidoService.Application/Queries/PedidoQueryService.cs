using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using PedidoService.Application.DTOs;
using PedidoService.Domain.Interfaces;

using Microsoft.Extensions.Options;
using PedidoService.Application.Configuration;

namespace PedidoService.Application.Queries
{
    public interface IPedidoQueryService
    {
        Task<PedidoDto?> ObterPorIdAsync(Guid id);
    }

    public class PedidoQueryService : IPedidoQueryService
    {
        private readonly int _cacheDuration;
        private readonly IDistributedCache _cache;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoItemRepository _pedidoItemRepository;

        public PedidoQueryService(
            IDistributedCache cache,
            IPedidoRepository pedidoRepository,
            IPedidoItemRepository pedidoItemRepository, IOptions<CacheSettings> options)
        {
            _cache = cache;
            _pedidoRepository = pedidoRepository;
            _pedidoItemRepository = pedidoItemRepository;
        }

        public async Task<PedidoDto?> ObterPorIdAsync(Guid id)
        {
            var cacheKey = $"pedido:{id}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<PedidoDto>(cached);

            var pedido = await _pedidoRepository.ObterPorIdAsync(id);
            if (pedido == null) return null;

            var itens = await _pedidoItemRepository.ObterPorPedidoIdAsync(id);
            var dto = new PedidoDto
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

            var json = JsonSerializer.Serialize(dto);
            await _cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDuration)
            });

            return dto;
        }
    }
}