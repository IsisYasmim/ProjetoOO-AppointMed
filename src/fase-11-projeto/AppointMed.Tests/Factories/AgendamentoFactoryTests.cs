using Xunit;
using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.Interfaces;
using AppointMed.AppointMed.Infrastructure.Factories;

namespace AppointMed.Tests.Factories
{
    public class AgendamentoFactoryTests
    {
        private readonly IAgendamentoFactory _factory;
        private readonly Guid _clienteId;

        public AgendamentoFactoryTests()
        {
            _factory = new AgendamentoFactory();
            _clienteId = Guid.NewGuid();
        }

        [Fact]
        public void CreateAgendamentoPadrao_ValidParameters_ReturnsAgendamentoWith30Minutes()
        {
            // Arrange
            var medico = "Dr. Silva";
            var especialidade = "Cardiologia";
            var dataHora = DateTime.Now.AddDays(1);

            // Act
            var agendamento = _factory.CreateAgendamentoPadrao(_clienteId, medico, especialidade, dataHora);

            // Assert
            Assert.NotNull(agendamento);
            Assert.Equal(_clienteId, agendamento.ClienteId);
            Assert.Equal(medico, agendamento.Medico);
            Assert.Equal(especialidade, agendamento.Especialidade);
            Assert.Equal(30, agendamento.DuracaoMinutos);
            Assert.NotNull(agendamento.Observacoes);
        }

        [Fact]
        public void CreatePrimeiraConsulta_ValidParameters_ReturnsAgendamentoWith60Minutes()
        {
            // Arrange
            var medico = "Dr. Santos";
            var especialidade = "Clínica Geral";
            var dataHora = DateTime.Now.AddDays(2);

            // Act
            var agendamento = _factory.CreatePrimeiraConsulta(_clienteId, medico, especialidade, dataHora);

            // Assert
            Assert.NotNull(agendamento);
            Assert.Equal(60, agendamento.DuracaoMinutos);
            Assert.Contains("Primeira consulta", agendamento.Observacoes);
        }

        [Fact]
        public void CreateAgendamentoEmergencia_ValidParameters_ReturnsConfirmedAgendamento()
        {
            // Arrange
            var medico = "Dr. Emergência";
            var especialidade = "Pronto Socorro";
            var dataHora = DateTime.Now.AddHours(2);

            // Act
            var agendamento = _factory.CreateAgendamentoEmergencia(_clienteId, medico, especialidade, dataHora);

            // Assert
            Assert.NotNull(agendamento);
            Assert.Equal(45, agendamento.DuracaoMinutos);
            Assert.Contains("EMERGÊNCIA", agendamento.Observacoes);
            Assert.NotNull(agendamento.DataConfirmacao);
        }

        [Fact]
        public void CreateAgendamentoRetorno_ValidParameters_ReturnsAgendamentoWith15Minutes()
        {
            // Arrange
            var medico = "Dr. Retorno";
            var especialidade = "Ortopedia";
            var dataHora = DateTime.Now.AddDays(7);

            // Act
            var agendamento = _factory.CreateAgendamentoRetorno(_clienteId, medico, especialidade, dataHora);

            // Assert
            Assert.NotNull(agendamento);
            Assert.Equal(15, agendamento.DuracaoMinutos);
            Assert.Contains("retorno", agendamento.Observacoes);
        }

        [Fact]
        public void CreateAgendamentoPadrao_PastDateTime_ThrowsException()
        {
            // Arrange
            var medico = "Dr. Teste";
            var especialidade = "Teste";
            var dataHora = DateTime.Now.AddDays(-1); // Data passada

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _factory.CreateAgendamentoPadrao(_clienteId, medico, especialidade, dataHora));
        }

        [Theory]
        [InlineData(1, false)] // 1 minuto no passado
        [InlineData(0, false)] // Agora
        [InlineData(1, true)]  // 1 minuto no futuro
        [InlineData(60, true)] // 1 hora no futuro
        [InlineData(1440, true)] // 1 dia no futuro
        public void CreateAgendamentoPadrao_DateTimeValidation_VariesByInput(int minutesFromNow, bool shouldSucceed)
        {
            // Arrange
            var medico = "Dr. Teste";
            var especialidade = "Teste";
            var dataHora = DateTime.Now.AddMinutes(minutesFromNow);

            // Act & Assert
            if (shouldSucceed)
            {
                var agendamento = _factory.CreateAgendamentoPadrao(_clienteId, medico, especialidade, dataHora);
                Assert.NotNull(agendamento);
            }
            else
            {
                Assert.Throws<ArgumentException>(() =>
                    _factory.CreateAgendamentoPadrao(_clienteId, medico, especialidade, dataHora));
            }
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        [InlineData("Dr. Silva", true)]
        public void CreateAgendamentoPadrao_MedicoValidation_VariesByInput(string medico, bool shouldSucceed)
        {
            // Arrange
            var especialidade = "Cardiologia";
            var dataHora = DateTime.Now.AddDays(1);

            // Act & Assert
            if (shouldSucceed)
            {
                var agendamento = _factory.CreateAgendamentoPadrao(_clienteId, medico, especialidade, dataHora);
                Assert.NotNull(agendamento);
                Assert.Equal(medico, agendamento.Medico);
            }
            else
            {
                Assert.Throws<ArgumentException>(() =>
                    _factory.CreateAgendamentoPadrao(_clienteId, medico, especialidade, dataHora));
            }
        }
    }
}