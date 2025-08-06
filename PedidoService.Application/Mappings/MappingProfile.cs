using AutoMapper;
using PedidoService.Application.DTOs;
using PedidoService.Domain.Entities;

namespace PedidoService.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pedido, PedidoDto>();
            CreateMap<PedidoItem, PedidoItemDto>();
        }
    }
}
