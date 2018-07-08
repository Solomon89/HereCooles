using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hakaton_Models
{
    /// <summary>
    /// Модель таблицы Users
    /// </summary>
    public class User
    {
        [Key]
        public long Id { get; set; }

        public string Fio { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public ICollection<UserPerformance> UserPerformances { get; set; } = new List<UserPerformance>();
    }
}