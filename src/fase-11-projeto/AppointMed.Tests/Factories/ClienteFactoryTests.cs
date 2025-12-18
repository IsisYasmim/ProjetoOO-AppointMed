using Xunit;
using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.Interfaces;
using AppointMed.AppointMed.Core.ValueObjects;
using AppointMed.AppointMed.Infrastructure.Factories;


namespace AppointMed.Tests.Factories
{
    public class ClienteFactoryTests
    {
        private readonly IClienteFactory _factory;

        public ClienteFactoryTests()
        {
            _factory = new ClienteFactory();
        }

        [Fact]
        public void CreateClientePadrao_ValidParameters_ReturnsCliente()
        {
            // Arrange
            var nome = "João Silva";
            var email = Email.Create("joao@email.com");
            var telefone = "11999999999";
            var cpf = "12345678901";
            var dataNascimento = new DateTime(1990, 1, 1);

            // Act
            var cliente = _factory.CreateClientePadrao(nome, email, telefone, cpf, dataNascimento);

            // Assert
            Assert.NotNull(cliente);
            Assert.Equal(nome, cliente.Nome);
            Assert.Equal(email, cliente.Email);
            Assert.Equal(cpf, cliente.CPF);
            Assert.True(cliente.Ativo);
        }

        [Fact]
        public void CreateClientePadrao_InvalidCpf_ThrowsException()
        {
            // Arrange
            var nome = "João Silva";
            var email = Email.Create("joao@email.com");
            var telefone = "11999999999";
            var cpf = "123"; // CPF inválido
            var dataNascimento = new DateTime(1990, 1, 1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _factory.CreateClientePadrao(nome, email, telefone, cpf, dataNascimento));
        }

        [Fact]
        public void CreateClientePrioritario_ValidParameters_ReturnsCliente()
        {
            // Arrange
            var nome = "Maria Santos";
            var email = Email.Create("maria@email.com");
            var telefone = "11988888888";
            var cpf = "98765432109";
            var dataNascimento = new DateTime(1950, 1, 1);
            var tipoPrioridade = "Idoso";

            // Act
            var cliente = _factory.CreateClientePrioritario(nome, email, telefone, cpf, dataNascimento, tipoPrioridade);

            // Assert
            Assert.NotNull(cliente);
            Assert.Equal(nome, cliente.Nome);
            Assert.Equal(cpf, cliente.CPF);
        }

        [Fact]
        public void CreateClienteCorporativo_ValidParameters_ReturnsCliente()
        {
            // Arrange
            var nome = "Empresa XYZ";
            var email = Email.Create("empresa@xyz.com");
            var telefone = "1133333333";
            var cpf = "11122233344";
            var dataNascimento = new DateTime(1980, 1, 1);
            var empresa = "XYZ Corp";

            // Act
            var cliente = _factory.CreateClienteCorporativo(nome, email, telefone, cpf, dataNascimento, empresa);

            // Assert
            Assert.NotNull(cliente);
            Assert.Equal(nome, cliente.Nome);
            Assert.Equal(cpf, cliente.CPF);
        }

        [Theory]
        [InlineData("12345678901", true)]
        [InlineData("123", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void CreateClientePadrao_CpfValidation_VariesByInput(string cpf, bool shouldSucceed)
        {
            // Arrange
            var nome = "Teste";
            var email = Email.Create("teste@email.com");
            var telefone = "11999999999";
            var dataNascimento = new DateTime(1990, 1, 1);

            // Act & Assert
            if (shouldSucceed)
            {
                var cliente = _factory.CreateClientePadrao(nome, email, telefone, cpf, dataNascimento);
                Assert.NotNull(cliente);
                Assert.Equal(cpf, cliente.CPF);
            }
            else
            {
                Assert.Throws<ArgumentException>(() =>
                    _factory.CreateClientePadrao(nome, email, telefone, cpf, dataNascimento));
            }
        }
    }
}