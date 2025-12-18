using System;
namespace AppointMed.Fase5.Domain.Interfaces
{
    // Interface principal: gerar notificação
    public interface INotificationGenerator
    {
        string GenerateNotification(string nome, string tipoConsulta, DateTime dataConsulta);
    }
}
