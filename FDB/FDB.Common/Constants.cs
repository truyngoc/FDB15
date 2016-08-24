using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FDB.Common
{
    public class Constants
    {
        public const string NOTIFY_ADD_SUCCESS = "Thêm mới {0} thành công";
        public const string NOTIFY_UPDATE_SUCCESS = "Cập nhật {0} thành công";
        public const string NOTIFY_DELETE_SUCCESS = "Xóa {0} thành công";

        public const int PageSize = 25;
    }


    public class MessageError
    {
        public const string SDONVI_COL_MA_DV_REQUIRED = "Mã đơn vị không được để trống";
        public const string SDONVI_COL_MA_DV_STRING_LENGTH = "Mã đơn vị không vượt quá 50 ký tự";
        public const string SDONVI_COL_TEN_DV_REQUIRED = "Tên đơn vị không được để trống";
        public const string SDONVI_COL_CAP_REQUIRED = "Cấp đơn vị không được để trống";
        public const string SDONVI_COL_MA_THONG_KE_STRING_LENGTH = "Mã thống kê không được vượt quá 50 ký tự";
    }

    public class CategoryCommon
    {
        public static SelectList CONG_DUNG_TAU_CAs = new SelectList(
            new List<SelectListItem> {
                new SelectListItem {Text = "Hậu cần dịch vụ", Value = "1"},
                new SelectListItem {Text = "Khai thác thủy sản", Value = "2"}
        }, "Value", "Text");
            

        public static SelectList NHOM_TAUs = new SelectList(
            new List<SelectListItem> {
                new SelectListItem {Text = "Không lắp máy", Value = "1"},
                new SelectListItem {Text = "Ne < 20 cv", Value = "2"},
                new SelectListItem {Text = "Ne từ 20 -<50cv", Value = "3"},
                new SelectListItem {Text = "Ne từ 50 - 90 cv", Value = "4"},
                new SelectListItem {Text = "Ne > 90 cv", Value = "5"}
        }, "Value", "Text");

        public static SelectList TINH_TRANG_TAUs = new SelectList(
            new List<SelectListItem> {
                new SelectListItem {Text = "Đóng mới", Value = "1"},
                new SelectListItem {Text = "Mua lại", Value = "2"},
                new SelectListItem {Text = "Giải bản", Value = "3"},
                new SelectListItem {Text = "Bán", Value = "4"}
        }, "Value", "Text");


        public static SelectList LOAI_KIEM_TRA_KY_THUATs = new SelectList(
           new List<SelectListItem> {
                new SelectListItem {Text = "Lần đầu", Value = "1"},
                new SelectListItem {Text = "Hàng năm", Value = "2"}
        }, "Value", "Text");


        public static SelectList DANG_KIEMs = new SelectList(
            new List<SelectListItem> {
                new SelectListItem {Text = "Chưa đăng kiểm", Value = "1"},
                new SelectListItem {Text = "Đăng kiểm lần đầu", Value = "2"},
                new SelectListItem {Text = "Đăng kiểm lại", Value = "3"}
        }, "Value", "Text");

        public static SelectList TINH_TRANG_DANG_KIEMs = new SelectList(
            new List<SelectListItem> {
                new SelectListItem {Text = "Thỏa mãn", Value = "1"},
                new SelectListItem {Text = "Hạn chế", Value = "2"},
                new SelectListItem {Text = "Cấm hoạt động", Value = "3"}
        }, "Value", "Text");

        public static SelectList VAT_LIEU_VOs = new SelectList(
            new List<SelectListItem> {
                new SelectListItem {Text = "Gỗ", Value = "1"},
                new SelectListItem {Text = "Thép", Value = "2"},
                new SelectListItem {Text = "Composite", Value = "3"}
        }, "Value", "Text");


        public static SelectList PHAN_LOAI_CANG_CAs = new SelectList(
            new List<SelectListItem> {
                new SelectListItem {Text = "Loại I", Value = "1"},
                new SelectListItem {Text = "Loại II", Value = "2"},
                new SelectListItem {Text = "Loại IA", Value = "3"}
        }, "Value", "Text");

        public static SelectList DEFAULT_VALUE_DDL = new SelectList(
            new List<SelectListItem> (), "Value", "Text");

        public static List<DM_MOHINH_NUOI> DM_MOHINH_NUOIs = new List<DM_MOHINH_NUOI>(
                new List<DM_MOHINH_NUOI> { 
                    new DM_MOHINH_NUOI{TEN="Nuôi/trồng thâm canh & siêu thâm canh",ID=1},
                    new DM_MOHINH_NUOI{TEN="Nuôi/trồng quảng canh cải tiến",ID=2}
                    //new DM_MOHINH_NUOI{TEN="Quảng canh",ID=3},
                    //new DM_MOHINH_NUOI{TEN="Quảng canh cải tiến",ID=4},
                    //new DM_MOHINH_NUOI{TEN="Đa dạng sinh học",ID=5},
                    //new DM_MOHINH_NUOI{TEN="Khác",ID=6}
                }
            );
        //thanhnc5:add ngay 12/01/2016
        public static List<SUCOVETAU> SUCOVETAU = new List<SUCOVETAU>(
              new List<SUCOVETAU> { 
                    new SUCOVETAU{TEN="Mất liên lạc",ID=1},
                    new SUCOVETAU{TEN="Hỏng máy trôi dạt",ID=2},
                    new SUCOVETAU{TEN="Đâm va",ID=3},
                    new SUCOVETAU{TEN="Chìm đắm",ID=4},
                    new SUCOVETAU{TEN="Cháy nổ",ID=5}
                }
          );

        public static List<SUCOVENGUOI> SUCOVENGUOI = new List<SUCOVENGUOI>(
            new List<SUCOVENGUOI> { 
                    new SUCOVENGUOI{TEN="Bị chết",ID=1},
                    new SUCOVENGUOI{TEN="Mất tích",ID=2},
                    new SUCOVENGUOI{TEN="Rơi xuống biển",ID=3},
                    new SUCOVENGUOI{TEN="Ốm/tai nạn lao động",ID=4}
                }
        );

        public static List<DM_DOITUONG_KT_CPUE> lstDMDoiTuongCPUE = new List<DM_DOITUONG_KT_CPUE>
        {
           
            new DM_DOITUONG_KT_CPUE(){ID=1,NAME="Tôm"},
            new DM_DOITUONG_KT_CPUE(){ID=2,NAME="Cá chọn"},
            new DM_DOITUONG_KT_CPUE(){ID=3,NAME="Cá xô"},
            new DM_DOITUONG_KT_CPUE(){ID=4,NAME="Cá tạp"},
            new DM_DOITUONG_KT_CPUE(){ID=5,NAME="Mực ống"},
            new DM_DOITUONG_KT_CPUE(){ID=6,NAME="Mực nang"},
            new DM_DOITUONG_KT_CPUE(){ID=7,NAME="Cá ngừ đại dương"},
            new DM_DOITUONG_KT_CPUE(){ID=8,NAME="Khác"},
        };
    }

    public class DM_DOITUONG_KT_CPUE
    {
        public int ID { get; set; }
        public string NAME { get; set; }
    }


    public class DM_MOHINH_NUOI
    {
        public int ID { get; set; }
        public string TEN { get; set; }
    }

    public class SUCOVETAU
    {
        public int ID { get; set; }
        public string TEN { get; set; }
    }

    public class SUCOVENGUOI
    {
        public int ID { get; set; }
        public string TEN { get; set; }
    }
}
