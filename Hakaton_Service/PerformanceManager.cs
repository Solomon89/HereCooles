using System;
using System.Linq;
using Hakaton_Db;
using Hakaton_Db.Models;

namespace Hakaton_Service
{
    /// <inheritdoc />
    /// <summary>
    ///     Класс для управления характеристиками пользователей
    /// </summary>
    public class PerformanceManager : Manager
    {
        public PerformanceManager(DataContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        ///     Метод добавления новой характеристики
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Performance AddPerformance(string name)
        {
            var performance = DataContext.Performances
                .FirstOrDefault(
                    x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (performance != null) return null; //Характеристика {name} уже была добавлена в базу!

            performance = new Performance
            {
                Name = name
            };
            DataContext.Performances.Add(performance);
            DataContext.SaveChanges();

            return performance;
        }

        /// <summary>
        ///     Метод удаления характеристики
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Performance DeletePerformance(string name)
        {
            var performance = DataContext.Performances
                .FirstOrDefault(
                    x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (performance == null) return null; //Характеристика {name} не найдена!

            DataContext.Performances.Remove(performance);
            DataContext.SaveChanges();
            return performance;
        }

        /// <summary>
        ///     Метод добавления новой характеристики пользователю
        /// </summary>
        /// <param name="login"></param>
        /// <param name="perfName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public UserPerformance AddPerformanceToUser(string login, string perfName, int level = 1)
        {
            var user = DataContext.Users
                .FirstOrDefault(
                    x => x.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase));
            if (user == null) return null; //Пользователь {login} не найден!

            var performance = DataContext.Performances
                .FirstOrDefault(
                    x => x.Name.Equals(perfName, StringComparison.CurrentCultureIgnoreCase));
            if (performance == null) return null; //Характеристика {perfName} не найдена!

            if (DataContext.UserPerformances
                    .FirstOrDefault(
                        x => x.User.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase) &&
                             x.Performance.Name.Equals(perfName, StringComparison.CurrentCultureIgnoreCase)) !=
                null)
                return null; //Характеристика {perfName} уже добавлена пользователю {login} !

            var userPerformance = new UserPerformance
            {
                User = user,
                Performance = performance,
                Level = level
            };
            DataContext.UserPerformances.Add(userPerformance);
            DataContext.SaveChanges();

            return userPerformance;
        }

        /// <summary>
        ///     Метод обновления характеристики пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="perfName"></param>
        /// <param name="newLevel"></param>
        /// <returns></returns>
        public UserPerformance UpdatePerformanceToUser(string login, string perfName, int newLevel)
        {
            var user = DataContext.Users
                .FirstOrDefault(
                    x => x.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase));
            if (user == null) return null; //Пользователь {login} не найден!

            var performance = DataContext.Performances
                .FirstOrDefault(
                    x => x.Name.Equals(perfName, StringComparison.CurrentCultureIgnoreCase));
            if (performance == null) return null; //Характеристика {perfName} не найдена!

            var userPerformance = DataContext.UserPerformances
                .FirstOrDefault(
                    x => x.User.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase) &&
                         x.Performance.Name.Equals(perfName, StringComparison.CurrentCultureIgnoreCase));
            if (userPerformance ==
                null)
                return null; //Характеристика {perfName} не добавлена пользователю {login} !

            userPerformance.Level = newLevel;
            DataContext.SaveChanges();

            return userPerformance;
        }
    }
}