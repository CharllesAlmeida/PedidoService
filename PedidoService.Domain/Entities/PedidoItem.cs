
namespace PedidoService.Domain.Entities
{
    public class PedidoItem
    {
        public Guid Id { get; private set; }            
        public Guid PedidoId { get; private set; }      
        public string Produto { get; private set; }
        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }

        private PedidoItem() { }

        public PedidoItem(Guid pedidoId, string produto, int quantidade, decimal precoUnitario)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            Produto = produto;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        public decimal CalcularSubtotal() => Quantidade * PrecoUnitario;
    }
}
