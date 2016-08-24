
GO
/****** Object:  StoredProcedure [dbo].[KT_ThietHai_Tong]    Script Date: 4/28/2016 4:24:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[KT_THIETHAIKHAITHAC_TONG]
	@fromdate datetime,
	@todate datetime
AS
BEGIN
	
	SET NOCOUNT ON;

   select top 1 sum(THIETHAI_UOCTINH) as [tong_thiet_hai] from KT_THIETHAIKHAITHAC
	where (@fromdate is null or   TG_GAPNAN >= @fromdate)
	 and 
	 (@todate is null or TG_GAPNAN <= @todate)


END
