﻿using System.Collections.Generic;
using System.Web.Mvc;
using Hakaton_Db.Models;
using Hakaton_Service;
using Hakaton_View.Controllers.Manage;

namespace Hakaton_View.Controllers
{
    public class LoginController : Controller
    {
        private const string HomeIndex = "/Map/Index";
        private const string LoginPage = "/Login/Login";
        private const string DistributionPage = "/Login/Distribution";
        private readonly DataManager _dataManager;

        public LoginController()
        {
            _dataManager = new DataManager();
            var verifyAccount = SessionAccount.VerifyAccount();
            var user = _dataManager.UserManager.Authenticate(verifyAccount);
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
            //if (SessionAccount.GetId() == null) return Redirect(HomeIndex);
            user = _dataManager.UserManager.Authenticate(user.Login, user.Password);
            if (user != null)
            {
                SessionAccount.AuthenticateAccount(user);

                ViewBag.Message =
                    $"Добро пожаловать, {SessionAccount.GetFio()}!";

                return Redirect(DistributionPage);
            }

            ViewBag.Message = "Не верный логин или пароль";
            return View();
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
            var message = "";
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

        [HttpGet]
        public ActionResult Distribution()
        {
            if (SessionAccount.GetId() == null) return Redirect(LoginPage);
            var user = SessionAccount.GetCurretAccount();

            if (user == null) user = new User();

            return View(user);
        }

        [HttpPost]
        public ActionResult Distribution(int range1, int range2, int range3, int range4, int range5)
        {
            if (SessionAccount.GetId() == null) return Redirect(LoginPage);
            var user = SessionAccount.GetCurretAccount();
            if (user == null || user.Id == 0)
            {
                user = new User();
            }
            else
            {
                var dictionary = new Dictionary<string, int>();
                dictionary.Add("Интеллигент", range1);
                dictionary.Add("Шопоголик", range2);
                dictionary.Add("Гик", range3);
                dictionary.Add("Гурман", range4);
                dictionary.Add("Алкаш", range5);
                _dataManager.UserManager.SetPerformancesUser(user.Id, dictionary);
            }

            return Redirect(HomeIndex);
        }

        public ActionResult ProfileInfo()
        {
            ViewBag.Map = true;
            return View();
        }
    }
}