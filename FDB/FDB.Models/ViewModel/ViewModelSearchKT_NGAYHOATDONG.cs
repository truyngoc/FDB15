using System;
using PagedList;
using System.ComponentModel.DataAnnotations;

namespace FDB.Models
{
    public class ViewModelSearchKT_NGAYHOATDONG
    {
        public int? Page { get; set; }

        public int? DM_NhomNgheID { get; set; }

        public int? DNHOM_TAUID { get; set; }

        public int? NAM { get; set; }

        public int? THANG { get; set; }
        public string MA_TINHTP { get; set; }
        public string SearchButton { get; set; }
        public IPagedList<KT_NGAYHOATDONG> SearchResults { get; set; }
    }
}
