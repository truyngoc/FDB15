using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
   public  class ViewModelSearchKT_DONGSUA_TAUTHUYEN
    {
        public int? Page { get; set; }

        [Display(Name = "Tên cơ sở")]
        public string TenCoSo { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Tên chủ cơ sở")]
        public string TenChuCoSo { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public String TThanhPho { get; set; }

        [Display(Name = "Quận/Huyện")]
        public String Qhuyen { get; set; }

        [Display(Name = "Phưỡng/Xã")]
        public String Phuongxa { get; set; }

        public IPagedList<KT_DONGSUA_TAUTHUYEN> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }
    }
}
