using Microsoft.EntityFrameworkCore;
using EcoWork.Api.Models;

namespace EcoWork.Api.Persistence
{
    public class EcoWorkDbContext : DbContext
    {
        public EcoWorkDbContext(DbContextOptions<EcoWorkDbContext> options)
            : base(options)
        {
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Dica> Dicas { get; set; }
        public DbSet<MetaSustentavel> Metas { get; set; }
        public DbSet<MetaSustentavel> MetasSustentaveis => Set<MetaSustentavel>();
        public DbSet<Departamento> Departamentos => Set<Departamento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}