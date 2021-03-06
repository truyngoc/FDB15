
/****** Object:  StoredProcedure [dbo].[NT_SANXUATGIONG_SEARCH]    Script Date: 05/11/2016 16:04:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[NT_SANXUATGIONG_SEARCH]
	  @DoiTuongSX int =null
	 ,@Thang int =null
	 ,@Nam int =null
	 ,@TThanhPho varchar(50) =null
AS
BEGIN
	
			SELECT
				 C.TEN_DOI_TUONG DOI_TUONG_SX 
				,SUM(B.SAN_LUONG_GIONG) SAN_LUONG_GIONG
				,SUM(B.TONG_COSO_SX_GIONG) TONG_COSO_SX_GIONG
				,SUM(B.TONG_COSO_LOAI_A) TONG_COSO_LOAI_A
				,SUM(B.TONG_COSO_LOAI_B) TONG_COSO_LOAI_B
				,SUM(B.TONG_COSO_LOAI_C) TONG_COSO_LOAI_C
				,SUM(B.TONG_TRAI_GIONG) TONG_TRAI_GIONG
				,SUM(B.TONG_GIONG_KIEMDICH) TONG_GIONG_KIEMDICH
				,SUM(B.TONG_BOME_SX) TONG_BOME_SX
				,SUM(B.TONG_BOME_NHAP) TONG_BOME_NHAP
			FROM NT_SANXUATGIONG A INNER JOIN NT_SANXUATGIONG_DETAIL B
									ON A.ID=B.ID_SANXUATGIONG
								 INNER JOIN DM_DOITUONG_NUOI_SANXUATGIONG C
									ON B.ID_NUOI_SX_DOITUONG =C.ID
			WHERE	(@DoiTuongSX is null or B.ID_NUOI_SX_DOITUONG=@DoiTuongSX)
					AND (@Thang is null or A.THANG=@Thang)
					AND (@Nam is null or A.NAM=@Nam)
					AND (@TThanhPho is null or A.MA_TINHTP=@TThanhPho)
			GROUP BY B.ID_NUOI_SX_DOITUONG,C.TEN_DOI_TUONG
			
		
	
END
