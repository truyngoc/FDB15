using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DM_NHOMDOITUONG_NUOI
    {        
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage="Tên nhóm là bắt buộc nhập")]
        [Display(Name="Tên nhóm đối tượng")]        
        public string TEN_NHOM { get; set; }

        public int ID_LOAI_MATNUOC_NUOI { get; set; }

        [Display(Name = "Mô tả")]
        public string MO_TA { get; set; }

        public virtual ICollection<DM_DOITUONG_NUOI> DM_DOITUONG_NUOIs { get; set; }
    }
}
