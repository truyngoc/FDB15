using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;   // for RemoteAttribute using to validate exist item in database


namespace FDB.Models
{
    public class KT_CPUE
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập Tỉnh/TP")]
        public string MA_TINHTP { get; set; }
        public string NGUOI_NHAP_PHIEU { get; set; }
        public string NGUOI_THU_PHIEU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_THU_PHIEU { get; set; }

        // thong tin tau ca
        [Required(ErrorMessage = "Số đăng ký là bắt buộc nhập")]
        public string SO_DK { get; set; }
        public string CHU_PHUONG_TIEN { get; set; }

        //[Range(0,int.MaxValue,ErrorMessage = "Số thuyền viên phải lớn hơn hoặc bằng 0")]
        public int? SO_THUYEN_VIEN { get; set; }        
        public decimal? KT_CHIEU_DAI { get; set; }

        [Required(ErrorMessage = "Tổng công suất là bắt buộc nhập")]
        public int? KT_TONG_CONG_SUAT { get; set; }
        
        [Required(ErrorMessage = "Nhóm công suất là bắt buộc nhập")]
        public int? DNHOM_TAUID { get; set; }

        // thong tin nghe, thong so ngu cu
        [Required(ErrorMessage = "Nghề khai thác là bắt buộc nhập")]
        public int? DM_NhomNgheID { get; set; }

        // re
        public decimal? RE_CHIEU_DAI_VANG_LUOI { get; set; }
        public decimal? RE_CHIEU_CAO_LUOI { get; set; }
        public decimal? RE_KICH_THUOC_MAT_LUOI_2A { get; set; }
        // keo
        public decimal? KEO_CHIEU_DAI_GIENG_PHAO { get; set; }
        public decimal? KEO_CHIEU_DAI_GIENG_CHI { get; set; }
        public decimal? KEO_KICH_THUOC_MAT_LUOI_DUT_2A { get; set; }
        // vay
        public decimal? VAY_CHIEU_DAI_LUOI { get; set; }
        public decimal? VAY_CHIEU_CAO_LUOI { get; set; }
        public decimal? VAY_KICH_THUOC_MAT_LUOI_TUNG_2A { get; set; }
        public decimal? VAY_SO_BONG_DEN { get; set; }
        public decimal? VAY_CONG_SUAT_BONG_DEN { get; set; }
        // cau
        public decimal? CAU_CHIEU_DAI_VANG_CAU { get; set; }
        public decimal? CAU_TONG_SO_LUOI_CAU { get; set; }
        // khac
        public string KHAC_KICH_THUOC_CHU_YEU { get; set; }


        // thong tin chuyen bien
        [Required(ErrorMessage = "Ngày xuất bến là bắt buộc nhập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_XUAT_BEN { get; set; }

        [Required(ErrorMessage = "Nơi cập bến là bắt buộc nhập")]
        public string NOI_CAP_BEN { get; set; }

        [Required(ErrorMessage = "Ngày cập bến là bắt buộc nhập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_CAP_BEN { get; set; }
        public string NGU_TRUONG { get; set; }
        public int? TONG_SO_ME_LUOI { get; set; }

        [Required(ErrorMessage = "Số ngày đánh cá trong chuyến là bắt buộc nhập")]
        public int? SO_NGAY_DANH_CA { get; set; }
        public int? SO_NGAY_DANH_CA_THANG_TRUOC { get; set; }
        public decimal? SAN_LUONG_CHUYEN_TAI { get; set; }

        [Required(ErrorMessage = "Tổng sản lượng là bắt buộc nhập")]
        public decimal? TONG_SAN_LUONG { get; set; }

        // san luong chuyen bien
        // tom
        public decimal? SAN_LUONG_TOM { get; set; }
        public int? GIA_BAN_TOM { get; set; }
        public decimal? THANH_TIEN_TOM { get; set; }
        // ca chon
        public decimal? SAN_LUONG_CA_CHON { get; set; }
        public int? GIA_BAN_CA_CHON { get; set; }
        public decimal? THANH_TIEN_CA_CHON { get; set; }
        // ca xo
        public decimal? SAN_LUONG_CA_XO { get; set; }
        public int? GIA_BAN_CA_XO { get; set; }
        public decimal? THANH_TIEN_CA_XO { get; set; }
        // ca tap
        public decimal? SAN_LUONG_CA_TAP { get; set; }
        public int? GIA_BAN_CA_TAP { get; set; }
        public decimal? THANH_TIEN_CA_TAP { get; set; }
        // ca ngu dai duong
        public decimal? SAN_LUONG_CA_NGU_DD { get; set; }
        public int? GIA_BAN_CA_NGU_DD { get; set; }
        public decimal? THANH_TIEN_CA_NGU_DD { get; set; }
        // Muc ong
        public decimal? SAN_LUONG_MUC_ONG { get; set; }
        public int? GIA_BAN_MUC_ONG { get; set; }
        public decimal? THANH_TIEN_MUC_ONG { get; set; }
        // Muc nang
        public decimal? SAN_LUONG_MUC_NANG { get; set; }
        public int? GIA_BAN_MUC_NANG { get; set; }
        public decimal? THANH_TIEN_MUC_NANG { get; set; }
        // Khac
        public decimal? SAN_LUONG_KHAC { get; set; }
        public int? GIA_BAN_KHAC { get; set; }
        public decimal? THANH_TIEN_KHAC { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_NM { get; set; }
        public string NGUOI_NM { get; set; }

        public virtual DNHOM_TAU DNHOM_TAU { get; set; }
        public virtual DM_NHOMNGHE DM_NHOMNGHE { get; set; }
        public virtual DTINHTP DTINHTP { get; set; }
    }
}
