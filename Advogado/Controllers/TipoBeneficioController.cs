using Advogado.Data;
using Advogado.Helpers;
using Advogado.Models;
using Advogado.ViewsModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebGrease;

namespace Advogado.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TipoBeneficioController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult OrdenarPerguntas(int tipoBeneficioId)
        {
            var perguntas = _db.Perguntas.Where(x => x.TiposBeneficios.Any(y => y.TipoBeneficioId == tipoBeneficioId));
            var perguntasOrdens = _db.PerguntasOrdens.Where(x => x.TipoBeneficioId == tipoBeneficioId).ToList();

            if (perguntasOrdens != null && perguntasOrdens.Any())
            {
                foreach (var pergunta in perguntas)
                {
                    var perguntaOrdem = perguntasOrdens.FirstOrDefault(x => pergunta.PerguntaId == x.PerguntaId);
                    if (perguntaOrdem != null)
                        pergunta.Ordem = perguntaOrdem.Ordem;
                }
            }
            ViewBag.TipoBeneficioId = tipoBeneficioId;
            return View(perguntas.ToList());
        }

        public ActionResult OrdenarDocumentos(int tipoBeneficioId)
        {
            var documentos = _db.TiposArquivos.Where(x => x.TiposBeneficios.Any(y => y.TipoBeneficioId == tipoBeneficioId));
            var documentosOrdens = _db.TiposArquivosOrdens.Where(x => x.TipoBeneficioId == tipoBeneficioId).ToList();

            if (documentosOrdens != null && documentosOrdens.Any())
            {
                foreach (var documento in documentos)
                {
                    var tipoArquivoOrdem = documentosOrdens.FirstOrDefault(x => documento.TipoArquivoId == x.TipoArquivoId);
                    if (tipoArquivoOrdem != null)
                        documento.Ordem = tipoArquivoOrdem.Ordem;
                }
            }
            ViewBag.TipoBeneficioId = tipoBeneficioId;
            return View(documentos.ToList());
        }


        [HttpPost]
        public ActionResult OrdenarPerguntasLista(List<PerguntaOrdemVm> lista, int tipoBeneficioId, string ordemVirtual)
        {
            try
            {       
                var ordensVirtuais = new JavaScriptSerializer().Deserialize<List<OrdemVirtualPerguntaVm>>(ordemVirtual);

                var perguntas = _db.Perguntas.Where(x => x.TiposBeneficios.Any(y => y.TipoBeneficioId == tipoBeneficioId));

                var perguntasOrdens = _db.PerguntasOrdens.Where(x => x.TipoBeneficioId == tipoBeneficioId).ToList();

                //Replica a mesma situação da tabela para a realidade
                foreach (var pergunta in perguntas)
                {
                    var perguntaOrdem = perguntasOrdens.FirstOrDefault(x => pergunta.PerguntaId == x.PerguntaId);
                    if (perguntaOrdem != null)
                    {
                        perguntaOrdem.Ordem = ordensVirtuais.First(x => x.PerguntaId == pergunta.PerguntaId).Ordem;
                    }
                    else
                    {
                        _db.PerguntasOrdens.Add(new PerguntaOrdem { TipoBeneficioId = tipoBeneficioId, PerguntaId = pergunta.PerguntaId, Ordem = ordensVirtuais.First(x => x.PerguntaId == pergunta.PerguntaId).Ordem });
                    }
                }

                if (lista == null || !lista.Any()) return Json(new { Success = "Sem itens para atualizar!" }, JsonRequestBehavior.AllowGet);

                //Salva as alterações atuais
                _db.SaveChanges();

                //Efetua as alterações de fato dos itens que foram mexidos e na lista virtual
                foreach (var item in lista)
                {
                    var perguntaOrdem = _db.PerguntasOrdens.First(x => x.PerguntaId == item.PerguntaId && x.TipoBeneficioId == tipoBeneficioId);
                    perguntaOrdem.Ordem = item.Para;
                    ordensVirtuais[ordensVirtuais.IndexOf(ordensVirtuais.First(x => x.PerguntaId == item.PerguntaId))].Ordem = item.Para;
                }

                //Salva as alterações finais
                _db.SaveChanges();

                var ordemVirtualSerializado = new JavaScriptSerializer().Serialize(ordensVirtuais);

                return Json(new { Success = "Ordem atualizada com sucesso!", OrdemVirtual = ordemVirtualSerializado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult OrdenarDocumentosLista(List<TipoArquivoOrdemVm> lista, int tipoBeneficioId, string ordemVirtual)
        {
            try
            {
                var ordensVirtuais = new JavaScriptSerializer().Deserialize<List<OrdemVirtualTipoArquivoVm>>(ordemVirtual);

                var documentos = _db.TiposArquivos.Where(x => x.TiposBeneficios.Any(y => y.TipoBeneficioId == tipoBeneficioId));

                var perguntasOrdens = _db.TiposArquivosOrdens.Where(x => x.TipoBeneficioId == tipoBeneficioId).ToList();

                //Replica a mesma situação da tabela para a realidade
                foreach (var documento in documentos)
                {
                    var documentoOrdem = perguntasOrdens.FirstOrDefault(x => documento.TipoArquivoId == x.TipoArquivoId);
                    if (documentoOrdem != null)
                    {
                        documentoOrdem.Ordem = ordensVirtuais.First(x => x.TipoArquivoId == documento.TipoArquivoId).Ordem;
                    }
                    else
                    {
                        _db.TiposArquivosOrdens.Add(new TipoArquivoOrdem { TipoBeneficioId = tipoBeneficioId, TipoArquivoId = documento.TipoArquivoId, Ordem = ordensVirtuais.First(x => x.TipoArquivoId == documento.TipoArquivoId).Ordem });
                    }
                }

                if (lista == null || !lista.Any()) return Json(new { Success = "Sem itens para atualizar!" }, JsonRequestBehavior.AllowGet);

                //Salva as alterações atuais
                _db.SaveChanges();

                //Efetua as alterações de fato dos itens que foram mexidos e na lista virtual
                foreach (var item in lista)
                {
                    var documentoOrdem = _db.TiposArquivosOrdens.First(x => x.TipoArquivoId == item.TipoArquivoId && x.TipoBeneficioId == tipoBeneficioId);
                    documentoOrdem.Ordem = item.Para;
                    ordensVirtuais[ordensVirtuais.IndexOf(ordensVirtuais.First(x => x.TipoArquivoId == item.TipoArquivoId))].Ordem = item.Para;
                }

                //Salva as alterações finais
                _db.SaveChanges();

                var ordemVirtualSerializado = new JavaScriptSerializer().Serialize(ordensVirtuais);

                return Json(new { Success = "Ordem atualizada com sucesso!", OrdemVirtual = ordemVirtualSerializado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index()
        {
            var list = _db.TiposBeneficios.ToList();
            return View(list);
        }

        public ActionResult Novo()
        {
            bool existeTipoArquivo = _db.TiposArquivos.Any(x => x.Ativo);
            if (!existeTipoArquivo) return RedirectToAction("Index").Error("Por favor cadastre um tipo de documento antes de tentar cadastrar um tipo de benefício!");
            bool existePergunta = _db.Perguntas.Any(x => x.Ativo);
            if (!existePergunta) return RedirectToAction("Index").Error("Por favor cadastre uma pergunta antes de tentar cadastrar uma tipo de benefício!");
            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Novo(TipoBeneficio model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception();
                model.Ativo = true;
                model = SetarLista(model);
                _db.TiposBeneficios.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index").Success();
            }
            catch (Exception e)
            {
                SetViewBag();
                return View(model).Error(e.Message);
            }
        }

        public ActionResult Editar(int id)
        {
            var model = _db.TiposBeneficios.Include(x => x.TiposArquivos).Include(x => x.Perguntas).First(x => x.TipoBeneficioId == id);
            model.SetPerguntasIds();
            model.SetTiposArquivosIds();
            SetViewBag();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(TipoBeneficio model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception();
                model = SetarLista(model);
                Balancear(model);
                _db.SaveChanges();
                return RedirectToAction("Index").Success();
            }
            catch (Exception e)
            {
                SetViewBag();
                return View(model).Error(e.Message);
            }
        }

        public ActionResult Remover(int id)
        {
            try
            {
                var model = _db.TiposBeneficios.Find(id);
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

        private TipoBeneficio SetarLista(TipoBeneficio model)
        {
            if (model.TiposArquivosIds == null || !model.TiposArquivosIds.Any()) throw new Exception("Você deve selecionar pelo menos um tipo de documento!");
            if (model.PerguntasIds == null || !model.PerguntasIds.Any()) throw new Exception("Você deve selecionar pelo menos uma pergunta!");
            model.TiposArquivos = new List<TipoArquivo>();
            model.Perguntas = new List<Pergunta>();
            foreach (var tipoArquivoId in model.TiposArquivosIds)
            {
                var tipoArquivo = _db.TiposArquivos.Find(tipoArquivoId);
                model.TiposArquivos.Add(tipoArquivo);
            }
            foreach (var perguntaId in model.PerguntasIds)
            {
                var pergunta = _db.Perguntas.Find(perguntaId);
                model.Perguntas.Add(pergunta);
            }
            return model;
        }

        private void BalancearListaPerguntas(TipoBeneficio tipoBeneficioAntigo, TipoBeneficio tipoBeneficioNovo)
        {
            var perguntasIds = tipoBeneficioNovo.Perguntas.Select(x => x.PerguntaId).ToArray();
            tipoBeneficioNovo.Perguntas = new List<Pergunta>();
            tipoBeneficioAntigo.Perguntas = tipoBeneficioAntigo.Perguntas ?? new List<Pergunta>();
            var perguntas = _db.Perguntas.ToList();
            foreach (var pergunta in perguntas)
            {
                if (perguntasIds != null && perguntasIds.Any(x => x.Equals(pergunta.PerguntaId)))
                {
                    tipoBeneficioNovo.Perguntas.Add(_db.Perguntas.FirstOrDefault(x => x.PerguntaId == pergunta.PerguntaId));
                }
            }
            foreach (var pergunta in perguntas)
            {
                var perguntaAntiga = tipoBeneficioAntigo.Perguntas.FirstOrDefault(x => x.PerguntaId == pergunta.PerguntaId);

                if (perguntasIds != null && perguntasIds.Contains(pergunta.PerguntaId) && perguntaAntiga == null)
                {
                    tipoBeneficioAntigo.Perguntas.Add(_db.Perguntas.FirstOrDefault(x => x.PerguntaId == pergunta.PerguntaId));
                }
                else
                {
                    if (perguntaAntiga == null) continue;
                    if (perguntasIds != null && perguntasIds.Contains(perguntaAntiga.PerguntaId)) continue;
                    tipoBeneficioAntigo.Perguntas.Remove(perguntaAntiga);
                    var perguntasOrdens = _db.PerguntasOrdens.Where(x => x.PerguntaId == perguntaAntiga.PerguntaId && x.TipoBeneficioId == tipoBeneficioAntigo.TipoBeneficioId);
                    if (perguntasOrdens != null && perguntasOrdens.Any())
                    {
                        _db.PerguntasOrdens.RemoveRange(perguntasOrdens);
                        _db.SaveChanges();
                    }
                }
            }
        }

        private void BalancearListaTiposArquivos(TipoBeneficio tipoBeneficioAntigo, TipoBeneficio tipoBeneficioNovo)
        {
            var tiposArquivosIds = tipoBeneficioNovo.TiposArquivos.Select(x => x.TipoArquivoId).ToArray();
            tipoBeneficioNovo.TiposArquivos = new List<TipoArquivo>();
            tipoBeneficioAntigo.TiposArquivos = tipoBeneficioAntigo.TiposArquivos ?? new List<TipoArquivo>();
            var tiposarquivos = _db.TiposArquivos.ToList();
            foreach (var tipoarquivo in tiposarquivos)
            {
                if (tiposArquivosIds != null && tiposArquivosIds.Any(x => x.Equals(tipoarquivo.TipoArquivoId)))
                {
                    tipoBeneficioNovo.TiposArquivos.Add(_db.TiposArquivos.FirstOrDefault(x => x.TipoArquivoId == tipoarquivo.TipoArquivoId));
                }
            }
            foreach (var tipoarquivo in tiposarquivos)
            {
                var tipoArquivoAntigo = tipoBeneficioAntigo.TiposArquivos.FirstOrDefault(x => x.TipoArquivoId == tipoarquivo.TipoArquivoId);
                if (tiposArquivosIds != null && tiposArquivosIds.Contains(tipoarquivo.TipoArquivoId) && tipoArquivoAntigo == null)
                {
                    tipoBeneficioAntigo.TiposArquivos.Add(_db.TiposArquivos.FirstOrDefault(x => x.TipoArquivoId == tipoarquivo.TipoArquivoId));
                }
                else
                {
                    if (tipoArquivoAntigo == null) continue;
                    if (tiposArquivosIds != null && tiposArquivosIds.Contains(tipoArquivoAntigo.TipoArquivoId)) continue;
                    tipoBeneficioAntigo.TiposArquivos.Remove(tipoArquivoAntigo);
                    var documentosOrdens = _db.TiposArquivosOrdens.Where(x => x.TipoArquivoId == tipoArquivoAntigo.TipoArquivoId && x.TipoBeneficioId == tipoBeneficioAntigo.TipoBeneficioId);
                    if (documentosOrdens != null && documentosOrdens.Any())
                    {
                        _db.TiposArquivosOrdens.RemoveRange(documentosOrdens);
                        _db.SaveChanges();
                    }
                }
            }
        }

        public void Balancear(TipoBeneficio tipoBeneficioNovo)
        {
            var tipoBeneficioAntigo = _db.TiposBeneficios.Include(x => x.Perguntas).Include(x => x.TiposArquivos).FirstOrDefault(x => x.TipoBeneficioId == tipoBeneficioNovo.TipoBeneficioId);
            tipoBeneficioNovo.Ativo = tipoBeneficioAntigo.Ativo;
            BalancearListaPerguntas(tipoBeneficioAntigo, tipoBeneficioNovo);
            BalancearListaTiposArquivos(tipoBeneficioAntigo, tipoBeneficioNovo);
            _db.Entry(tipoBeneficioAntigo).CurrentValues.SetValues(tipoBeneficioNovo);
        }

        private void SetViewBag()
        {
            ViewBag.TiposArquivos = _db.TiposArquivos.Where(x => x.Ativo).ToList();
            ViewBag.Perguntas = _db.Perguntas.Where(x => x.Ativo).ToList();
        }
    }
}