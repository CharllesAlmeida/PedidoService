using MediatR;
using PedidoService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoService.Application.Commands
{
    public class CriarPedidoCommand : IRequest<Guid>
    {
        public CriarPedidoDto Pedido { get; }

        public CriarPedidoCommand(CriarPedidoDto pedido)
        {
            Pedido = pedido;
        }
    }
}
