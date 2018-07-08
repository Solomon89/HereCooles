using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Hakaton_Db.Models
{
    /// <summary>
    /// Характеристики людей
    /// </summary>
    [Serializable]
    [DataContract(IsReference = false)]
    public class UserPerformance
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public User User { get; set; }

        [DataMember]
        public Performance Performance { get; set; }

        /// <summary>
        /// Уровень характеристики
        /// </summary>
        [DataMember]
        public int Level { get; set; }
    }
}