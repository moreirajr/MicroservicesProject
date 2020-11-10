using Clientes.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clientes.Infra.Data.EF.Config
{
    public class StatusClienteMap : IEntityTypeConfiguration<EStatusCliente>
    {
        public void Configure(EntityTypeBuilder<EStatusCliente> builder)
        {
            builder.ToTable("StatusCliente", ApplicationContextDb.DEFAULT_SCHEMA);

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Nome)
                .IsRequired();

        }
    }
}