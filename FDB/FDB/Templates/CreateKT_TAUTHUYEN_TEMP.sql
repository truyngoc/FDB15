
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
create PROCEDURE [dbo].[CreateTableKT_TAUTHUYEN_TEMP]
	@tablename varchar(100)
AS
BEGIN

declare @sqlCreateTempTable varchar(max)='';
--Create table temp:
set @sqlCreateTempTable='CREATE TABLE [dbo].['+@tablename+'](
                                    [SO_DK] [nvarchar](128) NOT NULL,
									[NGAY_DK] [datetime] NOT NULL,
									[DNHOM_TAUID] [int] NOT NULL,
									[DCONG_DUNG_TAUID] [int] NOT NULL,
									[DTINH_TRANG_TAUID] [int] NOT NULL,
									[TEN_TAU] [nvarchar](max) NULL,
									[NGHE_CHINH_ID] [int] NOT NULL,
									[NGHE_PHU_ID] [int] NULL,
									[SO_THUYEN_VIEN] [int] NULL,
									[CHU_PHUONG_TIEN] [nvarchar](max) NOT NULL,
									
									[SO_CMND] [nvarchar](max) NULL,
									[DIEN_THOAI] [nvarchar](max) NULL,
									[MA_TINHTP] [nvarchar](128) NOT NULL,
									[MA_QUANHUYEN] [nvarchar](128) NOT NULL,
									[MA_PHUONGXA] [nvarchar](128) NOT NULL,
									[DIA_CHI] [nvarchar](max) NOT NULL,
									
									[SO_SO_DANG_KIEM] [nvarchar](max) NULL,
									[NGAY_CAP_SO_DANG_KIEM] [datetime] NULL,
									
									[DTINH_TRANG_DANG_KIEMID] [int] NULL,
									
									[DCAP_TAU_DANG_KIEMID] [int] NULL,
									
									[CO_QUAN_DANG_KIEM] [nvarchar](max) NULL,
									[HAN_DANG_KIEM] [datetime] NULL,
									[DANG_KIEM_VIEN] [nvarchar](max) NULL,
									
									[SO_AN_CHI_ATKT] [nvarchar](max) NULL,
									
									[KT_CHIEU_DAI] [decimal](18, 2) NOT NULL,
									[KT_CHIEU_RONG] [decimal](18, 2) NOT NULL,
									[KT_CHIEU_CAO] [decimal](18, 2) NOT NULL,
									[KT_MON_NUOC] [decimal](18, 2) NOT NULL,
									[KT_MAN_KHO] [decimal](18, 2) NOT NULL,
									
									[KT_TAN_DANG_KY] [int] NULL,
									[DVAT_LIEU_VOID] [int] NOT NULL,
									[KT_TOC_DO_TAU] [int] NULL,
									[KT_NOI_DONG] [nvarchar](max) NULL,
									[KT_NAM_DONG] [int] NULL,
									[KT_SO_MAY_TAU] [int] NOT NULL,
									
									[KT_TONG_CONG_SUAT] [int] NOT NULL,
									[M1_KY_HIEU_MAY] [nvarchar](max) NULL,
									[M1_SO_MAY] [nvarchar](max) NULL,
									[M1_NOI_SX] [nvarchar](max) NULL,
									[M1_NAM_CHE_TAO] [int] NULL,
									[M1_HANG_MAY] [nvarchar](max) NULL,
									[M1_VONG_QUAY] [int] NULL,
									[M1_CONG_SUAT] [int] NULL,
									
									
                                     CONSTRAINT [PK_dbo.'+@tablename+'] PRIMARY KEY CLUSTERED 
                                    (
                                        [SO_DK] ASC
                                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                                    ) ON [PRIMARY]';
exec(@sqlCreateTempTable);                                   

END
