using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Hakaton_Db.Models
{
    [Serializable]
    [DataContract(IsReference = false)]
    public class PointType
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public ICollection<Point> Points { get; set; } = new List<Point>();
    }
}