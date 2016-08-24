using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;


namespace FDB.Models
{
    public class ViewModelSearchNT_NUOILONGBE
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
        public IPagedList<NT_NUOILONGBE> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }
    }

    public class ViewModelBaoCaoNT_NUOILONGBE
    {
        public int? Page { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public String TThanhPho { get; set; }

        [Display(Name = "Tháng")]
        public int? Thang { get; set; }

        [Display(Name = "Năm")]
        public int? Nam { get; set; }

        [Display(Name = "Loại mặt nước")]
        public int? LoaiMatNuoc { get; set; }

        public int? LoaiBaoCao { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }

        public List<ViewModelBaoCaoNT_NUOILONGBE_Detail> ReportResults { get; set; }

        [Display(Name = "Tra cứu")]
        public string SearchButton { get; set; }
    }
    public class ViewModelBaoCaoNT_NUOILONGBE_Detail
    {
        //lồng bè
        public string DOI_TUONG_NUOI { get; set; }
        public Decimal? THE_TICH_LONG { get; set; }
        public Decimal? SAN_LUONG_LONG { get; set; }
        public int? SO_LUONG_LONG { get; set; }
        public decimal? MAT_DO_THA { get; set; }

      
    }
}
