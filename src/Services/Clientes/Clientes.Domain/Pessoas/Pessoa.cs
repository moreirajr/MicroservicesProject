using Clientes.Domain.Clientes;
using Clientes.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clientes.Domain.Pessoas
{
    public class Pessoa : Entity
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public ICollection<Endereco> Enderecos { get; set; }
        public ICollection<Telefone> Telefones { get; set; }

        public long ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public Pessoa()
        {
            Enderecos = new List<Endereco>();
            Telefones = new List<Telefone>();
        }
        public Pessoa(string nome, string cpf, DateTime dataNascimento, Telefone telefonePadrao, Endereco enderecoPadrao)
            : this()
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = dataNascimento;

            AdicionarTelefone(telefonePadrao);
            AdicionarEndereco(enderecoPadrao);
        }

        public bool AdicionarTelefone(Telefone telefone)
        {
            if (Telefones.Any(x => x.Equals(telefone)))
                return false;

            if (telefone.TelefonePadrao)
                MudarTelefonePadrao();

            Telefones.Add(telefone);

            return true;
        }

        public bool AdicionarEndereco(Endereco endereco)
        {
            if (Enderecos.Any(x => x.Equals(endereco)))
                return false;

            if (endereco.EnderecoPadrao)
                MudarEnderecoPadrao();

            Enderecos.Add(endereco);

            return true;
        }

        private void MudarTelefonePadrao()
        {
            foreach (var item in Telefones)
            {
                item.TelefonePadrao = false;
            }
        }

        private void MudarEnderecoPadrao()
        {
            foreach (var item in Enderecos)
            {
                item.EnderecoPadrao = false;
            }
        }
    }
}