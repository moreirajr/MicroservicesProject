using Clientes.Domain.Pessoas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clientes.Infra.Data.EF.Config
{
    public class PessoaMap : ABaseMap<Pessoa>
    {
        public override void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            base.Configure(builder);
            builder.ToTable("Pessoa", ApplicationContextDb.DEFAULT_SCHEMA);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(p => p.CPF)
                .IsRequired()
                .HasMaxLength(12);

            builder.Property(p => p.DataNascimento)
                .IsRequired()
                .HasColumnType("Date");

            builder.HasMany(p => p.Enderecos)
                .WithOne(e => e.Pessoa)
                .HasForeignKey(e => e.PessoaId);

            builder.HasMany(p => p.Telefones)
                .WithOne(t => t.Pessoa)
                .HasForeignKey(t => t.PessoaId);
        }
    }
}
