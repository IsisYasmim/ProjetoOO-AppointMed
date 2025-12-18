using AppointMed.AppointMed.Core.Entities;

namespace AppointMed.AppointMed.Core.Interfaces
{
    // Factory para criação de agendamentos com diferentes tipos
    public interface IAgendamentoFactory
    {
        // Cria agendamento padrão (30 minutos)
   
        Agendamento CreateAgendamentoPadrao(Guid clienteId, string medico, string especialidade, DateTime dataHora);

        // Cria agendamento para retorno (15 minutos)
   
        Agendamento CreateAgendamentoRetorno(Guid clienteId, string medico, string especialidade, DateTime dataHora);

        // Cria agendamento para primeira consulta (60 minutos)
   
        Agendamento CreatePrimeiraConsulta(Guid clienteId, string medico, string especialidade, DateTime dataHora);

        // Cria agendamento de emergência (prioritário, pode ser no mesmo dia)
   
        Agendamento CreateAgendamentoEmergencia(Guid clienteId, string medico, string especialidade, DateTime dataHora);
    }
}
