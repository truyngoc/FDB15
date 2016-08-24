using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchKT_KHUNEODAU
    {
        public int? Page { get; set; }
        
        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

        [Display(Name = "Tên khu neo đậu")]
        public string TEN_KHUNEODAU { get; set; }


        [Display(Name = "Loại khu neo đậu")]
        public int? DPHAN_LOAI_KHUNEODAUID { get; set; }

        [Display(Name = "Địa chỉ khu neo đậu")]
        [MaxLength(500)]
        public string DIA_CHI { get; set; }

        [Display(Name = "Quận/huyện/thị xã")]
        public string MA_QUANHUYEN { get; set; }

        public string MA_PHUONGXA { get; set; }
        public IPagedList<KT_KHUNEODAU> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }
    }
}
