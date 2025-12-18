using System;
using System.Globalization;
using AppointMed.Fase5.Domain.Interfaces;

namespace AppointMed.Fase5.Domain
{

public class NotificationAtrasado : INotificationGenerator, INotificationFormatter
{
    // Implementação pública da interface INotificationGenerator
    public string GenerateNotification(string nome, string tipoConsulta, DateTime dataConsulta)
    {
        var culture = new CultureInfo("pt-BR");
        var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", culture);
        var daysLate = (DateTime.Now.Date - dataConsulta.Date).Days;
        var diasTexto = daysLate > 0 ? $"{daysLate} dia{(daysLate == 1 ? "" : "s")} de atraso" : "0 dias de atraso";

        return $"Paciente: {nome}\nConsulta: {tipoConsulta}\nData da consulta: {dt}\n\u001b[43m\u001b[30mStatus: Atrasado ({diasTexto})\u001b[0m";
    }

    // Implementação explícita da interface INotificationFormatter
    string INotificationFormatter.FormatDetails(string nome, string tipoConsulta, DateTime dataConsulta)
    {
        var culture = new CultureInfo("pt-BR");
        var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", culture);
        return $"Detalhes: {nome} | {tipoConsulta} | {dt} | Status: Atrasado";
    }
}
}