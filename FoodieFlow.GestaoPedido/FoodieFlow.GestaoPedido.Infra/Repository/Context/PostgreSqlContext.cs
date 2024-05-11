using FoodieFlow.GestaoPedido.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodieFlow.GestaoPedido.Infra.Repository.Context
{
    public class PostgreSqlContext : DbContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Habilitar detalhes de erros (apenas para desenvolvimento)
            optionsBuilder.EnableDetailedErrors();
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }


    }
}
