using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
   public class KT_KHUBAOVENGUONLOI
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên khu bảo vệ nguồn lợi bắt buộc nhập.")]
        [Display(Name = "Tên khu bảo vệ nguồn lợi")]
        public string TEN_KHUBAOVENGUONLOI { get; set; }

        [Display(Name = "Địa chỉ")]
        [MaxLength(500)]
        public string DIA_CHI { get; set; }

        [Display(Name = "Tổng diện tích(ha)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Tổng diện tích(ha) bắt buộc lớn hơn 0")]
        public decimal? TONG_DIENTICH { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Thời gian cấm đánh bắt từ")]
        public DateTime? THOIGIAN_CAM_TU { get; set; }

        [Display(Name = "Đến ngày")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB.Common.FDB_DateGreaterThan("THOIGIAN_CAM_TU", "Thời gian cấm đánh bắt đến ngày phải lớn hơn thời gian cấm đánh bắt từ ngày")]
        public DateTime? THOIGIAN_CAM_DEN { get; set; }

        [Display(Name = "Đối tượng cấm khai thác")]
        [MaxLength(1000)]
        public string DOITUONG_CAM_KHAITHAC { get; set; }

        [Display(Name = "Ghi chú")]
        [MaxLength(4000)]
        public string GHICHU { get; set; }

        [Display(Name = "Nơi trực tiếp quản lý")]
        [MaxLength(500)]
        public string NOI_QUANLY { get; set; }


        [Display(Name = "Đặc điểm hệ sinh thái")]
        [MaxLength(500)]
        public string HE_SINH_THAI { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP bắt buộc nhập")]
        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

      
        [Display(Name = "Quận/Huyện")]
        public string MA_QUANHUYEN { get; set; }

      
        [Display(Name = "Phường/Xã")]
        public string MA_PHUONGXA { get; set; }

        [Display(Name = "Phạm vi giới hạn(ha)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Phạm vi giới hạn(ha) bắt buộc lớn hơn 0")]
        public decimal? PHAMVI_GIOIHAN { get; set; }

        public virtual DTINHTP DTINHTP { get; set; }

        public virtual DQUANHUYEN DQUANHUYEN { get; set; }

        public virtual DPHUONGXA DPHUONGXA { get; set; }
      
    }
}
