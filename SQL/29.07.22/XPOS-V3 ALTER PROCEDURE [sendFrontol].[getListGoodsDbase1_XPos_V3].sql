USE [dbase1]
GO
/****** Object:  StoredProcedure [sendFrontol].[getListGoodsDbase1_XPos_V3]    Script Date: 27.07.2022 12:36:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Description:	<Получение списка товаров>
-- Author:		<Dorin D.A.>
-- Edit date:	<02.03.2022>
-- Description:	<Изменены условия >
-- =============================================

ALTER PROCEDURE [sendFrontol].[getListGoodsDbase1_XPos_V3]
	@isVOO bit = 0
AS
BEGIN


select 
	f.id_tovar,f.ntypeorg 
INTO 
	#ntypeorgTovar
from (
select  
	max(date) as date,
	id_tovar
from 
	[dbo].[goods_vs_firms]
where
	CAST(GETDATE() as date) >= date
	and 
	ntypeorg not in (6,17)									
group by
	id_tovar) as a inner join [dbo].[goods_vs_firms] f on f.id_tovar = a.id_tovar and f.date = a.date

select 
	ntypeorg ,id_departments
INTO 
	#ntypeorgDeps
from 
	[firms_vs_departments]
where
	DateStart < GETDATE() and DateEnd > GETDATE()
	and [default] = 1
order by id_departments

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
	tov.id_otdel as id_departments, --добавлен "as id_departments"
	tov.id as id_tovar,
	isnull(nTov.ntypeorg,nDeps.ntypeorg) as ntypeorg
INTO 
	#tableEAN
from dbo.s_tovar tov
	left join #ntypeorgTovar nTov on nTov.id_tovar = tov.id
	left join #ntypeorgDeps nDeps on nDeps.id_departments= tov.id_otdel
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
	'' as kodVVO
	,(select cname 
		from dbo.s_post 
		where id = 
		(
			select id_Post 
			from #mainOrg
			where nTypeOrg = 
			(
				ISNULL
				(
					(select top 1
						case when ntypeorg in 
						( 
							select ntypeorg 
							from [firms_vs_departments] with(nolock)
							where DateStart < GETDATE() and DateEnd > GETDATE() 
									and id_departments = (select id_otdel 
															from s_tovar 
															where id = t.id_tovar) 
							) then  ntypeorg 
						else NULL end
					from [dbo].[goods_vs_firms] with(nolock)
					where CAST(GETDATE() as date) >= date
						and id_tovar = t.id_tovar  and [send] = 1
					order by date desc
					),
					(
					select ntypeorg 
					from [firms_vs_departments] with(nolock)
					where DateStart < GETDATE() and DateEnd > GETDATE()
						and [default] = 1 
						and id_departments = (select id_otdel 
												from s_tovar 
												where id = t.id_tovar )
					)
				)
			)
		)	
   ) as firm
   ,(select id_Post 
   from  #mainOrg
   where nTypeOrg = 
	   (
			ISNULL
			(
				(
					select top 1
								case when ntypeorg in 
								( select ntypeorg 
									from [firms_vs_departments] with(nolock)
									where DateStart < GETDATE() and DateEnd > GETDATE() and id_departments = 
									(
										select id_otdel 
										from s_tovar 
										where id = t.id_tovar ) 
								) then  ntypeorg 
								else NULL end
					from [dbo].[goods_vs_firms] with(nolock)
					where CAST(GETDATE() as date) >= date
						and ntypeorg not in (6,17) and [send] = 1
						and id_tovar = t.id_tovar 
						order by date desc
				),
				(
					select ntypeorg from [firms_vs_departments] with(nolock)
					where DateStart < GETDATE() 
						and DateEnd > GETDATE()
						and [default] = 1 
						and id_departments = 
							(select id_otdel from s_tovar where id = t.id_tovar)
				)
			)
		)
	) as id_post
  ,	t.id_grp1
  , t.id_departments
  , tt.ntypetovar
  , tt.id_unit
  ,ISNULL(C.id_tovar, -1) as IsCatPromTovar
from 
	#tableEAN t
	left join dbo.v_tovar tt on t.id_tovar = tt.id_tovar
	left join (select DISTINCT id_tovar from dbase1.requests.j_CatalogPromotionalTovars ) C on C.id_tovar = t.id_tovar

DROP TABLE #tableEAN, #ntypeorgTovar,#ntypeorgDeps,#mainOrg

END



