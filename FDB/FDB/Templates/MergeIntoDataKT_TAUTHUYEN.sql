
GO
/****** Object:  StoredProcedure [dbo].[MergeIntoDataKT_TAUTHUYEN]    Script Date: 02/29/2016 10:09:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MergeIntoDataKT_TAUTHUYEN]
	@tablename varchar(100)
AS
BEGIN
declare @sql varchar(max)='';
                                
--Merge into data temp to KT_TAUTHUYEN
set @sql='
	MERGE KT_TAUTHUYEN bi
		USING '+@tablename+' bo
		ON (bi.SO_DK = bo.SO_DK)
		WHEN NOT MATCHED  THEN
		 INSERT   ( [SO_DK]
					  ,[NGAY_DK]
					  ,[DNHOM_TAUID]
					  ,[DCONG_DUNG_TAUID]
					  ,[DTINH_TRANG_TAUID]
					  ,[TEN_TAU]
					  ,[NGHE_CHINH_ID]
					  ,[NGHE_PHU_ID]
					  ,[SO_THUYEN_VIEN]
					  ,[CHU_PHUONG_TIEN]
					  ,[SO_CMND]
					  ,[DIEN_THOAI]
					  ,[MA_TINHTP]
					  ,[MA_QUANHUYEN]
					  ,[MA_PHUONGXA]
					  ,[DIA_CHI]
					  ,[SO_SO_DANG_KIEM]
					  ,[NGAY_CAP_SO_DANG_KIEM]
					  ,[DTINH_TRANG_DANG_KIEMID]
					  ,[DCAP_TAU_DANG_KIEMID]
					  ,[CO_QUAN_DANG_KIEM]
					  ,[HAN_DANG_KIEM]
					  ,[DANG_KIEM_VIEN]
					  ,[SO_AN_CHI_ATKT]
					  ,[KT_CHIEU_DAI]
					  ,[KT_CHIEU_RONG]
					  ,[KT_CHIEU_CAO]
					  ,[KT_MON_NUOC]
					  ,[KT_MAN_KHO]
					  ,[KT_TAN_DANG_KY]
					  ,[DVAT_LIEU_VOID]
					  ,[KT_TOC_DO_TAU]
					  ,[KT_NOI_DONG]
					  ,[KT_NAM_DONG]
					  ,[KT_SO_MAY_TAU]
					  ,[KT_TONG_CONG_SUAT]
					  ,[M1_KY_HIEU_MAY]
					  ,[M1_SO_MAY]
					  ,[M1_NOI_SX]
					  ,[M1_NAM_CHE_TAO]
					  ,[M1_HANG_MAY]
					  ,[M1_VONG_QUAY]
					  ,[M1_CONG_SUAT]
				 )
		  VALUES  ( bo.SO_DK
					  ,bo.NGAY_DK
					  ,bo.DNHOM_TAUID
					  ,bo.DCONG_DUNG_TAUID
					  ,bo.DTINH_TRANG_TAUID
					  ,bo.TEN_TAU
					  ,bo.NGHE_CHINH_ID
					  ,bo.NGHE_PHU_ID
					  ,bo.SO_THUYEN_VIEN
					  ,bo.CHU_PHUONG_TIEN
					  ,bo.SO_CMND
					  ,bo.DIEN_THOAI
					  ,bo.MA_TINHTP
					  ,bo.MA_QUANHUYEN
					  ,bo.MA_PHUONGXA
					  ,bo.DIA_CHI
					  ,bo.SO_SO_DANG_KIEM
					  ,bo.NGAY_CAP_SO_DANG_KIEM
					  ,bo.DTINH_TRANG_DANG_KIEMID
					  ,bo.DCAP_TAU_DANG_KIEMID
					  ,bo.CO_QUAN_DANG_KIEM
					  ,bo.HAN_DANG_KIEM
					  ,bo.DANG_KIEM_VIEN
					  ,bo.SO_AN_CHI_ATKT
					  ,bo.KT_CHIEU_DAI
					  ,bo.KT_CHIEU_RONG
					  ,bo.KT_CHIEU_CAO
					  ,bo.KT_MON_NUOC
					  ,bo.KT_MAN_KHO
					  ,bo.KT_TAN_DANG_KY
					  ,bo.DVAT_LIEU_VOID
					  ,bo.KT_TOC_DO_TAU
					  ,bo.KT_NOI_DONG
					  ,bo.KT_NAM_DONG
					  ,bo.KT_SO_MAY_TAU
					  ,bo.KT_TONG_CONG_SUAT
					  ,bo.M1_KY_HIEU_MAY
					  ,bo.M1_SO_MAY
					  ,bo.M1_NOI_SX
					  ,bo.M1_NAM_CHE_TAO
					  ,bo.M1_HANG_MAY
					  ,bo.M1_VONG_QUAY
					  ,bo.M1_CONG_SUAT
				  );';
   exec(@sql);
   exec('DROP TABLE '+@tablename);
END
