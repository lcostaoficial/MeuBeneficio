using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class ClienteConfig : EntityTypeConfiguration<Cliente>
    {
        public ClienteConfig()
        {
            HasKey(x => x.ClienteId);
            Property(x => x.ClienteId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).IsRequired().HasMaxLength(255);
            Property(x => x.NomeMae).IsRequired().HasMaxLength(255);

            Property(x => x.Cep).IsRequired().HasMaxLength(10);
            Property(x => x.Rua).IsRequired().HasMaxLength(255);
            Property(x => x.Numero).IsRequired().HasMaxLength(10);
            Property(x => x.Bairro).IsRequired().HasMaxLength(255);
            Property(x => x.Cidade).IsRequired().HasMaxLength(255);
            Property(x => x.Estado).IsRequired();
            Property(x => x.Naturalidade).IsRequired().HasMaxLength(100);
            Property(x => x.Nacionalidade).IsRequired().HasMaxLength(100);
            Property(x => x.DataNascimento).IsRequired();
            Property(x => x.EstadoNascimento).IsRequired();
            Property(x => x.Profissao).IsRequired().HasMaxLength(100);

            Property(x => x.Rg).IsOptional().HasMaxLength(20);
            Property(x => x.OrgaoExpedidorRg).IsOptional();
            Property(x => x.EstadoOrgaoExpedidor).IsOptional();

            Property(x => x.Cpf).IsRequired().HasMaxLength(14); 
            Property(x => x.TelefoneFixo).IsOptional().HasMaxLength(14); 
            Property(x => x.TelefoneMovel).IsRequired().HasMaxLength(15); 
            Property(x => x.Email).IsRequired().HasMaxLength(255);
            Property(x => x.EstadoCivil).IsRequired();

            Property(x => x.ClienteEstrangeiro).IsRequired();
            Property(x => x.DocumentoIdentificacaoEstrangeiro).IsOptional();

            ToTable("Cliente");
        }
    }
}