using System.Web.Mvc;

namespace Kanban.Controllers
{
    public class AppController : Controller
    {
        [HttpGet, Route("{*path}")]
        public ActionResult Index()
        {
            return View();
        }
    }
}