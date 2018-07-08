using System;
using System.Runtime.Serialization;

namespace Hakaton_Service.SubModels
{
    [DataContract]
    public class SubEvent
    {
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
    }
}