using System;
using System.Text;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        // --- cenários de teste ---
        // atraso
        CreateNotification("atrasado", "Isis Silva",  "Primeira Consulta", new DateTime(2025, 12, 08, 10, 0, 0));

        // cancelamento
        CreateNotification("cancelado","Isis Santos","Primeira Consulta", new DateTime(2025, 10, 11, 10, 15, 0));

        // confirmação de consulta
        CreateNotification("confirmado","Isis Sousa","Primeira Consulta", new DateTime(2025, 12, 08, 10, 0, 0));

        // confirmação de retorno
        CreateNotification("confirmado", "Isis Silva", "Retorno", new DateTime(2025, 11, 21, 0, 0, 0));
    }

    private static void CreateNotification(string status, string nomePaciente, string tipoConsulta, DateTime dataConsulta)
    {
        var msg = NotificationFactory.Create(status, nomePaciente, tipoConsulta, dataConsulta);
        Console.WriteLine(msg.Generate());
    }
}