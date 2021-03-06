
/****** Object:  StoredProcedure [dbo].[CreateTableKT_GIAYPHEP_TEMP]    Script Date: 05/18/2016 15:49:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateTableKT_GIAYPHEP_TEMP]
	@tablename varchar(100)
AS
BEGIN

declare @sqlCreateTempTable varchar(max)='';
--Create table temp:
set @sqlCreateTempTable='CREATE TABLE [dbo].['+@tablename+'](
							[SO_DK] [nvarchar](128) NOT NULL,
							[NGAY_DK] [datetime] NULL,
							[MA_TINHTP] [nvarchar](128) NOT NULL,
							[TEN_TAU] [nvarchar](max) NULL,
							[CHU_PHUONG_TIEN] [nvarchar](max) NULL,
							[DIA_CHI] [nvarchar](max) NULL,
							[DIEN_THOAI] [nvarchar](max) NULL,
                            [SO_GP] [nvarchar](max) NOT NULL,
							[NGAY_GP] [datetime] NOT NULL,
							[NGAY_HL_GP] [datetime] NOT NULL,
							[NGAY_HHL_GP] [datetime] NOT NULL,
							[CANG_DANG_KY] [nvarchar](max) NOT NULL,

							[DM_NHOMNGHEID] [varchar](max) NOT NULL,
							[IsNGHECHINH] [int] NOT NULL,
							[DVUNG_TUYENID] [varchar](max) NOT NULL,
							[CAP_PHEP_TU] [datetime]  NULL,
							[CAP_PHEP_DEN] [datetime]  NULL,
							[DM_NHOMNGHE2ID] [varchar](max) NULL,
							[IsNGHECHINH_2] [varchar](max)  NULL,
							[DVUNG_TUYEN2ID] [varchar](max) NULL,
							[CAP_PHEP_TU_2] [datetime] NULL,
							[CAP_PHEP_DEN_2] [datetime] NULL,
							[DM_NHOMNGHE3ID] [varchar](max) NULL,
							[IsNGHECHINH_3] [varchar](max)  NULL,
							[DVUNG_TUYEN3ID] [varchar](max) NULL,
							[CAP_PHEP_TU_3] [datetime] NULL,
							[CAP_PHEP_DEN_3] [datetime] NULL
							
                                     ) ON [PRIMARY]';
exec(@sqlCreateTempTable);                                   

END
