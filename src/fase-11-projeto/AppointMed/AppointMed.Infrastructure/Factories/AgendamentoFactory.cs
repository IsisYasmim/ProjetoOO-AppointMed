using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.Interfaces;

namespace AppointMed.AppointMed.Infrastructure.Factories
{
    public class AgendamentoFactory : IAgendamentoFactory
    {
        public Agendamento CreateAgendamentoPadrao(Guid clienteId, string medico, string especialidade, DateTime dataHora)
        {
            return Agendamento.Create(clienteId, medico, especialidade, dataHora, 30, "Consulta padrão");
        }

        public Agendamento CreateAgendamentoRetorno(Guid clienteId, string medico, string especialidade, DateTime dataHora)
        {
            return Agendamento.Create(clienteId, medico, especialidade, dataHora, 15, "Consulta de retorno");
        }

        public Agendamento CreatePrimeiraConsulta(Guid clienteId, string medico, string especialidade, DateTime dataHora)
        {
            return Agendamento.Create(clienteId, medico, especialidade, dataHora, 60, "Primeira consulta - necessário chegar 15 minutos antes");
        }

        public Agendamento CreateAgendamentoEmergencia(Guid clienteId, string medico, string especialidade, DateTime dataHora)
        {
            var agendamento = Agendamento.Create(clienteId, medico, especialidade, dataHora, 45, "EMERGÊNCIA - Prioridade máxima");
            // Marcar como confirmado automaticamente
            agendamento.Confirmar();
            return agendamento;
        }
    }
}
