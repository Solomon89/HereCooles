using Hakaton_Db;
using System;

namespace Hakaton_Service
{
    /// <summary>
    /// Класс доступа к БД
    /// </summary>
    public class DataManager : IDisposable
    {
        private readonly DataContext _dataContext;
        private PerformanceManager _performanceManager;
        private UserManager _userManager;
        private PointManager _pointManager;
        private EventManager _eventManager;

        public DataManager()
        {
            _dataContext = new DataContext();
        }

        public PerformanceManager PerformanceManager => _performanceManager ?? new PerformanceManager(_dataContext);

        public UserManager UserManager => _userManager ?? new UserManager(_dataContext);

        public PointManager PointManager => _pointManager ?? new PointManager(_dataContext);
        public EventManager EventManager => _eventManager ?? new EventManager(_dataContext);

        public void Dispose()
        {
            _dataContext?.Dispose();
        }
    }
}