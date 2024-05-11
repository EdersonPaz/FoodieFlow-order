using FoodieFlow.GestaoPedido.Core.Entities;

namespace FoodieFlow.GestaoPedido.Core.Interfaces.Repository
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Cliente GetByCPF(string cpf);
    }
}
