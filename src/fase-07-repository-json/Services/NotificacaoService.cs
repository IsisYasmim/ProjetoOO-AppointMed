using System;
using System.Collections.Generic;
using Fase07.Domain.Entities;
using Fase07.Domain.Interfaces;

namespace Fase07.Services
{
    public class NotificacaoService
    {
        private readonly INotificacaoRepository _repository;
        private readonly INotificaStatus _statusService;
        private readonly INotificaTipoConsulta _tipoService;

        public NotificacaoService(
            INotificacaoRepository repository,
            INotificaStatus statusService,
            INotificaTipoConsulta tipoService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _statusService = statusService ?? throw new ArgumentNullException(nameof(statusService));
            _tipoService = tipoService ?? throw new ArgumentNullException(nameof(tipoService));
        }

        public NotificacaoMedica CriarNotificacaoAtrasado(
            string nomePaciente, 
            string tipoConsulta, 
            DateTime dataConsulta,
            string canal = "whatsapp")
        {
            int diasAtraso = CalcularDiasAtraso(dataConsulta);
            var mensagem = _statusService.NotificarAtrasado(nomePaciente, tipoConsulta, dataConsulta, diasAtraso);
            
            var notificacao = new NotificacaoMedica
            {
                Id = _repository.NextId(),
                Tipo = "atrasado",
                NomePaciente = nomePaciente,
                TipoConsulta = tipoConsulta,
                DataConsulta = dataConsulta,
                Canal = canal,
                Mensagem = mensagem,
                Status = "pendente"
            };

            return _repository.Add(notificacao);
        }

        public NotificacaoMedica CriarNotificacaoCancelado(
            string nomePaciente, 
            string tipoConsulta, 
            DateTime dataConsulta,
            string canal = "email")
        {
            var mensagem = _statusService.NotificarCancelado(nomePaciente, tipoConsulta, dataConsulta);
            
            var notificacao = new NotificacaoMedica
            {
                Id = _repository.NextId(),
                Tipo = "cancelado",
                NomePaciente = nomePaciente,
                TipoConsulta = tipoConsulta,
                DataConsulta = dataConsulta,
                Canal = canal,
                Mensagem = mensagem,
                Status = "pendente"
            };

            return _repository.Add(notificacao);
        }

        public NotificacaoMedica CriarNotificacaoPrimeiraConsulta(
            string nomePaciente, 
            DateTime dataConsulta,
            string canal = "app")
        {
            var mensagem = _tipoService.NotificarPrimeiraConsulta(nomePaciente, dataConsulta);
            
            var notificacao = new NotificacaoMedica
            {
                Id = _repository.NextId(),
                Tipo = "primeiraConsulta",
                NomePaciente = nomePaciente,
                TipoConsulta = "Primeira Consulta",
                DataConsulta = dataConsulta,
                Canal = canal,
                Mensagem = mensagem,
                Status = "pendente"
            };

            return _repository.Add(notificacao);
        }

        public NotificacaoMedica CriarNotificacaoRetorno(
            string nomePaciente, 
            DateTime dataConsulta,
            string canal = "sms")
        {
            var mensagem = _tipoService.NotificarRetorno(nomePaciente, dataConsulta);
            
            var notificacao = new NotificacaoMedica
            {
                Id = _repository.NextId(),
                Tipo = "retorno",
                NomePaciente = nomePaciente,
                TipoConsulta = "Retorno",
                DataConsulta = dataConsulta,
                Canal = canal,
                Mensagem = mensagem,
                Status = "pendente"
            };

            return _repository.Add(notificacao);
        }

        public bool MarcarComoEnviada(int id)
        {
            var notificacao = _repository.GetById(id);
            if (notificacao == null)
                return false;

            notificacao.Status = "enviada";
            return _repository.Update(notificacao);
        }

        public bool MarcarComoLida(int id)
        {
            var notificacao = _repository.GetById(id);
            if (notificacao == null)
                return false;

            notificacao.Status = "lida";
            return _repository.Update(notificacao);
        }

        public IReadOnlyList<NotificacaoMedica> ListarPendentes()
        {
            return _repository.ListByStatus("pendente");
        }

        public IReadOnlyList<NotificacaoMedica> ListarPorTipo(string tipo)
        {
            return _repository.ListByTipo(tipo);
        }

        private int CalcularDiasAtraso(DateTime dataConsulta)
        {
            var dias = (DateTime.Now.Date - dataConsulta.Date).Days;
            return dias > 0 ? dias : 0;
        }
    }
}