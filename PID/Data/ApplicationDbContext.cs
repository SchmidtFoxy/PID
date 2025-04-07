using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PID.Models;

namespace PID.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Desenvolvimento> Desenvolvimentos { get; set; }
        public DbSet<ProjetoPD> ProjetosPD { get; set; }
        public DbSet<Dispendio> Dispendios { get; set; }
        public DbSet<Custo> Custos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Historico> Historicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de precisão para o campo Valor na tabela Custo
            modelBuilder.Entity<Custo>()
                .Property(c => c.Valor)
                .HasColumnType("decimal(18,2)");

            // Relacionamento um-para-muitos entre ProjetoPD e Desenvolvimento
            modelBuilder.Entity<ProjetoPD>()
                .HasMany(p => p.Desenvolvimentos)
                .WithOne(d => d.ProjetoPD)
                .HasForeignKey(d => d.ProjetoPDId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração de relacionamento entre ProjetoPD e Dispendio
            modelBuilder.Entity<ProjetoPD>()
                .HasOne(p => p.Dispendio)
                .WithMany()
                .HasForeignKey(p => p.IdDispendio)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração de relacionamento entre Custo e Dispendio
            modelBuilder.Entity<Custo>()
                .HasOne(c => c.Dispendio)
                .WithMany()
                .HasForeignKey(c => c.IdDispendio)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração de relacionamento entre Custo e Desenvolvimento
            modelBuilder.Entity<Custo>()
                .HasOne(c => c.Desenvolvimento)
                .WithMany(d => d.Custos)
                .HasForeignKey(c => c.IdDesenvolvimento)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração de relacionamento entre Tarefa e Desenvolvimento
            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.Desenvolvimento)
                .WithMany()
                .HasForeignKey(t => t.IdDesenvolvimento)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração de relacionamento entre Histórico e Desenvolvimento
            modelBuilder.Entity<Historico>()
                .HasOne(h => h.Desenvolvimento)
                .WithMany()
                .HasForeignKey(h => h.IdDesenvolvimento)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}