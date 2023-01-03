namespace Advogado.Models
{
    public class PerguntaOrdem
    {
        public int PerguntaOrdemId { get; set; }

        public int TipoBeneficioId { get; set; }
        public TipoBeneficio TipoBeneficio { get; set; }

        public int PerguntaId { get; set; } 

        public int Ordem { get; set; }
    }
}