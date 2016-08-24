using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
   public class KT_SANLUONG_DETAIL
    {
        public int ID { get; set; }


        public int ID_KHAITHAC_SANLUONG { get; set; }

        public int? ID_KHAITHAC_NHOM_DOITUONG { get; set; }

        public int? ID_KHAITHAC_NHOM_NGHE { get; set; }

        public int? ID_KHAITHAC_NHOM_CONGSUAT { get; set; }
    
        public float? SAN_LUONG { get; set; }


        [ForeignKey("ID_KHAITHAC_SANLUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual KT_SANLUONG KhaiThac_SanLuong { get; set; }
        [ForeignKey("ID_KHAITHAC_NHOM_DOITUONG"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_NHOMDOITUONG_KT DM_NhomDoiTuong_KT { get; set; }

        [ForeignKey("ID_KHAITHAC_NHOM_NGHE"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_NHOMNGHE DM_NhomNghe_KT { get; set; }

        [ForeignKey("ID_KHAITHAC_NHOM_CONGSUAT"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DNHOM_TAU DM_NhomCongSuat { get; set; }


    }
}
