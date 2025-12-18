using AppointMed.AppointMed.Core.Enums;
namespace AppointMed.AppointMed.Core.Entities
{
    // Representa um agendamento médico
    public class Agendamento
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public string Medico { get; private set; }
        public string Especialidade { get; private set; }
        public DateTime DataHora { get; private set; }
        public int DuracaoMinutos { get; private set; }
        public string Observacoes { get; private set; }
        public StatusAgendamento Status { get; private set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConfirmacao { get; private set; }
        public DateTime? DataCancelamento { get; private set; }
        public string MotivoCancelamento { get; private set; }

        // Navigation property
        public virtual Cliente Cliente { get; private set; }

        protected Agendamento() { } // EF Core constructor

        private Agendamento(
            Guid clienteId,
            string medico,
            string especialidade,
            DateTime dataHora,
            int duracaoMinutos,
            string observacoes)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            Medico = medico;
            Especialidade = especialidade;
            DataHora = dataHora;
            DuracaoMinutos = duracaoMinutos;
            Observacoes = observacoes;
            Status = StatusAgendamento.Agendado;
            DataCriacao = DateTime.UtcNow;
        }

        public static Agendamento Create(
            Guid clienteId,
            string medico,
            string especialidade,
            DateTime dataHora,
            int duracaoMinutos = 30,
            string observacoes = "")
        {
            if (clienteId == Guid.Empty)
                throw new ArgumentException("Cliente é obrigatório", nameof(clienteId));

            if (string.IsNullOrWhiteSpace(medico))
                throw new ArgumentException("Médico é obrigatório", nameof(medico));

            if (string.IsNullOrWhiteSpace(especialidade))
                throw new ArgumentException("Especialidade é obrigatória", nameof(especialidade));

            if (dataHora <= DateTime.UtcNow)
                throw new ArgumentException("Data/hora deve ser futura", nameof(dataHora));

            if (duracaoMinutos <= 0)
                throw new ArgumentException("Duração deve ser positiva", nameof(duracaoMinutos));

            return new Agendamento(clienteId, medico, especialidade, dataHora, duracaoMinutos, observacoes);
        }

        public void Confirmar()
        {
            if (Status == StatusAgendamento.Agendado)
            {
                Status = StatusAgendamento.Confirmado;
                DataConfirmacao = DateTime.UtcNow;
            }
        }

        public void Cancelar(string motivo)
        {
            if (Status != StatusAgendamento.Cancelado && Status != StatusAgendamento.Concluido)
            {
                Status = StatusAgendamento.Cancelado;
                DataCancelamento = DateTime.UtcNow;
                MotivoCancelamento = motivo;
            }
        }

        public void Concluir()
        {
            if (Status == StatusAgendamento.Confirmado)
            {
                Status = StatusAgendamento.Concluido;
            }
        }

        public void Reagendar(DateTime novaDataHora)
        {
            if (novaDataHora <= DateTime.UtcNow)
                throw new ArgumentException("Nova data/hora deve ser futura", nameof(novaDataHora));

            if (Status != StatusAgendamento.Cancelado)
            {
                DataHora = novaDataHora;
                Status = StatusAgendamento.Agendado; // Volta para agendado
            }
        }
    }
}
