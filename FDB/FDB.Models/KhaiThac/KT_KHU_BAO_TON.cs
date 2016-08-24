using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class KT_KHU_BAO_TON
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Loại khu bảo tồn là bắt buộc nhập.")]
        [Display(Name = "Loại khu bảo tồn")]
        public int DKHU_BAO_TONID { get; set; }

        [Display(Name = "Tên khu bảo tồn")]
        [Required(ErrorMessage = "Tên khu bảo tồn là bắt buộc nhập.")]
        public string TEN_KHU_BAO_TON { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DIA_CHI { get; set; }

        [Display(Name = "Tổng diện tích(ha)")]
        public int? TONG_DIENTICH { get; set; }

        [Display(Name = "Phần đất liền(ha)")]
        public int? PHAN_DATLIEN { get; set; }

        [Display(Name = "Phần biển(ha)")]
        public int? PHAN_BIEN { get; set; }

        [Display(Name = "Đặc điểm")]
        [MaxLength(500)]
        public string DAC_DIEM { get; set; }
        public virtual DKHU_BAO_TON DKHU_BAO_TON { get; set; }
    }
}
