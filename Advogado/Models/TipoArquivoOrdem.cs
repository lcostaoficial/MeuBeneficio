namespace Advogado.Models
{
    public class TipoArquivoOrdem
    {
        public int TipoArquivoOrdemId { get; set; }

        public int TipoBeneficioId { get; set; }
        public TipoBeneficio TipoBeneficio { get; set; }

        public int TipoArquivoId { get; set; }

        public int Ordem { get; set; }
    }
}