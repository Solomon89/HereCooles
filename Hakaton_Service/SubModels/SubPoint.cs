using Hakaton_Db.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hakaton_Service.SubModels
{
    [Serializable]
    [DataContract(IsReference = false)]
    public class SubPoint
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double X { get; set; }

        [DataMember]
        public double Y { get; set; }

        [DataMember]
        public string Description { get; set; }
        
        [DataMember]
        public PointType PointType { get; set; }
        [DataMember]
        public PerformancePoint PerformancePoint { get; set; }

        [DataMember]
        public List<SubEvent> SubEvents { get; set; }
    }
}