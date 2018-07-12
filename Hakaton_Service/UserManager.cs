using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Hakaton_Db;
using Hakaton_Db.Models;

namespace Hakaton_Service
{
    public class UserManager : Manager
    {
        public UserManager(DataContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        ///     Метод регистрации
        /// </summary>
        /// <param name="fio"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Register(string fio, string login, string password, ref string message)
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

        public User RegisterByVk(string fio, string token, long userId)
        {
            var userDb = DataContext.Users
                .FirstOrDefault(
                    x => x.Fio == fio && x.UserIdVk == userId);
            if (userDb != null) return null; //Пользователь {userId} уже зарегистрирован в базе!
            userDb = new User
            {
                Fio = fio,
                AccessTokenVk = token,
                UserIdVk = userId,
                Scores = 0
            };
            DataContext.Users.Add(userDb);
            DataContext.SaveChanges();
            return userDb;
        }

        /// <summary>
        ///     Метод аутентификации
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

        public User AuthenticateVk(long userId)
        {
            var user = DataContext.Users
                .FirstOrDefault(x => x.UserIdVk == userId);
            return user;
        }

        public User Authenticate(long userId)
        {
            var user = DataContext.Users
                .FirstOrDefault(x => x.Id == userId);
            return user;
        }

        /// <summary>
        ///     Удаление пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public User DeleteUser(string login)
        {
            var user = DataContext.Users.FirstOrDefault(x =>
                x.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase));
            if (user == null) return null; //Пользователь {login} не найден!

            DataContext.Users.Remove(user);
            DataContext.SaveChanges();

            return user;
        }

        public void SetPerformancesUser(long userId, Dictionary<string, int> perfList)
        {
            CheckPerfomance();
            var user = DataContext.Users.First(x => x.Id == userId);
            foreach (var item in perfList)
            {
                var performance = DataContext.Performances.First(x => x.Name.Equals(item.Key));
                var userPerformance = new UserPerformance
                {
                    Performance = performance,
                    Level = item.Value,
                    User = user
                };
                var usrPerf = DataContext.UserPerformances
                    .Include(x => x.User)
                    .Include(x => x.Performance)
                    .FirstOrDefault(x => x.User.Id == userId && x.Performance.Id == performance.Id);
                if (usrPerf == null)
                    DataContext.UserPerformances.Add(userPerformance);
                else
                    usrPerf.Level = item.Value;
            }

            DataContext.SaveChanges();
        }

        /// <summary>
        ///     Инциализация свойств человека
        /// </summary>
        private void CheckPerfomance()
        {
            if (!DataContext.Performances.Any())
            {
                DataContext.Performances.Add(new Performance {Name = "Интеллигент"});
                DataContext.Performances.Add(new Performance {Name = "Шопоголик"});
                DataContext.Performances.Add(new Performance {Name = "Гик"});
                DataContext.Performances.Add(new Performance {Name = "Гурман"});
                DataContext.Performances.Add(new Performance {Name = "Алкаш"});
                DataContext.SaveChanges();
            }
        }
    }
}