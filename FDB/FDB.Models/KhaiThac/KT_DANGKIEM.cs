using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;   // for RemoteAttribute using to validate exist item in database

namespace FDB.Models
{
    public class KT_DANGKIEM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        public string MA_TINHTP { get; set; }

        [Required(ErrorMessage = "Hình thức kiểm tra là bắt buộc chọn")]
        public int DLOAI_KIEM_TRA_KTID { get; set; }     // co danh muc-> sua lai

        public string SO_BB_KIEM_TRA_KT { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày biên bản KTKT là bắt buộc nhập")]
        public DateTime? NGAY_KIEM_TRA { get; set; }    // ngay bien ban kiem tra ky thuat

        [Required(ErrorMessage = "Số sổ đăng kiểm là bắt buộc nhập")]
        [Remote("check_SO_SO_DANG_KIEM_Exist", "KT_DANGKIEM", HttpMethod = "POST", AdditionalFields = "IsEdit,HinhThucKiemTra,MA_TINHTP")]
        public string SO_SO_DANG_KIEM { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày cấp sổ đăng kiểm là bắt buộc nhập")]
        public DateTime? NGAY_CAP_SO_DANG_KIEM { get; set; }
        public string CO_QUAN_DANG_KIEM { get; set; }
        public string DANG_KIEM_VIEN { get; set; }
        public string SO_AN_CHI_ATKT { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_CAP_ATKT_TCA { get; set; }        // New

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [FDB.Common.FDB_DateGreaterThanOrEqualTo("NGAY_CAP_ATKT_TCA", "Ngày hết hạn không được nhỏ hơn ngày chứng nhận ATKT")]
        public DateTime? NGAY_HH_ATKT_TCA { get; set; }        // New

        [Required(ErrorMessage = "Kết luận đăng kiểm là bắt buộc nhập")]
        public int? DTINH_TRANG_DANG_KIEMID { get; set; }

        [Required(ErrorMessage = "Cấp tàu là bắt buộc nhập")]
        public int? DCAP_TAU_DANG_KIEMID { get; set; }     // New - ? chua co danh muc

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày hết hạn đăng kiểm là bắt buộc nhập")]
        public DateTime? HAN_DANG_KIEM { get; set; }

        public string GHI_CHU { get; set; }
        public int? DA_DANG_KY { get; set; }   //  New - tau thuyen dang kiem, da mang di dang ky thi cap nhat 1

        // dac diem ky thuat
        //Tên tàu
        public string TEN_TAU { get; set; }
        //Số đăng ký        
        public string SO_DK { get; set; }
        // Noi dang ky
        public string NOI_DANG_KY { get; set; }
        //Số thuyền viên
        public int? SO_THUYEN_VIEN { get; set; }
        //Công dụng tàu cá
        public int? DCONG_DUNG_TAUID { get; set; }
        //Chủ phương tiện
        public string CHU_PHUONG_TIEN { get; set; }
        //Cơ sở đóng/sửa tàu thuyền
        public string KT_NOI_DONG { get; set; }
        //Năm đóng
        public int? KT_NAM_DONG { get; set; }
        //Tổng công suất (CV)
        [Required(ErrorMessage="Tổng công suất là bắt buộc nhập")]
        public int? KT_TONG_CONG_SUAT { get; set; }

        [Required(ErrorMessage = "Nhóm tàu là bắt buộc nhập")]
        public int? DNHOM_TAUID { get; set; }

        //Lmax, m
        public decimal? KT_CHIEU_DAI { get; set; }
        //Bmax, m
        public decimal? KT_CHIEU_RONG { get; set; }
        //D, m
        public decimal? KT_CHIEU_CAO { get; set; }
        //Ltk, m
        public decimal? KT_CHIEU_DAI_TK { get; set; }
        ////Btk, m
        //public decimal? CHIEU_RONG_TK { get; set; }
        //d, m
        public decimal? KT_MON_NUOC { get; set; }
        //Mạn khô f, m
        public decimal? KT_MAN_KHO { get; set; }        
        //Vật liệu vỏ
        public int? DVAT_LIEU_VOID { get; set; }
        //Tốc độ tàu
        public int? KT_TOC_DO_TAU { get; set; }
        
        [Required(ErrorMessage="Số máy tàu là bắt buộc nhập")]
        [Range(1, 5, ErrorMessage = "Số lượng máy tàu trong khoảng từ 1 -> 5")]
        public int? KT_SO_MAY_TAU { get; set; }

        #region "May tau"
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
        #endregion

            public virtual DLOAI_KIEM_TRA_KT DLOAI_KIEM_TRA_KT { get; set; }
            public virtual DCONG_DUNG_TAU DCONG_DUNG_TAU { get; set; }
            public virtual DTINHTP DTINHTP { get; set; }
            public virtual DVAT_LIEU_VO DVAT_LIEU_VO { get; set; }
            public virtual DCAP_TAU_DANG_KIEM DCAP_TAU_DANG_KIEM { get; set; }
            public virtual DTINH_TRANG_DANG_KIEM DTINH_TRANG_DANG_KIEM { get; set; }

        
    }

    public class RegisterShip
    {
        public int TAU_PHAI_DANG_KIEM { get; set; }

        [Common.FDB_RequiredIfEqualTo("TAU_PHAI_DANG_KIEM", "2", "Phải nhập số sổ đăng kiểm")]
        public string SO_SO_DANG_KIEM { get; set; }
    }
}
