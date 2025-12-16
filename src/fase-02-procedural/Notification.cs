using System.Globalization;
using System.Text;

public static class Notification
{
    /// Gera o texto que será exibido sobre a consulta médica,
    /// variando conforme o status e tipo da consulta.

    public static string Generate(string status, string nomePaciente, string tipoConsulta, DateTime dataHora)
    {
        var statusLower = (status ?? string.Empty).Trim().ToLowerInvariant();
        var tipoConsultaLower = (tipoConsulta ?? string.Empty).Trim().ToLowerInvariant();
        var pt = new CultureInfo("pt-BR");
        var dt = dataHora.ToString("dd/MM/yyyy 'às' HH:mm", pt);

        var mensagem = new StringBuilder();
        
        switch (statusLower)
        {
            case "cancelado":
                // Fundo vermelho (41), texto branco (37)
                mensagem.AppendLine("Paciente:" + nomePaciente);
                mensagem.AppendLine("Consulta:" + tipoConsulta);
                mensagem.AppendLine($"Data da consulta: {dt}");
                mensagem.AppendLine($"\u001b[41m\u001b[37mStatus: Cancelado\u001b[0m");
                break;

            case "atrasado":
                // Fundo amarelo (43), texto preto (30)
                mensagem.AppendLine($"Paciente: {nomePaciente}");
                mensagem.AppendLine($"Consulta: {tipoConsulta}");
                mensagem.AppendLine($"Data da consulta: {dt}");
                mensagem.AppendLine($"\u001b[43m\u001b[30mStatus: Atrasado\u001b[0m");
                break;

            case "confirmado":
                // Fundo verde (42), texto branco (37)
                if (tipoConsultaLower.Contains("retorno") || tipoConsultaLower == "retorno")
                {
                    mensagem.AppendLine($"Paciente: {nomePaciente}");
                    mensagem.AppendLine("Consulta: Retorno 🔄");
                    mensagem.AppendLine($"Data da consulta: {dt}");
                    mensagem.AppendLine($"\u001b[42m\u001b[37mStatus: Confirmado\u001b[0m");
                }
                else if (tipoConsultaLower.Contains("primeira") || 
                        tipoConsultaLower.Contains("primeira consulta"))
                {
                    mensagem.AppendLine($"Paciente: {nomePaciente}");
                    mensagem.AppendLine($"Consulta: Primeira Consulta");
                    mensagem.AppendLine($"Data da consulta: {dt}");
                    mensagem.AppendLine($"\u001b[42m\u001b[37mStatus: Confirmado\u001b[0m");
                }
                else
                {
                    mensagem.AppendLine($"Paciente: {nomePaciente}");
                    mensagem.AppendLine($"Consulta: {tipoConsulta}");
                    mensagem.AppendLine($"Data da consulta: {dt}");
                    mensagem.AppendLine($"\u001b[42m\u001b[37mStatus: Confirmado\u001b[0m");
                }
                break;

            default:
                // Fundo azul (44), texto branco (37)
                mensagem.AppendLine($"Paciente: {nomePaciente}");
                mensagem.AppendLine($"Consulta: {tipoConsulta}");
                mensagem.AppendLine($"Data da consulta: {dt}");
                mensagem.AppendLine($"\u001b[44m\u001b[37mStatus: {status}\u001b[0m");
                break;
        }

        return mensagem.ToString();
    }
}