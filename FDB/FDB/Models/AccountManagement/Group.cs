using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FDB.Models
{
    public class Group
    {
        public Group() { }


        public Group(string name, string Ma_TTP)
            : this()
        {
            this.Roles = new List<ApplicationRoleGroup>();
            this.Name = name;
            this.Ma_TTP = Ma_TTP;
        }


        [Key]
        [Required]
        public virtual int Id { get; set; }

        [Display(Name = "Tên nhóm")]
        [Required(ErrorMessage = "Tên nhóm là bắt buộc nhập")]
        public virtual string Name { get; set; }

        public string Ma_TTP { get; set; }

        public virtual ICollection<ApplicationRoleGroup> Roles { get; set; }
    }
}