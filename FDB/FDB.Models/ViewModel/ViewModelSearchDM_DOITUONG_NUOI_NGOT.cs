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
    public class ViewModelSearchDM_DOITUONG_NUOI_NGOT
    {
        public int? Page { get; set; }


        [Display(Name = "Hình thức nuôi")]
        public int? LOAI_DOI_TUONG { get; set; }

        [Display(Name = "Tên đối tượng")]
        public string TEN_DOI_TUONG { get; set; }


        public IPagedList<DM_DOITUONG_NUOI_NGOT> SearchResults { get; set; }
    }
}
