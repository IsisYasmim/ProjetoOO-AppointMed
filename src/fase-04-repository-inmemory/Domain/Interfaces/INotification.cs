using System;

public interface INotification
{
    string Generate(string nome, string tipoConsulta, DateTime dataConsulta);
}