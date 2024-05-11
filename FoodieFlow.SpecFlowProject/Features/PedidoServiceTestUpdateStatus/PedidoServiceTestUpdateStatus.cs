using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Enum;
using FoodieFlow.GestaoPedido.Core.Interfaces.Repository;
using FoodieFlow.GestaoPedido.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FoodieFlow.SpecFlowProject.Features.PedidoServiceTestUpdateStatus
{
    [Binding]
    public class PedidoServiceTestUpdateStatus
    {
        private readonly PedidoService _pedidoService;
        private readonly Mock<IBaseRepository<Pedido>> _baseRepositoryMock = new Mock<IBaseRepository<Pedido>>();
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        private readonly Mock<ILogger<PedidoService>> _loggerMock = new Mock<ILogger<PedidoService>>();
        private Pedido _pedido;

        public PedidoServiceTestUpdateStatus()
        {
            _pedidoService = new PedidoService(_baseRepositoryMock.Object, _pedidoRepositoryMock.Object, _loggerMock.Object);
        }

        [Given(@"I have a Pedido to update")]
        public void GivenIHaveAPedidoService()
        {
            // Arrange
            _pedido = new Pedido { Id = 1, Status = EnumStatus.pago };
            _pedidoRepositoryMock.Setup(repo => repo.GetByID(1)).Returns(_pedido);
        }

        [When(@"I call UpdateStatus with id '(.*)' and status '(.*)'")]
        public void WhenICallUpdateStatusWithIdAndStatus(int id, string status)
        {
            // Act
            _pedidoService.UpdateStatus(id, EnumStatus.pronto);
        }

        [Then(@"the status of pedido with id '(.*)' should be updated to '(.*)'")]
        public void ThenTheStatusOfPedidoWithIdShouldBeUpdatedTo(int id, string status)
        {
            // Assert
            Assert.Equal(EnumStatus.pronto, _pedido.Status);
            _pedidoRepositoryMock.Verify(repo => repo.Update(_pedido), Times.Once);
        }
    }
}