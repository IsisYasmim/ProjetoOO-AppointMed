using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.Interfaces;
using AppointMed.AppointMed.Core.ValueObjects;

namespace AppointMed.AppointMed.Services
{
    public class ClienteService
    {
        private readonly IRepository<Cliente> _repository;
        private readonly IClienteFactory _factory;

        public ClienteService(IRepository<Cliente> repository, IClienteFactory factory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<Cliente> CadastrarClientePadraoAsync(string nome, string email, string telefone, string cpf, DateTime dataNascimento)
        {
            var emailVo = Email.Create(email);
            var cliente = _factory.CreateClientePadrao(nome, emailVo, telefone, cpf, dataNascimento);
            return await _repository.AddAsync(cliente);
        }

        public async Task<Cliente?> ObterClientePorIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync(bool apenasAtivos = true)
        {
            var clientes = await _repository.GetAllAsync();

            if (apenasAtivos)
            {
                return clientes.Where(c => c.Ativo);
            }

            return clientes;
        }

        public async Task<Cliente?> BuscarPorCpfAsync(string cpf)
        {
            var clientes = await _repository.FindAsync(c => c.CPF == cpf);
            return clientes.FirstOrDefault();
        }

        public async Task<Cliente?> BuscarPorEmailAsync(string email)
        {
            var clientes = await _repository.FindAsync(c => c.Email.Valor == email);
            return clientes.FirstOrDefault();
        }

        public async Task AtualizarClienteAsync(Guid id, string? nome = null, string? telefone = null)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente com ID {id} não encontrado");

            cliente.Atualizar(nome ?? cliente.Nome, telefone ?? cliente.Telefone);
            await _repository.UpdateAsync(cliente);
        }

        public async Task DesativarClienteAsync(Guid id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente com ID {id} não encontrado");

            cliente.Desativar();
            await _repository.UpdateAsync(cliente);
        }

        public async Task AtivarClienteAsync(Guid id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente == null)
                throw new KeyNotFoundException($"Cliente com ID {id} não encontrado");

            cliente.Ativar();
            await _repository.UpdateAsync(cliente);
        }

        public async Task<int> ContarClientesAtivosAsync()
        {
            var clientes = await _repository.GetAllAsync();
            return clientes.Count(c => c.Ativo);
        }

        public async Task<IEnumerable<Cliente>> BuscarClientesPorNomeAsync(string nomeParcial)
        {
            return await _repository.FindAsync(c =>
                c.Nome.Contains(nomeParcial, StringComparison.OrdinalIgnoreCase));
        }
    }
}
