
/****** Object:  StoredProcedure [dbo].[MergeIntoDataKT_DANGKIEM]    Script Date: 05/18/2016 15:49:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MergeIntoDataKT_DANGKIEM]
	@tablename varchar(100)
AS
BEGIN
declare @sql varchar(max)='';
                                
--Merge into data temp to KT_TAUTHUYEN
set @sql='
		 INSERT INTO KT_DANGKIEM  ( 
							    [DLOAI_KIEM_TRA_KTID]
							   ,[MA_TINHTP]
							   ,[SO_SO_DANG_KIEM]
							   ,[NGAY_CAP_SO_DANG_KIEM]
							   ,[CO_QUAN_DANG_KIEM]
							   ,[DANG_KIEM_VIEN]
							   ,[SO_BB_KIEM_TRA_KT]
							   ,[NGAY_KIEM_TRA]
							   ,[SO_AN_CHI_ATKT]
							   ,[NGAY_CAP_ATKT_TCA]
							   ,[NGAY_HH_ATKT_TCA]
							   ,[SO_DK]
							   ,[NOI_DANG_KY]
							   ,[KT_NAM_DONG]
							   ,[CHU_PHUONG_TIEN]
							   ,[KT_CHIEU_DAI]
							   ,[KT_CHIEU_RONG]
							   ,[KT_CHIEU_CAO]
							   ,[KT_MON_NUOC]
							   ,[KT_MAN_KHO]
							   ,[KT_CHIEU_DAI_TK]
							   ,[KT_TOC_DO_TAU]
							   ,[DVAT_LIEU_VOID]
							   ,[KT_TONG_CONG_SUAT]
							   ,[KT_SO_MAY_TAU]
							   ,[DNHOM_TAUID]
							   ,[M1_KY_HIEU_MAY]
							   ,[M1_SO_MAY]
							   ,[M1_NOI_SX]
							   ,[M1_NAM_CHE_TAO]
							   ,[M1_HANG_MAY]
							   ,[M1_VONG_QUAY]
							   ,[M1_CONG_SUAT]
							   ,[M2_KY_HIEU_MAY]
							   ,[M2_SO_MAY]
							   ,[M2_NOI_SX]
							   ,[M2_NAM_CHE_TAO]
							   ,[M2_HANG_MAY]
							   ,[M2_VONG_QUAY]
							   ,[M2_CONG_SUAT]
							   ,[DTINH_TRANG_DANG_KIEMID]
							   ,[DCAP_TAU_DANG_KIEMID]
							   ,[HAN_DANG_KIEM]
							   ,[GHI_CHU]
							  )
				SELECT 
					[DLOAI_KIEM_TRA_KTID]
				   ,[MA_TINHTP]
				   ,[SO_SO_DANG_KIEM]
				   ,[NGAY_CAP_SO_DANG_KIEM]
				   ,[CO_QUAN_DANG_KIEM]
				   ,[DANG_KIEM_VIEN]
				   ,[SO_BB_KIEM_TRA_KT]
				   ,[NGAY_KIEM_TRA]
				   ,[SO_AN_CHI_ATKT]
				   ,[NGAY_CAP_ATKT_TCA]
				   ,[NGAY_HH_ATKT_TCA]
				   ,[SO_DK]
				   ,[NOI_DANG_KY]
				   ,[KT_NAM_DONG]
				   ,[CHU_PHUONG_TIEN]
				   ,[KT_CHIEU_DAI]
				   ,[KT_CHIEU_RONG]
				   ,[KT_CHIEU_CAO]
				   ,[KT_MON_NUOC]
				   ,[KT_MAN_KHO]
				   ,[KT_CHIEU_DAI_TK]
				   ,[KT_TOC_DO_TAU]
				   ,[DVAT_LIEU_VOID]
				   ,[KT_TONG_CONG_SUAT]
				   ,[KT_SO_MAY_TAU]
				   ,[DNHOM_TAUID]
				   ,[M1_KY_HIEU_MAY]
				   ,[M1_SO_MAY]
				   ,[M1_NOI_SX]
				   ,[M1_NAM_CHE_TAO]
				   ,[M1_HANG_MAY]
				   ,[M1_VONG_QUAY]
				   ,[M1_CONG_SUAT]
				   ,[M2_KY_HIEU_MAY]
				   ,[M2_SO_MAY]
				   ,[M2_NOI_SX]
				   ,[M2_NAM_CHE_TAO]
				   ,[M2_HANG_MAY]
				   ,[M2_VONG_QUAY]
				   ,[M2_CONG_SUAT]
				   ,[DTINH_TRANG_DANG_KIEMID]
				   ,[DCAP_TAU_DANG_KIEMID]
				   ,[HAN_DANG_KIEM]
				   ,[GHI_CHU]
				FROM '+@tablename ;
   exec(@sql);
   exec('DROP TABLE '+@tablename);
END
