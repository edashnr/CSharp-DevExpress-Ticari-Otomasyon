USE [DboTicariOtomasyon]
GO
/****** Object:  StoredProcedure [dbo].[BankaBilgileri]    Script Date: 26.03.2024 15:21:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--inner join
--select sutunlar tablo1 inner join tablo2 on tablo1.deger=tablo2.deger

ALTER procedure [dbo].[BankaBilgileri]
as
select TBL_BANKALAR.ID,Bankaadı,tbl_bankalar.IL,tbl_bankalar.ILCE,sube, 
ıban,HESAPNO ,YETKILI,TELEFON,TARIH,HESAPTURU,ad
from 
TBL_BANKALAR inner join TBL_FIRMALAR 
on 
TBL_BANKALAR.FIRMAID=TBL_FIRMALAR.ID
