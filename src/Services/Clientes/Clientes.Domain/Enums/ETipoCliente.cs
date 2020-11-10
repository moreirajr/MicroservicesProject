using Clientes.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace Clientes.Domain.Enums
{
    public class ETipoCliente : Enumeration
    {
        public static ETipoCliente Normal = new ETipoCliente(1, "Normal");
        public static ETipoCliente Especial = new ETipoCliente(2, "Especial");

        public ETipoCliente(int id, string nome) : base(id, nome) { }

        public static IEnumerable<ETipoCliente> GetAll() => GetAll<ETipoCliente>();

        public static ETipoCliente FromName(string nome)
        {
            return GetAll().FirstOrDefault(e => e.Nome == nome);
        }

        public static ETipoCliente FromId(int id)
        {
            return GetAll().FirstOrDefault(e => e.Id == id);
        }
    }
}