using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodieFlow.GestaoPedido.Core.Interfaces.Repository
{
    public interface IPedidoRepository : IBaseRepository<Pedido>
    {
        List<Pedido> GetAll(EnumStatus? status);
    }

}
