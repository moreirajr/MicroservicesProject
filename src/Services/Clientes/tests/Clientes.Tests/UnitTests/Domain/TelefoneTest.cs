using Clientes.Domain.Pessoas;
using Clientes.Tests.Util;
using System;
using System.Linq;
using Xunit;

namespace Clientes.Tests.UnitTests.Domain
{
    public class TelefoneTest
    {
        [Fact]
        public void AdicionarTelefone_9Digitos_Sucess()
        {
            var telefone = new Telefone("99", "88", "977777777", true);
            var expectedResult = "+99 (88) 97777-7777";

            Assert.NotNull(telefone);
            Assert.Equal(telefone.ToString(), expectedResult);
        }

        [Fact]
        public void AdicionarTelefone_8Digitos_Sucess()
        {
            var telefone = new Telefone("99", "88", "97777777", true);
            var expectedResult = "+99 (88) 9777-7777";

            Assert.NotNull(telefone);
            Assert.Equal(telefone.ToString(), expectedResult);
        }

        [Fact]
        public void AdicionarTelefone_DDI_Invalido_Fail()
        {
            Assert.Throws<ArgumentNullException>(() => new Telefone("", "88", "97777777", true));
            Assert.Throws<ArgumentException>(() => new Telefone("7A", "88", "97777777", true));
        }

        [Fact]
        public void AdicionarTelefone_DDD_Invalido_Fail()
        {
            Assert.Throws<ArgumentNullException>(() => new Telefone("99", "", "97777777", true));
            Assert.Throws<ArgumentException>(() => new Telefone("99", "B8", "97777777", true));
        }

        [Fact]
        public void AdicionarTelefone_Numero_Invalido_Fail()
        {
            Assert.Throws<ArgumentNullException>(() => new Telefone("99", "88", "", true));
            Assert.Throws<ArgumentException>(() => new Telefone("99", "88", "ABC", true));
        }

        [Fact]
        public void AdicionarTelefoneDuplicado_Fail()
        {
            var cliente = ClienteSeed.ClienteList.FirstOrDefault();
            var telefonePadraoAtual = new Telefone("55", "11", "911112222", true);

            Assert.False(cliente.Pessoa.AdicionarTelefone(telefonePadraoAtual));
        }

        [Fact]
        public void AdicionarNovoTelefonePadrao_Sucess()
        {
            var cliente = ClienteSeed.ClienteList.FirstOrDefault();
            //telefonePadraoAtual = new Telefone("55", "71", "911112222", true);

            var novoTelefoneNaoPadrao = new Telefone("55", "71", "91111333", false);
            var novoTelefonePadrao_1 = new Telefone("55", "71", "91111334", true);
            var novoTelefonePadrao_2 = new Telefone("55", "71", "91111335", true);

            Assert.True(cliente.Pessoa.AdicionarTelefone(novoTelefoneNaoPadrao));
            Assert.True(cliente.Pessoa.AdicionarTelefone(novoTelefonePadrao_1));
            Assert.True(cliente.Pessoa.AdicionarTelefone(novoTelefonePadrao_2));

            Assert.True(cliente.Pessoa.Telefones.Count == 4);
            Assert.True(cliente.Pessoa.Telefones.Count(x => x.TelefonePadrao) == 1);
            Assert.True(cliente.Pessoa.Telefones.FirstOrDefault(x => x.TelefonePadrao).Equals(novoTelefonePadrao_2));
        }
    }
}