using System;
using System.Globalization;
using AppointMed.Fase6.Domain.Interfaces;
namespace AppointMed.Fase6.Services
{
    public class NotificationStatusService : INotificaStatus, IFormataDetalhes
    {
        public string NotificarAtrasado(string nome, string tipoConsulta, DateTime dataConsulta, int diasAtraso)
        {
            var culture = new CultureInfo("pt-BR");
            var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", culture);
            var diasTexto = diasAtraso > 0 ? $"{diasAtraso} dia{(diasAtraso == 1 ? "" : "s")} de atraso" : "hoje";
            
            return $"\u001b[43m\u001b[30m⚠️  PACIENTE ATRASADO\u001b[0m\n" +
                $"Nome: {nome}\n" +
                $"Consulta: {tipoConsulta}\n" +
                $"Data agendada: {dt}\n" +
                $"Status: {diasTexto}\n" +
                $"Ação: Contatar paciente urgentemente";
        }

        public string NotificarCancelado(string nome, string tipoConsulta, DateTime dataConsulta)
        {
            var culture = new CultureInfo("pt-BR");
            var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", culture);
            
            return $"\u001b[41m\u001b[37m❌  CONSULTA CANCELADA\u001b[0m\n" +
                $"Nome: {nome}\n" +
                $"Consulta: {tipoConsulta}\n" +
                $"Data cancelada: {dt}\n" +
                $"Status: Cancelada pelo paciente\n" +
                $"Ação: Liberar horário para outros pacientes";
        }

        public string NotificarConfirmado(string nome, string tipoConsulta, DateTime dataConsulta)
        {
            var culture = new CultureInfo("pt-BR");
            var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", culture);
            
            return $"\u001b[42m\u001b[37m✅  CONSULTA CONFIRMADA\u001b[0m\n" +
                $"Nome: {nome}\n" +
                $"Consulta: {tipoConsulta}\n" +
                $"Data confirmada: {dt}\n" +
                $"Status: Confirmada\n" +
                $"Ação: Preparar consultório";
        }

        public string FormatDetails(string nome, string tipoConsulta, DateTime dataConsulta)
        {
            var culture = new CultureInfo("pt-BR");
            var dt = dataConsulta.ToString("dd/MM/yyyy HH:mm", culture);
            return $"{nome} | {tipoConsulta} | {dt}";
        }
    }
}