using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{ 
   public class NT_TT_THITRUONG
    {
         [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Đối tượng nuôi bắt buộc nhập")]
        [Display(Name = "Đối tượng nuôi")]
        public int DM_DOITUONG_GIA_THITRUONGID { get; set; }

     

        [Display(Name = "gía thành từ")]
        //[Range(0, Int32.MaxValue, ErrorMessage = "Giá thành từ bắt buộc lớn hơn 0")]
        //[DisplayFormat(NullDisplayText = "n/a", ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]

        
        public decimal? GIA_THANH_TU { get; set; }

        
        [Display(Name = "Giá thành đến")]
        //[DisplayFormat(NullDisplayText = "n/a", ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        //[Range(0, Int32.MaxValue, ErrorMessage = "Giá thành đến bắt buộc lớn hơn 0")]
        
        public decimal? GIA_THANH_DEN { get; set; }

        [Display(Name = "gía giống từ")]
        //[Range(0, float.MaxValue, ErrorMessage = "Giá giống từ bắt buộc lớn hơn 0")]

        public decimal? GIA_GIONG_TU { get; set; }

        [Display(Name = "Giá giống đến")]
        //[Range(0, float.MaxValue, ErrorMessage = "Giá giống đến bắt buộc lớn hơn 0")]

        public decimal? GIA_GIONG_DEN { get; set; }

        [Display(Name = "gía thức ăn từ")]
        //[Range(0, float.MaxValue, ErrorMessage = "Giá thức ăn từ bắt buộc lớn hơn 0")]

        public decimal? GIA_THUCAN_TU { get; set; }

        [Display(Name = "Giá thức ăn đến")]
        //[Range(0, float.MaxValue, ErrorMessage = "Giá thức ăn đến bắt buộc lớn hơn 0")]

        public decimal? GIA_THUCAN_DEN { get; set; }

        [Display(Name = "gía bán từ")]
       // [Range(0, float.MaxValue, ErrorMessage = "Giá bán từ bắt buộc lớn hơn 0")]

        public decimal? GIA_BAN_TU { get; set; }

        [Display(Name = "Giá bán đến")]
       // [Range(0, float.MaxValue, ErrorMessage = "Giá bán đến bắt buộc lớn hơn 0")]

        public decimal? GIA_BAN_DEN { get; set; }

        [Display(Name = "Kích cỡ")]
       // [Range(0, Int32.MaxValue, ErrorMessage = "Kích cỡ bắt buộc lớn hơn 0")]

        public string KICHCO { get; set; }

        [Display(Name = "Nơi thu thập")]

        public string NOITHUTHAP { get; set; }
        [Required(ErrorMessage = "Ngày thu thập bắt buộc nhập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày thu thập giá")]
        public DateTime? NGAY_THUTHAP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày nhập máy")]
        
        public DateTime? NGAY_NM { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string MA_TINHTP { get; set; }
    
         public virtual DM_DOITUONG_GIA_THITRUONG DM_DOITUONG_GIA_THITRUONG { get; set; }

         public virtual DTINHTP DTINHTP { get; set; }
    }
         
}
