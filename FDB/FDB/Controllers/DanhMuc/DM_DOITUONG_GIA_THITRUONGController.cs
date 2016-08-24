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
    public class DM_DOITUONG_GIA_THITRUONGController : FDBController
    {
        private FDBContext db = new FDBContext();


        public ActionResult Index(string txtTenDoiTuong)
        {
            var dm_doituong_gia = db.DM_DOITUONG_GIA_THITRUONG.Where(o => (string.IsNullOrEmpty(txtTenDoiTuong)  || o.Name.ToUpper().Contains(txtTenDoiTuong.ToUpper())));

            ViewBag.TotalRow = dm_doituong_gia.Count();

            return View(dm_doituong_gia.ToList());


        }

       

        // GET: /DM_DOITUONG_GIA_THITRUONG/Create
        public ActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( DM_DOITUONG_GIA_THITRUONG dm_doituong_gia_thitruong)
        {
            if (ModelState.IsValid)
            {
                var checkExist = db.DM_DOITUONG_GIA_THITRUONG.Where(b => b.Name.ToUpper().Contains(dm_doituong_gia_thitruong.Name.ToUpper().Trim())

                              );

                string msgErrs = string.Empty;
                if (checkExist.Count() > 0)
                {
                    DM_DOITUONG_GIA_THITRUONG obj = checkExist.FirstOrDefault();

                    msgErrs = string.Format("Đã tồn tại đối tượng có tên [{0}]", obj.Name.ToString()) + "<br />";

                }
                if (string.IsNullOrEmpty(msgErrs))
                {
                    db.DM_DOITUONG_GIA_THITRUONG.Add(dm_doituong_gia_thitruong);
                    db.SaveChanges();
                    this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, string.Empty));
                    return RedirectToAction("Index");
                }
                else
                {
                    Inline_Danger(msgErrs, true);
                }
               
            }

            return View(dm_doituong_gia_thitruong);
        }

        // GET: /DM_DOITUONG_GIA_THITRUONG/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_DOITUONG_GIA_THITRUONG dm_doituong_gia_thitruong = db.DM_DOITUONG_GIA_THITRUONG.Find(id);
            if (dm_doituong_gia_thitruong == null)
            {
                return HttpNotFound();
            }
            return View(dm_doituong_gia_thitruong);
        }

        // POST: /DM_DOITUONG_GIA_THITRUONG/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DM_DOITUONG_GIA_THITRUONG dm_doituong_gia_thitruong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dm_doituong_gia_thitruong).State = EntityState.Modified;
                db.SaveChanges();
                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));
                return RedirectToAction("Index");
            }
            return View(dm_doituong_gia_thitruong);
        }

      

        public ActionResult Delete(int id)
        {
            try
            {
                DM_DOITUONG_GIA_THITRUONG dm_doituong_gia_thitruong = db.DM_DOITUONG_GIA_THITRUONG.Find(id);
                db.DM_DOITUONG_GIA_THITRUONG.Remove(dm_doituong_gia_thitruong);
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "bản ghi"));

                
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
