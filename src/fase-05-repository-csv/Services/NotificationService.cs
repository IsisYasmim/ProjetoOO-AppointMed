using System;
using AppointMed.Fase5.Domain.Interfaces;
using AppointMed.Fase5.Domain;

namespace AppointMed.Fase5.Services
{
    public class NotificationService<T> where T : INotificationGenerator, new()
    {
        public string CreateWithExisting(T generator, string nome, string tipoConsulta, DateTime dataConsulta)
        {
            return generator.GenerateNotification(nome, tipoConsulta, dataConsulta);
        }

        public string CreateWithNew(string nome, string tipoConsulta, DateTime dataConsulta)
        {
            var generator = new T();
            return generator.GenerateNotification(nome, tipoConsulta, dataConsulta);
        }
    }
}