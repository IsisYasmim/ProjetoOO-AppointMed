using System;
namespace AppointMed.Fase6.Domain.Interfaces
{
    public interface IFormataDetalhes
    {
        string FormatDetails(string nome, string tipoConsulta, DateTime dataConsulta);
    }
}
