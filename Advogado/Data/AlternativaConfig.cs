using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class AlternativaConfig : EntityTypeConfiguration<Alternativa>
    {
        public AlternativaConfig()
        {
            HasKey(x => x.AlternativaId);
            Property(x => x.AlternativaId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Descricao).IsRequired().HasMaxLength(500);
            Property(x => x.Ativo).IsRequired();
            HasRequired(x => x.Pergunta).WithMany(x => x.Alternativas).HasForeignKey(x => x.PerguntaId).WillCascadeOnDelete(false);

            ToTable("Alternativa");
        }
    }
}