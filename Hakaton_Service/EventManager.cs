using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Hakaton_Db;
using Hakaton_Db.Models;

namespace Hakaton_Service
{
    public class EventManager : Manager
    {
        public EventManager(DataContext dataContext) : base(dataContext)
        {
        }

        public EventPoint AddEventPoint(string name, string description, DateTime createDate, decimal scoreAward,
            string pointName, string eventPointType, bool isPermanent, DateTime? timeLeft = null)
        {
            var point = DataContext.Points
                .FirstOrDefault(
                    x => x.Name.Equals(pointName, StringComparison.CurrentCultureIgnoreCase));
            if (point == null) return null; //Точка {pointName} не создана!
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
                EventPointType = pointType,
                Description = description
            };
            if (DataContext.EventPoints.ToList()
                .Exists(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) &&
                             x.Description.Equals(description, StringComparison.CurrentCultureIgnoreCase)))
                return null; //Событие {name} уже существует!

            DataContext.EventPoints.Add(eventPoint);
            DataContext.SaveChanges();
            return eventPoint;
        }

        public EventPointType AddEventPointType(string name)
        {
            var pointType = DataContext.EventPointTypes
                .FirstOrDefault(
                    x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (pointType != null) return null; //Тип события {name} уже существует!
            pointType = new EventPointType
            {
                Name = name
            };
            DataContext.EventPointTypes.Add(pointType);
            DataContext.SaveChanges();
            return pointType;
        }

        public List<EventPoint> GetEventsForPoint(string pointName)
        {
            CheckEventPoints();
            var eventPoints = DataContext.EventPoints
                .Include(x => x.Point)
                .Include(x => x.EventPointType)
                .Where(x => x.Point.Name.Equals(pointName, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
            return eventPoints;
        }

        public List<EventPoint> GetEventsForPoint(long pointId)
        {
            CheckEventPoints();
            var eventPoints = DataContext.EventPoints
                .Include(x => x.Point)
                .Include(x => x.EventPointType)
                .Where(x => x.Point.Id == pointId)
                .ToList();
            return eventPoints;
        }

        private void CheckEventPoints()
        {
            AddEventPointType("Фото");
            AddEventPointType("Покупка");

            AddEventPoint("Бронзовый век",
                "Внутри ресторана есть особая стена. Найди её, пришли нам снимок и " +
                "получи свои заслуженные баллы, которые потом сможешь потратить.",
                DateTime.Now, 5, "Ресторан Кухня Family", "Фото", true);
            AddEventPoint("Заходи на обед",
                "Пора перекусить! Получи 50 баллов за каждый заказанный салат в чеке. " +
                "Чем больше баллов — тем ближе следующий уровень.",
                DateTime.Now, 50, "Ресторан Кухня Family", "Покупка", true);
        }
    }
}