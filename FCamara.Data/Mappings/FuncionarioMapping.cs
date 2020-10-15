using FCamara.Bussiness.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCamara.Data.Mappings
{
    public class FuncionarioMapping : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.CPF)
                .IsRequired()
                .HasColumnType("varchar(14)");

            // 1 : 1 => Funcionario : Endereco
            builder.HasOne(f => f.Endereco)
                .WithOne(e => e.Funcionario);

            // 1 : N => Funcionario : Dependetes
            builder.HasMany(f => f.Dependentes)
                .WithOne(p => p.Funcionario)
                .HasForeignKey(p => p.FuncionarioId);

            builder.ToTable("Funcionarios");
        }
    }
}
