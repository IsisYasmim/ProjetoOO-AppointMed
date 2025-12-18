using System;
namespace AppointMed.Fase6.Domain.Interfaces
{
    // 4. Interface unificada minimalista (para compatibilidade)
    public interface INotificationGenerator
    {
        string GenerateNotification(string status, string nome, string tipoConsulta, DateTime dataConsulta);
    }
}
