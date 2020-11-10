using System.Data;

namespace Contas.Infra.Data.Dapper
{
    public interface IDapperContext
    {
        IDbConnection Connection { get; }
        IDbTransaction GetCurrentTransaction();
        bool HasActiveTransaction { get; }

        IDbTransaction BeginTransaction();
        void CommitTransaction(IDbTransaction transaction);
        void RollBackTransaction();
    }
}