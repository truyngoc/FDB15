using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FDB.Common;

namespace FDB.Models
{
    public class DM_DONVIHANHCHINH
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]     
        [Required]
        [Key]               
        public string MA_DV { get; set; }

        [Required]
        public string TEN_DV { get; set; }

        [Required]
        public int Cap { get; set; }
        public string MA_DV_CAPTREN { get; set; }

        public string TEN_DV_CAPTREN { get; set; }

        public virtual ICollection<KT_SANLUONG> DSKTSanLuong { get; set; }
    }
}
