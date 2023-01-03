using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class TipoArquivoOrdemConfig : EntityTypeConfiguration<TipoArquivoOrdem>
    {
        public TipoArquivoOrdemConfig()
        {
            HasKey(x => x.TipoArquivoOrdemId);
            Property(x => x.TipoArquivoOrdemId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TipoArquivoId).IsRequired();
            Property(x => x.Ordem).IsRequired();

            HasRequired(x => x.TipoBeneficio).WithMany(x => x.TipoArquivosOrdens).HasForeignKey(x => x.TipoBeneficioId).WillCascadeOnDelete(false);

            ToTable("TipoArquivoOrdem");
        }
    }
}