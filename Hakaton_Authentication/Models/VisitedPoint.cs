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