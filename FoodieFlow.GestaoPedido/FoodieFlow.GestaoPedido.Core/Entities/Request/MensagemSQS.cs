using FoodieFlow.GestaoPedido.Core.Enum;
using FoodieFlow.GestaoPedido.SharedKernel.Bases;

namespace FoodieFlow.GestaoPedido.Core.Entities.Request
{
    public class MensagemSQS : BaseEntity
    {
        public float TransactionAmount { get; set; }
        public string Token { get; set; }
        public string Description { get; set; }
        public EnumMetodoPagamento PaymentMethodId { get; set; }
        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public string PayerDocument { get; set; }
        public EnumStatus Status { get; set; }
    }
}
