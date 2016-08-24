using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DM_DOITUONG_KT_IUU
    {

        //public DM_DOITUONG_KT_IUU()
        //{
        //    this.KT_IUU_GIAYXN_NGUYENLIEU_DETAILs = new HashSet<KT_IUU_GIAYXN_NGUYENLIEU_DETAIL>();
        //}

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

       // public int? KT_IUU_GIAYXN_NGUYENLIEU_DETAILID { get; set; }


        //public virtual KT_IUU_GIAYXN_NGUYENLIEU_DETAIL KT_IUU_GIAYXN_NGUYENLIEU_DETAILs { get; set; }

      public virtual ICollection<KT_IUU_GIAYXN_NGUYENLIEU_DETAIL> KT_IUU_GIAYXN_NGUYENLIEU_DETAILs { get; set; }

      public virtual ICollection<KT_IUU_GIAYCHUNGNHAN_DETAIL> KT_IUU_GIAYCHUNGNHAN_DETAILs { get; set; }
    }
}
