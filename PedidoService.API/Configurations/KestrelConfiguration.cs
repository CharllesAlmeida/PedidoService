namespace PedidoService.API.Configurations
{
    public static class KestrelConfiguration
    {
        public static void ConfigureKestrel(this WebApplicationBuilder builder)
        {
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(8080);
            });
        }
    }

}
