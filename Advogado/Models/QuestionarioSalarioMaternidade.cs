using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Advogado.Models
{
    public class QuestionarioSalarioMaternidade
    {
        public int QuestionarioSalarioMaternidadeId { get; set; }       

        public DateTime DataResposta { get; set; }

        #region Dados Pessoais

        public string Nome { get; set; }

        public string Telefone { get; set; }

        public string Cidade { get; set; }

        public Estado Estado { get; set; }

        #endregion
       
        public bool TrabalhouCarteiraAssinada { get; set; }
        
        public bool PagouInssMeiOuContribuinteIndividual { get; set; }

        public bool HouveAborto { get; set; }    

        public bool AbortoOcorreuApos23Semana { get; set; }    

        public DateTime? DataNascimentoObitoCrianca { get; set; }
        
        public bool DesempregadaAposGravidez { get; set; }
              
        public DateTime? DataSaidaEmprego { get; set; }        
       
        public bool RecebeuSeguroDesempregoAntes { get; set; }

        public bool FoiAoSineAntesDoNascimentoObito { get; set; }

        public bool TemDireito { get; set; }
        
        [NotMapped]
        public PassoSalarioMaternidade PassoSalarioMaternidade { get; set; }

        [NotMapped]
        public string ObjetoSerializado { get; set; }        
    }
}