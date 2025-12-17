using System;
using System.Globalization;

public class NotificationAtrasado : INotification
{
    public string Generate(string nome, string tipoConsulta, DateTime dataConsulta)
    {
        var pt = new CultureInfo("pt-BR");
        var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", pt);
        var daysLate = (DateTime.Now.Date - dataConsulta.Date).Days;
        var diasTexto = daysLate > 0 ? $"{daysLate} dia{(daysLate == 1 ? "" : "s")} de atraso" : "0 dias de atraso";
        
        return $"Paciente: {nome}\nConsulta: {tipoConsulta}\nData da consulta: {dt}\n\u001b[43m\u001b[30mStatus: Atrasado ({diasTexto})\u001b[0m";
    }
}