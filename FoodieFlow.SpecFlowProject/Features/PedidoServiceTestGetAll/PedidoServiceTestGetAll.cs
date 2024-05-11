using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Enum;
using FoodieFlow.GestaoPedido.Core.Interfaces.Repository;
using FoodieFlow.GestaoPedido.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FoodieFlow.SpecFlowProject.Features.PedidoServiceTestGetAll
{
    [Binding]
    public class PedidoServiceTestGetAll
    {
        private readonly PedidoService _pedidoService;
        private readonly Mock<IBaseRepository<Pedido>> _baseRepositoryMock = new Mock<IBaseRepository<Pedido>>();
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        private readonly Mock<ILogger<PedidoService>> _loggerMock = new Mock<ILogger<PedidoService>>();
        private List<Pedido> _result;

        public PedidoServiceTestGetAll()
        {
            _pedidoService = new PedidoService(_baseRepositoryMock.Object, _pedidoRepositoryMock.Object, _loggerMock.Object);
        }

        [Given(@"I have a kitchen and have pedidos")]
        public void GivenIHaveAKitchenAndHavePedidos()
        {
            // Arrange
            var pedidos = new List<Pedido>
        {
            new Pedido { Status = EnumStatus.a_pagar },
            new Pedido { Status = EnumStatus.a_pagar }
        };

            _pedidoRepositoryMock.Setup(repo => repo.GetAll(EnumStatus.a_pagar)).Returns(pedidos);
        }

        [When(@"I call GetAll with status '(.*)'")]
        public void WhenICallGetAllWithStatus(string status)
        {
            // Act
            _result = _pedidoService.GetAll(EnumStatus.a_pagar);
        }

        [Then(@"I should get a list of all pedidos with status '(.*)'")]
        public void ThenIShouldGetAListOfAllPedidosWithStatus(string status)
        {
            // Assert
            Assert.Equal(2, _result.Count);
            _pedidoRepositoryMock.Verify(repo => repo.GetAll(EnumStatus.a_pagar), Times.Once);
        }
    }
}





