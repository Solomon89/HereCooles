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