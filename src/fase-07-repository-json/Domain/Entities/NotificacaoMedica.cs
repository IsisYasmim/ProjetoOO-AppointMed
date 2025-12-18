using System;
using System.Text.Json.Serialization;

namespace Fase07.Domain.Entities
{
    public class NotificacaoMedica
    {
        public int Id { get; set; }
        
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } // "atrasado", "cancelado", "primeiraConsulta", "retorno"
        
        [JsonPropertyName("nomePaciente")]
        public string NomePaciente { get; set; }
        
        [JsonPropertyName("tipoConsulta")]
        public string TipoConsulta { get; set; }
        
        [JsonPropertyName("dataConsulta")]
        public DateTime DataConsulta { get; set; }
        
        [JsonPropertyName("dataNotificacao")]
        public DateTime DataNotificacao { get; set; }
        
        [JsonPropertyName("status")]
        public string Status { get; set; } // "pendente", "enviada", "lida", "erro"
        
        [JsonPropertyName("mensagem")]
        public string Mensagem { get; set; }
        
        [JsonPropertyName("canal")]
        public string Canal { get; set; } // "email", "whatsapp", "sms", "app"
        
        public NotificacaoMedica()
        {
            DataNotificacao = DateTime.Now;
            Status = "pendente";
        }
        
        public NotificacaoMedica(
            int id, 
            string tipo, 
            string nomePaciente, 
            string tipoConsulta, 
            DateTime dataConsulta, 
            string canal) : this()
        {
            Id = id;
            Tipo = tipo;
            NomePaciente = nomePaciente;
            TipoConsulta = tipoConsulta;
            DataConsulta = dataConsulta;
            Canal = canal;
        }
    }
}