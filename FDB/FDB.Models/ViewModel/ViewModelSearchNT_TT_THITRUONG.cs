using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
   public class ViewModelSearchNT_TT_THITRUONG
    {
        public int? Page { get; set; }


        [Display(Name = "Đối tượng nuôi")]
        public int? DM_DOITUONG_GIA_THITRUONGID { get; set; }

        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Từ ngày")]

        public DateTime? TU_NGAY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Đến ngày")]

        public DateTime? DEN_NGAY { get; set; }
  

        public IPagedList<NT_TT_THITRUONG> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}
