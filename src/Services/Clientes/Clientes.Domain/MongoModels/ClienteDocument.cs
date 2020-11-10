using Clientes.Domain.Clientes;
using Clientes.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace Clientes.Domain.MongoModels
{
    public class ClienteDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public long IdCliente { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public string TipoCliente { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string CEP { get; set; }

        public ClienteDocument() { }
        public ClienteDocument(Cliente cliente)
        {
            Id = "";
            IdCliente = cliente.Id;
            Nome = cliente.Pessoa.Nome;
            Status = EStatusCliente.FromId(cliente.StatusId).Nome;
            TipoCliente = ETipoCliente.FromId(cliente.TipoClienteId).Nome;
            CPF = cliente.Pessoa.CPF;
            Telefone = cliente.Pessoa.Telefones.FirstOrDefault(x => x.TelefonePadrao).ToString();
            CEP = cliente.Pessoa.Enderecos.FirstOrDefault(x => x.EnderecoPadrao).CEP;
        }
    }
}