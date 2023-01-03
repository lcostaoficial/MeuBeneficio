namespace Advogado.Models
{
    public class Arquivo
    {
        public int ArquivoId { get; set; }
        public string Nome { get; set; }
        public string Caminho { get; set; }

        public int TipoArquivoId { get; set; }
        public TipoArquivo TipoArquivo { get; set; }

        public int CasoId { get; set; }
        public Caso Caso { get; set; }    
        
        public void AtualizarDocumento(Arquivo model)
        {
            Nome = model.Nome;
            Caminho = model.Caminho;
        }
    }
}