using System;
namespace Fase07.Domain.Interfaces
{
    // 1. Notificações baseadas em STATUS do agendamento
    public interface INotificaStatus
    {
        string NotificarAtrasado(string nome, string tipoConsulta, DateTime dataConsulta, int diasAtraso);
        string NotificarCancelado(string nome, string tipoConsulta, DateTime dataConsulta);
        string NotificarConfirmado(string nome, string tipoConsulta, DateTime dataConsulta);
    }
}
