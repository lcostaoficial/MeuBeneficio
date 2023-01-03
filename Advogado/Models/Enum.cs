using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Advogado.Models
{
    public enum PassoSalarioMaternidade
    {
        Identificacao,
        CarteiraAssinada,
        PagouInssMeiContribuinteIndividual,
        HouveAborto,
        AbortoOcorreuApos23Semana,
        DataNascimentoObitoCrianca,
        DataFalecimento,
        DesempregadaAposGravidez,
        DataSaidaEmprego,
        RecebeuSeguroDesempregoAntes,
        FoiAoSineAntesDoNascimentoObito
    } 

    public enum TipoTarefa
    {
        Comum = 1,
        EntregaDocumentacao = 2,
        EntregaQuestionario = 3,
        ProtocoloAdministrativo = 4,
        ProtocoloJudicial = 5
    }

    public enum TipoJustificativaTarefa
    {
        AlteracaoPrazo = 1,      
        AlteracaoResponsavel = 2    
    }

    public enum EstadoCivil
    {
        Solteiro = 1,
        Casado = 2,
        Separado = 3,
        Divorciado = 4,
        Viuvo = 5
    }

    public enum Perfil
    {
        Administrador = 1,
        Secretaria = 2       
    }

    public enum SituacaoCaso
    {
        Pendente = 0,
        Aberto = 1,       
        Cancelado = 2,
        Concluido = 3
    }

    public enum TipoMovimento
    {
        Minima = 1,
        Baixa = 2,
        Normal = 3,         
        Urgente = 4,
        Alta = 5
    }

    public enum OrgaoExpedidorRg
    {
        [Display(Name = "Secretaria de Segurança Pública")]
        [Description("Secretaria de Segurança Pública")]
        SSP = 1,

        [Display(Name = "Polícia Civil")]
        [Description("Polícia Civil")]
        PC = 2
    }

    public enum Estado
    {
        AC = 1,
        AL = 2,
        AP = 3,
        AM = 4,
        BA = 5,
        CE = 6,
        DF = 7,
        ES = 8,
        GO = 9,
        MA = 10,
        MT = 11,
        MS = 12,
        MG = 13,
        PA = 14,
        PB = 15,
        PR = 16,
        PE = 17,
        PI = 18,
        RJ = 19,
        RN = 20,
        RS = 21,
        RO = 22,
        RR = 23,
        SC = 24,
        SP = 25,
        SE = 26,
        TO = 27
    }
}