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
    public class NT_NUOILONGBE
    {
        public NT_NUOILONGBE()
        {
            this.DSNT_NuoiLongBeDetail = new HashSet<NT_NUOILONGBE_DETAIL>();
        }

        [Required]
        [Key]
        public int ID { get; set; }


        //[Required(ErrorMessage = "Tháng bắt buộc nhập")]
        [Display(Name = "Tháng")]
        [Range(1, 12, ErrorMessage = "Tháng phải thuộc khoảng từ 1 đến 12")]
        public int? THANG { get; set; }


        // [Index("IX_REG", IsClustered = true, IsUnique = true)] -- cái này chỉ có EF 6.1 trở lên :)
        [Required(ErrorMessage = "Năm bắt buộc nhập")]
        [Display(Name = "Năm")]
        [Range(1900, 2100, ErrorMessage = "Năm phải thuộc khoảng từ 1900 đến 2100")]
        public int NAM { get; set; }

        //[ValidateQuyLessThanQuyNow("NAM", "Quý không được lớn hơn quý hiện tại.")]
        //[Display(Name = "Quý")]
        //[Range(1, 4, ErrorMessage = "Quý phải thuộc khoảng từ 1 đến 4")]
        //public int? QUY { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        [Display(Name = "Tỉnh/thành phố")]
        public string MA_TINHTP { get; set; }


        [Display(Name = "Người nhập")]
        [MaxLength(50)]
        public String NGUOI_NHAP { get; set; }


        [Required(ErrorMessage = "Ngày nhập bắt buộc nhập")]
        [DataType(DataType.Date, ErrorMessage = "Sai định dạng ngày nhập:(dd/MM/yyyy)!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày nhập")]
        [ValidateDateLessThanDateNow]
        public DateTime NGAY_NHAP { get; set; }


        [ForeignKey("MA_TINHTP"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DTINHTP DMTinhTP { get; set; }
        public virtual ICollection<NT_NUOILONGBE_DETAIL> DSNT_NuoiLongBeDetail { get; set; }
    }
    public class NT_NUOILONGBE_DETAIL
    {
        [Required]
        [Key]
        public int ID { get; set; }
        public int ID_NUOILONGBE { get; set; }

        public int ID_NUOITRONG_LOAIMATNUOC { get; set; }
        public int ID_NUOITRONG_NHOMDOITUONG { get; set; }
        public int? ID_NUOITRONG_DOITUONG { get; set; }

        public int? SO_LUONG_LONG { get; set; }

        public int? MAT_DO_THA { get; set; }
        public float? SAN_LUONG { get; set; }
        public float? THE_TICH_LONG { get; set; }
        public float? NANG_SUAT_TU { get; set; }
        public float? NANG_SUAT_DEN { get; set; }
        public float? THUC_AN { get; set; }

      

        [ForeignKey("ID_NUOITRONG_NHOMDOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_NHOMDOITUONG_NUOI DM_NhomDoiTuong_KT { get; set; }

        [ForeignKey("ID_NUOITRONG_DOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_DOITUONG_NUOI DM_DoiTuong_Nuoi { get; set; }

        [ForeignKey("ID_NUOILONGBE"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual NT_NUOILONGBE NT_NuoiLongBe { get; set; }

    }
}
