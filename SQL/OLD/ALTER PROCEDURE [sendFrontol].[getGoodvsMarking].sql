USE [dbase1]
GO
/****** Object:  StoredProcedure [sendFrontol].[getGoodvsMarking]    Script Date: 03.08.2022 15:17:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sporykhin G.Y.
-- Create date: 2022-01-22
-- Description:	Получение списка товаров для маркировки
-- =============================================
ALTER PROCEDURE [sendFrontol].[getGoodvsMarking]		 	
AS
BEGIN
	SET NOCOUNT ON;

select 
	t.id_tovar,
	t.id_grp1,
	trim(t.ean) as ean,
	trim(t.cname) as nameGood,
	g.is_CheckMarking,
	tt.typeXposCode

from 
	sendFrontol.grp1_vs_TypeMarking g
	inner join dbo.v_tovar t on t.id_grp1 = g.id_grp1
	left join sendFrontol.s_TovarMarking m on m.id_tovar = t.id_tovar
	left join sendFrontol.s_TypeMarking tt on tt.id = g.id_TypeMarking

		
END
