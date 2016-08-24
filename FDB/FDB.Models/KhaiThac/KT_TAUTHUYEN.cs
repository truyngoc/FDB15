using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;   // for RemoteAttribute using to validate exist item in database

namespace FDB.Models
{
    public class KT_TAUTHUYEN
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage="Số đăng ký là bắt buộc nhập")]
        [Key]
        [Remote("check_SO_DK_Exist", "KT_TAUTHUYEN", HttpMethod = "POST", ErrorMessage = "Số đăng ký đã tồn tại", AdditionalFields = "IsEdit")]
        public string SO_DK { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        [Required(ErrorMessage="Ngày đăng ký là bắt buộc nhập")]
        public DateTime NGAY_DK { get; set; }
   

        [Required(ErrorMessage = "Tên chủ phương tiện là bắt buộc nhập")]
        public string CHU_PHUONG_TIEN { get; set; }
        public string SO_CMND { get; set; }
        public string DIEN_THOAI { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        public string MA_TINHTP { get; set; }

        [Required(ErrorMessage = "Quận/huyện là bắt buộc nhập")]
        public string MA_QUANHUYEN { get; set; }

        [Required(ErrorMessage = "Phường/xã là bắt buộc nhập")]
        public string MA_PHUONGXA { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc nhập")]
        public string DIA_CHI { get; set; }
        public string TEN_TAU { get; set; }
        public string HO_HIEU { get; set; }

        [Required(ErrorMessage = "Công dụng tàu là bắt buộc nhập")]
        public int DCONG_DUNG_TAUID { get; set; }

        [Required(ErrorMessage = "Nhóm tàu là bắt buộc nhập")]
        public int DNHOM_TAUID { get; set; }      
                                
        [Range(1,Int32.MaxValue,ErrorMessage="Không được nhỏ hơn 1")]
        public int? SO_THUYEN_VIEN { get; set; }

        [Required(ErrorMessage = "Tình trạng tàu là bắt buộc nhập")]
        public int DTINH_TRANG_TAUID { get; set; }

        //[Required(ErrorMessage = "Nghề khai thác chính là bắt buộc nhập")]
        public int? NGHE_CHINH_ID { get; set; }

        public int? NGHE_PHU_ID { get; set; }

        public string SO_SO_DANG_KIEM { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_CAP_SO_DANG_KIEM { get; set; }             
        public int? DTINH_TRANG_DANG_KIEMID { get; set; }
        public int? DCAP_TAU_DANG_KIEMID { get; set; }  
        public string CO_QUAN_DANG_KIEM { get; set; }
        

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? HAN_DANG_KIEM { get; set; }
        public string DANG_KIEM_VIEN { get; set; }       
        public string SO_AN_CHI_ATKT { get; set; }


        [Required(ErrorMessage = "Chiều dài tàu là bắt buộc nhập")]
        [Range(0,Int32.MaxValue,ErrorMessage = "Không được nhỏ hơn 0")]
        public decimal KT_CHIEU_DAI { get; set; }

        [Required(ErrorMessage = "Chiều rộng tàu là bắt buộc nhập")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Không được nhỏ hơn 0")]
        public decimal KT_CHIEU_RONG { get; set; }

        [Required(ErrorMessage = "Chiều cao tàu là bắt buộc nhập")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Không được nhỏ hơn 0")]
        public decimal KT_CHIEU_CAO { get; set; }

        [Required(ErrorMessage = "Mạn khô là bắt buộc nhập")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Không được nhỏ hơn 0")]
        public decimal KT_MAN_KHO { get; set; }

        [Required(ErrorMessage = "Mớn nước là bắt buộc nhập")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Không được nhỏ hơn 0")]
        public decimal KT_MON_NUOC { get; set; }
        
        [Range(0, Int32.MaxValue, ErrorMessage = "Không được nhỏ hơn 0")]
        public int? KT_TAN_DANG_KY { get; set; }

        [Required(ErrorMessage = "Vật liệu vỏ là bắt buộc nhập")]
        public int DVAT_LIEU_VOID { get; set; }

        [Required(ErrorMessage="Tổng công suất là bắt buộc nhập")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Không được nhỏ hơn 0")]
        public int? KT_TONG_CONG_SUAT { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Không được nhỏ hơn 0")]
        public int? KT_TOC_DO_TAU { get; set; }
        public string KT_NOI_DONG { get; set; }

        [Range(1900, 2100, ErrorMessage = "Năm phải thuộc khoảng từ 1900 đến 2100")]
        public int? KT_NAM_DONG { get; set; }
        public string KT_TEN_NGUOI_DONG { get; set; }
        public string KT_MAU_THIET_KE { get; set; }
        public string KT_CQ_THIET_KE { get; set; }
        public string KT_SO_PD_TK { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? KT_NGAY_PD_TK { get; set; }
        public string KT_TEN_THIET_KE { get; set; }
        public string KT_LOAI_THIET_KE { get; set; }

        [Range(1900, 2100, ErrorMessage = "Năm phải thuộc khoảng từ 1900 đến 2100")]
        public int? KT_NAM_THIET_KE { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập số máy tàu")]
        [Range(1, 5, ErrorMessage = "Số lượng máy tàu trong khoảng từ 1 -> 5")]
        public int? KT_SO_MAY_TAU { get; set; }
                
        // may 1
        public string M1_KY_HIEU_MAY { get; set; }      
        public string M1_SO_MAY { get; set; }
        public string M1_NOI_SX { get; set; }

        [Range(1900, 2100, ErrorMessage = "Năm phải thuộc khoảng từ 1900 đến 2100")]
        public int? M1_NAM_CHE_TAO { get; set; }
        public string M1_HANG_MAY { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Không được nhỏ hơn 0")]
        public int? M1_VONG_QUAY { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Không được nhỏ hơn 0")]
        public int? M1_CONG_SUAT { get; set; }

        // may 2
        public string M2_KY_HIEU_MAY { get; set; }      
        public string M2_SO_MAY { get; set; }
        public string M2_NOI_SX { get; set; }
        public int? M2_NAM_CHE_TAO { get; set; }
        public string M2_HANG_MAY { get; set; }
        public int? M2_VONG_QUAY { get; set; }
        public int? M2_CONG_SUAT { get; set; }

        // may 3
        public string M3_KY_HIEU_MAY { get; set; }      
        public string M3_SO_MAY { get; set; }
        public string M3_NOI_SX { get; set; }
        public int? M3_NAM_CHE_TAO { get; set; }
        public string M3_HANG_MAY { get; set; }
        public int? M3_VONG_QUAY { get; set; }
        public int? M3_CONG_SUAT { get; set; }

        // may 4
        public string M4_KY_HIEU_MAY { get; set; }      
        public string M4_SO_MAY { get; set; }
        public string M4_NOI_SX { get; set; }
        public int? M4_NAM_CHE_TAO { get; set; }
        public string M4_HANG_MAY { get; set; }
        public int? M4_VONG_QUAY { get; set; }
        public int? M4_CONG_SUAT { get; set; }

        // may 5
        public string M5_KY_HIEU_MAY { get; set; }      
        public string M5_SO_MAY { get; set; }
        public string M5_NOI_SX { get; set; }
        public int? M5_NAM_CHE_TAO { get; set; }
        public string M5_HANG_MAY { get; set; }
        public int? M5_VONG_QUAY { get; set; }
        public int? M5_CONG_SUAT { get; set; }
        
        public bool? IsCapPhep { get; set; }
        public bool? IsDangKiem { get; set; }   // true: neu truong so so dang kiem <> empty
        public virtual DCONG_DUNG_TAU DCONG_DUNG_TAU { get; set; }
        public virtual DTINH_TRANG_TAU DTINH_TRANG_TAU { get; set; }
        public virtual DNHOM_TAU DNHOM_TAU { get; set; }
        public virtual DCAP_TAU_DANG_KIEM DCAP_TAU_DANG_KIEM { get; set; }
        public virtual DTINH_TRANG_DANG_KIEM DTINH_TRANG_DANG_KIEM { get; set; }
        public virtual DVAT_LIEU_VO DVAT_LIEU_VO { get; set; }
        public virtual DTINHTP DTINHTP { get; set; }
        public virtual DQUANHUYEN DQUANHUYEN { get; set; }
        public virtual DPHUONGXA DPHUONGXA { get; set; }
        public virtual DLOAI_KIEM_TRA_KT DLOAI_KIEM_TRA_KT { get; set; }

        public virtual ICollection<KT_GIAYPHEP> KT_GIAYPHEPs { get; set; }      

    }
}
