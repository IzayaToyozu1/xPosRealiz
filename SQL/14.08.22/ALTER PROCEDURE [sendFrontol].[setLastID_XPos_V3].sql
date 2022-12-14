USE [dbase1]
GO
/****** Object:  StoredProcedure [sendFrontol].[setLastID_XPos_V3]    Script Date: 12.08.2022 16:17:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alekseev KU>
-- Create date: <22.03.2017>
-- Description:	<Получает последний id из таблицы goods_updates>
-- Editor:		Dorin D.A
-- Edit date:	21.07.2022
-- Description	
-- =============================================
ALTER PROCEDURE [sendFrontol].[setLastID_XPos_V3]
  @number int,  
  @lastIDTerminal bigint,
  @id_user int
AS
BEGIN
 declare @LastIDGoodsUpdate int
 set @LastIDGoodsUpdate = (Select top(1) id from KassRealiz.dbo.goods_updates order by id desc)
	if(@lastIDTerminal <> 0)
	begin
		update 
			sendFrontol.s_Terminal
		set 
			id_gu = @lastIDTerminal,
			DateGoodsSend = GETDATE(),
			id_GoodsSender = @id_user
		where 
			Number = @number
	end
	
	update sendFrontol.s_Terminal
	set last_id_gu = @LastIDGoodsUpdate
	where
		isActive = 1
END
