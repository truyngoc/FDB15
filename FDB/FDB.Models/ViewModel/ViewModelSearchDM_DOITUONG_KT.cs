using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;
using FDB.Common;
using System.Web.Mvc;


namespace FDB.Models
{
   public class ViewModelSearchDM_DOITUONG_KT
    {
        public int? Page { get; set; }

        [Display(Name = "Loại khai thác")]
        public int? LOAI_KHAI_THAC { get; set; }

        [Display(Name = "Nhóm đối tượng")]
        public int? DM_NHOMDOITUONG_KTID { get; set; }

        [Display(Name = "Tên đối tượng")]
        public string TEN_DOI_TUONG { get; set; }

        public IPagedList<DM_DOITUONG_KT> SearchResults { get; set; }
    }
}
