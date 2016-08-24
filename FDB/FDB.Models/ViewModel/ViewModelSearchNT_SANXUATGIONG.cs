using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;


namespace FDB.Models
{
    public class ViewModelSearchNT_SANXUATGIONG
    {
        public int? Page { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public String TThanhPho { get; set; }

        [Display(Name = "Tháng")]
        public int? Thang { get; set; }

        [Display(Name = "Năm")]
        public int? Nam { get; set; }

        [Display(Name = "Nhóm đối tượng nuôi")]
        public int? NhomDoiTuongNuoi { get; set; }

        [Display(Name = "Đối tượng nuôi")]
        public int? DoiTuongNuoi { get; set; }

        [Display(Name = "Hình thức sản xuất")]
        public int? HinhThucSX { get; set; }
        public IPagedList<NT_SANXUATGIONG> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }
    }
    public class ViewModelBaocaoNT_SANXUATGIONG
    {
        public int? Page { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public String TThanhPho { get; set; }

        [Display(Name = "Tháng")]
        public int? Thang { get; set; }

        [Display(Name = "Năm")]
        public int? Nam { get; set; }

       
        [Display(Name = "Đối tượng nuôi")]
        public int? DoiTuongSX { get; set; }

      
        public List<ViewModelBaocaoNT_SANXUATGIONG_DETAIL> ReportResults { get; set; }

        [Display(Name = "Tra cứu")]
        public string SearchButton { get; set; }
    }
    public class ViewModelBaocaoNT_SANXUATGIONG_DETAIL
    {
        public string DOI_TUONG_SX { get; set; }
        public decimal? SAN_LUONG_GIONG { get; set; }
        public int? TONG_COSO_SX_GIONG { get; set; }
        public int? TONG_COSO_LOAI_A { get; set; }
        public int? TONG_COSO_LOAI_B { get; set; }
        public int? TONG_COSO_LOAI_C { get; set; }

        public int? TONG_TRAI_GIONG { get; set; }
        public decimal? TONG_GIONG_KIEMDICH { get; set; }
        public decimal? TONG_BOME_SX { get; set; }
        public decimal? TONG_BOME_NHAP { get; set; }
    }
}
