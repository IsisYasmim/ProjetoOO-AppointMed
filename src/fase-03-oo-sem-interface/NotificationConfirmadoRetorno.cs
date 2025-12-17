using System;

public sealed class NotificationConfirmadoRetorno : Notification
{
    public NotificationConfirmadoRetorno(string nome, string tipoConsulta, DateTime dataConsulta)
        : base(nome, tipoConsulta, dataConsulta) { }

    protected override string NotificationText()
    {
        var dt = DateTimeFormat();
        return $"Paciente: {NomePaciente}\nConsulta: Retorno 🔄\nData da consulta: {dt}\n\u001b[42m\u001b[37mStatus: Confirmado\u001b[0m";
    }
}