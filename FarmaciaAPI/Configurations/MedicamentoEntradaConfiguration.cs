﻿using FarmaciaAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmaciaAPI.Configurations
{
    public class MedicamentoEntradaConfiguration : IEntityTypeConfiguration<MedicamentoEntrada>
    {
        public void Configure(EntityTypeBuilder<MedicamentoEntrada> builder)
        {
            builder.HasKey(me => me.Id);

            builder.Property(me => me.Data)
                .IsRequired();

            builder.Property(me => me.Quantidade)
                .IsRequired();

            builder.ToTable("TB_MedicamentoEntrada");
        }
    }
}
