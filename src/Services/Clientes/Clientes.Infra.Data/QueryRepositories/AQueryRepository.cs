using Clientes.Infra.Data.Mongo;
using System;

namespace Clientes.Infra.Data.QueryRepositories
{
    public abstract class AQueryRepository
    {
        private readonly IApplicationContextDbNoSql _context;

        public AQueryRepository(IApplicationContextDbNoSql context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected IApplicationContextDbNoSql Context => _context;
    }
}