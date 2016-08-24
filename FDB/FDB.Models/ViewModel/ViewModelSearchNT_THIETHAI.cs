using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;


namespace FDB.Models
{
   public class ViewModelSearchNT_THIETHAI
    {
        public int? Page { get; set; }


        [Display(Name = "Đối tượng nuôi")]
        public int? DM_DOITUONG_NUOI_THIETHAIID { get; set; }

        [Display(Name = "Nguyên nhân")]
        public int? DNGUYENNHAN_THIETHAIID { get; set; }

        [Display(Name = "Tỷ lệ thiệt hại")]
        public int? DTYLE_THIETHAIID { get; set; }

        public int? DO_MOITRUONG { get; set; }
        public int? DO_DICHBENH { get; set; }

        [Display(Name = "Tháng")]
        [Range(1, 12, ErrorMessage = "Tháng phải thuộc khoảng từ 1 đến 12")]
        public int? THANG { get; set; }

        
        [Display(Name = "Năm")]
        [Range(1900, 2100, ErrorMessage = "Năm phải thuộc khoảng từ 1900 đến 2100")]
        public int? NAM { get; set; }
     

        [Display(Name = "Tỉnh/TP")]
        public string MA_TINHTP { get; set; }

        [Display(Name = "Quận/huyện/thị xã")]
        public string MA_QUANHUYEN { get; set; }

        public string MA_PHUONGXA { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Từ ngày")]

        public DateTime? TU_NGAY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Đến ngày")]

        public DateTime? DEN_NGAY { get; set; }

        public IPagedList<NT_THIETHAI> SearchResults { get; set; }
    }
}
