using FarmaciaAPI.Domain.Entities;
using FarmaciaAPI.Domain.Enumerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmaciaAPI.Infra.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.CPF).IsUnique();

            builder.Property(u => u.Nome)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.CPF)
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(u => u.Tipo)
                .HasDefaultValue(EnumTipoUsuario.Usuario)
                .IsRequired();

            builder.Property(u => u.Telefone)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.Senha)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(u => u.ChaveResetSenha)
                .IsRequired(false);

            builder.ToTable("TB_Usuario");
        }
    }
}
