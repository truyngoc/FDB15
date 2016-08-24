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

namespace FDB.Controllers.DanhMuc
{
    public class DM_NHOMNGHEController : FDBController
    {
        private FDBContext db = new FDBContext();

        // GET: /DM_NhomNghe/
        public ActionResult Index(string txtTenNhom)
        {
            var result = db.DM_NHOMNGHE.Where(d => (txtTenNhom == null || txtTenNhom == "") || d.TenNhomNghe.ToUpper().Contains(txtTenNhom.ToUpper()));

            ViewBag.TotalRow = result.Count();

            return View(result.ToList());
        }


        // GET: /DM_NhomNghe/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /DM_NhomNghe/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DM_NhomNgheID,TenNhomNghe,MoTa")] DM_NHOMNGHE dm_nhomnghe)
        {
            if (ModelState.IsValid)
            {
                var checkExist = db.DM_NHOMNGHE.Where(b => b.TenNhomNghe.ToUpper().Contains(dm_nhomnghe.TenNhomNghe.ToUpper().Trim())

                              );

                string msgErrs = string.Empty;
                if (checkExist.Count() > 0)
                {
                    DM_NHOMNGHE obj = checkExist.FirstOrDefault();

                    msgErrs = string.Format("Đã tồn tại nhóm nghề có tên [{0}]", obj.TenNhomNghe.ToString()) + "<br />";

                }

                if (string.IsNullOrEmpty(msgErrs))
                {
                    db.DM_NHOMNGHE.Add(dm_nhomnghe);
                    db.SaveChanges();

                    this.Information(string.Format(Constants.NOTIFY_ADD_SUCCESS, string.Empty));

                    return RedirectToAction("Index");
                }
                else
                {
                    Inline_Danger(msgErrs, true);
                }
               
            }

            return View(dm_nhomnghe);
        }

        // GET: /DM_NhomNghe/Edit/5
        public ActionResult Edit(int id)
        {
            DM_NHOMNGHE dm_nhomnghe = db.DM_NHOMNGHE.Find(id);
            if (dm_nhomnghe == null)
            {
                return HttpNotFound();
            }
            return View(dm_nhomnghe);
        }

        // POST: /DM_NhomNghe/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="DM_NhomNgheID,TenNhomNghe,MoTa")] DM_NHOMNGHE dm_nhomnghe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dm_nhomnghe).State = EntityState.Modified;
                db.SaveChanges();

                this.Information(string.Format(Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));

                return RedirectToAction("Index");
            }
            return View(dm_nhomnghe);
        }
                  
        public ActionResult Delete(int id)
        {
            try
            {
                DM_NHOMNGHE dm_nhomnghe = db.DM_NHOMNGHE.Find(id);
                db.DM_NHOMNGHE.Remove(dm_nhomnghe);
                db.SaveChanges();

                this.Information(string.Format(Constants.NOTIFY_DELETE_SUCCESS, string.Empty));

                
            }
            catch
            {
                //this.Error("Không thể xóa nhóm nghề khi tồn tại danh sách nghề thuộc nhóm, hoặc nghề thuộc nhóm đã được sử dụng");
                Inline_Danger("Nhóm nghề này đã được sử dụng tại chức năng khác nên không thể xóa", true);
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
