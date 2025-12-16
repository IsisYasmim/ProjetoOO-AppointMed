using System;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        Console.WriteLine("=== Notificações de Consulta Médica ===\n");
        
        // Teste 1: Status Cancelado
        Console.WriteLine("1. Consulta Cancelada:");
        Console.WriteLine(Notification.Generate(
            "Cancelado", 
            "Isis Silva", 
            "Primeira consulta", 
            new DateTime(2025, 11, 15, 14, 30, 0)
        ));
        
        Console.WriteLine();
        
        // Teste 2: Status Atrasado
        Console.WriteLine("2. Consulta Atrasada:");
        Console.WriteLine(Notification.Generate(
            "Atrasado", 
            "Isis Santos", 
            "Primeira consulta", 
            new DateTime(2025, 11, 16, 9, 0, 0)
        ));
        
        Console.WriteLine();
        
        // Teste 3: Status Confirmado - Retorno (com emoji)
        Console.WriteLine("3. Consulta Confirmada - Retorno:");
        Console.WriteLine(Notification.Generate(
            "Confirmado", 
            "Isis Oliveira", 
            "Retorno", 
            new DateTime(2025, 11, 17, 10, 0, 0)
        ));
        
        Console.WriteLine();
        
        // Teste 4: Status Confirmado - Primeira Consulta (sem emoji)
        Console.WriteLine("4. Consulta Confirmada - Primeira Consulta:");
        Console.WriteLine(Notification.Generate(
            "Confirmado", 
            "Isis Oliveira", 
            "Primeira Consulta", 
            new DateTime(2025, 11, 18, 15, 30, 0)
        ));
    }
}