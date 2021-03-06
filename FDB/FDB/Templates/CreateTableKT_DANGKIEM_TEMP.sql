
/****** Object:  StoredProcedure [dbo].[CreateTableKT_DANGKIEM_TEMP]    Script Date: 05/18/2016 15:49:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateTableKT_DANGKIEM_TEMP]
	@tablename varchar(100)
AS
BEGIN

declare @sqlCreateTempTable varchar(max)='';
--Create table temp:
set @sqlCreateTempTable='CREATE TABLE [dbo].['+@tablename+'](
							[DLOAI_KIEM_TRA_KTID] [int] NOT NULL,
                            [MA_TINHTP] [nvarchar](128) NOT NULL,
							[SO_SO_DANG_KIEM] [nvarchar](max) NOT NULL,
							[NGAY_CAP_SO_DANG_KIEM] [datetime] NOT NULL,
							[CO_QUAN_DANG_KIEM] [nvarchar](max) NULL,
							[DANG_KIEM_VIEN] [nvarchar](max) NULL,
							[SO_BB_KIEM_TRA_KT] [nvarchar](max) NULL,
							[NGAY_KIEM_TRA] [datetime] NOT NULL,
							[SO_AN_CHI_ATKT] [nvarchar](max) NULL,
							[NGAY_CAP_ATKT_TCA] [datetime] NULL,
							[NGAY_HH_ATKT_TCA] [datetime] NULL,
							[SO_DK] [nvarchar](max) NULL,
							[NOI_DANG_KY] [nvarchar](max) NULL,
							[KT_NAM_DONG] [int] NULL,
							[CHU_PHUONG_TIEN] [nvarchar](max) NULL,
							[KT_CHIEU_DAI] [decimal](18, 2) NULL,
							[KT_CHIEU_RONG] [decimal](18, 2) NULL,
							[KT_CHIEU_CAO] [decimal](18, 2) NULL,
							[KT_MON_NUOC] [decimal](18, 2) NULL,
							[KT_MAN_KHO] [decimal](18, 2) NULL,
							[KT_CHIEU_DAI_TK] [decimal](18, 2) NULL,
							[KT_TOC_DO_TAU] [int] NULL,
							[DVAT_LIEU_VOID] [int] NULL,
							[KT_TONG_CONG_SUAT] [int] NOT NULL,
							[KT_SO_MAY_TAU] [int] NOT NULL,
							[DNHOM_TAUID] [int] NOT NULL,
							[M1_KY_HIEU_MAY] [nvarchar](max) NULL,
							[M1_SO_MAY] [nvarchar](max) NULL,
							[M1_NOI_SX] [nvarchar](max) NULL,
							[M1_NAM_CHE_TAO] [int] NULL,
							[M1_HANG_MAY] [nvarchar](max) NULL,
							[M1_VONG_QUAY] [int] NULL,
							[M1_CONG_SUAT] [int] NULL,
							[M2_KY_HIEU_MAY] [nvarchar](max) NULL,
							[M2_SO_MAY] [nvarchar](max) NULL,
							[M2_NOI_SX] [nvarchar](max) NULL,
							[M2_NAM_CHE_TAO] [int] NULL,
							[M2_HANG_MAY] [nvarchar](max) NULL,
							[M2_VONG_QUAY] [int] NULL,
							[M2_CONG_SUAT] [int] NULL,
							
							[DTINH_TRANG_DANG_KIEMID] [int] NOT NULL,
							[DCAP_TAU_DANG_KIEMID] [int] NOT NULL,
							[HAN_DANG_KIEM] [datetime] NOT NULL,
							[GHI_CHU] [nvarchar](max) NULL
							

                                     ) ON [PRIMARY]';
exec(@sqlCreateTempTable);                                   

END
