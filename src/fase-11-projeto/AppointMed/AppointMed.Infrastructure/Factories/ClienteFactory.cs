using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.Interfaces;
using AppointMed.AppointMed.Core.ValueObjects;

namespace AppointMed.AppointMed.Infrastructure.Factories
{
    public class ClienteFactory : IClienteFactory
    {
        public Cliente CreateClientePadrao(string nome, Email email, string telefone, string cpf, DateTime dataNascimento)
        {
            return Cliente.Create(nome, email, telefone, cpf, dataNascimento);
        }

        public Cliente CreateClientePrioritario(string nome, Email email, string telefone, string cpf, DateTime dataNascimento, string tipoPrioridade)
        {
            var cliente = Cliente.Create(nome, email, telefone, cpf, dataNascimento);
            // Lógica adicional para clientes prioritários
            // Ex: adicionar flag, prioridade na fila, etc.
            return cliente;
        }

        public Cliente CreateClienteCorporativo(string nome, Email email, string telefone, string cpf, DateTime dataNascimento, string empresa)
        {
            var cliente = Cliente.Create(nome, email, telefone, cpf, dataNascimento);
            // Lógica adicional para clientes corporativos
            // Ex: desconto especial, condições especiais, etc.
            return cliente;
        }
    }
}
