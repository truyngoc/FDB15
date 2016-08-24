using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FDB.Common;
using System.Web.Mvc;   // for RemoteAttribute using to validate exist item in database

namespace FDB.Models
{
    public class KT_GIAYPHEP
    {
        [Key]
        public int KT_GIAYPHEPID { get; set; }

        [Required(ErrorMessage = "Số giấy phép là bắt buộc nhập")]
        [Remote("check_SO_GP_Exist", "KT_GIAYPHEP", HttpMethod = "POST", ErrorMessage = "Số giấy phép đã tồn tại", AdditionalFields = "IsEdit,MA_TINHTP")]
        public string SO_GP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày cấp phép là bắt buộc nhập")]
        public DateTime? NGAY_GP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày hiệu lực là bắt buộc nhập")]
        [FDB_DateGreaterThanOrEqualTo("NGAY_GP", "Phải lớn hơn hoặc bằng ngày cấp phép")]
        public DateTime? NGAY_HL_GP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày hết hiệu lực là bắt buộc nhập")]
        [FDB_DateGreaterThan("NGAY_HL_GP", "Phải lớn hơn ngày hiệu lực")]
        public DateTime? NGAY_HHL_GP { get; set; }

        [Required(ErrorMessage = "Cảng đăng ký là bắt buộc nhập")]
        public string CANG_DANG_KY { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        public string MA_TINHTP { get; set; }
        
        [Required(ErrorMessage = "Số đăng ký là bắt buộc nhập")]
        [Remote("check_SO_DK_Exist", "KT_GIAYPHEP", HttpMethod = "POST", ErrorMessage = "Số đăng ký không tồn tại trên hệ thống", AdditionalFields = "IsEdit,MA_TINHTP")]
        public string SO_DK { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]  
        public DateTime? NGAY_DK { get; set; }

        public string TEN_TAU { get; set; }
        public string CHU_PHUONG_TIEN { get; set; }
        public string DIA_CHI { get; set; }
        public string DIEN_THOAI { get; set; }

        // 1
        [ForeignKey("DM_NHOMNGHE")]
        [Required(ErrorMessage = "Ngành nghề là bắt buộc nhập")]
        public int? DM_NHOMNGHEID { get; set; }

        [Required]
        public bool IsNGHECHINH { get; set; }

        [Required(ErrorMessage = "Vùng tuyến là bắt buộc nhập")]
        public int DVUNG_TUYENID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        [FDB_DateGreaterThanOrEqualTo("NGAY_HL_GP", "Phải lớn hơn hoặc bằng ngày HL giấy phép")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]
        public DateTime? CAP_PHEP_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Bắt buộc nhập")]
        [FDB_DateGreaterThan("CAP_PHEP_TU", "Phải lớn hơn thời gian cấp phép từ")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]
        public DateTime? CAP_PHEP_DEN { get; set; }
        // 2
        [ForeignKey("DM_NHOMNGHE2")]
        public int? DM_NHOMNGHE2ID { get; set; }
        public bool IsNGHECHINH_2 { get; set; }

        [ForeignKey("DVUNG_TUYEN2")]
        public int? DVUNG_TUYEN2ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB_DateGreaterThanOrEqualTo("NGAY_HL_GP", "Phải lớn hơn hoặc bằng ngày HL giấy phép")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]
        public DateTime? CAP_PHEP_TU_2 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB_DateGreaterThan("CAP_PHEP_TU_2", "Phải lớn hơn thời gian cấp phép từ")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]
        public DateTime? CAP_PHEP_DEN_2 { get; set; }
        // 3
        [ForeignKey("DM_NHOMNGHE3")]
        public int? DM_NHOMNGHE3ID { get; set; }
        public bool IsNGHECHINH_3 { get; set; }

        [ForeignKey("DVUNG_TUYEN3")]
        public int? DVUNG_TUYEN3ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB_DateGreaterThanOrEqualTo("NGAY_HL_GP", "Phải lớn hơn hoặc bằng ngày HL giấy phép")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]
        public DateTime? CAP_PHEP_TU_3 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB_DateGreaterThan("CAP_PHEP_TU_3", "Phải lớn hơn thời gian cấp phép từ")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]
        public DateTime? CAP_PHEP_DEN_3 { get; set; }
        // 4
        [ForeignKey("DM_NHOMNGHE4")]
        public int? DM_NHOMNGHE4ID { get; set; }
        public bool IsNGHECHINH_4 { get; set; }

        [ForeignKey("DVUNG_TUYEN4")]
        public int? DVUNG_TUYEN4ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB_DateGreaterThanOrEqualTo("NGAY_HL_GP", "Phải lớn hơn hoặc bằng ngày HL giấy phép")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]
        public DateTime? CAP_PHEP_TU_4 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB_DateGreaterThan("CAP_PHEP_TU_4", "Phải lớn hơn thời gian cấp phép từ")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]
        public DateTime? CAP_PHEP_DEN_4 { get; set; }
        // 5
        [ForeignKey("DM_NHOMNGHE5")]
        public int? DM_NHOMNGHE5ID { get; set; }
        public bool IsNGHECHINH_5 { get; set; }

        [ForeignKey("DVUNG_TUYEN5")]
        public int? DVUNG_TUYEN5ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB_DateGreaterThanOrEqualTo("NGAY_HL_GP", "Phải lớn hơn hoặc bằng ngày HL giấy phép")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]
        public DateTime? CAP_PHEP_TU_5 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB_DateGreaterThan("CAP_PHEP_TU_5", "Phải lớn hơn thời gian cấp phép từ")]
        [FDB_DateLessThanOrEqualTo("NGAY_HHL_GP", "Phải nhỏ hơn hoặc bằng ngày HHL giấy phép")]        
        public DateTime? CAP_PHEP_DEN_5 { get; set; }

        // gia han
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_GIA_HAN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_GIA_HAN_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_GIA_HAN_DEN { get; set; }
        public string GHI_CHU { get; set; }

        public int? TRANG_THAI { get; set; }    // null:đang hiệu lực (tao moi), 1:hủy, 2:gia han
        // collection
        public virtual KT_TAUTHUYEN KT_TAUTHUYEN { get; set; }
        public virtual DTINHTP DTINHTP { get; set; }
        public virtual ICollection<KT_GIAYPHEP_NK> KT_GIAYPHEP_NKs { get; set; }

        public virtual DM_NHOMNGHE DM_NHOMNGHE { get; set; }
        public virtual DM_NHOMNGHE DM_NHOMNGHE2 { get; set; }
        public virtual DM_NHOMNGHE DM_NHOMNGHE3 { get; set; }
        public virtual DM_NHOMNGHE DM_NHOMNGHE4 { get; set; }
        public virtual DM_NHOMNGHE DM_NHOMNGHE5 { get; set; }
        public virtual DVUNG_TUYEN DVUNG_TUYEN { get; set; }
        public virtual DVUNG_TUYEN DVUNG_TUYEN2 { get; set; }
        public virtual DVUNG_TUYEN DVUNG_TUYEN3 { get; set; }
        public virtual DVUNG_TUYEN DVUNG_TUYEN4 { get; set; }
        public virtual DVUNG_TUYEN DVUNG_TUYEN5 { get; set; }
    }

    public class KT_GIAYPHEP_NK
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_GP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HL_GP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HHL_GP { get; set; }
        public string GHI_CHU { get; set; }

        public int KT_GIAYPHEPID { get; set; }

        public virtual KT_GIAYPHEP KT_GIAYPHEP { get; set; }
    }

    public class KT_GIAYPHEP_GIAHAN_ViewModel
    {
        public int KT_GIAYPHEPID { get; set; }
        
        public string SO_GP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_GP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HL_GP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HHL_GP { get; set; }        

        public string SO_DK { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_DK { get; set; }

        
        // gia han
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày gia hạn là bắt buộc nhập")]
        [FDB_DateLessThanOrEqualTo("NGAY_GIA_HAN_TU", "Ngày gia hạn phải nhỏ hơn hoặc bằng gia hạn từ ngày")]
        public DateTime? NGAY_GIA_HAN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày gia hạn từ là bắt buộc nhập")]
        [FDB_DateGreaterThan("NGAY_HHL_GP", "Phải lớn hơn ngày HHL giấy phép")]
        [FDB_DateGreaterThanOrEqualTo("NGAY_GIA_HAN_DEN_OLD", "Phải lớn hơn ngày gia hạn cuối cùng")]
        public DateTime? NGAY_GIA_HAN_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày gia hạn đến là bắt buộc nhập")]
        [FDB_DateGreaterThan("NGAY_GIA_HAN_TU", "Phải lớn hơn ngày gia hạn từ")]
        public DateTime? NGAY_GIA_HAN_DEN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_GIA_HAN_DEN_OLD { get; set; }  // de luu gia tri cua ngay hhl lan gia han (dung validate lan gia han tiep theo phai lon hon lan cu)

        public string GHI_CHU { get; set; }

        public int? TRANG_THAI { get; set; }

        public virtual ICollection<KT_GIAYPHEP_NK> KT_GIAYPHEP_NKs { get; set; }
    }
}
