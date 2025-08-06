
namespace PedidoService.Application.DTOs
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public List<PedidoItemDto> Itens { get; set; }
    }
}
