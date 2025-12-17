using System;
using System.Globalization;

public abstract class Notification
{
    protected string NomePaciente { get; }
    protected string TipoConsulta { get; }
    protected DateTime DataConsulta { get; }
    protected CultureInfo PtBr { get; } = new CultureInfo("pt-BR");

    protected Notification(string nomePaciente, string tipoConsulta, DateTime dataConsulta)
    {
        NomePaciente = nomePaciente ?? string.Empty;
        TipoConsulta = tipoConsulta ?? string.Empty;
        DataConsulta = dataConsulta;
    }

    public string Generate()
    {
        return NotificationText();
    }

    protected abstract string NotificationText();

    protected string DateTimeFormat() => DataConsulta.ToString("dd/MM 'às' HH:mm", PtBr);
}