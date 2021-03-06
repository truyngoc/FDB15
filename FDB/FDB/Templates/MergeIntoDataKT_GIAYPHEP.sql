
/****** Object:  StoredProcedure [dbo].[MergeIntoDataKT_GIAYPHEP]    Script Date: 05/18/2016 15:50:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MergeIntoDataKT_GIAYPHEP]
	@tablename varchar(100)
AS
BEGIN
declare @sql varchar(max)='';
declare @sqlUpdateIsCapPhep varchar(max)='';   
                            
--Merge into data temp to KT_TAUTHUYEN
set @sql='INSERT INTO [dbo].[KT_GIAYPHEP]
					   ([SO_DK]
						  ,[NGAY_DK]
						  ,[MA_TINHTP]
						  ,[TEN_TAU]
						  ,[CHU_PHUONG_TIEN]
						  ,[DIA_CHI]
						  ,[DIEN_THOAI]
						  ,[SO_GP]
						  ,[NGAY_GP]
						  ,[NGAY_HL_GP]
						  ,[NGAY_HHL_GP]
						  ,[CANG_DANG_KY]
						  ,[DM_NHOMNGHEID]
						  ,[IsNGHECHINH]
						  ,[DVUNG_TUYENID]
						  ,[CAP_PHEP_TU]
						  ,[CAP_PHEP_DEN]
						  ,[DM_NHOMNGHE2ID]
						  ,[IsNGHECHINH_2]
						  ,[DVUNG_TUYEN2ID]
						  ,[CAP_PHEP_TU_2]
						  ,[CAP_PHEP_DEN_2]
						  ,[DM_NHOMNGHE3ID]
						  ,[IsNGHECHINH_3]
						  ,[DVUNG_TUYEN3ID]
						  ,[CAP_PHEP_TU_3]
						  ,[CAP_PHEP_DEN_3]
						  ,[IsNGHECHINH_4]
						  ,[IsNGHECHINH_5]
					  
					   )
				SELECT  T.[SO_DK]
					  ,T.[NGAY_DK]
					  ,T.[MA_TINHTP]
					  ,T.[TEN_TAU]
					  ,T.[CHU_PHUONG_TIEN]
					  ,T.[DIA_CHI]
					  ,T.[DIEN_THOAI]
					  ,T.[SO_GP]
					  ,T.[NGAY_GP]
					  ,T.[NGAY_HL_GP]
					  ,T.[NGAY_HHL_GP]
					  ,T.[CANG_DANG_KY]
					  ,T.[DM_NHOMNGHEID]
					  ,T.[IsNGHECHINH]
					  ,T.[DVUNG_TUYENID]
					  ,T.[CAP_PHEP_TU]
					  ,T.[CAP_PHEP_DEN]
					  ,T.[DM_NHOMNGHE2ID]
					  ,T.[IsNGHECHINH_2]
					  ,T.[DVUNG_TUYEN2ID]
					  ,T.[CAP_PHEP_TU_2]
					  ,T.[CAP_PHEP_DEN_2]
					  ,T.[DM_NHOMNGHE3ID]
					  ,T.[IsNGHECHINH_3]
					  ,T.[DVUNG_TUYEN3ID]
					  ,T.[CAP_PHEP_TU_3]
					  ,T.[CAP_PHEP_DEN_3]
					  ,0
					  ,0
					   
			   FROM ' +@tablename +' as T INNER JOIN KT_TAUTHUYEN TT 
										   ON T.SO_DK=TT.SO_DK
										  LEFT JOIN KT_GIAYPHEP GP
										   ON T.SO_GP=GP.SO_GP
			   WHERE GP.SO_GP IS NULL ';
   exec(@sql);
   
   --Update IsCapPhep bang KT_TAUTHUYEN
   SET @sqlUpdateIsCapPhep='UPDATE TT 
							SET TT.IsCapPhep = 1 
							FROM KT_TAUTHUYEN AS TT
							INNER JOIN KT_GIAYPHEP AS GP 
							 ON TT.SO_DK = GP.SO_DK
							INNER JOIN '+@tablename+' AS T 
							 ON TT.SO_DK=T.SO_DK
							 ';
   exec(@sqlUpdateIsCapPhep);
   
   ---Get SO_DK không có trong table KT_TAUTHUYEN
	declare @sqlGetSoDKNotExist varchar(max)='';
	set @sqlGetSoDKNotExist=' SELECT   T.*
									  
							   FROM ' +@tablename +' as T LEFT JOIN  KT_TAUTHUYEN TT 
														   ON T.SO_DK=TT.SO_DK
														
							   WHERE TT.SO_DK IS NULL ';
	EXEC(@sqlGetSoDKNotExist);
   
   --Drop temp table
  exec('DROP TABLE '+@tablename);
END
