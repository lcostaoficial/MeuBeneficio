using System.Web.Mvc;

namespace Advogado.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Caso");
        }
    }
}