using FDB.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDB.Models
{
    public class NT_NUOC_MANLO
    {
        public NT_NUOC_MANLO()
        {
            this.DSNT_NuocManNoDetail = new HashSet<NT_NUOC_MANLO_DETAIL>();
        }

        [Required]
        [Key]
        public int ID { get; set; }

        [Display(Name = "Tháng")]
        [Range(1, 12, ErrorMessage = "Tháng phải thuộc khoảng từ 1 đến 12")]
        public int? THANG { get; set; }

        [Required(ErrorMessage = "Năm bắt buộc nhập")]
        [Display(Name = "Năm")]
        [Range(1900, 2100, ErrorMessage = "Năm phải thuộc khoảng từ 1900 đến 2100")]
        public int NAM { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        [Display(Name = "Tỉnh/thành phố")]
        public string MA_TINHTP { get; set; }

    
        [Display(Name = "Người nhập")]
        [MaxLength(50)]
        public String NGUOI_NHAP { get; set; }

 
        [DataType(DataType.Date, ErrorMessage = "Sai định dạng ngày nhập:(dd/MM/yyyy)!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày nhập")]
        [ValidateDateLessThanDateNow]
        public DateTime NGAY_NHAP { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Sai định dạng ngày báo cáo từ:(dd/MM/yyyy)!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày báo cáo từ")]
        public DateTime? NGAY_BAO_CAO_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Báo cáo đến ngày")]
        [FDB_DateGreaterThanOrEqualTo("NGAY_BAO_CAO_TU", "Báo cáo đến ngày lớn hơn hoặc bằng báo cáo từ ngày")]
        public DateTime? NGAY_BAO_CAO_DEN { get; set; }
        

        [Required(ErrorMessage = "Loại báo cáo là bắt buộc nhập")]
        public int LOAI_BAO_CAO { get; set; }

        [ForeignKey("MA_TINHTP"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DTINHTP DMTinhTP { get; set; }
        public virtual ICollection<NT_NUOC_MANLO_DETAIL> DSNT_NuocManNoDetail { get; set; }
    }

    public class NT_NUOC_MANLO_DETAIL
    {
        [Required]
        [Key]
        public int ID { get; set; }
        public int ID_NUOC_MANLO{ get; set; }
        public int ID_DOITUONG_NUOI_MANLO { get; set; }
        public int ID_HINHTHUC_NUOI { get; set; }
        public int? SO_LUONG_LONG_LONGBE { get; set; }
        public decimal? THE_TICH_LONG_LONGBE { get; set; }
        public decimal? MAT_DO_THA_LONGBE { get; set; }
        public decimal? SAN_LUONG_LONGBE { get; set; }
        public decimal? SO_LUONG_GIONG_THA_LONGBE { get; set; }

        //khác:
        public decimal? DIEN_TICH_THANUOI_TRONGKY_KHAC { get; set; }
        public decimal? DIEN_TICH_THANUOI_LUYKE_KHAC { get; set; }
        public decimal? NUOI_TRONG_QUY_HOACH_KHAC { get; set; }
        public decimal? NUOI_NGOAI_QUY_HOACH_KHAC { get; set; }
        public decimal? DIEN_TICH_GAP_TUONGTU_KHAC { get; set; }
        public decimal? SO_LUONG_GIONG_THA_KHAC { get; set; }
        public int? ID_PHUONG_THUC_NUOI_KHAC { get; set; }
        public decimal? SAN_LUONG_KHAC { get; set; }


        [ForeignKey("ID_DOITUONG_NUOI_MANLO"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_DOITUONG_NUOI_MANLO DM_DoiTuong_KT { get; set; }

        [ForeignKey("ID_NUOC_MANLO"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual NT_NUOC_MANLO NT_Nuoc_ManNo { get; set; }

        [ForeignKey("ID_HINHTHUC_NUOI"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_HINHTHUC_NUOI NT_HinhThuc_Nuoi { get; set; }

    }
}
