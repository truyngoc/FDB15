using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Data.Entity.ModelConfiguration;

namespace FDB.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //Extending the IdentityModel Class with Additional Properties        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        //public string Email { get; set; }

        public virtual ICollection<ApplicationUserGroup> Groups { get; set; }

        public string MA_TINHTP { get; set; }
        public string MA_QUANHUYEN { get; set; }
        public string MA_PHUONGXA { get; set; }
        public string DON_VI_CONG_TAC { get; set; }
        public string CHUC_VU { get; set; }
    }


}