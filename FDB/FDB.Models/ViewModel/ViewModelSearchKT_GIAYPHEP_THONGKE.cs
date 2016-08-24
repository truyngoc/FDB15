using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchKT_GIAYPHEP_THONGKE
    {

        public int? Page { get; set; }

        public string MA_TINHTP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Từ ngày")]

        public DateTime? TU_NGAY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Đến ngày")]

        public DateTime? DEN_NGAY { get; set; }


        public int? TAU_20_NGHEKEO { get; set; }
        public int? TAU_20_NGHEKEO_CP { get; set; }

        public int? TAU_50_NGHEKEO { get; set; }
        public int? TAU_50_NGHEKEO_CP { get; set; }

        public int? TAU_90_NGHEKEO { get; set; }
        public int? TAU_90_NGHEKEO_CP { get; set; }

        public int? TAU_250_NGHEKEO { get; set; }
        public int? TAU_250_NGHEKEO_CP { get; set; }

        public int? TAU_400_NGHEKEO { get; set; }
        public int? TAU_400_NGHEKEO_CP { get; set; }

        public int? TAU_TREN_400_NGHEKEO { get; set; }
        public int? TAU_TREN_400_NGHEKEO_CP { get; set; }

        public int? TAU_20_NGHERE { get; set; }
        public int? TAU_20_NGHERE_CP { get; set; }

        public int? TAU_50_NGHERE { get; set; }
        public int? TAU_50_NGHERE_CP { get; set; }

        public int? TAU_90_NGHERE { get; set; }
        public int? TAU_90_NGHERE_CP { get; set; }

        public int? TAU_250_NGHERE { get; set; }
        public int? TAU_250_NGHERE_CP { get; set; }

        public int? TAU_400_NGHERE { get; set; }
        public int? TAU_400_NGHERE_CP { get; set; }

        public int? TAU_TREN_400_NGHERE { get; set; }
        public int? TAU_TREN_400_NGHERE_CP { get; set; }

        public int? TAU_20_NGHEVAY { get; set; }
        public int? TAU_20_NGHEVAY_CP { get; set; }

        public int? TAU_50_NGHEVAY { get; set; }
        public int? TAU_50_NGHEVAY_CP { get; set; }

        public int? TAU_90_NGHEVAY { get; set; }
        public int? TAU_90_NGHEVAY_CP { get; set; }

        public int? TAU_250_NGHEVAY { get; set; }
        public int? TAU_250_NGHEVAY_CP { get; set; }

        public int? TAU_400_NGHEVAY { get; set; }
        public int? TAU_400_NGHEVAY_CP { get; set; }

        public int? TAU_TREN_400_NGHEVAY { get; set; }
        public int? TAU_TREN_400_NGHEVAY_CP { get; set; }

        public int? TAU_20_NGHECAU { get; set; }
        public int? TAU_20_NGHECAU_CP { get; set; }

        public int? TAU_50_NGHECAU { get; set; }
        public int? TAU_50_NGHECAU_CP { get; set; }

        public int? TAU_90_NGHECAU { get; set; }
        public int? TAU_90_NGHECAU_CP { get; set; }

        public int? TAU_250_NGHECAU { get; set; }
        public int? TAU_250_NGHECAU_CP { get; set; }

        public int? TAU_400_NGHECAU { get; set; }
        public int? TAU_400_NGHECAU_CP { get; set; }

        public int? TAU_TREN_400_NGHECAU { get; set; }
        public int? TAU_TREN_400_NGHECAU_CP { get; set; }


        public int? TAU_20_NGHE_CANGU { get; set; }
        public int? TAU_20_NGHE_CANGU_CP { get; set; }

        public int? TAU_50_NGHE_CANGU { get; set; }
        public int? TAU_50_NGHE_CANGU_CP { get; set; }

        public int? TAU_90_NGHE_CANGU  { get; set; }
        public int? TAU_90_NGHE_CANGU_CP { get; set; }

        public int? TAU_250_NGHE_CANGU { get; set; }
        public int? TAU_250_NGHE_CANGU_CP { get; set; }

        public int? TAU_400_NGHE_CANGU { get; set; }
        public int? TAU_400_NGHE_CANGU_CP { get; set; }

        public int? TAU_TREN_400_NGHE_CANGU { get; set; }
        public int? TAU_TREN_400_NGHE_CANGU_CP { get; set; }

        public int? TAU_20_NGHEKHAC { get; set; }
        public int? TAU_20_NGHEKHAC_CP { get; set; }

        public int? TAU_50_NGHEKHAC { get; set; }
        public int? TAU_50_NGHEKHAC_CP { get; set; }

        public int? TAU_90_NGHEKHAC { get; set; }
        public int? TAU_90_NGHEKHAC_CP { get; set; }

        public int? TAU_250_NGHEKHAC { get; set; }
        public int? TAU_250_NGHEKHAC_CP { get; set; }

        public int? TAU_400_NGHEKHAC { get; set; }
        public int? TAU_400_NGHEKHAC_CP { get; set; }

        public int? TAU_TREN_400_NGHEKHAC { get; set; }
        public int? TAU_TREN_400_NGHEKHAC_CP { get; set; }

        public int? TOTAL_SO_DK_KEO { get; set; }
        public int? TOTAL_SO_DK_CP_KEO { get; set; }

        public int? TOTAL_SO_DK_VAY { get; set; }
        public int? TOTAL_SO_DK_CP_VAY { get; set; }

        public int? TOTAL_SO_DK_RE { get; set; }
        public int? TOTAL_SO_DK_CP_RE { get; set; }

        public int? TOTAL_SO_DK_CAU { get; set; }
        public int? TOTAL_SO_DK_CP_CAU { get; set; }

        public int? TOTAL_SO_DK_KHAC { get; set; }
        public int? TOTAL_SO_DK_CP_KHAC { get; set; }

    

        public Dictionary<string, int> F { get; set; }
        public Dictionary<string, int> Sum_F_By_Fishery { get; set; }

        // field dynamic
         public string NHOM_NGHE { get; set; }

         public int? TAU_20 { get; set; }
         public int? TAU_20_CP { get; set; }
         public int? TAU_20_GP_HH { get; set; }
        

         public int? TAU_50 { get; set; }
         public int? TAU_50_CP { get; set; }

         public int? TAU_50_GP_HH { get; set; }

         public int? TAU_90 { get; set; }
         public int? TAU_90_CP { get; set; }

         public int? TAU_90_GP_HH { get; set; }

         public int? TAU_250 { get; set; }
         public int? TAU_250_CP { get; set; }

         public int? TAU_250_GP_HH { get; set; }

         public int? TAU_400 { get; set; }
         public int? TAU_400_CP { get; set; }
         public int? TAU_400_GP_HH { get; set; }

        public int? TAU_MORE_400 { get; set; }
        public int? TAU_MORE_400_CP { get; set; }
        public int? TAU_MORE_400_GP_HH { get; set; }

        public int? TOTAL_SO_DK { get; set; }
        public int? TOTAL_SO_DK_CP { get; set; }
        public int? TOTAL_SO_GP_HH { get; set; }
        public List<ViewModelSearchKT_GIAYPHEP_THONGKE> StatisticsResults { get; set; }
      
    }
}
