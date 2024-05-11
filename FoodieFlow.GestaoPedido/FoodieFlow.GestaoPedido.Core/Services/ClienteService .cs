using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Entities.Request;
using FoodieFlow.GestaoPedido.Core.Interfaces.Repository;
using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using Microsoft.Extensions.Logging;

namespace FoodieFlow.GestaoPedido.Core.Services
{
    public class ClienteService : BaseService<Cliente>, IClienteService 
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly ILogger<ClienteService> _logger;
        public ClienteService(
            IBaseRepository<Cliente> repository,
            IClienteRepository clienteRepository, 
            ILogger<ClienteService> logger) : base(repository)
        {
            _clienteRepository = clienteRepository;
            _logger = logger;
        }

        public Cliente New(MensagemSQS mensagem)
        {
            var existingCliente = _clienteRepository.GetByCPF(mensagem.PayerDocument);
            if (existingCliente != null)
            {
                return existingCliente;
            }

            Cliente novoCliente = new()
            {
                Cpf = mensagem.PayerDocument,
                Nome = mensagem.PayerName,
                Email = mensagem.PayerEmail
            };

            _clienteRepository.Insert(novoCliente);
            return novoCliente;
        }
    }
}
