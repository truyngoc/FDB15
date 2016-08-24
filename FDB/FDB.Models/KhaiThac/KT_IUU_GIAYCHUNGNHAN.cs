using System;
using FDB.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace FDB.Models
{
    public class KT_IUU_GIAYCHUNGNHAN
    {

        public KT_IUU_GIAYCHUNGNHAN()
        {
            this.DSKT_IUU_GIAYCHUNGNHAN_DETAILs = new HashSet<KT_IUU_GIAYCHUNGNHAN_DETAIL>();
        }


        [Required(ErrorMessage = "Số chứng nhận là bắt buộc nhập")]
        [Key]
        [Remote("check_SO_CN_Exist", "KT_IUU", HttpMethod = "POST", ErrorMessage = "Số chứng nhận đã tồn tại", AdditionalFields = "IsEdit")]
        public string SO_CN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày chứng nhận là bắt buộc nhập")]
        public DateTime NGAY_CN { get; set; }

         [Required(ErrorMessage = "Tên doanh nghiệp là bắt buộc nhập")]
        public string TEN_DN { get; set; }

         [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
         public string MA_TINHTP { get; set; }

         public string MST { get; set; }

         public string DIACHI { get; set; }

         public string NUOC_DEN { get; set; }

         public string GHICHU { get; set; }

         public virtual DTINHTP DTINHTP { get; set; }

         public virtual ICollection<KT_IUU_GIAYCHUNGNHAN_DETAIL> DSKT_IUU_GIAYCHUNGNHAN_DETAILs { get; set; }
    }

    public class KT_IUU_GIAYCHUNGNHAN_DETAIL
    {
        [Required]
        [Key]
        public int ID { get; set; }

        public string SO_CN { get; set; }

                    
        public string SO_XN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        
        public DateTime? NGAY_XN { get; set; }
        public string SO_DK_TAU { get; set; }
        //con thieu dm_nhom_doi tuong va doi tuong cua iuu:
        public int? ID_DM_DOITUONG_KT_IUU { get; set; }

        public string MA_SP { get; set; }
        public decimal? KL_KHAITHAC { get; set; }

       

        public decimal? KL_DUOC_CHUNGNHAN { get; set; }
        public decimal? KL_NL_DA_XACNHAN { get; set; }



        public virtual KT_IUU_GIAYCHUNGNHAN KT_IUU_GIAYCHUNGNHAN { get; set; }

        [ForeignKey("ID_DM_DOITUONG_KT_IUU"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual DM_DOITUONG_KT_IUU DM_DOITUONG_KT_IUU { get; set; }

        
    }
}
