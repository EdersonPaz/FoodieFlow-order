using Amazon.SQS.Model;
using FoodieFlow.GestaoPedido.Core.Entities.Request;
using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using FoodieFlow.GestaoPedido.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodieFlow.SpecFlowProject.Features.ProcessamentoConsumeSQSProcess
{
    [Binding]
    public class ProcessamentoConsumeSQSProcess
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ProcessamentoService _processamentoService;
        private readonly Mock<IAwsService> _awsServiceMock = new Mock<IAwsService>();
        private readonly Mock<IPedidoService> _pedidoServiceMock = new Mock<IPedidoService>();
        private readonly Mock<IClienteService> _clienteServiceMock = new Mock<IClienteService>();
        private readonly Mock<ILogger<PedidoService>> _loggerMock = new Mock<ILogger<PedidoService>>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();

        public ProcessamentoConsumeSQSProcess(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _processamentoService = new ProcessamentoService(_awsServiceMock.Object, _configurationMock.Object, _pedidoServiceMock.Object, _clienteServiceMock.Object, _loggerMock.Object);
        }

        [Given(@"I have a queue with messages")]
        public void GivenIHaveAQueueWithMessages()
        {
            // Arrange
            var messages = new List<Message>
        {
            new Message { Body = JsonConvert.SerializeObject(new MensagemSQS()) }
        };
            _awsServiceMock.Setup(service => service.ObterMensagensAsync(It.IsAny<string>())).Returns(Task.FromResult(messages));
        }

        [When(@"I call ConsumirFilaProcessarMensagens")]
        public void WhenICallConsumirFilaProcessarMensagens()
        {
            // Act
            _processamentoService.ConsumirFilaProcessarMensagens();
        }

        [Then(@"the messages should be processed and deleted from the queue")]
        public void ThenTheMessagesShouldBeProcessedAndDeletedFromTheQueue()
        {
            // Assert
            _awsServiceMock.Verify(service => service.DeletarMensagemAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}