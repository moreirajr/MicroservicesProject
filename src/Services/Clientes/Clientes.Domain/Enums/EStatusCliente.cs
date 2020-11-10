using Clientes.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace Clientes.Domain.Enums
{
    public class EStatusCliente : Enumeration
    {
        public static EStatusCliente Ativo = new EStatusCliente(1, "Ativo");
        public static EStatusCliente Bloqueado = new EStatusCliente(2, "Bloqueado");

        public EStatusCliente(int id, string nome) : base(id, nome) { }

        public static IEnumerable<EStatusCliente> GetAll() => GetAll<EStatusCliente>();

        public static EStatusCliente FromName(string nome)
        {
            return GetAll().FirstOrDefault(e => e.Nome == nome);
        }

        public static EStatusCliente FromId(int id)
        {
            return GetAll().FirstOrDefault(e => e.Id == id);
        }
    }
}