USE [dbase1]
GO
/****** Object:  StoredProcedure [sendFrontol].[getListTovar]    Script Date: 10.08.2022 16:34:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<AKU>
-- Create date: <2016-06-28>
-- Description:	<Создание списка товаров для xPOS>
-- Edited:      <Molotkova_IS>
-- Edit date:   <2019-03-28>
-- Description: <Добавлено поле id_post>
-- Edited:      <Bychkov_SA>
-- Edit date:   <2019-07-11>
-- Description: <Ускорено, актуалиировал, добавлено условие поиска по g_u  по полю ActualRow>
-- Edited:      <Dorin D.A.>
-- Edit date:   <2022-04-20>
-- Description: <Добавлены поля ntypetovar, id_unit>
-- =============================================
ALTER PROCEDURE [sendFrontol].[getListTovar] 
AS
BEGIN

create table #s_MainOrg (id_Post int, nTypeOrg tinyint)

insert into #s_MainOrg(id_Post, nTypeOrg)
select id_Post, nTypeOrg
from dbo.s_MainOrg
where DateStart < GETDATE() and DateEnd > GETDATE()

select distinct
	 a.ean
	,a.r_time
	,a.name
	,a.price
	,case when grp1.id is null  then 
				case when a.id_departments = 1 then 10
					 when a.id_departments = 2 then 20
					 when a.id_departments = 3 then 30
					 when a.id_departments = 4 then 40
					 when a.id_departments = 5 then 50
					 when a.id_departments = 6 then 60
					 when a.id_departments = 7 then 70
					 when a.id_departments = 8 then 80
					 when a.id_departments = 10 then 11
					 when a.id_departments = 11 then 349
				end
		  else a.grp
     end as grp
	,a.tax
	,case 
		when len(t.id_tovar) = 3 then '0'+cast(t.id_tovar as varchar(3))
		when len(t.id_tovar) = 2 then '00'+cast(t.id_tovar as varchar(2))
		else cast(t.id_tovar as varchar) end as id_tovar
	,case when aa.kodVVO = '_' then NULL else aa.kodVVO end as kodVVO 
	,(
	select cname from dbo.s_post where id =
		(
			select id_Post from #s_MainOrg
			where nTypeOrg = 
				(
				ISNULL
					(
						(  
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
   ,(
		select id_Post 
		from  #s_MainOrg
		where nTypeOrg = 
	   (
		ISNULL
			(
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
	)
  as id_post
  ,a.id_departments
  ,vt.ntypetovar
  ,vt.id_unit	
from 
	(
	select ltrim(rtrim(t.ean)) as ean, t.id as id_tovar from s_tovar t	where id_otdel<>6
	)  t
	inner join dbo.s_ntovar nt with(nolock) on nt.id_tovar=t.id_tovar
	inner join 
	(
	select distinct
		 ltrim(rtrim(ean)) as ean 
		,r_time
		,name
		,cast(price as numeric(13,3))/100 as price
		,grp
		,id_departments
		,tax
	from [KassRealiz].[dbo].[goods_updates]
	where ActualRow = 1 --and id_departments <> 6 
	) a on t.ean collate Cyrillic_General_CS_AI = a.ean	 collate Cyrillic_General_CS_AI
	inner join dbo.s_Proizvodstvo p with(nolock) on p.id_tovar = t.id_tovar
	inner join dbo.s_tovar_inf aa with(nolock) on aa.id_proizvodstvo = p.id	and aa.ldeystv=1
	left join  dbo.s_grp1 grp1 with(nolock) on grp1.id = a.grp
	inner join dbo.v_tovar vt with(nolock) on vt.id_tovar = t.id_tovar
where nt.ntypetovar in (0,1) --and id_departments <> 6
order by ean asc
option(recompile)

drop table #s_MainOrg

END