using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Advogado.Models
{
    public class TipoArquivo
    {
        public int TipoArquivoId { get; set; }

        [Display(Name = "Descrição")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Descricao { get; set; }

        [Display(Name = "Observação")]
        [MinLength(1, ErrorMessage = "Mínimo 1 caractere")]
        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Observacao { get; set; }
     
        public string Modelo { get; set; }

        [Display(Name = "Este documento é obrigatório?")]
        public bool Obrigatorio { get; set; }

        [NotMapped]
        public int Ordem { get; set; }

        public bool Ativo { get; set; }        

        public ICollection<TipoBeneficio> TiposBeneficios { get; set; }     
        public ICollection<Arquivo> Arquivos { get; set; }

        public void InverterAtivo()
        {
            Ativo = !Ativo;
        }

        public void Atualizar(TipoArquivo model)
        {
            Descricao = model.Descricao;
            Observacao = model.Observacao;
            Modelo = model.Modelo;
            Obrigatorio = model.Obrigatorio;
        }

        public void AtualizarOrdem(int ordem)
        {
            Ordem = ordem;
        }

        #region Não mapeados
        [NotMapped]
        public HttpPostedFileBase ModeloArquivo { get; set; }

        [NotMapped]
        public bool MantemMesmoModelo { get; set; } = false;        
        #endregion
    }
}