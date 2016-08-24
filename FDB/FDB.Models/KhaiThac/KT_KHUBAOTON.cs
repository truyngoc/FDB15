using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class KT_KHUBAOTON
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Loại khu bảo tồn bắt buộc nhập")]
        [Display(Name = "Loại khu bảo tồn")]
        public int DKHU_BAO_TONID { get; set; }

        [Display(Name = "Tên khu bảo tồn")]
        [Required(ErrorMessage = "Tên khu bảo tồn bắt buộc nhập")]
        public string TEN_KHU_BAO_TON { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DIA_CHI { get; set; }
        
        [Display(Name = "Tổng diện tích(ha)")]
        [Range(0,Int32.MaxValue,ErrorMessage = "Tổng diện tích bắt buộc lớn hơn 0")]
        public decimal? TONG_DIENTICH { get; set; }

       
        [Display(Name = "Phần đất liền(ha)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Phần đất liền bắt buộc lớn hơn 0")]
        public decimal? PHAN_DATLIEN { get; set; }

         
        [Display(Name = "Phần nước(ha)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Phần nước bắt buộc lớn hơn 0")]
        public decimal? PHAN_NUOC { get; set; }


        public string VIDO { get; set; }

        public string KINHDO { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP bắt buộc nhập")]
        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

        //[Required(ErrorMessage = "Quận/Huyện bắt buộc nhập")]
        [Display(Name = "Quận/Huyện")]
        public string MA_QUANHUYEN { get; set; }

         //[Required(ErrorMessage = "Phường/Xã bắt buộc nhập")]
        [Display(Name = "Phường/Xã")]
        public string MA_PHUONGXA { get; set; }

        [Display(Name = "Nơi sâu nhất(m)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Nơi sâu nhất bắt buộc lớn hơn 0")]
        public decimal? NOI_SAU_NHAT { get; set; }

        [Display(Name = "Đặc điểm hệ sinh thái")]
        [MaxLength(4000)]
        public string HE_SINH_THAI { get; set; }

        [Display(Name = "Công tác bảo tồn")]
        [MaxLength(4000)]
        public string CONGTAC_BAOTON { get; set; }

        [Display(Name = "Ghi chú")]
      
        public string GHICHU { get; set; }

        [Display(Name = "Tổng số khu bảo tồn đưa vào hoạt động")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Tổng số khu bảo tồn bắt buộc lớn hơn 0")]
        public int? TONG_KHUBAOTON { get; set; }

        [Display(Name = "Phạm vi giới hạn(ha)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Phạm vi giới hạn(ha) bắt buộc lớn hơn 0")]
        public decimal? PHAMVI_GIOIHAN { get; set; }
     

        public virtual DKHU_BAO_TON DKHU_BAO_TON { get; set; }
        public virtual DTINHTP DTINHTP { get; set; }

        public virtual DQUANHUYEN DQUANHUYEN { get; set; }

        public virtual DPHUONGXA DPHUONGXA { get; set; }
    }
}
