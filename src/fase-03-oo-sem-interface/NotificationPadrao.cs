using System;

public sealed class NotificationPadrao : Notification
{
    public NotificationPadrao(string nome, string tipoConsulta, DateTime dataConsulta)
        : base(nome, tipoConsulta, dataConsulta) { }

    protected override string NotificationText()
    {
        var dt = DateTimeFormat();
        return $"Paciente: {NomePaciente}\nConsulta: {TipoConsulta}\nData da consulta: {dt}";
    }
}