using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FDB.Models;
using FDB.DataAccessLayer;
using FDB.Common;
using PagedList;

namespace FDB.Controllers.KhaiThac
{
    public class KT_THIETHAIKHAITHACManager
    {
        public static void Add(string so_dk_tau, int? so_thuyenvien, string khuvuc_gapnan, DateTime? tg_gannan, List<int> sucovetau, List<int> sucovenguoi, string coquanxuly, decimal? thiethai_uoctinh, string vido, string kinhdo, string tau_khac, string nguoi_khac, int? chet, int? mattich)
        {
            using (  FDBContext db = new FDBContext())
            {
                var kt_thiethaikhaithac = new KT_THIETHAIKHAITHAC()
                {
                    SO_DK_TAU = so_dk_tau,
                    SO_THUYENVIEN = so_thuyenvien,
                    KHUVUC_GAPNAN = khuvuc_gapnan,
                    TG_GAPNAN = tg_gannan,
                    COQUAN_XULY = coquanxuly,
                    THIETHAI_UOCTINH = thiethai_uoctinh,
                    VIDO = vido,
                    KINHDO = kinhdo,
                    TAU_KHAC = tau_khac,
                    NGUOI_KHAC = nguoi_khac,
                    SO_NGUOI_CHET = chet,
                    SO_NGUOI_MAT_TICH = mattich

                };

                //sucovetau
                foreach (var sucovetauID in sucovetau)
                {
                    var sucovetaus = db.DSUCOVETAU.Find(sucovetauID);

                    kt_thiethaikhaithac.SUCOVETAU.Add(sucovetaus);
                }

                //sucovenguoi
                foreach (var sucovenguoiID in sucovenguoi)
                {
                    var sucovenguois = db.DSUCOVENGUOI.Find(sucovenguoiID);

                    kt_thiethaikhaithac.SUCOVENGUOI.Add(sucovenguois);
                }

                db.KT_THIETHAIKHAITHAC.Add(kt_thiethaikhaithac);

                db.SaveChanges();
            }
        }

        public static void Edit(int id, string so_dk_tau, int? so_thuyenvien, string khuvuc_gapnan, DateTime? tg_gannan, List<int> sucovetau, List<int> sucovenguoi, string coquanxuly, decimal? thiethai_uoctinh, string vido, string kinhdo, string tau_khac, string nguoi_khac, int? chet, int? mattich)
        {
            using (FDBContext db = new FDBContext())
            {
                var kt_thiethaikhaithac = db.KT_THIETHAIKHAITHAC.Where(x => x.ID == id).First();
                kt_thiethaikhaithac.SO_DK_TAU = so_dk_tau;
                kt_thiethaikhaithac.SO_THUYENVIEN = so_thuyenvien;
                kt_thiethaikhaithac.KHUVUC_GAPNAN = khuvuc_gapnan;
                kt_thiethaikhaithac.TG_GAPNAN = tg_gannan;
                kt_thiethaikhaithac.COQUAN_XULY = coquanxuly;
                kt_thiethaikhaithac.THIETHAI_UOCTINH = thiethai_uoctinh;
                kt_thiethaikhaithac.VIDO = vido;
                kt_thiethaikhaithac.KINHDO = kinhdo;
                kt_thiethaikhaithac.TAU_KHAC = tau_khac;
                kt_thiethaikhaithac.NGUOI_KHAC = nguoi_khac;
                kt_thiethaikhaithac.SO_NGUOI_CHET = chet;
                kt_thiethaikhaithac.SO_NGUOI_MAT_TICH = mattich;

                kt_thiethaikhaithac.SUCOVETAU.Clear();
               
                foreach (var sucovetauID in sucovetau)
                {
                    var sucovetaus = db.DSUCOVETAU.Find(sucovetauID);
                    kt_thiethaikhaithac.SUCOVETAU.Add(sucovetaus);
                }
                kt_thiethaikhaithac.SUCOVENGUOI.Clear();
                foreach (var sucovenguoiID in sucovenguoi)
                {
                    var sucovenguois = db.DSUCOVENGUOI.Find(sucovenguoiID);
                    kt_thiethaikhaithac.SUCOVENGUOI.Add(sucovenguois);
                }

                db.SaveChanges();
            }
        }

        public static void Delete(int? id)
        {
            using (FDBContext db = new FDBContext())
            {
                var kt_thiethaikhaithac = db.KT_THIETHAIKHAITHAC.Where(x => x.ID == id).First();
                db.KT_THIETHAIKHAITHAC.Remove(kt_thiethaikhaithac);
                db.SaveChanges();
            }
        }

        //public static List<KT_THIETHAIKHAITHAC> GetAll()
        //{
        //    using (FDBContext context = new FDBContext())
        //    {
        //        var kt_thiethaikhaithacs = context.KT_THIETHAIKHAITHAC.Include(x => x.SUCOVETAU ).OrderBy(x => x.ID).ToList();
        //        var kt_thiethaikhaithac = context.KT_THIETHAIKHAITHAC.ToList();

        //        return kt_thiethaikhaithacs;
        //    }
        //}

        public static KT_THIETHAIKHAITHAC GetByID(int? id)
        {
            using (FDBContext context = new FDBContext())
            {
                var kt_thiethai = context.KT_THIETHAIKHAITHAC.Include(x => x.SUCOVETAU).Include(x => x.SUCOVENGUOI).Where(x => x.ID == id).First();
                return kt_thiethai;
            }
        }

        #region
        public static List<DSUCOVETAU> GetSUCOVETAUByID(int? id)
        {
            using (FDBContext context = new FDBContext())
            {
                return context.KT_THIETHAIKHAITHAC.Where(x => x.ID == id).First().SUCOVETAU.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<DSUCOVENGUOI> GetSUCOVENGUOIByID(int? id)
        {
            using (FDBContext context = new FDBContext())
            {
                return context.KT_THIETHAIKHAITHAC.Where(x => x.ID == id).First().SUCOVENGUOI.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<DSUCOVETAU> GetAllSUCOVETAU()
        {
            using (FDBContext context = new FDBContext())
            {
                return context.DSUCOVETAU.OrderByDescending(x => x.ID).ToList();
            }
        }

        public static List<DSUCOVENGUOI> GetAllSUCOVENGUOI()
        {
            using (FDBContext context = new FDBContext())
            {
                return context.DSUCOVENGUOI.OrderByDescending(x => x.ID).ToList();
            }
        }

     
        #endregion
    }
}