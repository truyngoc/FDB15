using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FDB.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }


        public ApplicationRole(string name, string description, string Ma_TTP)
            : base(name)
        {
            this.Description = description;
            this.Ma_TTP = Ma_TTP;
        }
        public virtual string Description { get; set; }

        public string Ma_TTP { get; set; }
    }
}