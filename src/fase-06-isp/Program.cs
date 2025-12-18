using System;
using System.Text;
using AppointMed.Fase6.Services;
using AppointMed.Fase6.Domain.Interfaces;
using System.Globalization;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        Console.WriteLine("=== FASE 6: ISP NA PRÁTICA ===");
        Console.WriteLine("Sistema de Notificação Médica Segregado\n");
        
        // Criando serviços especializados
        var statusService = new NotificationStatusService();
        var tipoService = new NotificationTipoConsultaService();
        var compatService = new NotificationCompatibilityService(statusService, tipoService);
        
        // Criando casos de uso específicos
        var atrasoService = new AtrasoService(statusService);
        var primeiraConsultaService = new PrimeiraConsultaService(tipoService);
        var formatadorService = new FormatadorService(statusService); // pode usar qualquer implementador
        
        Console.WriteLine("--- NOTIFICAÇÕES POR STATUS ---");
        
        Console.WriteLine("\n1. PACIENTE ATRASADO:");
        Console.WriteLine(atrasoService.NotificarAtraso(
            "Santos Silva", 
            "Consulta Cardiológica", 
            new DateTime(2025, 11, 10, 14, 30, 0)
        ));
        
        Console.WriteLine("\n2. CONSULTA CANCELADA:");
        Console.WriteLine(statusService.NotificarCancelado(
            "Ana Oliveira", 
            "Exame de Sangue", 
            new DateTime(2025, 12, 15, 9, 0, 0)
        ));
        
        Console.WriteLine("\n--- NOTIFICAÇÕES POR TIPO DE CONSULTA ---");
        
        Console.WriteLine("\n3. PRIMEIRA CONSULTA:");
        Console.WriteLine(primeiraConsultaService.NotificarPrimeiraConsulta(
            "Mariana Costa",
            new DateTime(2025, 11, 25, 10, 0, 0)
        ));
        
        Console.WriteLine("\n4. CONSULTA DE RETORNO:");
        Console.WriteLine(tipoService.NotificarRetorno(
            "Roberto Santos",
            new DateTime(2025, 12, 5, 16, 30, 0)
        ));
        
        Console.WriteLine("\n--- FORMATAÇÃO DE DETALHES ---");
        
        Console.WriteLine("\n5. DETALHES FORMATADOS:");
        Console.WriteLine(formatadorService.FormatDetalhes(
            "Fernanda Lima",
            "Avaliação Ortopédica",
            new DateTime(2025, 11, 28, 11, 15, 0)
        ));
        
        Console.WriteLine("\n--- COMPATIBILIDADE (interface antiga) ---");
        
        Console.WriteLine("\n6. USANDO INTERFACE ANTIGA (compatibilidade):");
        Console.WriteLine(compatService.GenerateNotification(
            "atrasado",
            "João Mendes",
            "Fisioterapia",
            new DateTime(2025, 11, 5, 8, 0, 0)
        ));
        
        Console.WriteLine("\n=== DEMONSTRAÇÃO ISP ===");
        Console.WriteLine($"StatusService implementa INotificaStatus: {statusService is INotificaStatus}");
        Console.WriteLine($"StatusService implementa INotificaTipoConsulta: {statusService is INotificaTipoConsulta}");
        Console.WriteLine($"TipoService implementa INotificaTipoConsulta: {tipoService is INotificaTipoConsulta}");
        Console.WriteLine($"Ambos implementam IFormataDetalhes: {(statusService is IFormataDetalhes) && (tipoService is IFormataDetalhes)}");
        
        Console.WriteLine("\n✅ ISP aplicado com sucesso!");
        Console.WriteLine("Cada serviço depende apenas das interfaces que realmente usa.");
    }
}