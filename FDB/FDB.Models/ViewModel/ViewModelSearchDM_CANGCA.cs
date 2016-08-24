using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchDM_CANGCA
    {
        public int? Page { get; set; }


        [Display(Name = "Tên cảng")]
        public string TEN_CANG { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DIA_CHI { get; set; }

        [Display(Name = "Phân loại cảng")]
        public int? DPHAN_LOAI_CANGID { get; set; }

        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

        [Display(Name = "Quận/huyện/thị xã")]
        public string MA_QUANHUYEN { get; set; }

        public string MA_PHUONGXA { get; set; }

        public IPagedList<DM_CANGCA> SearchResults { get; set; }
    }
}
