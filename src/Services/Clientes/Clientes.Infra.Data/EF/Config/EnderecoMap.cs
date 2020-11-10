using Clientes.Domain.Pessoas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clientes.Infra.Data.EF.Config
{
    public class EnderecoMap : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("Endereco", ApplicationContextDb.DEFAULT_SCHEMA);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.CEP)
                .IsRequired()
                .HasMaxLength(9);

            builder.Property(e => e.Complemento)
                .IsRequired(false)
                .HasMaxLength(40);

            builder.Property(e => e.EnderecoPadrao)
               .IsRequired()
               .HasDefaultValue(false);

            builder.Property(e => e.Logradouro)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(e => e.Numero)
             .IsRequired()
             .HasMaxLength(10);
        }
    }
}