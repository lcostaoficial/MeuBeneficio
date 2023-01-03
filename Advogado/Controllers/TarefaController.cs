using Advogado.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System;
using Advogado.Models;

namespace Advogado.Controllers
{
    [Authorize(Roles = "Administrador, Secretaria")]
    public class TarefaController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult QuadroGeral()
        {
            ViewBag.Responsaveis = _db.Funcionarios.ToList();
            return View();
        }

        public ActionResult TarefasQuadroGeral()
        {
            var tarefas = _db.Tarefas.Include(x => x.FuncionarioResponsavel).ToList().Select(x => new
            {
                id = x.TarefaId,
                title = x.Titulo,
                description = x.Descricao,
                start = x.DataExpiracao.ToString("yyyy-MM-dd"),
                className = x.CorTarefa,
                author = x.Responsavel
            });
            return Json(tarefas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int casoId)
        {
            var tarefas = _db.Tarefas.Where(x => x.CasoId == casoId).Include(x => x.FuncionarioResponsavel).ToList().Select(x => new
            {
                id = x.TarefaId,
                title = x.Titulo,
                description = x.Descricao,
                start = x.DataExpiracao.ToString("yyyy-MM-dd"),
                className = x.CorTarefa,
                author = x.Responsavel
            });
            return Json(tarefas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExcluirTarefa(int tarefaId)
        {
            try
            {
                if (tarefaId == 0) throw new Exception("ID da tarefa não identificado!");
                var tarefa = _db.Tarefas.First(x => x.TarefaId == tarefaId);
                var justificativas = _db.JustificativasTarefas.Where(x => x.TarefaId == tarefa.TarefaId);
                _db.JustificativasTarefas.RemoveRange(justificativas);                
                _db.Entry(tarefa).State = EntityState.Deleted;
                _db.SaveChanges();
                return Json(new { success = "Tarefa excluída permanentemente!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AssumirTarefa(int tarefaId)
        {
            try
            {
                if (tarefaId == 0) throw new Exception("ID da tarefa não identificado!");
                var tarefa = _db.Tarefas.First(x => x.TarefaId == tarefaId);
                tarefa.DataInicio = DateTime.Now;
                tarefa.FuncionarioResponsavelId = Funcionario.FuncionarioLogado().FuncionarioId;
                _db.SaveChanges();
                return Json(new { success = "Tarefa assumida com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FinalizarTarefa(Tarefa model)
        {
            try
            {
                if (model.TarefaId == 0) throw new Exception("ID da tarefa não identificado!");
                if (string.IsNullOrEmpty(model.Solucao)) throw new Exception("Solução não preenchida!");
                var tarefa = _db.Tarefas.First(x => x.TarefaId == model.TarefaId);
                if (tarefa.FuncionarioResponsavelId == 0) throw new Exception("Não é possível finalizar tarefa em que não há um responsável.");
                tarefa.DataFinalizacao = DateTime.Now;
                tarefa.Solucao = model.Solucao;
                tarefa.Finalizada = true;
                _db.SaveChanges();
                return Json(new { success = "Tarefa finalizada com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AlterarPrazo(Tarefa model)
        {
            try
            {
                if (model.TarefaId == 0) throw new Exception("ID da tarefa não identificado!");
                if (string.IsNullOrEmpty(model.JustificativaDilatacaoPrazo)) throw new Exception("Justificativa não preenchida!");
                if (model.NovaDataExpiracao == null) throw new Exception("Nova data de expiração não foi informada!");
                var tarefa = _db.Tarefas.First(x => x.TarefaId == model.TarefaId);
                var justificativaTarefa = new JustificativaTarefa
                {
                    Descricao = model.JustificativaDilatacaoPrazo,
                    Movimentacao = $"O(a) funcionário(a) {Funcionario.FuncionarioLogado().Nome} alterou o prazo da tarefa de número {tarefa.TarefaId} de {tarefa.DataExpiracao.ToShortDateString()} para {model.NovaDataExpiracao.ToShortDateString()}",
                    DataJustificativa = DateTime.Now,
                    TipoJustificativaTarefa = TipoJustificativaTarefa.AlteracaoPrazo,
                    FuncionarioId = Funcionario.FuncionarioLogado().FuncionarioId,
                    TarefaId = model.TarefaId
                };
                tarefa.DataExpiracao = model.NovaDataExpiracao.Date;
                _db.JustificativasTarefas.Add(justificativaTarefa);
                _db.SaveChanges();
                return Json(new { success = "Prazo alterado com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AlterarResponsavel(Tarefa model)
        {
            try
            {
                if (model.FuncionarioResponsavelId == null) throw new Exception("Nenhum funcionário responsável atribuído!");
                if (string.IsNullOrEmpty(model.JustificativaAlteracaoResponsavel)) throw new Exception("Justificativa não preenchida!");
                var tarefa = _db.Tarefas.First(x => x.TarefaId == model.TarefaId);
                var novoResponsavel = _db.Funcionarios.Find(model.FuncionarioResponsavelId);
                var antigoResponsavel = _db.Funcionarios.Find(tarefa.FuncionarioResponsavelId);
                var movimentacao = string.Empty;
                if (antigoResponsavel == null)
                {
                    movimentacao = $"O(a) funcionário(a) {Funcionario.FuncionarioLogado().Nome} alterou o responsável da tarefa para o funcionário(a) {novoResponsavel.Nome}";
                }
                else
                {
                    movimentacao = $"O(a) funcionário(a) {Funcionario.FuncionarioLogado().Nome} alterou o responsável de {antigoResponsavel.Nome} para {novoResponsavel.Nome}";
                }
                var justificativaTarefa = new JustificativaTarefa
                {
                    Descricao = model.JustificativaAlteracaoResponsavel,
                    Movimentacao = movimentacao,
                    DataJustificativa = DateTime.Now,
                    TipoJustificativaTarefa = TipoJustificativaTarefa.AlteracaoResponsavel,
                    FuncionarioId = Funcionario.FuncionarioLogado().FuncionarioId,
                    TarefaId = model.TarefaId
                };
                tarefa.FuncionarioResponsavelId = model.FuncionarioResponsavelId;

                _db.JustificativasTarefas.Add(justificativaTarefa);

                _db.SaveChanges();
                return Json(new { success = "Responsável alterado com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DetalhesTarefa(int tarefaId)
        {
            var tarefa = _db.Tarefas.Include(x => x.FuncionarioResponsavel).Include(x => x.FuncionarioCriador).First(x => x.TarefaId == tarefaId);
            return PartialView("_DetalhesTarefa", tarefa);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult RelatorioDeCasosAtrasados()
        {
            try
            {
                var casosAtrasados = _db.Casos.Include(x => x.Cliente).Include(x => x.Tarefas.Select(y => y.FuncionarioResponsavel)).Where(x => x.Tarefas.Any(y => y.Finalizada == false && y.DataExpiracao < DateTime.Now)).ToList();
                casosAtrasados.ForEach(x => x.RemoverTarefasEmDias());
                return Json(new { Success = "Dados da impressão carregados com sucesso!", Model = casosAtrasados }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}