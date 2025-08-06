using Microsoft.EntityFrameworkCore;
using PedidoService.Infrastructure.Persistence;
using Polly;

public static class DBMigrationConfiguration
{
    public static void ApplyMigrations(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PedidoDbContext>();

        var retry = Policy
            .Handle<Exception>()
            .WaitAndRetry(5, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                (ex, time, retryCount, _) =>
                {
                    Console.WriteLine($"[Migration Retry {retryCount}] Aguarde {time.TotalSeconds}s. Erro: {ex.Message}");
                });

        retry.Execute(() =>
        {
            Console.WriteLine("Aplicando migrations...");
            db.Database.Migrate();
            Console.WriteLine("Migrations aplicadas com sucesso.");
        });
    }
}
