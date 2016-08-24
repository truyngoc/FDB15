using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchKT_KHU_BAO_TON
    {
        public int? Page { get; set; }


        [Display(Name = "Tên khu bảo tồn")]
        public string TEN_KHU_BAO_TON { get; set; }

        [Display(Name = "Địa chỉ")]
        [MaxLength(500)]
        public string DIA_CHI { get; set; }

        [Display(Name = "Loại khu bảo tồn")]
        public int? DKHU_BAO_TONID { get; set; }

       
        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }


        [Display(Name = "Quận/huyện")]
        public string MA_QUANHUYEN { get; set; }

        public string MA_PHUONGXA { get; set; }

        public IPagedList<KT_KHUBAOTON> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}
