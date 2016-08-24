using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;


namespace FDB.Models
{
    public class ViewModelAccountLogs
    {
        

        public string MA_TINHTP { get; set; }
        public string TEN_TTP { get; set; }

        public string UserName { get; set; }

        public int SO_LAN_DANG_NHAP { get; set; }
    }
}
