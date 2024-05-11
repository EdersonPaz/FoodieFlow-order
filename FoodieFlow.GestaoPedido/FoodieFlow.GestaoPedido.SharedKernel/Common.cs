namespace FoodieFlow.GestaoPedido.SharedKernel
{
    public static class Common
    {
        public static string ObterMensagemErro(Exception ex, int tamanhoMensagem = 0)
        {
            string mensagemErro = $"Erro ao enviar processamento para o motor de crédito : {ex.Message} ";
            mensagemErro += $" | Stack Trace: {ex.StackTrace} ";

            if (ex.InnerException != null)
            {
                mensagemErro += $" | Inner Exception : {ex.InnerException.Message} ";
                mensagemErro += $" | Inner Exception Stack Trace: {ex.InnerException.StackTrace} ";
            }

            if (tamanhoMensagem > 0 && mensagemErro.Length > tamanhoMensagem)
                mensagemErro = mensagemErro.Substring(0, tamanhoMensagem);

            return mensagemErro;
        }

    }
}
