using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchKT_IUU_CN
    {
        public int? Page { get; set; }

        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

        [Display(Name = "Số xác nhận")]
        public string SO_CN { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày chứng nhận")]

        public DateTime? NGAY_CN { get; set; }

        [Display(Name = "Tên doanh nghiệp")]

        public string TEN_DN { get; set; }

        [Display(Name = "Mã số thuế")]

        public string MST { get; set; }

        [Display(Name = "Địa chỉ")]

        public string DIACHI { get; set; }

        public IPagedList<KT_IUU_GIAYCHUNGNHAN> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }
    }
}
