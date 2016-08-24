using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FDB.DAL;
using FDB.Models;
using System.Data.SqlClient;
using System.Timers;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity;


namespace FDB.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private FDB.DataAccessLayer.FDBContext db_ = new FDB.DataAccessLayer.FDBContext();
        public ActionResult Index()
        {
            ApplicationUser curUser = getCurrentUser();
            //count so giay phep het han
            var Count_GPHH = db_.KT_GIAYPHEP.Where(o => (((o.TRANG_THAI == null) && o.NGAY_HHL_GP < DateTime.Today) || ((o.TRANG_THAI == 1) && o.NGAY_GIA_HAN_DEN < DateTime.Today))
                                                       && (!String.IsNullOrEmpty(curUser.MA_TINHTP) && o.MA_TINHTP == curUser.MA_TINHTP) 
                ).Count();
            ViewBag.TotalRow = Count_GPHH;

            //count so giay phep se het hạn trong 10 ngay tới
            var allGP = db_.KT_GIAYPHEP.Where(o => !String.IsNullOrEmpty(curUser.MA_TINHTP) && o.MA_TINHTP == curUser.MA_TINHTP).ToList();
            int i = 0;

            foreach (var kt_giayphep in allGP)
            {
                if (kt_giayphep.NGAY_HHL_GP.HasValue || kt_giayphep.NGAY_GIA_HAN_DEN.HasValue)
                {
                    
                        int ngayHHL = Convert.ToInt32((kt_giayphep.NGAY_HHL_GP.GetValueOrDefault() - DateTime.Today).TotalDays);
                        int ngayGH = Convert.ToInt32((kt_giayphep.NGAY_GIA_HAN_DEN.GetValueOrDefault() - DateTime.Today).TotalDays);
                        if (((ngayHHL >=1 && ngayHHL <= 10) && kt_giayphep.TRANG_THAI == null) || ((ngayGH >= 1 && ngayGH <= 10) && kt_giayphep.TRANG_THAI == 1))
                        {
                            i++;
                        }
                    
                }

            }

            ViewBag.CountGPSapHetHan = i;

           
            //count so đang kiem het han
            
           
            //lay id max trong danh sach khi group by theo so dang kiem

            var ID_Max = db_.KT_DANGKIEM.Where(o => !String.IsNullOrEmpty(curUser.MA_TINHTP) && o.MA_TINHTP == curUser.MA_TINHTP).GroupBy(x => x.SO_SO_DANG_KIEM)
                .Select(xs => xs.Max(x => x.ID));
            
            var Items_DKHH = db_.KT_DANGKIEM.Where(o => ID_Max.Contains(o.ID));

            var Count_DKHH = Items_DKHH.Where(o => o.HAN_DANG_KIEM < DateTime.Today).Count();
            //COUNT SO DANG KIEM TRONG 10 NGAY TOI SE HET HAN
            var Count_DK_SAP_HH = Items_DKHH.Where(o => (DbFunctions.DiffDays(DateTime.Today, o.HAN_DANG_KIEM)) >= 1 && (DbFunctions.DiffDays(DateTime.Today, o.HAN_DANG_KIEM) <= 10)).Count();


            ViewBag.CountDKHetHan = Count_DKHH;
            ViewBag.CountDKSapHetHan = Count_DK_SAP_HH;

            return View();
        }

        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = db.Users.Find(userId);

            return _currentUser;
        }

        public ActionResult Denied()
        {
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}