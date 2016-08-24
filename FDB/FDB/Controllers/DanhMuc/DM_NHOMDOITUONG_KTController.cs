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
    public class DM_NHOMDOITUONG_KTController : FDBController
    {
        private FDBContext db = new FDBContext();


        public ActionResult Index(ViewModelSearchDM_NHOMDOITUONG_KT SearchModel)
        {
            BindComboDM();
            var DM_NHOM_DOITUONG_KTs = db.DM_NHOMDOITUONG_KT.Where(o => (SearchModel.TEN_NHOM == null || o.TEN_NHOM.ToUpper().Contains(SearchModel.TEN_NHOM.ToUpper()))
                                                      //&& (SearchModel.LOAI_MAT_NUOC == null || o.ID_LOAI_MATNUOC_KT == SearchModel.LOAI_MAT_NUOC)
                                          ).OrderBy(x => x.ID);

            ViewBag.TotalRow = DM_NHOM_DOITUONG_KTs.Count();

            int pageSize = FDB.Common.Constants.PageSize;
            int pageNumber = SearchModel.Page ?? 1;

            SearchModel.SearchResults = DM_NHOM_DOITUONG_KTs.ToPagedList(pageNumber, pageSize);

            return View(SearchModel);
        }


        private void BindComboDM()
        {
            //ViewBag.LOAIMATNUOCs = new SelectList(db.DM_LOAI_MATNUOC_KT, "ID", "TEN_LOAI");
        }

        public ActionResult Create()
        {
            BindComboDM();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DM_NHOMDOITUONG_KT dm_nhomdoituong)
        {
            if (ModelState.IsValid)
            {
                var checkExist = db.DM_NHOMDOITUONG_KT.Where(b => b.TEN_NHOM.ToUpper().Contains(dm_nhomdoituong.TEN_NHOM.ToUpper().Trim())

                                        // && b.ID_LOAI_MATNUOC_KT == dm_nhomdoituong.ID_LOAI_MATNUOC_KT

                                     );

                string msgErrs = string.Empty;
                if (checkExist.Count() > 0)
                {
                    DM_NHOMDOITUONG_KT obj = checkExist.FirstOrDefault();
                    
                    //var loai_khai_thac = db.DM_LOAI_MATNUOC_KT.Where(b => b.ID == dm_nhomdoituong.ID_LOAI_MATNUOC_KT).Select(b => b.TEN_LOAI);
                    //msgErrs = string.Format("Đã tồn tại nhóm đối tượng có tên [{0}] thuộc loại [{1}]", dm_nhomdoituong.TEN_NHOM.ToString(), obj.DM_LOAI_MATNUOC_KT.TEN_LOAI.ToString()) + "<br />";
                    msgErrs = string.Format("Đã tồn tại đối tượng có tên [{0}]", dm_nhomdoituong.TEN_NHOM.ToString()) + "<br />";

                }

                if (string.IsNullOrEmpty(msgErrs))
                {
                    db.DM_NHOMDOITUONG_KT.Add(dm_nhomdoituong);
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
            return View(dm_nhomdoituong);
        }


        public ActionResult Edit(int id)
        {
            DM_NHOMDOITUONG_KT dm_doituong = db.DM_NHOMDOITUONG_KT.Find(id);
            if (dm_doituong == null)
            {
                return HttpNotFound();
            }
            BindComboDM();
            return View(dm_doituong);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DM_NHOMDOITUONG_KT dm_doituong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dm_doituong).State = EntityState.Modified;
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, string.Empty));

                return RedirectToAction("Index");
            }
            BindComboDM();
            return View(dm_doituong);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                DM_NHOMDOITUONG_KT dm_doituong = db.DM_NHOMDOITUONG_KT.Find(id);
                db.DM_NHOMDOITUONG_KT.Remove(dm_doituong);
                db.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, string.Empty));


               
            }
            catch
            {
                //this.Error("Không thể xóa nhóm đối tượng khai thác khi tồn tại đối tượng khai thác thuộc nhóm, hoặc đối tượng khai thác thuộc nhóm đã được sử dụng");
                //return RedirectToAction("Index");
                Inline_Danger("Đối tượng này đã được sử dụng tại chức năng khác nên không thể xóa",true);
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