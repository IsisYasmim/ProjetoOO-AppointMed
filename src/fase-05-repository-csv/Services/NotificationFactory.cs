using System;
using AppointMed.Fase5.Domain.Interfaces;
using AppointMed.Fase5.Domain;

namespace AppointMed.Fase5.Services
{
    public static class NotificationFactory
    {
        public static INotificationGenerator Create(string status, string tipoConsulta)
        {
            if (string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(tipoConsulta))
                return new NotificationPadrao();

            var tc = tipoConsulta.Trim().ToLowerInvariant();
            var st = status.Trim().ToLowerInvariant();

            return st switch
            {
                "atrasado" => new NotificationAtrasado(),
                "cancelado" => new NotificationCancelado(),
                "confirmado" => tc.Contains("primeira consulta") ? new NotificationConfirmado() :
                                tc.Contains("retorno") ? new NotificationRetorno() : new NotificationPadrao(),
                _ => new NotificationPadrao()
            };
        }
    }
}