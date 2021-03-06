
/****** Object:  StoredProcedure [dbo].[NT_NUOILONGBE_SEARCH]    Script Date: 05/11/2016 16:04:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[NT_NUOILONGBE_SEARCH]
	  @LoaiBaoCao int 
	 ,@LoaiMatNuoc int =null
	 ,@Thang int =null
	 ,@Nam int =nul
	 ,@TuNgay datetime =null
	 ,@DenNgay datetime =null
	 ,@TThanhPho varchar(50) =null
AS
BEGIN
	IF(@LoaiMatNuoc is null) --union
		BEGIN
			SELECT N'<b>Nước ngọt</b>' DOI_TUONG_NUOI,NULL THE_TICH_LONG,NULL SAN_LUONG_LONG,NULL SO_LUONG_LONG,NULL MAT_DO_THA
		UNION ALL
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
					AND (@Thang is null or A.THANG=@Thang)
					AND (@Nam is null or A.NAM=@Nam)
					AND (@TuNgay is null or A.NGAY_BAO_CAO_TU>=@TuNgay)
					AND (@DenNgay is null or A.NGAY_BAO_CAO_DEN<=@DenNgay)
					AND (@TThanhPho is null or A.MA_TINHTP=@TThanhPho)
			GROUP BY B.ID_DOITUONG_NUOI_NGOT,C.TEN_DOI_TUONG
		UNION ALL
			SELECT N'<b>Nước mặn, lợ</b>' DOI_TUONG_NUOI,NULL THE_TICH_LONG,NULL SAN_LUONG_LONG,NULL SO_LUONG_LONG,NULL MAT_DO_THA
		UNION ALL
			SELECT
				 C.TEN_DOI_TUONG DOI_TUONG_NUOI 
				,SUM(B.THE_TICH_LONG_LONGBE) THE_TICH_LONG
				,SUM(B.SAN_LUONG_LONGBE) SAN_LUONG_LONG
				,SUM(B.SO_LUONG_LONG_LONGBE) SO_LUONG_LONG
				,SUM(ISNULL(B.SO_LUONG_GIONG_THA_LONGBE,0)) MAT_DO_THA
			FROM NT_NUOC_MANLO A INNER JOIN NT_NUOC_MANLO_DETAIL B
									ON A.ID=B.ID_NUOC_MANLO
								 INNER JOIN DM_DOITUONG_NUOI_MANLO C
									ON B.ID_DOITUONG_NUOI_MANLO =C.ID
			WHERE		A.LOAI_BAO_CAO=@LoaiBaoCao 
					AND (@Thang is null or A.THANG=@Thang)
					AND (@Nam is null or A.NAM=@Nam)
					AND (@TuNgay is null or A.NGAY_BAO_CAO_TU>=@TuNgay)
					AND (@DenNgay is null or A.NGAY_BAO_CAO_DEN<=@DenNgay)
					AND (@TThanhPho is null or A.MA_TINHTP=@TThanhPho)
			GROUP BY B.ID_DOITUONG_NUOI_MANLO,C.TEN_DOI_TUONG
		
			
		END
	ELSE
		BEGIN
			IF (@LoaiMatNuoc=1) --Nước ngọt
				BEGIN
					SELECT N'<b>Nước ngọt</b>' DOI_TUONG_NUOI,NULL THE_TICH_LONG,NULL SAN_LUONG_LONG,NULL SO_LUONG_LONG,NULL MAT_DO_THA
				UNION ALL
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
							AND (@Thang is null or A.THANG=@Thang)
							AND (@Nam is null or A.NAM=@Nam)
							AND (@TuNgay is null or A.NGAY_BAO_CAO_TU>=@TuNgay)
							AND (@DenNgay is null or A.NGAY_BAO_CAO_DEN<=@DenNgay)
							AND (@TThanhPho is null or A.MA_TINHTP=@TThanhPho)
					GROUP BY B.ID_DOITUONG_NUOI_NGOT,C.TEN_DOI_TUONG
					
				END
			ELSE -- Nước mặn, lợ
				BEGIN
					SELECT N'<b>Nước mặn, lợ</b>' DOI_TUONG_NUOI,NULL THE_TICH_LONG,NULL SAN_LUONG_LONG,NULL SO_LUONG_LONG,NULL MAT_DO_THA
				UNION ALL
					SELECT
						 C.TEN_DOI_TUONG DOI_TUONG_NUOI 
						,SUM(B.THE_TICH_LONG_LONGBE) THE_TICH_LONG
						,SUM(B.SAN_LUONG_LONGBE) SAN_LUONG_LONG
						,SUM(B.SO_LUONG_LONG_LONGBE) SO_LUONG_LONG
						,SUM(ISNULL(B.SO_LUONG_GIONG_THA_LONGBE,0)) MAT_DO_THA
					FROM NT_NUOC_MANLO A INNER JOIN NT_NUOC_MANLO_DETAIL B
											ON A.ID=B.ID_NUOC_MANLO
										 INNER JOIN DM_DOITUONG_NUOI_MANLO C
											ON B.ID_DOITUONG_NUOI_MANLO =C.ID
					WHERE		A.LOAI_BAO_CAO=@LoaiBaoCao 
							AND (@Thang is null or A.THANG=@Thang)
							AND (@Nam is null or A.NAM=@Nam)
							AND (@TuNgay is null or A.NGAY_BAO_CAO_TU>=@TuNgay)
							AND (@DenNgay is null or A.NGAY_BAO_CAO_DEN<=@DenNgay)
							AND (@TThanhPho is null or A.MA_TINHTP=@TThanhPho)
					GROUP BY B.ID_DOITUONG_NUOI_MANLO,C.TEN_DOI_TUONG
				END
			 END
	
END
