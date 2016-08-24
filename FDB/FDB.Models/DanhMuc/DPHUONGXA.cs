using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DPHUONGXA
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Key]
        public string MA_PHUONGXA { get; set; }

        [Required]
        public string TEN_PHUONGXA { get; set; }

        [Required]
        public string MA_QUANHUYEN { get; set; }

        //public virtual DQUANHUYEN DQUANHUYEN { get; set; }
    }
}
