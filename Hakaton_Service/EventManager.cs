using Hakaton_Db;
using Hakaton_Db.Models;
using System;
using System.Linq;

namespace Hakaton_Service
{
    public class EventManager : Manager
    {
        public EventManager(DataContext dataContext) : base(dataContext)
        {
        }

        public string AddEventPoint(string name, DateTime createDate, decimal scoreAward, string pointName,
            string eventPointType, bool isPermanent, DateTime? timeLeft = null)
        {
            var point = DataContext.Points
                .FirstOrDefault(
                    x => x.Name.Equals(pointName, StringComparison.CurrentCultureIgnoreCase));
            if (point == null) return JsonManager.JsonError($"Точка {pointName} не создана!");
            var pointType = DataContext.EventPointTypes
                .FirstOrDefault(
                    x => x.Name.Equals(eventPointType, StringComparison.CurrentCultureIgnoreCase));
            var eventPoint = new EventPoint
            {
                Name = name,
                Point = point,
                DateCreate = createDate,
                IsPermanent = isPermanent,
                TimeLeft = timeLeft,
                ScoreAward = scoreAward,
                EventPointType = pointType
            };
            if (DataContext.EventPoints.Contains(eventPoint, new EventPointComparer()))
            {
                return JsonManager.JsonError($"Событие {name} уже существует!");
            }

            DataContext.EventPoints.Add(eventPoint);
            DataContext.SaveChanges();
            return JsonManager.GetJsonString(eventPoint);
        }

        public string AddEventPointType(string name)
        {
            var pointType = DataContext.EventPointTypes
                .FirstOrDefault(
                    x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (pointType != null) return JsonManager.JsonError($"Тип события {name} уже существует!");
            pointType = new EventPointType
            {
                Name = name
            };
            DataContext.EventPointTypes.Add(pointType);
            DataContext.SaveChanges();
            return JsonManager.GetJsonString(pointType);
        }
    }
}