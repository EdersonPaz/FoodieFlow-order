using FoodieFlow.GestaoPedido.Core.Entities.Request;

namespace FoodieFlow.GestaoPedido.Core.Interfaces.Service
{
    public interface IProcessamentoService
    {
        void ProcessarMensagemSQS(MensagemSQS mensagem);
        void ConsumirFilaProcessarMensagens();
    }
}