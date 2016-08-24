using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;   // for RemoteAttribute using to validate exist item in database

namespace FDB.Models
{
    public class KT_BAC
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Ngày điều tra là bắt buộc nhập")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_DIEU_TRA { get; set; }
        public string CAN_BO_DIEU_TRA { get; set; }

        [Required(ErrorMessage = "Nghề khai thác là bắt buộc nhập")]
        public int? DM_NhomNgheID { get; set; }

        [Required(ErrorMessage = "Nhóm công suất là bắt buộc nhập")]
        public int? DNHOM_TAUID { get; set; }

        [Required(ErrorMessage = "Năm là bắt buộc nhập")]
        public int? NAM { get; set; }

        [Required(ErrorMessage = "Tháng là bắt buộc nhập")]
        public int? THANG { get; set; }

        [Required(ErrorMessage = "Số tàu chọn mẫu là bắt buộc nhập")]
        public int? SO_TAU_CHON_MAU { get; set; }

        
        [Required(ErrorMessage = "Số tàu chọn mẫu đi biển là bắt buộc nhập")]
        public int? SO_TAU_CHON_MAU_DI_BIEN { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_NM { get; set; }
        public string NGUOI_NM { get; set; }

        public virtual DNHOM_TAU DNHOM_TAU { get; set; }
        public virtual DM_NHOMNGHE DM_NHOMNGHE { get; set; }
        public virtual DTINHTP DTINHTP { get; set; }
    }
}
