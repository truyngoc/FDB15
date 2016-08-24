using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDB.Models
{
    public class DM_LOAI_MATNUOC_NUOI_LONGBE
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên loại là bắt buộc nhập")]
        [Display(Name = "Tên loại mặt nước")]
        public string TEN_LOAI { get; set; }
    }
}
