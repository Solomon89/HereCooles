using Hakaton_Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hakaton_View.Controllers.Manage
{
    public static class SessionAccount
    {
        private static List<SessionInfo> Accounts { get; set; } = new List<SessionInfo>();

        public static long? GetId() => CurrentSession()?.Account.Id;

        public static string GetFio() => CurrentSession()?.Account.Fio;

        public static User GetCurretAccount() => CurrentSession()?.Account;

        public static long VerifyAccount()
        {
            if (HttpContext.Current.Request.Cookies["Login"] == null) return 0;

            var accountInfo =
                Encoding.UTF8.GetString(Convert.FromBase64String(HttpContext.Current.Request.Cookies["Login"].Value))
                    .Split(' ');
            return HttpContext.Current.Request.UserHostAddress == accountInfo[2] ? long.Parse(accountInfo[0]) : 0;
        }

        public static void AuthenticateAccount(User user)
        {
            var curAcc = CurrentSession();
            if (curAcc != null || user == null) return;
            Accounts.Add(new SessionInfo()
            {
                Account = user,
                SessionId = Accounts.Count + 1,
                TimeOpened = DateTime.Now
            });
            var ip = HttpContext.Current.Request.UserHostAddress;
            var sBuilder =
                new StringBuilder(
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id + " " + user.Login + " " + ip)));

            HttpContext.Current.Response.SetCookie(new HttpCookie("Login", sBuilder.ToString()));
        }

        public static void DeAuthenticateAccount()
        {
            var curAcc = CurrentSession();
            if (curAcc == null) return;
            Accounts.Remove(curAcc);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("Login")
            {
                Expires = DateTime.Now.AddDays(-1d)
            });
        }

        public static void UpdateAccount(User user)
        {
            CurrentSession().Account = user;
        }

        private static SessionInfo CurrentSession()
        {
            var idAcc = HttpContext.Current.Request.Cookies["Login"] != null
                ? long.Parse(Encoding.UTF8
                    .GetString(Convert.FromBase64String(HttpContext.Current.Request.Cookies["Login"].Value))
                    .Split(' ')[0])
                : 0;
            return Accounts.FirstOrDefault(x => x.Account.Id == idAcc);
        }
    }

    internal class SessionInfo
    {
        public User Account { get; set; }
        public DateTime TimeOpened { get; set; }
        public int SessionId { get; set; }
    }
}