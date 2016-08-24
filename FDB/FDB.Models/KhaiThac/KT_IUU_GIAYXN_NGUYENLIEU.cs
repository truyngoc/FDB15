using FDB.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace FDB.Models
{
 

    public class KT_IUU_GIAYXN_NGUYENLIEU
    {

        public KT_IUU_GIAYXN_NGUYENLIEU()
        {
            this.DSKT_IUU_GIAYXN_NGUYENLIEU_DETAILs = new HashSet<KT_IUU_GIAYXN_NGUYENLIEU_DETAIL>();
        }

        //[Required]
        //[Key]
        //[Column(Order = 1)]
        //public int ID { get; set; }


        [Required(ErrorMessage = "Số xác nhận là bắt buộc nhập")]
        [Key]
        //[Column(Order = 2)]
        [Remote("check_SO_XN_Exist", "KT_IUU", HttpMethod = "POST", ErrorMessage = "Số xác nhận đã tồn tại", AdditionalFields = "IsEdit")]
        public string SO_XN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày xác nhận là bắt buộc nhập")]
        public DateTime NGAY_XN { get; set; }

        public string SO_DK_TAU { get; set; }

        [Required(ErrorMessage = "Tỉnh/TP là bắt buộc nhập")]
        public string MA_TINHTP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Thời gian khai thác từ")]

        public DateTime? TU_NGAY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Đến ngày")]

        public DateTime? DEN_NGAY { get; set; }

        public string VUNG_KHAITHAC { get; set; }

        public string CANG_DANGKY { get; set; }

        public string GHICHU { get; set; }

        public virtual DTINHTP DTINHTP { get; set; }

        public virtual ICollection<KT_IUU_GIAYXN_NGUYENLIEU_DETAIL> DSKT_IUU_GIAYXN_NGUYENLIEU_DETAILs { get; set; }

    }


    public class KT_IUU_GIAYXN_NGUYENLIEU_DETAIL
    {

        //public KT_IUU_GIAYXN_NGUYENLIEU_DETAIL()
        //{
        //    this.DM_DOITUONG_KT_IUUs = new HashSet<DM_DOITUONG_KT_IUU>();
        //}
        [Required]
        [Key]
        public int ID { get; set; }


        public string SO_XN { get; set; }
        //public int? KT_IUU_GIAYXN_NGUYENLIEUID { get; set; }

        //con thieu dm_nhom_doi tuong va doi tuong cua iuu:
       public int? ID_DM_DOITUONG_KT_IUU { get; set; }
        public decimal? KL_KHAITHAC { get; set; }

        public decimal? KL_NGUYENLIEU { get; set; }

        public string SO_CN { get; set; }

        public decimal? KL_DACHUNGNHAN { get; set; }
        public decimal? KL_TON_SAUCHUNGNHAN { get; set; }


        
        public virtual KT_IUU_GIAYXN_NGUYENLIEU KT_IUU_GIAYXN_NGUYENLIEU { get; set; }

         [ForeignKey("ID_DM_DOITUONG_KT_IUU"), DatabaseGenerated(DatabaseGeneratedOption.None)]
       public virtual DM_DOITUONG_KT_IUU DM_DOITUONG_KT_IUU { get; set; }

        //public virtual ICollection<DM_DOITUONG_KT_IUU> DM_DOITUONG_KT_IUUs { get; set; }
    }
}
