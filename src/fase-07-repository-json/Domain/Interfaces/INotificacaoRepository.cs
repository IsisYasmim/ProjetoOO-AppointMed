using System.Collections.Generic;
using Fase07.Domain.Entities;

namespace Fase07.Domain.Interfaces
{
    public interface INotificacaoRepository
    {
        NotificacaoMedica Add(NotificacaoMedica notificacao);
        NotificacaoMedica? GetById(int id);
        IReadOnlyList<NotificacaoMedica> ListAll();
        IReadOnlyList<NotificacaoMedica> ListByTipo(string tipo);
        IReadOnlyList<NotificacaoMedica> ListByStatus(string status);
        bool Update(NotificacaoMedica notificacao);
        bool Remove(int id);
        int NextId();
    }
}