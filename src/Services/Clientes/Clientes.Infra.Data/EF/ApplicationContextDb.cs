using Clientes.Domain.Clientes;
using Clientes.Domain.Enums;
using Clientes.Domain.Pessoas;
using Clientes.Domain.SeedWork;
using Clientes.Infra.Data.EF.Config;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Clientes.Infra.Data.EF
{
    public class ApplicationContextDb : DbContext, IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;
        public const string DEFAULT_SCHEMA = "dbo";
        private readonly IMediator _mediator;

        public ApplicationContextDb(DbContextOptions<ApplicationContextDb> options, IMediator mediator, bool ensureCreatedDb = true) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            if (ensureCreatedDb)
                CreateDatabaseIfNotExists();

            ChangeTracker.LazyLoadingEnabled = false;
        }

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        private void CreateDatabaseIfNotExists()
        {
            if (!(Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                Database.EnsureCreated();
                Seed();
            }
        }

        private void Seed()
        {
            string sqlScript = @$"
                {DbSeedHelper.SeedEnumeration("dbo", "StatusCliente", "Nome", EStatusCliente.GetAll())}
                {DbSeedHelper.SeedEnumeration("dbo", "TipoCliente", "Descricao", ETipoCliente.GetAll())}
            ";

            Database.ExecuteSqlRaw(sqlScript);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken), bool saveBeforePublishDomainEvents = true)
        {
            if (saveBeforePublishDomainEvents)
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                await _mediator.DispatchDomainEventsAsync(this);
            }
            else
            {
                await _mediator.DispatchDomainEventsAsync(this);
                var result = await base.SaveChangesAsync(cancellationToken);
            }

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new PessoaMap());
            modelBuilder.ApplyConfiguration(new TelefoneMap());
            modelBuilder.ApplyConfiguration(new EnderecoMap());
            modelBuilder.ApplyConfiguration(new TipoClienteMap());
            modelBuilder.ApplyConfiguration(new StatusClienteMap());
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #region DB Sets
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
        #endregion
    }
}