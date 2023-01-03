using Advogado.Data;
using Advogado.Helpers;
using Advogado.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Advogado.Controllers
{
    public class ContaController : Controller
    {
        private readonly MainContext _db = new MainContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Cpf, string Senha)
        {
            try
            {
                if (string.IsNullOrEmpty(Cpf)) throw new Exception("Informe um CPF!");
                if (string.IsNullOrEmpty(Senha)) throw new Exception("Informe uma senha!");
                var usuario = _db.Funcionarios.FirstOrDefault(x => x.Cpf == Cpf);
                if (usuario == null) throw new Exception("Usuário ou senha inválidos!");
                if (usuario.Ativo == false) throw new Exception("Usuário desativado!");
                var hashExistente = usuario.Senha;                
                var usuarioParaValidar = new Funcionario
                { 
                    Cpf = Cpf, 
                    Senha = Senha 
                };                     
                if (usuarioParaValidar.ValidarUsuarioSenha(hashExistente))
                {
                    var usuarioJson = new JavaScriptSerializer().Serialize(usuario);
                    FormsAuthentication.SetAuthCookie(usuarioJson, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    throw new Exception("Usuário ou senha inválidos!");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Conta").Error(e.Message);
            }
        }

        [HttpPost]
        public ActionResult AlterarSenha(string novaSenha)
        {
            try
            {
                var funcionarioLogado = Funcionario.FuncionarioLogado();
                var funcionario = _db.Funcionarios.Find(funcionarioLogado.FuncionarioId);
                funcionario.Senha = funcionario.GerarMd5(novaSenha);
                _db.SaveChanges();
                return Json(new { Success = "Senha alterada com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Conta");
        }
    }
}