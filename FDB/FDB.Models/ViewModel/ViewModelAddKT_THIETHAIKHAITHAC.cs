using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;
using FDB.Common;
using System.Web.Mvc;
using FDB.Models;

namespace FDB.Models
{
   public class ViewModelAddKT_THIETHAIKHAITHAC
    {
        [Required(ErrorMessage = "Số đăng ký tàu bắt buộc nhập")]
        [Display(Name = "Số đăng ký tàu")]
       public string SO_DK_TAU { get; set; }

        [Display(Name = "Số thuyền viên")]
        [Range(0, int.MaxValue, ErrorMessage = "Số thuyền viên bắt buộc lớn hơn 0")]
        public int? SO_THUYENVIEN { get; set; }

        [Display(Name = "Khu vực gặp nạn")]
        public string KHUVUC_GAPNAN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Thời gian gặp nạn")]
        [FDB.Common.ValidateDateLessThanDateNow(ErrorMessage = "Thời gian gặp nạn không được lớn hơn ngày hiện tại")]
        public DateTime? TG_GAPNAN { get; set; }

         
        public List<CheckBoxListItem> SUCOVETAU { get; set; }

       
        public List<CheckBoxListItem> SUCOVENGUOI { get; set; }

        [Display(Name = "Cơ quan xử lý")]
        public string COQUAN_XULY { get; set; }

        [Display(Name = "Thiệt hại ước tính")]
        
        
       

        public decimal? THIETHAI_UOCTINH { get; set; }


        public string VIDO { get; set; }

        public string KINHDO { get; set; }


        public string TAU_KHAC { get; set; }

        public string NGUOI_KHAC { get; set; }

        public int? SO_NGUOI_MAT_TICH { get; set; }
        public int? SO_NGUOI_CHET { get; set; }

                public ViewModelAddKT_THIETHAIKHAITHAC()
            {
                  
                SUCOVETAU = new List<CheckBoxListItem>();

                SUCOVENGUOI = new List<CheckBoxListItem>();
            }
    }

}
