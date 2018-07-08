using Hakaton_Db.Models;
using Hakaton_Service;
using Hakaton_Service.SubModels;
using Hakaton_View.Controllers.Manage;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Hakaton_View.Controllers
{
    public class MapController : Controller
    {
        private readonly DataManager _dataManager;
        private const string HomeIndex = "/Map/Index";
        private const string LoginPage = "/Login/Login";
        private const string DistributionPage = "/Login/Distribution";

        public MapController()
        {
            _dataManager = new DataManager();
        }

        // GET: Map
        [HttpGet]
        public ActionResult Index(double x = 0, double y = 0)
        {
            ViewBag.Map = true;
            if (SessionAccount.GetId() == null) return Redirect(LoginPage);
            var user = SessionAccount.GetCurretAccount();
            if (x == 0 && y == 0 || user?.Id != 0)
            {
                return View();
            }
            else
            {
                var points = _dataManager.PointManager.GetNearestPoints(x, y, user.Id);
                return View(points);
            }
        }

        [HttpPost]
        public string GetWay(string x, string y)
        {
            if (SessionAccount.GetId() == null) return null;
            var user = SessionAccount.GetCurretAccount();
            if (string.IsNullOrWhiteSpace(x) || string.IsNullOrWhiteSpace(y) || user == null || user.Id == 0)
            {
                return null;
            }
            else
            {
                var points = _dataManager.PointManager.GetNearestPoints(double.Parse(x), double.Parse(y), user.Id);

                return points;
            }
        }
        
        // POST: Map
        [HttpPost]
        public ActionResult Index(List<Point> points)
        {
            ViewBag.Map = true;

            return View(points);
        }
        [HttpPost]
        public ActionResult TaskOnMap(string idStr)
        {
            int id = int.Parse(idStr);
            ViewData.Model = _dataManager.PointManager.GetPoint(id);
            return PartialView();
        }
    }
}