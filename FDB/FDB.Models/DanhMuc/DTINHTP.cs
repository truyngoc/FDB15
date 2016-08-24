using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DTINHTP
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Key]
        public string MA_TINHTP { get; set; }

        [Required]
        public string TEN_TINHTP { get; set; }

        //public virtual ICollection<DQUANHUYEN> DQUANHUYENs { get; set; }
    }
}
