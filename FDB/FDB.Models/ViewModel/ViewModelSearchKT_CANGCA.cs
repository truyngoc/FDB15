using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
   public class ViewModelSearchKT_CANGCA
    {
        public int? Page { get; set; }


        [Display(Name = "Tên cảng")]
        public int? DM_CANGCAID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Từ ngày")]
        
        public DateTime? TU_NGAY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Đến ngày")]
        
        public DateTime? DEN_NGAY { get; set; }

        public IPagedList<KT_CANGCA> SearchResults { get; set; }
    }
}
