using iscas_lune.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iscas_lune.Api.Domain.EntityConfiguration;

public class ProdutosMaisVendidosConfiguration : IEntityTypeConfiguration<ProdutosMaisVendidos>
{
    public void Configure(EntityTypeBuilder<ProdutosMaisVendidos> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DataCriacao)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.DataAtualizacao)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("now()");
        builder.Property(x => x.Numero)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.ProdutoId)
            .IsRequired();
        builder.Property(x => x.Quantidade)
            .IsRequired()
            .HasPrecision(12, 2);
    }
}
