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
    public class NT_NUOCNGOT_MANNO
    {
        public NT_NUOCNGOT_MANNO()
        {
            this.DSNT_NuocNgotManNoDetail = new HashSet<NT_NUOCNGOT_MANNO_DETAIL>();
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

 
        [DataType(DataType.Date, ErrorMessage = "Sai định dạng ngày nhập:(dd/MM/yyyy)!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày nhập")]
        [ValidateDateLessThanDateNow]
        public DateTime NGAY_NHAP { get; set; }


        [ForeignKey("MA_TINHTP"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DTINHTP DMTinhTP { get; set; }
        public virtual ICollection<NT_NUOCNGOT_MANNO_DETAIL> DSNT_NuocNgotManNoDetail { get; set; }
    }

    public class NT_NUOCNGOT_MANNO_DETAIL
    {
        [Required]
        [Key]
        public int ID { get; set; }

        public int ID_NUOCNGOT_MANNO { get; set; }

        public int ID_NUOITRONG_LOAIMATNUOC { get; set; }
        public int ID_NUOITRONG_NHOMDOITUONG { get; set; }
        public int? ID_NUOITRONG_DOITUONG { get; set; }

        public int ID_MOHINH_NUOI { get; set; }
        public float? SAN_LUONG { get; set; }

        public float? DIEN_TICH { get; set; }
        public float? DIEN_TICH_GAP { get; set; }

        public float? DIEN_TICH_NUOI_CDK { get; set; }


        [ForeignKey("ID_NUOITRONG_LOAIMATNUOC"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_LOAI_MATNUOC_NGOTMANLO DM_LoaiMatNuocNgotManLo { get; set; }

        [ForeignKey("ID_NUOITRONG_NHOMDOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_NHOMDOITUONG_NUOI DM_NhomDoiTuong_KT { get; set; }

        [ForeignKey("ID_NUOITRONG_DOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_DOITUONG_NUOI DM_DoiTuong_KT { get; set; }

        [ForeignKey("ID_NUOCNGOT_MANNO"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual NT_NUOCNGOT_MANNO NT_NuocNgot_ManNo { get; set; }

        public float? NANG_SUAT_TU { get; set; }

        public float? NANG_SUAT_DEN { get; set; }
    }
}
