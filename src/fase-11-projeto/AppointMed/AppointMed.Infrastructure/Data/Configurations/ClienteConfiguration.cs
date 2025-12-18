using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AppointMed.AppointMed.Core.Entities;

namespace AppointMed.AppointMed.Infrastructure.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.OwnsOne(c => c.Email, e =>
            {
                e.Property(email => email.Valor)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(100);
            });

            builder.Property(c => c.Telefone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.CPF)
                .IsRequired()
                .HasMaxLength(11)
                .IsFixedLength();

            builder.Property(c => c.DataNascimento)
                .IsRequired();

            builder.Property(c => c.DataCadastro)
                .IsRequired();

            builder.Property(c => c.Ativo)
                .IsRequired();
        }
    }
}
