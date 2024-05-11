using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Entities.Request;
using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using FoodieFlow.GestaoPedido.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FoodieFlow.SpecFlowProject.Features.ProcessamentoMensageSQS
{
    [Binding]
    public class ProcessamentoMensageSQS
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ProcessamentoService _processamentoService;
        private readonly Mock<IAwsService> _awsServiceMock = new Mock<IAwsService>();
        private readonly Mock<IPedidoService> _pedidoServiceMock = new Mock<IPedidoService>();
        private readonly Mock<IClienteService> _clienteServiceMock = new Mock<IClienteService>();
        private readonly Mock<ILogger<PedidoService>> _loggerMock = new Mock<ILogger<PedidoService>>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();

        public ProcessamentoMensageSQS(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _processamentoService = new ProcessamentoService(_awsServiceMock.Object, _configurationMock.Object, _pedidoServiceMock.Object, _clienteServiceMock.Object, _loggerMock.Object);
        }

        [Given(@"I have a MensagemSQS")]
        public void GivenIHaveAMensagemSQS()
        {
            // Arrange
            var mensagemSQS = new MensagemSQS();
            _scenarioContext["MensagemSQS"] = mensagemSQS;
        }

        [When(@"I call ProcessarMensagemSQS")]
        public void WhenICallProcessarMensagemSQS()
        {
            // Act
            var mensagemSQS = (MensagemSQS)_scenarioContext["MensagemSQS"];
            _processamentoService.ProcessarMensagemSQS(mensagemSQS);
        }

        [Then(@"the MensagemSQS should be processed")]
        public void ThenTheMensagemSQSShouldBeProcessed()
        {
            // Assert
            _pedidoServiceMock.Verify(service => service.New(It.IsAny<MensagemSQS>(), It.IsAny<Cliente>()), Times.Once);
        }
    }
}