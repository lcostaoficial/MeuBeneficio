using Advogado.Data;
using Advogado.Helpers;
using Advogado.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Advogado.Controllers
{
    [Authorize(Roles = "Administrador, Secretaria")]
    public class ClienteController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            var list = _db.Clientes.ToList();
            return View(list);
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Novo(Cliente model)
        {
            try
            {
                ValidarEstrangeiro(model);
                if (!ModelState.IsValid) throw new Exception();
                if (!model.ValidarCpf()) throw new Exception("O CPF informado é inválido!");
                var cpfJaExiste = _db.Clientes.Any(x => x.Cpf == model.Cpf);
                if (cpfJaExiste) throw new Exception("O CPF informado já foi utilizado!");
                _db.Clientes.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index").Success();
            }
            catch (Exception e)
            {
                return View(model).Error(e.Message);
            }
        }

        public ActionResult Editar(int id)
        {
            var model = _db.Clientes.Find(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Cliente model)
        {
            try
            {
                ValidarEstrangeiro(model);
                ModelState.Remove("Cpf");
                if (!ModelState.IsValid) throw new Exception();
                var novo = _db.Clientes.Find(model.ClienteId);
                novo.Atualizar(model);
                _db.Entry(novo).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index").Success();
            }
            catch (Exception e)
            {
                return View(model).Error(e.Message);
            }
        }

        public ActionResult Remover(int id)
        {
            try
            {
                //Cliente para excluir
                var cliente = _db.Clientes.Find(id);

                //Casos para excluir
                var casos = _db.Casos.Where(x => x.ClienteId == id);

                //Arquivos do caso para excluir
                var arquivosDoCaso = _db.Arquivos.Where(x => x.Caso.ClienteId == id);

                //Movimentações do caso para excluir
                var movimentacoesDoCaso = _db.Movimentacoes.Where(x => x.Caso.ClienteId == id);

                //Grupo familiar do caso para excluir
                var grupoFamiliarDoCaso = _db.GruposFamiliares.Where(x => x.Caso.ClienteId == id);

                //Tarefas do caso para excluir
                var tarefasDoCaso = _db.Tarefas.Where(x => x.Caso.ClienteId == id);

                //Justificativa das tarefas do caso para excluir
                var justificativasTarefasDoCaso = _db.JustificativasTarefas.Where(x => x.Tarefa.Caso.ClienteId == id);

                //Respostas do caso para excluir
                var respostasDoCaso = _db.Respostas.Include(x => x.Alternativas).Where(x => x.Caso.ClienteId == id);

                _db.Arquivos.RemoveRange(arquivosDoCaso);

                _db.Movimentacoes.RemoveRange(movimentacoesDoCaso);

                _db.GruposFamiliares.RemoveRange(grupoFamiliarDoCaso);

                _db.JustificativasTarefas.RemoveRange(justificativasTarefasDoCaso);

                _db.Tarefas.RemoveRange(tarefasDoCaso);

                _db.Respostas.RemoveRange(respostasDoCaso);

                _db.Casos.RemoveRange(casos);

                _db.Entry(cliente).State = EntityState.Deleted;

                _db.SaveChanges();

                return RedirectToAction("Index").Success();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index").Error(e.Message);
            }
        }

        public void ValidarEstrangeiro(Cliente model)
        {
            if (model.ClienteEstrangeiro == true)
            {
                if (string.IsNullOrEmpty(model.DocumentoIdentificacaoEstrangeiro)) throw new Exception("Documento do estrangeiro não preenchido!");
            }
            else
            {
                if (string.IsNullOrEmpty(model.Rg)) throw new Exception("RG não preenchido!");
                if (model.OrgaoExpedidorRg == 0) throw new Exception("Orgão expedidor do RG não preenchido!");
                if (model.EstadoOrgaoExpedidor == 0) throw new Exception("Estado emissor do RG não preenchido!");
            }
        }
    }
}