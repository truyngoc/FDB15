using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using FDB.Models;

namespace FDB.DataAccessLayer
{
    public class FDBContext : DbContext
    {
        public FDBContext() :
            base("FDBContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<DCONG_DUNG_TAU> DCONG_DUNG_TAU { get; set; }
        public DbSet<DNHOM_TAU> DNHOM_TAU { get; set; }  
        public DbSet<DTINH_TRANG_DANG_KIEM> DTINH_TRANG_DANG_KIEM { get; set; }
        public DbSet<DTINH_TRANG_TAU> DTINH_TRANG_TAU { get; set; }
        public DbSet<DLOAI_KIEM_TRA_KT> DLOAI_KIEM_TRA_KT { get; set; }
        public DbSet<DVAT_LIEU_VO> DVAT_LIEU_VO { get; set; }
        public DbSet<DTINHTP> DTINHTP { get; set; }
        public DbSet<DQUANHUYEN> DQUANHUYEN { get; set; }
        public DbSet<DPHUONGXA> DPHUONGXA { get; set; }
        public DbSet<DVUNG_TUYEN> DVUNG_TUYEN { get; set; }               
        public DbSet<DCAP_TAU_DANG_KIEM> DCAP_TAU_DANG_KIEM { get; set; }
        public DbSet<DM_NHOMNGHE> DM_NHOMNGHE { get; set; }
        public DbSet<DM_NHOMDOITUONG_KT> DM_NHOMDOITUONG_KT { get; set; }

        // khai thac
        // cang ca
        public DbSet<KT_CANGCA> KT_CANGCA { get; set; }
        public DbSet<DM_CANGCA> DM_CANGCA { get; set; }
        public DbSet<DPHAN_LOAI_CANG> DPHAN_LOAI_CANG { get; set; }

        // khu bao ton
        public DbSet<KT_KHUBAOTON> KT_KHUBAOTON { get; set; }
        public DbSet<DKHU_BAO_TON> DKHU_BAO_TON { get; set; }

        // khu bao ve nguon loi
        public DbSet<KT_KHUBAOVENGUONLOI> KT_KHUBAOVENGUONLOI { get; set; }

        // khu neo dau
        public DbSet<KT_KHUNEODAU> KT_KHUNEODAU { get; set; }
        public DbSet<DPHAN_LOAI_KHUNEODAU> DPHAN_LOAI_KHUNEODAU { get; set; }

        // co so dong sua tau thuyen
        public DbSet<KT_DONGSUA_TAUTHUYEN> KT_DONGSUA_TAUTHUYEN { get; set; }
        public DbSet<KT_DONGSUA_TAUTHUYEN_DETAIL> KT_DONGSUA_TAUTHUYEN_DETAIL { get; set; }

        // thiet hai khai thac
        public DbSet<KT_THIETHAIKHAITHAC> KT_THIETHAIKHAITHAC { get; set; }
        public DbSet<DSUCOVETAU> DSUCOVETAU { get; set; }
        public DbSet<DSUCOVENGUOI> DSUCOVENGUOI { get; set; }

        // dang ky tau thuyen
        public DbSet<KT_TAUTHUYEN> KT_TAUTHUYEN { get; set; }

        // giay phep khai thac
        public DbSet<KT_GIAYPHEP> KT_GIAYPHEP { get; set; }
        public DbSet<KT_GIAYPHEP_NK> KT_GIAYPHEP_NK { get; set; }

        // dang kiem
        public DbSet<KT_DANGKIEM> KT_DANGKIEM { get; set; }

        // san luong FAO
        public DbSet<KT_NGAYHOATDONG> KT_NGAYHOATDONG { get; set; }
        public DbSet<KT_BAC> KT_BAC { get; set; }
        public DbSet<KT_CPUE> KT_CPUE { get; set; }

        // san luong truc tiep
        public DbSet<KT_SANLUONG> KT_SANLUONG { get; set; }
        public DbSet<KT_SANLUONG_DETAIL> KT_SANLUONG_DETAIL { get; set; }
       

      


        // nuoi trong 

        // thong tin gia thuy san
        public DbSet<NT_TT_THITRUONG> NT_TT_THITRUONG { get; set; }
        public DbSet<DM_DOITUONG_GIA_THITRUONG> DM_DOITUONG_GIA_THITRUONG { get; set; }

        // thiet hai nuoi trong
        public DbSet<NT_THIETHAI> NT_THIETHAI { get; set; }
        public DbSet<DM_DOITUONG_NUOI_THIETHAI> DM_DOITUONG_NUOI_THIETHAI { get; set; }
        public DbSet<DTYLE_THIETHAI> DTYLE_THIETHAI { get; set; }
        public DbSet<DNGUYENNHAN_THIETHAI> DNGUYENNHAN_THIETHAI { get; set; }

        // nuoi nuoc ngot
        public DbSet<NT_NUOC_NGOT> NT_NUOC_NGOT { get; set; }
        public DbSet<NT_NUOC_NGOT_DETAIL> NT_NUOC_NGOT_DETAIL { get; set; }
        public DbSet<DM_DOITUONG_NUOI_NGOT> DM_DOITUONG_NUOI_NGOT { get; set; }
        public DbSet<DM_HINHTHUC_NUOI> DM_HINHTHUC_NUOI { get; set; }


        // nuoi man, lo
        public DbSet<NT_NUOC_MANLO> NT_NUOC_MANLO { get; set; }
        public DbSet<NT_NUOC_MANLO_DETAIL> NT_NUOC_MANLO_DETAIL { get; set; }
        public DbSet<DM_DOITUONG_NUOI_MANLO> DM_DOITUONG_NUOI_MANLO { get; set; }

        // san xuat giong
        public DbSet<NT_SANXUATGIONG> NT_SANXUATGIONG { get; set; }
        public DbSet<NT_SANXUATGIONG_DETAIL> NT_SANXUATGIONG_DETAIL { get; set; }
        public DbSet<DM_DOITUONG_NUOI_SANXUATGIONG> DM_DOITUONG_NUOI_SANXUATGIONG { get; set; }


        //THANHNC5:22/03/2016
        // public DbSet<KT_IUU_GIAYXN_NGUYENLIEU> KT_IUU_GIAYXN_NGUYENLIEU { get; set; }
        //public DbSet<KT_IUU_GIAYXN_NGUYENLIEU_DETAIL> KT_IUU_GIAYXN_NGUYENLIEU_DETAIL { get; set; }
        //public DbSet<KT_IUU_GIAYCHUNGNHAN> KT_IUU_GIAYCHUNGNHAN { get; set; }
        //public DbSet<KT_IUU_GIAYCHUNGNHAN_DETAIL> KT_IUU_GIAYCHUNGNHAN_DETAIL { get; set; }

        //public DbSet<DM_DOITUONG_KT> DM_DOITUONG_KT { get; set; }
        //public DbSet<DM_DOITUONG_KT_IUU> DM_DOITUONG_KT_IUU { get; set; }

        //public DbSet<DM_NGHE> DM_NGHE { get; set; }


        //public DbSet<DM_NHOMDOITUONG_NUOI> DM_NHOMDOITUONG_NUOI { get; set; }
        //public DbSet<DM_DOITUONG_NUOI> DM_DOITUONG_NUOI { get; set; }
        //public DbSet<DM_LOAI_MATNUOC_NGOTMANLO> DM_LOAI_MATNUOC_NGOTMANLO { get; set; }
        //public DbSet<DM_LOAI_MATNUOC_NUOI_LONGBE> DM_LOAI_MATNUOC_NUOI_LONGBE { get; set; }
        //public DbSet<DM_LOAI_MATNUOC_NUOI_SANXUATGIONG> DM_LOAI_MATNUOC_NUOI_SANXUATGIONG { get; set; }
        //public DbSet<DM_LOAI_MATNUOC_KT> DM_LOAI_MATNUOC_KT { get; set; }
        //public DbSet<DM_DOITUONG_NUOI_LONGBE> DM_DOITUONG_NUOI_LONGBE { get; set; }
        //public DbSet<DM_NHOMDOITUONG_NUOI_LONGBE> DM_NHOMDOITUONG_NUOI_LONGBE { get; set; }
        //public DbSet<DM_NHOMDOITUONG_NUOI_SANXUATGIONG> DM_NHOMDOITUONG_NUOI_SANXUATGIONG { get; set; }
        //public DbSet<NT_NUOCNGOT_MANNO> NT_NUOCNGOT_MANNO { get; set; }
        //public DbSet<NT_NUOCNGOT_MANNO_DETAIL> NT_NUOCNGOT_MANNO_DETAIL { get; set; }
        //public DbSet<NT_NUOILONGBE> NT_NUOILONGBE { get; set; }
        //public DbSet<NT_NUOILONGBE_DETAIL> NT_NUOILONGBE_DETAIL { get; set; }    
        //public DbSet<NT_YEUTOVATNUOIDAUVAO> NT_YEUTOVATNUOIDAUVAO { get; set; }
        //public DbSet<NT_YEUTOVATNUOIDAUVAO_DETAIL> NT_YEUTOVATNUOIDAUVAO_DETAIL { get; set; }
        //public DbSet<NT_COSO_HATANG> NT_COSO_HATANG { get; set; }
        //public DbSet<NT_COSO_HATANG_DETAIL> NT_COSO_HATANG_DETAIL { get; set; }
    }

  
}