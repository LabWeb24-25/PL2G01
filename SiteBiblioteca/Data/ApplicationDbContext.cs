using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiteBiblioteca.Models;

namespace SiteBiblioteca.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Adicional { get; set; }
        public DbSet<Requisitar> requisicoes { get; set; }
        public DbSet<Livro> livros { get; set; }
        public DbSet<CRUD> CRUDs { get; set; }
        public DbSet<CriarAdministrador> AdministradoresCriados { get; set; }
        public DbSet<Bloqueio> bloqueios { get; set; }
        public DbSet<Autor> autores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bloqueio>()
                .HasKey(b => new { b.userId, b.adminId, b.dataBloqueio });
            modelBuilder.Entity<CriarAdministrador>()
                .HasKey(b => new { b.idCriador, b.idCriado });
            modelBuilder.Entity<Requisitar>()
                .HasKey(b => new { b.livroId, b.leitorId, b.data_requisicao });
        }
    }
}
