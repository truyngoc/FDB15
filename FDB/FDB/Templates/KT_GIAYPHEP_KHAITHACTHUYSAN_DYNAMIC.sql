USE [FDB15]
GO
/****** Object:  StoredProcedure [dbo].[KT_GIAYPHEP_KHAITHACTHUYSAN_DYNAMIC]    Script Date: 5/6/2016 5:42:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[KT_GIAYPHEP_KHAITHACTHUYSAN_DYNAMIC]
	-- Add the parameters for the stored procedure here
	@matinh_tp varchar(10),
	@fromdate datetime,
	@todate datetime

	AS
BEGIN
	
	SET NOCOUNT ON;

  select   (SELECT TenNhomNghe  FROM DM_NHOMNGHE WHERE DM_NHOMNGHE.DM_NhomNgheID=NGHE_CHINH_ID  ) AS NHOM_NGHE,
		  
		  COUNT(CASE WHEN  B.NGAY_HHL_GP < GETDATE()  AND B.TRANG_THAI IS NULL AND A.DNHOM_TAUID = 1 THEN 1
		    END) TAU_20_GP_HH,
		  
		  COUNT(case  
				when B.SO_DK is not null AND DNHOM_TAUID=1 then 1 
				
		       end
		     ) TAU_20_CP,
		   COUNT(case  
				when  DNHOM_TAUID=1 then 1 
				
		       end
		     ) TAU_20,
			COUNT(CASE WHEN  (B.NGAY_HHL_GP < GETDATE()  AND B.TRANG_THAI IS NULL) AND A.DNHOM_TAUID = 2 THEN 1
		    END) TAU_50_GP_HH,
		  
		    COUNT(case  
				when B.SO_DK is not null AND DNHOM_TAUID=2 then 1
				
		       end
		     ) TAU_50_CP,
		   COUNT(case  
				when DNHOM_TAUID=2 then 1 
				
		       end
		     ) TAU_50,
			  COUNT(CASE WHEN  B.NGAY_HHL_GP < GETDATE()  AND B.TRANG_THAI IS NULL AND A.DNHOM_TAUID = 3 THEN 1
		    END) TAU_90_GP_HH,
		    COUNT(case  
				when B.SO_DK is not null AND DNHOM_TAUID=3 then 1 
				
		       end
		     ) TAU_90_CP,
		   COUNT(case  
				when DNHOM_TAUID=3 then 1 
				
		       end
		     ) TAU_90,
			  COUNT(CASE WHEN  B.NGAY_HHL_GP < GETDATE()  AND B.TRANG_THAI IS NULL AND A.DNHOM_TAUID = 4 THEN 1
		    END) TAU_250_GP_HH,
		    COUNT(case  
				when B.SO_DK is not null AND DNHOM_TAUID=4 then 1 
				
		       end
		     ) TAU_250_CP,
			 
		   COUNT(case  
				when  DNHOM_TAUID=4 then 1 
				
		       end
		     ) TAU_250,
			  COUNT(CASE WHEN  B.NGAY_HHL_GP < GETDATE()  AND B.TRANG_THAI IS NULL AND A.DNHOM_TAUID = 5 THEN 1
		    END) TAU_400_GP_HH,
		    COUNT(case  
				when B.SO_DK is not null AND DNHOM_TAUID=5 then 1 
				
		       end
		     ) TAU_400_CP,
		   COUNT(case  
				when  DNHOM_TAUID=5 then 1 
				
		       end
		     ) TAU_400,
			  COUNT(CASE WHEN  B.NGAY_HHL_GP < GETDATE()  AND B.TRANG_THAI IS NULL AND A.DNHOM_TAUID = 6 THEN 1
		    END) TAU_MORE_400_GP_HH,
		    COUNT(case  
				when B.SO_DK is not null  AND DNHOM_TAUID=6 then 1 
				
		       end
		     ) TAU_MORE_400_CP,
		   COUNT(case  
				when  DNHOM_TAUID=6 then 1 
				
		       end
		     ) TAU_MORE_400,

		  COUNT(A.SO_DK) AS TOTAL_SO_DK,
		  SUM(CASE WHEN A.IsCapPhep = 1 THEN 1 ELSE 0 END)  TOTAL_SO_DK_CP,
		  SUM(CASE WHEN  B.NGAY_HHL_GP < GETDATE()  AND B.TRANG_THAI IS NULL THEN 1 ELSE 0 END) TOTAL_SO_GP_HH

 from KT_TAUTHUYEN A
 LEFT JOIN KT_GIAYPHEP B
 ON A.SO_DK = B.SO_DK
 where	(@matinh_tp is null or A.MA_TINHTP = @matinh_tp)
			and
			(@fromdate is null or   B.NGAY_GP >= @fromdate)
			and 
			(@todate is null or B.NGAY_GP <= @todate)

			 group BY A.NGHE_CHINH_ID 
 


			 
END
