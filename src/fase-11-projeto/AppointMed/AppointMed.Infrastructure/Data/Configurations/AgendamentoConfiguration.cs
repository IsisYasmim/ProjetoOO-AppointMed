using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Core.Enums;

namespace AppointMed.AppointMed.Infrastructure.Data.Configurations
{
    public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.ClienteId)
                .IsRequired();

            builder.Property(a => a.Medico)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Especialidade)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.DataHora)
                .IsRequired();

            builder.Property(a => a.DuracaoMinutos)
                .IsRequired()
                .HasDefaultValue(30);

            builder.Property(a => a.Observacoes)
                .HasMaxLength(500);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (StatusAgendamento)Enum.Parse(typeof(StatusAgendamento), v))
                .HasDefaultValue(StatusAgendamento.Agendado);

            builder.Property(a => a.DataCriacao)
                .IsRequired();

            builder.Property(a => a.MotivoCancelamento)
                .HasMaxLength(200);
        }
    }
}
