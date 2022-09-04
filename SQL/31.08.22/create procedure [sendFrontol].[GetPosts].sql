Use dbase1
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dorin D.A.
-- Create date: 31.08.2022
-- Description:	Получение поставщика
-- =============================================
CREATE PROCEDURE [sendFrontol].[GetPosts]
	@idPost int
AS
BEGIN
	SELECT post.id
      ,cname
	  ,inn
	  , docum.Phone
	FROM [dbo].[s_post] post
		join dbo.s_DocumentEntries docum on docum.id_Supplier = post.id
	WHERE post.id = @idPost
END
GO
