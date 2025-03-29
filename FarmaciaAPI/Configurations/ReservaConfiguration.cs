using FarmaciaAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmaciaAPI.Configurations
{
    public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.DataReserva)
                .IsRequired();

            builder.Property(r => r.ImagemReceita)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(r => r.EnumTipoAtendimento)
                .IsRequired();

            builder.Property(r => r.Quantidade)
                .IsRequired(false);

            builder.Property(r => r.RetiranteNome)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(r => r.RetiranteCpf)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.ToTable("TB_Reserva");
        }
    }
}
