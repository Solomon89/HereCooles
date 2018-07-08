using System.ComponentModel.DataAnnotations;

namespace Hakaton_Models
{
    /// <summary>
    /// Характеристики людей
    /// </summary>
    public class UserPerformance
    {
        [Key]
        public long Id { get; set; }

        public User User { get; set; }
        public Performance Performance { get; set; }

        /// <summary>
        /// Уровень характеристики
        /// </summary>
        public int Level { get; set; }
    }
}