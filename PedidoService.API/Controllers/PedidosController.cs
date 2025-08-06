using MediatR;
using Microsoft.AspNetCore.Mvc;
using PedidoService.Application.Commands;
using PedidoService.Application.DTOs;
using PedidoService.Application.Queries; 
namespace PedidoService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoQueryService _queryService;
        private readonly IMediator _mediator;
        public PedidosController(IPedidoQueryService queryService, IMediator mediator)
        {
            _queryService = queryService;
            _mediator = mediator;
        }
    


        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _mediator.Send(new CriarPedidoCommand(dto));
            return CreatedAtAction(nameof(ObterPedido), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPedido(Guid id)
        {
            var resultado = await _mediator.Send(new ObterPedidoQuery(id));
            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }
    }
}
