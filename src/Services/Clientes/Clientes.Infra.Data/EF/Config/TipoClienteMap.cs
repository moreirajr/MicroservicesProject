using Clientes.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clientes.Infra.Data.EF.Config
{
    public class TipoClienteMap : IEntityTypeConfiguration<ETipoCliente>
    {
        public void Configure(EntityTypeBuilder<ETipoCliente> builder)
        {
            builder.ToTable("TipoCliente", ApplicationContextDb.DEFAULT_SCHEMA);

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Nome)
                .HasColumnName("Descricao")
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}