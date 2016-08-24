using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class ViewModelSearchKT_SANLUONG
    {
        public int? Page { get; set; }

        [Display(Name = "Đơn vị")]
        public string DonVi { get; set; }

        [Display(Name = "Tháng")]
        public int? Thang { get; set; }

        [Display(Name = "Năm")]
        public int? Nam { get; set; }

        [Display(Name = "Nhóm Đối tượng")]
        public int? NhomDoiTuongKhaiThac { get; set; }

        [Display(Name = "Nghề")]
        public int? NgheKhaiThac { get; set; }

        public int? LoaiCongSuat { get; set; }

        public IPagedList<KT_SANLUONG> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }



    }


}
