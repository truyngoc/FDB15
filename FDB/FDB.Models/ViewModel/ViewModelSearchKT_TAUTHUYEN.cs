using System;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace FDB.Models
{
    public class ViewModelSearchKT_TAUTHUYEN
    {
        public int? Page { get; set; }
        public string SO_DK { get; set; }
        public string CHU_PHUONG_TIEN { get; set; }
        public string MA_TINHTP { get; set; }
        public string MA_QUANHUYEN { get; set; }
        public string MA_PHUONGXA { get; set; }
        public int? DCONG_DUNG_TAUID { get; set; }
        public int? DNHOM_TAUID { get; set; }
        public int? DTINH_TRANG_TAUID { get; set; }
        public int? DTTDANG_KIEMID { get; set; }
        public int? DTINH_TRANG_DANG_KIEMID { get; set; }

        public int? DNHOM_NGHECHINHID { get; set; }
        public int? DNHOM_NGHEPHUID { get; set; }
        public int? KT_TONG_CONG_SUAT_TU { get; set; }
        public int? KT_TONG_CONG_SUAT_DEN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] 
        public DateTime? NGAY_DK_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] 
        public DateTime? NGAY_DK_DEN { get; set; }

        public IPagedList<KT_TAUTHUYEN> SearchResults { get; set; }

        [Display(Name = "Tìm kiếm")]
        public string SearchButton { get; set; }
    }
}
