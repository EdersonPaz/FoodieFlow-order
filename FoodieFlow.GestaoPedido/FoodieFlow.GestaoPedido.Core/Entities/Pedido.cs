using FoodieFlow.GestaoPedido.Core.Enum;
using FoodieFlow.GestaoPedido.SharedKernel.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodieFlow.GestaoPedido.Core.Entities
{
    public class Pedido : BaseEntity
    {
        public int IdCliente { get; set; }
        public string Token { get; set; }
        public EnumStatus Status { get; set; }
    }
}
