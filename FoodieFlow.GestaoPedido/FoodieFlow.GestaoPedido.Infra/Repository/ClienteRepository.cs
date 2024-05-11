using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Interfaces.Repository;
using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using FoodieFlow.GestaoPedido.Infra.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodieFlow.GestaoPedido.Infra.Repository
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(PostgreSqlContext context, IAwsService awsService, IConfiguration configuration)
            : base(context, awsService, configuration)
        {
        }

        public Cliente GetByCPF(string cpf)
        {
           return Get(c => c.Cpf == cpf).FirstOrDefault();
        }
    }
}
