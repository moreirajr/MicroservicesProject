using MongoDB.Driver;
using System.Collections.Generic;

namespace Clientes.Infra.Data.Mongo
{
    public interface IApplicationContextDbNoSql
    {
        IMongoDatabase DataBase { get; }
        List<string> ListAllCollectionNames();
    }

    public class ApplicationContextDbNoSql : IApplicationContextDbNoSql
    {
        private IMongoDatabase _mongoDb;

        public ApplicationContextDbNoSql(string connectionString, string databaseName)
        {
            _mongoDb = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public IMongoDatabase DataBase => _mongoDb;

        public List<string> ListAllCollectionNames()
        {
            return DataBase.ListCollectionNames().ToList();
        }
    }
}