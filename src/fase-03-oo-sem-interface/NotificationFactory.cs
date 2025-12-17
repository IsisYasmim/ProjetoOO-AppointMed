using System;

public static class NotificationFactory
{
    public static Notification Create(string status, string nomePaciente, string tipoConsulta, DateTime dataConsulta)
    {
        var st = (status ?? string.Empty).Trim().ToLowerInvariant();
        var tp = (tipoConsulta ?? string.Empty).Trim().ToLowerInvariant();

        switch (st)
        {
            case "cancelado":
                return new NotificationCancelado(nomePaciente, tipoConsulta, dataConsulta);

            case "atrasado":
                return new NotificationAtrasado(nomePaciente, tipoConsulta, dataConsulta);

            case "confirmado":
                if (tp.Contains("retorno") || tp == "retorno")
                {
                    return new NotificationConfirmadoRetorno(nomePaciente, tipoConsulta, dataConsulta);
                }
                else if (tp.Contains("primeira") || tp.Contains("primeira consulta"))
                {
                    return new NotificationConfirmado(nomePaciente, tipoConsulta, dataConsulta);
                }
                else
                {
                    return new NotificationPadrao(nomePaciente, tipoConsulta, dataConsulta);
                }

            default:
                return new NotificationPadrao(nomePaciente, tipoConsulta, dataConsulta);
        }
    }
}