using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FDB.Models
{
    public class DM_NHOMNGHE
    {
        [Required(ErrorMessage="Mã nhóm nghề là bắt buộc nhập")]        
        [Display(Name = "Mã Nhóm nghề")]
        public int DM_NhomNgheID { get; set; }

        [Required(ErrorMessage = "Tên nhóm nghề là bắt buộc nhập")]
        [MaxLength(255)]
        [Display(Name = "Tên nhóm nghề")]
        public string TenNhomNghe { get; set; }

        [Display(Name = "Mô tả")]
        [MaxLength(2000)]
        public string MoTa { get; set; }
               
        //public virtual ICollection<DM_NGHE> DM_Nghes { get; set; }
    }
}