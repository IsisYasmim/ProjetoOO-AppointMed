using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.Enums;
using AppointMed.AppointMed.Core.Interfaces;

namespace AppointMed.AppointMed.Services
{
    public class AgendamentoService
    {
        private readonly IRepository<Agendamento> _repository;
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IAgendamentoFactory _factory;

        public AgendamentoService(
            IRepository<Agendamento> repository,
            IRepository<Cliente> clienteRepository,
            IAgendamentoFactory factory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<Agendamento> CriarAgendamentoPadraoAsync(Guid clienteId, string medico, string especialidade, DateTime dataHora)
        {
            await ValidarClienteAsync(clienteId);
            ValidarDisponibilidade(medico, dataHora);

            var agendamento = _factory.CreateAgendamentoPadrao(clienteId, medico, especialidade, dataHora);
            return await _repository.AddAsync(agendamento);
        }

        public async Task<Agendamento> CriarPrimeiraConsultaAsync(Guid clienteId, string medico, string especialidade, DateTime dataHora)
        {
            await ValidarClienteAsync(clienteId);
            ValidarDisponibilidade(medico, dataHora);

            var agendamento = _factory.CreatePrimeiraConsulta(clienteId, medico, especialidade, dataHora);
            return await _repository.AddAsync(agendamento);
        }

        public async Task<Agendamento> CriarAgendamentoEmergenciaAsync(Guid clienteId, string medico, string especialidade, DateTime dataHora)
        {
            await ValidarClienteAsync(clienteId);

            // Emergência pode sobrescrever outros agendamentos
            var agendamento = _factory.CreateAgendamentoEmergencia(clienteId, medico, especialidade, dataHora);
            return await _repository.AddAsync(agendamento);
        }

        public async Task<Agendamento?> ObterAgendamentoPorIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Agendamento>> ObterAgendamentosPorClienteAsync(Guid clienteId)
        {
            return await _repository.FindAsync(a => a.ClienteId == clienteId);
        }

        public async Task<IEnumerable<Agendamento>> ObterAgendamentosPorMedicoAsync(string medico, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var query = await _repository.FindAsync(a => a.Medico == medico);

            if (dataInicio.HasValue)
                query = query.Where(a => a.DataHora >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(a => a.DataHora <= dataFim.Value);

            return query.OrderBy(a => a.DataHora);
        }

        public async Task ConfirmarAgendamentoAsync(Guid id)
        {
            var agendamento = await _repository.GetByIdAsync(id);
            if (agendamento == null)
                throw new KeyNotFoundException($"Agendamento com ID {id} não encontrado");

            agendamento.Confirmar();
            await _repository.UpdateAsync(agendamento);
        }

        public async Task CancelarAgendamentoAsync(Guid id, string motivo)
        {
            var agendamento = await _repository.GetByIdAsync(id);
            if (agendamento == null)
                throw new KeyNotFoundException($"Agendamento com ID {id} não encontrado");

            agendamento.Cancelar(motivo);
            await _repository.UpdateAsync(agendamento);
        }

        public async Task ReagendarAsync(Guid id, DateTime novaDataHora)
        {
            var agendamento = await _repository.GetByIdAsync(id);
            if (agendamento == null)
                throw new KeyNotFoundException($"Agendamento com ID {id} não encontrado");

            ValidarDisponibilidade(agendamento.Medico, novaDataHora, id);
            agendamento.Reagendar(novaDataHora);
            await _repository.UpdateAsync(agendamento);
        }

        public async Task<IEnumerable<Agendamento>> ObterAgendamentosFuturosAsync()
        {
            var agendamentos = await _repository.GetAllAsync();
            return agendamentos
                .Where(a => a.DataHora > DateTime.UtcNow && a.Status != StatusAgendamento.Cancelado)
                .OrderBy(a => a.DataHora);
        }

        public async Task<IEnumerable<Agendamento>> ObterAgendamentosDoDiaAsync(DateTime data)
        {
            var inicioDia = data.Date;
            var fimDia = data.Date.AddDays(1).AddTicks(-1);

            var agendamentos = await _repository.GetAllAsync();
            return agendamentos
                .Where(a => a.DataHora >= inicioDia && a.DataHora <= fimDia)
                .OrderBy(a => a.DataHora);
        }

        private async Task ValidarClienteAsync(Guid clienteId)
        {
            var cliente = await _clienteRepository.GetByIdAsync(clienteId);
            if (cliente == null || !cliente.Ativo)
                throw new InvalidOperationException("Cliente não encontrado ou inativo");
        }

        private void ValidarDisponibilidade(string medico, DateTime dataHora, Guid? agendamentoId = null)
        {
            // Em produção, verificar com regras mais complexas
            // Aqui apenas valida horário comercial
            if (dataHora.Hour < 8 || dataHora.Hour >= 18)
                throw new InvalidOperationException("Fora do horário comercial");

            // Verificar se já existe agendamento no mesmo horário
            // Esta verificação seria feita no banco em produção
        }
    }
}
