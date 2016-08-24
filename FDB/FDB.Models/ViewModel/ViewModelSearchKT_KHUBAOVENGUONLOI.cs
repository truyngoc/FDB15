using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchKT_KHUBAOVENGUONLOI
    {
        public int? Page { get; set; }

        [Display(Name = "Tên khu bảo tồn")]
        public string TEN_KHUBAOVENGUONLOI { get; set; }

        [Display(Name = "Địa chỉ")]
        [MaxLength(500)]
        public string DIA_CHI { get; set; }

       
        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }


        [Display(Name = "Quận/huyện")]
        public string MA_QUANHUYEN { get; set; }

        public string MA_PHUONGXA { get; set; }
        public IPagedList<KT_KHUBAOVENGUONLOI> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}
