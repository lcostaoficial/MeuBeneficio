using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Advogado.Models
{
    public class Caso
    {
        public int CasoId { get; set; }

        public string Protocolo => $"{CasoId}";

        public DateTime DataAbertura { get; set; }

        public SituacaoCaso SituacaoCaso { get; set; }

        [Display(Name = "Tipo de benefício")]
        public int TipoBeneficioId { get; set; }
        public TipoBeneficio TipoBeneficio { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Display(Name = "Representante legal")]
        public int? RepresentanteLegalId { get; set; }
        public Cliente RepresentanteLegal { get; set; }

        public bool Ativo { get; set; }

        public string DataEmissao => $"{DateTime.Now.ToString("M")} de {DateTime.Now.Year}";

        public string RepresentanteLegalNome => RepresentanteLegal != null ? RepresentanteLegal.Nome : "Sem representante";

        public Collection<Arquivo> Arquivos { get; set; }
        public Collection<Resposta> Respostas { get; set; }
        public Collection<Movimentacao> Movimentacoes { get; set; }
        public Collection<GrupoFamiliar> GruposFamiliares { get; set; } 
        
        public Collection<Tarefa> Tarefas { get; set; }

        public int TotalTarefasAtrasadas => Tarefas != null ? Tarefas.Where(x => x.Finalizada == false && x.DataExpiracao < DateTime.Now).Count() : 0;

        public void RemoverTarefasEmDias()
        {
            var tarefasAtrasados = Tarefas.Where(x => x.Finalizada == false && x.DataExpiracao < DateTime.Now);
            Tarefas = new Collection<Tarefa>(tarefasAtrasados.ToList());
        }

        public void AtualizarCaso(Caso model)
        {
            TipoBeneficioId = model.TipoBeneficioId;
            RepresentanteLegalId = model.RepresentanteLegalId;
        }

        public void AtualizarCliente(Caso model)
        {
            Cliente.Atualizar(model.Cliente);           
        }       

        public void InverterAtivo()
        {
            Ativo = !Ativo;
        }

        #region Não mapeados       

        [NotMapped]
        public string Nome { get; set; }

        [NotMapped]
        public string Cpf { get; set; }

        [NotMapped]
        public DateTime DataNascimento { get; set; }

        [NotMapped]
        public string GrauParentesco { get; set; }
        #endregion
    }
}