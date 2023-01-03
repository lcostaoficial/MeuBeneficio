using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class PerguntaOrdemConfig : EntityTypeConfiguration<PerguntaOrdem>
    {
        public PerguntaOrdemConfig()
        {
            HasKey(x => x.PerguntaOrdemId);
            Property(x => x.PerguntaOrdemId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PerguntaId).IsRequired();
            Property(x => x.Ordem).IsRequired();
            
            HasRequired(x => x.TipoBeneficio).WithMany(x => x.PerguntaOrdens).HasForeignKey(x => x.TipoBeneficioId).WillCascadeOnDelete(false);

            ToTable("PerguntaOrdem");
        }
    }
}