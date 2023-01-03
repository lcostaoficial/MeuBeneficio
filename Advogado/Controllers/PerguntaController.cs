using Advogado.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Advogado.Models;
using System.Collections.Generic;
using Advogado.Helpers;
using System;

namespace Advogado.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PerguntaController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Index()
        {
            var list = _db.Perguntas.ToList();            
            return View(list);
        }

        public ActionResult Novo()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Novo(Pergunta model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Campos inválidos!");
                if (model.MultiplaAlternativa && model.Alternativas == null) throw new Exception("Uma pergunta com múltiplas alternativas deve possuir alternativas!");
                if (model.MultiplaAlternativa && !model.Alternativas.Any()) throw new Exception("Uma pergunta com múltiplas alternativas deve possuir alternativas!");
                model.Ativo = true;
                _db.Perguntas.Add(model);
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
            var model = _db.Perguntas.Include(x => x.Alternativas).FirstOrDefault(x => x.PerguntaId == id);          
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Pergunta model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception("Campos inválidos!");
                var possuiRespostas = _db.Respostas.Any(x => x.PerguntaId == model.PerguntaId);
                if (possuiRespostas) throw new Exception("Não é possível alterar uma pergunta quando ela já possuir respostas!");
                if (model.MultiplaAlternativa && model.Alternativas == null) throw new Exception("Uma pergunta com múltiplas alternativas deve possuir alternativas!");
                if (model.MultiplaAlternativa && !model.Alternativas.Any()) throw new Exception("Uma pergunta com múltiplas alternativas deve possuir alternativas!");
                Atualizar(model);   
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
                var model = _db.Perguntas.Find(id);
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

        private Pergunta Atualizar(Pergunta perguntaNova)
        {
            var perguntaAntiga = _db.Perguntas.Include(x => x.Alternativas).FirstOrDefault(x => x.PerguntaId == perguntaNova.PerguntaId);
            perguntaNova.Ativo = perguntaAntiga.Ativo;
            BalancearListaAlternativas(perguntaAntiga, perguntaNova);
            _db.Entry(perguntaAntiga).CurrentValues.SetValues(perguntaNova);
            return perguntaNova;
        }

        private void BalancearListaAlternativas(Pergunta perguntaAntiga, Pergunta perguntaNova)
        {
            if (perguntaNova.Alternativas == null || !perguntaNova.Alternativas.Any())
            {
                var listaToRemove = perguntaAntiga.Alternativas.Where(x => x.Ativo).Select(s => s.AlternativaId).ToList();
                listaToRemove.ForEach(x =>
                {
                    var alternativaToRemove = perguntaAntiga.Alternativas.FirstOrDefault(y => y.AlternativaId == x);
                    _db.Alternativas.Remove(alternativaToRemove);
                });
            }
            else
            {
                var listaToAdd = perguntaNova.Alternativas.Where(x => x.AlternativaId == 0).ToList();
                var listaToUpdate = perguntaNova.Alternativas.Where(x => perguntaAntiga.Alternativas.Any(y => y.AlternativaId == x.AlternativaId)).ToList();
                var listaToRemove = perguntaAntiga.Alternativas.Where(x => x.Ativo && perguntaNova.Alternativas.All(y => y.AlternativaId != x.AlternativaId)).Select(s => s.AlternativaId).ToList();
                listaToAdd.ForEach(x => perguntaAntiga.Alternativas.Add(x));
                listaToUpdate.ForEach(x =>
                {
                    var alternativaUpdate = perguntaAntiga.Alternativas.FirstOrDefault(y => y.AlternativaId == x.AlternativaId);
                    if (alternativaUpdate != null) alternativaUpdate.Descricao = x.Descricao;
                });
                listaToRemove.ForEach(x =>
                {
                    var alternativaToRemove = perguntaAntiga.Alternativas.FirstOrDefault(y => y.AlternativaId == x);
                    _db.Alternativas.Remove(alternativaToRemove);
                });
            }
        }        

        public ActionResult Alternativas()
        {
            return PartialView("_Alternativas");
        }

        public ActionResult AtualizarAlternativas(int id)
        {
            var pergunta = _db.Perguntas.Include(x => x.Alternativas).FirstOrDefault(x => x.PerguntaId == id);
            return PartialView("_Alternativas", pergunta);
        }

        [HttpPost]
        public ActionResult AdicionarAlternativa(Pergunta perguntaAlternativa)
        {
            perguntaAlternativa.Alternativas = perguntaAlternativa.Alternativas ?? new List<Alternativa>();
            perguntaAlternativa.Alternativas.Add(new Alternativa { Descricao = perguntaAlternativa.Descricao });
            perguntaAlternativa.Descricao = string.Empty;
            SetarUrl();
            return PartialView("_ListaAlternativas", perguntaAlternativa.Alternativas);
        }

        [HttpPost]
        public ActionResult RemoverAlternativa(Pergunta perguntaAlternativa, int index)
        {
            var alternativas = perguntaAlternativa.Alternativas?.ToList();
            alternativas.RemoveAt(index);
            perguntaAlternativa.Alternativas = alternativas;
            SetarUrl();
            return PartialView("_ListaAlternativas", perguntaAlternativa.Alternativas);
        }

        public ActionResult ListaAlternativas(Pergunta perguntaAlternativa)
        {
            perguntaAlternativa = perguntaAlternativa ?? new Pergunta();
            SetarUrl();
            return PartialView("_ListaAlternativas", perguntaAlternativa.Alternativas?.Where(x => x.Ativo).ToList() ?? perguntaAlternativa.Alternativas);
        }      

        private void SetarUrl()
        {
            ViewBag.UrlAdd = Url.Action("AdicionarAlternativa", "Pergunta");
            ViewBag.UrlRemove = Url.Action("RemoverAlternativa", "Pergunta");
        }
    }
}