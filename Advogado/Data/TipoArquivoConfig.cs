using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class TipoArquivoConfig : EntityTypeConfiguration<TipoArquivo>
    {
        public TipoArquivoConfig()
        {
            HasKey(x => x.TipoArquivoId);
            Property(x => x.TipoArquivoId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Descricao).IsRequired().HasMaxLength(255);
            Property(x => x.Observacao).IsRequired().HasMaxLength(255);
            Property(x => x.Modelo).IsRequired().HasMaxLength(500);
            Property(x => x.Obrigatorio).IsRequired();            
            Property(x => x.Ativo).IsRequired();            

            ToTable("TipoArquivo");
        }
    }
}