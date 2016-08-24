using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class KT_KHUNEODAU
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên khu neo đậu bắt buộc nhập.")]
        [Display(Name = "Tên khu neo đậu")]
        public string TEN_KHUNEODAU { get; set; }

        [Required(ErrorMessage = "Địa chỉ bắt buộc nhập.")]
        [Display(Name = "Địa chỉ")]
        [MaxLength(500)]
        public string DIA_CHI { get; set; }

        [Display(Name = "Số lượng tàu được neo đậu")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượng tàu được neo đậu bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượng tàu neo đậu phải là số nguyên")]
        public int? SOLUONGTAU_NEODAU { get; set; }

        [Display(Name = "Công suất tàu tối đa được neo đậu")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Công suất tàu tối đa được neo đậu bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Công suất tàu tối đa được neo đậu phải là số nguyên")]
        public int? CONGSUATTAU_NEODAU { get; set; }

        [Display(Name = "Phao neo đậu độc lập(cái)")]
        [Range(0, int.MaxValue, ErrorMessage = "Phao neo đậu độc lập(cái) bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Phao neo đậu độc lập phải là số nguyên")]
        public int? PHAO_NEODAU { get; set; }


        [Display(Name = "Trụ neo bờ độc lập(cái)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Trụ neo bờ độc lập(cái) bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Trụ neo bờ độc lập phải là số nguyên")]
        public int? TRUNEO { get; set; }

        [Display(Name = "Phao tiêu báo hiệu(cái)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Phao tiêu báo hiệu(cái) bắt buộc lớn hơn 0")]
         [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Phao tiêu báo hiệu phải là số nguyên")]
        public int? PHAOTIEU { get; set; }

        //ThanhNC5:ngay 18/02/2016
        
        [Display(Name = "Loại khu neo đậu")]
        [Required(ErrorMessage = "Loại khu neo đậu bắt buộc nhập.")]
        public int DPHAN_LOAI_KHUNEODAUID { get; set; }

      
        [MaxLength(500)]
        [Required(ErrorMessage = "Vĩ độ bắt buộc nhập.")]
        public string VIDO { get; set; }

      
        [MaxLength(500)]
        [Required(ErrorMessage = "Kinh độ bắt buộc nhập.")]
        public string KINHDO { get; set; }


        [Display(Name = "Độ sâu vùng nước đậu tàu(m)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Độ sâu vùng nước đậu tàu(m) bắt buộc lớn hơn 0")]
        public decimal? DOSAU_DAUTAU { get; set; }

        [Display(Name = "Vị trí bắt đầu vào luồng")]
        [MaxLength(500)]
        public string VITRI_VAOLUONG { get; set; }

        [Display(Name = "Hướng luồng")]
        [MaxLength(500)]
        public string HUONG_LUONG { get; set; }

        [Display(Name = "Chiều dài luồng(m)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Chiều dài luồng(m) bắt buộc lớn hơn 0")]
        public decimal? CHIEUDAI_LUONG { get; set; }

        [Display(Name = "Số điện thoại")]
        [MaxLength(11, ErrorMessage = "Số điện thoại không quá 11 số")]
        public string SO_DIEN_THOAI { get; set; }

        [Display(Name = "Tần số liên lạc")]
        [MaxLength(500)]
        public string TAN_SO { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP bắt buộc nhập")]
        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string MA_QUANHUYEN { get; set; }

        [Display(Name = "Phường/Xã")]
        public string MA_PHUONGXA { get; set; }

        public virtual DPHAN_LOAI_KHUNEODAU DPHAN_LOAI_KHUNEODAU { get; set; }
        public virtual DTINHTP DTINHTP { get; set; }

        public virtual DQUANHUYEN DQUANHUYEN { get; set; }

        public virtual DPHUONGXA DPHUONGXA { get; set; }


    }
}
