using System;
using AppointMed.Fase6.Domain.Interfaces;

namespace AppointMed.Fase6.Services
{
    public class AtrasoService
    {
        private readonly INotificaStatus _notificador;

        public AtrasoService(INotificaStatus notificador)
        {
            _notificador = notificador;
        }

        public string NotificarAtraso(string nome, string tipoConsulta, DateTime dataConsulta)
        {
            int diasAtraso = (DateTime.Now.Date - dataConsulta.Date).Days;
            if (diasAtraso < 0) diasAtraso = 0;
            
            return _notificador.NotificarAtrasado(nome, tipoConsulta, dataConsulta, diasAtraso);
        }
    }
}