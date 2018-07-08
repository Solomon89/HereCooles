using Hakaton_Db.Models;
using System.Data.Common;
using System.Data.Entity;

namespace Hakaton_Db
{
    /// <inheritdoc />
    /// <summary>
    /// Класс соединения приложения с БД
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext() : base("DbConnection")
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<DataContext>(null);
        //    base.OnModelCreating(modelBuilder);
        //}

        /// <summary>
        /// Таблица Users
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица Performances
        /// </summary>
        public DbSet<Performance> Performances { get; set; }

        /// <summary>
        /// Характеристики пользователей
        /// </summary>
        public DbSet<UserPerformance> UserPerformances { get; set; }

        public DbSet<Point> Points { get; set; }
        public DbSet<PointType> PointTypes { get; set; }
        public DbSet<EventPoint> EventPoints { get; set; }
        public DbSet<EventPointType> EventPointTypes { get; set; }
        public DbSet<VisitedPoint> VisitedPoints { get; set; }
        public DbSet<PerformancePoint> PerformancePoints { get; set; }
    }
}