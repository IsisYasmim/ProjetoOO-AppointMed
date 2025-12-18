using System;
using System.Globalization;
using AppointMed.Fase6.Domain.Interfaces;
namespace AppointMed.Fase6.Services
{
    public class NotificationTipoConsultaService : INotificaTipoConsulta, IFormataDetalhes
    {
        public string NotificarPrimeiraConsulta(string nome, DateTime dataConsulta)
        {
            var culture = new CultureInfo("pt-BR");
            var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", culture);
            
            return $"\u001b[42m\u001b[37m🎯  PRIMEIRA CONSULTA\u001b[0m\n" +
                $"Paciente: {nome}\n" +
                $"Tipo: Primeira Consulta\n" +
                $"Data: {dt}\n" +
                $"Ações necessárias:\n" +
                $"  • Coletar histórico completo\n" +
                $"  • Preparar formulários de admissão\n" +
                $"  • Reservar 60 minutos para consulta";
        }

        public string NotificarRetorno(string nome, DateTime dataConsulta)
        {
            var culture = new CultureInfo("pt-BR");
            var dt = dataConsulta.ToString("dd/MM 'às' HH:mm", culture);
            
            return $"\u001b[44m\u001b[37m🔄  CONSULTA DE RETORNO\u001b[0m\n" +
                $"Paciente: {nome}\n" +
                $"Tipo: Retorno\n" +
                $"Data: {dt}\n" +
                $"Ações necessárias:\n" +
                $"  • Buscar prontuário anterior\n" +
                $"  • Preparar evolução do caso\n" +
                $"  • Reservar 30 minutos para consulta";
        }

        public string FormatDetails(string nome, string tipoConsulta, DateTime dataConsulta)
        {
            var culture = new CultureInfo("pt-BR");
            var dt = dataConsulta.ToString("dd/MM/yyyy HH:mm", culture);
            return $"{tipoConsulta.ToUpper()} | {nome} | {dt}";
        }
    }
}