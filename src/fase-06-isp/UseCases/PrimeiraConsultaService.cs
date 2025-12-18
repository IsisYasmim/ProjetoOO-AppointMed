using System;
using AppointMed.Fase6.Domain.Interfaces;
namespace AppointMed.Fase6.Services
{
    public class PrimeiraConsultaService
    {
        private readonly INotificaTipoConsulta _notificador;

        public PrimeiraConsultaService(INotificaTipoConsulta notificador)
        {
            _notificador = notificador;
        }

        public string NotificarPrimeiraConsulta(string nome, DateTime dataConsulta)
        {
            return _notificador.NotificarPrimeiraConsulta(nome, dataConsulta);
        }
    }
}