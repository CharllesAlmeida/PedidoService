using PedidoService.Application.Handlers;

namespace PedidoService.API.Extensions
{
    public static class MediatRExtension
    {
        public static void AddMediatRHandlers(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CriarPedidoCommandHandler).Assembly));
        }
    }

}
