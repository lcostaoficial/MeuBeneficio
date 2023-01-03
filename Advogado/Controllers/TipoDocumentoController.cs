using Advogado.Data;
using Advogado.Helpers;
using Advogado.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Advogado.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TipoDocumentoController : Controller
    {
        private readonly MainContext _db = new MainContext();        

        public ActionResult Index()
        {          
            var list = _db.TiposArquivos.ToList();
            return View(list);
        }

        public ActionResult Novo()
        {
            //bool existeGrupoTipoArquivo = _db.GruposTiposArquivos.Any(x => x.Ativo);
            //if (!existeGrupoTipoArquivo) return RedirectToAction("Index").Error("Por favor cadastre um grupo de tipo de documento antes de tentar cadastrar um tipo de documento!");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Novo(TipoArquivo model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception();
                model.Modelo = string.Empty;
                if (model.ModeloArquivo != null && model.ModeloArquivo.ContentLength > 0) model = Anexar(model);
                model.Ativo = true;
                _db.TiposArquivos.Add(model);
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
            var model = _db.TiposArquivos.Find(id);            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(TipoArquivo model)
        {
            try
            {
                if (!ModelState.IsValid) throw new Exception();
                var novo = _db.TiposArquivos.Find(model.TipoArquivoId);
                if (model.MantemMesmoModelo == false)
                {
                    model = Anexar(model);
                }                   
                else
                {
                    model.Modelo = novo.Modelo;
                }
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
                var model = _db.TiposArquivos.Find(id);
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

        public ActionResult ModeloArquivo()
        {
            return PartialView("_ModeloArquivo");
        }

        private TipoArquivo Anexar(TipoArquivo model)
        {
            int ByteSize = 1048576;
            int SizeInBytesMax = ByteSize * 10;
            var virtualPath = "~/Uploads/TipoArquivo";
            var allowedExtensions = new List<string> { ".pdf", ".docx", ".doc" };
            var tipoArquivo = new TipoArquivo();
            if (model.TipoArquivoId != 0) tipoArquivo = _db.TiposArquivos.Find(model.TipoArquivoId);
            if (model.ModeloArquivo != null && model.ModeloArquivo.ContentLength > 0)
            {
                int sizeByte = model.ModeloArquivo.ContentLength;
                if (!(sizeByte <= SizeInBytesMax)) throw new Exception("Limite de envio de arquivo atingido!");
                string extension = Path.GetExtension(model.ModeloArquivo.FileName);
                if (!allowedExtensions.Contains(extension.ToLower())) throw new Exception("Extensão não permitida");
                var physicalPath = Server.MapPath(virtualPath);
                if (!Directory.Exists(physicalPath)) Directory.CreateDirectory(physicalPath);
                if (!Directory.Exists(physicalPath)) throw new Exception($"Diretório {physicalPath} não existe!");
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(physicalPath, fileName);
                if (model.TipoArquivoId != 0)
                {
                    var fileOld = Server.MapPath(tipoArquivo.Modelo);
                    if (System.IO.File.Exists(fileOld)) System.IO.File.Delete(fileOld);
                }
                model.ModeloArquivo.SaveAs(filePath);
                model.Modelo = $"{virtualPath}/{fileName}";
            }
            else if (model.TipoArquivoId != 0)
            {
                var fileOld = Server.MapPath(tipoArquivo.Modelo);
                if (System.IO.File.Exists(fileOld)) System.IO.File.Delete(fileOld);
                model.Modelo = string.Empty;
            }
            return model;
        }
    }
}