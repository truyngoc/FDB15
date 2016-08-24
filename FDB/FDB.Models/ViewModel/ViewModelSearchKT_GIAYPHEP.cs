using System;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace FDB.Models
{
    public class ViewModelSearchKT_GIAYPHEP
    {
        public int? Page { get; set; }
        public string SO_DK { get; set; }
        public string SO_GP { get; set; }
        public string MA_TINHTP { get; set; }
        public string CANG_DANG_KY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_GP_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_GP_DEN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HL_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HL_DEN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HHL_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_HHL_DEN { get; set; }

        public IPagedList<KT_GIAYPHEP> SearchResults { get; set; }

        public string SearchButton { get; set; }
    }
}
