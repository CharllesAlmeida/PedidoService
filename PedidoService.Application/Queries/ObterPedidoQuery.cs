using MediatR;
using PedidoService.Application.DTOs;

namespace PedidoService.Application.Queries
{
    public class ObterPedidoQuery : IRequest<PedidoDto?>
    {
        public Guid PedidoId { get; }

        public ObterPedidoQuery(Guid pedidoId)
        {
            PedidoId = pedidoId;
        }
    }
}
