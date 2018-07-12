using Hakaton_Db;

namespace Hakaton_Service
{
    /// <summary>
    ///     Общий класс с дата контекстом
    /// </summary>
    public abstract class Manager
    {
        protected Manager(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        protected DataContext DataContext { get; set; }
    }
}