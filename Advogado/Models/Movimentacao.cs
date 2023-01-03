using System;
using System.ComponentModel.DataAnnotations;

namespace Advogado.Models
{
    public class Movimentacao
    {
        public int MovimentacaoId { get; set; }
        public DateTime Data { get; set; }

        [Display(Name = "Detalhes")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Descricao { get; set; }       
        
        public string NomeAnexo { get; set; }
        public string CaminhoAnexo { get; set; }

        [Display(Name = "Prioridade de acompanhamento")]        
        [Required(ErrorMessage = "Campo obrigatório")]
        public TipoMovimento TipoMovimento { get; set; }

        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

        public int CasoId { get; set; }
        public Caso Caso { get; set; }        

        public void AtualizarMovimentacao(Movimentacao movimentacao)
        {            
            Descricao = movimentacao.Descricao;           
            TipoMovimento = movimentacao.TipoMovimento;
        }
    }
}