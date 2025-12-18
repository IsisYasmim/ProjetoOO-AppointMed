using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.Interfaces;

namespace AppointMed.AppointMed.Services
{
    
    /// Serviço para envio de notificações sobre agendamentos
    
    public interface INotificacaoService
    {
        Task EnviarNotificacaoConfirmacaoAsync(Agendamento agendamento);
        Task EnviarNotificacaoLembreteAsync(Agendamento agendamento);
        Task EnviarNotificacaoCancelamentoAsync(Agendamento agendamento, string motivo);
    }

    
    /// Factory para diferentes tipos de notificadores
    
    public interface INotificadorFactory
    {
        INotificador CriarNotificadorEmail();
        INotificador CriarNotificadorSMS();
        INotificador CriarNotificadorWhatsApp();
    }

    public interface INotificador
    {
        Task EnviarAsync(string destinatario, string mensagem);
    }

    public class NotificacaoService : INotificacaoService
    {
        private readonly INotificadorFactory _factory;
        private readonly IRepository<Cliente> _clienteRepository;

        public NotificacaoService(INotificadorFactory factory, IRepository<Cliente> clienteRepository)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
        }

        public async Task EnviarNotificacaoConfirmacaoAsync(Agendamento agendamento)
        {
            var cliente = await _clienteRepository.GetByIdAsync(agendamento.ClienteId);
            if (cliente == null) return;

            var mensagem = $"Olá {cliente.Nome}, seu agendamento com Dr(a). {agendamento.Medico} " +
                          $"para {agendamento.DataHora:dd/MM/yyyy HH:mm} foi confirmado.";

            // Envia por email como padrão
            var notificador = _factory.CriarNotificadorEmail();
            await notificador.EnviarAsync(cliente.Email.Valor, mensagem);
        }

        public async Task EnviarNotificacaoLembreteAsync(Agendamento agendamento)
        {
            var cliente = await _clienteRepository.GetByIdAsync(agendamento.ClienteId);
            if (cliente == null) return;

            var mensagem = $"Lembrete: você tem consulta com Dr(a). {agendamento.Medico} " +
                          $"amanhã às {agendamento.DataHora:HH:mm}. Chegar 15 minutos antes.";

            // Tenta WhatsApp primeiro, depois SMS
            INotificador notificador;

            if (!string.IsNullOrEmpty(cliente.Telefone) && cliente.Telefone.Length >= 10)
            {
                try
                {
                    notificador = _factory.CriarNotificadorWhatsApp();
                    await notificador.EnviarAsync(cliente.Telefone, mensagem);
                    return;
                }
                catch
                {
                    // Fallback para SMS
                }
            }

            notificador = _factory.CriarNotificadorSMS();
            await notificador.EnviarAsync(cliente.Telefone, mensagem);
        }

        public async Task EnviarNotificacaoCancelamentoAsync(Agendamento agendamento, string motivo)
        {
            var cliente = await _clienteRepository.GetByIdAsync(agendamento.ClienteId);
            if (cliente == null) return;

            var mensagem = $"Olá {cliente.Nome}, seu agendamento com Dr(a). {agendamento.Medico} " +
                          $"para {agendamento.DataHora:dd/MM/yyyy HH:mm} foi cancelado. " +
                          $"Motivo: {motivo}";

            // Envia por email e SMS
            var emailNotificador = _factory.CriarNotificadorEmail();
            var smsNotificador = _factory.CriarNotificadorSMS();

            await Task.WhenAll(
                emailNotificador.EnviarAsync(cliente.Email.Valor, mensagem),
                smsNotificador.EnviarAsync(cliente.Telefone, mensagem)
            );
        }
    }

    // Implementações dos notificadores
    public class EmailNotificador : INotificador
    {
        public async Task EnviarAsync(string destinatario, string mensagem)
        {
            // Implementação real usaria SMTP, SendGrid, etc.
            Console.WriteLine($"EMAIL para {destinatario}: {mensagem}");
            await Task.CompletedTask;
        }
    }

    public class SMSNotificador : INotificador
    {
        public async Task EnviarAsync(string destinatario, string mensagem)
        {
            // Implementação real usaria API de SMS
            Console.WriteLine($"SMS para {destinatario}: {mensagem}");
            await Task.CompletedTask;
        }
    }

    public class WhatsAppNotificador : INotificador
    {
        public async Task EnviarAsync(string destinatario, string mensagem)
        {
            // Implementação real usaria API do WhatsApp
            Console.WriteLine($"WHATSAPP para {destinatario}: {mensagem}");
            await Task.CompletedTask;
        }
    }

    public class NotificadorFactory : INotificadorFactory
    {
        public INotificador CriarNotificadorEmail()
        {
            return new EmailNotificador();
        }

        public INotificador CriarNotificadorSMS()
        {
            return new SMSNotificador();
        }

        public INotificador CriarNotificadorWhatsApp()
        {
            return new WhatsAppNotificador();
        }
    }
}
