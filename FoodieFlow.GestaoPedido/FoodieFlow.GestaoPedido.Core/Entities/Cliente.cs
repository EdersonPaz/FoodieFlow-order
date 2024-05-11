using FoodieFlow.GestaoPedido.SharedKernel.Bases;

namespace FoodieFlow.GestaoPedido.Core.Entities
{
    public class Cliente : BaseEntity
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

}
