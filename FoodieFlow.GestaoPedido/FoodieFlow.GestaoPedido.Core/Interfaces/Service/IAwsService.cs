using Amazon.SQS.Model;
using FoodieFlow.GestaoPedido.Core.Entities;

namespace FoodieFlow.GestaoPedido.Core.Interfaces.Service
{
    public interface IAwsService
    {
        Task EnviarMensagemAsync(string urlFila, string message);
        Task<List<Message>> ObterMensagensAsync(string urlFila);
        Task DeletarMensagemAsync(string urlFila, string codigoMensagem);
        void MoverMensagemFila(string urlFilaOrigem, string urlFilaDestino, Message mensagem);
        Task<SecretManager> BuscarSenhaSecretAsync(string chave);
        Task EscreverArquivoS3Async(MemoryStream conteudo, string bucket, string pasta, string nomeArquivo, string tipoArquivo);
        Task<bool> VerificarBucketExisteS3(string bucketName);
    }
}
