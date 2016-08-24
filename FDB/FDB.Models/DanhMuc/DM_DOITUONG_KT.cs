using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DM_DOITUONG_KT
    {
        [Required]
        public int ID { get; set; }

        
        //[Display(Name = "Loại khai thác")]
        //public int? LOAI_KHAI_THAC { get; set; }

        //[Required(ErrorMessage = "Phải chọn nhóm đối tượng")]
        //[Display(Name = "Mã nhóm đối tượng")]   
        //public int DM_NHOMDOITUONG_KTID { get; set; }

        [Required(ErrorMessage = "Tên đối tượng là bắt buộc nhập")]
        [Display(Name = "Tên đối tượng")]        
        public string TEN_DOI_TUONG { get; set; }

        [Display(Name = "Mô tả")]        
        public string MO_TA { get; set; }
        //public virtual DM_NHOMDOITUONG_KT DM_NHOMDOITUONG_KT { get; set; }

        //[ForeignKey("LOAI_KHAI_THAC")]
        //public virtual DM_LOAI_MATNUOC_KT DM_LOAI_MATNUOC_KT { get; set; }
    }
}
