using Hakaton_Db;
using Hakaton_Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using User = Hakaton_Db.Models.User;

namespace Hakaton_Service
{
    public class UserManager : Manager
    {
        public UserManager(DataContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Метод регистрации
        /// </summary>
        /// <param name="fio"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Register(string fio, string login, string password,ref string message)
        {
            var user = DataContext.Users
                .FirstOrDefault(
                    x => x.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase));
            if (user != null)
            {
                message = $"Пользователь с ником {login} уже зарегистрирован";
                return null;
            }
            user = new User
            {
                Fio = fio,
                Login = login,
                Password = password,
                Scores = 0
            };
            DataContext.Users.Add(user);
            DataContext.SaveChanges();
            return user;
        }

        public string RegisterByVk(string fio, string token, long userId)
        {
            var userDb = DataContext.Users
                .FirstOrDefault(
                    x => x.Fio == fio && x.UserIdVk == userId);
            if (userDb != null) return JsonManager.JsonError($"Пользователь {userId} уже зарегистрирован в базе!");
            userDb = new User
            {
                Fio = fio,
                AccessTokenVk = token,
                UserIdVk = userId,
                Scores = 0
            };
            DataContext.Users.Add(userDb);
            DataContext.SaveChanges();
            return JsonManager.GetJsonString(userDb);
        }

        /// <summary>
        /// Метод аутентификации
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Authenticate(string login, string password)
        {
            var user = DataContext.Users.FirstOrDefault(x =>
                x.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase) &&
                x.Password == password);
            return user;
        }

        public string AuthenticateVk(long userId)
        {
            var user = DataContext.Users
                .FirstOrDefault(x => x.UserIdVk == userId);
            return user == null
                ? JsonManager.JsonError($"Пользователь {userId} не зарегистрирован!")
                : JsonManager.GetJsonString(user);
        }

        public string Authenticate(long userId)
        {
            var user = DataContext.Users
                .FirstOrDefault(x => x.Id == userId);
            return user == null
                ? JsonManager.JsonError($"Пользователь {userId} не зарегистрирован!")
                : JsonManager.GetJsonString(user);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public string DeleteUser(string login)
        {
            var user = DataContext.Users.FirstOrDefault(x =>
                x.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase));
            if (user == null)
            {
                return JsonManager.JsonError($"Пользователь {login} не найден!");
            }

            DataContext.Users.Remove(user);
            DataContext.SaveChanges();

            return JsonManager.GetJsonString(user);
        }

        public void SetPerformancesUser(long userId, List<(string, int)> perfList)
        {
            var user = DataContext.Users.First(x => x.Id == userId);
            foreach (var tuple in perfList)
            {
                var performance = DataContext.Performances.First(x => x.Name.Equals(tuple.Item1));
                var userPerformance = new UserPerformance
                {
                    Performance = performance,
                    Level = tuple.Item2,
                    User = user
                };
                DataContext.UserPerformances.Add(userPerformance);
            }

            DataContext.SaveChanges();
        }
    }
}