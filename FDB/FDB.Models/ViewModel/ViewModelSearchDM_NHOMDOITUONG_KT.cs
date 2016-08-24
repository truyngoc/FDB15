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
    public class ViewModelSearchDM_NHOMDOITUONG_KT
    {

        public int? Page { get; set; }


        [Display(Name = "Loại mặt nước")]
        public int? LOAI_MAT_NUOC { get; set; }

        [Display(Name = "Tên nhóm")]
        public string TEN_NHOM { get; set; }

      

        public IPagedList<DM_NHOMDOITUONG_KT> SearchResults { get; set; }
    }
}
