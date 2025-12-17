using System;

public sealed class NotificationCancelado : Notification
{
    public NotificationCancelado(string nome, string servico, DateTime dataConsulta)
        : base(nome, servico, dataConsulta) { }

    protected override string NotificationText()
    {
        var dt = DateTimeFormat();
        return $"Paciente: {NomePaciente}\nConsulta: {TipoConsulta}\nData da consulta: {dt}\n\u001b[41m\u001b[37mStatus: Cancelado\u001b[0m";
    }
}