using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace FDB.Models
{
    public class AccountLog
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string MA_TTP { get; set; }
        public string TEN_TTP { get; set; }
        public DateTime? Logtime { get; set; }
        public DateTime? LogtimeEnd { get; set; }

        public string MenuId { get; set; }
    }



    public class ViewModelSearchAcc_Login
    {
        public int? Page { get; set; }

        public string MA_TINHTP { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Từ ngày")]

        public DateTime? TU_NGAY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Đến ngày")]

        public DateTime? DEN_NGAY { get; set; }

        public List<ViewModelAccountLogs> SearchResults { get; set; }


    }



    public class ViewModelSearchAcc_Login_Details
    {
        public int? Page { get; set; }

        public string MA_TINHTP { get; set; }

        public string UserName { get; set; }
     


        public IPagedList<AccountLog> SearchResults { get; set; }

    }
}