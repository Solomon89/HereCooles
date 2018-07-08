using Hakaton_Service;
using System.Web.Mvc;

namespace Hakaton_View.Controllers
{
    public class MapController : Controller
    {
        private readonly DataManager _dataManager;

        public MapController()
        {
            _dataManager = new DataManager();
        }
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }
    }
}