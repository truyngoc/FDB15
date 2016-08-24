using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDB.Models
{
    public class KT_View_SANLUONG_FAO
    {
        public Dictionary<string, KT_SANLUONG_FAO_NHOMTAU> dictSANLUONG_FAO { get; set; }
        public Dictionary<string, KT_SANLUONG_FAO_NHOMNGHE> dictSANLUONG_NHOMNGHE { get; set; }
        public Dictionary<string, KT_SANLUONG_FAO_NHOMLOAI> dictSANLUONG_NHOMLOAI_NHOMNGHE { get; set; }

        public KT_SANLUONG_FAO_THANG ktSANLUONG_THANG { get; set; }
    }


    //Tính toán sản lượng theo tháng
    public class KT_SANLUONG_FAO_THANG
    {
        public KT_SANLUONG_FAO_THANG()
        {

        }

        public KT_SANLUONG_FAO_THANG(string _Ma_TTP, int _Thang, int _Nam, List<DM_NHOMNGHE> _DNhomNghes, Dictionary<string, KT_SANLUONG_FAO_NHOMNGHE> _dict_SanLuong_FAO_NhomNghe)
        {
            this.MA_TTP = _Ma_TTP;
            this.THANG = _Thang;
            this.NAM = _Nam;
            this.DNhomNghes = _DNhomNghes;
            this.dict_SanLuong_FAO_NhomNghe = _dict_SanLuong_FAO_NhomNghe;
            this.KT_SanLuong_FAO_NhomNghe = new List<KT_SANLUONG_FAO_NHOMNGHE>();
        }
        public string MA_TTP { get; set; }
        public int THANG { get; set; } //tháng điều tra
        public int NAM { get; set; }//năm điều tra
        public List<DM_NHOMNGHE> DNhomNghes { get; set; }
        public List<KT_SANLUONG_FAO_NHOMNGHE> KT_SanLuong_FAO_NhomNghe { get; set; }

        public Dictionary<string, KT_SANLUONG_FAO_NHOMNGHE> dict_SanLuong_FAO_NhomNghe { get; set; }

        public double dKT_SANLUONG_FAO_THANG
        {
            get
            {
                double _SL = 0.0;
                if (this.dict_SanLuong_FAO_NhomNghe != null && this.dict_SanLuong_FAO_NhomNghe.Count > 0)
                {
                    foreach (var _itemNhomNghe in this.DNhomNghes)
                        _SL += this.dict_SanLuong_FAO_NhomNghe[this.MA_TTP + "#" + this.THANG.ToString() + "#" + this.NAM.ToString() + "#" + _itemNhomNghe.DM_NhomNgheID.ToString()].dKT_SANLUONG_NHOMNGHE;
                }
                return _SL;
            }
        }
    }


    //Tính toán sản lượng theo nhóm loài:
    public static class KT_DM_SANLUONG_NHOMLOAI
    {
        public static Dictionary<string, string> dictKT_DM_SANLUONG_NHOMLOAI = new Dictionary<string, string>
        {
             {"SAN_LUONG_TOM", "Tôm"},
             {"SAN_LUONG_CA_CHON", "Cá chọn"},
             {"SAN_LUONG_CA_XO", "Cá xô"},
             {"SAN_LUONG_CA_TAP", "Cá tạp"},
             {"SAN_LUONG_CA_NGU_DD", "Cá ngừ đại dương"},
             {"SAN_LUONG_MUC_ONG", "Mực ống"},
             {"SAN_LUONG_MUC_NANG", "Mực nang"},
             {"SAN_LUONG_KHAC", "Khác"}
        };
    }
    public class KT_SANLUONG_FAO_NHOMLOAI
    {
       
        public KT_SANLUONG_FAO_NHOMLOAI()
        {

        }

        public KT_SANLUONG_FAO_NHOMLOAI(string _Ma_TTP, int _Thang, int _Nam, DM_NHOMNGHE _DNhomNghe, Dictionary<string, double> _dictSanLuongNhomLoaiNhomNgheMau, Dictionary<string, string> _dictNhomLoai, double _KT_SanLuong_NhomNghe, double _KT_TongSanLuong_NhomNghe_Mau)
        {
            this.MA_TTP = _Ma_TTP;
            this.THANG = _Thang;
            this.NAM = _Nam;
            this.DNhomNghe = _DNhomNghe;
            this.dictKT_SANLUONG_NHOMLOAI_NHOMNGHE_MAU = _dictSanLuongNhomLoaiNhomNgheMau;
            this.dictNHOMLOAIs = _dictNhomLoai;
            this.dKT_SANLUONG_NHOMNGHE = _KT_SanLuong_NhomNghe;
            this.dKT_TONG_SANLUONG_NHOMNGHE_MAU = _KT_TongSanLuong_NhomNghe_Mau;
        }

        public string MA_TTP { get; set; }
        public int THANG { get; set; } //tháng điều tra
        public int NAM { get; set; }//năm điều tra
        public DM_NHOMNGHE DNhomNghe { get; set; }
        public Dictionary<string, string> dictNHOMLOAIs { get; set; }
        public Dictionary<string, double> dictKT_SANLUONG_NHOMLOAI_NHOMNGHE_MAU
        {
            get; 
            set; 
        }


        //Tính tổng sản lượng =SUM(Từng item sản lượng theo nhóm loài, nhóm nghề)
        //public double dKT_TONG_SANLUONG_NHOMNGHE_MAU
        //{
        //    get
        //    {
        //        var _SL = 0.0;
        //        if (this.dictKT_SANLUONG_NHOMLOAI_NHOMNGHE_MAU != null && this.dictKT_SANLUONG_NHOMLOAI_NHOMNGHE_MAU.Count > 0)
        //        {
        //            foreach (var _itemNhomLoaiKey in this.dictNHOMLOAIs.Keys)
        //            {
        //                _SL += this.dictKT_SANLUONG_NHOMLOAI_NHOMNGHE_MAU[this.MA_TTP + "#" + this.THANG.ToString() + "#" + this.NAM.ToString() + "#" + _itemNhomLoaiKey + "#" + this.DNhomNghe.DM_NhomNgheID.ToString()];
        //            }
        //        }
        //        return _SL;

        //    }
           
        //}


        //tính tổng sản lượng = SUM(TONG_SAN_LUONG) theo nhóm nghề
        public double dKT_TONG_SANLUONG_NHOMNGHE_MAU
        {
            //tính tổng sản lượng = SUM(TONG_SAN_LUONG) theo nhóm nghề
            get;
            set;

        }

        public double dKT_SANLUONG_NHOMNGHE { get;set; }

        public Dictionary<string,double> dictKT_SANLUONG_NHOMNGHE_FAO
        {
            get
            {
                Dictionary<string, double> _dictSLFAO = new Dictionary<string, double>();
                
                if (this.dictKT_SANLUONG_NHOMLOAI_NHOMNGHE_MAU != null && this.dictKT_SANLUONG_NHOMLOAI_NHOMNGHE_MAU.Count > 0)
                {
                   
                    foreach (var _itemNhomLoaiKey in this.dictNHOMLOAIs.Keys)
                    {
                        var _SL = 0.0;
                        if(this.dKT_TONG_SANLUONG_NHOMNGHE_MAU>0)
                        {
                            _SL = (double)this.dictKT_SANLUONG_NHOMLOAI_NHOMNGHE_MAU[this.MA_TTP + "#" + this.THANG.ToString() + "#" + this.NAM.ToString() + "#" + _itemNhomLoaiKey + "#" + this.DNhomNghe.DM_NhomNgheID.ToString()] * this.dKT_SANLUONG_NHOMNGHE / this.dKT_TONG_SANLUONG_NHOMNGHE_MAU;
                        }
                        _dictSLFAO.Add(this.MA_TTP + "#" + this.THANG.ToString() + "#" + this.NAM.ToString() + "#" + _itemNhomLoaiKey + "#" + this.DNhomNghe.DM_NhomNgheID.ToString(), _SL);
                    }
                }

                return _dictSLFAO;
            }
        }
     
    }

    //Tính toán sản lượng theo nhóm nghề khai thác
    public class KT_SANLUONG_FAO_NHOMNGHE
    {
        public KT_SANLUONG_FAO_NHOMNGHE()
        {

        }

        public KT_SANLUONG_FAO_NHOMNGHE(string _Ma_TTP, int _Thang, int _Nam, DM_NHOMNGHE _DNhomNghe, List<DNHOM_TAU> _NhomTaus, Dictionary<string, KT_SANLUONG_FAO_NHOMTAU> _dictKTSanLuong_FAO)
        {
            this.MA_TTP = _Ma_TTP;
            this.THANG = _Thang;
            this.NAM = _Nam;
            this.DNhomNghe = _DNhomNghe;
            this.NhomTaus = _NhomTaus;
            this.dictKTSanLuong_FAO = _dictKTSanLuong_FAO;
            this.KT_SanLuong_FAO_NhomTau = new List<KT_SANLUONG_FAO_NHOMTAU>();
        }

        public string MA_TTP { get; set; }
        public int THANG { get; set; } //tháng điều tra
        public int NAM { get; set; }//năm điều tra
        public DM_NHOMNGHE DNhomNghe { get; set; }
        public List<DNHOM_TAU> NhomTaus { get; set; }

        public List<KT_SANLUONG_FAO_NHOMTAU> KT_SanLuong_FAO_NhomTau { get; set; }
        public Dictionary<string, KT_SANLUONG_FAO_NHOMTAU> dictKTSanLuong_FAO { get; set; }

        public double dKT_TONG_SAN_LUONG_MAU
        {
            get
            {
                double _SL = 0.0;
                if (this.dictKTSanLuong_FAO != null && this.dictKTSanLuong_FAO.Count > 0)
                {
                    foreach (var _itemNhomTau in this.NhomTaus)
                    {
                        _SL += this.dictKTSanLuong_FAO[this.MA_TTP + "#" + this.THANG.ToString() + "#" + this.NAM.ToString() + "#" + _itemNhomTau.ID.ToString() + "#" + this.DNhomNghe.DM_NhomNgheID.ToString()].dKT_CPUE_TONG_SAN_LUONG_MAU;
                    }
                }
                return _SL;
            }
        }
        public double dKT_SANLUONG_NHOMNGHE
        {
            get
            {
                double _SL = 0.0;
                if (this.dictKTSanLuong_FAO != null && this.dictKTSanLuong_FAO.Count > 0)
                {
                    foreach (var _itemNhomTau in this.NhomTaus)
                    {
                        _SL += this.dictKTSanLuong_FAO[this.MA_TTP + "#" + this.THANG.ToString() + "#" + this.NAM.ToString() + "#" + _itemNhomTau.ID.ToString() + "#" + this.DNhomNghe.DM_NhomNgheID.ToString()].dKT_SANLUONG_FAO;
                    }
                }
                return _SL;
            }
        }
    }

    //Tính toán sản lượng theo nhóm công suất tàu
    public class KT_SANLUONG_FAO_NHOMTAU
    {
        public KT_SANLUONG_FAO_NHOMTAU()
        {

        }

        public KT_SANLUONG_FAO_NHOMTAU(string _Ma_TTP, int _Thang, int _Nam, int _NhomTauID, int _NhomNgheID, List<KT_BAC> _lstKT_BAC, List<KT_CPUE> _lstKT_CPUE, List<KT_NGAYHOATDONG> _lstKT_NGAYHOATDONG, int _iF)
        {
            this.MA_TTP = _Ma_TTP;
            this.THANG = _Thang;
            this.NAM = _Nam;
            this.NhomTauID = _NhomTauID;
            this.NhomNgheID = _NhomNgheID;
            this.lstKT_BAC = _lstKT_BAC;
            this.lstKT_CPUE = _lstKT_CPUE;
            this.lstKT_NGAYHOATDONG = _lstKT_NGAYHOATDONG;
            this.iF = _iF;
        }

        public string MA_TTP { get; set; }

        public int THANG { get; set; } //tháng điều tra
        public int NAM { get; set; }//năm điều tra
        public int NhomTauID { get; set; }
        public int NhomNgheID { get; set; }

        public List<KT_BAC> lstKT_BAC { get; set; }
        public List<KT_CPUE> lstKT_CPUE { get; set; }
        public List<KT_NGAYHOATDONG> lstKT_NGAYHOATDONG { get; set; }

        public double dKT_BAC
        {
            get
            {
                int _TotalMauDiBien = 0;
                int _TotalMauChon = 0;
                double _valBAC = 0;
                if (lstKT_BAC != null && lstKT_BAC.Count > 0)
                {
                    for (int i = 0; i < lstKT_BAC.Count; i++)
                    {
                        _TotalMauDiBien += lstKT_BAC[i].SO_TAU_CHON_MAU_DI_BIEN.Value;
                        _TotalMauChon += lstKT_BAC[i].SO_TAU_CHON_MAU.Value;
                    }
                    _valBAC = ((double)_TotalMauDiBien / (double)_TotalMauChon);
                }

                return _valBAC;
            }
        } //Hệ số hoạt động của tàu từng nhóm công suất của từng nhóm nghề

        public double dKT_CPUE_TONG_SAN_LUONG_MAU
        {
            get
            {
                double _TotalSanLuong = 0;
                if (lstKT_CPUE != null && lstKT_CPUE.Count > 0)
                {
                    for (int i = 0; i < lstKT_CPUE.Count; i++)
                    {
                        _TotalSanLuong += (double)lstKT_CPUE[i].TONG_SAN_LUONG.Value;
                       
                    }
                   
                }
                return _TotalSanLuong;
            }
        }

        public double dKT_CPUE
        {
            get
            {
                int _TotalSoNgayDanhCa = 0;
                double _TotalSanLuong = 0;
                double _valKT_CPUE = 0;
                if (lstKT_CPUE != null && lstKT_CPUE.Count > 0)
                {
                    for (int i = 0; i < lstKT_CPUE.Count; i++)
                    {
                        _TotalSanLuong += (double)lstKT_CPUE[i].TONG_SAN_LUONG.Value;
                        _TotalSoNgayDanhCa += lstKT_CPUE[i].SO_NGAY_DANH_CA.Value;
                    }
                    _valKT_CPUE = ((double)_TotalSanLuong / (double)_TotalSoNgayDanhCa);
                }

                return _valKT_CPUE;
            }
        } //năng suất khai thác trên 1 đơn vị cường lực của từng nhóm công suất của từng nhóm nghề
        public int iA
        {
            get
            {
                var _SO_NGAY = 0;
                if (lstKT_NGAYHOATDONG != null && lstKT_NGAYHOATDONG.Count > 0)
                {
                    this.lstKT_NGAYHOATDONG.ForEach(n => _SO_NGAY += n.SO_NGAY.Value);
                }

                return _SO_NGAY;
            }
        } //Số ngày khai thác đi biển của tàu /1 tháng của từng nhóm công suất của từng nhóm nghề
        public int iF { get; set; }//Tổng số tàu khai thác của từng nhóm tàu theo từng nghề khai thác

        public double dKT_SANLUONG_FAO
        {
            get
            {
                return dKT_CPUE * iA * dKT_BAC * iF;
            }
        }

        public DNHOM_TAU DNhomTau { get; set; }
        public DM_NHOMNGHE DNhomNghe { get; set; }
    }
}
