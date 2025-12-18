using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.ValueObjects;

namespace AppointMed.AppointMed.Core.Interfaces
{
    // Factory para criação de clientes com diferentes estratégias

    public interface IClienteFactory
    {
        // Cria um cliente padrão
        Cliente CreateClientePadrao(string nome, Email email, string telefone, string cpf, DateTime dataNascimento);

        // Cria um cliente prioritário (idoso, gestante, etc)
        Cliente CreateClientePrioritario(string nome, Email email, string telefone, string cpf, DateTime dataNascimento, string tipoPrioridade);

        // Cria um cliente corporativo (convênio empresarial)
        Cliente CreateClienteCorporativo(string nome, Email email, string telefone, string cpf, DateTime dataNascimento, string empresa);
    }
}
