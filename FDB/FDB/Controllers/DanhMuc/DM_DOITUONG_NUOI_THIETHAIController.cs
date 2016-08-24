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
    public class DM_DOITUONG_NUOI_THIETHAIController : FDBController
    {
        private FDBContext db = new FDBContext();

        // GET: /DM_DOITUONG_NUOI_THIETHAI/
        public ActionResult Index(string txtTenDoiTuong)
        {
            var dm_doituong_thiethai = db.DM_DOITUONG_NUOI_THIETHAI.Where(o => (string.IsNullOrEmpty(txtTenDoiTuong) || o.Name.ToUpper().Contains(txtTenDoiTuong.ToUpper())));

            ViewBag.TotalRow = dm_doituong_thiethai.Count();

            return View(dm_doituong_thiethai.ToList());

         
        }

       

        
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( DM_DOITUONG_NUOI_THIETHAI dm_doituong_nuoi_thiethai)
        {
            if (ModelState.IsValid)
            {
                var checkExist = db.DM_DOITUONG_NUOI_THIETHAI.Where(b => b.Name.ToUpper().Contains(dm_doituong_nuoi_thiethai.Name.ToUpper().Trim())

                                );

                string msgErrs = string.Empty;
                if (checkExist.Count() > 0)
                {
                    DM_DOITUONG_NUOI_THIETHAI obj = checkExist.FirstOrDefault();

                    msgErrs = string.Format("Đã tồn tại đối tượng có tên [{0}]", obj.Name.ToString()) + "<br />";

                }

                if (string.IsNullOrEmpty(msgErrs))
                {
                    db.DM_DOITUONG_NUOI_THIETHAI.Add(dm_doituong_nuoi_thiethai);
                    db.SaveChanges();
                    this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, string.Empty));
                    return RedirectToAction("Index");
                }
                else
                {
                    Inline_Danger(msgErrs, true);
                }
               
            }

            return View(dm_doituong_nuoi_thiethai);
        }

        // GET: /DM_DOITUONG_NUOI_THIETHAI/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_DOITUONG_NUOI_THIETHAI dm_doituong_nuoi_thiethai = db.DM_DOITUONG_NUOI_THIETHAI.Find(id);
            if (dm_doituong_nuoi_thiethai == null)
            {
                return HttpNotFound();
            }
            return View(dm_doituong_nuoi_thiethai);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DM_DOITUONG_NUOI_THIETHAI dm_doituong_nuoi_thiethai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dm_doituong_nuoi_thiethai).State = EntityState.Modified;
                db.SaveChanges();
                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));
                return RedirectToAction("Index");
            }
            return View(dm_doituong_nuoi_thiethai);
        }

     
        public ActionResult Delete(int id)
        {
            try
            {
                DM_DOITUONG_NUOI_THIETHAI dm_doituong_nuoi_thiethai = db.DM_DOITUONG_NUOI_THIETHAI.Find(id);
                db.DM_DOITUONG_NUOI_THIETHAI.Remove(dm_doituong_nuoi_thiethai);
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
