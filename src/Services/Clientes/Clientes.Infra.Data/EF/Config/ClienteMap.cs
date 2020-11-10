using Clientes.Domain.Clientes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clientes.Infra.Data.EF.Config
{
    public class ClienteMap : ABaseMap<Cliente>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            base.Configure(builder);

            builder.ToTable("Cliente", ApplicationContextDb.DEFAULT_SCHEMA);

            builder.HasKey(c => c.Id);

            builder.Property(c => c.DataCadastro)
                .IsRequired()
                .HasDefaultValueSql("getdate()");

            builder.Ignore(c => c.Status);

            builder.HasOne(c => c.Pessoa)
                .WithOne(p => p.Cliente)
                .HasForeignKey<Cliente>(c => c.PessoaId);

            builder.HasOne(c => c.TipoCliente)
                .WithMany()
                .HasForeignKey(c => c.TipoClienteId);

            builder.HasOne(c => c.Status)
                .WithMany()
                .HasForeignKey(c => c.StatusId);
        }
    }
}
