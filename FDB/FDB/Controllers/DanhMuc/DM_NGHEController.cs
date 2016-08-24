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
    public class DM_NGHEController : FDBController
    {
        //private FDBContext db = new FDBContext();

        //// GET: /DM_Nghe/
        //public ActionResult Index(string txtTenNhom, string txtTenDoiTuong)
        //{
        //    var dm_nghe = db.DM_NGHE.Include(d => d.DM_NhomNghe).Where(d => ((txtTenNhom == null || txtTenNhom == "") || d.DM_NhomNghe.TenNhomNghe.ToUpper().Contains(txtTenNhom.ToUpper()))
        //                                                                                    && ((txtTenDoiTuong == null || txtTenDoiTuong == "") || d.TenNghe.ToUpper().Contains(txtTenDoiTuong.ToUpper()))

        //        );

        //    ViewBag.TotalRow = dm_nghe.Count();

        //    return View(dm_nghe.ToList());
        //}

        //// GET: /DM_Nghe/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DM_NGHE dm_nghe = db.DM_NGHE.Find(id);
        //    if (dm_nghe == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(dm_nghe);
        //}

        //// GET: /DM_Nghe/Create
        //public ActionResult Create()
        //{
        //    ViewBag.DM_NhomNghes = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
        //    return View();
        //}

        //// POST: /DM_Nghe/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="DM_NgheID,DM_NhomNgheID,TenNghe,MoTa")] DM_NGHE dm_nghe)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.DM_NGHE.Add(dm_nghe);
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_ADD_SUCCESS, string.Empty));

        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.DM_NhomNghes = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe", dm_nghe.DM_NhomNgheID);
        //    return View(dm_nghe);
        //}

        //// GET: /DM_Nghe/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DM_NGHE dm_nghe = db.DM_NGHE.Find(id);
        //    if (dm_nghe == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.DM_NhomNghes = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe", dm_nghe.DM_NhomNgheID);
        //    return View(dm_nghe);
        //}

        //// POST: /DM_Nghe/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include="DM_NgheID,DM_NhomNgheID,TenNghe,MoTa")] DM_NGHE dm_nghe)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(dm_nghe).State = EntityState.Modified;
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));

        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DM_NhomNghes = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe", dm_nghe.DM_NhomNgheID);
        //    return View(dm_nghe);
        //}

       
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        DM_NGHE dm_nghe = db.DM_NGHE.Find(id);
        //        db.DM_NGHE.Remove(dm_nghe);
        //        db.SaveChanges();

        //        this.Information(string.Format(Constants.NOTIFY_DELETE_SUCCESS, string.Empty));

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
