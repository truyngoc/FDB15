using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
   public class ViewModelSearchKT_THIETHAIKHAITHAC
    {
        public int? Page { get; set; }


        [Display(Name = "Số đăng ký tàu")]
        public string SODK_TAU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Thời gian gặp nạn")]

        public DateTime? TU_NGAY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Đến")]

        public DateTime? DEN_NGAY { get; set; }
        public IPagedList<KT_THIETHAIKHAITHAC> SearchResults { get; set; }

        public IPagedList<ViewModelAddKT_THIETHAIKHAITHAC> StatisticsResults { get; set; }
    }
}
