using Clientes.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace Clientes.Infra.Data.EF
{
    public class DbSeedHelper
    {
        public static string SeedEnumeration<T>(string schema, string table, string propriedade, IEnumerable<T> enumeration) where T : Enumeration
        {
            string inserts = string.Join(' ', enumeration.Select(x => $"INSERT INTO [{schema}].[{table}](Id, {propriedade}) VALUES ({x.Id}, '{x.Nome}')"));

            return @$"
                {SetIdentityInsert(table)}
                {inserts}
                {SetIdentityInsert(table, false)}";
        }

        private static string SetIdentityInsert(string table, bool identity = true)
        {
            string idt = identity ? "ON" : "OFF";
            return $"SET IDENTITY_INSERT [{table}] {idt}";
        }
    }
}