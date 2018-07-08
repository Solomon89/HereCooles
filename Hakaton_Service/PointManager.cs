using Hakaton_Db;
using Hakaton_Db.Models;
using Hakaton_Service.SubModels;
using System;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;

namespace Hakaton_Service
{
    public class PointManager : Manager
    {
        public PointManager(DataContext dataContext) : base(dataContext)
        {
        }

        public string AddPoint(string name, string description, double x, double y, DateTime dateCreate, bool isSpecial,
            string pointType, string[] performances)
        {
            var point = DataContext.Points
                .FirstOrDefault(
                    p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (point != null) return JsonManager.JsonError($"Точка {name} уже существует!");

            var type = DataContext.PointTypes
                .FirstOrDefault(
                    p => p.Name.Equals(pointType, StringComparison.CurrentCultureIgnoreCase));
            if (type == null) return JsonManager.JsonError($"Не найден тип {pointType} !");

            var listPerf = DataContext.Performances.Where(p => performances.Contains(p.Name)).ToList();

            if (listPerf.Count != performances.Length)
            {
                return JsonManager.JsonError("Не найдены характеристики для точки");
            }

            point = new Point
            {
                Name = name,
                PointType = type,
                DateCreate = dateCreate,
                IsSpecial = isSpecial,
                X = x,
                Y = y,
                Description = description
            };
            DataContext.Points.Add(point);
            DataContext.SaveChanges();

            foreach (var performance in listPerf)
            {
                var perfPoint = new PerformancePoint
                {
                    Performance = performance,
                    Point = point
                };
                DataContext.PerformancePoints.Add(perfPoint);
            }

            DataContext.SaveChanges();

            var subPoint = new SubPoint
            {
                Name = point.Name,
                Description = point.Description,
                Id = point.Id,
                Y = point.Y,
                X = point.X,
                PointType = type,
                PerformancePoint =
                    DataContext.PerformancePoints.Include(p => p.Point).First(p => p.Point.Id == point.Id)
            };
            return JsonManager.GetJsonString(subPoint);
        }

        public string AddPointType(string name)
        {
            var pointType = DataContext.PointTypes
                .FirstOrDefault(
                    x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (pointType != null) return JsonManager.JsonError($"Тип {name} уже существует!");
            pointType = new PointType
            {
                Name = name
            };
            DataContext.PointTypes.Add(pointType);
            DataContext.SaveChanges();
            return JsonManager.GetJsonString(pointType);
        }

        public string GetNearestPoints(double x, double y, long id, double radius = 1000)
        {
            CheckPoints();
            var user = DataContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return JsonManager.JsonError($"Пользователь {id} не найден!");

            var userPerformances = DataContext.UserPerformances
                .Include(p => p.User)
                .Include(p => p.Performance)
                .Where(p => p.User.Id == id)
                .ToList();
            var maxLvl = userPerformances.Max(m => m.Level);
            var performance = userPerformances.FirstOrDefault(p => p.Level == maxLvl);

            var performancePoints =
                DataContext.PerformancePoints
                    .Include(p => p.Performance)
                    .Include(p => p.Point)
                    .Include(p => p.Point.PointType)
                    .Include(p => p.Point.EventPoints)
                    .Where(p => p.Performance.Id == performance.Performance.Id)
                    .ToList();

            var geoMe = new GeoCoordinate(x, y);
            var points = performancePoints
                .Where(p =>
                {
                    //Расстояние в метрах
                    var distanceTo = geoMe.GetDistanceTo(new GeoCoordinate(p.Point.X, p.Point.Y));
                    return distanceTo < radius;
                })
                .Select(p =>
                    new SubPoint
                    {
                        Name = p.Point.Name,
                        Description = p.Point.Description,
                        Id = p.Point.Id,
                        Y = p.Point.Y,
                        X = p.Point.X,
                        PerformancePoint =
                            DataContext.PerformancePoints.Include(per => per.Point)
                                .First(per => per.Point.Id == p.Point.Id),
                        PointType = p.Point.PointType,
                        SubEvents = p.Point.EventPoints.Select(e => new SubEvent
                        {
                            Name = e.Name,
                            DateCreate = e.DateCreate,
                            TimeLeft = e.TimeLeft,
                            Id = e.Id,
                            IsPermanent = e.IsPermanent,
                            ScoreAward = e.ScoreAward
                        }).ToList()
                    })
                .ToList();

            return JsonManager.GetJsonString(points);
        }

        private void CheckPoints()
        {
            if (!DataContext.PointTypes.Any(x => x.Name == "Кафе"))
            {
                AddPointType("Кафе");
            }

            var arrKafe = new[] {"Solo", "Go! Вафли", "Хлебная лавка", "Rione", "Sushi Time", "Макдоналдс"};

            if (!DataContext.Points.All(x => arrKafe.Contains(x.Name)))
            {
                AddPoint("Solo", "Караоке-клуб Solo", 45.048694, 41.982936, DateTime.Now, false, "Кафе",
                    new[] {"Гурман"});
                AddPoint("Go! Вафли", "Go! Вафли, Кафе, кофейня, кондитерская", 45.048014, 41.984812, DateTime.Now,
                    false, "Кафе", new[] {"Гурман"});
                AddPoint("Хлебная лавка", "Булочная, пекарня", 45.047970, 41.984375, DateTime.Now, false, "Кафе",
                    new[] {"Гурман"});
                AddPoint("Rione", "Пиццерия, кафе, доставка еды, бизнес-ланч, кофе с собой", 45.038063, 41.977236,
                    DateTime.Now, false, "Кафе", new[] {"Гурман"});
                AddPoint("Sushi Time", "Доставка еды и обедов, пиццерия", 45.022342, 41.964276, DateTime.Now, false,
                    "Кафе", new[] {"Гурман"});
                AddPoint("Макдоналдс", "Быстрое питание, sресторан", 45.050131, 41.985521, DateTime.Now, false, "Кафе",
                    new[] {"Гурман"});
            }
        }
    }
}