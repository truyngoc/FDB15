using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDB.Models
{
    public class DSUCOVETAU
    {

        public DSUCOVETAU()
        {
            this.KT_THIETHAIKHAITHAC = new HashSet<KT_THIETHAIKHAITHAC>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<KT_THIETHAIKHAITHAC> KT_THIETHAIKHAITHAC { get; set; }
      
    }
}
