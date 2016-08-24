using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FDB.Common;

namespace FDB.Models
{
    public class KT_THIETHAIKHAITHAC
    {
        public KT_THIETHAIKHAITHAC()
        {
            this.SUCOVETAU = new HashSet<DSUCOVETAU>();
            this.SUCOVENGUOI = new HashSet<DSUCOVENGUOI>();
        }

        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Số đăng ký tàu bắt buộc nhập")]
        [Display(Name = "Số đăng ký tàu")]
        public string SO_DK_TAU { get; set; }

        [Display(Name = "Số thuyền viên")]
       
        public int? SO_THUYENVIEN { get; set; }

        [Display(Name = "Khu vực gặp nạn")]
        public string KHUVUC_GAPNAN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Thời gian gặp nạn")]
        public DateTime? TG_GAPNAN { get; set; }


        public string VIDO { get; set; }

        public string KINHDO { get; set; }


        public string TAU_KHAC { get; set; }

        public string NGUOI_KHAC { get; set; }

        public int? SO_NGUOI_MAT_TICH { get; set; }
        public int? SO_NGUOI_CHET { get; set; }

        public virtual ICollection<DSUCOVETAU> SUCOVETAU { get; set; }

   
        public virtual ICollection<DSUCOVENGUOI> SUCOVENGUOI { get; set; }

        [Display(Name = "Cơ quan xử lý")]
        public string COQUAN_XULY { get; set; }

        [Display(Name = "Thiệt hại ước tính")]
        public decimal? THIETHAI_UOCTINH { get; set; }

    }
}
