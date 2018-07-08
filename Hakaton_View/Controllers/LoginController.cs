﻿using Hakaton_Db.Models;
using Hakaton_Service;
using Hakaton_View.Controllers.Manage;
using System.Web.Mvc;

namespace Hakaton_View.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataManager _dataManager;
        private const string HomeIndex = "/Home/Index";

        public LoginController()
        {
            _dataManager = new DataManager();
            var verifyAccount = SessionAccount.VerifyAccount();
            var userStr = _dataManager.UserManager.Authenticate(verifyAccount);
            var user = JsonManager.FromJson<User>(userStr);
            SessionAccount.AuthenticateAccount(user);
        }

        // GET: Login
        public ActionResult Login()
        {
            ViewData.Model = new User();
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            user = _dataManager.UserManager.Authenticate(user.Login, user.Password);
            if (user != null)
            {
                SessionAccount.AuthenticateAccount(user);

                ViewBag.Message =
                    $"Добро пожаловать, {SessionAccount.GetFio()}!";

                return Redirect(Request.UrlReferrer?.AbsolutePath ?? HomeIndex);
            }
            else
            {
                ViewBag.Message = "Не верный логин или пароль";
                return View();
            }
        }

        public RedirectResult LogOut()
        {
            SessionAccount.DeAuthenticateAccount();
            return Redirect(Request.UrlReferrer?.AbsolutePath ?? HomeIndex);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            //if (SessionAccount.GetId() != null) return Redirect(HomeIndex);
            return View(new User());
        }

        [HttpPost]
        public ActionResult Registration(User user)
        {
            //if (SessionAccount.GetId() != null) return Redirect(HomeIndex);
            string message = "";
            user = _dataManager.UserManager.Register(user.Fio, user.Login, user.Password, ref message);
            if (user == null)
            {
                ViewBag.Message = message;
                return View();
            }

            SessionAccount.AuthenticateAccount(user);
            TempData["sAlertMessage"] = $"Добро пожаловать, {SessionAccount.GetFio()}!";
            return Redirect("/Account/ProfilePerson");
        }
    }
}