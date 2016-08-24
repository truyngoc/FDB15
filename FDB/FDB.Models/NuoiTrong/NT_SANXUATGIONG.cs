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
    public class NT_SANXUATGIONG
    {
        public NT_SANXUATGIONG()
        {
            this.DSNT_SanXuatGiongDetail = new HashSet<NT_SANXUATGIONG_DETAIL>();
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
        public virtual ICollection<NT_SANXUATGIONG_DETAIL> DSNT_SanXuatGiongDetail { get; set; }
    }
    public class NT_SANXUATGIONG_DETAIL
    {
        [Required]
        [Key]
        public int ID { get; set; }
        public int ID_SANXUATGIONG { get; set; }
        public int? ID_NUOI_SX_DOITUONG { get; set; }
        public decimal? SAN_LUONG_GIONG { get; set; }
        public int? TONG_COSO_SX_GIONG { get; set; }
        public int? TONG_COSO_LOAI_A { get; set; }
        public int? TONG_COSO_LOAI_B { get; set; }
        public int? TONG_COSO_LOAI_C { get; set; }

        public int? TONG_TRAI_GIONG { get; set; }
        public decimal? TONG_GIONG_KIEMDICH { get; set; }
        public decimal? TONG_BOME_SX { get; set; }
        public decimal? TONG_BOME_NHAP { get; set; }

        [ForeignKey("ID_NUOI_SX_DOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_DOITUONG_NUOI_SANXUATGIONG DM_DoiTuong_Nuoi_SanXuatGiong { get; set; }

        [ForeignKey("ID_SANXUATGIONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual NT_SANXUATGIONG NT_SanXuatGiong { get; set; }

    }
}
