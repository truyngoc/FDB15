using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchKT_IUU_XN
    {
        public int? Page { get; set; }
       
        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

        [Display(Name = "Số xác nhận")]
        public string SO_XN { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày xác nhận")]

        public DateTime? NGAY_XN { get; set; }

        [Display(Name = "Số đăng ký tàu")]
        
        public string SO_DK_TAU { get; set; }

        public IPagedList<KT_IUU_GIAYXN_NGUYENLIEU> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }
    }
}
