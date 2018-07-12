using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Hakaton_Db.Models
{
    /// <summary>
    ///     Модель таблицы Users
    /// </summary>
    [Serializable]
    [DataContract(IsReference = false)]
    public class User
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Fio { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public decimal Scores { get; set; }

        [DataMember]
        public string AccessTokenVk { get; set; }

        [DataMember]
        public long UserIdVk { get; set; }

        public ICollection<UserPerformance> UserPerformances { get; set; } = new List<UserPerformance>();
        public ICollection<VisitedPoint> VisitedPoints { get; set; } = new List<VisitedPoint>();
    }
}