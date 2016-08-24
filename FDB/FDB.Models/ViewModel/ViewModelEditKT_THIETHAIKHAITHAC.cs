using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;
using FDB.Common;
using System.Web.Mvc;
using FDB.Models;

namespace FDB.Models
{
   public class ViewModelEditKT_THIETHAIKHAITHAC : ViewModelAddKT_THIETHAIKHAITHAC
    {
       public int ID { get; set; }


       //[Display(Name = "Thiệt hại ước tính")]
       //[Range(0, Int32.MaxValue, ErrorMessage = "Thiệt hại ước tính bắt buộc lớn hơn 0")]

       //public decimal THIETHAI_UOCTINH { get; set; }
    }
}
