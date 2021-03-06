USE [FDB15]
GO
/****** Object:  StoredProcedure [dbo].[KT_CANGCA_HANGHOAQUACANG]    Script Date: 5/4/2016 9:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[KT_CANGCA_HANGHOAQUACANG]
@cangca_id int,
@fromdate datetime,
@todate datetime

AS
BEGIN
	SET NOCOUNT ON;

    select 		
		sum(tau_20cv) TAU_20CV ,
		SUM(TAU_50V) TAU_50V,
		SUM(TAU_90CV) TAU_90CV,
		SUM(TAU_250V) TAU_250V,
		SUM(TAU_400V) TAU_400V,
		SUM(TAU_TREN_400V) TAU_TREN_400V,
		SUM(TAU_KHAC) TAU_KHAC,
		SUM(SANLUONG_CA) SANLUONG_CA,
		SUM(SANLUONG_TOM) SANLUONG_TOM,
		SUM(SANLUONG_MUC) SANLUONG_MUC,
		SUM(SANLUONG_KHAC) SANLUONG_KHAC,
		SUM(HANG_NUOCDA) HANG_NUOCDA,
		SUM(HANG_XANGDAU) HANG_XANGDAU,
		SUM(HANG_NUOCNGOT) HANG_NUOCNGOT,
		SUM(HANG_KHAC) HANG_KHAC	
		--ISNULL(SUM(HANG_NUOCNGOT),0) HANG_NUOCNGOT,
		--ISNULL(SUM(HANG_KHAC),0) HANG_KHAC
	from kt_cangca 
	where	(@cangca_id is null or DM_CANGCAID = @cangca_id)
			and
			(@fromdate is null or   NGAY_GHINHAN >= @fromdate)
			and 
			(@todate is null or NGAY_GHINHAN <= @todate)

END
