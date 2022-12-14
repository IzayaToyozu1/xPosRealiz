USE [dbase1]
GO
/****** Object:  StoredProcedure [sendFrontol].[getListGoodsDbase1]    Script Date: 14.07.2022 11:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Description:	<Получение списка товаров>
-- Editor:	<Dorin D.A.>
-- Edit date:	<02.03.2022>
-- Description:	<Изменены условия >
-- =============================================

CREATE PROCEDURE [sendFrontol].[getListGoodsDbase1_XPos_V3] 
	@isVOO bit = 1
AS
BEGIN

select 
	m.id_Post,
	m.nTypeOrg
INTO 
	#mainOrg
from  
	[dbo].[s_MainOrg] m
where 
	m.DateStart < GETDATE()
	and m.DateEnd > GETDATE()

select 
	tov.ean,
	tov.id_grp1,
	tov.id_otdel as id_departments,
	tov.id as id_tovar
INTO 
#tableEAN
from dbo.s_tovar tov
where 
	(@isVOO = 1 and id_otdel = 6) or (@isVOO = 0 and id_otdel <> 6  )  

select 
	t.ean,
	getdate() as r_time,
	''as name,	
	0.0 as price,
	0 as grp,
	0 as 'tax',
	case 
			when len(t.id_tovar) = 3 then '0'+cast(t.id_tovar as varchar(3))
			when len(t.id_tovar) = 2 then '00'+cast(t.id_tovar as varchar(2))
			else cast(t.id_tovar as varchar)
			end as id_tovar, 
	'' as kodVVO,
	(select cname from dbo.s_post where id =
		(select id_Post from #mainOrg
			 where nTypeOrg = 
				 (
					ISNULL((  
							select top 1
							case when ntypeorg in ( select ntypeorg 
									from [firms_vs_departments] with(nolock)
									where DateStart < GETDATE() and DateEnd > GETDATE() and id_departments = 
									(select id_otdel from s_tovar where id = t.id_tovar ) ) then  ntypeorg 
									else NULL end
							
							
							from [dbo].[goods_vs_firms] with(nolock)
							where CAST(GETDATE() as date) >= date
							and id_tovar = t.id_tovar  and [send] = 1
							order by date desc),
								(
									select ntypeorg 
									from [firms_vs_departments] with(nolock)
									where DateStart < GETDATE() and DateEnd > GETDATE()
									and [default] = 1 and id_departments = 
									(select id_otdel from s_tovar where id = t.id_tovar ))
								)
							)
		)
   ) as firm
   ,(select id_Post 
   from  #mainOrg
   where nTypeOrg = (
    ISNULL( 
				(
					select top 1
							case when ntypeorg in ( select ntypeorg 
									from [firms_vs_departments] with(nolock)
									where DateStart < GETDATE() and DateEnd > GETDATE() and id_departments = 
									(select id_otdel from s_tovar where id = t.id_tovar ) ) then  ntypeorg 
									else NULL end
					from [dbo].[goods_vs_firms] with(nolock)
					where CAST(GETDATE() as date) >= date
					and ntypeorg not in (6,17) and [send] = 1
					and id_tovar = t.id_tovar 
					order by date desc
				),(
					select ntypeorg from [firms_vs_departments] with(nolock)
					where DateStart < GETDATE() 
						and DateEnd > GETDATE()
						and [default] = 1 
						and id_departments = 
							(select id_otdel from s_tovar where id = t.id_tovar)
				)
			)
		)
	)
  as id_post
  ,t.id_grp1
  ,t.id_departments
  ,tt.ntypetovar
  ,tt.id_unit
  ,-1 as IsCatPromTovar
from 
	#tableEAN t
	inner join dbo.v_tovar tt on t.id_tovar = tt.id_tovar

DROP TABLE #tableEAN, #mainOrg

END