using FarmaciaAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmaciaAPI.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.CPF)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.Telefone)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.Senha)
                .HasMaxLength(250)
                .IsRequired();

            builder.ToTable("TB_Usuario");
        }
    }
}
