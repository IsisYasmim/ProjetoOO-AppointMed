using System;
using System.Globalization;

public class NotificationPadrao : INotification
{
    public string Generate(string nome, string tipoConsulta, DateTime dataConsulta)
    {
        var pt = new CultureInfo("pt-BR");
        var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", pt);
        var daysLate = (DateTime.Now.Date - dataConsulta.Date).Days;
        return $"Paciente: {nome}\nConsulta: {tipoConsulta}\nData da consulta: {dt}";
    }
}