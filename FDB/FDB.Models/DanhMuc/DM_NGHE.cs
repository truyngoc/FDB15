using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FDB.Models
{
    public class DM_NGHE
    {
        [Display(Name="Mã nghề")]
        [Required(ErrorMessage = "Mã nghề là bắt buộc nhập")]
        public int DM_NgheID { get; set; }

        [Required(ErrorMessage = "Phải chọn nhóm nghề")]
        [Display(Name = "Mã nhóm nghề")]
        public int DM_NhomNgheID { get; set; }

        [Required(ErrorMessage = "Tên nghề là bắt buộc nhập")]
        [Display(Name = "Tên nghề")]
        [MaxLength(255)]
        public string TenNghe { get; set; }
    
        [Display(Name = "Mô tả")]
        [MaxLength(2000)]
        public string MoTa { get; set; }

        public virtual DM_NHOMNGHE DM_NhomNghe { get; set; }
    }
}