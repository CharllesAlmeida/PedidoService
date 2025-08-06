

namespace PedidoService.Application.DTOs
{
    public class CriarPedidoDto
    {
        public Guid ClienteId { get; set; }

        public List<ItemDto> Itens { get; set; } = new();

        public class ItemDto
        {
            public string Produto { get; set; }
            public int Quantidade { get; set; }
            public decimal PrecoUnitario { get; set; }
        }
    }
}
