using Hakaton_Service;
using System.Web.Mvc;

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
        public ActionResult Index()
        {
            return View();
        }
    }
}