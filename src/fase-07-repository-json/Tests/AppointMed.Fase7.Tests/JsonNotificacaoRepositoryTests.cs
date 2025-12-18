using System;
using System.IO;
using System.Linq;
using Xunit;
using Fase07.Domain.Entities;
using Fase07.Infrastructure.Repositories;

namespace Fase07.Tests
{
    public class JsonNotificacaoRepositoryTests
    {
        private string GetTempFilePath()
        {
            return Path.Combine(Path.GetTempPath(), $"test_notificacoes_{Guid.NewGuid()}.json");
        }

        [Fact]
        public void Constructor_CriaDiretorioSeNaoExiste()
        {
            // Arrange
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var filePath = Path.Combine(tempDir, "notificacoes.json");
            
            // Act
            var repo = new JsonNotificacaoRepository(filePath);
            
            // Assert
            Assert.True(Directory.Exists(tempDir));
            
            // Cleanup
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void ListAll_ComArquivoInexistente_RetornaListaVazia()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            
            // Act
            var result = repo.ListAll();
            
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void Add_CriaArquivoEPersisteNotificacao()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            var notificacao = new NotificacaoMedica
            {
                Id = 1,
                Tipo = "atrasado",
                NomePaciente = "Teste",
                TipoConsulta = "Consulta",
                DataConsulta = DateTime.Now,
                Canal = "whatsapp"
            };
            
            // Act
            var added = repo.Add(notificacao);
            
            // Assert
            Assert.NotNull(added);
            Assert.Equal(1, added.Id);
            Assert.True(File.Exists(filePath));
            
            var fromFile = repo.GetById(1);
            Assert.NotNull(fromFile);
            Assert.Equal("Teste", fromFile.NomePaciente);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void GetById_NotificacaoExistente_RetornaNotificacao()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            var notificacao = new NotificacaoMedica
            {
                Id = 99,
                Tipo = "cancelado",
                NomePaciente = "Paciente Teste",
                DataConsulta = DateTime.Now
            };
            repo.Add(notificacao);
            
            // Act
            var result = repo.GetById(99);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(99, result.Id);
            Assert.Equal("Paciente Teste", result.NomePaciente);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void GetById_NotificacaoInexistente_RetornaNull()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            
            // Act
            var result = repo.GetById(999);
            
            // Assert
            Assert.Null(result);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void Update_NotificacaoExistente_RetornaTrueEAtualiza()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            
            var original = new NotificacaoMedica
            {
                Id = 1,
                NomePaciente = "Original",
                Status = "pendente"
            };
            repo.Add(original);
            
            var atualizada = new NotificacaoMedica
            {
                Id = 1,
                NomePaciente = "Atualizado",
                Status = "enviada"
            };
            
            // Act
            var sucesso = repo.Update(atualizada);
            var result = repo.GetById(1);
            
            // Assert
            Assert.True(sucesso);
            Assert.NotNull(result);
            Assert.Equal("Atualizado", result.NomePaciente);
            Assert.Equal("enviada", result.Status);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void Update_NotificacaoInexistente_RetornaFalse()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            var notificacao = new NotificacaoMedica { Id = 999 };
            
            // Act
            var sucesso = repo.Update(notificacao);
            
            // Assert
            Assert.False(sucesso);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void Remove_NotificacaoExistente_RetornaTrueERemove()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            
            var notificacao = new NotificacaoMedica { Id = 1, NomePaciente = "Para Remover" };
            repo.Add(notificacao);
            
            // Act
            var sucesso = repo.Remove(1);
            var result = repo.GetById(1);
            
            // Assert
            Assert.True(sucesso);
            Assert.Null(result);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void Remove_NotificacaoInexistente_RetornaFalse()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            
            // Act
            var sucesso = repo.Remove(999);
            
            // Assert
            Assert.False(sucesso);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void NextId_ListaVazia_Retorna1()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            
            // Act
            var nextId = repo.NextId();
            
            // Assert
            Assert.Equal(1, nextId);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void NextId_ComItens_RetornaMaxIdMais1()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            
            repo.Add(new NotificacaoMedica { Id = 1 });
            repo.Add(new NotificacaoMedica { Id = 5 });
            repo.Add(new NotificacaoMedica { Id = 3 });
            
            // Act
            var nextId = repo.NextId();
            
            // Assert
            Assert.Equal(6, nextId);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void Load_ArquivoVazio_RetornaListaVazia()
        {
            // Arrange
            var filePath = GetTempFilePath();
            File.WriteAllText(filePath, ""); // Arquivo vazio
            var repo = new JsonNotificacaoRepository(filePath);
            
            // Act
            var result = repo.ListAll();
            
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void Load_ArquivoComWhitespace_RetornaListaVazia()
        {
            // Arrange
            var filePath = GetTempFilePath();
            File.WriteAllText(filePath, "   \n\t  "); // Só whitespace
            var repo = new JsonNotificacaoRepository(filePath);
            
            // Act
            var result = repo.ListAll();
            
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void Load_JsonInvalido_RetornaListaVazia()
        {
            // Arrange
            var filePath = GetTempFilePath();
            File.WriteAllText(filePath, "{ isto não é um JSON válido }");
            var repo = new JsonNotificacaoRepository(filePath);
            
            // Act
            var result = repo.ListAll();
            
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void ListByTipo_FiltraNotificacoesCorretamente()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            
            repo.Add(new NotificacaoMedica { Id = 1, Tipo = "atrasado" });
            repo.Add(new NotificacaoMedica { Id = 2, Tipo = "cancelado" });
            repo.Add(new NotificacaoMedica { Id = 3, Tipo = "atrasado" });
            
            // Act
            var atrasados = repo.ListByTipo("atrasado");
            var cancelados = repo.ListByTipo("cancelado");
            var inexistentes = repo.ListByTipo("inexistente");
            
            // Assert
            Assert.Equal(2, atrasados.Count);
            Assert.Equal(1, cancelados.Count);
            Assert.Empty(inexistentes);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }

        [Fact]
        public void ListByStatus_FiltraNotificacoesCorretamente()
        {
            // Arrange
            var filePath = GetTempFilePath();
            var repo = new JsonNotificacaoRepository(filePath);
            
            repo.Add(new NotificacaoMedica { Id = 1, Status = "pendente" });
            repo.Add(new NotificacaoMedica { Id = 2, Status = "enviada" });
            repo.Add(new NotificacaoMedica { Id = 3, Status = "pendente" });
            
            // Act
            var pendentes = repo.ListByStatus("pendente");
            var enviadas = repo.ListByStatus("enviada");
            
            // Assert
            Assert.Equal(2, pendentes.Count);
            Assert.Equal(1, enviadas.Count);
            
            // Cleanup
            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }
}