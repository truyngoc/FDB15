USE [FDB15]
GO
/****** Object:  StoredProcedure [dbo].[KT_GIAYPHEP_KHAITHACTHUYSAN]    Script Date: 5/5/2016 4:07:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[KT_GIAYPHEP_KHAITHACTHUYSAN]
	-- Add the parameters for the stored procedure here
	@matinh_tp varchar(10),
	@fromdate datetime,
	@todate datetime

	AS
BEGIN
	
	SET NOCOUNT ON;

  select   
		sum(case  
				when  A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1  then 1 else 0 
				
		       end
		     ) TAU_20_NGHEKEO,
		  sum(case  
				when B.SO_DK is not null AND A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1  then 1 else 0 
				
		       end
		     ) TAU_20_NGHEKEO_CP,
		      sum(case  
				when DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1  then 1 else 0 
				
		       end
		     ) TAU_50_NGHEKEO,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1  then 1 else 0 
				
		       end
		     ) TAU_50_NGHEKEO_CP,
		   sum(case  
				when  DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1  then 1 else 0 
				
		       end
		     ) TAU_90_NGHEKEO,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
				
		       end
		     ) TAU_90_NGHEKEO_CP,
		   sum(case  
				when  DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
				
		       end
		     ) TAU_250_NGHEKEO,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
				
		       end
		     ) TAU_250_NGHEKEO_CP,
		   sum(case  
				when  DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
				
		       end
		     ) TAU_400_NGHEKEO,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
				
		       end
		     ) TAU_400_NGHEKEO_CP,
		  sum(case  
				when  DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHEKEO,
		    sum(case  
				when B.SO_DK is not null  AND DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHEKEO_CP,

			 -- nghe id = 3 (rê)

			 sum(case  
				when  A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3  then 1 else 0 
				
		       end
		     ) TAU_20_NGHERE,
		  sum(case  
				when B.SO_DK is not null AND A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3  then 1 else 0 
				
		       end
		     ) TAU_20_NGHERE_CP,
		      sum(case  
				when DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3  then 1 else 0 
				
		       end
		     ) TAU_50_NGHERE,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3  then 1 else 0 
				
		       end
		     ) TAU_50_NGHERE_CP,
		   sum(case  
				when  DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3  then 1 else 0 
				
		       end
		     ) TAU_90_NGHERE,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
				
		       end
		     ) TAU_90_NGHERE_CP,
		   sum(case  
				when  DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3 then 1 else 0 
				
		       end
		     ) TAU_250_NGHERE,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3 then 1 else 0 
				
		       end
		     ) TAU_250_NGHERE_CP,
		   sum(case  
				when  DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3 then 1 else 0 
				
		       end
		     ) TAU_400_NGHERE,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3 then 1 else 0 
				
		       end
		     ) TAU_400_NGHERE_CP,
		  sum(case  
				when  DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHERE,
		    sum(case  
				when B.SO_DK is not null  AND DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHERE_CP,

			  -- nghe id = 4 (câu)

			 sum(case  
				when  A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4  then 1 else 0 
				
		       end
		     ) TAU_20_NGHECAU,
		  sum(case  
				when B.SO_DK is not null AND A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4  then 1 else 0 
				
		       end
		     ) TAU_20_NGHECAU_CP,
		      sum(case  
				when DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4  then 1 else 0 
				
		       end
		     ) TAU_50_NGHECAU,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4  then 1 else 0 
				
		       end
		     ) TAU_50_NGHECAU_CP,
		   sum(case  
				when  DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4  then 1 else 0 
				
		       end
		     ) TAU_90_NGHECAU,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4 then 1 else 0 
				
		       end
		     ) TAU_90_NGHECAU_CP,
		   sum(case  
				when  DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4 then 1 else 0 
				
		       end
		     ) TAU_250_NGHECAU,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4 then 1 else 0 
				
		       end
		     ) TAU_250_NGHECAU_CP,
		   sum(case  
				when  DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4 then 1 else 0 
				
		       end
		     ) TAU_400_NGHECAU,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4 then 1 else 0 
				
		       end
		     ) TAU_400_NGHECAU_CP,
		  sum(case  
				when  DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHECAU,
		    sum(case  
				when B.SO_DK is not null  AND DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHECAU_CP,

			  -- nghe id = 2 (vây)

			 sum(case  
				when  A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2  then 1 else 0 
				
		       end
		     ) TAU_20_NGHEVAY,
		  sum(case  
				when B.SO_DK is not null AND A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2  then 1 else 0 
				
		       end
		     ) TAU_20_NGHEVAY_CP,
		      sum(case  
				when DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2  then 1 else 0 
				
		       end
		     ) TAU_50_NGHEVAY,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2  then 1 else 0 
				
		       end
		     ) TAU_50_NGHEVAY_CP,
		   sum(case  
				when  DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2  then 1 else 0 
				
		       end
		     ) TAU_90_NGHEVAY,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2 then 1 else 0 
				
		       end
		     ) TAU_90_NGHEVAY_CP,
		   sum(case  
				when  DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2 then 1 else 0 
				
		       end
		     ) TAU_250_NGHEVAY,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2 then 1 else 0 
				
		       end
		     ) TAU_250_NGHEVAY_CP,
		   sum(case  
				when  DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2 then 1 else 0 
				
		       end
		     ) TAU_400_NGHEVAY,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2 then 1 else 0 
				
		       end
		     ) TAU_400_NGHEVAY_CP,
		  sum(case  
				when  DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHEVAY,
		    sum(case  
				when B.SO_DK is not null  AND DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHEVAY_CP,

			  -- nghe id = 5 (khác)

			 sum(case  
				when  A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5  then 1 else 0 
				
		       end
		     ) TAU_20_NGHEKHAC,
		  sum(case  
				when B.SO_DK is not null AND A.DNHOM_TAUID=1 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5  then 1 else 0 
				
		       end
		     ) TAU_20_NGHEKHAC_CP,
		      sum(case  
				when DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5  then 1 else 0 
				
		       end
		     ) TAU_50_NGHEKHAC,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=2 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5  then 1 else 0 
				
		       end
		     ) TAU_50_NGHEKHAC_CP,
		   sum(case  
				when  DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5  then 1 else 0 
				
		       end
		     ) TAU_90_NGHEKHAC,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=3 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5 then 1 else 0 
				
		       end
		     ) TAU_90_NGHEKHAC_CP,
		   sum(case  
				when  DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5 then 1 else 0 
				
		       end
		     ) TAU_250_NGHEKHAC,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=4 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5 then 1 else 0 
				
		       end
		     ) TAU_250_NGHEKHAC_CP,
		   sum(case  
				when  DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5 then 1 else 0 
				
		       end
		     ) TAU_400_NGHEKHAC,
		    sum(case  
				when B.SO_DK is not null AND DNHOM_TAUID=5 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5 then 1 else 0 
				
		       end
		     ) TAU_400_NGHEKHAC_CP,
		  sum(case  
				when  DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHEKHAC,
		    sum(case  
				when B.SO_DK is not null  AND DNHOM_TAUID=6 AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5 then 1 else 0 
				
		       end
		     ) TAU_TREN_400_NGHEKHAC_CP,
			  sum(case  
				when A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5 then 1 else 0 
		       end
		     ) TOTAL_SO_DK_KHAC,
		     sum(case  
				when A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
		       end
		     ) TOTAL_SO_DK_KEO,
			 
			  sum(case  
				when A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2 then 1 else 0 
		       end
		     ) TOTAL_SO_DK_VAY,
			  sum(case  
				when A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3 then 1 else 0 
		       end
		     ) TOTAL_SO_DK_RE,
			  sum(case  
				when A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4 then 1 else 0 
		       end
		     ) TOTAL_SO_DK_CAU,
		     sum(case  
				when B.SO_DK is not null AND A.NGHE_CHINH_ID = 1 OR A.NGHE_PHU_ID = 1 then 1 else 0 
				
		       end
		     ) TOTAL_SO_DK_CP_KEO,

			  sum(case  
				when B.SO_DK is not null AND A.NGHE_CHINH_ID = 2 OR A.NGHE_PHU_ID = 2 then 1 else 0 
				
		       end
		     ) TOTAL_SO_DK_CP_VAY,

			   sum(case  
				when B.SO_DK is not null AND A.NGHE_CHINH_ID = 3 OR A.NGHE_PHU_ID = 3 then 1 else 0 
				
		       end
		     ) TOTAL_SO_DK_CP_RE,

			   sum(case  
				when B.SO_DK is not null AND A.NGHE_CHINH_ID = 4 OR A.NGHE_PHU_ID = 4 then 1 else 0 
				
		       end
		     ) TOTAL_SO_DK_CP_CAU,

			  sum(case  
				when B.SO_DK is not null AND A.NGHE_CHINH_ID = 5 OR A.NGHE_PHU_ID = 5 then 1 else 0 
				
		       end
		     ) TOTAL_SO_DK_CP_KHAC

 from KT_TAUTHUYEN A 
 LEFT JOIN KT_GIAYPHEP B
 ON A.SO_DK = B.SO_DK
	where	(@matinh_tp is null or A.MA_TINHTP = @matinh_tp)
			and
			(@fromdate is null or   B.NGAY_GP >= @fromdate)
			and 
			(@todate is null or B.NGAY_GP <= @todate)


END
