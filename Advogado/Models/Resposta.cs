using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;

namespace Advogado.Models
{
    public class Resposta
    {
        public int RespostaId { get; set; }
        public string RespostaDescritiva { get; set; }

        public int PerguntaId { get; set; }

        public Pergunta Pergunta { get; set; }

        public int CasoId { get; set; }

        [ScriptIgnore]
        public Caso Caso { get; set; }

        public Collection<Alternativa> Alternativas { get; set; }

        public string RespostaFormatada
        {
            get
            {
                string resposta = string.Empty;
                int index = 0;

                if (Alternativas == null || !Alternativas.Any())
                {
                    return RespostaDescritiva;
                }
                else
                {
                    foreach (var alternativa in Alternativas)
                    {
                        if (Alternativas.Count - 1 == index)
                        {
                            resposta += alternativa.Descricao;
                        }
                        else
                        {
                            resposta += alternativa.Descricao + ",";
                        }
                        index++;
                    }
                    return resposta;
                }
            }
        }

        public void AtualizarRespostaDescritiva(string respostaDescritiva)
        {
            RespostaDescritiva = respostaDescritiva;
        }

        #region Não mapeados
        [NotMapped]
        public int AlternativaId { get; set; }

        [NotMapped]
        public int[] AlternativasIds { get; set; }
        #endregion
    }
}