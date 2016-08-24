using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchKT_DANGKIEM
    {
        public int? Page { get; set; }
        public string SO_SO_DANG_KIEM { get; set; }
        public int? DLOAI_KIEM_TRA_KTID { get; set; }

        public int? DTINH_TRANG_DANG_KIEMID { get; set; }
        public int? DCAP_TAU_DANG_KIEMID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HH_DK_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HH_DK_DEN { get; set; }

        public string SO_DK { get; set; }
        public string CHU_PHUONG_TIEN { get; set; }
        public string MA_TINHTP { get; set; }
        public int? DCONG_DUNG_TAUID { get; set; }

        
        public string SearchButton { get; set; }
        public IPagedList<KT_DANGKIEM> SearchResults { get; set; }        
    }

    public class ViewModelSearchKT_DANGKIEM_ThongKe
    {
        public string MA_TINHTP { get; set; }
        public int? DCONG_DUNG_TAUID { get; set; }
        public int? DVAT_LIEU_VOID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_KIEM_TRA_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_KIEM_TRA_DEN { get; set; }

        public Dictionary<string, int> BaoCaoDangKiem { get; set; }
    }
}
