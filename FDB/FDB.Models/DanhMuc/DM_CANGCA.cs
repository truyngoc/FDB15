using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DM_CANGCA
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên cảng bắt buộc nhập")]
        [StringLength(250, ErrorMessage = "Tên cảng không được quá 250 ký tự.")]
        [Display(Name = "Tên cảng")]
        public string TEN_CANG { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ không được quá 500 ký tự.")]
        [Display(Name = "Địa chỉ")]
        public string DIA_CHI { get; set; }

        [Display(Name = "Điện thoại")]
        [MaxLength(11)]
        public string DIEN_THOAI { get; set; }

        [Required(ErrorMessage= "Phân loại cảng bắt buộc nhập")]
        [Display(Name = "Phân loại cảng")]
        public int DPHAN_LOAI_CANGID { get; set; }

         [Required(ErrorMessage = "Tỉnh/TP bắt buộc nhập")]
         [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

         [Display(Name = "Quận/Huyện")]
         public string MA_QUANHUYEN { get; set; }

         [Display(Name = "Phường/Xã")]
         public string MA_PHUONGXA { get; set; }

        [Display(Name = "Ghi chú")]
        public string GHI_CHU { get; set; }

       //thanhnc5:ngay 19/02/2016
        [Display(Name = "Vị trí tọa độ(Kinh,vĩ độ)")]
        public string VITRI_TOADO { get; set; }

        [Display(Name = "Cỡ tàu lớn nhất (CV)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Cỡ tàu lớn nhất bắt buộc lớn hơn 0")]
        public decimal? COTAU { get; set; }

        [Display(Name = "Độ sâu đậu tàu")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Độ sâu đậu tầu bắt buộc lớn hơn 0")]
        public decimal? DOSAU_DAUTAU { get; set; }

        [Display(Name = "Vị trí bắt đầu của luồng")]
        public string VITRI_LUONG { get; set; }

        [Display(Name = "Độ sâu luồng vào")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Độ sâu luồng vào bắt buộc lớn hơn 0")]
        public decimal? DOSAU_LUONGVAO { get; set; }

        [Display(Name = "Chiều dài cầu cảng(m)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Chiều dài cầu cảng(m) bắt buộc lớn hơn 0")]
        public decimal? CHIEUDAI_CAUCANG { get; set; }

        [Display(Name = "Năng lực bốc xếp(tấn)")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Năng lực bốc xếp(tấn) bắt buộc lớn hơn 0")]
        public decimal? NANGLUC_BOCXEP{ get; set; }

        [Display(Name = "Hướng luồng vào (độ N)")]
        public string HUONG_LUONGVAO { get; set; }


        [Display(Name = "Tần số liên lạc")]
        public string TANSO { get; set; }

        [Display(Name = "Trạng thái,đóng mở")]
        public bool Status { get; set; }

        public virtual DTINHTP DTINHTP { get; set; }

        public virtual DQUANHUYEN DQUANHUYEN { get; set; }

        public virtual DPHUONGXA DPHUONGXA { get; set; }

      
        public virtual DPHAN_LOAI_CANG DPHAN_LOAI_CANG { get; set; }
        
    }
}
