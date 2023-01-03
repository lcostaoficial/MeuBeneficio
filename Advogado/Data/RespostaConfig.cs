using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class RespostaConfig : EntityTypeConfiguration<Resposta>
    {
        public RespostaConfig()
        {
            HasKey(x => x.RespostaId);
            Property(x => x.RespostaId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.RespostaDescritiva).IsOptional().HasMaxLength(500);
            HasRequired(x => x.Pergunta).WithMany(x => x.Respostas).HasForeignKey(x => x.PerguntaId).WillCascadeOnDelete(false);
            HasRequired(x => x.Caso).WithMany(x => x.Respostas).HasForeignKey(x => x.CasoId).WillCascadeOnDelete(false);

            ToTable("Resposta");
        }
    }
}