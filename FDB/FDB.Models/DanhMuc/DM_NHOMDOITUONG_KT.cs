using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DM_NHOMDOITUONG_KT
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên nhóm là bắt buộc nhập")]
        [Display(Name = "Tên nhóm đối tượng")]
        public string TEN_NHOM { get; set; }

        [Display(Name = "Mô tả")]
        public string MO_TA { get; set; }

        //[Required(ErrorMessage = "Loại khai thác là bắt buộc nhập")]
        //public int ID_LOAI_MATNUOC_KT { get; set; }

        //[ForeignKey("ID_LOAI_MATNUOC_KT")]
        //public virtual DM_LOAI_MATNUOC_KT DM_LOAI_MATNUOC_KT { get; set; }

        //public virtual ICollection<DM_DOITUONG_KT> DM_DOITUONG_KTs { get; set; }
    }
}
