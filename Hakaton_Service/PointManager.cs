using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using Hakaton_Db;
using Hakaton_Db.Models;
using Hakaton_Service.SubModels;

namespace Hakaton_Service
{
    public class PointManager : Manager
    {
        public PointManager(DataContext dataContext) : base(dataContext)
        {
        }

        public SubPoint AddPoint(string name, string description, double x, double y, DateTime dateCreate,
            bool isSpecial,
            string pointType, string[] performances)
        {
            var point = DataContext.Points
                .FirstOrDefault(
                    p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (point != null) return null; //Точка {name} уже существует!

            var type = DataContext.PointTypes
                .FirstOrDefault(
                    p => p.Name.Equals(pointType, StringComparison.CurrentCultureIgnoreCase));
            if (type == null) return null; //Не найден тип {pointType} !

            var listPerf = DataContext.Performances.Where(p => performances.Contains(p.Name)).ToList();

            if (listPerf.Count != performances.Length) return null; //Не найдены характеристики для точки

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
            return subPoint;
        }

        public Point GetPoint(int id)
        {
            return DataContext.Points.Include(i => i.EventPoints).Include(i => i.PointType)
                .FirstOrDefault(i => i.Id == id);
        }

        public PointType AddPointType(string name)
        {
            var pointType = DataContext.PointTypes
                .FirstOrDefault(
                    x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (pointType != null) return null; //Тип {name} уже существует!
            pointType = new PointType
            {
                Name = name
            };
            DataContext.PointTypes.Add(pointType);
            DataContext.SaveChanges();
            return pointType;
        }

        public List<SubPoint> GetNearestPoints(double x, double y, long id, double radius = 1000)
        {
            CheckPoints();
            var user = DataContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null; //Пользователь {id} не найден!

            var userPerformances = DataContext.UserPerformances
                .Include(p => p.User)
                .Include(p => p.Performance)
                .Where(p => p.User.Id == id && p.Level > 0)
                .ToList();
            if (!userPerformances.Any()) return null;

            var performancePoints =
                DataContext.PerformancePoints
                    .Include(p => p.Performance)
                    .Include(p => p.Point)
                    .Include(p => p.Point.PointType)
                    .Include(p => p.Point.EventPoints)
                    .ToList()
                    .Where(p => userPerformances.Exists(u => u.Performance.Id == p.Performance.Id))
                    .ToList();
            if (!performancePoints.Any()) return null;

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
            points.Add(new SubPoint
            {
                Name = "Я",
                Description = "Тут находишься ты",
                X = x,
                Y = y
            });
            var sortWayPoints = SortWayPoints(points, x, y);
            return sortWayPoints;
        }

        private List<SubPoint> SortWayPoints(List<SubPoint> points, double x, double y)
        {
            var lastGeo = new GeoCoordinate(x, y);
            var lastPoint = new SubPoint();
            var isOver = false;
            var minDist = double.MaxValue;
            var list = new List<SubPoint>();
            while (!isOver)
            {
                isOver = true;
                foreach (var point in points)
                {
                    var distanceTo = lastGeo.GetDistanceTo(new GeoCoordinate(point.X, point.Y));
                    if (distanceTo < minDist)
                    {
                        isOver = false;
                        minDist = distanceTo;
                        lastPoint = point;
                    }
                }

                if (!isOver)
                {
                    list.Add(lastPoint);
                    points.Remove(lastPoint);
                    minDist = double.MaxValue;
                }
            }

            return list;
        }

        private void CheckPoints()
        {
            AddPointType("Кафе");
            AddPointType("Кинотеатр");
            AddPointType("Музей");
            AddPointType("Бар");
            AddPointType("Книжный магазин");

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
            AddPoint("Ресторан Кухня Family", "Европейская, итальянская, русская, японская, домашняя, смешанная",
                45.044981, 41.975774, DateTime.Now, false, "Кафе",
                new[] {"Гурман"});

            AddPoint("Кино Max", "Кинотеатр",
                45.050106, 41.985191, DateTime.Now, false, "Кинотеатр",
                new[] {"Интеллигент"});
            AddPoint("Моя Страна. Моя История", "Музей",
                45.048483, 41.982150, DateTime.Now, false, "Музей",
                new[] {"Интеллигент"});

            AddPoint("Бар XxxX", "Бар, паб, ночной клуб",
                45.050035, 41.983132, DateTime.Now, false, "Бар",
                new[] {"Алкаш"});

            AddPoint("Wacko shop", "Книжный магазиндетские игрушки и игры",
                45.050129, 41.985246, DateTime.Now, false, "Книжный магазин",
                new[] {"Гик"});
        }
    }
}