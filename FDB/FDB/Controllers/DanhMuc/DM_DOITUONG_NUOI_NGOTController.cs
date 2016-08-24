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
    public class DM_DOITUONG_NUOI_NGOTController : FDBController
    {
        private FDBContext db = new FDBContext();

        // GET: /DM_DOITUONG_NUOI_NGOT/
        public ActionResult Index(ViewModelSearchDM_DOITUONG_NUOI_NGOT SearchModel)
        {
            BindComboDM();
            var DM_DOITUONG_NGOTs = db.DM_DOITUONG_NUOI_NGOT.Where(o => (SearchModel.TEN_DOI_TUONG == null || o.TEN_DOI_TUONG.ToUpper().Contains(SearchModel.TEN_DOI_TUONG.ToUpper()))                                                                                                         
                                                      && (SearchModel.LOAI_DOI_TUONG == null || o.LOAI_DOI_TUONG == SearchModel.LOAI_DOI_TUONG)
                                          ).OrderBy(x => x.ID);

            ViewBag.TotalRow = DM_DOITUONG_NGOTs.Count();

            int pageSize = FDB.Common.Constants.PageSize;
            int pageNumber = SearchModel.Page ?? 1;

            SearchModel.SearchResults = DM_DOITUONG_NGOTs.ToPagedList(pageNumber, pageSize);

            return View(SearchModel);
         }

      

        // GET: /DM_DOITUONG_NUOI_NGOT/Create
        public ActionResult Create()
        {
            BindComboDM();
            return View();
        }

        private void BindComboDM()
        {
            ViewBag.HINHTHUCNUOIs = new SelectList(db.DM_HINHTHUC_NUOI, "ID", "TEN_HINH_THUC");
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (DM_DOITUONG_NUOI_NGOT dm_doituong_nuoi_ngot)
        {
            if (ModelState.IsValid)
            {
                var checkExist = db.DM_DOITUONG_NUOI_NGOT.Where(b => b.TEN_DOI_TUONG.ToUpper().Contains(dm_doituong_nuoi_ngot.TEN_DOI_TUONG.ToUpper().Trim())
                                         && b.LOAI_DOI_TUONG == dm_doituong_nuoi_ngot.LOAI_DOI_TUONG
                                         

                                     );

                string msgErrs = string.Empty;
                if (checkExist.Count() > 0)
                {
                    DM_DOITUONG_NUOI_NGOT obj = checkExist.FirstOrDefault();
                   
                    msgErrs = string.Format("Đã tồn tại đối tượng có tên [{0}] có hình thức nuôi [{1}]", obj.TEN_DOI_TUONG.ToString(), obj.DM_HINHTHUC_NUOI.TEN_HINH_THUC.ToString()) + "<br />";

                }

                if (string.IsNullOrEmpty(msgErrs))
                {
                    db.DM_DOITUONG_NUOI_NGOT.Add(dm_doituong_nuoi_ngot);
                    db.SaveChanges();
                    this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, string.Empty));
                    return RedirectToAction("Index");
                }
                else
                {
                    Inline_Danger(msgErrs, true);
                }
              
            }
            BindComboDM();
            return View(dm_doituong_nuoi_ngot);
        }

        // GET: /DM_DOITUONG_NUOI_NGOT/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DM_DOITUONG_NUOI_NGOT dm_doituong_nuoi_ngot = db.DM_DOITUONG_NUOI_NGOT.Find(id);
            if (dm_doituong_nuoi_ngot == null)
            {
                return HttpNotFound();
            }
            BindComboDM();
            return View(dm_doituong_nuoi_ngot);
        }

        // POST: /DM_DOITUONG_NUOI_NGOT/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DM_DOITUONG_NUOI_NGOT dm_doituong_nuoi_ngot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dm_doituong_nuoi_ngot).State = EntityState.Modified;
                db.SaveChanges();
                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));
                return RedirectToAction("Index");
            }
            BindComboDM();
            return View(dm_doituong_nuoi_ngot);
        }

     
        public ActionResult Delete(int id)
        {
            try
            {
                DM_DOITUONG_NUOI_NGOT dm_doituong_nuoi_ngot = db.DM_DOITUONG_NUOI_NGOT.Find(id);
                db.DM_DOITUONG_NUOI_NGOT.Remove(dm_doituong_nuoi_ngot);
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
