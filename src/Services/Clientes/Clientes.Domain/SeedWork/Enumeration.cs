using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clientes.Domain.SeedWork
{
    public abstract class Enumeration : IComparable
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        protected Enumeration(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        protected static IEnumerable<T> GetAll<T>()
            where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object otherObj)
        {
            var otherValue = otherObj as Enumeration;

            if (otherValue == null) return false;

            var typeMatches = GetType().Equals(otherObj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Nome;
        }

        public int CompareTo(object otherObj)
        {
            return Id.CompareTo(((Enumeration)otherObj).Id);
        }
    }
}