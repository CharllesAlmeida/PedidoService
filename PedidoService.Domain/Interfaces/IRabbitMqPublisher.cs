namespace PedidoService.Domain.Interfaces
{
    public interface IRabbitMqPublisher
    {
        Task PublicarAsync<T>(T mensagem) where T : class;
    }
}
