using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Hakaton_Db.Models
{
    [Serializable]
    [DataContract(IsReference = false)]
    public class Point
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }

        [DataMember]
        public DateTime DateCreate { get; set; }

        [DataMember]
        public PointType PointType { get; set; }

        [DataMember]
        public bool IsSpecial { get; set; }

        public ICollection<EventPoint> EventPoints { get; set; } = new List<EventPoint>();
        public ICollection<VisitedPoint> VisitedPoints { get; set; } = new List<VisitedPoint>();
        public ICollection<PerformancePoint> PerformancePoints { get; set; } = new List<PerformancePoint>();
    }
}