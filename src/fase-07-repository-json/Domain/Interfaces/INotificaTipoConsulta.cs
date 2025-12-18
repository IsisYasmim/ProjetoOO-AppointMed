using System;
namespace Fase07.Domain.Interfaces
{
    public interface INotificaTipoConsulta
    {
        string NotificarPrimeiraConsulta(string nome, DateTime dataConsulta);
        string NotificarRetorno(string nome, DateTime dataConsulta);
    }
}
