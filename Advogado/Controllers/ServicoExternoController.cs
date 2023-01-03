using Advogado.Data;
using Advogado.Helpers;
using Advogado.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Advogado.Controllers
{
    public class ServicoExternoController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult SalarioMaternidade()
        {
            return View();
        }

        public bool ValidarRegraIdade(DateTime dataNascimento)
        {
            if (dataNascimento.Date >= DateTime.Now.Date) return true;
            int Anos = new DateTime(DateTime.Now.Subtract(dataNascimento).Ticks).Year - 1;
            return Anos > 5 ? false : true;
        }

        public void GravarSemDireito(QuestionarioSalarioMaternidade model)
        {
            model.DataResposta = DateTime.Now;
            model.TemDireito = false;
            _db.QuestionariosSalariosMaternidades.Add(model);
            _db.SaveChanges();
        }

        public void GravarComDireito(QuestionarioSalarioMaternidade model)
        {
            model.DataResposta = DateTime.Now;
            model.TemDireito = true;
            _db.QuestionariosSalariosMaternidades.Add(model);
            _db.SaveChanges();
        }

        [HttpPost]
        public async Task<ActionResult> SalarioMaternidade(QuestionarioSalarioMaternidade model)
        {
            try
            {
                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.Identificacao)
                {
                    if (string.IsNullOrEmpty(model.Nome)) throw new Exception("Por favor preencha seu nome!");
                    if (string.IsNullOrEmpty(model.Telefone)) throw new Exception("Por favor preencha seu telefone!");
                    if (string.IsNullOrEmpty(model.Cidade)) throw new Exception("Por favor preencha sua cidade!");
                    if (model.Estado == 0) throw new Exception("Por favor preencha um estado!");
                    if (!Regex.Match(model.Telefone, @"(\(?\d{2}\)?\s)?(\d{4,5}\-\d{4})").Success) throw new Exception("Número de telefone inválido!");
                    model.ObjetoSerializado = JsonConvert.SerializeObject(model);
                    var html = PartialView("_CarteiraAssinada", model).RenderToString();
                    return Json(new { html }, JsonRequestBehavior.AllowGet);
                }

                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.CarteiraAssinada)
                {
                    var novoObjeto = DesserializarSalarioMaternidade(model.ObjetoSerializado);
                    novoObjeto.TrabalhouCarteiraAssinada = model.TrabalhouCarteiraAssinada;
                    novoObjeto.ObjetoSerializado = JsonConvert.SerializeObject(novoObjeto);
                    if (novoObjeto.TrabalhouCarteiraAssinada == true)
                    {
                        var html = PartialView("_HouveAborto", novoObjeto).RenderToString();
                        return Json(new { html }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var html = PartialView("_PagouInssMeiContribuinteIndividual", novoObjeto).RenderToString();
                        return Json(new { html }, JsonRequestBehavior.AllowGet);
                    }
                }

                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.PagouInssMeiContribuinteIndividual)
                {
                    var novoObjeto = DesserializarSalarioMaternidade(model.ObjetoSerializado);
                    novoObjeto.PagouInssMeiOuContribuinteIndividual = model.PagouInssMeiOuContribuinteIndividual;
                    novoObjeto.ObjetoSerializado = JsonConvert.SerializeObject(novoObjeto);

                    if (novoObjeto.PagouInssMeiOuContribuinteIndividual == true)
                    {
                        var html = PartialView("_HouveAborto", novoObjeto).RenderToString();
                        return Json(new { html }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        GravarSemDireito(novoObjeto);
                        return Json(new
                        {
                            NaoTemDireito = "No seu caso, infelizmente não é possível receber esse benefício. Para ter direito é necessário ter trabalhado ao menos um dia com registro em carteira antes do nascimento da criança ou ter pagado o INSS como MEI ou Contribuinte Individual."
                        }, JsonRequestBehavior.AllowGet);
                    }
                }

                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.HouveAborto)
                {
                    var novoObjeto = DesserializarSalarioMaternidade(model.ObjetoSerializado);
                    novoObjeto.HouveAborto = model.HouveAborto;
                    novoObjeto.ObjetoSerializado = JsonConvert.SerializeObject(novoObjeto);

                    if (novoObjeto.HouveAborto == true)
                    {
                        var html = PartialView("_AbortoApos23Semana", novoObjeto).RenderToString();
                        return Json(new { html }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var html = PartialView("_DataNascimentoCriancaObito", novoObjeto).RenderToString();
                        return Json(new { html }, JsonRequestBehavior.AllowGet);
                    }
                }

                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.AbortoOcorreuApos23Semana)
                {
                    var novoObjeto = DesserializarSalarioMaternidade(model.ObjetoSerializado);
                    novoObjeto.AbortoOcorreuApos23Semana = model.AbortoOcorreuApos23Semana;
                    novoObjeto.ObjetoSerializado = JsonConvert.SerializeObject(novoObjeto);
                    if (novoObjeto.AbortoOcorreuApos23Semana == true)
                    {
                        var html = PartialView("_DataNascimentoCriancaObito", novoObjeto).RenderToString();
                        return Json(new { html }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        GravarSemDireito(novoObjeto);
                        return Json(new { NaoTemDireito = "Infelizmente você não tem direito ao benefício." }, JsonRequestBehavior.AllowGet);
                    }
                }

                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.DataNascimentoObitoCrianca)
                {
                    if (model.DataNascimentoObitoCrianca == null) throw new Exception("Por favor informe uma data válida!");
                    var novoObjeto = DesserializarSalarioMaternidade(model.ObjetoSerializado);
                    novoObjeto.DataNascimentoObitoCrianca = model.DataNascimentoObitoCrianca;
                    novoObjeto.ObjetoSerializado = JsonConvert.SerializeObject(novoObjeto);

                    if (ValidarRegraIdade(novoObjeto.DataNascimentoObitoCrianca.Value))
                    {
                        var html = PartialView("_DesempregadaAposGravidez", novoObjeto).RenderToString();
                        return Json(new { html }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        GravarSemDireito(novoObjeto);
                        if (novoObjeto.HouveAborto == true)
                        {
                            return Json(new { NaoTemDireito = "No seu caso, infelizmente não tem direito, pois sua criança já faleceu a mais de 5 anos." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { NaoTemDireito = "No seu caso, infelizmente não tem direito, pois o governo só paga esse benefício para mães que tem filho com menos de cinco anos." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.DesempregadaAposGravidez)
                {
                    var novoObjeto = DesserializarSalarioMaternidade(model.ObjetoSerializado);
                    novoObjeto.DesempregadaAposGravidez = model.DesempregadaAposGravidez;
                    novoObjeto.ObjetoSerializado = JsonConvert.SerializeObject(novoObjeto);
                    if (novoObjeto.DesempregadaAposGravidez)
                    {
                        var html = PartialView("_DataSaidaEmprego", novoObjeto).RenderToString();
                        return Json(new { html }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        GravarSemDireito(novoObjeto);
                        return Json(new { NaoTemDireito = "Você estava empregada quando sua criança nasceu. Para que você fosse beneficiada, deveria estar desempregada na data que a criança nasceu." }, JsonRequestBehavior.AllowGet);
                    }
                }

                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.DataSaidaEmprego)
                {
                    if (model.DataSaidaEmprego == null) throw new Exception("Por favor informe uma data válida!");
                    var novoObjeto = DesserializarSalarioMaternidade(model.ObjetoSerializado);
                    novoObjeto.DataSaidaEmprego = model.DataSaidaEmprego;
                    novoObjeto.ObjetoSerializado = JsonConvert.SerializeObject(novoObjeto);
                    var html = PartialView("_RecebeuSeguroDesempregoAntes", novoObjeto).RenderToString();
                    return Json(new { html }, JsonRequestBehavior.AllowGet);
                }

                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.RecebeuSeguroDesempregoAntes)
                {
                    var novoObjeto = DesserializarSalarioMaternidade(model.ObjetoSerializado);
                    novoObjeto.RecebeuSeguroDesempregoAntes = model.RecebeuSeguroDesempregoAntes;
                    novoObjeto.ObjetoSerializado = JsonConvert.SerializeObject(novoObjeto);
                    var html = PartialView("_FoiAoSineAntesDoNascimentoObito", novoObjeto).RenderToString();
                    return Json(new { html }, JsonRequestBehavior.AllowGet);
                }

                if (model.PassoSalarioMaternidade == PassoSalarioMaternidade.FoiAoSineAntesDoNascimentoObito)
                {
                    var novoObjeto = DesserializarSalarioMaternidade(model.ObjetoSerializado);
                    novoObjeto.FoiAoSineAntesDoNascimentoObito = model.FoiAoSineAntesDoNascimentoObito;
                    novoObjeto.ObjetoSerializado = JsonConvert.SerializeObject(novoObjeto);

                    //Forçar alteração dos dados para testes
                    //novoObjeto.DataSaidaEmprego = Convert.ToDateTime("24/11/2014");
                    //novoObjeto.DataNascimentoObitoCrianca = Convert.ToDateTime("20/02/2016");
                    //novoObjeto.RecebeuSeguroDesempregoAntes = false;
                    //novoObjeto.FoiAoSineAntesDoNascimentoObito = false;

                    //Por padrão 1 ano de proteção
                    var anosProtecao = 1;

                    //Se recebeu seguro desemprego ou foi ao SINE acrescenta +1 ano
                    anosProtecao += (novoObjeto.RecebeuSeguroDesempregoAntes || novoObjeto.FoiAoSineAntesDoNascimentoObito ? 1 : 0);

                    //Data de saída do emprego
                    var dataSaidaEmprego = novoObjeto.DataSaidaEmprego.Value;

                    //Adiciona os anos de proteção na data de saída do emprego
                    dataSaidaEmprego = dataSaidaEmprego.AddYears(anosProtecao);

                    //Acrescenta mês subsequente
                    var dataFinalProtecao = dataSaidaEmprego.AddMonths(2);

                    //Encontra o décimo quinto dia do mês subsequente
                    dataFinalProtecao = new DateTime(dataFinalProtecao.Year, dataFinalProtecao.Month, 15);

                    //Soma mais 28 dias
                    dataFinalProtecao = dataFinalProtecao.AddDays(28);

                    if (novoObjeto.DataNascimentoObitoCrianca.Value >= novoObjeto.DataSaidaEmprego && novoObjeto.DataNascimentoObitoCrianca.Value <= dataFinalProtecao)
                    {
                        GravarComDireito(novoObjeto);
                        var email = new EmailManager(new ArrayList { /*"jose.lucas@infouniron.com",*/ "marcosdasilva.ro@gmail.com"/*, "elbervm@gmail.com"*/ });
                        email.SalarioMaternidade(novoObjeto);
                        await email.Enviar();
                        return Json(new { TemDireito = $"Vou te dar uma ótima notícia <b>{novoObjeto.Nome}</b>! <br><br> Pelas informações que você forneceu, é possível que consiga receber até R$ 4.500,00. <br><br> Logo mais entraremos em contato com você.", EnvioMsg = $"Olá, meu nome é {novoObjeto.Nome}, possivelmente tenho direito ao benefício Salário Maternidade, você pode me ajudar?" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        GravarSemDireito(novoObjeto);
                        return Json(new { NaoTemDireito = $"Você teria direito se o seu bebê tivesse nascido até o dia {dataFinalProtecao.ToShortDateString()}." }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { error = "Erro desconhecido" }, JsonRequestBehavior.AllowGet); ;

            }
            catch (Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet); ;
            }
        }

        public QuestionarioSalarioMaternidade DesserializarSalarioMaternidade(string objeto)
        {
            return JsonConvert.DeserializeObject<QuestionarioSalarioMaternidade>(objeto);
        }
    }
}