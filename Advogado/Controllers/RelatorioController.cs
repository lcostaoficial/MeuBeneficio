using System.Web.Mvc;

namespace Advogado.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RelatorioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}