using System;
using System.IO;
using System.Text;
using Fase07.Domain.Interfaces;
using Fase07.Infrastructure.Repositories;
using Fase07.Services;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        Console.WriteLine("=== FASE 7: REPOSITORY JSON ===");
        Console.WriteLine("Sistema de Notificação Médica com Persistência JSON\n");
        
        // Caminho para o arquivo JSON (usando diretório atual)
        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "notificacoes.json");
        Console.WriteLine($"Arquivo JSON: {jsonPath}");
        
        // Criar repository
        var repository = new JsonNotificacaoRepository(jsonPath);
        
        // Serviços das fases anteriores (ISP)
        var statusService = new NotificationStatusService();
        var tipoService = new NotificationTipoConsultaService();
        
        // Serviço principal
        var notificacaoService = new NotificacaoService(repository, statusService, tipoService);
        
        Console.WriteLine("\n--- OPERAÇÕES CRUD COM JSON ---");
        
        // 1. Criar algumas notificações
        Console.WriteLine("\n1. Criando notificações...");
        
        var notif1 = notificacaoService.CriarNotificacaoAtrasado(
            "Carlos Silva",
            "Consulta Cardiológica",
            new DateTime(2025, 11, 10, 14, 30, 0),
            "whatsapp"
        );
        Console.WriteLine($"   Notificação criada: ID={notif1.Id}, Tipo={notif1.Tipo}");
        
        var notif2 = notificacaoService.CriarNotificacaoCancelado(
            "Ana Oliveira",
            "Exame de Sangue",
            new DateTime(2025, 12, 15, 9, 0, 0),
            "email"
        );
        Console.WriteLine($"   Notificação criada: ID={notif2.Id}, Tipo={notif2.Tipo}");
        
        var notif3 = notificacaoService.CriarNotificacaoPrimeiraConsulta(
            "Mariana Costa",
            new DateTime(2025, 11, 25, 10, 0, 0),
            "app"
        );
        Console.WriteLine($"   Notificação criada: ID={notif3.Id}, Tipo={notif3.Tipo}");
        
        // 2. Listar todas as notificações
        Console.WriteLine("\n2. Listando todas as notificações:");
        var todas = repository.ListAll();
        foreach (var n in todas)
        {
            Console.WriteLine($"   ID={n.Id}, {n.Tipo}, Paciente={n.NomePaciente}, Status={n.Status}");
        }
        
        // 3. Buscar por ID
        Console.WriteLine("\n3. Buscando notificação por ID (ID=2):");
        var encontrada = repository.GetById(2);
        if (encontrada != null)
        {
            Console.WriteLine($"   Encontrada: {encontrada.NomePaciente} - {encontrada.Mensagem}");
        }
        
        // 4. Atualizar status
        Console.WriteLine("\n4. Atualizando status da notificação 1 para 'enviada':");
        var sucesso = notificacaoService.MarcarComoEnviada(1);
        Console.WriteLine($"   Status atualizado: {sucesso}");
        
        // 5. Listar pendentes
        Console.WriteLine("\n5. Notificações pendentes:");
        var pendentes = notificacaoService.ListarPendentes();
        foreach (var p in pendentes)
        {
            Console.WriteLine($"   ID={p.Id}, Paciente={p.NomePaciente}, Status={p.Status}");
        }
        
        // 6. Listar por tipo
        Console.WriteLine("\n6. Notificações do tipo 'atrasado':");
        var atrasados = notificacaoService.ListarPorTipo("atrasado");
        foreach (var a in atrasados)
        {
            Console.WriteLine($"   ID={a.Id}, Paciente={a.NomePaciente}, Dias de atraso: {a.DataConsulta}");
        }
        
        // 7. Remover uma notificação
        Console.WriteLine("\n7. Removendo notificação com ID=2:");
        var removido = repository.Remove(2);
        Console.WriteLine($"   Removido: {removido}");
        
        // 8. Verificar conteúdo do arquivo
        Console.WriteLine("\n8. Conteúdo do arquivo JSON (primeiras 500 caracteres):");
        if (File.Exists(jsonPath))
        {
            var jsonContent = File.ReadAllText(jsonPath);
            var preview = jsonContent.Length > 500 ? jsonContent.Substring(0, 500) + "..." : jsonContent;
            Console.WriteLine(preview);
        }
        
        // 9. Próximo ID disponível
        Console.WriteLine("\n9. Próximo ID disponível:");
        Console.WriteLine($"   NextId() = {repository.NextId()}");
        
        Console.WriteLine("\n✅ Repository JSON implementado com sucesso!");
        Console.WriteLine($"Arquivo salvo em: {jsonPath}");
    }
}