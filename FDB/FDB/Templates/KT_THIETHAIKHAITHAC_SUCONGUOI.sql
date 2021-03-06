
GO
/****** Object:  StoredProcedure [dbo].[KT_SucoNguoi]    Script Date: 4/28/2016 4:23:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[KT_THIETHAIKHAITHAC_SUCONGUOI]
	@fromdate datetime,
	@todate datetime
AS
BEGIN
	
	SET NOCOUNT ON;

   select 
	sum(case when DSUCOVENGUOI_ID=1 then 1 else 0 end) bi_chet,
	sum(case when DSUCOVENGUOI_ID=2 then 1 else 0 end) mat_tich,
	sum(case when DSUCOVENGUOI_ID=3 then 1 else 0 end) roi_xuong_bien,
	sum(case when DSUCOVENGUOI_ID=4 then 1 else 0 end) tai_nan_lao_dong
	
	from KT_THIETHAIKHAITHACDSUCOVENGUOI a
	inner join KT_THIETHAIKHAITHAC b
	on a.KT_THIETHAIKHAITHAC_ID =   b.ID
	where (@fromdate is null or   b.TG_GAPNAN >= @fromdate)
	 and 
	 (@todate is null or b.TG_GAPNAN <= @todate)


END
