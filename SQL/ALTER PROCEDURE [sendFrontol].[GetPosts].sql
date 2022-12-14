USE [dbase1]
GO
/****** Object:  StoredProcedure [sendFrontol].[GetPosts]    Script Date: 03.09.2022 9:25:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dorin D.A.
-- Create date: 31.08.2022
-- Description:	Получение поставщика
-- =============================================
ALTER PROCEDURE [sendFrontol].[GetPosts]
	@idPost int
AS
BEGIN
	SELECT post.id
      ,cname
	  ,Trim(inn) as inn
	  , docum.Phone
	FROM dbo.s_post post
		join dbo.s_DocumentEntries docum on docum.id_Supplier = post.id
	WHERE post.id = @idPost
END