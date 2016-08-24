using System;
using FDB.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
   public class DM_DOITUONG_NUOI_MANLO
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên đối tượng là bắt buộc nhập")]
        [Display(Name = "Tên đối tượng")]
        public string TEN_DOI_TUONG { get; set; }

        [Required(ErrorMessage = "Loại hình thức nuôi là bắt buộc nhập")]
        public int LOAI_DOI_TUONG { get; set; }

        [ForeignKey("LOAI_DOI_TUONG")]
        public virtual DM_HINHTHUC_NUOI DM_HINHTHUC_NUOI { get; set; } 
    }
}
