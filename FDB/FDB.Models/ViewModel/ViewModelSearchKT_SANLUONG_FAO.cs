using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDB.Models
{
   public class ViewModelSearchKT_SANLUONG_FAO
    {
       public int? Nam { get; set; }
       public int? Thang { get; set; }
       public string TinhTP { get; set; }
       public int LoaiThongKe { get; set; }

       public KT_View_SANLUONG_FAO KTSanLuongFao { get; set; }
    }

   public class ViewModelSearchKT_SANLUONG_TRUCTIEP
   {
       public int? Nam { get; set; }
       public int? Thang { get; set; }
       public string TinhTP { get; set; }

       public string TinhTP_User { get; set; }
       public int LoaiThongKe { get; set; }

       public Dictionary<string, float> San_Luong_Theo_Nghe { get; set; }
       public Dictionary<string, float> San_Luong_Theo_Nhom_Nghe { get; set; }
       public float San_Luong_Thang { get; set; }

       public Dictionary<string, float> San_Luong_Theo_Loai { get; set; }
       public Dictionary<string, float> San_Luong_Theo_Nhom_Loai { get; set; }
       public float San_Luong_Loai_Thang { get; set; }
   }
}
