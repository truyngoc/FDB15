using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace FDB.Models
{
    public class KT_DONGSUA_TAUTHUYEN
    {
        public KT_DONGSUA_TAUTHUYEN()
        {
            this.DSDongSuaTauThuyenDetail = new HashSet<KT_DONGSUA_TAUTHUYEN_DETAIL>();
        }

        [Required]
        [Key]
        public int ID { get; set; }


        [Required(ErrorMessage = "Tên cơ sở bắt buộc nhập!")]
        [Display(Name = "Tên cơ sở")]
        [MaxLength(500)]
        public String TEN_COSO { get; set; }


        [Display(Name = "Tên chủ cơ sở")]
        [MaxLength(500)]
        public String TEN_CHU_COSO { get; set; }

        [Required(ErrorMessage = "Địa chỉ bắt buộc nhập!")]
        [Display(Name = "Địa chỉ")]
        [MaxLength(500)]
        public String DIA_CHI { get; set; }

        [Display(Name = "Điện thoại")]
        [MaxLength(50)]
        public String DIEN_THOAI { get; set; }


        [Required(ErrorMessage = "Tỉnh thành phố bắt buộc nhập")]
        [Display(Name = "Tỉnh/Thành phố")]
        public String MA_TINHTP { get; set; }

        [Display(Name = "Quận/huyện")]
        public String MA_QUANHUYEN { get; set; }

        [Display(Name = "Phường/xã")]
        public String MA_PHUONGXA { get; set; }

        [Display(Name = "Người nhập")]
        [MaxLength(50)]
        public String NGUOI_NHAP { get; set; }

        //[Required(ErrorMessage = "Ngày nhập bắt buộc nhập")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày nhập")]
        public DateTime? NGAY_NHAP { get; set; }


        public int? SO_TAU_DONG_1_NAM { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public Decimal? TRONG_TAI_TOIDA_COTHE { get; set; }

        [ForeignKey("MA_TINHTP")]
        public virtual DTINHTP DTinhTP { get; set; }
        public virtual ICollection<KT_DONGSUA_TAUTHUYEN_DETAIL> DSDongSuaTauThuyenDetail { get; set; }


    }

    public class KT_DONGSUA_TAUTHUYEN_DETAIL
    {
        [Required]
        [Key]
        public int ID { get; set; }

        [Required]
        public int ID_DONGSUA_TAUTHUYEN { get; set; }
        public int? DONGMOI_VOGO { get; set; }

        public int? DONGMOI_VOTHEP { get; set; }

        public int? DONGMOI_VOCOMPOSITE { get; set; }
        public int? SUA_CHUA { get; set; }

        public int? GIAI_BAN { get; set; }

        public int? BAN_TINHKHAC { get; set; }

        public int? NAM { get; set; }

        [ForeignKey("ID_DONGSUA_TAUTHUYEN"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual KT_DONGSUA_TAUTHUYEN KT_DongSua_TauThuyen { get; set; }


        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public Decimal? TONG_TAITRONG { get; set; }
    }
}
