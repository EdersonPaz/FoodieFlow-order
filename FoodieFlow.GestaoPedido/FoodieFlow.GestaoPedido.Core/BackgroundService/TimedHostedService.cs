using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using FoodieFlow.GestaoPedido.SharedKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Timers;

namespace FoodieFlow.GestaoPedido.Core.BackgroundService
{
    public class TimedHostedService : IHostedService
    {
        private readonly ILogger<TimedHostedService> _logger;
        private System.Timers.Timer _timerProcessamento;
        private System.Timers.Timer _timerProcessamentoSqs;
        private readonly IConfiguration _configuration;
        private readonly string _urlFilaEntradaProcessamento;
        private readonly IServiceProvider _serviceProvider;


        public TimedHostedService(
            IServiceProvider serviceProvider,
            ILogger<TimedHostedService> logger,
            IConfiguration configuration
         )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;

            if (configuration.GetSection("AWS") != null && configuration.GetSection("AWS")["UrlFilaEntradaProcessamento"] != null)
                _urlFilaEntradaProcessamento = configuration.GetSection("AWS")["UrlFilaEntradaProcessamento"].ToString();

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando serviço de background");

            string configExecutarBackground = "";
            if (_configuration.GetSection("ProcessarBackground") != null && _configuration.GetSection("ProcessarBackground")["Ativar"] != null)
                configExecutarBackground = _configuration.GetSection("ProcessarBackground")["Ativar"].ToString();

            bool executarBackground = true;

            bool.TryParse(configExecutarBackground, out executarBackground);

            if (executarBackground)
            {
                _timerProcessamentoSqs = new System.Timers.Timer(15000);
                _timerProcessamentoSqs.Elapsed += consumoSqs;
                _timerProcessamentoSqs.AutoReset = true;
                _timerProcessamentoSqs.Enabled = true;
            }

            return Task.CompletedTask;
        }


        private void consumoSqs(object source, ElapsedEventArgs e)
        {
            try
            {
                _logger.LogInformation("Iniciando ConsumoSqs");
                _timerProcessamentoSqs.Stop();

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    var mensagemSQSService = scope.ServiceProvider.GetRequiredService<IProcessamentoService>();
                    mensagemSQSService.ConsumirFilaProcessarMensagens();
                }

                _timerProcessamentoSqs.Start();

                _logger.LogInformation("Fim ConsumoSqs");
            }
            catch (Exception ex)
            {
                string mensagemErro = Common.ObterMensagemErro(ex);
                _logger.LogError($"Erro no processamento ConsumoSqs | Mensagem {mensagemErro}");
                _timerProcessamentoSqs.Start();
            }
        }
   
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Parando serviço background");

            _timerProcessamento.Stop();
            _timerProcessamentoSqs.Stop();

            return Task.CompletedTask;
        }
    }
}
