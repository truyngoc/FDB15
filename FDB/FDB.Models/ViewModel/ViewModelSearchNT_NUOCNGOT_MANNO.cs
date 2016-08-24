using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchNT_NUOCNGOT_MANNO
    {
        public int? Page { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public String TThanhPho { get; set; }

        [Display(Name = "Quận/huyện/thị xã")]
        public String Qhuyen { get; set; }

        [Display(Name = "Tháng")]
        public int? Thang { get; set; }

        [Display(Name = "Năm")]
        public int? Nam { get; set; }

        [Display(Name = "Đối tượng nuôi")]
        public int? DoiTuongNuoi { get; set; }


        [Display(Name = "Nhóm đối tượng nuôi")]
        public int? NhomDoiTuongNuoi { get; set; }

        [Display(Name = "Mô hình nuôi")]
        public int? MoHinhNuoi { get; set; }
        public IPagedList<NT_NUOCNGOT_MANNO> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }
    }
}
