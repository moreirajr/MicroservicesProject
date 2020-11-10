using Clientes.Domain.Clientes;
using Clientes.Domain.SeedWork;
using Clientes.Infra.Data.EF;
using System;
using System.Threading.Tasks;

namespace Clientes.Infra.Data.CommandRepositories
{
    public class ClienteCommandRepository : IClienteCommandRepository
    {
        private readonly ApplicationContextDb _context;

        public ClienteCommandRepository(ApplicationContextDb context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _context; }
        }

        public async Task BeginTransactionAsync()
        {
            await _context.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.CommitTransactionAsync(_context.GetCurrentTransaction());
        }

        public async Task<Cliente> CadastrarClienteAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);

            return cliente;
        }
    }
}