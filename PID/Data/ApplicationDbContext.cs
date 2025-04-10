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
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<HistoricoEdicaoDesenvolvimento> HistoricoEdicoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de precisão para o campo Valor na tabela Custo
            modelBuilder.Entity<Custo>()
                .Property(c => c.Valor)
                .HasColumnType("decimal(18,2)");

            // Relacionamento entre ProjetoPD e Desenvolvimento
            modelBuilder.Entity<ProjetoPD>()
                .HasMany(p => p.Desenvolvimentos)
                .WithOne(d => d.ProjetoPD)
                .HasForeignKey(d => d.ProjetoPDId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProjetoPD -> Dispendio
            modelBuilder.Entity<ProjetoPD>()
                .HasOne(p => p.Dispendio)
                .WithMany()
                .HasForeignKey(p => p.IdDispendio)
                .OnDelete(DeleteBehavior.Restrict);

            // Custo -> Dispendio
            modelBuilder.Entity<Custo>()
                .HasOne(c => c.Dispendio)
                .WithMany()
                .HasForeignKey(c => c.IdDispendio)
                .OnDelete(DeleteBehavior.Restrict);

            // Custo -> Desenvolvimento
            modelBuilder.Entity<Custo>()
                .HasOne(c => c.Desenvolvimento)
                .WithMany(d => d.Custos)
                .HasForeignKey(c => c.IdDesenvolvimento)
                .OnDelete(DeleteBehavior.Restrict);

            // Tarefa -> Desenvolvimento
            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.Desenvolvimento)
                .WithMany()
                .HasForeignKey(t => t.IdDesenvolvimento)
                .OnDelete(DeleteBehavior.Restrict);

            // Histórico -> Desenvolvimento
            modelBuilder.Entity<Historico>()
                .HasOne(h => h.Desenvolvimento)
                .WithMany()
                .HasForeignKey(h => h.IdDesenvolvimento)
                .OnDelete(DeleteBehavior.Restrict);

            // Comentário -> Desenvolvimento
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Desenvolvimento)
                .WithMany(d => d.Comentarios)
                .HasForeignKey(c => c.DesenvolvimentoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Comentário -> Usuario
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            // Histórico de Edição -> Desenvolvimento
            modelBuilder.Entity<HistoricoEdicaoDesenvolvimento>()
                .HasOne(h => h.Desenvolvimento)
                .WithMany(d => d.HistoricoEdicoes)
                .HasForeignKey(h => h.IdDesenvolvimento)
                .OnDelete(DeleteBehavior.Cascade);

            // Histórico de Edição -> Usuario
            modelBuilder.Entity<HistoricoEdicaoDesenvolvimento>()
                .HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
