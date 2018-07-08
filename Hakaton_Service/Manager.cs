using Hakaton_Db;

namespace Hakaton_Service
{
    /// <summary>
    /// Общий класс с дата контекстом
    /// </summary>
    public abstract class Manager
    {
        protected DataContext DataContext { get; set; }

        protected Manager(DataContext dataContext)
        {
            DataContext = dataContext;
        }
    }
}