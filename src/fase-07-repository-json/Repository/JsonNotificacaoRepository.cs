using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fase07.Domain.Entities;
using Fase07.Domain.Interfaces;

namespace Fase07.Infrastructure.Repositories
{
    public sealed class JsonNotificacaoRepository : INotificacaoRepository
    {
        private readonly string _path;
        
        private static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public JsonNotificacaoRepository(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            
            // Garante que o diretório existe
            var directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public NotificacaoMedica Add(NotificacaoMedica notificacao)
        {
            if (notificacao == null)
                throw new ArgumentNullException(nameof(notificacao));

            var list = Load();
            list.RemoveAll(n => n.Id == notificacao.Id);
            list.Add(notificacao);
            Save(list);
            
            return notificacao;
        }

        public NotificacaoMedica? GetById(int id)
        {
            return Load().FirstOrDefault(n => n.Id == id);
        }

        public IReadOnlyList<NotificacaoMedica> ListAll()
        {
            return Load().AsReadOnly();
        }

        public IReadOnlyList<NotificacaoMedica> ListByTipo(string tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo))
                return new List<NotificacaoMedica>().AsReadOnly();

            return Load()
                .Where(n => n.Tipo?.Equals(tipo, StringComparison.OrdinalIgnoreCase) ?? false)
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<NotificacaoMedica> ListByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return new List<NotificacaoMedica>().AsReadOnly();

            return Load()
                .Where(n => n.Status?.Equals(status, StringComparison.OrdinalIgnoreCase) ?? false)
                .ToList()
                .AsReadOnly();
        }

        public bool Update(NotificacaoMedica notificacao)
        {
            if (notificacao == null)
                throw new ArgumentNullException(nameof(notificacao));

            var list = Load();
            var index = list.FindIndex(n => n.Id == notificacao.Id);
            
            if (index < 0)
                return false;

            list[index] = notificacao;
            Save(list);
            return true;
        }

        public bool Remove(int id)
        {
            var list = Load();
            var countRemoved = list.RemoveAll(n => n.Id == id);
            
            if (countRemoved > 0)
            {
                Save(list);
                return true;
            }
            
            return false;
        }

        public int NextId()
        {
            var list = Load();
            if (list.Count == 0)
                return 1;

            return list.Max(n => n.Id) + 1;
        }

        private List<NotificacaoMedica> Load()
        {
            // Se o arquivo não existe, retorna lista vazia
            if (!File.Exists(_path))
                return new List<NotificacaoMedica>();

            try
            {
                var json = File.ReadAllText(_path);
                
                // Se o arquivo está vazio ou só tem whitespace, retorna lista vazia
                if (string.IsNullOrWhiteSpace(json))
                    return new List<NotificacaoMedica>();

                var result = JsonSerializer.Deserialize<List<NotificacaoMedica>>(json, _options);
                return result ?? new List<NotificacaoMedica>();
            }
            catch (JsonException)
            {
                // Se o JSON é inválido, retorna lista vazia
                return new List<NotificacaoMedica>();
            }
        }

        private void Save(List<NotificacaoMedica> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            var json = JsonSerializer.Serialize(list, _options);
            File.WriteAllText(_path, json);
        }
    }
}