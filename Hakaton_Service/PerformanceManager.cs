using Hakaton_Db;
using Hakaton_Db.Models;
using System;
using System.Linq;

namespace Hakaton_Service
{
    /// <inheritdoc />
    /// <summary>
    /// Класс для управления характеристиками пользователей
    /// </summary>
    public class PerformanceManager : Manager
    {
        /// <summary>
        /// Метод добавления новой характеристики
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string AddPerformance(string name)
        {
            var performance = DataContext.Performances
                .FirstOrDefault(
                    x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (performance != null)
            {
                return JsonManager.JsonError($"Характеристика {name} уже была добавлена в базу!");
            }

            performance = new Performance
            {
                Name = name
            };
            DataContext.Performances.Add(performance);
            DataContext.SaveChanges();

            return JsonManager.GetJsonString(performance);
        }

        /// <summary>
        /// Метод удаления характеристики
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string DeletePerformance(string name)
        {
            var performance = DataContext.Performances
                .FirstOrDefault(
                    x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (performance == null)
            {
                return JsonManager.JsonError($"Характеристика {name} не найдена!");
            }

            DataContext.Performances.Remove(performance);
            DataContext.SaveChanges();
            return JsonManager.GetJsonString(performance);
        }

        /// <summary>
        /// Метод добавления новой характеристики пользователю
        /// </summary>
        /// <param name="login"></param>
        /// <param name="perfName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public string AddPerformanceToUser(string login, string perfName, int level = 1)
        {
            var user = DataContext.Users
                .FirstOrDefault(
                    x => x.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase));
            if (user == null)
            {
                return JsonManager.JsonError($"Пользователь {login} не найден!");
            }

            var performance = DataContext.Performances
                .FirstOrDefault(
                    x => x.Name.Equals(perfName, StringComparison.CurrentCultureIgnoreCase));
            if (performance == null)
            {
                return JsonManager.JsonError($"Характеристика {perfName} не найдена!");
            }

            if (DataContext.UserPerformances
                    .FirstOrDefault(
                        x => x.User.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase) &&
                             x.Performance.Name.Equals(perfName, StringComparison.CurrentCultureIgnoreCase)) !=
                null)
            {
                return JsonManager.JsonError($"Характеристика {perfName} уже добавлена пользователю {login} !");
            }

            var userPerformance = new UserPerformance
            {
                User = user,
                Performance = performance,
                Level = level
            };
            DataContext.UserPerformances.Add(userPerformance);
            DataContext.SaveChanges();

            return JsonManager.GetJsonString(userPerformance);
        }

        /// <summary>
        /// Метод обновления характеристики пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="perfName"></param>
        /// <param name="newLevel"></param>
        /// <returns></returns>
        public string UpdatePerformanceToUser(string login, string perfName, int newLevel)
        {
            var user = DataContext.Users
                .FirstOrDefault(
                    x => x.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase));
            if (user == null)
            {
                return JsonManager.JsonError($"Пользователь {login} не найден!");
            }

            var performance = DataContext.Performances
                .FirstOrDefault(
                    x => x.Name.Equals(perfName, StringComparison.CurrentCultureIgnoreCase));
            if (performance == null)
            {
                return JsonManager.JsonError($"Характеристика {perfName} не найдена!");
            }

            var userPerformance = DataContext.UserPerformances
                .FirstOrDefault(
                    x => x.User.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase) &&
                         x.Performance.Name.Equals(perfName, StringComparison.CurrentCultureIgnoreCase));
            if (userPerformance ==
                null)
            {
                return JsonManager.JsonError($"Характеристика {perfName} не добавлена пользователю {login} !");
            }

            userPerformance.Level = newLevel;
            DataContext.SaveChanges();

            return JsonManager.GetJsonString(userPerformance);
        }

        public PerformanceManager(DataContext dataContext) : base(dataContext)
        {
        }
    }
}