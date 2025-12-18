using AppointMed.AppointMed.Core.ValueObjects;
namespace AppointMed.AppointMed.Core.Entities
{
    // Representa um cliente/paciente do sistema médico
    public class Cliente
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public string Telefone { get; private set; }
        public string CPF { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; private set; }

        // Navigation property
        public virtual ICollection<Agendamento> Agendamentos { get; private set; }

        protected Cliente() { } // EF Core constructor

        private Cliente(string nome, Email email, string telefone, string cpf, DateTime dataNascimento)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            Telefone = telefone;
            CPF = cpf;
            DataNascimento = dataNascimento;
            DataCadastro = DateTime.UtcNow;
            Ativo = true;
            Agendamentos = new List<Agendamento>();
        }

        public static Cliente Create(string nome, Email email, string telefone, string cpf, DateTime dataNascimento)
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório", nameof(nome));

            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                throw new ArgumentException("CPF inválido", nameof(cpf));

            return new Cliente(nome, email, telefone, cpf, dataNascimento);
        }

        public void Atualizar(string nome, string telefone)
        {
            if (!string.IsNullOrWhiteSpace(nome))
                Nome = nome;

            if (!string.IsNullOrWhiteSpace(telefone))
                Telefone = telefone;
        }

        public void Desativar() => Ativo = false;
        public void Ativar() => Ativo = true;
    }
}
