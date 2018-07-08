using Hakaton_Db;
using Hakaton_Db.Models;
using System;
using System.Data.Entity;
using System.Linq;
using Hakaton_Service.SubModels;

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

            return JsonManager.GetJsonString(new SubPoint
            {
                Name = point.Name,
                Description = point.Description,
                Id = point.Id,
                Y = point.Y,
                X = point.X
            });
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

        public string GetNearestPoints(double x, double y, long id, double radius = 1)
        {
            var min = DataContext.Points.Min(p => CalcDistance(x, y, p.X, p.Y));
            var user = DataContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return JsonManager.JsonError($"Пользователь {id} не найден!");

            var userPerformances = DataContext.UserPerformances
                .Include(p => p.User)
                .Where(p => p.User.Id == id)
                .ToList();
            var maxLvl = userPerformances.Max(m => m.Level);
            var performance = userPerformances.FirstOrDefault(p => p.Level == maxLvl);

            var performancePoints =
                DataContext.PerformancePoints
                    .Include(p => p.Performance)
                    .Include(p => p.Point)
                    .Include(p => p.Point.EventPoints)
                    .Where(p => p.Performance.Id == performance.Id)
                    .ToList();

            var points = performancePoints.Where(p => Math.Abs(CalcDistance(x, y, p.Point.X, p.Point.Y) - min) < radius)
                .Select(p => new SubPoint
                {
                    Name = p.Point.Name,
                    Description = p.Point.Description,
                    Id = p.Point.Id,
                    Y = p.Point.Y,
                    X = p.Point.X,
                    SubEvents = p.Point.EventPoints.Select(e => new SubEvent
                    {
                        Name = e.Name,
                        DateCreate = e.DateCreate,
                        TimeLeft = e.TimeLeft,
                        Id = e.Id,
                        IsPermanent = e.IsPermanent,
                        ScoreAward = e.ScoreAward
                    }).ToList()
                }).ToList();

            return JsonManager.GetJsonString(points);
        }

        /// <summary>
        /// Расстояние в километрах
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        private double CalcDistance(double x1, double y1, double x2, double y2)
        {
            const double radEarth = 6371.008;
            var acos = Math.Acos(Math.Sin(x1) * Math.Sin(x2) + Math.Cos(x1) * Math.Cos(x2) * Math.Cos(y1 - y2));
            return radEarth * acos;
        }
    }
}