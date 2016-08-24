using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class ViewModelSearchKT_CANGCA_THONGKE
    {
        public int? DM_CANGCAID { get; set; }

          [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Từ ngày")]

        public DateTime? TU_NGAY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Đến ngày")]

        public DateTime? DEN_NGAY { get; set; }
        

        public int? TAU_20CV { get; set; }
       
        public int? TAU_50V { get; set; }

       
        public int? TAU_90CV { get; set; }

     
        public int? TAU_250V { get; set; }

      
        public int? TAU_400V { get; set; }

      
        public int? TAU_TREN_400V { get; set; }



    
        public int? TAU_KHAC { get; set; }


     
        public decimal? SANLUONG_CA { get; set; }

       
        public decimal? SANLUONG_MUC { get; set; }

      
        public decimal? SANLUONG_TOM { get; set; }

      
        public decimal? SANLUONG_KHAC { get; set; }


       
        public decimal? HANG_NUOCDA { get; set; }



        
        public decimal? HANG_XANGDAU { get; set; }

      
        public decimal? HANG_NUOCNGOT { get; set; }

        
        public decimal? HANG_KHAC { get; set; }

       
    }
}
