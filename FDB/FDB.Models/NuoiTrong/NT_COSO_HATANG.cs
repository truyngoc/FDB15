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
    public class NT_COSO_HATANG
    {
        public NT_COSO_HATANG()
        {
            this.DSNT_CoSoHaTangDetail = new HashSet<NT_COSO_HATANG_DETAIL>();
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

        [ValidateQuyLessThanQuyNow("NAM", "Quý không được lớn hơn quý hiện tại.")]
        [Display(Name = "Quý")]
        [Range(1, 4, ErrorMessage = "Quý phải thuộc khoảng từ 1 đến 4")]
        public int? QUY { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        [Display(Name = "Tỉnh/thành phố")]
        public string MA_TINHTP { get; set; }

        [Required(ErrorMessage = "Quận/huyện là bắt buộc nhập")]
        [Display(Name = "Quận/huyện/thị xã")]
        public string MA_QUANHUYEN { get; set; }


        [Display(Name = "Người nhập")]
        [MaxLength(50)]
        public String NGUOI_NHAP { get; set; }


        [Required(ErrorMessage = "Ngày nhập bắt buộc nhập")]
        [DataType(DataType.Date, ErrorMessage = "Sai định dạng ngày nhập:(dd/MM/yyyy)!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày nhập")]
        [ValidateDateLessThanDateNow]
        public DateTime? NGAY_NHAP { get; set; }

        [ForeignKey("MA_QUANHUYEN"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DQUANHUYEN DMQuanHuyen { get; set; }

        [ForeignKey("MA_TINHTP"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DTINHTP DMTinhTP { get; set; }
        public virtual ICollection<NT_COSO_HATANG_DETAIL> DSNT_CoSoHaTangDetail { get; set; }
    }

    public class NT_COSO_HATANG_DETAIL
    {
        [Required]
        [Key]
        public int ID { get; set; }

        public int ID_COSO_HATANG { get; set; }

        public int ID_NUOITRONG_DOITUONG { get; set; }

        public bool? AO_CHUA { get; set; }

        public bool? KENH_CAP { get; set; }

        public bool? KENH_THOAT { get; set; }

        public bool? AO_XULY { get; set; }

        public bool? DAO_NUOC { get; set; }

        public bool? DIEN { get; set; }

        public bool? TB_MOITRUONG { get; set; }

        [ForeignKey("ID_NUOITRONG_DOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_DOITUONG_NUOI DM_DoiTuong_Nuoi { get; set; }

        //[ForeignKey("ID_NUOITRONG_DOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public virtual DM_DOITUONG_NUOI DM_HinhThuc_SX { get; set; }

        [ForeignKey("ID_COSO_HATANG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual NT_COSO_HATANG NT_CoSoHaTang { get; set; }
    }
}
