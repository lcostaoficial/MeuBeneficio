using System.Collections.ObjectModel;
using System.Web.Script.Serialization;

namespace Advogado.Models
{
    public class Alternativa
    {
        public int AlternativaId { get; set; }
        public string Descricao { get; set; }

        public int PerguntaId { get; set; }
        public Pergunta Pergunta { get; set; }

        public bool Ativo { get; set; }
        
        [ScriptIgnore]
        public Collection<Resposta> Alternativas { get; set; }
    }
}