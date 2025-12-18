using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.Interfaces;
using AppointMed.AppointMed.Infrastructure.Data;
using AppointMed.AppointMed.Infrastructure.Factories;
using AppointMed.AppointMed.Infrastructure.Repositories;
using AppointMed.AppointMed.Services;


class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== AppointMed - Sistema de Agendamento Médico ===");

        // Configurar DI Container
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Database
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("Data Source=appointmed.db"));

                // Repositories
                services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

                // Factories
                services.AddSingleton<IClienteFactory, ClienteFactory>();
                services.AddSingleton<IAgendamentoFactory, AgendamentoFactory>();
                services.AddSingleton<INotificadorFactory, NotificadorFactory>();

                // Services
                services.AddScoped<ClienteService>();
                services.AddScoped<AgendamentoService>();
                services.AddScoped<INotificacaoService, NotificacaoService>();
            })
            .Build();

        // Criar e migrar banco de dados
        using (var scope = host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            Console.WriteLine("Banco de dados criado/migrado com sucesso.");
        }

        // Menu interativo
        var running = true;
        while (running)
        {
            Console.WriteLine("\n=== MENU PRINCIPAL ===");
            Console.WriteLine("1. Cadastrar Cliente");
            Console.WriteLine("2. Listar Clientes");
            Console.WriteLine("3. Criar Agendamento");
            Console.WriteLine("4. Listar Agendamentos");
            Console.WriteLine("5. Confirmar Agendamento");
            Console.WriteLine("6. Cancelar Agendamento");
            Console.WriteLine("7. Estatísticas");
            Console.WriteLine("0. Sair");
            Console.Write("\nEscolha uma opção: ");

            var option = Console.ReadLine();

            using var scope = host.Services.CreateScope();
            var clienteService = scope.ServiceProvider.GetRequiredService<ClienteService>();
            var agendamentoService = scope.ServiceProvider.GetRequiredService<AgendamentoService>();
            var notificacaoService = scope.ServiceProvider.GetRequiredService<INotificacaoService>();

            try
            {
                switch (option)
                {
                    case "1":
                        await CadastrarClienteAsync(clienteService);
                        break;
                    case "2":
                        await ListarClientesAsync(clienteService);
                        break;
                    case "3":
                        await CriarAgendamentoAsync(clienteService, agendamentoService);
                        break;
                    case "4":
                        await ListarAgendamentosAsync(agendamentoService);
                        break;
                    case "5":
                        await ConfirmarAgendamentoAsync(agendamentoService, notificacaoService);
                        break;
                    case "6":
                        await CancelarAgendamentoAsync(agendamentoService, notificacaoService);
                        break;
                    case "7":
                        await MostrarEstatisticasAsync(clienteService, agendamentoService);
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Encerrando sistema...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }

    static async Task CadastrarClienteAsync(ClienteService service)
    {
        Console.WriteLine("\n--- CADASTRAR CLIENTE ---");

        Console.Write("Nome: ");
        var nome = Console.ReadLine();

        Console.Write("Email: ");
        var email = Console.ReadLine();

        Console.Write("Telefone: ");
        var telefone = Console.ReadLine();

        Console.Write("CPF (apenas números): ");
        var cpf = Console.ReadLine();

        Console.Write("Data de Nascimento (dd/MM/yyyy): ");
        if (DateTime.TryParse(Console.ReadLine(), out var dataNascimento))
        {
            var cliente = await service.CadastrarClientePadraoAsync(nome!, email!, telefone!, cpf!, dataNascimento);
            Console.WriteLine($"Cliente cadastrado com ID: {cliente.Id}");
        }
        else
        {
            Console.WriteLine("Data inválida!");
        }
    }

    static async Task ListarClientesAsync(ClienteService service)
    {
        Console.WriteLine("\n--- LISTA DE CLIENTES ---");
        var clientes = await service.ObterTodosClientesAsync();

        foreach (var cliente in clientes)
        {
            Console.WriteLine($"ID: {cliente.Id}");
            Console.WriteLine($"Nome: {cliente.Nome}");
            Console.WriteLine($"Email: {cliente.Email}");
            Console.WriteLine($"Telefone: {cliente.Telefone}");
            Console.WriteLine($"Status: {(cliente.Ativo ? "Ativo" : "Inativo")}");
            Console.WriteLine("---");
        }

        Console.WriteLine($"Total: {clientes.Count()} clientes");
    }

    static async Task CriarAgendamentoAsync(ClienteService clienteService, AgendamentoService agendamentoService)
    {
        Console.WriteLine("\n--- CRIAR AGENDAMENTO ---");

        // Listar clientes para escolha
        var clientes = await clienteService.ObterTodosClientesAsync();
        if (!clientes.Any())
        {
            Console.WriteLine("Nenhum cliente cadastrado!");
            return;
        }

        Console.WriteLine("Clientes disponíveis:");
        foreach (var cliente in clientes)
        {
            Console.WriteLine($"{cliente.Id} - {cliente.Nome}");
        }

        Console.Write("\nID do Cliente: ");
        if (!Guid.TryParse(Console.ReadLine(), out var clienteId))
        {
            Console.WriteLine("ID inválido!");
            return;
        }

        Console.Write("Médico: ");
        var medico = Console.ReadLine();

        Console.Write("Especialidade: ");
        var especialidade = Console.ReadLine();

        Console.Write("Data e Hora (dd/MM/yyyy HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var dataHora))
        {
            Console.WriteLine("Data/hora inválida!");
            return;
        }

        Console.WriteLine("\nTipo de Agendamento:");
        Console.WriteLine("1. Consulta Padrão (30min)");
        Console.WriteLine("2. Primeira Consulta (60min)");
        Console.WriteLine("3. Emergência");
        Console.Write("Escolha: ");

        var tipo = Console.ReadLine();
        Agendamento agendamento;

        switch (tipo)
        {
            case "1":
                agendamento = await agendamentoService.CriarAgendamentoPadraoAsync(clienteId, medico!, especialidade!, dataHora);
                break;
            case "2":
                agendamento = await agendamentoService.CriarPrimeiraConsultaAsync(clienteId, medico!, especialidade!, dataHora);
                break;
            case "3":
                agendamento = await agendamentoService.CriarAgendamentoEmergenciaAsync(clienteId, medico!, especialidade!, dataHora);
                break;
            default:
                Console.WriteLine("Tipo inválido!");
                return;
        }

        Console.WriteLine($"Agendamento criado com ID: {agendamento.Id}");
        Console.WriteLine($"Status: {agendamento.Status}");
    }

    static async Task ListarAgendamentosAsync(AgendamentoService service)
    {
        Console.WriteLine("\n--- LISTA DE AGENDAMENTOS ---");
        var agendamentos = await service.ObterAgendamentosFuturosAsync();

        foreach (var ag in agendamentos)
        {
            Console.WriteLine($"ID: {ag.Id}");
            Console.WriteLine($"Médico: {ag.Medico}");
            Console.WriteLine($"Especialidade: {ag.Especialidade}");
            Console.WriteLine($"Data/Hora: {ag.DataHora:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Duração: {ag.DuracaoMinutos}min");
            Console.WriteLine($"Status: {ag.Status}");
            Console.WriteLine("---");
        }

        Console.WriteLine($"Total: {agendamentos.Count()} agendamentos futuros");
    }

    static async Task ConfirmarAgendamentoAsync(AgendamentoService agendamentoService, INotificacaoService notificacaoService)
    {
        Console.WriteLine("\n--- CONFIRMAR AGENDAMENTO ---");
        Console.Write("ID do Agendamento: ");

        if (!Guid.TryParse(Console.ReadLine(), out var agendamentoId))
        {
            Console.WriteLine("ID inválido!");
            return;
        }

        await agendamentoService.ConfirmarAgendamentoAsync(agendamentoId);

        var agendamento = await agendamentoService.ObterAgendamentoPorIdAsync(agendamentoId);
        if (agendamento != null)
        {
            await notificacaoService.EnviarNotificacaoConfirmacaoAsync(agendamento);
            Console.WriteLine("Agendamento confirmado e notificação enviada!");
        }
    }

    static async Task CancelarAgendamentoAsync(AgendamentoService agendamentoService, INotificacaoService notificacaoService)
    {
        Console.WriteLine("\n--- CANCELAR AGENDAMENTO ---");
        Console.Write("ID do Agendamento: ");

        if (!Guid.TryParse(Console.ReadLine(), out var agendamentoId))
        {
            Console.WriteLine("ID inválido!");
            return;
        }

        Console.Write("Motivo do cancelamento: ");
        var motivo = Console.ReadLine();

        await agendamentoService.CancelarAgendamentoAsync(agendamentoId, motivo!);

        var agendamento = await agendamentoService.ObterAgendamentoPorIdAsync(agendamentoId);
        if (agendamento != null)
        {
            await notificacaoService.EnviarNotificacaoCancelamentoAsync(agendamento, motivo!);
            Console.WriteLine("Agendamento cancelado e notificação enviada!");
        }
    }

    static async Task MostrarEstatisticasAsync(ClienteService clienteService, AgendamentoService agendamentoService)
    {
        Console.WriteLine("\n=== ESTATÍSTICAS ===");

        var totalClientes = await clienteService.ContarClientesAtivosAsync();
        var agendamentosFuturos = await agendamentoService.ObterAgendamentosFuturosAsync();
        var hoje = DateTime.Today;
        var agendamentosHoje = await agendamentoService.ObterAgendamentosDoDiaAsync(hoje);

        Console.WriteLine($"Clientes ativos: {totalClientes}");
        Console.WriteLine($"Agendamentos futuros: {agendamentosFuturos.Count()}");
        Console.WriteLine($"Agendamentos hoje ({hoje:dd/MM/yyyy}): {agendamentosHoje.Count()}");

        // Estatísticas por status
        var todosAgendamentos = await agendamentoService.ObterAgendamentosFuturosAsync();
        var porStatus = todosAgendamentos.GroupBy(a => a.Status);

        foreach (var grupo in porStatus)
        {
            Console.WriteLine($"  {grupo.Key}: {grupo.Count()}");
        }
    }
}