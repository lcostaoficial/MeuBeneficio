using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace Advogado.Models
{
    public class Pergunta
    {
        public int PerguntaId { get; set; }        

        public string Enunciado { get; set; }

        [Display(Name = "Muitas alternativas?")]
        public bool MultiplaAlternativa { get; set; }

        [Display(Name = "Muitas respostas?")]
        public bool MultiplaResposta { get; set; }

        [Display(Name = "Reposta obrigatória?")]
        public bool Obrigatoria { get; set; }

        [Display(Name = "Habilitar caixa de seleção?")]
        public bool CaixaSelecao { get; set; }

        public bool Ativo { get; set; }       

        [ScriptIgnore]
        public ICollection<TipoBeneficio> TiposBeneficios { get; set; }

        [ScriptIgnore]
        public ICollection<Alternativa> Alternativas { get; set; }

        [ScriptIgnore]
        public ICollection<Resposta> Respostas { get; set; }
        
        [NotMapped]
        public int Ordem { get; set; }

        public void Atualizar(Pergunta pergunta)
        {
            Enunciado = pergunta.Enunciado;
            MultiplaAlternativa = pergunta.MultiplaAlternativa;
            MultiplaResposta = pergunta.MultiplaResposta;
            CaixaSelecao = pergunta.CaixaSelecao;
            Ativo = pergunta.Ativo;
            Obrigatoria = pergunta.Obrigatoria;            
        }        

        public void InverterAtivo()
        {
            Ativo = !Ativo;
        }

        #region Não mapeados
        [NotMapped]
        public Resposta RespostaCache { get; set; }

        [NotMapped]
        public string Descricao { get; set; }
        #endregion
    }
}