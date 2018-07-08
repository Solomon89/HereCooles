using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Hakaton_Db.Models
{
    [Serializable]
    [DataContract(IsReference = false)]
    public class Performance
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public ICollection<UserPerformance> UserPerformances { get; set; } = new List<UserPerformance>();
        public ICollection<PerformancePoint> PerformancePoints { get; set; } = new List<PerformancePoint>();
    }
}