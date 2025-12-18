using System;
namespace AppointMed.Fase6.Domain.Interfaces
{
    public interface INotificaTipoConsulta
    {
        string NotificarPrimeiraConsulta(string nome, DateTime dataConsulta);
        string NotificarRetorno(string nome, DateTime dataConsulta);
    }
}
