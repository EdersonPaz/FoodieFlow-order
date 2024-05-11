using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Entities.Request;
using FoodieFlow.GestaoPedido.Core.Enum;
using FoodieFlow.GestaoPedido.Core.Interfaces.Repository;
using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using FoodieFlow.GestaoPedido.SharedKernel;
using Microsoft.Extensions.Logging;

namespace FoodieFlow.GestaoPedido.Core.Services
{
    public class PedidoService : BaseService<Pedido>, IPedidoService 
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ILogger<PedidoService> _logger;
        public PedidoService(
            IBaseRepository<Pedido> repository,
            IPedidoRepository pedidoRepository, 
            ILogger<PedidoService> logger) : base(repository)
        {
            _pedidoRepository = pedidoRepository;
            _logger = logger;
        }


        public Pedido New(MensagemSQS mensagem, Cliente cliente)
        {
            Pedido novoPedido = new()
            {
                IdCliente = cliente.Id,
                Token = mensagem.Token,
                Status = mensagem.Status
            };

            _pedidoRepository.Insert(novoPedido);
            return novoPedido;
        }

        public void UpdateStatus(int id, EnumStatus status)
        {
            var pedido = _pedidoRepository.GetByID(id);
            if (pedido != null)
            {
                pedido.Status = status;
                _pedidoRepository.Update(pedido);
            }
        }

        public List<Pedido> GetAll(EnumStatus? status)
        {
            try
            {
                _logger.LogInformation("(PedidoService) Iniciando Metodo GetAll");
                List<Pedido> pedidos = _pedidoRepository.GetAll(status);
                _logger.LogInformation("(PedidoService) Fim Metodo GetAll");
                return pedidos;
            }
            catch (Exception ex)
            {
                _logger.LogError(Common.ObterMensagemErro(ex));
                throw;
            }
        }
    }
}
