using FoodieFlow.GestaoPedido.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodieFlow.GestaoPedido.Infra.Repository.Context
{
    public class PostgreSqlContext : DbContext
    {
        private readonly string _connectionString;

        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Se uma string de conexão foi fornecida, use-a para configurar o DbContext
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }

            // Habilitar detalhes de erros (apenas para desenvolvimento)
            optionsBuilder.EnableDetailedErrors();
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
    }

}
