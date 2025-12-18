using System;
using System.Text;
using System.Globalization;
using AppointMed.Fase5.Domain;
using AppointMed.Fase5.Domain.Interfaces;
using AppointMed.Fase5.Services;

namespace AppointMed.Fase5
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // --- 1. Uso da classe que implementa duas interfaces ---
            Console.WriteLine("=== Classe com duas interfaces (exemplo com atraso) ===");
            var notifAtrasado = new NotificationAtrasado();

            // Usando INotificationGenerator
            Console.WriteLine(notifAtrasado.GenerateNotification("Isis Silva", "Primeira Consulta", new DateTime(2025, 12, 08, 10, 0, 0)));

            // Usando INotificationFormatter (via casting)
            if (notifAtrasado is INotificationFormatter formatter)
            {
                Console.WriteLine(formatter.FormatDetails("Isis Silva", "Primeira Consulta", new DateTime(2025, 12, 08, 10, 0, 0)));
            }

            Console.WriteLine();

            // --- 2. Uso do serviço genérico com constraints ---
            Console.WriteLine("=== Serviço genérico NotificationService<T> ===");
            var service = new NotificationService<NotificationAtrasado>();
            Console.WriteLine(service.CreateWithNew("Maria", "Retorno", DateTime.Now));

            Console.WriteLine();

            // --- 3. Cenários de teste usando a fábrica original (mantida para compatibilidade) ---
            Console.WriteLine("=== Cenários de teste via fábrica ===");
            CreateNotification("atrasado", "Isis Silva", "Primeira Consulta", new DateTime(2025, 12, 08, 10, 0, 0));
            CreateNotification("cancelado", "Isis Santos", "Primeira Consulta", new DateTime(2025, 10, 11, 10, 15, 0));
            CreateNotification("confirmado", "Isis Sousa", "Primeira Consulta", new DateTime(2025, 12, 08, 10, 0, 0));
            CreateNotification("confirmado", "Isis Silva", "Retorno", new DateTime(2025, 11, 21, 0, 0, 0));
        }

        static void CreateNotification(string status, string nome, string tipoConsulta, DateTime data)
        {
            INotificationGenerator notification = NotificationFactory.Create(status, tipoConsulta);
            Console.WriteLine(notification.GenerateNotification(nome, tipoConsulta, data));
        }
    }
}