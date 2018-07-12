using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Hakaton_Db.Models
{
    [Serializable]
    [DataContract(IsReference = false)]
    public class EventPoint
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime DateCreate { get; set; }

        [DataMember]
        public bool IsPermanent { get; set; }

        [DataMember]
        public DateTime? TimeLeft { get; set; }

        [DataMember]
        public decimal ScoreAward { get; set; }

        [DataMember]
        public Point Point { get; set; }

        [DataMember]
        public EventPointType EventPointType { get; set; }
    }
}