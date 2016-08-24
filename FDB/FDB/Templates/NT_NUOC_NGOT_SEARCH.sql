
/****** Object:  StoredProcedure [dbo].[NT_NUOC_NGOT_SEARCH]    Script Date: 05/11/2016 16:04:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[NT_NUOC_NGOT_SEARCH]
	  @LoaiBaoCao int 
	 ,@HinhThucNuoi int
	 ,@Thang int =null
	 ,@Nam int =nul
	 ,@TuNgay datetime =null
	 ,@DenNgay datetime =null
	 ,@TThanhPho varchar(50) =null
AS
BEGIN
	IF (@HinhThucNuoi=1) 
		BEGIN
			SELECT
				 C.TEN_DOI_TUONG DOI_TUONG_NUOI 
				,SUM(B.THE_TICH_LONG_LONGBE) THE_TICH_LONG
				,SUM(B.SAN_LUONG_LONGBE) SAN_LUONG_LONG
				,SUM(B.SO_LUONG_LONG_LONGBE) SO_LUONG_LONG
				,SUM(ISNULL(B.SO_LUONG_GIONG_THA_LONGBE,0)) MAT_DO_THA
			FROM NT_NUOC_NGOT A INNER JOIN NT_NUOC_NGOT_DETAIL B
									ON A.ID=B.ID_NUOC_NGOT
								 INNER JOIN DM_DOITUONG_NUOI_NGOT C
									ON B.ID_DOITUONG_NUOI_NGOT =C.ID
			WHERE		A.LOAI_BAO_CAO=@LoaiBaoCao 
					AND B.ID_HINHTHUC_NUOI=@HinhThucNuoi
					AND (@Thang is null or A.THANG=@Thang)
					AND (@Nam is null or A.NAM=@Nam)
					AND (@TuNgay is null or A.NGAY_BAO_CAO_TU>=@TuNgay)
					AND (@DenNgay is null or A.NGAY_BAO_CAO_DEN<=@DenNgay)
					AND (@TThanhPho is null or A.MA_TINHTP=@TThanhPho)
			GROUP BY B.ID_DOITUONG_NUOI_NGOT,C.TEN_DOI_TUONG
			
		END
	ELSE
		BEGIN
			SELECT
				 C.TEN_DOI_TUONG DOI_TUONG_NUOI 
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=1 THEN B.DIEN_TICH_THANUOI_TRONGKY_KHAC  
					      ELSE 0
					 END
					) DIEN_TICH_THANUOI_TRONGKY_KHAC_1
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=1 THEN B.NUOI_TRONG_QUY_HOACH_KHAC  
					      ELSE 0
					 END
					) NUOI_TRONG_QUY_HOACH_KHAC_1
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=1 THEN B.NUOI_NGOAI_QUY_HOACH_KHAC  
					      ELSE 0
					 END
					) NUOI_NGOAI_QUY_HOACH_KHAC_1
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=1 THEN B.DIEN_TICH_GAP_TUONGTU_KHAC  
					      ELSE 0
					 END
					) DIEN_TICH_GAP_TUONGTU_KHAC_1
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=1 THEN B.SAN_LUONG_KHAC  
					      ELSE 0
					 END
					) SAN_LUONG_KHAC_1
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=1 THEN B.SO_LUONG_GIONG_THA_KHAC
						  ELSE 0
					 END 
					) SO_LUONG_GIONG_THA_KHAC_1
					
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=2 THEN B.DIEN_TICH_THANUOI_TRONGKY_KHAC  
					      ELSE 0
					 END
					) DIEN_TICH_THANUOI_TRONGKY_KHAC_2
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=2 THEN B.NUOI_TRONG_QUY_HOACH_KHAC  
					      ELSE 0
					 END
					) NUOI_TRONG_QUY_HOACH_KHAC_2
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=2 THEN B.NUOI_NGOAI_QUY_HOACH_KHAC  
					      ELSE 0
					 END
					) NUOI_NGOAI_QUY_HOACH_KHAC_2
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=2 THEN B.DIEN_TICH_GAP_TUONGTU_KHAC  
					      ELSE 0
					 END
					) DIEN_TICH_GAP_TUONGTU_KHAC_2
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=2 THEN B.SAN_LUONG_KHAC  
					      ELSE 0
					 END
					) SAN_LUONG_KHAC_2
				,SUM(CASE WHEN B.ID_PHUONG_THUC_NUOI_KHAC=2 THEN B.SO_LUONG_GIONG_THA_KHAC
						  ELSE 0
					 END 
					) SO_LUONG_GIONG_THA_KHAC_2
				
			FROM NT_NUOC_NGOT A INNER JOIN NT_NUOC_NGOT_DETAIL B
									ON A.ID=B.ID_NUOC_NGOT
								 INNER JOIN DM_DOITUONG_NUOI_NGOT C
									ON B.ID_DOITUONG_NUOI_NGOT =C.ID
			WHERE		A.LOAI_BAO_CAO=@LoaiBaoCao 
					AND B.ID_HINHTHUC_NUOI=@HinhThucNuoi
					AND (@Thang is null or A.THANG=@Thang)
					AND (@Nam is null or A.NAM=@Nam)
					AND (@TuNgay is null or A.NGAY_BAO_CAO_TU>=@TuNgay)
					AND (@DenNgay is null or A.NGAY_BAO_CAO_DEN<=@DenNgay)
					AND (@TThanhPho is null or A.MA_TINHTP=@TThanhPho)
			GROUP BY B.ID_DOITUONG_NUOI_NGOT,C.TEN_DOI_TUONG
		END
	 
	
END
