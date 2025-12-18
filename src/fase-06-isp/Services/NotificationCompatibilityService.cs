using System;
using AppointMed.Fase6.Domain.Interfaces;
namespace AppointMed.Fase6.Services
{
    public class NotificationCompatibilityService : INotificationGenerator
    {
        private readonly INotificaStatus _statusService;
        private readonly INotificaTipoConsulta _tipoService;

        public NotificationCompatibilityService(
            INotificaStatus statusService,
            INotificaTipoConsulta tipoService)
        {
            _statusService = statusService;
            _tipoService = tipoService;
        }

        public string GenerateNotification(string status, string nome, string tipoConsulta, DateTime dataConsulta)
        {
            // Calcula dias de atraso se for status atrasado
            int diasAtraso = 0;
            if (status == "atrasado")
            {
                diasAtraso = (DateTime.Now.Date - dataConsulta.Date).Days;
                if (diasAtraso < 0) diasAtraso = 0;
            }

            return status switch
            {
                "atrasado" => _statusService.NotificarAtrasado(nome, tipoConsulta, dataConsulta, diasAtraso),
                "cancelado" => _statusService.NotificarCancelado(nome, tipoConsulta, dataConsulta),
                "confirmado" => _statusService.NotificarConfirmado(nome, tipoConsulta, dataConsulta),
                _ => tipoConsulta switch
                {
                    "primeira consulta" => _tipoService.NotificarPrimeiraConsulta(nome, dataConsulta),
                    "retorno" => _tipoService.NotificarRetorno(nome, dataConsulta),
                    _ => $"Notificação padrão para {nome}"
                }
            };
        }
    }
}