
/****** Object:  StoredProcedure [dbo].[KT_SucoTau]    Script Date: 4/28/2016 4:23:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[KT_THIETHAIKHAITHAC_SUCOTAU]
	@fromdate datetime,
	@todate datetime
AS
BEGIN
	
	SET NOCOUNT ON;

   select 
	sum(case when DSUCOVETAU_ID=1 then 1 else 0 end) mat_lien_lac,
	sum(case when DSUCOVETAU_ID=2 then 1 else 0 end) hong_may_troi_dat,
	sum(case when DSUCOVETAU_ID=3 then 1 else 0 end) dam_va,
	sum(case when DSUCOVETAU_ID=4 then 1 else 0 end) chim_dam, 
	sum(case when DSUCOVETAU_ID=5 then 1 else 0 end) chay_no  
	from DSUCOVETAUKT_THIETHAIKHAITHAC a
	inner join KT_THIETHAIKHAITHAC b
	on a.KT_THIETHAIKHAITHAC_ID =   b.ID
	where (@fromdate is null or   b.TG_GAPNAN >= @fromdate)
	 and 
	 (@todate is null or b.TG_GAPNAN <= @todate)


END
