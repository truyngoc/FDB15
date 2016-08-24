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
    public class NT_YEUTOVATNUOIDAUVAO
    {
        public NT_YEUTOVATNUOIDAUVAO()
        {
            this.DSNT_YeuToVatNuoiDauVaoDetail = new HashSet<NT_YEUTOVATNUOIDAUVAO_DETAIL>();
        }

        [Required]
        [Key]
        public int ID { get; set; }


        [Required(ErrorMessage = "Năm bắt buộc nhập")]
        [Display(Name = "Năm")]
        [Range(1900, 2100, ErrorMessage = "Năm phải thuộc khoảng từ 1900 đến 2100")]
        public int NAM { get; set; }

        [Required(ErrorMessage = "Quý bắt buộc nhập")]
        [ValidateQuyLessThanQuyNow("NAM", "Quý không được lớn hơn quý hiện tại.")]
        [Display(Name = "Quý")]
        [Range(1, 4, ErrorMessage = "Quý phải thuộc khoảng từ 1 đến 4")]
        public int? QUY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Báo cáo từ ngày là bắt buộc nhập")]
        [Display(Name = "Báo cáo từ ngày")]
        public DateTime? NGAY_BAOCAO_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Báo cáo đến ngày là bắt buộc nhập")]
        [Display(Name = "Báo cáo đến ngày")]
        [FDB_DateGreaterThanOrEqualTo("NGAY_BAOCAO_TU", "Báo cáo đến ngày lớn hơn hoặc bằng báo cáo từ ngày")]
        public DateTime? NGAY_BAOCAO_DEN { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        [Display(Name = "Tỉnh/thành phố")]
        public string MA_TINHTP { get; set; }

        [Display(Name = "Người nhập")]
        [MaxLength(50)]
        public String NGUOI_NHAP { get; set; }
        public DateTime NGAY_NHAP { get; set; }


        [ForeignKey("MA_TINHTP"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DTINHTP DMTinhTP { get; set; }
        public virtual ICollection<NT_YEUTOVATNUOIDAUVAO_DETAIL> DSNT_YeuToVatNuoiDauVaoDetail { get; set; }
    }
    public class NT_YEUTOVATNUOIDAUVAO_DETAIL
    {
        [Required]
        [Key]
        public int ID { get; set; }

        public int ID_YEUTOVATNUOIDAUVAO { get; set; }
        public int ID_NUOITRONG_NHOMDOITUONG { get; set; }
        public int? ID_NUOITRONG_DOITUONG { get; set; }

        public float? THUC_AN_NHAP { get; set; }
        public float? THUC_AN_SX_TRONGNUOC { get; set; }
        public float? THUC_AN_TUTAO { get; set; }

        public float? VI_SINH { get; set; }
        public float? KHOANG_CHAT { get; set; }
        public float? HOA_CHAT { get; set; }
        public float? THUOC_THU_Y { get; set; }

        [ForeignKey("ID_NUOITRONG_NHOMDOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_NHOMDOITUONG_NUOI DM_NhomDoiTuong_KT { get; set; }

        [ForeignKey("ID_NUOITRONG_DOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_DOITUONG_NUOI DM_DoiTuong_Nuoi { get; set; }


        [ForeignKey("ID_YEUTOVATNUOIDAUVAO"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual NT_YEUTOVATNUOIDAUVAO NT_YeuToVatTuDauVao { get; set; }
    }
}
