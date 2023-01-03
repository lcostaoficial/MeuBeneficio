using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace Advogado.Models
{
    public class Tarefa
    {
        public int TarefaId { get; set; }

        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Solução")]
        public string Solucao { get; set; }

        public TipoTarefa TipoTarefa { get; set; }
       
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Data de expiração")]
        public DateTime DataExpiracao { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFinalizacao { get; set; }

        public int? FuncionarioCriadorId { get; set; }
        public Funcionario FuncionarioCriador { get; set; }

        [Display(Name = "Funcionário responsável")]
        public int? FuncionarioResponsavelId { get; set; }
        public Funcionario FuncionarioResponsavel { get; set; }

        public bool Finalizada { get; set; }

        public int CasoId { get; set; }

        [ScriptIgnore]
        public Caso Caso { get; set; }

        public ICollection<JustificativaTarefa> JustificativasTarefas { get; set; }

        public string Responsavel => FuncionarioResponsavel != null ? FuncionarioResponsavel.Nome : string.Empty;

        [NotMapped]
        [Display(Name = "Justificativa")]
        public string JustificativaAlteracaoResponsavel { get; set; }

        [NotMapped]
        [Display(Name = "Justificativa")]
        public string JustificativaDilatacaoPrazo { get; set; }

        [NotMapped]
        [Display(Name = "Novo prazo")]
        public DateTime NovaDataExpiracao { get; set; }

        //public string DataExpiracaoFormatada => DataExpiracao.ToString("yyyy-MM-dd'T'HH:mm:ss");

        public string CorTarefa
        {
            get
            {
                var dataHoje = DateTime.Now;

                if (Finalizada == true)
                {
                    return "bg-success";
                }

                //A data de hoje é igual data de expiração, ou seja, a tarefa expira hoje deve ter atenção.
                if (dataHoje.Date == DataExpiracao.Date)
                {
                    return "bg-warning";
                }

                //A data de hoje é menor do que a de expiração, ou seja, a tarefa está em dias
                if (dataHoje.Date < DataExpiracao.Date)
                {
                    return "bg-primary";
                }

                //A data de hoje é maior do que a de expiração, ou seja,
                if (dataHoje.Date > DataExpiracao.Date)
                {
                    return "bg-danger";
                }
                return "bg-primary";
            }
        }
    }
}