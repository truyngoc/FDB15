using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DM_DOITUONG_NUOI
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Phải chọn nhóm đối tượng")]
        [Display(Name = "Mã nhóm đối tượng")]        
        public int DM_NHOMDOITUONG_NUOIID { get; set; }

        [Required(ErrorMessage= "Tên đối tượng là bắt buộc nhập")]
        [Display(Name = "Tên đối tượng")]        
        public string TEN_DOI_TUONG { get; set; }

        [Display(Name = "Mô tả")]        
        public string MO_TA { get; set; }
        public virtual DM_NHOMDOITUONG_NUOI DM_NHOMDOITUONG_NUOI { get; set; }
    }
}
