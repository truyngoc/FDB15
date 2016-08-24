using FDB.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace FDB.Models
{
    public class KT_SANLUONG
    {
        public KT_SANLUONG()
        {
            this.DSSanLuongDetail = new HashSet<KT_SANLUONG_DETAIL>();
        }

        [Required]
        [Key]
        public int ID { get; set; }


        [Required(ErrorMessage = "Tháng bắt buộc nhập")]
        [Display(Name = "Tháng")]
        [Range(1, 12, ErrorMessage = "Tháng phải thuộc khoảng từ 1 đến 12")]
        public int THANG { get; set; }


        [Required(ErrorMessage = "Năm bắt buộc nhập")]
        [Display(Name = "Năm")]
        [Range(1900, 2100, ErrorMessage = "Năm phải thuộc khoảng từ 1900 đến 2100")]
        public int NAM { get; set; }


      
        [Display(Name = "Đơn vị khai thác")]
        public String MA_TINHTP { get; set; }

        [Display(Name = "Người nhập")]
        [MaxLength(50)]
        public String NGUOI_NHAP { get; set; }

    
        [DataType(DataType.Date,ErrorMessage="Sai định dạng ngày nhập:(dd/MM/yyyy)!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày nhập")]
     
        public DateTime NGAY_NHAP { get; set; }

        [ForeignKey("MA_TINHTP")]
        public virtual DTINHTP DTinhTP { get; set; }

        public virtual ICollection<KT_SANLUONG_DETAIL> DSSanLuongDetail { get; set; }
    }
}
