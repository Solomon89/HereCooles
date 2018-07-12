using System.Web.Mvc;
using Hakaton_Service;

namespace Hakaton_View.Controllers
{
    public class MenuController : Controller
    {
        private readonly DataManager _dataManager;

        public MenuController()
        {
            _dataManager = new DataManager();
        }

        // GET: Menu
        [HttpGet]
        public ActionResult Menu()
        {
            return View();
        }
    }
}