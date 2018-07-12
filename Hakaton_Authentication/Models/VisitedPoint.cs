using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Hakaton_Db.Models
{
    [Serializable]
    [DataContract(IsReference = false)]
    public class VisitedPoint
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public User User { get; set; }

        [DataMember]
        public Point Point { get; set; }

        [DataMember]
        public DateTime TimeVisit { get; set; }

        [DataMember]
        public DateTime TimeSpending { get; set; }
    }
}