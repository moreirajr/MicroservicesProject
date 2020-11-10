using Clientes.Application.Clientes.Commands;
using Clientes.Domain.Clientes;
using Clientes.Domain.Enums;
using Clientes.Domain.Pessoas;
using System;
using System.Collections.Generic;

namespace Clientes.Tests.Util
{
    public class ClienteSeed
    {
        public static IEnumerable<Cliente> ClienteList =>
             new List<Cliente>()
            {
                new Cliente(
                    ETipoCliente.Normal.Id,
                    EStatusCliente.Ativo.Id,
                    new Pessoa(
                        "Teste A",
                        "01234567890",
                        new DateTime(2000, 01, 01),
                        new Telefone("55", "11", "911112222", true),
                        new Endereco("06000111", "Logradouro A", "1234", "Complemento A", true)
                    )),
                 new Cliente(
                    ETipoCliente.Normal.Id,
                    EStatusCliente.Ativo.Id,
                    new Pessoa(
                        "Teste B",
                        "12345678999",
                        new DateTime(2000, 09, 02),
                        new Telefone("55", "71", "911112233", true),
                        new Endereco("06000222", "Logradouro B", "4321", "Complemento B", true)
                    ))
            };


        public static CadastrarClienteCommand CadastroClienteCommand => new CadastrarClienteCommand()
        {
            CEP = "06123999",
            Complemento = "Comp Teste Command",
            CPF = "09876543210",
            DataNascimento = new DateTime(1989, 01, 01),
            DDD = "99",
            DDI = "99",
            Logradouro = "Logr Teste Command",
            Nome = "Cliente Teste Command",
            NumeroEndereco = "100",
            NumeroTelefone = "912345678",
            TipoClienteId = ETipoCliente.Normal.Id
        };
    }
}