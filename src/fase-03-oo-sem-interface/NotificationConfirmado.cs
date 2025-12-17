using System;

public sealed class NotificationConfirmado : Notification
{
    public NotificationConfirmado(string nome, string tipoConsulta, DateTime dataConsulta)
        : base(nome, tipoConsulta, dataConsulta) { }

    protected override string NotificationText()
    {
        var dt = DateTimeFormat();
        return $"Paciente: {NomePaciente}\nConsulta: {TipoConsulta}\nData da consulta: {dt}\n\u001b[42m\u001b[37mStatus: Confirmado\u001b[0m";
    }
}