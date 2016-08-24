using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FDB.Models
{
  public  class KT_SUCOVENGUOI
    {
        public int? bi_chet { get; set; }

        public int? mat_tich { get; set; }

        public int? roi_xuong_bien   { get; set; }

        public int? tai_nan_lao_dong { get; set; }
       
    }
}
