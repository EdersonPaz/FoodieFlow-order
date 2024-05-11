using FoodieFlow.GestaoPedido.Core.Entities.Request;
using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodieFlow.GestaoPedido.Core.Interfaces.Service
{
    public interface IPedidoService : IBaseService<Pedido>
    {
        Pedido New(MensagemSQS mensagem, Cliente cliente);
        void UpdateStatus(int id, EnumStatus status);
        List<Pedido> GetAll(EnumStatus? status);
    }

}
