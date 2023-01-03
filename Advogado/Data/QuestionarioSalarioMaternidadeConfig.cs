using Advogado.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Advogado.Data
{
    public class QuestionarioSalarioMaternidadeConfig : EntityTypeConfiguration<QuestionarioSalarioMaternidade>
    {
        public QuestionarioSalarioMaternidadeConfig()
        {
            HasKey(x => x.QuestionarioSalarioMaternidadeId);
            Property(x => x.QuestionarioSalarioMaternidadeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.DataResposta).IsRequired();
            Property(x => x.Nome).IsRequired().HasMaxLength(255);
            Property(x => x.Telefone).IsRequired().HasMaxLength(15); 
            Property(x => x.Cidade).IsRequired().HasMaxLength(500);
            Property(x => x.Estado).IsRequired();
            Property(x => x.TrabalhouCarteiraAssinada).IsRequired();
            Property(x => x.PagouInssMeiOuContribuinteIndividual).IsRequired();
            Property(x => x.HouveAborto).IsRequired();
            Property(x => x.AbortoOcorreuApos23Semana).IsRequired();           
            Property(x => x.DataNascimentoObitoCrianca).IsOptional();
            Property(x => x.DesempregadaAposGravidez).IsRequired();
            Property(x => x.DataSaidaEmprego).IsOptional();
            Property(x => x.RecebeuSeguroDesempregoAntes).IsRequired();
            Property(x => x.FoiAoSineAntesDoNascimentoObito).IsRequired();
            Property(x => x.TemDireito).IsRequired();            

            ToTable("QuestionarioSalarioMaternidade");
        }
    }
}