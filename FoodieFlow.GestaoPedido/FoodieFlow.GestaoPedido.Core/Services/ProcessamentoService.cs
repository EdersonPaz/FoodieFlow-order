using FoodieFlow.GestaoPedido.Core.Entities.Request;
using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using Microsoft.Extensions.Configuration;
using System.Text;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using FoodieFlow.GestaoPedido.SharedKernel;
using Microsoft.Extensions.Logging;

namespace FoodieFlow.GestaoPedido.Core.Services
{
    public class ProcessamentoService : IProcessamentoService
    {
        private readonly IAwsService _awsService;
        private readonly IConfiguration _configuration;
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;
        private readonly ILogger<PedidoService> _logger;
        private readonly string _urlFilaEntradaProcessamento;
        private readonly string _urlFilaDLQ;
        private readonly string _bucketS3;
        private readonly string _pastaS3;

        public ProcessamentoService(
            IAwsService awsService, 
            IConfiguration configuration,
            IPedidoService pedidoService, 
            IClienteService clienteService, 
            ILogger<PedidoService> logger)
        {
            _awsService = awsService;
            _configuration = configuration;
            _pedidoService = pedidoService;
            _clienteService = clienteService;
            _logger = logger;

            if (_configuration.GetSection("AWS") != null)
            {
                if (_configuration.GetSection("AWS")["UrlFilaEntradaProcessamento"] != null)
                    _urlFilaEntradaProcessamento = _configuration.GetSection("AWS")["UrlFilaEntradaProcessamento"].ToString();

                if (_configuration.GetSection("AWS")["UrlFilaDLQ"] != null)
                    _urlFilaDLQ = _configuration.GetSection("AWS")["UrlFilaDLQ"].ToString();

                if (_configuration.GetSection("AWS")["BucketS3"] != null)
                    _bucketS3 = _configuration.GetSection("AWS")["BucketS3"].ToString();

                if (_configuration.GetSection("AWS")["PastaS3"] != null)
                    _pastaS3 = _configuration.GetSection("AWS")["PastaS3"].ToString();
            }
        }

        public void ProcessarMensagemSQS(MensagemSQS mensagem)
        {
            _logger.LogInformation("Iniciando o processamento da mensagem SQS...");
            _logger.LogInformation("Criando um novo cliente...");
            Cliente cliente = _clienteService.New(mensagem);

            _logger.LogInformation("Criando um novo pedido...");
            Pedido pedido = _pedidoService.New(mensagem, cliente);

            _logger.LogInformation("Processamento da mensagem SQS concluído.");
        }

        public void ConsumirFilaProcessarMensagens()
        {
            _logger.LogInformation("Iniciando a consumação da fila...");

            try
            {
                // Obter as mensagens da fila
                _logger.LogInformation("Obtendo mensagens da fila...");
                Task<List<Message>> mensagens = _awsService.ObterMensagensAsync(_urlFilaEntradaProcessamento);

                foreach (Message mensagem in mensagens.Result)
                {
                    try
                    {
                        _logger.LogInformation("Deserializando a mensagem...");
                        MensagemSQS mensagemSQS = JsonConvert.DeserializeObject<MensagemSQS>(mensagem.Body);

                        _logger.LogInformation("Processando a mensagem...");
                        ProcessarMensagemSQS(mensagemSQS);

                        _logger.LogInformation("Escrevendo o conteúdo no S3...");
                        MemoryStream conteudo = new MemoryStream(Encoding.UTF8.GetBytes(mensagem.Body));
                        _awsService.EscreverArquivoS3Async(conteudo, _bucketS3, _pastaS3, mensagem.MessageId, "text/plain");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Erro durante o processamento da mensagem: {ex.Message}");
                        _logger.LogInformation("Movendo a mensagem para a fila DLQ...");
                        _awsService.MoverMensagemFila(_urlFilaEntradaProcessamento, _urlFilaDLQ, mensagem);
                    }

                    _logger.LogInformation("Deletando a mensagem da fila original...");
                    _awsService.DeletarMensagemAsync(_urlFilaEntradaProcessamento, mensagem.ReceiptHandle);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao consumir a fila e processar as mensagens: {ex.Message}");
                throw;
            }

            _logger.LogInformation("Consumação da fila concluída.");
        }

    }
}


