using Clientes.Domain.Pessoas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clientes.Infra.Data.EF.Config
{
    public class TelefoneMap : IEntityTypeConfiguration<Telefone>
    {
        public void Configure(EntityTypeBuilder<Telefone> builder)
        {
            builder.ToTable("Telefone", ApplicationContextDb.DEFAULT_SCHEMA);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.DDD)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(p => p.DDI)
                 .IsRequired()
                 .HasMaxLength(3);

            builder.Property(p => p.Numero)
                .IsRequired()
                .HasMaxLength(9);
        }
    }
}