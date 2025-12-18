using System;
using System.Globalization;
using AppointMed.Fase5.Domain.Interfaces;

namespace AppointMed.Fase5.Domain
{

public class NotificationPadrao: INotificationGenerator, INotificationFormatter
{
    // Implementação pública da interface INotificationGenerator
    public string GenerateNotification(string nome, string tipoConsulta, DateTime dataConsulta)
    {
        var culture = new CultureInfo("pt-BR");
        var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", culture);

        return $"Paciente: {nome}\nConsulta: {tipoConsulta}\nData da consulta: {dt}";
    }

    // Implementação explícita da interface INotificationFormatter
    string INotificationFormatter.FormatDetails(string nome, string tipoConsulta, DateTime dataConsulta)
    {
        var culture = new CultureInfo("pt-BR");
        var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", culture);
        return $"Detalhes: {nome} | {tipoConsulta} | {dt}";
    }
}
}