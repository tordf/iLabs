/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: USS_Procedures.sql,v 1.5 2007/06/28 14:08:05 pbailey Exp $
 */

/****** Object:  Stored Procedure dbo.AddReservation    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddReservation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddReservation]
GO

/****** Object:  Stored Procedure dbo.AddUssPolicy    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddUssPolicy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddUssPolicy]
GO

/****** Object:  Stored Procedure dbo.DeleteReservation    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteReservation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteReservation]
GO

/****** Object:  Stored Procedure dbo.DeleteUSSPolicy    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteUSSPolicy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteUSSPolicy]
GO

/****** Object:  Stored Procedure dbo.ModifyReservation    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyReservation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyReservation]
GO

/****** Object:  Stored Procedure dbo.ModifyUSSPolicy    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyUSSPolicy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyUSSPolicy]
GO

/****** Object:  Stored Procedure dbo.RetrieveReservationByID    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveReservationByID]
GO

/****** Object:  Stored Procedure dbo.RetrieveReservationIDByUser    Script Date: 10/4/2006 12:39:19 PM ******/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationIDByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[RetrieveReservationIDByUser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationIDByUserAndGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[RetrieveReservationIDByUserAndGroup]

GO

/****** Object:  Stored Procedure dbo.RetrieveReservationIDsByLabServer    Script Date: 10/4/2006 12:39:19 PM ******/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationIDsByLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[RetrieveReservationIDsByLabServer]

GO



/****** Object:  Stored Procedure dbo.RetrieveReservationIDsByUser    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationIDsByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveReservationIDsByUser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveReservations]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationsByAll]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveReservationsByAll]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationsByExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveReservationsByExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationsByGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveReservationsByGroup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationsByGroupAndExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveReservationsByGroupAndExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationsByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveReservationsByUser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationsByUserAndGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[RetrieveReservationsByUserAndGroup]

GO

/****** Object:  Stored Procedure dbo.RetrieveUSSPolicyByID    Script Date: 10/4/2006 12:39:19 PM ******/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUSSPolicyByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[RetrieveUSSPolicyByID]

GO



/****** Object:  Stored Procedure dbo.RetrieveUSSPolicyIDs    Script Date: 10/4/2006 12:39:19 PM ******/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUSSPolicyIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[RetrieveUSSPolicyIDs]

GO



/****** Object:  Stored Procedure dbo.RetrieveUSSPolicyIDsByGroup    Script Date: 10/4/2006 12:39:19 PM ******/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUSSPolicyIDsByGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[RetrieveUSSPolicyIDsByGroup]

GO



/****** Object:  Stored Procedure dbo.RetrieveUSSPolicyIDsByGroupandExp    Script Date: 10/4/2006 12:39:19 PM ******/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUSSPolicyIDsByGroupandExp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[RetrieveUSSPolicyIDsByGroupandExp]

GO



/****** Object:  Stored Procedure dbo.AddExperimentInfo    Script Date: 10/4/2006 12:39:19 PM ******/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddExperimentInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[AddExperimentInfo]

GO



/****** Object:  Stored Procedure dbo.DeleteExperimentInfo    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteExperimentInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteExperimentInfo]
GO

/****** Object:  Stored Procedure dbo.ModifyExperimentInfo    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyExperimentInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyExperimentInfo]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyExperimentInfoByGuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyExperimentInfoByGuid]
GO
/****** Object:  Stored Procedure dbo.ModifyExperimentInfo    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyExperimentLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyExperimentLabServer]
GO
/****** Object:  Stored Procedure dbo.RetrieveExperimentIDs    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInfoByID    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentInfoByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentInfoByID]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInfoIDByExperiment    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentInfoIDByExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentInfoIDByExperiment]
GO

/****** Object:  Stored Procedure dbo.RetrieveLSSIDbyExperiment    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLSSIDbyExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLSSIDbyExperiment]
GO

/****** Object:  Stored Procedure dbo.RetrieveLSSURLbyExperiment    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLSSURLbyExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLSSURLbyExperiment]
GO

/****** Object:  Stored Procedure dbo.AddCredentialSet    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddCredentialSet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddCredentialSet]
GO

/****** Object:  Stored Procedure dbo.AddCredentialSet    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCredentialSetID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCredentialSetID]
GO


/****** Object:  Stored Procedure dbo.AddLSSInfo    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddLSSInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddLSSInfo]
GO



/****** Object:  Stored Procedure dbo.DeletLSSInfo    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeletLSSInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeletLSSInfo]
GO

/****** Object:  Stored Procedure dbo.DeleteCredentialSet    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteCredentialSet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteCredentialSet]
GO

/****** Object:  Stored Procedure dbo.DeleteCredentialSetByID    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteCredentialSetByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteCredentialSetByID]
GO

/****** Object:  Stored Procedure dbo.ModifyLssInfo    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyLssInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyLssInfo]
GO

/****** Object:  Stored Procedure dbo.ModifyLssInfo    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyLssInfoByGuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyLssInfoByGuid]
GO

/****** Object:  Stored Procedure dbo.RetrieveCredentialSetByID    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveCredentialSetByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveCredentialSetByID]
GO

/****** Object:  Stored Procedure dbo.RetrieveCredentialSetIDs    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveCredentialSetIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveCredentialSetIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveLSSInfoByID    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLSSInfoByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLSSInfoByID]
GO

/****** Object:  Stored Procedure dbo.RetrieveLSSInfoByID    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLSSInfoByGUID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLSSInfoByGUID]
GO

/****** Object:  Stored Procedure dbo.RetrieveLssInfoIDs    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLssInfoIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLssInfoIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveReservationIDsByGroup    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveReservationIDsByGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveReservationIDsByGroup]
GO

/****** Object:  Stored Procedure dbo.ModifyCredentialSet    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyCredentialSet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyCredentialSet]
GO

/****** Object:  Stored Procedure dbo.ModifyCredentialSet    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyCredentialSetServiceBroker]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyCredentialSetServiceBroker]
GO
/****** Object:  Stored Procedure dbo.ModifyCredentialSet    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE ModifyCredentialSet
@credentialSetID int,
@groupName nvarchar(256),
@serviceBrokerGUID varchar(50),
@serviceBrokerName nvarchar(256)

AS

update Credential_Sets set Group_Name=@groupName, Service_Broker_GUID=@serviceBrokerGUID, Service_Broker_Name=@serviceBrokerName 
where Credential_Set_ID=@credentialSetID
return @@rowcount

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE ModifyCredentialSetServiceBroker
@originalGUID varchar(50),
@serviceBrokerGUID varchar(50),
@serviceBrokerName nvarchar(256)

AS

update Credential_Sets set Service_Broker_GUID=@serviceBrokerGUID, Service_Broker_Name=@serviceBrokerName 
where  Service_Broker_GUID=@originalGUID
return @@rowcount

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddCredentialSet    Script Date: 10/4/2006 12:39:22 PM ******/
/****** Object:  Stored Procedure dbo.AddCredentialSet    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE AddCredentialSet
@groupName nvarchar(256),
@serviceBrokerGUID varchar(50),
@serviceBrokerName nvarchar(256)

AS

insert into Credential_Sets( Group_Name, Service_Broker_GUID, Service_Broker_Name)values(@groupName, @serviceBrokerGUID,@serviceBrokerName)
select ident_current('Credential_Sets')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
Go

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE GetCredentialSetID

@serviceBrokerGUID varchar(50),
@groupName nvarchar(256)

AS
select Credential_Set_ID from Credential_Sets
where Service_Broker_GUID = @serviceBrokerGuid AND Group_Name = @groupName

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddLSSInfo    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE AddLSSInfo 
@lssGUID varchar(50),
@lssName nvarchar(256),
@lssURL nvarchar(512)

AS

insert into LSS_Info(LSS_GUID, LSS_Name, LSS_URL) values (@lssGUID, @lssName, @lssURL)
select ident_current('LSS_Info')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeletLSSInfo    Script Date: 11/16/2005 4:19:04 PM ******/
CREATE PROCEDURE DeletLSSInfo
@lssInfoID int

AS

delete from LSS_Info where LSS_Info_ID= @lssInfoID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteCredentialSet    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE DeleteCredentialSet
@groupName nvarchar(256),
@serviceBrokerGUID varchar(50),
@serviceBrokerName nvarchar(256)

 AS

delete from Credential_Sets where Group_Name = @groupName and Service_Broker_GUID = @serviceBrokerGUID and Service_Broker_Name = @serviceBrokerName

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteCredentialSetByID    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE DeleteCredentialSetByID
@credentialSetID int

AS

delete from Credential_Sets where Credential_Set_ID= @credentialSetID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyLssInfo    Script Date: 10/4/2006 12:39:23 PM ******/
/****** Object:  Stored Procedure dbo.ModifyLssInfo    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE ModifyLssInfo
@lssInfoID int,
@lssGUID varchar(50),
@lssName nvarchar(256),
@lssURL nvarchar(512)

 AS

update LSS_Info set LSS_GUID=@lssGUID, LSS_Name=@lssName, LSS_URL=@lssURL where LSS_Info_ID=@lssInfoID
select @@rowcount
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE ModifyLssInfoByGuid
@lssGUID varchar(50),
@lssName nvarchar(256),
@lssURL nvarchar(512)

 AS

update LSS_Info set LSS_Name=@lssName, LSS_URL=@lssURL where LSS_GUID=@lssGUID
select @@rowcount
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO
/****** Object:  Stored Procedure dbo.RetrieveCredentialSetByID    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveCredentialSetByID
@credentialSetID int

 AS

select Group_Name, Service_Broker_GUID,Service_Broker_Name from Credential_Sets where Credential_Set_ID=@credentialSetID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveCredentialSetIDs    Script Date: 11/16/2005 4:19:04 PM ******/
CREATE PROCEDURE RetrieveCredentialSetIDs

 AS

select Credential_Set_ID from Credential_Sets
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLSSInfoByID    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveLSSInfoByID
@lssInfoID int

AS

select LSS_GUID, LSS_Name, LSS_URL from LSS_Info where LSS_Info_ID=@lssInfoID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLSSInfoByID    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveLSSInfoByGUID
@lssGuid varchar(50)

AS

select LSS_Info_ID, LSS_GUID, LSS_Name, LSS_URL from LSS_Info where LSS_GUID=@lssGuid
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLssInfoIDs    Script Date: 10/4/2006 12:39:23 PM ******/
/****** Object:  Stored Procedure dbo.RetrieveLssInfoIDs    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveLssInfoIDs

 AS

select LSS_Info_ID from LSS_Info
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveReservationIDsByGroup    Script Date: 10/4/2006 12:39:23 PM ******/
/****** Object:  Stored Procedure dbo.RetrieveReservationIDsByGroup    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveReservationIDsByGroup
@groupName nvarchar(256),
@serviceBrokerGUID varchar(50)

AS

DECLARE @credentialSetID int
select @credentialSetID=(select Credential_Set_ID from Credential_Sets where Group_Name=@groupName and Service_Broker_GUID=@serviceBrokerGUID)
select Reservation_ID from Reservatons where Credential_Set_ID=@credentialSetID  order by Start_Time asc
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddExperimentInfo    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE AddExperimentInfo
@labClientGUID varchar(50),
@labServerGUID varchar(50),
@labServerName nvarchar(256),
@labClientVersion nvarchar(50),
@labClientName nvarchar(256),
@providerName nvarchar(256),
@lssGUID varchar(50)

 AS

insert into Experiment_Info(Lab_Client_GUID,Lab_Server_GUID, Lab_Server_Name, Lab_Client_Version, Lab_Client_Name, Provider_Name, LSS_GUID)
 values (@labClientGUID,@labServerGUID,@labServerName,@labClientVersion,@labClientName,@providerName,@lssGUID)

select ident_current('Experiment_Info')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteExperimentInfo    Script Date: 10/4/2006 12:39:23 PM ******/
/****** Object:  Stored Procedure dbo.DeleteExperimentInfo    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE DeleteExperimentInfo
@experimentInfoID int
AS

delete from Experiment_Info where Experiment_Info_ID= @experimentInfoID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.ModifyExperimentInfo    Script Date: 10/4/2006 12:39:23 PM ******/

CREATE PROCEDURE ModifyExperimentInfo
@experimentInfoID int,
@labClientGUID varchar(50),
@labServerGUID varchar(50),
@labServerName nvarchar(256),
@labClientVersion nvarchar(50),
@labClientName nvarchar(256),
@providerName nvarchar(256),
@lssGUID varchar(50)

AS

update Experiment_Info set Lab_Client_GUID=@labClientGUID,Lab_Server_GUID=@labServerGUID, Lab_Server_Name=@labServerName, Lab_Client_Version=@labClientVersion,Lab_Client_Name=@labClientName,Provider_Name=@providerName,LSS_GUID=@lssGUID where Experiment_Info_ID=@experimentInfoID
select @@rowcount

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ModifyExperimentInfoByGuid
@labClientGUID varchar(50),
@labServerGUID varchar(50),
@labServerName nvarchar(256),
@labClientVersion nvarchar(50),
@labClientName nvarchar(256),
@providerName nvarchar(256),
@lssGUID varchar(50)

AS

update Experiment_Info set  Lab_Server_Name=@labServerName, Lab_Client_Version=@labClientVersion,Lab_Client_Name=@labClientName,Provider_Name=@providerName,LSS_GUID=@lssGUID 
where Lab_Client_GUID=@labClientGUID and Lab_Server_GUID=@labServerGUID
select @@rowcount

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE ModifyExperimentLabServer
@labServerGUID varchar(50),
@labServerName nvarchar(256)

AS

update Experiment_Info set Lab_Server_Name=@labServerName
where Lab_Server_GUID=@labServerGUID
select @@rowcount

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentIDs    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveExperimentIDs

 AS

select Experiment_Info_ID from Experiment_Info
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInfoByID    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveExperimentInfoByID
@experimentInfoID int

AS

select Lab_Client_GUID,Lab_Server_GUID,Lab_Server_Name, Lab_Client_Version, Lab_Client_Name, 
Provider_Name,LSS_GUID 
from Experiment_Info where Experiment_Info_ID=@experimentInfoID

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInfoIDByExperiment    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveExperimentInfoIDByExperiment

@clientGuid varchar(50),
@labServerGuid varchar(50)

AS

select Experiment_Info_ID from Experiment_Info where Lab_Client_Guid=@clientGuid and Lab_Server_Guid = @labServerGuid
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLSSIDbyExperiment    Script Date: 07/05/2006 4:19:04 PM ******/

CREATE PROCEDURE RetrieveLSSIDbyExperiment 

@clientGuid varchar(50),
@labServerGuid varchar(50)

AS

Select LSS_GUID from Experiment_Info where Lab_Client_Guid = @clientGuid and Lab_Server_GUID = @labServerGuid

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLSSURLbyExperiment    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveLSSURLbyExperiment 
@clientGuid varchar(50),
@labServerGuid varchar(50)

AS

Select LSS_URL from Experiment_Info join LSS_Info on Experiment_Info.LSS_GUID = LSS_Info.LSS_GUID 
where Lab_Client_GUID = @clientGuid and Lab_Server_Guid = @labServerGuid
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.AddReservation    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE AddReservation
@userName nvarchar(256),
@startTime datetime,
@endTime  datetime,
@serviceBrokerGUID varchar(50),
@groupName nvarchar(256),
@experimentInfoID int

AS

DECLARE @credentialSetID int
select @credentialSetID =(select Credential_Set_ID from Credential_Sets where Group_Name=@groupName and Service_Broker_GUID = @serviceBrokerGUID)

insert into Reservations (User_Name, Start_Time, End_Time, Credential_Set_ID, Experiment_Info_ID)
	values (@userName,@startTime, @endTime, @credentialSetID, @experimentInfoID)

select ident_current('Reservations')

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddUssPolicy    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE AddUssPolicy
@experimentInfoID int,
@groupName nvarchar(256),
@rule varchar(2048),
@serviceBrokerGUID varchar(50)

AS

Declare @credentialSetID int

select @credentialSetID = (select Credential_Set_ID from Credential_Sets where Group_Name=@groupName and Service_Broker_GUID=@serviceBrokerGUID)

insert into USS_Policy(Experiment_Info_ID,  [Rule], Credential_Set_ID) values (@experimentInfoID,  @rule, @credentialSetID) 
select ident_current('USS_Policy')

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteReservation    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE DeleteReservation 
@reservationID int 

AS

delete from Reservations where Reservation_ID= @reservationID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteUSSPolicy    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE DeleteUSSPolicy
@ussPolicyID int

AS

delete from USS_Policy where USS_Policy_ID=@ussPolicyID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyReservation    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE ModifyReservation
@reservationID int,
@experimentInfoID int,
@startTime datetime,
@endTime datetime

AS

update Reservations set Start_Time=@startTime, End_Time=@endTime, Experiment_Info_ID=@experimentInfoID where Reservation_ID=@reservationID
select @@rowcount
return

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyUSSPolicy    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE ModifyUSSPolicy
@ussPolicyID int,
@experimentInfoID int,
@rule varchar(2048),
@credentialSetID int

AS

update USS_Policy set Experiment_Info_ID=@experimentInfoID, [Rule]=@rule, Credential_Set_ID=@credentialSetID where USS_Policy_ID= @ussPolicyID
select @@rowcount
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveReservationByID    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveReservationByID
@reservationID int

AS

select User_Name, Start_Time, End_Time, Credential_Set_ID, Experiment_Info_ID from Reservations where Reservation_ID=@reservationID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveReservationIDByUser    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveReservationIDByUser
@userName nvarchar(256),
@serviceBrokerGUID varchar(50),
@clientGuid varchar(50),
@labServerGuid varchar(50),
@timeUTC datetime

AS

declare @experimentInfoID int

select @experimentInfoID = (select Experiment_Info_ID from Experiment_Info 
where Lab_Client_Guid = @clientGuid and Lab_Server_Guid = @labServerGuid)


select Reservation_ID from Reservations  Join Credential_Sets  on Reservations.Credential_Set_ID = Credential_Sets.Credential_Set_ID 
	where Experiment_Info_ID=@experimentInfoID and [User_Name] = @userName and Service_Broker_GUID = @serviceBrokerGUID and Start_Time <= @timeUTC and End_Time > @timeUTC


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveReservationIDsByLabServer    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveReservationIDsByLabServer
@labServerGUID varchar(50),
@startTime datetime,
@endTime datetime

AS

select Reservation_ID from Reservations Join Experiment_Info on Reservations.Experiment_Info_ID = Experiment_Info.Experiment_Info_ID 
where Lab_Server_GUID = @labServerGUID and  Start_Time < @endTime and End_Time > @startTime
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveReservationIDsByUser    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveReservationIDsByUser
@userName nvarchar(256),
@serviceBrokerGUID varchar(50),
@experimentInfoID int

AS

select Reservation_ID from Reservations JOIN Credential_Sets ON Reservations.Credential_Set_ID = Credential_Sets.Credential_Set_ID where [User_Name] = @userName and Service_Broker_GUID = @serviceBrokerGUID and Experiment_Info_ID = @experimentInfoID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE RetrieveReservations
@start DateTime,
@end dateTime
AS
select Reservation_ID, User_Name, Start_Time, End_Time, Credential_Set_ID, Experiment_Info_ID 
from Reservations 
where End_Time>@start and Start_Time<@end
order by start_time
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE RetrieveReservationsByAll
@start DateTime,
@end dateTime,
@userName nvarchar(256),
@credSetID int,
@experInfoID int

AS
select Reservation_ID, User_Name, Start_Time, End_Time, Credential_Set_ID, Experiment_Info_ID 
from Reservations 
where Credential_Set_ID = @credSetId AND experiment_info_id = @experInfoID 
AND user_Name = @userName
and End_Time>@start and Start_Time<@end
order by start_time
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE RetrieveReservationsByExperiment
@start DateTime,
@end dateTime,
@experInfoID int
AS
select Reservation_ID, User_Name, Start_Time, End_Time, Credential_Set_ID, Experiment_Info_ID 
from Reservations 
where Experiment_Info_ID=@experInfoID 
and End_Time>@start and Start_Time<@end
order by start_time
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE RetrieveReservationsByGroup
@start DateTime,
@end dateTime,
@credSetID int

AS
select Reservation_ID, User_Name, Start_Time, End_Time, Credential_Set_ID, Experiment_Info_ID 
from Reservations 
where Credential_Set_ID =@credSetId 
and End_Time>@start and Start_Time<@end
order by start_time
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE RetrieveReservationsByGroupAndExperiment

@start DateTime,
@end dateTime,
@credSetID int,
@experInfoID int
AS
select Reservation_ID, User_Name, Start_Time, End_Time, Credential_Set_ID, Experiment_Info_ID 
from Reservations 
where Credential_Set_ID =@credSetId AND Experiment_Info_ID=@experInfoID 
and End_Time>@start and Start_Time<@end
order by start_time
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE RetrieveReservationsByUser
@start DateTime,
@end dateTime,
@userName nvarchar(256)


AS
select Reservation_ID, User_Name, Start_Time, End_Time, Credential_Set_ID, Experiment_Info_ID 
from Reservations 
where user_Name = @userName
and End_Time>@start and Start_Time<@end
order by start_time
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE RetrieveReservationsByUserAndGroup
@start DateTime,
@end dateTime,
@userName nvarchar(256),
@credSetID int

AS
select Reservation_ID, User_Name, Start_Time, End_Time, Credential_Set_ID, Experiment_Info_ID 
from Reservations 
where Credential_Set_ID = @credSetId AND user_Name = @userName
and End_Time>@start and Start_Time<@end
order by start_time
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
/****** Object:  Stored Procedure dbo.RetrieveUSSPolicyByID    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveUSSPolicyByID
@ussPolicyID int

AS

select  Experiment_Info_ID, [Rule], Credential_Set_ID  from USS_Policy where USS_Policy_ID= @ussPolicyID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUSSPolicyIDs    Script Date: 10/4/2006 12:39:23 PM ******/

CREATE PROCEDURE RetrieveUSSPolicyIDs AS

select USS_Policy_ID from USS_Policy

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUSSPolicyIDsByGroup    Script Date: 11/16/2005 4:19:04 PM ******/

CREATE PROCEDURE RetrieveUSSPolicyIDsByGroup

@groupName nvarchar(256),
@serviceBrokerGUID varchar(50)

AS

declare @credentialSetID int

select @credentialSetID=(select Credential_Set_ID from Credential_Sets 
where Group_Name=@groupName and Service_Broker_GUID = @serviceBrokerGUID)

select USS_Policy_ID from USS_Policy where Credential_Set_ID=@credentialSetID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUSSPolicyIDsByGroupandExp    Script Date: 10/4/2006 12:39:23 PM ******/

CREATE PROCEDURE RetrieveUSSPolicyIDsByGroupAndExp
@groupName nvarchar(256),
@serviceBrokerGUID varchar(50),
@clientGuid varchar(50),
@labServerGuid varchar(50)

AS

declare @credentialSetID int
declare @experimentInfoID int

select @credentialSetID=(select Credential_Set_ID from Credential_Sets
 where Group_Name=@groupName and Service_Broker_GUID = @serviceBrokerGUID)

select @experimentInfoID = (select Experiment_Info_ID from Experiment_Info
 where Lab_Client_Guid = @clientGuid and Lab_Server_Guid =@labServerGuid)

select USS_Policy_ID from USS_Policy where Credential_Set_ID=@credentialSetID and Experiment_Info_ID = @experimentInfoID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



