using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
   public class KT_CANGCA
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Cảng cá bắt buộc nhập")]
        [Display(Name = "Cảng cá")]
        public int DM_CANGCAID { get; set; }
        
        //[Display(Name = "Tổng trọng lượng hàng qua cảng")]
        //[Range(0, float.MaxValue, ErrorMessage = "Tổng trọng lượng hàng qua cảng bắt buộc lớn hơn 0")]
        //public float? TONG_TRONGLUONG_HANG_QUACANG { get; set; }

        //[Display(Name = "Sản lượng thủy sản")]
        //[Range(0, float.MaxValue, ErrorMessage = "Sản lượng thủy sản bắt buộc lớn hơn 0")]
        //public float? SANLUONG_THUYSAN { get; set; }

        //[Display(Name = "Số lượt tàu cập cảng")]
        //[Range(0, int.MaxValue, ErrorMessage = "Số lượt tàu cập cảng bắt buộc lớn hơn 0")]
        //public int? SO_LUOTTAU_CAPCANG { get; set; }

        
        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu < 20 CV bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu < 20 CV phải là số nguyên")]
        public int? TAU_20CV { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu 20 -< 50 CV bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu 20 -< 50 CV phải là số nguyên")]
        public int? TAU_50V { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu 50 -< 90 CV bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu 50 -< 90 CV phải là số nguyên")]
        public int? TAU_90CV { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu 90 -< 250 CV bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu 90 -< 250 CV phải là số nguyên")]
        public int? TAU_250V { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu 250 -< 400 CV bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu 250 -< 400 CV phải là số nguyên")]
        public int? TAU_400V { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu > 400 CV bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu > 400 CV phải là số nguyên")]
        public int? TAU_TREN_400V { get; set; }


        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu 400 -< 700 CV bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu 400 -< 700 CV phải là số nguyên")]
        public int? TAU_700V { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu 700 -< 1000 CV bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu 700 -< 1000 CV phải là số nguyên")]
        public int? TAU_1000V { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu => 1000 CV bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu >= 1000 CV phải là số nguyên")]
        public int? TAU_TREN_1000V { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượt tàu khác bắt buộc lớn hơn 0")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Số lượt tàu khác phải là số nguyên")]
        public int? TAU_KHAC { get; set; }

        
        [Range(0, Int32.MaxValue, ErrorMessage = "Sản lượng cá bắt buộc lớn hơn 0")]
        public decimal? SANLUONG_CA { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Sản lượng mực bắt buộc lớn hơn 0")]
        public decimal? SANLUONG_MUC { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Sản lượng tôm bắt buộc lớn hơn 0")]
        public decimal? SANLUONG_TOM { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Sản lượng thủy sản khác bắt buộc lớn hơn 0")]
        public decimal? SANLUONG_KHAC { get; set; }


        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượng nước đá(tấn) bắt buộc lớn hơn 0")]
        public decimal? HANG_NUOCDA { get; set; }



        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượng xăng dầu(lít) bắt buộc lớn hơn 0")]
        public decimal? HANG_XANGDAU { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượng nước ngột(khối) bắt buộc lớn hơn 0")]
        public decimal? HANG_NUOCNGOT { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Số lượng hàng khác bắt buộc lớn hơn 0")]
        public decimal? HANG_KHAC { get; set; }



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày ghi nhận")]
        public DateTime? NGAY_GHINHAN { get; set; }


        [Display(Name = "Người cập nhật")]
        [MaxLength(50)]
        public string NGUOI_CAPNHAP { get; set; }
        public virtual DM_CANGCA DM_CANGCA { get; set; }

        
        
    }
}
