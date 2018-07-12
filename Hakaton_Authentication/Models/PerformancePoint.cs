using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Hakaton_Db.Models
{
    [Serializable]
    [DataContract(IsReference = false)]
    public class PerformancePoint
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public Performance Performance { get; set; }

        [DataMember]
        public Point Point { get; set; }
    }
}