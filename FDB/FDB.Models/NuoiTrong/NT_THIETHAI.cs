using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FDB.Models
{
    public class NT_THIETHAI
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Đối tượng nuôi bắt buộc nhập")]
        [Display(Name = "Đối tượng nuôi")]
        public int DM_DOITUONG_NUOI_THIETHAIID { get; set; }

        [Required(ErrorMessage = "Diện tích thiệt hại bắt buộc nhập")]
        [Display(Name = "Diện tích thiệt hại")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Diện tích thiệt hại bắt buộc lớn hơn 0")]
     
        public decimal? DIENTICH_THIETHAI { get; set; }

        [Display(Name = "Sản lượng thiệt hại")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Sản lượng thiệt hại bắt buộc lớn hơn 0")]
        
        public decimal? SANLUONG_THIETHAI { get; set; }

        [Display(Name = "Kích thước")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Kích thước bắt buộc lớn hơn 0")]
        public decimal? KICHTHUOC { get; set; }

        [Display(Name = "Số ngày nuôi")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Số ngày nuôi bắt buộc lớn hơn 0")]
        [Column]
       
        public decimal? SONGAYNUOI { get; set; }

        [Display(Name = "Nguyên nhân chi tiết")]
        [MaxLength(500)]
        public string NGUYENNHAN { get; set; }

        [Display(Name = "Thiệt hại ước tính")]
       // [Range(0, Int32.MaxValue, ErrorMessage = "Thiệt hại ước tính bắt buộc lớn hơn 0")]
        
        public decimal? THIETHAI_UOCTINH { get; set; }

        //[Display(Name = "Tỷ lệ thiệt hại")]
        //[Range(0, Int32.MaxValue, ErrorMessage = "Tỷ lệ thiệt hại bắt buộc lớn hơn 0")]
        //public decimal? TYLE_THIETHAI { get; set; }

        public int? DO_MOITRUONG { get; set; }
        public int? DO_DICHBENH { get; set; }

        
     


       
        [Display(Name = "Tỉnh/Thành phố")]
        public string MA_TINHTP { get; set; }

        //[Required(ErrorMessage = "Nguyên nhân thiệt hại bắt buộc nhập")]
        //[Display(Name = "NGUYÊN NHÂN")]
        //public int DNGUYENNHAN_THIETHAIID { get; set; }



        [Required(ErrorMessage = "Tỷ lệ thiệt hại bắt buộc nhập")]
        [Display(Name = "TỶ LỆ THIỆT HẠI")]
        public int DTYLE_THIETHAIID { get; set; }

        [Display(Name = "Tháng")]
        [Range(1, 12, ErrorMessage = "Tháng phải thuộc khoảng từ 1 đến 12")]
        public int? THANG { get; set; }

        [Required(ErrorMessage = "Năm bắt buộc nhập")]
        [Display(Name = "Năm")]
        [Range(1900, 2100, ErrorMessage = "Năm phải thuộc khoảng từ 1900 đến 2100")]
        public int NAM { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string MA_QUANHUYEN { get; set; }

        [Display(Name = "Phường/Xã")]
        public string MA_PHUONGXA { get; set; }

        
        //navigation property
        public virtual DM_DOITUONG_NUOI_THIETHAI DM_DOITUONG_NUOI_THIETHAI { get; set; }

         public virtual DTINHTP DTINHTP { get; set; }

         //public virtual DNGUYENNHAN_THIETHAI DNGUYENNHAN_THIETHAI { get; set; }

         public virtual DTYLE_THIETHAI DTYLE_THIETHAI { get; set; }


         public virtual DQUANHUYEN DQUANHUYEN { get; set; }

         public virtual DPHUONGXA DPHUONGXA { get; set; }

    }
}
