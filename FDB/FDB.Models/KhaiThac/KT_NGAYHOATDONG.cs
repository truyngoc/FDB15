using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;   // for RemoteAttribute using to validate exist item in database

namespace FDB.Models
{
    public class KT_NGAYHOATDONG
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Cán bộ điều tra là bắt buộc nhập")]
        public string CAN_BO_DIEU_TRA { get; set; }

        [Required(ErrorMessage = "Nghề khai thác là bắt buộc nhập")]
        public int? DM_NhomNgheID { get; set; }

        [Required(ErrorMessage = "Nhóm công suất là bắt buộc nhập")]
        public int? DNHOM_TAUID { get; set; }

        [Required(ErrorMessage = "Năm là bắt buộc nhập")]
        public int? NAM { get; set; }

        [Required(ErrorMessage = "Tháng là bắt buộc nhập")]
        public int? THANG { get; set; }

        [Required(ErrorMessage = "Số ngày hoạt động tiềm năng là bắt buộc nhập")]
        [Range(0, 31, ErrorMessage = "Số ngày phải nằm trong khoảng 0 -> 31")]
        public int? SO_NGAY { get; set; }

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
