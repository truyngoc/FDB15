using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class KT_SUCOVETAU
    {
        public int? mat_lien_lac { get; set; }

        public int? hong_may_troi_dat { get; set; }

        public int? dam_va { get; set; }

        public int? chim_dam { get; set; }
        public int? chay_no { get; set; }

        public decimal? tong_thiet_hai { get; set; }
    }
}
