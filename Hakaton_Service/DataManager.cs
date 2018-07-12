using System;
using Hakaton_Db;

namespace Hakaton_Service
{
    /// <summary>
    ///     Класс доступа к БД
    /// </summary>
    public class DataManager : IDisposable
    {
        private readonly DataContext _dataContext;
        private EventManager _eventManager;
        private PerformanceManager _performanceManager;
        private PointManager _pointManager;
        private UserManager _userManager;

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