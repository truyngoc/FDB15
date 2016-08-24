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

using Microsoft.AspNet.Identity;

namespace FDB.Controllers.KhaiThac
{
    public class KT_SANLUONG_FAOController : Controller
    {
        FDBContext db = new FDBContext();
        public ApplicationUser getCurrentUser()
        {
            ApplicationDbContext dbUser = new ApplicationDbContext();

            string userId = User.Identity.GetUserId();

            ApplicationUser _currentUser = dbUser.Users.Find(userId);

            return _currentUser;
        }

        public void initialCategorySearchAction(ref string ma_Tinh)
        {
            ApplicationUser curUser = getCurrentUser();

            ViewBag.DM_NHOMNGHEs = new SelectList(db.DM_NHOMNGHE, "DM_NhomNgheID", "TenNhomNghe");
            ViewBag.NHOM_TAUs = new SelectList(db.DNHOM_TAU, "ID", "Name");

            //var tinh = db.DTINHTP.Where(u => curUser.MA_TINHTP == null || u.MA_TINHTP == curUser.MA_TINHTP);
            var tinh = db.DTINHTP.Where(u => (string.IsNullOrEmpty(curUser.MA_TINHTP)||curUser.MA_TINHTP.StartsWith("Z")) || u.MA_TINHTP == curUser.MA_TINHTP).Except(db.DTINHTP.Where(d => d.MA_TINHTP.StartsWith("Z") == true));
            ViewBag.TINH_THANHPHOs = new SelectList(tinh, "MA_TINHTP", "TEN_TINHTP");

            ma_Tinh = curUser.MA_TINHTP;
            ViewBag.MA_TINHTP = curUser.MA_TINHTP;
        }

        //
        // GET: /KT_SANLUONG_FAO/
        public ActionResult Index(ViewModelSearchKT_SANLUONG_FAO t)
        {
            string strTitle = "";
            string ma_TinhTP = "";
            if (t.Thang == null) t.Thang = DateTime.Today.Month;
            if (t.Nam == null) t.Nam = DateTime.Today.Year;
            initialCategorySearchAction(ref ma_TinhTP);

            if (t != null && t.Nam != null && t.Thang != null && t.TinhTP != null)
            {
                string _MA_TTP = t.TinhTP;
                int _THANG = t.Thang.Value;
                int _NAM = t.Nam.Value;

                Dictionary<string, KT_SANLUONG_FAO_NHOMTAU> _dictKT_SANLUONG = this.KT_SANLUONG_FAO_NHOMTAU(_MA_TTP, _THANG, _NAM);
                Dictionary<string, KT_SANLUONG_FAO_NHOMNGHE> _dictKT_SANLUONG_NHOMNGHE = new Dictionary<string, Models.KT_SANLUONG_FAO_NHOMNGHE>();
                Dictionary<string, KT_SANLUONG_FAO_NHOMLOAI> dictSANLUONG_NHOMLOAI_NHOMNGHE = new Dictionary<string, KT_SANLUONG_FAO_NHOMLOAI>();

                List<DM_NHOMNGHE> _lstDMNhomNghe = db.DM_NHOMNGHE.ToList();
                List<DNHOM_TAU> _lstDMNhomTau = db.DNHOM_TAU.ToList();

                foreach (var _itemNhomNghe in _lstDMNhomNghe)
                {
                    //tính toán sản lượng từng nhóm nghề
                    _dictKT_SANLUONG_NHOMNGHE.Add(_MA_TTP + "#" + _THANG.ToString() + "#" + _NAM.ToString() + "#" + _itemNhomNghe.DM_NhomNgheID.ToString(), new KT_SANLUONG_FAO_NHOMNGHE(_MA_TTP, _THANG, _NAM, _itemNhomNghe, _lstDMNhomTau, _dictKT_SANLUONG));
                }
                //tính toán sản lượng của tháng
                KT_SANLUONG_FAO_THANG _ktSanLuong_Thang = new KT_SANLUONG_FAO_THANG(_MA_TTP, _THANG, _NAM, _lstDMNhomNghe, _dictKT_SANLUONG_NHOMNGHE);


                KT_View_SANLUONG_FAO _ktDictSL = new KT_View_SANLUONG_FAO();
                _ktDictSL.dictSANLUONG_FAO = _dictKT_SANLUONG;
                _ktDictSL.dictSANLUONG_NHOMNGHE = _dictKT_SANLUONG_NHOMNGHE;
                _ktDictSL.ktSANLUONG_THANG = _ktSanLuong_Thang;

                if (t.LoaiThongKe == 2)
                {
                    //Thống kê theo nhóm nghề nhóm loài:

                    var sumSanLuongs = db.KT_CPUE.Where(o => o.MA_TINHTP == _MA_TTP && o.NGAY_CAP_BEN.Value.Month == _THANG && o.NGAY_CAP_BEN.Value.Year == _NAM)
                     .GroupBy(g => g.DM_NhomNgheID)
                     .Select(r => new
                     {
                         DM_NhomNgheID = r.Key.Value,
                         SAN_LUONG_TOM = r.Sum(x => x.SAN_LUONG_TOM),
                         SAN_LUONG_CA_CHON = r.Sum(x => x.SAN_LUONG_CA_CHON),
                         SAN_LUONG_CA_XO = r.Sum(x => x.SAN_LUONG_CA_XO),
                         SAN_LUONG_CA_TAP = r.Sum(x => x.SAN_LUONG_CA_TAP),
                         SAN_LUONG_CA_NGU_DD = r.Sum(x => x.SAN_LUONG_CA_NGU_DD),
                         SAN_LUONG_MUC_ONG = r.Sum(x => x.SAN_LUONG_MUC_ONG),
                         SAN_LUONG_MUC_NANG = r.Sum(x => x.SAN_LUONG_MUC_NANG),
                         SAN_LUONG_KHAC = r.Sum(x => x.SAN_LUONG_KHAC)
                     }).ToDictionary(o => o.DM_NhomNgheID);// lấy danh mục nhóm nghề id làm key

                    Dictionary<string, double> _dictKT_SANLUONG_NHOMLOAI_NHOMNGHE;

                    foreach (var _itemNhomNghe in _lstDMNhomNghe)
                    {
                        _dictKT_SANLUONG_NHOMLOAI_NHOMNGHE = new Dictionary<string, double>();
                        var _key = _itemNhomNghe.DM_NhomNgheID;

                        foreach (var _itemNhomLoaiKey in KT_DM_SANLUONG_NHOMLOAI.dictKT_DM_SANLUONG_NHOMLOAI.Keys)
                        {
                            var _SL = 0.0;

                            if (sumSanLuongs != null && sumSanLuongs.ContainsKey(_key))
                            {
                                switch (_itemNhomLoaiKey)
                                {
                                    case "SAN_LUONG_TOM":
                                        var _SL1 = sumSanLuongs[_key].SAN_LUONG_TOM;
                                        _SL = _SL1 == null ? 0.0 : (double)_SL1.Value;
                                        break;
                                    case "SAN_LUONG_CA_CHON":
                                        var _SL2 = sumSanLuongs[_key].SAN_LUONG_CA_CHON;
                                        _SL = _SL2 == null ? 0.0 : (double)_SL2.Value;
                                        break;
                                    case "SAN_LUONG_CA_XO":
                                        var _SL3 = sumSanLuongs[_key].SAN_LUONG_CA_XO;
                                        _SL = _SL3 == null ? 0.0 : (double)_SL3.Value;
                                        break;
                                    case "SAN_LUONG_CA_TAP":
                                        var _SL4 = sumSanLuongs[_key].SAN_LUONG_CA_TAP;
                                        _SL = _SL4 == null ? 0.0 : (double)_SL4.Value;
                                        break;
                                    case "SAN_LUONG_CA_NGU_DD":
                                        var _SL8 = sumSanLuongs[_key].SAN_LUONG_CA_NGU_DD;
                                        _SL = _SL8 == null ? 0.0 : (double)_SL8.Value;
                                        break;
                                    case "SAN_LUONG_MUC_ONG":
                                        var _SL5 = sumSanLuongs[_key].SAN_LUONG_MUC_ONG;
                                        _SL = _SL5 == null ? 0.0 : (double)_SL5.Value;
                                        break;
                                    case "SAN_LUONG_MUC_NANG":
                                        var _SL6 = sumSanLuongs[_key].SAN_LUONG_MUC_NANG;
                                        _SL = _SL6 == null ? 0.0 : (double)_SL6.Value;
                                        break;
                                    case "SAN_LUONG_KHAC":
                                        var _SL7 = sumSanLuongs[_key].SAN_LUONG_KHAC;
                                        _SL = _SL7 == null ? 0.0 : (double)_SL7.Value;
                                        break;
                                    case "SAN_LUONG_CANGU_DD":
                                        break;
                                }
                            }

                            _dictKT_SANLUONG_NHOMLOAI_NHOMNGHE.Add(_MA_TTP + "#" + _THANG.ToString() + "#" + _NAM.ToString() + "#" + _itemNhomLoaiKey + "#" + _itemNhomNghe.DM_NhomNgheID.ToString(), _SL);
                            _SL = 0.0;
                        }

                        dictSANLUONG_NHOMLOAI_NHOMNGHE.Add(_MA_TTP + "#" + _THANG.ToString() + "#" + _NAM.ToString() + "#" + _itemNhomNghe.DM_NhomNgheID.ToString()
                                        , new KT_SANLUONG_FAO_NHOMLOAI(_MA_TTP, _THANG, _NAM, _itemNhomNghe
                                        , _dictKT_SANLUONG_NHOMLOAI_NHOMNGHE, KT_DM_SANLUONG_NHOMLOAI.dictKT_DM_SANLUONG_NHOMLOAI
                                        , _dictKT_SANLUONG_NHOMNGHE[_MA_TTP + "#" + _THANG.ToString() + "#" + _NAM.ToString() + "#" + _itemNhomNghe.DM_NhomNgheID.ToString()].dKT_SANLUONG_NHOMNGHE
                                        , _dictKT_SANLUONG_NHOMNGHE[_MA_TTP + "#" + _THANG.ToString() + "#" + _NAM.ToString() + "#" + _itemNhomNghe.DM_NhomNgheID.ToString()].dKT_TONG_SAN_LUONG_MAU
                                         )
                                       );
                    }

                    _ktDictSL.dictSANLUONG_NHOMLOAI_NHOMNGHE = dictSANLUONG_NHOMLOAI_NHOMNGHE;

                }

                t.KTSanLuongFao = _ktDictSL;
                strTitle = "Kết quả thống kê sản lượng theo công thức thống kê FAO Tháng " + (_THANG < 10 ? "0" + _THANG.ToString() : _THANG.ToString()) + "/" + _NAM.ToString() + " của " + db.DTINHTP.FirstOrDefault(a => a.MA_TINHTP == _MA_TTP).TEN_TINHTP;
                ViewBag.strTitle = strTitle;

            }


            return View(t);
        }

        #region "Công thức tính toán sản lượng theo FAO"
        public Dictionary<string, KT_SANLUONG_FAO_NHOMTAU> KT_SANLUONG_FAO_NHOMTAU(String pMA_TTP, int pTHANG, int pNAM)
        {

            int _iTHANG = pTHANG;
            int _iNAM = pNAM;
            string _MA_TTP = pMA_TTP;

            List<DM_NHOMNGHE> _lstDMNhomNghe = db.DM_NHOMNGHE.ToList();
            List<DNHOM_TAU> _lstDMNhomTau = db.DNHOM_TAU.ToList();
            List<KT_TAUTHUYEN> _lstTauThuyen = db.KT_TAUTHUYEN.Where(t => t.MA_TINHTP == _MA_TTP && t.DCONG_DUNG_TAUID == 1 && t.DTINH_TRANG_TAUID < 3).ToList();


            ////Dictionary để lưu thông tin Tổng số tàu theo nhóm công suất của từng nhóm nghề
            //Dictionary<string, int> _dictF = new Dictionary<string, int>();

            ////Tính tổng F theo nhóm tàu, nhóm nghề:
            //foreach (var itemNhomNghe in _lstDMNhomNghe)
            //{
            //    foreach (var itemNhomTau in _lstDMNhomTau)
            //    {
            //        var _iF1 = 0;
            //        _iF1 = _lstTauThuyen.Count(t => (t.NGHE_CHINH_ID == itemNhomNghe.DM_NhomNgheID || t.NGHE_PHU_ID == itemNhomNghe.DM_NhomNgheID) && t.DNHOM_TAUID == itemNhomTau.ID);


            //        _dictF.Add(_MA_TTP + "#" + itemNhomTau.ID.ToString() + "#" + itemNhomNghe.DM_NhomNgheID.ToString(), _iF1);
            //    }
            //}

            //get KT_BAC by tháng
            List<KT_BAC> _lstKT_BAC = new List<KT_BAC>();
            _lstKT_BAC = db.KT_BAC.Where(k => k.THANG == _iTHANG && k.NAM == _iNAM && k.MA_TINHTP == _MA_TTP).ToList();

            //get KT_CPUE by tháng
            List<KT_CPUE> _lstKT_CPUE = new List<KT_CPUE>();
            _lstKT_CPUE = db.KT_CPUE.Where(k => k.NGAY_CAP_BEN.Value.Month == _iTHANG && k.NGAY_CAP_BEN.Value.Year == _iNAM && k.MA_TINHTP == _MA_TTP).ToList();

            //get KT_NGAYHOATDONG by tháng
            List<KT_NGAYHOATDONG> _lstKT_NGAYHOATDONG = new List<KT_NGAYHOATDONG>();
            _lstKT_NGAYHOATDONG = db.KT_NGAYHOATDONG.Where(k => k.THANG == _iTHANG && k.NAM == _iNAM && k.MA_TINHTP == _MA_TTP).ToList();

            //Tính toán sản lượng theo nhóm tàu, nhóm nghề:
            KT_SANLUONG_FAO_NHOMTAU _ktSanLuongFao_NhomTau = new KT_SANLUONG_FAO_NHOMTAU();
            Dictionary<string, KT_SANLUONG_FAO_NHOMTAU> dictKT_SANLUONGByNhomTauNhomNghe = new Dictionary<string, KT_SANLUONG_FAO_NHOMTAU>();

            foreach (var _itemNhomNghe in _lstDMNhomNghe)
            {

                foreach (var _itemNhomTau in _lstDMNhomTau)
                {
                    var _lstKT_BAC_By_NhomTauNghe = _lstKT_BAC.Where(t => t.DM_NhomNgheID == _itemNhomNghe.DM_NhomNgheID && t.DNHOM_TAUID == _itemNhomTau.ID).ToList();
                    var _lstKT_CPUE_By_NhomTauNghe = _lstKT_CPUE.Where(t => t.DM_NhomNgheID == _itemNhomNghe.DM_NhomNgheID && t.DNHOM_TAUID == _itemNhomTau.ID).ToList();
                    var _lstKT_NGAYHOATDONG_NhomTauNghe = _lstKT_NGAYHOATDONG.Where(t => t.DM_NhomNgheID == _itemNhomNghe.DM_NhomNgheID && t.DNHOM_TAUID == _itemNhomTau.ID).ToList();
                    var _F = _lstTauThuyen.Count(t => (t.NGHE_CHINH_ID == _itemNhomNghe.DM_NhomNgheID || t.NGHE_PHU_ID == _itemNhomNghe.DM_NhomNgheID) && t.DNHOM_TAUID == _itemNhomTau.ID);

                    KT_SANLUONG_FAO_NHOMTAU _ktSanLuongFAO = new KT_SANLUONG_FAO_NHOMTAU
                                                                (_MA_TTP, _iTHANG, _iNAM, _itemNhomTau.ID, _itemNhomNghe.DM_NhomNgheID
                                                                    , _lstKT_BAC_By_NhomTauNghe, _lstKT_CPUE_By_NhomTauNghe, _lstKT_NGAYHOATDONG_NhomTauNghe
                                                                    , _F //_dictF[_MA_TTP + "#" + _itemNhomTau.ID.ToString() + "#" + _itemNhomNghe.DM_NhomNgheID.ToString()]
                                                                );
                    dictKT_SANLUONGByNhomTauNhomNghe.Add(_MA_TTP + "#" + _iTHANG.ToString() + "#" + _iNAM.ToString() + "#" + _itemNhomTau.ID.ToString() + "#" + _itemNhomNghe.DM_NhomNgheID.ToString(), _ktSanLuongFAO);
                }
            }

            return dictKT_SANLUONGByNhomTauNhomNghe;
        }

        #endregion


    }
}