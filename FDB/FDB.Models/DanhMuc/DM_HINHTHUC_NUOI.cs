using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDB.Models
{
    public class DM_HINHTHUC_NUOI
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Tên hình thức là bắt buộc nhập")]
        [Display(Name = "Tên hình thức")]
        public string TEN_HINH_THUC { get; set; }
    }
}
