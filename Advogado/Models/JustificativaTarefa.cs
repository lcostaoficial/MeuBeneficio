using System;

namespace Advogado.Models
{
    public class JustificativaTarefa
    {
        public int JustificativaTarefaId { get; set; }
        public string Descricao { get; set; }
        public string Movimentacao { get; set; } /* O sistema preencherá este campo de acordo com os critérios */
        public DateTime DataJustificativa { get; set; }   
        public TipoJustificativaTarefa TipoJustificativaTarefa { get; set; }
        
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

        public int TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }
    }
}