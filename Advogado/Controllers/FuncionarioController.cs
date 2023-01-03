using Advogado.Data;
using Advogado.Helpers;
using Advogado.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Advogado.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class FuncionarioController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            var list = _db.Funcionarios.ToList();
            return View(list);
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Novo(Funcionario model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception();
                var cpfJaExiste = _db.Funcionarios.Any(x => x.Cpf == model.Cpf);
                if (cpfJaExiste) throw new Exception("O CPF informado já foi cadastrado!");
                var cpfValido = model.ValidarCpf();
                if (!cpfValido) throw new Exception("O CPF informado não é um CPF válido!");
                model.Ativo = true;
                model.SenhaPadrao();
                _db.Funcionarios.Add(model);
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
            var model = _db.Funcionarios.Find(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Funcionario model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception();
                var novo = _db.Funcionarios.Find(model.FuncionarioId);
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
                var model = _db.Funcionarios.Find(id);
                model.InverterAtivo();
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index").Success();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index").Error(e.Message);
            }
        }
    }
}