using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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

    public class EventPointComparer : EqualityComparer<EventPoint>
    {
        public override bool Equals(EventPoint x, EventPoint y)
        {
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Name == y.Name && x.IsPermanent == y.IsPermanent && x.DateCreate == y.DateCreate &&
                   x.Point.Equals(y.Point) && x.ScoreAward == y.ScoreAward && x.TimeLeft == y.TimeLeft &&
                   x.EventPointType.Equals(y.EventPointType);
        }

        public override int GetHashCode(EventPoint obj)
        {
            var hashVacancyName = obj?.Name.GetHashCode() ?? 0;

            var hashVacancyCode = obj?.Name.GetHashCode() ?? 0;

            return hashVacancyName ^ hashVacancyCode;
        }
    }
}