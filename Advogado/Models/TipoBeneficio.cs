using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;

namespace Advogado.Models
{
    public class TipoBeneficio
    {
        public int TipoBeneficioId { get; set; }

        [Display(Name = "Descrição")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Descricao { get; set; }


        [Display(Name = "Remuneração")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Remuneracao { get; set; }

        [Display(Name = "Este tipo de benefício necessita da composição do grupo familiar?")]
        public bool HabilitarGrupoFamiliar { get; set; }

        public bool Ativo { get; set; }

        public void Atualizar(TipoBeneficio model)
        {
            Descricao = model.Descricao;
        }

        public void InverterAtivo()
        {
            Ativo = !Ativo;
        }

        public void SetPerguntasIds()
        {
            if (Perguntas != null && Perguntas.Any()) PerguntasIds = Perguntas.Select(x => x.PerguntaId).ToArray();
        }

        public void SetTiposArquivosIds()
        {
            if (TiposArquivos != null && TiposArquivos.Any()) TiposArquivosIds = TiposArquivos.Select(x => x.TipoArquivoId).ToArray();
        } 

        public ICollection<TipoArquivo> TiposArquivos { get; set; }
        public ICollection<Pergunta> Perguntas { get; set; }
        public ICollection<PerguntaOrdem> PerguntaOrdens { get; set; }
        public ICollection<TipoArquivoOrdem> TipoArquivosOrdens { get; set; }

        [ScriptIgnore]
        public ICollection<Caso> Casos { get; set; }

        #region Não mapeados
        [NotMapped]
        [Display(Name = "Tipos de Documentos")]
        public int[] TiposArquivosIds { get; set; }

        [Display(Name = "Perguntas")]
        public int[] PerguntasIds { get; set; }        
        #endregion
    }
}