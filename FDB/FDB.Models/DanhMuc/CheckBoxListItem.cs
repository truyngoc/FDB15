using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using FDB.Models;
using FDB.Common;   

namespace FDB.Models
{
   public class CheckBoxListItem
    {
        public int ID { get; set; }
        public string Display { get; set; }

   
        public bool IsChecked { get; set; }
    }

  
}
