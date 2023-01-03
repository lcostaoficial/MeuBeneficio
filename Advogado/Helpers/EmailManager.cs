using Advogado.Models;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Advogado.Helpers
{
    public class EmailManager
    {
        public string Assunto { get; set; }
        public string Corpo { get; set; }
        public ArrayList Destinatarios { get; set; }       

        public EmailManager(ArrayList destinatarios)
        {
            Destinatarios = destinatarios;
        }

        public async Task<bool> Enviar()
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("envio@sistemameubeneficio.com", "Sistema de Questionários"),
                Priority = MailPriority.Normal,
                IsBodyHtml = true,
                Subject = Assunto,
                Body = Corpo,
                SubjectEncoding = Encoding.GetEncoding("ISO-8859-1"),
                BodyEncoding = Encoding.GetEncoding("ISO-8859-1")
            };

            foreach (var t in Destinatarios) mailMessage.To.Add(t.ToString());

            var smtp = new SmtpClient
            {
                UseDefaultCredentials = false,
                Host = "mail.sistemameubeneficio.com",
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("envio@sistemameubeneficio.com", "Lucas@1995"),
                Port = 587
            };

            await smtp.SendMailAsync(mailMessage);

            return true;
        }       

        public void SalarioMaternidade(QuestionarioSalarioMaternidade model)
        {
            Assunto = $"Cliente: {model.Nome} possívelmente pode receber o Salário Maternidade";
            Destinatarios = new ArrayList { "jose.lucas@infouniron.com", "marcosdasilva.ro@gmail.com", "elbervm@gmail.com" };
            Corpo = $@"
                    <section style='width: 400px; padding: 1em;'>
                        <h1 style='text-align: center; text-transform: uppercase;margin: 0'>Salário Maternidade<br /><small>Possível cliente com direito ao benefício</small></h1>
                        <hr />
                        <p><b>Data de Resposta:</b> {model.DataResposta}</p>
                        <p><b>Nome:</b> {model.Nome}</p>
                        <p><b>Telefone:</b> {model.Telefone}</p>
                        <p><b>Cidade:</b> {model.Cidade}</p>
                        <p><b>Estado:</b> {model.Estado.ToString()}</p>
                        <p><b>Trabalhou de Carteira Assinada:</b> {(model.TrabalhouCarteiraAssinada ? "Sim" : "Não")}</p>
                        <p><b>Pagou INSS como MEI ou Contribuinte Individual:</b> {(model.PagouInssMeiOuContribuinteIndividual ? "Sim" : "Não")}</p>
                        <p><b>Houve Aborto:</b> {(model.HouveAborto ? "Sim" : "Não")}</p>
                        <p><b>Data de Nascimento/Aborto/Óbito da Criança:</b> {model.DataNascimentoObitoCrianca.Value.ToShortDateString()}</p>
                        <p><b>Desempregada Após Parto/Aborto/Óbito:</b> {(model.DesempregadaAposGravidez ? "Sim" : "Não")}</p>
                        <p><b>Data de Saída do Último Emprego:</b> {model.DataSaidaEmprego.Value.ToShortDateString()}</p>
                        <p><b>Recebeu Seguro Desemprego:</b> {(model.RecebeuSeguroDesempregoAntes ? "Sim" : "Não")}</p>
                        <p><b>Procurou SINE/CAT/PAT:</b> {(model.FoiAoSineAntesDoNascimentoObito ? "Sim" : "Não")}</p>                        
                    </section>
                ";
        }
    }
}