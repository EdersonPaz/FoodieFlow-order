using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using FoodieFlow.GestaoPedido.Core.Entities;

namespace FoodieFlow.GestaoPedido.Core.Services
{
    public class AwsService : IAwsService
    {
        private readonly ILogger<AwsService> _logger;

        public AwsService(ILogger<AwsService> logger)
        {
            _logger = logger;
        }


        public async Task<string> ObterQueueUrl(AmazonSQSClient sqsClient, string queueName)
        {
            _logger.LogInformation($"(AwsService) Busca pela fila sqs: {queueName}");
            var getQueueUrlRequest = new GetQueueUrlRequest
            {
                QueueName = queueName
            };
            GetQueueUrlResponse getQueueUrlResponse = await sqsClient.GetQueueUrlAsync(getQueueUrlRequest);
            _logger.LogInformation($"(AwsService) Url encontrada para a a fila {queueName}: {getQueueUrlResponse.QueueUrl} ");

            return getQueueUrlResponse.QueueUrl;
        }


        public async Task EnviarMensagemAsync(string urlFila, string message)
        {
            using (var client = new AmazonSQSClient(RegionEndpoint.USEast1))
            {
                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = urlFila,
                    MessageBody = message
                };

                await client.SendMessageAsync(sendMessageRequest);
            }
        }

        public async Task<List<Message>> ObterMensagensAsync(string urlFila)
        {
            using (var client = new AmazonSQSClient(RegionEndpoint.USEast1))
            {
                var receiveMessageRequest = new ReceiveMessageRequest
                {
                    QueueUrl = urlFila,
                    MaxNumberOfMessages = 10
                };

                var response = await client.ReceiveMessageAsync(receiveMessageRequest);

                return response.Messages;
            }
        }

        public async Task DeletarMensagemAsync(string urlFila, string codigoMensagem)
        {
            using (var client = new AmazonSQSClient(RegionEndpoint.USEast1))
            {
                var deleteMessageRequest = new DeleteMessageRequest
                {
                    QueueUrl = urlFila,
                    ReceiptHandle = codigoMensagem
                };

                await client.DeleteMessageAsync(deleteMessageRequest);
            }
        }

        public void MoverMensagemFila(string urlFilaOrigem, string urlFilaDestino, Message mensagem)
        {
            EnviarMensagemAsync(urlFilaDestino, mensagem.Body);
            DeletarMensagemAsync(urlFilaOrigem, mensagem.ReceiptHandle);
        }

        //-----------------------------------------------------------------------
        public async Task<SecretManager> BuscarSenhaSecretAsync(string chave)
        {
            try
            {
                _logger.LogInformation($"(AwsService) Metodo de BuscarSecret:{chave} iniciado");

                using (var client = new AmazonSecretsManagerClient(RegionEndpoint.USEast1))
                {
                    var request = new GetSecretValueRequest
                    {
                        SecretId = chave
                    };

                    GetSecretValueResponse response = await client.GetSecretValueAsync(request);

                    SecretManager secretManager = JsonConvert.DeserializeObject<SecretManager>(response.SecretString);

                    _logger.LogInformation($"(AwsService) Fim metodo BuscarSecret");

                    return secretManager;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao Buscar Secret : " + ex.Message);
                throw;
            }
        }


        public async Task EscreverArquivoS3Async(MemoryStream conteudo, string bucket, string pasta, string nomeArquivo, string tipoArquivo)
        {
            try
            {
                _logger.LogInformation($"(AwsService) Inicio metodo moverS3");

                string caminhoArquivo = string.IsNullOrEmpty(pasta) ? nomeArquivo : $"{pasta}/{nomeArquivo}";

                using (var client = new AmazonS3Client(RegionEndpoint.USEast1))
                {
                    var putObjectRequest = new PutObjectRequest
                    {
                        BucketName = bucket,
                        Key = caminhoArquivo,
                        InputStream = conteudo,
                        ContentType = tipoArquivo
                    };

                    var response = await client.PutObjectAsync(putObjectRequest);

                    _logger.LogInformation($"(AwsService) Fim metodo moverS3");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"(AwsService) Erro ao escrever arquivo no bucket | {ex.Message} | {nomeArquivo}");
                throw;
            }
        }


        public async Task<bool> VerificarBucketExisteS3(string bucketName)
        {
            try
            {
                using (var client = new AmazonS3Client(RegionEndpoint.SAEast1))
                {
                    await client.GetBucketLocationAsync(new GetBucketLocationRequest
                    {
                        BucketName = bucketName
                    });

                    return true;
                }
            }
            catch (AmazonS3Exception ex) when (ex.ErrorCode == "NoSuchBucket")
            {
                return false;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao verificar a existência do bucket: {ex.Message}");
                return false;
            }
        }


    }
}

