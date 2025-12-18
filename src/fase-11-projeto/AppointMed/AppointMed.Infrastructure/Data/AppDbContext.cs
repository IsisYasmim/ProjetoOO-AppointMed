using Microsoft.EntityFrameworkCore;
using AppointMed.AppointMed.Core.Entities;
using AppointMed.AppointMed.Infrastructure.Data.Configurations;

namespace AppointMed.AppointMed.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar configurações
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new AgendamentoConfiguration());

            // Configurações adicionais
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CPF)
                .IsUnique();

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Email.Valor)
                .IsUnique();

            modelBuilder.Entity<Agendamento>()
                .HasIndex(a => new { a.Medico, a.DataHora });

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Agendamentos)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Auditoria automática
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Cliente || e.Entity is Agendamento);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    // Data de criação
                    if (entityEntry.Entity is Cliente cliente)
                    {
                        cliente.DataCadastro = DateTime.UtcNow;
                    }
                    else if (entityEntry.Entity is Agendamento agendamento)
                    {
                        agendamento.DataCriacao = DateTime.UtcNow;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
