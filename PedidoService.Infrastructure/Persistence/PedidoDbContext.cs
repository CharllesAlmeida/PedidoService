using Microsoft.EntityFrameworkCore;
using PedidoService.Domain.Entities;

namespace PedidoService.Infrastructure.Persistence
{
    public class PedidoDbContext : DbContext
    {
        public PedidoDbContext(DbContextOptions<PedidoDbContext> options) : base(options)
        {
        }

        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("Pedidos");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.ClienteId)
                      .IsRequired();

                entity.Property(p => p.Data)
                      .IsRequired();

                entity.Property(p => p.Status)
                      .IsRequired()
                      .HasMaxLength(20);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
