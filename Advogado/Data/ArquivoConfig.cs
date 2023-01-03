using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class ArquivoConfig : EntityTypeConfiguration<Arquivo>
    {
        public ArquivoConfig()
        {
            HasKey(x => x.ArquivoId);
            Property(x => x.ArquivoId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nome).IsRequired().HasMaxLength(500);
            Property(x => x.Caminho).IsRequired().HasMaxLength(500);
            HasRequired(x => x.TipoArquivo).WithMany(x => x.Arquivos).HasForeignKey(x => x.TipoArquivoId).WillCascadeOnDelete(false);
            HasRequired(x => x.Caso).WithMany(x => x.Arquivos).HasForeignKey(x => x.CasoId).WillCascadeOnDelete(false);

            ToTable("Arquivo");
        }
    }
}