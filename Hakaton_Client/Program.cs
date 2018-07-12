using System;
using Hakaton_Service;

namespace Hakaton_Client
{
    internal class Program
    {
        private static void Main()
        {
            using (var dataManager = new DataManager())
            {
                var str = "";
                dataManager.UserManager.Register("Горшков Сергей Николаевич", "qwe@mail.ru", "123", ref str);
                var eventsForPoint = dataManager.EventManager.GetEventsForPoint("Ресторан Кухня Family");

                dataManager.PointManager.AddPointType("Кафе");
                var point = dataManager.PointManager.AddPoint("Solo", "desc", 45.048694, 41.982936, DateTime.Now, false,
                    "Кафе", new[] {"Гурман"});
                var point1 = dataManager.PointManager.AddPoint("Go! Вафли", "desc", 45.048014, 41.984812, DateTime.Now,
                    false,
                    "Кафе", new[] {"Гурман"});
                var point2 = dataManager.PointManager.AddPoint("Хлебная лавка", "desc", 45.047970, 41.984375,
                    DateTime.Now, false,
                    "Кафе", new[] {"Гурман"});
                var point3 = dataManager.PointManager.AddPoint("Rione", "desc", 45.038063, 41.977236, DateTime.Now,
                    false,
                    "Кафе", new[] {"Гурман"});
                var point4 = dataManager.PointManager.AddPoint("Sushi Time", "desc", 45.022342, 41.964276, DateTime.Now,
                    false,
                    "Кафе", new[] {"Гурман"});
                var point5 = dataManager.PointManager.AddPoint("Макдоналдс", "desc", 45.050131, 41.985521, DateTime.Now,
                    false,
                    "Кафе", new[] {"Гурман"});

                var test = dataManager.PointManager.GetNearestPoints(45.049027, 41.983806, 2);

                //const ulong appId = 6626908;
                //var apiManager = VkApiManager.GetInstance(appId, "sergik-gorshkov@mail.ru", "j97d53k1");

                //var value = apiManager.GetUserId().Value;

                //var byVk = dataManager.UserManager.RegisterByVk(apiManager.GetFio(), apiManager.GetAccessToken(),
                //    value);

                //var me = dataManager.UserManager.AuthenticateVk(value);
                //var friends = apiManager.GetFriends();
                //dataManager.PointManager.AddPointType("Культура");
                //dataManager.PerformanceManager.AddPerformance("Культура");
                //dataManager.PerformanceManager.AddPerformance("Спорт");
                //var point = dataManager.PointManager.AddPoint("test", "desc", 0, 0, DateTime.Now, false, "Культура",
                //    new[] {"Культура", "Спорт"});
                //var point1 = dataManager.PointManager.AddPoint("test2", "desc", 10, 10, DateTime.Now, false, "Культура",
                //    new[] {"Культура", "Спорт"});
                //var point2 = dataManager.PointManager.AddPoint("test3", "desc", 2, 2, DateTime.Now, false, "Культура",
                //    new[] {"Культура", "Спорт"});
                //var point3 = dataManager.PointManager.AddPoint("test4", "desc", 30, 30, DateTime.Now, false, "Культура",
                //    new[] {"Культура", "Спорт"});
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}