using Microsoft.EntityFrameworkCore;
using FarmaciaAPI.Domain;

namespace FarmaciaAPI.Data
{
    public class FarmaciaContext : DbContext
    {
        public DbSet<Categoria> CategoriaSet { get; set; }
        public DbSet<Usuario> UsuarioSet { get; set; }
        public DbSet<Medicamento> MedicamentoSet { get; set; }
        public DbSet<Reserva> ReservaSet { get; set; }
        public DbSet<MedicamentoEntrada> MedicamentoEntradaSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string conexao = "server=mysql.tccnapratica.com.br;database=tccnapratica10;port=3306;uid=tccnapratica10;password=Etec3h3";
            optionsBuilder.UseMySql(conexao, ServerVersion.AutoDetect(conexao));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FarmaciaContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
