using System;
namespace AppointMed.Fase5.Domain.Interfaces
{
    // Interface secundária: formatar detalhes da notificação
    public interface INotificationFormatter
    {
        string FormatDetails(string nome, string tipoConsulta, DateTime dataConsulta);
    }
}