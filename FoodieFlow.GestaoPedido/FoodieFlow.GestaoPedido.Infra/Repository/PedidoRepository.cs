using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using FoodieFlow.GestaoPedido.Infra.Repository.Context;
using FoodieFlow.GestaoPedido.Infra.Repository;
using Microsoft.Extensions.Configuration;
using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Interfaces.Repository;
using FoodieFlow.GestaoPedido.Core.Enum;

namespace FoodieFlow.GestaoPedido.Infra.Repository
{
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(PostgreSqlContext context, IAwsService awsService, IConfiguration configuration)
            : base(context, awsService, configuration)
        {
        }

        public List<Pedido> GetAll(EnumStatus? status)
        {
            if (status.HasValue)
            {
                return Get(p => p.Status == status, orderBy: q => q.OrderByDescending(p => p.DtCriacao)).ToList();
            }
            else
            {
                return Get(orderBy: q => q.OrderByDescending(p => p.DtCriacao)).ToList();
            }
        }
    
    }
}