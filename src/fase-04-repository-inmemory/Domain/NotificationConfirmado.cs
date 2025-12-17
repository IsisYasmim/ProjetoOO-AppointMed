using System;
using System.Globalization;

public class NotificationConfirmado : INotification
{
    public string Generate(string nome, string tipoConsulta, DateTime dataConsulta)
    {
        var pt = new CultureInfo("pt-BR");
        var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", pt);
        
        return $"Paciente: {nome}\nConsulta: {tipoConsulta}\nData da consulta: {dt}\n\u001b[42m\u001b[37mStatus: Confirmado\u001b[0m";
    }
}