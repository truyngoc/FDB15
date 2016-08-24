using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchNT_NUOC_MANLO
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
        public int? HinhThucNuoi { get; set; }

        public int? LoaiBaoCao { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }

        public IPagedList<NT_NUOC_MANLO> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }
    }

    public class ViewModelBaoCaoNT_NUOC_MANLO
    {
        public int? Page { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public String TThanhPho { get; set; }

        [Display(Name = "Tháng")]
        public int? Thang { get; set; }

        [Display(Name = "Năm")]
        public int? Nam { get; set; }

        [Display(Name = "Hình thức nuôi")]
        public int? HinhThucNuoi { get; set; }

        public int? LoaiBaoCao { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }

        public List<ViewModelBaoCaoNT_NUOC_MANLO_Detail> ReportResults { get; set; }

        [Display(Name = "Tra cứu")]
        public string SearchButton { get; set; }
    }
    public class ViewModelBaoCaoNT_NUOC_MANLO_Detail
    {
        //lồng bè
        public string DOI_TUONG_NUOI { get; set; }
        public decimal? THE_TICH_LONG { get; set; }
        public decimal? SAN_LUONG_LONG { get; set; }
        public int? SO_LUONG_LONG { get; set; }
        public decimal? MAT_DO_THA { get; set; }

        //khác:
        public decimal? DIEN_TICH_THANUOI_TRONGKY_KHAC_1 { get; set; }
        public decimal? NUOI_TRONG_QUY_HOACH_KHAC_1 { get; set; }
        public decimal? NUOI_NGOAI_QUY_HOACH_KHAC_1 { get; set; }
        public decimal? DIEN_TICH_GAP_TUONGTU_KHAC_1 { get; set; }
        public decimal? SAN_LUONG_KHAC_1 { get; set; }
        public decimal? SO_LUONG_GIONG_THA_KHAC_1 { get; set; }

        public decimal? DIEN_TICH_THANUOI_TRONGKY_KHAC_2 { get; set; }
        public decimal? NUOI_TRONG_QUY_HOACH_KHAC_2 { get; set; }
        public decimal? NUOI_NGOAI_QUY_HOACH_KHAC_2 { get; set; }
        public decimal? DIEN_TICH_GAP_TUONGTU_KHAC_2 { get; set; }
        public decimal? SAN_LUONG_KHAC_2 { get; set; }
        public decimal? SO_LUONG_GIONG_THA_KHAC_2 { get; set; }



    }
}
