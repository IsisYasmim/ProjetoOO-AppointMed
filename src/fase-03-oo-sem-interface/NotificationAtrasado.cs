using System;

public sealed class NotificationAtrasado : Notification
{
    public NotificationAtrasado(string nome, string tipoConsulta, DateTime dataConsulta)
        : base(nome, tipoConsulta, dataConsulta) { }

    protected override string NotificationText()
    {
        var dt = DateTimeFormat();
        var daysLate = (DateTime.Now.Date - DataConsulta.Date).Days;
        var diasTexto = daysLate > 0 ? $"{daysLate} dia{(daysLate == 1 ? "" : "s")} de atraso" : "0 dias de atraso";
        return $"Paciente: {NomePaciente}\nConsulta: {TipoConsulta}\nData da consulta: {dt}\n\u001b[43m\u001b[30mStatus: Atrasado ({diasTexto})\u001b[0m";
    }
}