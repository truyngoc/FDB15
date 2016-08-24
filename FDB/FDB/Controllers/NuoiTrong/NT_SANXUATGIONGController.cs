using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDB.Models;
using FDB.DataAccessLayer;
using FDB.Common;
using PagedList;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using Microsoft.AspNet.Identity;
using FDB.Helpers;
using System.Data.SqlClient;

namespace FDB.Controllers.NuoiTrong
{
    public class NT_SANXUATGIONGController : FDBController
    {
        private FDBContext _context = new FDBContext();

        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }
        //
        // GET: /SanXuatGiong/

        public ActionResult Index(ViewModelSearchNT_SANXUATGIONG Searchmodel)
        {
            ApplicationUser curUser = this.getCurrentUser();
            //Load danh mục:
            this.LoadDanhMuc(Searchmodel);

            var models = _context.NT_SANXUATGIONG.Where(o => ((string.IsNullOrEmpty(Searchmodel.TThanhPho) && (string.IsNullOrEmpty(curUser.MA_TINHTP) || curUser.MA_TINHTP.StartsWith("Z"))) || (string.IsNullOrEmpty(Searchmodel.TThanhPho) && o.MA_TINHTP == curUser.MA_TINHTP) || o.MA_TINHTP == Searchmodel.TThanhPho)
                                                        && (Searchmodel.Thang == null || o.THANG == Searchmodel.Thang)
                                                        && (Searchmodel.Nam == null || o.NAM == Searchmodel.Nam)
                                                        && (Searchmodel.DoiTuongNuoi == null || o.DSNT_SanXuatGiongDetail.Any(d => d.ID_NUOI_SX_DOITUONG == Searchmodel.DoiTuongNuoi))
                                            )
                                            ;

            ViewBag.TotalRow = models.Count().ToString();
            models = models.OrderByDescending(m => m.ID);
            //Phân trang ở đây:
            var pageIndex = Searchmodel.Page ?? 1;
            Searchmodel.SearchResults = models.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

            return View(Searchmodel);
        }

        public ActionResult Search(ViewModelBaocaoNT_SANXUATGIONG BaocaoModel)
        {
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                        .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DMTThanhPho = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP", BaocaoModel.TThanhPho);
            // ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN", Searchmodel.Qhuyen);
            ViewBag.DMDoiTuongSX = new SelectList(_context.DM_DOITUONG_NUOI_SANXUATGIONG, "ID", "TEN_DOI_TUONG", BaocaoModel.DoiTuongSX);

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;


            string _MA_TTP = BaocaoModel.TThanhPho;
            if (BaocaoModel.Nam != null)
            {


                var results = _context.Database.SqlQuery<ViewModelBaocaoNT_SANXUATGIONG_DETAIL>("exec NT_SANXUATGIONG_SEARCH @DoiTuongSX, @Thang, @Nam, @TThanhPho "
                     , new SqlParameter("@DoiTuongSX", BaocaoModel.DoiTuongSX == null ? (object)DBNull.Value : BaocaoModel.DoiTuongSX)
                     , new SqlParameter("@Thang", BaocaoModel.Thang == null ? (object)DBNull.Value : BaocaoModel.Thang)
                     , new SqlParameter("@Nam", BaocaoModel.Nam == null ? (object)DBNull.Value : BaocaoModel.Nam)
                     , new SqlParameter("@TThanhPho", BaocaoModel.TThanhPho == null ? (object)DBNull.Value : BaocaoModel.TThanhPho)
                     ).ToList();


                BaocaoModel.ReportResults = results;
                string strTitle = @"Kết quả thống kê sản xuất giống " + (BaocaoModel.DoiTuongSX!=null?" của đối tượng " +_context.DM_DOITUONG_NUOI_SANXUATGIONG.FirstOrDefault(a => a.ID == BaocaoModel.DoiTuongSX).TEN_DOI_TUONG:"")
                                + (BaocaoModel.Thang != null ? " tháng " + BaocaoModel.Thang.ToString() + " Năm " + BaocaoModel.Nam.ToString() : " Năm " + BaocaoModel.Nam.ToString())
                    ;

                strTitle = strTitle + (BaocaoModel.TThanhPho != null ? " của " + _context.DTINHTP.FirstOrDefault(a => a.MA_TINHTP == _MA_TTP).TEN_TINHTP : " của toàn quốc ");

                ViewBag.strTitle = strTitle;
            }

            return View(BaocaoModel);
        }

        public ActionResult Create()
        {
            this.LoadDanhMuc();
            NT_SANXUATGIONG model = new NT_SANXUATGIONG();
            model.NGUOI_NHAP = User.Identity.Name;
            model.NGAY_NHAP = DateTime.Now;
            model.NAM = DateTime.Now.Year;
            //  model.THANG = DateTime.Now.Month;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection _form, NT_SANXUATGIONG _obj)
        {
            List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_NUOI_SX_DOITUONG_")).ToList<String>();
            List<int> lstInt = new List<int>();
            lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

            if (ModelState.IsValid)
            {
                //Save Header
                _obj.NGUOI_NHAP = User.Identity.Name;
                _obj.NGAY_NHAP = DateTime.Now;
                _context.NT_SANXUATGIONG.Add(_obj);
                _context.SaveChanges();

                int Id = _obj.ID;

                if (lstKeyName != null)
                {

                    for (int i = 0; i < lstInt.Count; i++)
                    {
                        NT_SANXUATGIONG_DETAIL _objDetail = new Models.NT_SANXUATGIONG_DETAIL();
                        FDB.Common.Helpers.GetValueForm<NT_SANXUATGIONG_DETAIL>(_form, lstInt[i], ref _objDetail);
                        _objDetail.ID_SANXUATGIONG = Id;
                        _context.NT_SANXUATGIONG_DETAIL.Add(_objDetail);
                    }
                    //Save data:
                    _context.SaveChanges();
                }

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Sản xuất giống thủy sản"));
                return RedirectToAction("Index");
            }
            else
            {
                TempData["_SUCCESS"] = "";
                this.LoadDanhMuc();
                //build html :
                int maxID = 0;
                String strHTML = string.Empty; //this.GenderHTML(lstInt, _form, ref maxID);
                ViewBag.AddHTML = strHTML;
                ViewBag.sMaxID = maxID + 1;
                return View(_obj);
            }
        }

        [EncryptedActionParameter]
        public ActionResult Edit(String questionId="0")
        {
            int _ID = int.Parse(questionId);
            var model = _context.NT_SANXUATGIONG.Find(_ID);
            if (model == null)
            {
                return HttpNotFound();
            }
            this.LoadDanhMuc();

            return View(model);
        }

        [HttpPost]
        //async Task<ActionResult>
        public ActionResult Edit(FormCollection _form, NT_SANXUATGIONG _obj)
        {
            List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_NUOI_SX_DOITUONG_")).ToList<String>();
            List<int> lstInt = new List<int>();
            lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

            if (ModelState.IsValid)
            {
                var model = _context.NT_SANXUATGIONG.First(o => o.ID == _obj.ID);
                FDB.Common.Helpers.CopyObject<NT_SANXUATGIONG>(_obj, ref model);
                model.NGUOI_NHAP = User.Identity.Name;
                var dbEntityEntry = _context.Entry(model);

                _context.NT_SANXUATGIONG.Attach(model);
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;

                //Xóa những detail cũ:
                _context.NT_SANXUATGIONG_DETAIL.Where(o => o.ID_SANXUATGIONG == _obj.ID).ToList().ForEach(o => _context.NT_SANXUATGIONG_DETAIL.Remove(o));

                //Thêm mới detail đã sửa
                int Id = _obj.ID;

                if (lstKeyName != null)
                {

                    for (int i = 0; i < lstInt.Count; i++)
                    {
                        NT_SANXUATGIONG_DETAIL _objDetail = new Models.NT_SANXUATGIONG_DETAIL();
                        FDB.Common.Helpers.GetValueForm<NT_SANXUATGIONG_DETAIL>(_form, lstInt[i], ref _objDetail);
                        _objDetail.ID_SANXUATGIONG = Id;
                        _context.NT_SANXUATGIONG_DETAIL.Add(_objDetail);
                    }

                }
                //Save data:
                _context.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Sản xuất giống thủy sản"));
                return RedirectToAction("Index");
            }
            else
            {
                TempData["_SUCCESS"] = "";
                this.LoadDanhMuc();
                _obj.DSNT_SanXuatGiongDetail = new List<NT_SANXUATGIONG_DETAIL>();
                //build html :
                int maxID = 0;
                String strHTML = string.Empty; //this.GenderHTML(lstInt, _form, ref maxID);
                ViewBag.sEditHTML = strHTML;
                ViewBag.sMaxID = maxID + 1;
                return View(_obj);
            }
        }

         [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            //Xóa header
            NT_SANXUATGIONG _obj = _context.NT_SANXUATGIONG.Find(id);
            _context.NT_SANXUATGIONG.Remove(_obj);
            _context.NT_SANXUATGIONG_DETAIL.Where(o => o.ID_SANXUATGIONG == _obj.ID).ToList().ForEach(o => _context.NT_SANXUATGIONG_DETAIL.Remove(o));
            //Update thay đổi vào DB
            _context.SaveChanges();
            this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "Sản xuất giống thủy sản"));
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult getDistrict(string ma_TinhTP)
        {
            var districts = _context.DQUANHUYEN.Where(d => d.MA_TINHTP == ma_TinhTP).Select(a => "<option value='" + a.MA_QUANHUYEN + "'>" + a.TEN_QUANHUYEN + "</option>");

            return Content(string.Join("", districts));
        }

        [HttpPost]
        public ActionResult getWard(string ma_QuanHuyen)
        {
            var districts = _context.DPHUONGXA.Where(d => d.MA_QUANHUYEN == ma_QuanHuyen).Select(a => "<option value='" + a.MA_PHUONGXA + "'>" + a.TEN_PHUONGXA + "</option>");

            return Content(string.Join("", districts));
        }


        #region "Function"
        //Nếu dùng Jquery Validate thì hàm này có thể bỏ
        //private String GenderHTML(List<int> lstInt, FormCollection _form, ref int MaxID)
        //{
        //    String strHTML = "";
        //    int _maxID = 0;
        //    if (lstInt != null && lstInt.Count > 0)
        //    {
        //        List<DM_NHOMDOITUONG_NUOI> lstDMNhomDoiTuongNuoi = _context.DM_NHOMDOITUONG_NUOI.ToList();
        //        List<DM_DOITUONG_NUOI> lstDMDoiTuongNuoi = _context.DM_DOITUONG_NUOI.ToList();
        //        List<DM_DOITUONG_NUOI> lstDMDoiTuongNuoiSelected = new List<DM_DOITUONG_NUOI>();
        //        int ID_NhomDoiTuongSelected = 1;

        //        for (int i = 0; i < lstInt.Count; i++)
        //        {
        //            strHTML += "<tr><td><select class=\"selectNhomNghe\" name=\"ddlID_NUOITRONG_NHOMDOITUONG_" + lstInt[i].ToString() + "\" id=\"ddlID_NUOITRONG_NHOMDOITUONG_" + lstInt[i].ToString() + "\">";
        //            lstDMNhomDoiTuongNuoi.ForEach(d =>
        //            {
        //                if (d.ID.ToString() == _form["ddlID_NUOITRONG_NHOMDOITUONG_" + lstInt[i].ToString()])
        //                {
        //                    ID_NhomDoiTuongSelected = d.ID;
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" selected=\"selected\">" + d.TEN_NHOM + "</option>";
        //                }

        //                else
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" >" + d.TEN_NHOM + "</option>";
        //            }
        //            );
        //            strHTML += "</td>";
        //            lstDMDoiTuongNuoiSelected = lstDMDoiTuongNuoi.Where(o => o.DM_NHOMDOITUONG_NUOIID == ID_NhomDoiTuongSelected).ToList();
        //            strHTML += "<td><select class=\"form-control\" name=\"ddlID_NUOITRONG_DOITUONG_" + lstInt[i].ToString() + "\" id=\"ddlID_NUOITRONG_DOITUONG_" + lstInt[i].ToString() + "\"><option value></option>";
        //            lstDMDoiTuongNuoiSelected.ForEach(d =>
        //            {
        //                if (d.ID.ToString() == _form["ddlID_NUOITRONG_DOITUONG_" + lstInt[i].ToString()])
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" selected=\"selected\">" + d.TEN_DOI_TUONG + "</option>";
        //                else
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" >" + d.TEN_DOI_TUONG + "</option>";
        //            }
        //                );

        //            strHTML += "</select>";
        //            strHTML += "</td>";

        //            strHTML += "<td><select class=\"form-control\" name=\"ddlID_HINHTHUC_SANXUAT_" + lstInt[i].ToString() + "\">";
        //            if (_form["ddlID_HINHTHUC_SANXUAT_" + lstInt[i].ToString()] == "1")
        //            {
        //                strHTML += "<option value=\"" + "1" + "\" selected=\"selected\">" + "Bố mẹ đến giống" + "</option>";
        //                strHTML += "<option value=\"" + "2" + "\" >" + "Bột/rong mầm đến giống" + "</option>";
        //            }
        //            else
        //            {
        //                strHTML += "<option value=\"" + "1" + ">" + "Bố mẹ đến giống" + "</option>";
        //                strHTML += "<option value=\"" + "2" + "\" selected=\"selected\">" + "Bột/rong mầm đến giống" + "</option>";
        //            }
        //            strHTML += "</select>";
        //            strHTML += "</td>";

        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtTONG_THETICH_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtTONG_THETICH_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtSAN_LUONG_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtSAN_LUONG_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtTONG_THETICH_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtTONG_THETICH_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_TU_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_TU_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_DEN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_DEN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNGUON_TRONG_NUOC_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNGUON_TRONG_NUOC_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNGUON_NGOAI_NUOC_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNGUON_NGOAI_NUOC_" + lstInt[i].ToString() + "\"/></td>";

        //            strHTML += "<td><label class=\"BtnPlus\" style=\"cursor:pointer\"><img src=\"/fonts/button-add-icon.png\" title=\"Thêm mới\" /></label>";
        //            strHTML += "<label class=\"BtnMinus\" id=\"lblDetail_\"" + (i + 1).ToString() + " style=\"cursor:pointer\"><img src=\"/fonts/DELETE.GIF\" title=\"Xóa\" /></label></td>";
        //            strHTML += "</tr>";

        //            if (lstInt[i] > _maxID)
        //                _maxID = lstInt[i];
        //        }

        //    }
        //    MaxID = _maxID;
        //    return strHTML;
        //}

        public void LoadDanhMuc(ViewModelSearchNT_SANXUATGIONG Searchmodel)
        {
            //Load danh mục:
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                        .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DMTThanhPho = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP", Searchmodel.TThanhPho);
            // ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN", Searchmodel.Qhuyen);
            ViewBag.DMDoiTuongSX = new SelectList(_context.DM_DOITUONG_NUOI_SANXUATGIONG, "ID", "TEN_DOI_TUONG", Searchmodel.DoiTuongNuoi);

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

        }

        public void LoadDanhMuc()
        {
            //Load danh mục:
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                    .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DMTThanhPho = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            //Load đối tượng:
            string sDoiTuongSXs = "";
            _context.DM_DOITUONG_NUOI_SANXUATGIONG.ToList().ForEach(d =>
            {
                sDoiTuongSXs += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_DOI_TUONG + "</option>";
            });

            ViewBag.DMDoiTuongSX = sDoiTuongSXs;

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }

        #endregion
    }
}