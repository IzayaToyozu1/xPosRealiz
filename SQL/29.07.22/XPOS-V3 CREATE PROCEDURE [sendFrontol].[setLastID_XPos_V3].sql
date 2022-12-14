USE [dbase1]
GO
/****** Object:  StoredProcedure [sendFrontol].[setLastID]    Script Date: 11.07.2022 10:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Alekseev KU>
-- Create date: <22.03.2017>
-- Description:	<Получает последний id из таблицы goods_updates>
-- =============================================
CREATE PROCEDURE [sendFrontol].[setLastID_XPos_V3]
  @number int,  
  @lastID bigint,
  @isOk bit = 0,
  @id_user int
AS
BEGIN
	IF @isOk = 1
	BEGIN
		update 
			sendFrontol.s_Terminal
		set 
			last_id_gu = @lastID,
			id_gu = @lastID,
			DateGoodsSend = GETDATE(),
			id_GoodsSender = @id_user
		where 
			Number = @number
	END
	ELSE
	BEGIN
		IF @number = -1
		begin
			update
				sendFrontol.s_Terminal
			set 
				last_id_gu = @lastID
		end
		ELSE
		begin
			update 
				sendFrontol.s_Terminal
			set 
				last_id_gu = @lastID
				--DateGoodsSend = GETDATE(),
				--id_GoodsSender = @id_user
			where 
				Number = @number
		end
	END
END
