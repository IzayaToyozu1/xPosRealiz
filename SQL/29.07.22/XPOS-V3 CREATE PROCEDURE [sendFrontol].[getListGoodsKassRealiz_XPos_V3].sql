USE [KassRealiz]
GO
/****** Object:  StoredProcedure [sendFrontol].[getListGoodsKassRealiz_XPos_V3]    Script Date: 14.07.2022 16:42:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Description:	<Получение списка актуальных товаров из goods_updates>
-- Editor		Dorin D.A.
-- Date			2022
-- Description	Добвлено условие получение товара
-- =============================================

CREATE PROCEDURE [sendFrontol].[getListGoodsKassRealiz_XPos_V3] 
@LastIdGoods int = 0
AS
BEGIN

Declare @MinIdGoodsUpdate int 
set @MinIdGoodsUpdate = (select top(1) id_gu from dbase1.sendFrontol.s_Terminal where last_id_gu is not null and isActive = 1 order by id_gu asc)

select
	g.id,
	g.r_time,
	ltrim(rtrim(g.ean)) as ean,
	isnull(ltrim(rtrim(g.name)),'') as name,
	isnull(g.price/100.0,0.0) as price,
	cast(g.grp as int) as grp,
	--g.tax,
	cast(case when g.tax<>20 then g.tax else 18 end as int)  as tax,
	g.id_departments
from 
	dbo.goods_updates g
where 
	ActualRow = 1 and id > @MinIdGoodsUpdate and g.id > @LastIdGoods

END
