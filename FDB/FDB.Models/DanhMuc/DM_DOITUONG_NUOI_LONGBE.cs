using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDB.Models
{
    public class DM_DOITUONG_NUOI_LONGBE
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Phải chọn nhóm đối tượng")]
        [Display(Name = "Mã nhóm đối tượng")]
        public int DM_NHOMDOITUONG_NUOI_LONGBE_ID { get; set; }

        [Required(ErrorMessage = "Tên đối tượng là bắt buộc nhập")]
        [Display(Name = "Tên đối tượng")]
        public string TEN_DOI_TUONG { get; set; }

        [Display(Name = "Mô tả")]
        public string MO_TA { get; set; }
        public virtual DM_NHOMDOITUONG_NUOI_LONGBE DM_NHOMDOITUONG_NUOI_LONGBE { get; set; }
    }
}
