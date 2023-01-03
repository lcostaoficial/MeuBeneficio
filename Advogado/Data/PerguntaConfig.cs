using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class PerguntaConfig : EntityTypeConfiguration<Pergunta>
    {
        public PerguntaConfig()
        {
            HasKey(x => x.PerguntaId);
            Property(x => x.PerguntaId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Enunciado).IsRequired().HasMaxLength(500);
            Property(x => x.MultiplaAlternativa).IsRequired();
            Property(x => x.MultiplaResposta).IsRequired();
            Property(x => x.Obrigatoria).IsRequired();
            Property(x => x.CaixaSelecao).IsRequired();            
            Property(x => x.Ativo).IsRequired();
            
            ToTable("Pergunta");
        }
    }
}