using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DQUANHUYEN
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Key]
        public string MA_QUANHUYEN { get; set; }

        [Required]
        public string TEN_QUANHUYEN { get; set; }

        [Required]
        public string MA_TINHTP { get; set; }

        //public virtual DTINHTP DTINHTP { get; set; }

        //public virtual ICollection<DPHUONGXA> DPHUONGXAs { get; set; }
    }
}
