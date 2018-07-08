using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hakaton_Models
{
    public class Performance
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserPerformance> UserPerformances { get; set; } = new List<UserPerformance>();
    }
}