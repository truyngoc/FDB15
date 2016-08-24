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
    public class NT_NUOC_MANLOController : FDBController
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
        // GET: /Nuoi trong man ngot no/   
        public ActionResult Index(ViewModelSearchNT_NUOC_MANLO Searchmodel)
        {
            ApplicationUser curUser = this.getCurrentUser();
            //Load danh mục:
            this.LoadDanhMuc(Searchmodel);

            var models = _context.NT_NUOC_MANLO.Where(o => ((string.IsNullOrEmpty(Searchmodel.TThanhPho) && (string.IsNullOrEmpty(curUser.MA_TINHTP) || curUser.MA_TINHTP.StartsWith("Z"))) || (string.IsNullOrEmpty(Searchmodel.TThanhPho) && o.MA_TINHTP == curUser.MA_TINHTP) || o.MA_TINHTP == Searchmodel.TThanhPho)
                                                        && (Searchmodel.Thang == null || o.THANG == Searchmodel.Thang)
                                                        && (Searchmodel.Nam == null || o.NAM == Searchmodel.Nam)
                                                        && (Searchmodel.LoaiBaoCao == null || o.LOAI_BAO_CAO == Searchmodel.LoaiBaoCao)
                                                        && (Searchmodel.TuNgay == null || (o.NGAY_BAO_CAO_TU >= Searchmodel.TuNgay))
                                                        && (Searchmodel.DenNgay == null || (o.NGAY_BAO_CAO_DEN <= Searchmodel.DenNgay))
                                                        && (Searchmodel.DoiTuongNuoi == null || o.DSNT_NuocManNoDetail.Any(d => d.ID_DOITUONG_NUOI_MANLO == Searchmodel.DoiTuongNuoi))
                                                        && (Searchmodel.HinhThucNuoi == null || o.DSNT_NuocManNoDetail.Any(d => d.ID_HINHTHUC_NUOI == Searchmodel.HinhThucNuoi))
                                            )
                                            ;

            ViewBag.TotalRow = models.Count().ToString();
            models = models.OrderByDescending(m => m.ID);
            //Phân trang ở đây:
            var pageIndex = Searchmodel.Page ?? 1;
            Searchmodel.SearchResults = models.ToPagedList(pageIndex, FDB.Common.Constants.PageSize);

            return View(Searchmodel);
        }

        public ActionResult Search(ViewModelBaoCaoNT_NUOC_MANLO BaocaoModel)
        {
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                            .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DMTThanhPho = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP", BaocaoModel.TThanhPho);
            ViewBag.DMMoHinhNuoi = new SelectList(_context.DM_HINHTHUC_NUOI, "ID", "TEN_HINH_THUC", BaocaoModel.HinhThucNuoi);


            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;


            string _MA_TTP = BaocaoModel.TThanhPho;
            if (BaocaoModel.HinhThucNuoi != null)
            {


                var results = _context.Database.SqlQuery<ViewModelBaoCaoNT_NUOC_MANLO_Detail>("exec NT_NUOC_MANLO_SEARCH @LoaiBaoCao, @HinhThucNuoi, @Thang, @Nam, @TuNgay, @DenNgay, @TThanhPho "
                     , new SqlParameter("@LoaiBaoCao", BaocaoModel.LoaiBaoCao == null ? (object)DBNull.Value : BaocaoModel.LoaiBaoCao)
                     , new SqlParameter("@HinhThucNuoi", BaocaoModel.HinhThucNuoi == null ? (object)DBNull.Value : BaocaoModel.HinhThucNuoi)
                     , new SqlParameter("@Thang", BaocaoModel.Thang == null ? (object)DBNull.Value : BaocaoModel.Thang)
                     , new SqlParameter("@Nam", BaocaoModel.Nam == null ? (object)DBNull.Value : BaocaoModel.Nam)
                     , new SqlParameter("@TuNgay", BaocaoModel.TuNgay == null ? (object)DBNull.Value : BaocaoModel.TuNgay)
                     , new SqlParameter("@DenNgay", BaocaoModel.DenNgay == null ? (object)DBNull.Value : BaocaoModel.DenNgay)
                     , new SqlParameter("@TThanhPho", BaocaoModel.TThanhPho == null ? (object)DBNull.Value : BaocaoModel.TThanhPho)
                     ).ToList();


                BaocaoModel.ReportResults = results;
                string strTitle = @"Kết quả thống kê nuôi nước mặn, lợ theo hình thức nuôi " + (BaocaoModel.HinhThucNuoi == 1 ? "lồng bè " : "khác ") 
                                + (BaocaoModel.LoaiBaoCao == 1 ? "(tuần từ ngày " + BaocaoModel.TuNgay.Value.ToString("dd/MM/yyyy") + " đến ngày " + BaocaoModel.DenNgay.Value.ToString("dd/MM/yyyy") + ")"
                                : BaocaoModel.Thang != null ? " tháng " + BaocaoModel.Thang.ToString() + " Năm " + BaocaoModel.Nam.ToString() : " Năm " + BaocaoModel.Nam.ToString())
                    ;

                strTitle=strTitle+ (BaocaoModel.TThanhPho != null?" của " + _context.DTINHTP.FirstOrDefault(a => a.MA_TINHTP == _MA_TTP).TEN_TINHTP:" của toàn quốc ");

                ViewBag.strTitle = strTitle;
            }

            return View(BaocaoModel);
        }

        public ActionResult Create()
        {
            this.LoadDanhMuc();
            NT_NUOC_MANLO model = new NT_NUOC_MANLO();
            model.NGUOI_NHAP = User.Identity.Name;
            model.NGAY_NHAP = DateTime.Now;
            model.NAM = DateTime.Now.Year;
            //model.THANG = DateTime.Now.Month;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(FormCollection _form, NT_NUOC_MANLO _obj)
        {
            List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_DOITUONG_NUOI_MANLO_")).ToList<String>();
            List<int> lstInt = new List<int>();
            lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

            if (ModelState.IsValid)
            {
                //Save Header
                _obj.NGUOI_NHAP = User.Identity.Name;
                _obj.NGAY_NHAP = DateTime.Now;
                _obj.NAM = _obj.LOAI_BAO_CAO == 1 ? _obj.NGAY_BAO_CAO_TU.Value.Year : _obj.NAM;
                _context.NT_NUOC_MANLO.Add(_obj);
                _context.SaveChanges();

                int Id = _obj.ID;

                if (lstKeyName != null)
                {

                    for (int i = 0; i < lstInt.Count; i++)
                    {
                        NT_NUOC_MANLO_DETAIL _objDetail = new Models.NT_NUOC_MANLO_DETAIL();
                        FDB.Common.Helpers.GetValueForm<NT_NUOC_MANLO_DETAIL>(_form, lstInt[i], ref _objDetail);
                        _objDetail.ID_NUOC_MANLO = Id;
                        _context.NT_NUOC_MANLO_DETAIL.Add(_objDetail);
                    }
                    //Save data:
                    _context.SaveChanges();
                }

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_ADD_SUCCESS, "Nuôi thủy sản nước mặn và lợ"));
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
        public ActionResult Edit(String questionId = "0")
        {
            int _ID = Int32.Parse(questionId);
            var model = _context.NT_NUOC_MANLO.Find(_ID);
            if (model == null)
            {
                return HttpNotFound();
            }
            this.LoadDanhMuc();


            return View(model);
        }

        [HttpPost]
        //async Task<ActionResult>
        public ActionResult Edit(FormCollection _form, NT_NUOC_MANLO _obj)
        {
            List<string> lstKeyName = _form.AllKeys.ToList().Where(s => s.StartsWith("ddlID_DOITUONG_NUOI_MANLO_")).ToList<String>();
            List<int> lstInt = new List<int>();
            lstKeyName.ForEach(o => lstInt.Add(int.Parse(o.Split('_')[o.Split('_').Count() - 1])));

            if (ModelState.IsValid)
            {
                _obj.NAM = _obj.LOAI_BAO_CAO == 1 ? _obj.NGAY_BAO_CAO_TU.Value.Year : _obj.NAM;
                var model = _context.NT_NUOC_MANLO.First(o => o.ID == _obj.ID);

                FDB.Common.Helpers.CopyObject<NT_NUOC_MANLO>(_obj, ref model);
                model.NGUOI_NHAP = User.Identity.Name;

                var dbEntityEntry = _context.Entry(model);

                _context.NT_NUOC_MANLO.Attach(model);
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;

                //Xóa những detail cũ:
                _context.NT_NUOC_MANLO_DETAIL.Where(o => o.ID_NUOC_MANLO == _obj.ID).ToList().ForEach(o => _context.NT_NUOC_MANLO_DETAIL.Remove(o));

                //Thêm mới detail đã sửa
                int Id = _obj.ID;

                if (lstKeyName != null)
                {

                    for (int i = 0; i < lstInt.Count; i++)
                    {
                        NT_NUOC_MANLO_DETAIL _objDetail = new Models.NT_NUOC_MANLO_DETAIL();
                        FDB.Common.Helpers.GetValueForm<NT_NUOC_MANLO_DETAIL>(_form, lstInt[i], ref _objDetail);
                        _objDetail.ID_NUOC_MANLO = Id;
                        _context.NT_NUOC_MANLO_DETAIL.Add(_objDetail);
                    }

                }
                //Save data:
                _context.SaveChanges();

                this.Information(string.Format(FDB.Common.Constants.NOTIFY_UPDATE_SUCCESS, "Nuôi thủy sản nước mặn và lợ"));
                return RedirectToAction("Index");
            }
            else
            {
                TempData["_SUCCESS"] = "";
                this.LoadDanhMuc();
                _obj.DSNT_NuocManNoDetail = new List<NT_NUOC_MANLO_DETAIL>();
                //build html :
                int maxID = 0;
                String strHTML = string.Empty; //this.GenderHTML(lstInt, _form, ref maxID);
                ViewBag.sEditHTML = strHTML;
                ViewBag.sMaxID = maxID + 1;
                return View(_obj);
            }
        }

        #region"Delete"
        //[EncryptedActionParameter]
        //public ActionResult Delete(object id)
        //{
        //    //Xóa header
        //    int _id = Convert.ToInt32(id);
        //    NT_NUOC_MANLO _obj = _context.NT_NUOC_MANLO.Find(_id);
        //    _context.NT_NUOC_MANLO.Remove(_obj);
        //    _context.NT_NUOC_MANLO_DETAIL.Where(o => o.ID_NUOC_MANLO == _obj.ID).ToList().ForEach(o => _context.NT_NUOC_MANLO_DETAIL.Remove(o));
        //    //Update thay đổi vào DB
        //    _context.SaveChanges();
        //    this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "Nuôi trồng nước mặn và lợ"));
        //    return RedirectToAction("Index");
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string id)
        {
            //Xóa header
            int _id = Convert.ToInt32(id);
            NT_NUOC_MANLO _obj = _context.NT_NUOC_MANLO.Find(_id);
            _context.NT_NUOC_MANLO.Remove(_obj);
            _context.NT_NUOC_MANLO_DETAIL.Where(o => o.ID_NUOC_MANLO == _obj.ID).ToList().ForEach(o => _context.NT_NUOC_MANLO_DETAIL.Remove(o));
            //Update thay đổi vào DB
            _context.SaveChanges();
            this.Information(string.Format(FDB.Common.Constants.NOTIFY_DELETE_SUCCESS, "Nuôi thủy sản nước mặn và lợ"));
            return RedirectToAction("Index");
        }

        #endregion


        [HttpPost]
        public ActionResult getNhomDoiTuongByHinhThuc(string ma_HinhThuc)
        {
            int IDHinhThuc = 0;
            if (!string.IsNullOrEmpty(ma_HinhThuc))
                IDHinhThuc = int.Parse(ma_HinhThuc);
            var DoiTuongs = _context.DM_DOITUONG_NUOI_MANLO.Where(d => d.LOAI_DOI_TUONG == IDHinhThuc).Select(a => "<option value='" + a.ID + "'>" + a.TEN_DOI_TUONG + "</option>");

            return Content(string.Join("", DoiTuongs));
        }


        //[HttpPost]
        //public ActionResult getDoiTuongByNhom(string ma_NhomDoiTuong)
        //{
        //    if (String.IsNullOrEmpty(ma_NhomDoiTuong))
        //        return Content(String.Join("", new Object[] { "", "" }));

        //    int IDNhomDoiTuong = int.Parse(ma_NhomDoiTuong);
        //    var Nghes = _context.DM_DOITUONG_NUOI.Where(d => d.DM_NHOMDOITUONG_NUOIID == IDNhomDoiTuong).Select(a => "<option value='" + a.ID + "'>" + a.TEN_DOI_TUONG + "</option>");

        //    return Content(string.Join("", Nghes));
        //}


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
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtDIEN_TICH_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDIEN_TICH_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtSAN_LUONG_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtSAN_LUONG_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_TU_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_TU_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtNANG_SUAT_DEN_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtNANG_SUAT_DEN_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtDIEN_TICH_GAP_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDIEN_TICH_GAP_" + lstInt[i].ToString() + "\"/></td>";
        //            strHTML += "<td><input type=\"text\" value=\"" + _form["txtDIEN_TICH_NUOI_CDK_" + lstInt[i].ToString()] + "\" class=\"form-control\" name=\"txtDIEN_TICH_NUOI_CDK_" + lstInt[i].ToString() + "\"/></td>";

        //            strHTML += "<td><select class=\"form-control\" name=\"ddlID_MOHINH_NUOI_" + lstInt[i].ToString() + "\">";
        //            CategoryCommon.DM_MOHINH_NUOIs.ForEach(d =>
        //            {
        //                if (d.ID.ToString() == _form["ddlID_MOHINH_NUOI_" + lstInt[i].ToString()])
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" selected=\"selected\">" + d.TEN + "</option>";
        //                else
        //                    strHTML += "<option value=\"" + d.ID.ToString() + "\" >" + d.TEN + "</option>";
        //            }
        //               );

        //            strHTML += "</select>";
        //            strHTML += "</td>";

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

        public void LoadDanhMuc(ViewModelSearchNT_NUOC_MANLO Searchmodel)
        {
            //Load danh mục:
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                            .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            ViewBag.DMTThanhPho = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP", Searchmodel.TThanhPho);
            // ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN", Searchmodel.Qhuyen);
            ViewBag.DMMoHinhNuoi = new SelectList(_context.DM_HINHTHUC_NUOI, "ID", "TEN_HINH_THUC", Searchmodel.HinhThucNuoi);

            var doituongManLo = _context.DM_DOITUONG_NUOI_MANLO.Where(d => d.LOAI_DOI_TUONG == Searchmodel.DoiTuongNuoi);
            ViewBag.DMDoiTuongNuoi = new SelectList(doituongManLo, "ID", "TEN_DOI_TUONG", Searchmodel.DoiTuongNuoi);

            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;

        }

        public void LoadDanhMuc()
        {
            ApplicationUser curUser = this.getCurrentUser();
            var tinh = _context.DTINHTP.Where(u => (curUser.MA_TINHTP == null || curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP)
                .Except(_context.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z")));
            //Load danh mục:
            ViewBag.DMTThanhPho = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP", curUser.MA_TINHTP);
            // ViewBag.DMQhuyen = new SelectList(_context.DQUANHUYEN, "MA_QUANHUYEN", "TEN_QUANHUYEN");


            //Load đối tượng:
            string sDoiTuongNuoiLongBes = "";
            _context.DM_DOITUONG_NUOI_MANLO.Where(o => o.LOAI_DOI_TUONG == 1).ToList().ForEach(d =>
            {
                sDoiTuongNuoiLongBes += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_DOI_TUONG + "</option>";
            });

            ViewBag.DMDoiTuongNuoiLongBe = sDoiTuongNuoiLongBes;

            string sDoiTuongNuoiKhacs = "";
            _context.DM_DOITUONG_NUOI_MANLO.Where(o => o.LOAI_DOI_TUONG == 2).ToList().ForEach(d =>
            {
                sDoiTuongNuoiKhacs += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_DOI_TUONG + "</option>";
            });

            ViewBag.DMDoiTuongNuoiKhac = sDoiTuongNuoiKhacs;

            //Load hình thức nuôi:
            string sHinhThucNuois = "";
            _context.DM_HINHTHUC_NUOI.ToList().ForEach(d =>
            {
                sHinhThucNuois += "<option value=\"" + d.ID.ToString() + "\">" + d.TEN_HINH_THUC + "</option>";
            });
            ViewBag.DMHinhThucNuoi = sHinhThucNuois;



            ViewBag.DEFAULT_VALUE_DDL = CategoryCommon.DEFAULT_VALUE_DDL;
        }

        #endregion
    }
}