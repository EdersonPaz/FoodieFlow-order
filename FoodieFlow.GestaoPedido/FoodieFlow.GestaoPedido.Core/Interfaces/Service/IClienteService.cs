using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Entities.Request;

namespace FoodieFlow.GestaoPedido.Core.Interfaces.Service
{
    public interface IClienteService : IBaseService<Cliente> 
    {
        Cliente New(MensagemSQS mensagem);
    }
}
