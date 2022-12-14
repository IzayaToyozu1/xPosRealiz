USE [KassRealiz]
GO
/****** Object:  StoredProcedure [sendFrontol].[GetLastIdGoodTerminal_XPos_V3]    Script Date: 26.07.2022 16:00:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Dorin D.A.>
-- Create date: <2022.07.14>
-- Description:	<Получение последнего id goods_updates отправленого на кассы>
-- =============================================
ALTER PROCEDURE [sendFrontol].[GetLastIdGoodTerminal_XPos_V3]
AS
BEGIN
	select top(1) id_gu
	from 
		dbase1.sendFrontol.s_Terminal 
	where 
		last_id_gu is not null and isActive = 1 and id_gu <> 0 order by id_gu asc
END
