using FarmaciaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmaciaAPI.Infra.Data.Configurations
{
    public class MedicamentoConfiguration : IEntityTypeConfiguration<Medicamento>
    {
        public void Configure(EntityTypeBuilder<Medicamento> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.NomeComercial)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.NomeQuimico)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.TipoMedicamento)
                .IsRequired();

            builder.Property(m => m.Descricao)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(m => m.Quantidade)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(m => m.UnidadeMedida)
                .IsRequired();

            builder.ToTable("TB_Medicamento");
        }
    }
}
