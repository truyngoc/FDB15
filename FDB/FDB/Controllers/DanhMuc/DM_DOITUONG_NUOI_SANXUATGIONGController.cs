using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using FDB.Common;
using FDB.DataAccessLayer;
using FDB.Models;
using PagedList;
using System.Data.Entity.Validation;
using Excel;
using System.Text;
using System.IO;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Data.SqlClient;
using System.Transactions;
using Aron.Sinoai.OfficeHelper;
using System.Data.Entity;

using Microsoft.AspNet.Identity;

namespace FDB.Controllers
{
    public class DM_DOITUONG_NUOI_SANXUATGIONGController : FDBController
    {
        private FDBContext db = new FDBContext();

        // GET: /DM_DOITUONG_NUOI_SANXUATGIONG/
        public ActionResult Index(string txtTenDoiTuong)
        {
            var dm_doituong_giong = db.DM_DOITUONG_NUOI_SANXUATGIONG.Where(o => (string.IsNullOrEmpty(txtTenDoiTuong) || o.TEN_DOI_TUONG.ToUpper().Contains(txtTenDoiTuong.ToUpper())));

            ViewBag.TotalRow = dm_doituong_giong.Count();

            return View(dm_doituong_giong.ToList());


        }

        

        // GET: /DM_DOITUONG_NUOI_SANXUATGIONG/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( DM_DOITUONG_NUOI_SANXUATGIONG dm_doituong_nuoi_sanxuatgiong)
        {
            if (ModelState.IsValid)
            {
                var checkExist = db.DM_DOITUONG_NUOI_SANXUATGIONG.Where(b => b.TEN_DOI_TUONG.ToUpper().Contains(dm_doituong_nuoi_sanxuatgiong.TEN_DOI_TUONG.ToUpper().Trim())

                                  );

                string msgErrs = string.Empty;
                if (checkExist.Count() > 0)
                {
                    DM_DOITUONG_NUOI_SANXUATGIONG obj = checkExist.FirstOrDefault();

                    msgErrs = string.Format("Đã tồn tại đối tượng có tên [{0}]", obj.TEN_DOI_TUONG.ToString()) + "<br />";

                }
                if (string.IsNullOrEmpty(msgErrs))
                {
                    db.DM_DOITUONG_NUOI_SANXUATGIONG.Add(dm_doituong_nuoi_sanxuatgiong);
                    db.SaveChanges();
                    this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, string.Empty));
                    return RedirectToAction("Index");
                }
                else
                {
                    Inline_Danger(msgErrs, true);
                }
              
            }

            return View(dm_doituong_nuoi_sanxuatgiong);
        }

        // GET: /DM_DOITUONG_NUOI_SANXUATGIONG/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_DOITUONG_NUOI_SANXUATGIONG dm_doituong_nuoi_sanxuatgiong = db.DM_DOITUONG_NUOI_SANXUATGIONG.Find(id);
            if (dm_doituong_nuoi_sanxuatgiong == null)
            {
                return HttpNotFound();
            }
            return View(dm_doituong_nuoi_sanxuatgiong);
        }

        // POST: /DM_DOITUONG_NUOI_SANXUATGIONG/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DM_DOITUONG_NUOI_SANXUATGIONG dm_doituong_nuoi_sanxuatgiong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dm_doituong_nuoi_sanxuatgiong).State = EntityState.Modified;
                db.SaveChanges();
                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));
                return RedirectToAction("Index");
            }
            return View(dm_doituong_nuoi_sanxuatgiong);
        }

      

        public ActionResult Delete(int id)
        {
            try
            {
                DM_DOITUONG_NUOI_SANXUATGIONG dm_doituong_nuoi_sanxuatgiong = db.DM_DOITUONG_NUOI_SANXUATGIONG.Find(id);
                db.DM_DOITUONG_NUOI_SANXUATGIONG.Remove(dm_doituong_nuoi_sanxuatgiong);
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, string.Empty));

                
            }
            catch 
            {
                Inline_Danger("Đối tượng này đã được sử dụng tại chức năng khác nên không thể xóa", true);
            }

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
