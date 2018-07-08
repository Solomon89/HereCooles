using System;
using System.Data.Common;
using Hakaton_Service;

namespace Hakaton_Client
{
    class Program
    {
        static void Main()
        {
            using (var dataManager = new DataManager())
            {
                const ulong appId = 6626908;
                var apiManager = VkApiManager.GetInstance(appId, "sergik-gorshkov@mail.ru", "j97d53k1");

                var value = apiManager.GetUserId().Value;

                var byVk = dataManager.UserManager.RegisterByVk(apiManager.GetFio(), apiManager.GetAccessToken(),
                    value);

                var me = dataManager.UserManager.AuthenticateVk(value);
                var friends = apiManager.GetFriends();
                dataManager.PointManager.AddPointType("Культура");
                dataManager.PerformanceManager.AddPerformance("Культура");
                dataManager.PerformanceManager.AddPerformance("Спорт");
                var point = dataManager.PointManager.AddPoint("test", "desc", 0, 0, DateTime.Now, false, "Культура",
                    new[] {"Культура", "Спорт"});
                var point1 = dataManager.PointManager.AddPoint("test2", "desc", 10, 10, DateTime.Now, false, "Культура",
                    new[] {"Культура", "Спорт"});
                var point2 = dataManager.PointManager.AddPoint("test3", "desc", 2, 2, DateTime.Now, false, "Культура",
                    new[] {"Культура", "Спорт"});
                var point3 = dataManager.PointManager.AddPoint("test4", "desc", 30, 30, DateTime.Now, false, "Культура",
                    new[] {"Культура", "Спорт"});
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}