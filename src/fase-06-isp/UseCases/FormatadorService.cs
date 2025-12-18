using System;
using AppointMed.Fase6.Domain.Interfaces;
namespace AppointMed.Fase6.Services
{
    public class FormatadorService
    {
        private readonly IFormataDetalhes _formatador;

        public FormatadorService(IFormataDetalhes formatador)
        {
            _formatador = formatador;
        }

        public string FormatDetalhes(string nome, string tipoConsulta, DateTime dataConsulta)
        {
            return _formatador.FormatDetails(nome, tipoConsulta, dataConsulta);
        }
    }
}