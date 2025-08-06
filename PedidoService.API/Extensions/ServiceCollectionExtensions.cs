using PedidoService.Application.Mappings;
using PedidoService.Domain.Interfaces;
using PedidoService.Infrastructure.Repository;

namespace PedidoService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
