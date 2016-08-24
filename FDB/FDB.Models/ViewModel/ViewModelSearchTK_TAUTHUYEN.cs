using System;
using PagedList;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FDB.Models
{
    public class ViewModelSearchTK_TAUTHUYEN
    {
        public string MA_TINHTP { get; set; }
        public string MA_QUANHUYEN { get; set; }
        public string MA_PHUONGXA { get; set; }
        public int? DCONG_DUNG_TAUID { get; set; }

        public int? DVAT_LIEU_VOID { get; set; }   

        public Dictionary<string, int> F { get; set; }
        public Dictionary<string, int> Sum_F_By_Fishery { get; set; }

        public Dictionary<string, int> F_by_Status { get; set; }

        public Dictionary<string, int> Sum_F_by_Status { get; set; }

        public Dictionary<string, int> A { get; set; }
        public Dictionary<string, double> CPUE { get; set; }
        public Dictionary<string, double> BAC { get; set; }
        public Dictionary<string, double> SL { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_DK_TU { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NGAY_DK_DEN { get; set; }

        public int? DTINH_TRANG_TAUID { get; set; }
    }
}
