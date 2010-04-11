/****** Object:  Stored Procedure dbo.AddBlobToExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsSuperuser]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[IsSuperUser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getUserExperimentIDs]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getUserExperimentIDs]
GO

/****** Object:  Stored Procedure dbo.AddBlobToExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddBlobToExperimentRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddBlobToExperimentRecord]
GO

/****** Object:  Stored Procedure dbo.AddClientInfo    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddClientInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddClientInfo]
GO

/****** Object:  Stored Procedure dbo.AddExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddExperimentRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddExperimentRecord]
GO

/****** Object:  Stored Procedure dbo.AddGrant    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddGrant]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddGrant]
GO

/****** Object:  Stored Procedure dbo.AddGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddGroup]
GO

/****** Object:  Stored Procedure dbo.AddLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddLabClient]
GO

/****** Object:  Stored Procedure dbo.AddLabServerClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddLabServerClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddLabServerClient]
GO

/****** Object:  Stored Procedure dbo.AddMemberToGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddMemberToGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddMemberToGroup]
GO

/****** Object:  Stored Procedure dbo.AddQualifier    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddQualifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddQualifier]
GO

/****** Object:  Stored Procedure dbo.AddQualifierHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddQualifierHierarchy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddQualifierHierarchy]
GO

/****** Object:  Stored Procedure dbo.AddQualifier    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyQualifierName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyQualifierName]
GO

/****** Object:  Stored Procedure dbo.AddRecordAttribute    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddRecordAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddRecordAttribute]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.AddSBID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddSBID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddSBID]
GO

/****** Object:  Stored Procedure dbo.AddSystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddSystemMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddSystemMessage]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.AddToAgentHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddToAgentHierarchy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddToAgentHierarchy]
GO

/****** Object:  Stored Procedure dbo.AddUser    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddUser]
GO

/****** Object:  Stored Procedure dbo.AddUserLogin    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddUserLogin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddUserLogin]
GO

/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddUserSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddUserSession]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyUserSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyUserSession]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetSessionGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetSessionGroup]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetSessionClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetSessionClient]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetSessionKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetSessionKey]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetSessionTzOffset]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetSessionTzOffset]
GO

/****** Object:  Stored Procedure dbo.CloseExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CloseExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CloseExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CountServerClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CountServerClients]
GO

/****** Object:  Stored Procedure dbo.CreateBLOB    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateBLOB]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateBLOB]
GO

/****** Object:  Stored Procedure dbo.CreateExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateExperiment]
GO

/****** Object:  Stored Procedure dbo.CreateExperimentIndex    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateExperimentIndex]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateExperimentIndex]
GO

/****** Object:  Stored Procedure dbo.GetEssInfoForExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetEssInfoForExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetEssInfoForExperiment]
GO

/****** Object:  Stored Procedure dbo.CreateNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateNativePrincipal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateNativePrincipal]
GO

/****** Object:  Stored Procedure dbo.DeleteClientInfo    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteClientInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteClientInfo]
GO

/****** Object:  Stored Procedure dbo.DeleteClientItem    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteClientItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteClientItem]
GO

/****** Object:  Stored Procedure dbo.DeleteExperimentCoupon    Script Date: 12/12/2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteExperimentCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteExperimentCoupon]
GO

/****** Object:  Stored Procedure dbo.InsertExperimentCoupon    Script Date: 12/12/2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateExperimentStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateExperimentStatus]
GO

/****** Object:  Stored Procedure dbo.InsertExperimentCoupon    Script Date: 12/12/2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateExperimentStatusCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateExperimentStatusCode]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExperimentStatusCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExperimentStatusCode]
GO

/****** Object:  Stored Procedure dbo.InsertExperimentCoupon    Script Date: 12/12/2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertExperimentCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertExperimentCoupon]
GO
/****** Object:  Stored Procedure dbo.RetrieveExperimentCoupon    Script Date: 12/12/2006 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentCoupon]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentCouponID    Script Date: 12/12/2006 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentCouponID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentCouponID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentAdminInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentAdminInfo]
GO

/****** Object:  Stored Procedure dbo.DeleteExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteExperiment]
GO

/****** Object:  Stored Procedure dbo.DeleteExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteExperimentInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteExperimentInformation]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.DeleteExperimentInformation_rb    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteExperimentInformation_rb]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteExperimentInformation_rb]
GO

/****** Object:  Stored Procedure dbo.DeleteGrant    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteGrant]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteGrant]
GO

/****** Object:  Stored Procedure dbo.DeleteGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteGroup]
GO

/****** Object:  Stored Procedure dbo.DeleteLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteLabClient]
GO

/****** Object:  Stored Procedure dbo.DeleteLabServerClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteLabServerClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteLabServerClient]
GO

/****** Object:  Stored Procedure dbo.DeleteMemberFromGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteMemberFromGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteMemberFromGroup]
GO

/****** Object:  Stored Procedure dbo.NUMERICClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NUMERICClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[NUMERICClient]
GO

/****** Object:  Stored Procedure dbo.DeletePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeletePrincipal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeletePrincipal]
GO

/****** Object:  Stored Procedure dbo.DeleteNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteNativePrincipal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteNativePrincipal]
GO

/****** Object:  Stored Procedure dbo.DeleteQualifier    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteQualifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteQualifier]
GO

/****** Object:  Stored Procedure dbo.DeleteQualifierHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteQualifierHierarchy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteQualifierHierarchy]
GO

/****** Object:  Stored Procedure dbo.DeleteRecordAttribute    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteRecordAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteRecordAttribute]
GO

/****** Object:  Stored Procedure dbo.DeleteSystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteSystemMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteSystemMessage]
GO

/****** Object:  Stored Procedure dbo.DeleteSystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteSystemMessageByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteSystemMessageByID]
GO

/****** Object:  Stored Procedure dbo.DeleteSystemMessages    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteSystemMessages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteSystemMessages]
GO

/****** Object:  Stored Procedure dbo.DeleteUser    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteUser]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.GetBlobAccess    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBlobAccess]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBlobAccess]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.GetBlobAssociation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBlobAssociation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBlobAssociation]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.GetBlobStorage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBlobStorage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBlobStorage]
GO

/****** Object:  Stored Procedure dbo.GetExperimentIdleTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCollectionCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCollectionCount]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.GetExperimentIdleTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExperimentIdleTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExperimentIdleTime]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.FindExperiments    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FindExperiments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[FindExperiments]
GO

/****** Object:  Stored Procedure dbo.ModifyExperimentOwner    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyExperimentOwner]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyExperimentOwner]
GO

/****** Object:  Stored Procedure dbo.ModifyExperimentStatus    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyExperimentStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyExperimentStatus]
GO

/****** Object:  Stored Procedure dbo.ModifyGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyGroup]
GO

/****** Object:  Stored Procedure dbo.ModifyLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyLabClient]
GO

/****** Object:  Stored Procedure dbo.ModifySystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifySystemMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifySystemMessage]
GO

/****** Object:  Stored Procedure dbo.ModifyUser    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyUser]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.ResetAgentHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResetAgentHierarchy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ResetAgentHierarchy]
GO


/****** Object:  Stored Procedure dbo.RetrieveAgent    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgent]
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgentGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgentGroup]
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentHierarchyTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgentHierarchyTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgentHierarchyTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentType    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgentType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgentType]
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentsTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgentsTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgentsTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveAssociatedGroupID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAssociatedGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAssociatedGroupID]
GO

/****** Object:  Stored Procedure dbo.RetrieveBlobAccess    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveBlobAccess]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveBlobAccess]
GO

/****** Object:  Stored Procedure dbo.RetrieveBlobAssociation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveBlobAssociation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveBlobAssociation]
GO

/****** Object:  Stored Procedure dbo.RetrieveBlobStorage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveBlobStorage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveBlobStorage]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveExecutionError    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExecutionError]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExecutionError]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveExecutionWarnings    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExecutionWarnings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExecutionWarnings]
GO

/****** Object:  Stored Procedure dbo.RetrieveBLOBsForExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveBLOBsForExperimentRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveBLOBsForExperimentRecord]
GO

/****** Object:  Stored Procedure dbo.RetrieveClientInfo    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveClientInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveClientInfo]
GO

/****** Object:  Stored Procedure dbo.RetrieveClientItem    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveClientItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveClientItem]
GO

/****** Object:  Stored Procedure dbo.RetrieveClientItemNames    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveClientItemNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveClientItemNames]
GO

/****** Object:  Stored Procedure dbo.RetrieveClientServerIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveClientServerIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveClientServerIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperiment]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentRawData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentRawData]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentAnnotation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentAnnotation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentAnnotation]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentGroup]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentIdleTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentIdleTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentIdleTime]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentInfo]
GO
/****** Object:  Stored Procedure dbo.RetrieveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentInformation]
GO


/****** Object:  Stored Procedure dbo.RetrieveAuthorizedExpIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAuthorizedExpIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformat    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveActiveExpIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveActiveExpIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformat    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAuthorizedExpIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAuthorizedExpIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformat    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAuthorizedExpIDsCriteria]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAuthorizedExpIDsCriteria]
GO



/****** Object:  Stored Procedure dbo.RetrieveExperimentOwner    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentOwner]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentOwner]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentRecord]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentAdminInfos]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentAdminInfos]
GO
/****** Object:  Stored Procedure dbo.RetrieveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentSummary]
GO
/****** Object:  Stored Procedure dbo.RetrieveGrantsTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGrantsTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGrantsTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserGroups]
GO
/****** Object:  Stored Procedure dbo.RetrieveGroupAdminGroupID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupAdminGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupAdminGroupID]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupExperimentIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupExperimentIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupExperimentIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveAdminGroupIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAdminGroupIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAdminGroupIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupID]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupName   Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupName]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupMembers    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupMembers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupMembers]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupRequestGroupID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupRequestGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupRequestGroupID]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClient]
GO
/****** Object:  Stored Procedure dbo.RetrieveLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientByGuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientByGuid]
GO
/****** Object:  Stored Procedure dbo.RetrieveLabClientIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientID]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientTypes    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientTypes]
GO

/****** Object:  Stored Procedure dbo.RetrieveNativePassword    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveNativePassword]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveNativePassword]
GO

/****** Object:  Stored Procedure dbo.RetrieveNativePrincipals    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveNativePrincipals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveNativePrincipals]
GO

/****** Object:  Stored Procedure dbo.RetrieveOrphanedUserIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveOrphanedUserIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveOrphanedUserIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveQualifierHierarchyTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveQualifierHierarchyTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveQualifierHierarchyTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveQualifiersTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveQualifiersTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveQualifiersTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveRecordAttributeByID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveRecordAttributeByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveRecordAttributeByID]
GO

/****** Object:  Stored Procedure dbo.RetrieveRecordAttributeByName    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveRecordAttributeByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveRecordAttributeByName]
GO

/****** Object:  Stored Procedure dbo.RetrieveRecordsForExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveRecordsForExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveRecordsForExperiment]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveSBIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSBIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSBIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessageByID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSystemMessageByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSystemMessageByID]
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessageByIDForAdmin    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSystemMessageByIDForAdmin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSystemMessageByIDForAdmin]
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessages    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSystemMessages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSystemMessages]
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessagesForAdmin    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSystemMessagesForAdmin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSystemMessagesForAdmin]
GO

/****** Object:  Stored Procedure dbo.RetrieveUser    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUser]
GO

/****** Object:  Stored Procedure dbo.RetrieveUserEmail    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserEmail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserEmail]
GO

/****** Object:  Stored Procedure dbo.RetrieveUserID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserID]
GO

/****** Object:  Stored Procedure dbo.RetrieveUserName    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserName]
GO
/****** Object:  Stored Procedure dbo.RetrieveUserIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserIDs]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveValidationError    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveValidationError]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveValidationError]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveValidationWarnings    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveValidationWarnings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveValidationWarnings]
GO

/****** Object:  Stored Procedure dbo.SaveBlobXMLExtensionSchemaURL    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveBlobXMLExtensionSchemaURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveBlobXMLExtensionSchemaURL]
GO

/****** Object:  Stored Procedure dbo.SaveClientItem    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveClientItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveClientItem]
GO

/****** Object:  Stored Procedure dbo.SaveExperimentAnnotation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveExperimentAnnotation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveExperimentAnnotation]
GO

/****** Object:  Stored Procedure dbo.SaveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveExperimentInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveExperimentInformation]
GO

/****** Object:  Stored Procedure dbo.SaveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateEssInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateEssInfo]
GO

/****** Object:  Stored Procedure dbo.SaveGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveGroup]
GO

/****** Object:  Stored Procedure dbo.SaveLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveLabClient]
GO
/****** Object:  Stored Procedure dbo.SaveNativePassword    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveNativePassword]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveNativePassword]
GO

/****** Object:  Stored Procedure dbo.SaveNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveNativePrincipal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveNativePrincipal]
GO

/****** Object:  Stored Procedure dbo.SaveResultXMLExtensionSchemaURL    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveResultXMLExtensionSchemaURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveResultXMLExtensionSchemaURL]
GO

/****** Object:  Stored Procedure dbo.SaveUserLogoutTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveUserLogoutTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveUserLogoutTime]
GO

/****** Object:  Stored Procedure dbo.SaveUserSessionEndTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveUserSessionEndTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveUserSessionEndTime]
GO

/****** Object:  Stored Procedure dbo.SelectAllUserSessions    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectAllUserSessions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectAllUserSessions]
GO

/****** Object:  Stored Procedure dbo.SelectUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectUserSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectUserSession]
GO
/****** Object:  Stored Procedure dbo.SelectUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectSessionInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectSessionInfo]
GO
/****** Object:  Stored Procedure dbo.SelectUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateEssInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateEssInfo]
GO

/****** Object:  Stored Procedure dbo.GetAdminProcessAgentTags    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAdminProcessAgentTags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAdminProcessAgentTags]
GO

/****** Object:  Stored Procedure dbo.GetAdminProcessAgentTags    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAdminServiceTags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAdminServiceTags]
GO

/****** Object:  Stored Procedure dbo.GetProcessAgentAdminGrants    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetProcessAgentAdminGrants]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetProcessAgentAdminGrants]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteAdminURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteAdminURL]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteAdminURLbyID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteAdminURLbyID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertAdminURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertAdminURL]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyAdminURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyAdminURL]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyAdminUrlCodebase]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyAdminUrlCodebase]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyClientCodebase]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyClientCodebase]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAdminURLs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAdminURLs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceTypeStrings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceTypeStrings]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddResourceMappingKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddResourceMappingKey]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddResourceMappingValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddResourceMappingValue]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddResourceMappingString]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddResourceMappingString]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateResourceMappingString]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateResourceMappingString]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddResourceMappingResourceType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddResourceMappingResourceType]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceIDsByKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceIDsByKey]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceMappingByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceMappingByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceTypeString]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceTypeString]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceStringByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceStringByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceTypeByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceTypeByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceMappingIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceMappingIDs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetMappingIdByKeyValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetMappingIdByKeyValue]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceMapIdsByValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceMapIdsByValue]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceTypeNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceTypeNames]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetMappingStringTag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetMappingStringTag]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertResourceMappingKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertResourceMappingKey]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertResourceMappingValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertResourceMappingValue]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteResourceMapping]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteResourceMapping]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertRegistration]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertRegistration]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistrations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistrations]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistrationRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistrationRecord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistration]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistration]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistrationByStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistrationByStatus]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistrationByRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistrationByRange]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateRegistrationStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateRegistrationStatus]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLoaderScript]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLoaderScript]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetLoaderScript]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetLoaderScript]
GO


create Function IsSuperuser(@userid int,@groupid int)
returns bit
as
BEGIN
declare @status bit 
SET @status = 0 
if (select user_id from users where user_name ='superUser') = @userid
BEGIN
SET @status = 1
END
if (select group_ID from groups where group_name = 'SuperUserGroup') = @groupid
BEGIN
SET @status =1
END
return @status
END
GO

create function getUserExperimentIDs(@userid int,@groupid int)
returns table
AS
Return ( SELECT  experiment_ID from experiments
WHERE (USER_ID = @userID) or group_id in
(select qualifier_reference_ID from qualifiers
where qualifier_type_id = 6 
and Qualifier_id in (select qualifier_id from grants 
where agent_id = @groupid and function_id = 5))

)
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/* CREATE STORED PROCEDURES */



/****** Object:  Stored Procedure dbo.AddClientInfo    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddClientInfo
	@labClientID int,
	@infoURL nvarchar (512),
	@infoName nvarchar(256),
	@description nvarchar(2048),
	@displayOrder int
AS
	insert into client_info (client_id,info_url,info_name,description, display_order)
		values (@labClientID, @infoURL,  @infoName, @description, @displayOrder)
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.AddGrant    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddGrant
	@agentID int,
	@functionName varchar(128),
	@qualifierID int
AS
	DECLARE @functionID int
	
	select @functionID = (select function_id from functions where 
								upper(function_name)=upper(@functionName))
	insert into grants(agent_ID, function_ID,qualifier_ID)
	values (@agentID,@functionID,@qualifierID)

	select ident_current('grants')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddGroup    Script Date: 5/18/2005 4:17:55 PM ******/
CREATE PROCEDURE AddGroup
	@groupName nvarchar(256),
	@description nvarchar(2048),
	@email nvarchar(256),
	@parentGroupID int, 
	@groupType varchar(256),
	@associatedGroupID int
AS
BEGIN TRANSACTION
	DECLARE @agentID int
	DECLARE @groupTypeID int

	select @groupTypeID = (select  group_type_id from group_types where description=@groupType);	
	
	insert into agents (agent_name, is_group)
	values (@groupName, 1)
	if (@@error > 0)
		goto on_error
	select @agentID=(select ident_current('Agents'))

	/* Assume that the parent group id here is a legal value. 
	Any corrections for -1 will be done in API code */
	insert into  agent_hierarchy (parent_group_ID, agent_ID)
	values (@parentGroupID, @agentID)
	if (@@error > 0)
		goto on_error
	
	insert into groups (group_id, group_name,description,email, group_type_id, associated_group_id)
	values (@agentID, @groupname, @description,@email, @groupTypeID, @associatedGroupID)
	if (@@error > 0)
		goto on_error

COMMIT TRANSACTION
	select @agentID
return
	on_error: 
	ROLLBACK TRANSACTION
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddLabClient    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddLabClient
	@guid varchar(50),
	@labClientName nvarchar(256),
	@shortDescription nvarchar(256),
	@longDescription ntext,
	@version nvarchar(50),
	@loaderScript nvarchar(2000),
	@clientType varchar (100),
	@email nvarchar(256),
	@firstName nvarchar(128),
	@lastName nvarchar(128),
	@notes nvarchar(2048),
	@needsScheduling bit,
	@needsESS bit,
	@isReentrant bit
AS
		DECLARE @clientTypeID INT
		
		SELECT @clientTypeID = (SELECT client_type_id FROM client_types 
							WHERE upper(description) = upper(@clientType))
							
		INSERT INTO lab_clients (Client_Guid, lab_client_name, short_description, long_description,version, 
				loader_script, client_type_ID, contact_email, contact_first_name, 
				contact_last_name, notes,NeedsScheduling,NeedsESS,IsReentrant)
		VALUES (@guid, @labClientName, @shortDescription, @longDescription, @Version,
				@loaderScript,@clientTypeID, @email, @firstName, @lastName, @notes,
				@needsScheduling,@needsESS,@isReentrant)
		
		SELECT ident_current('lab_clients');
RETURN
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetLoaderScript
	@id int
AS
	SELECT loader_script from lab_clients where client_ID = @id
return
GO

CREATE PROCEDURE setLoaderScript
	@id int,
	@script nvarchar(2000)
AS
	UPDATE loader_script set loader_script = @script  where client_ID = @id
return
GO
/****** Object:  Stored Procedure dbo.AddLabServerClient    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddLabServerClient
	@labClientID int,
	@labServerID int,
	@displayOrder int
AS
	insert into lab_server_to_client_map (client_id,agent_id, display_order)
	values (@labClientID, @labServerID, @displayOrder)
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddMemberToGroup    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddMemberToGroup
	@groupID int,
	@memberID int
AS
BEGIN TRANSACTION
	DECLARE @isGroup bit;
	DECLARE @orphanedGroupID int;
	DECLARE @newUserGroupID int;
	DECLARE @rootGroupID int

	select @isGroup = (select is_group from agents  where (agent_id=@memberID) );
	select @rootGroupID = (select group_id from groups where group_name = 'ROOT')

	begin
		if (@isGroup = 0)
		begin
			if (@groupID!=@rootGroupID) -- not trying to transfer to root
			begin 
				-- if agent is member of orphaned user group then delete them from it.
				select @orphanedGroupID = (select group_id from groups where group_name = 'OrphanedUserGroup');

				delete from agent_hierarchy 
				where agent_id= @memberID and parent_group_id = @orphanedGroupID;
				if (@@error > 0)
					goto on_error

				-- add to other group
				insert into agent_hierarchy (agent_id, parent_group_id)
				values (@memberID, @groupID);
				if (@@error > 0)
					goto on_error

			end
		end
		else 	/*If group then set qualifiier parents appropriately */
		begin
			DECLARE @groupQualifierID int;
			DECLARE @parentGroupQualifierID int;
			DECLARE @ECQualifierID int;
			DECLARE @parentECQualifierID int;

			DECLARE @rootQualifierID int;
			select @rootQualifierID = (select qualifier_id from Qualifiers where qualifier_name = 'ROOT');


			-- set group qualifiers
			select @groupQualifierID = (select qualifier_id from Qualifiers where Qualifier_reference_ID=@memberID 
							and Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Group') );

			-- if added to root group then set parent qualifier to root	
			if (@groupID = @rootGroupID)
				select @parentGroupQualifierID = @rootQualifierID
			else
				select @parentGroupQualifierID = (select qualifier_id from Qualifiers where Qualifier_reference_ID=@groupID 
							and Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Group') );

			insert into qualifier_hierarchy (qualifier_ID, parent_qualifier_ID)
			values (@groupQualifierID, @parentGroupQualifierID);
			if (@@error > 0)
				goto on_error
			
			-- set experiment qualifiers
			select @ECQualifierID = (select qualifier_id from Qualifiers where Qualifier_reference_ID=@memberID 
							and Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection') );
						
			-- if added to root group then set experiment collection qualifier parent to root	
			if (@groupID = @rootGroupID)
				select @parentECQualifierID = @rootQualifierID
			else
				select @parentECQualifierID = (select qualifier_id from Qualifiers where Qualifier_reference_ID=@groupID 
					and Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection') );
			
			insert into qualifier_hierarchy (qualifier_ID, parent_qualifier_ID)
			values (@ECQualifierID, @parentECQualifierID);
			if (@@error > 0)
				goto on_error
			
			insert into  agent_hierarchy (parent_group_ID, agent_ID)
			values (@groupID, @memberID);
			if (@@error > 0)
				goto on_error
		-- This is an insert and  NOT an update because agents can belong to multiple groups
		end
	end

COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddQualifier    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddQualifier
	@qualifierTypeID int,
	@qualifierReferenceID int,
	@qualifierName nvarchar (512)
AS
	/*DECLARE @qualifierTypeID int
	select @qualifierTypeID = (select qualifier_type_id from qualifier_Types where 
				upper(description) = upper(@qualifierType))*/
	insert into qualifiers(qualifier_Type_id, qualifier_reference_id, qualifier_name)
	values (@qualifierTypeID,@qualifierReferenceID, @qualifierName)
	
	select ident_current('qualifiers')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddQualifierHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddQualifierHierarchy
	@parentQualifierID int,
	@qualifierID int
AS
	
	insert into  qualifier_hierarchy (parent_qualifier_ID, qualifier_ID)
	values (@parentQualifierID, @qualifierID)
-- This is an insert and  NOT an update because qualifiers can have multiple parents
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ModifyQualifierName
@qualifierTypeID int,
@refID int,
@newName nvarchar(512)

AS

UPDATE Qualifiers SET Qualifier_name = @newName
where Qualifier_Type_ID =@qualifierTypeID and Qualifier_Reference_ID = @refID
select @@rowcount

GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddSystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddSystemMessage
	@messageType varchar(256),
	@messageTitle nvarchar(256),
	@toBeDisplayed bit,
	@groupID int,
	@clientID int,
	@agentID int,
	@messageBody nvarchar(3000)

AS 
	DECLARE @messageTypeID int

	select @messageTypeID = (select message_type_id from message_types 
							where upper(description) = upper(@messageType))
	insert into System_Messages (message_type_id, to_be_displayed, agent_id, client_ID, group_id, 
		message_body, message_title, date_created,last_modified)
		values (@messageTypeID, @toBeDisplayed, @agentID, @clientID, @groupID, @messageTitle, @messageBody,
			getUtcdate(),getUtcdate())
	
	select ident_current('system_messages')
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddUser    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddUser
	@userName nvarchar(256),
	@firstName nvarchar(256),
	@lastName nvarchar(256),
	@email nvarchar(256),
	@affiliation nvarchar(256),
	@reason nvarchar(2048),
	@XMLExtension text,
	@lockUser bit,
	@principalString nvarchar (256),
	@authenType varchar(256),
	@initialGroupID int
AS
	DECLARE @authTypeID int
	DECLARE @agentID int
	--DECLARE @parentGroupID int
	DECLARE @userID int

BEGIN TRANSACTION
	
	begin
		insert into agents (agent_name, is_group)
		values (@userName, 0)
		if (@@error > 0)
			goto on_error
		select @agentID=(select ident_current('Agents'))

		--select @parentGroupID = (select group_id from groups 
		--						where upper(group_name)=upper(@initialGroupID))
		insert into  agent_hierarchy (parent_group_ID, agent_ID)
		values (@initialGroupID, @agentID)
		if (@@error > 0)
			goto on_error
	
		insert into users (user_id, user_name,first_name,last_name,email, 
							affiliation, signup_reason, XML_Extension, lock_user)
		values (@agentID, @userName, @firstName,  @lastName, @email, 
				@affiliation, @reason, @XMLExtension, @lockUser)
		if (@@error > 0)
			goto on_error
		
		select @authTypeID = (select auth_type_id from authentication_types 
							where upper(description)=upper(@authenType))
		insert into principals (user_id, principal_string, auth_type_ID)
		values (@agentID,@principalString, @authTypeID)
		if (@@error > 0)
			goto on_error
	end
COMMIT TRANSACTION	
		
		select @agentID
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddUserSession
	@userID int,
	@groupID int,
	@tzOffset int,
	@sessionKey varchar(512)
AS 
	insert into user_sessions (modify_time,user_id, effective_group_id,TZ_Offset, session_key)
		values (getutcdate(), @userID, @groupID, @tzOffset, @sessionKey )
	
	select ident_current('user_sessions')
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ModifyUserSession
	@sessionID bigint,
	@groupID int,
	@clientID int,
	@tzOffset int,
	@sessionKey varchar(512)
AS 
	update user_sessions set modify_time =getutcdate(), effective_group_id = @groupID,
 		client_ID=@clientID,TZ_Offset=@tzOffset,session_Key=@sessionKey WHERE session_ID = @sessionID
	
	
return @@rowcount
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE SetSessionGroup
	@sessionid bigint,
	@groupID int
	
AS 
	update user_sessions set modify_time =getutcdate(), effective_group_id = @groupID
 		WHERE session_ID = @sessionID
return @@rowcount
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE SetSessionClient
	@sessionid bigint,
	@clientID int
AS 
	update user_sessions set modify_time =getutcdate(),
 		client_ID=@clientID WHERE session_ID = @sessionID
return @@rowcount
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SetSessionKey
	@sessionID bigint,
	@sessionKey varchar(512)
AS 
	update user_sessions set modify_time =getutcdate(),
 		session_key=@sessionKey WHERE session_ID = @sessionID
return @@rowcount
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SetSessionTZOffset
	@sessionid bigint,
	@tzOffset int
AS 
	update user_sessions set modify_time =getutcdate(),
 		TZ_Offset=@tzOffset WHERE session_ID = @sessionID
return @@rowcount
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.CloseExperiment    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE CloseExperiment 
	@experimentID BigInt,
	@status int
AS
BEGIN TRANSACTION
	
	 update experiments set closeTime = getUtcdate(), status = @status where experiment_id = @experimentID
	IF (@@ERROR <> 0) goto on_error

	

COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE CountServerClients
@groupID int,
@serverID int
AS
select count(client_id) from Lab_Server_To_Client_map where agent_id = @serverID and client_ID in
(select qualifier_reference_ID from Qualifiers where qualifier_id in
(select qualifier_ID from grants where agent_id = @groupID 
and function_id = (select function_ID from functions where Function_Name = 'useLabClient')))

return
GO

/****** Object:  Stored Procedure dbo.CreateExperiment    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE CreateExperiment

	@status int, 
	@user int,
	@group int,
	@ls int,
	@client int,
	@ess int,
	@start datetime,
	@duration bigint
AS
	insert into Experiments (status, User_ID, Group_ID, Agent_ID,
		Client_ID, ESS_ID, ScheduledStart, duration, CreationTime)
	values (@status, @user, @group, @ls, @client, @ess, @start, @duration, getUtcdate())

	select ident_current('experiments')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetEssInfoForExperiment
@experimentID bigint
AS
declare @essid int
select @essid=ess_id from experiments where experiment_ID = @experimentID
if @essid is Null or @essid <= 0
BEGIN
	return 0
END
ELSE
BEGIN
select Agent_ID, Agent_GUID,  Agent_Name, ProcessAgent_Type_ID,
Codebase_URL, WebService_URL,  Domain_Guid, issuer_GUID, 
IdentIn_ID, (Select passkey from coupon where ProcessAgent.Issuer_Guid != Null AND  coupon_id = IdentIn_ID AND ProcessAgent.Issuer_GUID = Coupon.Issuer_GUID), 
IdentOut_ID, (Select passkey from coupon where  ProcessAgent.Issuer_Guid != Null AND coupon_id = IdentOut_ID AND ProcessAgent.Issuer_GUID = Coupon.Issuer_GUID),
retired
from ProcessAgent
where Agent_ID = @essID
return 0
END
GO

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE UpdateExperimentStatus
	@experimentID bigint,
	@status int,
	@closeTime DateTime,
	@recordCount int
AS

update experiments set status=@status, CloseTime=@closeTime,Record_count=@recordCount
where Experiment_ID=@experimentID

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE UpdateExperimentStatusCode
	@experimentID bigint,
	@status int
AS

update experiments set status=@status
where Experiment_ID=@experimentID
select @@rowcount
return
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.CreateNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE CreateNativePrincipal
	@userName nvarchar(256)
AS
	DECLARE @authTypeID int
	DECLARE @userID int
	
	select @authTypeID=(select auth_type_id from authentication_types where
					upper(description) = 'NATIVE')	
	select @userID = (select user_ID from users where user_name=@userName)
	
	-- since no principal string is specified in authen api, inserting the username as principal string
	insert into Principals (user_ID, auth_type_id, principal_string)
	values (@userID,@authTypeID, @userName)
	
	select @userID
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteClientInfo    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteClientInfo
	@labClientID int

AS
	delete from client_info 
		where client_id=@labClientID
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteClientItem    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteClientItem
	@clientID int,
	@userID int,
	@itemName nvarchar(256)
AS
		delete from client_items 
		where client_id = @ClientID and user_ID=@userID and item_Name = @itemName

return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteExperimentCoupon    Script Date: 12/12/2006 P******/

CREATE PROCEDURE DeleteExperimentCoupon
	@experimentID BigInt,
	@couponID BigInt
AS
	delete from ExperimentCoupon
	where Experiment_ID = @experimentID and Coupon_ID = @couponID;

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.InsertExperimentCoupon    Script Date: 12/12/2006 P******/

CREATE PROCEDURE InsertExperimentCoupon
	@experimentID BigInt,
	@couponID BigInt
AS
	insert into ExperimentCoupon (Experiment_ID, Coupon_ID) 
	values ( @experimentID,@couponID );

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
/****** Object:  Stored Procedure dbo.DeleteExperimentCoupon    Script Date: 12/12/2006 P******/

CREATE PROCEDURE RetrieveExperimentCouponID
	@experimentID BigInt
	
AS
	select Coupon_ID from ExperimentCoupon
	where Experiment_ID = @experimentID;

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteExperimentCoupon    Script Date: 12/12/2006 P******/

CREATE PROCEDURE RetrieveExperimentCoupon
	@experimentID BigInt
	
AS
declare @couponId bigint
	select @couponId = Coupon_ID from ExperimentCoupon
	where Experiment_ID = @experimentID;
	if @couponId > 0
	begin
	select cancelled, coupon_ID, passkey from IssuedCoupon where coupon_ID = @couponId
	end
else
return

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.DeleteExperiment    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteExperiment
	@experimentID BigInt
AS
	delete from Experiments
	where Experiment_ID = @experimentID;

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO






/****** Object:  Stored Procedure dbo.DeleteGrant    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteGrant
	@grantID int
AS
	delete from Grants
	where Grant_ID = @grantID;
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteGroup    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteGroup
	@groupID int
AS
BEGIN TRANSACTION
	delete from Agents 
	where agent_id = @groupID
	if (@@error > 0)
		goto on_error

	delete from Agent_Hierarchy
	where Parent_Group_ID = @groupID;
	if (@@error > 0)
		goto on_error
-- AgentId is taken care of by referential integrity
	
	delete from Groups
	where Group_ID = @groupID;
	if (@@error > 0)
		goto on_error
		
	delete from Qualifiers
	where Qualifier_Reference_ID = @groupID and 
		Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Group')
	if (@@error > 0)
		goto on_error
		
	delete from Qualifiers
	where Qualifier_Reference_ID = @groupID and 
		Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection')
		
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteLabClient    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteLabClient
	@labClientID int
AS
BEGIN TRANSACTION
	delete from Lab_Clients
	where Client_ID = @labClientID;
	if (@@error > 0)
		goto on_error
	
	delete from Qualifiers
	where qualifier_reference_ID = @labClientID and 
	qualifier_type_ID = (select qualifier_type_ID from qualifier_Types 
				where description = 'Lab Client');
	if (@@error > 0)
		goto on_error
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteLabServerClient    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteLabServerClient
	@labClientID int
AS
	delete from lab_server_to_client_map
	where client_id = @labClientID
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteMemberFromGroup    Script Date: 5/18/2005 4:17:55 PM ******/
CREATE PROCEDURE DeleteMemberFromGroup
	@groupID int,
	@memberID int
AS
BEGIN TRANSACTION
	DECLARE @isGroup bit;
	select @isGroup = (select is_group from agents
				 where (agent_id=@memberID) )

	DECLARE @rootGroupID int
	select @rootGroupID = (select group_id from groups where group_name = 'ROOT')
	
	if (@isGroup = 0) /* if user */
	begin
		/* Get Orphaned user group ID */
		DECLARE @orphanedGroupID int;
		select @orphanedGroupID = (select group_id from groups where group_name = 'OrphanedUserGroup');
		
		/* If user only belongs to the Orphaned Users Group - delete from system */
 		if( (@groupID=@orphanedGroupID) 
 			and  ((select count( agent_id) from agent_hierarchy where agent_id=@memberID and parent_group_id=@groupID)  = 1)
 			and  ((select count( agent_id) from agent_hierarchy where agent_id=@memberID)  = 1))

		begin
			delete from users where user_id=@memberID;
			if (@@error > 0)
				goto on_error

			delete from agents where agent_id=@memberID;
			if (@@error > 0)
				goto on_error

		end
		else
			/*check parents of member*/
			if ((select count( parent_group_id) from agent_hierarchy where agent_id=@memberID)>1)
				/* multiple parents*/
				delete from agent_hierarchy where agent_id=@memberID and parent_group_id=@groupID
			else
			begin
				/* if single parent */

				-- single parent cannot be root
				if (@groupID != @rootGroupID)
					update agent_hierarchy 
					set parent_group_ID = @orphanedGroupID
					where parent_group_ID = @groupID and  agent_ID = @memberID;
			end
	end
	else
	if (@isGroup = 1) /* Group */
	begin
		DECLARE @rootQualifierID int;
		select @rootQualifierID = (select qualifier_id from Qualifiers where qualifier_name = 'ROOT');

		/* get group qualifier */
		DECLARE @qualifierID int;
		select @qualifierID = (select qualifier_id from Qualifiers where qualifier_reference_id=@memberID 
					and qualifier_type_id  = (select Qualifier_Type_ID from Qualifier_Types where description='Group'))
		
		
		/* get experiment collection qualifier */
		DECLARE @experimentCollectionQualifierID int;
		select @experimentCollectionqualifierID = (select qualifier_id from Qualifiers where qualifier_reference_id=@memberID 
					and qualifier_type_id  = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection'))

		DECLARE @parentQualifierID int;
		DECLARE @parentECQualifierID int;

		-- if being removed from root
		if (@groupID = @rootGroupID)
		begin
			select @parentQualifierID = @rootQualifierID;
			select @parentECQualifierID = @rootQualifierID;
		end
		else
		begin
			/* get parent qualifier */
			select @parentQualifierID = (select qualifier_id from Qualifiers where qualifier_reference_id=@groupID 
				and qualifier_type_id  = (select Qualifier_Type_ID from Qualifier_Types where description='Group'))

			/* get parent experiment collection qualifier */
			select @parentECQualifierID = (select qualifier_id from Qualifiers where qualifier_reference_id=@groupID 
				and qualifier_type_id  = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection'))
		end
		

		/*check parents of agent*/
		if ((select count( parent_group_id) from agent_hierarchy where agent_id=@memberID)>1)
		
		/* multiple parents - delete relationships*/
		begin
			delete from agent_hierarchy where agent_id=@memberID and parent_group_id=@groupID
			if (@@error > 0)
				goto on_error
				
			delete from qualifier_hierarchy where qualifier_id = @qualifierID and parent_qualifier_id=@parentQualifierID
			if (@@error > 0)
				goto on_error
				
			delete from qualifier_hierarchy where qualifier_id = @experimentCollectionQualifierID and parent_qualifier_id=@parentECQualifierID
			if (@@error > 0)
				goto on_error
		end
		else
		/* single parent - move all to ROOT */
		-- single parent cannot be root
		if (@groupID != @rootGroupID)
		begin
			update agent_hierarchy 
			set parent_group_ID = @rootGroupID
			where parent_group_ID = @groupID and  agent_ID = @memberID;
			if (@@error > 0)
				goto on_error
				
			update qualifier_hierarchy
			set parent_qualifier_ID = @rootQualifierID
			where parent_qualifier_ID = @parentQualifierID and qualifier_ID=@qualifierID
			if (@@error > 0)
				goto on_error
				
			update qualifier_hierarchy
			set parent_qualifier_ID = @rootQualifierID
			where parent_qualifier_ID = @parentECQualifierID and qualifier_ID=@experimentCollectionQualifierID
			if (@@error > 0)
				goto on_error
		end
	end
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.DeleteNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteNativePrincipal
	@userID int
AS
	DECLARE @NativeTypeID int;
	
	select @NativeTypeID = (select auth_type_id from Authentication_Types where upper(description)='NATIVE')
	
	delete from Principals
	where user_ID = @userID and auth_type_id=@NativeTypeID;
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteQualifier    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteQualifier
	@qualifierID int
AS
	delete from Qualifiers
	where Qualifier_ID = @qualifierID;
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteQualifierHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteQualifierHierarchy
	@parentQualifierID int,
	@qualifierID int
AS
	delete from  qualifier_hierarchy 
	where parent_qualifier_ID = @parentQualifierID and qualifier_ID = @qualifierID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.DeleteSystemMessageByID    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteSystemMessageByID
	@messageID int
AS
	delete from System_Messages
	where System_Message_ID = @messageID;
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteSystemMessages    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteSystemMessages
	@messageType varchar(256),
	@clientID int,
	@groupID int,
	@agentID int

/* Delete by message type & group/lab server */
AS
	DECLARE @messageTypeID int

	select @messageTypeID = (select message_type_id from message_types 
							where upper(description) = upper(@messageType))
							
	delete from System_Messages
	where message_type_id = @messageTypeID and group_ID = @groupID and agent_ID=@agentID and client_ID = @clientID;

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteUser    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteUser
	@userID int
AS
BEGIN TRANSACTION
	DECLARE @userName nvarchar(256)
	DECLARE @qualifierTypeID int
	DECLARE @agentID int
	
	select @agentID = (select agent_ID from Agents where agent_id = @userID)
	select @userName = (select user_name from Users where user_ID = @userID)
	delete from Agents 
	where agent_name = @userName
	if (@@error > 0)
		goto on_error
		
	/* Must update this logic if a qualifier type user is added */
	select @qualifierTypeID = (select qualifier_type_id from qualifier_types	
										where description = 'Agent')
	delete from qualifiers
	where qualifier_reference_ID = @agentID and qualifier_type_id=@qualifierTypeID
	if (@@error > 0)
		goto on_error
		
	delete from users
	where user_ID = @userID
	if (@@error > 0)
		goto on_error
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetCollectionCount
	@couponID BigInt
AS
	select count(ticket_ID) from IssuedTickets where coupon_ID = @couponID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyExperimentOwner    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE ModifyExperimentOwner
	@experimentID BigInt,
	@newUserID int
AS
	update experiments
	set user_Id = @newUserID
	where experiment_ID = @experimentID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.ModifyExperimentStatus    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyExperimentStatus
	@experimentID BigInt,
	@statusCode int
AS
	begin
		update experiments
		set status = @statusCode
		where experiment_id = @experimentID
	end
	
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyGroup    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyGroup
	@groupID int,
	@groupName nvarchar(256),
	@description nvarchar(4000),
	@email nvarchar(256)
AS
BEGIN TRANSACTION
	DECLARE @oldGroupName nvarchar(256)
	DECLARE @qualifierTypeID1 int
	DECLARE @qualifierTypeID2 int
	
	begin
		select @oldGroupName = (select group_name from Groups where group_id=@groupID)
		if (@oldGroupName <> @groupName)
		begin
			update agents
			set agent_name = @groupName
			where agent_name = @oldGroupName;
			if (@@error > 0)
				goto on_error
				
			select @qualifierTypeID1 = (select qualifier_type_id from qualifier_types	
										where description = 'Agent')
			select @qualifierTypeID2 = (select qualifier_type_id from qualifier_types	
								where description = 'Group')
								
			update qualifiers
			set qualifier_name = @groupName
			where qualifier_reference_id = @groupID and qualifier_name = @oldGroupName 
					and (qualifier_type_id = @qualifierTypeID1 or qualifier_type_id = @qualifierTypeID2)
			if (@@error > 0)
				goto on_error
		end
	
		update groups 
		set group_name = @groupName, description = @description, email=@email
		where group_id = @groupID
		if (@@error > 0)
			goto on_error
	end
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.ModifyLabClient    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyLabClient
	@labClientID int,
	@labClientName nvarchar(256),
	@shortDescription nvarchar(256),
	@longDescription ntext,
	@version nvarchar(50),
	@loaderScript nvarchar(2000),
	@clientType varchar (128),
	@email nvarchar(256),
	@firstName nvarchar(128),
	@lastName nvarchar(128),
	@notes nvarchar(2048),
	@needsScheduling bit,
	@needsESS bit,
	@isReentrant bit
AS
		DECLARE @clientTypeID INT
		
		SELECT @clientTypeID = (SELECT client_type_id FROM client_types 
							WHERE upper(description) = upper(@clientType))
							
		UPDATE Lab_Clients
		SET lab_client_name = @labClientName, short_description=@shortDescription, 
			long_description=@longDescription, version = @version, 
			loader_script = @loaderScript, client_type_id=@clientTypeID, contact_email = @email, 
			contact_first_name=@firstName, contact_last_name=@lastName, notes=@notes,
			NeedsScheduling=@needsScheduling,NeedsESS=@needsESS,IsReentrant= @isReentrant
		WHERE client_ID = @labClientID
RETURN
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.ModifySystemMessage    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifySystemMessage
	@messageID	int,
	@messageType varchar(256),
	@messageTitle nvarchar(256),
	@toBeDisplayed bit,
	@groupID int,
	@agentID int,
	@clientID int,
	@messageBody nvarchar (3000) 
AS
	DECLARE @messageTypeID int

	select @messageTypeID = (select message_type_id from message_types 
							where upper(description) = upper(@messageType))
							
	update System_Messages
	set message_type_id = @messageTypeID, to_be_displayed=@toBeDisplayed, 
	group_ID = @groupID, agent_id=@agentID, client_ID = @clientID, message_title = @messageTitle,
	message_body = @messageBody, last_modified = getUtcdate()
	where system_message_ID = @messageID;

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyUser    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyUser
	@userID int,
	@userName nvarchar(256),
	@firstName nvarchar(256),
	@lastName nvarchar(256),
	@email varchar(256),
	@affiliation nvarchar(256),
	@reason nvarchar(2048),
	@XMLExtension text,
	@lockUser bit,
	@principalString nvarchar (256),
	@authenType varchar(256)
AS
BEGIN TRANSACTION
	DECLARE @authTypeID int
	DECLARE @oldUserName nvarchar(256)
	DECLARE @qualifierTypeID int
		
	begin
		select @oldUserName = (select user_name from Users where user_id=@userID)
		if (@oldUserName != @userName)
		begin
			update agents
			set agent_name = @userName
			where agent_name = @oldUserName;
			if (@@error > 0)
				goto on_error
				
		/* Must update this logic if a qualifier type user is added */
			select @qualifierTypeID = (select qualifier_type_id from qualifier_types	
										where description = 'Agent')
			update qualifiers
			set qualifier_name = @userName
			where qualifier_reference_id = @userID and qualifier_name = @oldUserName 
					and qualifier_type_id = @qualifierTypeID
			if (@@error > 0)
				goto on_error
		end
		
		update users
		set user_name=@userName, first_name = @firstName, last_name=@lastName, 
			affiliation = @affiliation,  signup_reason=@reason, 
			XML_Extension = @XMLExtension, email = @email, lock_user=@lockUser
		where user_ID = @userID;
		if (@@error > 0)
			goto on_error
	
		select @authTypeID = (select auth_type_id from authentication_types where upper(description)=upper(@authenType))		
		update Principals 
		set principal_string=@principalString, auth_type_id = @authTypeID
		where user_id = @userID;
		if (@@error > 0)
			goto on_error
	end
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAgent    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgent
	@agentID int
AS
	select agent_ID, is_group
	from   agents
	where agent_ID = @agentID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 

GO

CREATE PROCEDURE RetrieveUserGroups
	@userID int
AS
	select parent_group_ID 
	from   agent_hierarchy
	where agent_ID = @userID and parent_group_id NOT 
	in (select group_id from groups where  group_type_id = 2 or group_name='ROOT' or group_name='Group not assigned'
	or group_name='NewUserGroup' or group_name='OrphanedUserGroup')

GO


/****** Object:  Stored Procedure dbo.RetrieveAgentGroup    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgentGroup
	@agentID int
AS
	select parent_group_ID 
	from   agent_hierarchy
	where agent_ID = @agentID --and (parent_group_id!= (select group_id from groups where group_name='ROOT'))
	-- don't select root as a group. root is parent for everyone!
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentHierarchyTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgentHierarchyTable
AS
	select agent_id,parent_group_id
	from agent_hierarchy
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentType    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgentType
	@agentID int
AS
	select  is_group
	from  agents
	where (agent_id=@agentID)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentsTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgentsTable
-- Procedure was added by Karim - need this for constructing the tree control
AS
	select agent_id, is_group
	from agents
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAssociatedGroupID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAssociatedGroupID
	@groupID int
AS
	select associated_group_ID
	from groups
	where group_id = @groupID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.RetrieveClientInfo    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveClientInfo
	@labClientID int
AS
	select client_info_id, info_URL, info_name, display_order, description
	from client_info
	where client_id=@labClientID
	order by display_order
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveClientItem    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveClientItem
	@clientID int,
	@userID int,
	@itemName nvarchar(256)
AS
	select item_value 
	from client_items
	where client_id = @ClientID and user_ID=@userID and item_Name = @itemName;
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveClientItemNames    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveClientItemNames
	@clientID int,
	@userID int
AS
	select item_name
	 from client_items
	where client_id = @ClientID and user_ID=@userID;
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveClientServerIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveClientServerIDs
	@labClientID int
AS
	select agent_id
	from lab_server_to_client_map
	where client_id=@labClientID
	order by display_order
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveExperiment    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveExperiment
	@experimentID BigInt
AS
	select user_id, group_id, agent_ID, client_ID,status, ess_ID, scheduledStart, duration,creationTime, closeTime, annotation, record_count
	from Experiments 
	where Experiment_ID = @experimentID 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentRawData    Script Date: 11/27/2007 ******/

CREATE PROCEDURE RetrieveExperimentRawData
	@experimentID BigInt
AS
	select user_id, Group_ID,Agent_ID,Client_ID,ESS_ID,status
	from Experiments
	where Experiment_ID = @experimentID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE GetExperimentStatusCode
@experimentID bigint

AS
BEGIN

select status
from Experiments where Experiment_ID = @experimentID
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE RetrieveExperimentAdminInfo
@experimentID bigint

AS
BEGIN

select experiment_ID,user_ID,group_ID,agent_ID,client_ID,ess_ID,
status,record_Count,duration,scheduledStart,creationTime,closeTime,annotation
from Experiments where Experiment_ID = @experimentID
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE  PROCEDURE RetrieveExperimentAdminInfos
@ids varchar(8000)

AS
BEGIN

select experiment_ID,user_ID,group_ID,agent_ID,client_ID,ess_ID,
status,record_Count,duration,scheduledStart,creationTime,closeTime,annotation
from Experiments where Experiment_ID in (SELECT * FROM [dbo].[toLongArray](@ids))
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentSummary    Script Date: 02/16/2007 ******/

CREATE  PROCEDURE RetrieveExperimentSummary
	@experimentID BigInt
AS
declare @essID int
Select @essID= ess_id from experiments where experiment_ID = @experimentId
if @essID > 0

	select u.user_Name,g.group_Name,pa.Agent_Guid,pa.Agent_Name,
	 c.Lab_Client_Name,c.version, status, pa2.Agent_Guid, scheduledStart, 
	duration, creationTime, closeTime, annotation, record_count
	from Experiments ei,ProcessAgent pa, ProcessAgent pa2, Groups g, Lab_Clients c, Users u
	where ei.Experiment_ID = @experimentID and ei.agent_ID = pa.Agent_ID 
	and ei.ess_ID = pa2.Agent_ID and ei.user_ID= u.user_id 
	and ei.group_id = g.group_id and ei.client_ID= c.Client_ID
else
select u.user_Name,g.group_Name,pa.Agent_Guid,pa.Agent_Name,
	 c.Lab_Client_Name,c.version, status, null, scheduledStart, 
	duration, creationTime, closeTime, annotation, record_count
	from Experiments ei,ProcessAgent pa, ProcessAgent pa2, Groups g, Lab_Clients c, Users u
	where ei.Experiment_ID = @experimentID and ei.agent_ID = pa.Agent_ID 
	and ei.user_ID= u.user_id 
	and ei.group_id = g.group_id and ei.client_ID= c.Client_ID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO





/****** Object:  Stored Procedure dbo.RetrieveExperimentAnnotation    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveExperimentAnnotation
	@experimentID BigInt
AS
	select annotation
	from Experiments
	where Experiment_ID = @experimentID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentGroup    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveExperimentGroup
	@experimentID BigInt
AS
	select group_id
	from Experiments
	where Experiment_ID = @experimentID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveExperimentOwner    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveExperimentOwner
	@experimentID BigInt
AS
	select user_id
	from Experiments
	where Experiment_ID = @experimentID
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

--DROP procedure RetrieveActiveExpIDs
GO
create procedure RetrieveActiveExpIDs
@userId int,
@groupId int,
@serverId int,
@clientId int
AS
 
select experiment_ID from experiments
where user_ID = @userId and Group_ID = @groupId and agent_ID = @serverId and Client_ID = @clientID
AND DATEADD(second,duration,scheduledStart) > GETUTCDATE()

return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

--DROP procedure RetrieveAuthorizedExpIDs
GO
create procedure RetrieveAuthorizedExpIDs
@userid int,
@groupId int
AS
 
declare @exps TABLE (experiment_id bigint,ess_id int)

declare @status bit
SET @status = dbo.IsSuperuser(@userid,@groupid)
if @status = 1
BEGIN
insert @exps SELECT  experiment_ID,ess_id from experiments
END
ELSE
BEGIN
insert @exps SELECT  experiment_ID,ess_id from experiments
WHERE (USER_ID = @userID) or group_id in
(select qualifier_reference_ID from qualifiers
where qualifier_type_id = 6 
and Qualifier_id in (select qualifier_id from grants 
where agent_id = @groupid and function_id = 5))
END
select * from @exps

return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO
--drop procedure RetrieveAuthorizedExpIDsCriteria
GO
create procedure RetrieveAuthorizedExpIDsCriteria
@userid int,
@groupId int,
@criteria varchar (7000)
AS

declare @queryStr varchar(7500)

create table #expsFiltered (experiment_id bigint, ess_id int)
declare @status bit
SET @status = dbo.IsSuperuser(@userid,@groupid)
if @status = 0
BEGIN
-- select * from userExperiments(@userid,@groupid)
	create table #exps (experiment_id bigint)
	insert #exps SELECT  experiment_ID from experiments
		WHERE (USER_ID = @userID) or group_id in
		(select qualifier_reference_ID from qualifiers
		where qualifier_type_id = 6 
		and Qualifier_id in (select qualifier_id from grants 
		where agent_id = @groupid and function_id = 5))

	if @criteria is not null
	BEGIN
		select @queryStr = 'insert #expsFiltered select experiment_id,ess_id from experiments where experiment_id in ( '
			 + 'select experiment_id from #exps ) AND ( ' + @criteria + ')'

		EXEC(@queryStr)
	END
	ELSE
	BEGIN
		insert #expsFiltered select experiment_id,ess_id from experiments 
		where experiment_id in ( select experiment_id from #exps )
		
	END
	drop table #exps
END

ELSE
BEGIN

	if @criteria is not null
	BEGIN
		select @queryStr = 'insert #expsFiltered select experiment_id,ess_id from experiments where '
			+  @criteria 
		EXEC(@queryStr)
	END
	ELSE
	BEGIN
		insert #expsFiltered select experiment_id,ess_id from experiments 
	END
END

select experiment_id, ess_id from #expsFiltered
select distinct ess_id from #expsFiltered where ess_id IS NOT NULL

drop table #expsFiltered
RETURN
GO



/****** Object:  Stored Procedure dbo.RetrieveGrantsTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGrantsTable
AS
	select agent_id, f.function_name, qualifier_id, grant_id
	from grants g, functions f
	where g.function_id =f.function_id
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroup    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroup
	@groupID int
AS
	select  group_name, g.description AS description, email, gt.description AS group_type, date_created
	from groups g, group_types gt
	where group_ID= @groupID and g.group_type_id=gt.group_type_id
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupExperimentIDs    Script Date: 5/18/2005 4:17:56 PM ******/
CREATE PROCEDURE RetrieveGroupExperimentIDs
	@groupID int
AS
	select Experiment_ID
	from Experiments
	where Group_ID = @groupID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupIDs
AS
	select group_id
	from groups
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAdminGroupIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAdminGroupIDs
AS
	DECLARE @adminGroupType int
	
	select @adminGroupType = (Select group_type_id from group_types where description
								 = 'Service Administration Group');
	select group_ID from groups
	where group_type_id = @adminGroupType
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupID
	@groupName nvarchar(256)
AS
	select group_ID
	from groups
	where group_name = @groupName
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE RetrieveGroupName
	@groupID int
AS
	select group_Name
	from groups
	where group_ID = @groupID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupMembers    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupMembers
	@groupID int
AS
	select ah.agent_ID, ag.is_group, ag.agent_name
	from   agent_hierarchy ah, agents ag
	where ah.parent_group_ID = @groupID and ah.agent_id=ag.agent_id
	order by agent_name
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupRequestGroupID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupRequestGroupID
	@groupID int
AS
	DECLARE @requestGroupType int
	
	select @requestGroupType = (Select group_type_id from group_types where description
								 = 'Request Group');
	select group_ID from groups
	where associated_group_id = @groupID and group_type_id = @requestGroupType
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupAdminGroupID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupAdminGroupID
	@groupID int
AS
	DECLARE @adminGroupType int
	
	select @adminGroupType = (Select group_type_id from group_types where description
								 = 'Course Staff Group');
	select group_ID from groups
	where associated_group_id = @groupID and group_type_id = @adminGroupType
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClient    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabClient
	@labClientID int
AS
	select client_guid, lab_client_name, short_description, long_description, version, loader_script, 
		contact_email, contact_first_name, contact_last_name, notes, date_created,
		client_types.description,NeedsScheduling,NeedsESS,IsReentrant
	from lab_clients, client_types
	where client_id = @labClientID and lab_clients.client_type_id = client_types.client_type_id
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE RetrieveLabClientByGuid
	@clientGuid varchar(50)
AS
	select client_id, client_guid, lab_client_name, short_description, long_description, version, loader_script, 
		contact_email, contact_first_name, contact_last_name, notes, date_created,
		client_types.description,NeedsScheduling,NeedsESS,IsReentrant
	from lab_clients, client_types
	where client_guid = @clientGuid and lab_clients.client_type_id = client_types.client_type_id
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabClientID
@guid varchar (50)
AS
	select client_id
	from lab_clients where client_guid = @guid
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabClientIDs
AS
	select client_id
	from lab_clients
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientTypes    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabClientTypes
AS
	select description
	from client_types
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveNativePassword    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveNativePassword
	@userID int
AS
	select password
	from users
	where user_ID =@userID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveNativePrincipals    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveNativePrincipals
AS
	select user_ID
	from   principals 
	where auth_type_id=(select auth_type_id from authentication_types where
					upper(description) = 'NATIVE')
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveOrphanedUserIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveOrphanedUserIDs
AS
	DECLARE @orphanedGroupID int
	
	select @orphanedGroupID = (select group_ID from Groups where group_name = 'OrphanedUserGroup')
	
	select user_id
	from users, agent_hierarchy ah, agents
	where ah.parent_group_id=@orphanedGroupID and ah.agent_id=agents.agent_id and agents.agent_name = users.user_name
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveQualifierHierarchyTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveQualifierHierarchyTable
AS
	select qualifier_id, parent_qualifier_id
	from qualifier_hierarchy
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveQualifiersTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveQualifiersTable
AS
	select qualifier_id, qualifier_Reference_ID, qualifier_Type_ID, qualifier_name, date_created
	from qualifiers
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessageByID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveSystemMessageByID
	@messageID int
/*Retrieve by message ID*/
AS
	select  description, to_be_displayed, last_modified, agent_ID, client_ID, group_id, 
			message_title, message_body
	from system_messages sm, message_types mt
	where sm.system_message_ID= @messageID and to_be_displayed = 1
		and sm.message_type_id=mt.message_type_id 
	
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessageByIDForAdmin    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveSystemMessageByIDForAdmin
	@messageID int
/*Retrieve by message ID for admin pages (all messages)*/
AS
	select  message_body, to_be_displayed, last_modified, client_ID, group_id, 
			agent_id,message_title, description
	from system_messages sm, message_types mt
	where sm.system_message_ID= @messageID and sm.message_type_id=mt.message_type_id
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessages    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveSystemMessages
/*Retrieves by message type and group */
	@messageType varchar(256),
	@agentID int,
	@clientID int,
	@groupID int
AS
	DECLARE @messageTypeID int
	
	select @messageTypeID = (select message_type_id from message_types 
						where upper(description) = upper(@messageType))
	
	select system_message_ID, message_body, to_be_displayed, last_modified, message_title
	from system_messages sm
	where sm.message_type_id=@messageTypeID and to_be_displayed =1 
			and group_ID=@groupID and agent_ID=@agentID and client_ID =@clientID
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessagesForAdmin    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveSystemMessagesForAdmin
/*Retrieves by message type and group */
	@messageType varchar(256),
	@clientID int,
	@groupID int,
	@agentID int
AS
	DECLARE @messageTypeID int
	
	select @messageTypeID = (select message_type_id from message_types 
						where upper(description) = upper(@messageType))
	
	select system_message_ID, message_body, to_be_displayed, last_modified, message_title
	from system_messages sm
	where sm.message_type_id=@messageTypeID	and group_ID=@groupID and agent_ID=@agentID and client_ID = @clientID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUser    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveUser
	@userID int
AS
	select user_name, first_name, last_name, affiliation, XML_extension, signup_reason, 
			email, date_created, lock_user, password
	from users u 
	where u.user_id = @userID 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUserEmail    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveUserEmail
	@userName nvarchar(256)
AS
	select email
	from users
	where user_name = @userName
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUserID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveUserID
	@userName nvarchar(256)
AS
	select user_ID
	from users
	where user_name = @userName
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUserIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveUserIDs
AS
	select user_id
	from users
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUserName    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveUserName
	@userID int
AS
	select user_Name
	from users
	where user_id = @userID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SaveBlobXMLExtensionSchemaURL    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE SaveBlobXMLExtensionSchemaURL
 	@labserverID varchar(256),
	@URL nvarchar(512)
AS
/* hardcoded account ID*/
	update accounts
	set blob_XML_Extension_schema_URL = @URL
	where account_ID = 2
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SaveClientItem    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE SaveClientItem
	@clientID int,
	@userID int,
	@itemName nvarchar(256),
	@itemValue ntext
AS
	DECLARE @clientItemID bigint
	select @clientItemID = (select client_item_id from client_items where client_id = @ClientID and user_ID=@userID and item_Name = @itemName);
	if (@clientItemID is not null) 
	begin
		update Client_Items
		set item_Value = @itemValue
		where client_item_ID = @clientItemID;
		if (@@error > 0)
			goto on_error
	end
	else
	begin
	begin transaction
		insert into client_items (user_id,client_id, item_name,item_value)
		values (@userID, @clientID, @itemName,  @itemValue)
		if (@@error > 0)
			goto on_error
	commit transaction
	end
	return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SaveExperimentAnnotation    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE SaveExperimentAnnotation
	@experimentID BigInt,
	@annotation ntext 
AS
	/*select annotation 
	from experiments 
	where experiment_id = @experimentID*/
	
	update Experiments
	set Annotation=@annotation
	where Experiment_ID = @experimentID;

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO





/****** Object:  Stored Procedure dbo.SaveNativePassword    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SaveNativePassword
	@userID int,
	@password nvarchar(256)
AS
BEGIN TRANSACTION
	update users
	set password = @password
	where user_id =@userID
	IF (@@ERROR <> 0) goto on_error
COMMIT TRANSACTION
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SaveResultXMLExtensionSchemaURL    Script Date: 5/18/2005 4:17:57 PM ******/
/* This procedure is probably not being used anywhere */
CREATE PROCEDURE SaveResultXMLExtensionSchemaURL
 	@labserverID int,
	@URL varchar(512)
AS
/* hardcoded account ID*/
	update accounts
	set result_XML_Extension_schema_URL = @URL
	where account_ID = 2
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SaveUserSessionEndTime    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SaveUserSessionEndTime
	@sessionID BigInt

AS 
	update user_sessions set session_end_time = getutcdate()
	where session_id=@sessionID
	
	select session_end_time from user_sessions where session_ID = @sessionID
	return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SelectAllUserSessions    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SelectAllUserSessions
	@userID int,
	@groupID int,
	@TimeBefore DateTime,
	@TimeAfter DateTime

AS 
	select session_ID, session_start_time, session_end_time, effective_group_ID, tz_Offset,session_key
	from user_sessions 
	where user_ID=@userID
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


-- Only return user sessions that have not been closed
CREATE PROCEDURE SelectSessionInfo
	@sessionID BigInt
AS 
	
	select s.user_id, s.effective_group_id, s.client_ID, u.user_Name, g.group_Name,s.TZ_Offset
	 from user_sessions s,users u, groups g where session_ID = @sessionID
	AND s.Session_End_Time = NULL
	AND s.user_id = u.user_id and s.effective_group_ID=g.group_ID 
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SelectUserSession    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SelectUserSession
	@sessionID BigInt
AS 
	
	select user_id, effective_group_id, client_ID, tz_Offset, session_start_time, session_end_time, session_key
	 from user_sessions where session_ID = @sessionID
return
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Create Procedure GetAdminProcessAgentTags
@groupId int

as

select pa.Agent_id, pa.Agent_Name 

FROM ProcessAgent pa 

where pa.Agent_id 
in ( select Qualifier_Reference_id
from Qualifiers where qualifier_id 
in ( select Qualifier_id from grants 
where agent_id = @groupID AND function_ID between 13 and 19))
go

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Create Procedure GetProcessAgentAdminGrants
@agentId int,
@groupID int

as

select g.Agent_id, f.function_Name, g.grant_ID, g.qualifier_ID

FROM Grants g, Functions f

 

where g.Agent_ID = @groupID AND g.qualifier_id 
in ( select Qualifier_id
from Qualifiers where qualifier_Reference_ID = @agentID 
AND qualifier_id 
in ( select Qualifier_id from grants 
where agent_id = @groupID AND function_ID between 13 and 19))
AND g.Function_id = f.Function_id
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE procedure GetAdminServiceTags
@groupId int

as

select agent_id, Agent_Name from ProcessAgent
where agent_id in
(select distinct mappingvalue from ResourceMappingValues where MappingValue_Type = 1 AND
mapping_ID in (select Qualifier_reference_id  from Qualifiers where Qualifier_Type_id = 11 
and qualifier_id in
( select qualifier_id from grants where agent_id = @groupId and Function_id between 13 and 19 ) ) )

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


/****** Object:  Stored Procedure dbo.DeleteAdminURL    Script Date: 5/2/2006 4:17:55 PM ******/

CREATE PROCEDURE DeleteAdminURL
@processAgentID int,
@adminURL nvarchar (512),
@ticketType varchar (100)
AS
DECLARE @ticket_Type_ID int

select @Ticket_Type_ID = (select Ticket_Type_ID  from Ticket_Type where (upper(Name) = upper(@ticketType )))
delete from AdminURLs  where ProcessAgentID = @processAgentID and  AdminURL = @adminURL and Ticket_Type_ID = @ticket_Type_ID;
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE DeleteAdminURLbyID

       @adminURLID int

AS
       delete from AdminURLs
        where id= @adminURLID
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


CREATE PROCEDURE InsertAdminURL

@processAgentID int,
@adminURL nvarchar (512),
@ticketType varchar (100)
AS
DECLARE @ticket_Type_ID int

select @Ticket_Type_ID = (select Ticket_Type_ID  from Ticket_Type where (upper(Name) = upper(@ticketType )))

insert into AdminURLs(ProcessAgentID, AdminURL, Ticket_Type_ID) values (@processAgentID, @adminURL, @ticket_Type_ID)
select ident_current('AdminURLs')
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ModifyAdminURL
@id int,
@url nvarchar(512),
@typeID int
AS
update AdminURLs set AdminURL=@url where ProcessAgentID=@id and Ticket_Type_ID = @typeID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ModifyAdminUrlCodebase
@id int,
@old nvarchar(512),
@new nvarchar(512)
AS
update AdminURLs set AdminURL=REPLACE(AdminURL,@old,@new) where ProcessAgentID=@id
select @@rowcount
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE RetrieveAdminURLs 

@processAgentID int
AS

SELECT      AdminURLs.ID, AdminURLs.ProcessAgentID, AdminURLs.AdminURL, Ticket_Type.Name
FROM         AdminURLs INNER JOIN
                      Ticket_Type ON AdminURLs.Ticket_Type_ID = Ticket_Type.Ticket_Type_ID
WHERE     (AdminURLs.ProcessAgentID = @processAgentID and AdminURLs.Ticket_Type_ID = Ticket_Type.Ticket_Type_ID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE AddResourceMappingKey 
	@mappingKey_Type int,
	@mappingKey int
 AS
declare @idmap int

	
		insert into ResourceMappingKeys (MappingKey_Type, MappingKey) values (@mappingKey_Type, @mappingKey)
		select ident_current('ResourceMappingKeys')
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE AddResourceMappingValue
	@mapping_ID int,
	@mappingValue_Type int,
	@mappingValue int
 AS
	insert into ResourceMappingValues (Mapping_ID, MappingValue, MappingValue_Type) values (@mapping_ID, @mappingValue, @mappingValue_Type)
	select ident_current('ResourceMappingValues')
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE AddResourceMappingString
	@string_Value nvarchar(2048)
 AS

	insert into ResourceMappingStrings (String_Value) values (@string_Value)
	select ident_current('ResourceMappingStrings')

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
Create Procedure GetResourceTypeString
@resourceType nvarchar( 256),
@type int,
@target int

as 
declare @mapId int


select @mapId = mapping_id from resourceMappingValues 
where mapping_ID 
in ( select mapping_id from ResourceMappingKeys
where MappingKey_type = @type and MappingKey = @target)
and MappingValue_Type = 7 and MappingValue =(select id from ResourceMappingResourceTypes
where resourceType_value = @resourceType )

if @mapID > 0
select mappingvalue, string_value from resourceMappingValues, ResourceMappingStrings
where mappingValue_Type = 4 and mapping_ID = @mapId and ID = mappingvalue
else
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

Create Procedure GetResourceTypeNames
@type int,
@target int

as 

select mapping_id, ResourceType_Value
from resourceMappingValues mv, ResourceMappingResourceTypes rt
where mapping_ID 
in ( select mapping_id from ResourceMappingKeys where MappingKey_type = @type and MappingKey = @target)
and MappingValue_Type = 7 
and mv.MappingValue = rt.ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Create PROCEDURE GetMappingStringTag
@mapid int

AS

if (select count(*) from ResourceMappingValues where mapping_ID = @mapid and MappingValue_Type = 4) > 0
	select mv.mappingValue, rs.String_Value
	from ResourceMappingValues mv, ResourceMappingStrings rs
	where mapping_ID = @mapid and MappingValue_Type = 4 
	and mv.MappingValue = rs.ID
else
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

CREATE PROCEDURE AddResourceMappingResourceType
	@resourceType_Value nvarchar(256)
 AS
	if(select count(*) from ResourceMappingResourceTypes where ResourceType_Value = @resourceType_Value) = 0
		begin
		insert into ResourceMappingResourceTypes (ResourceType_Value) values (@resourceType_Value)
		select ident_current('ResourceMappingResourceTypes')
		end
	else
		select ID from ResourceMappingResourceTypes where ResourceType_Value = @resourceType_Value
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetResourceIDsByKey
	@mappingKey_Type int,
	@mappingKey int
 AS
	select Mapping_ID from ResourceMappingKeys where (MappingKey_Type = @mappingKey_Type AND MappingKey = @mappingKey)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetResourceMappingByID
	@mappingID int
AS 
	DECLARE @mappingKey_Type int
	DECLARE @mappingKey int

	/* retrieve map key type and key */
	select @mappingKey_Type  = ( select MappingKey_Type FROM ResourceMappingKeys where (Mapping_ID = @mappingID))
	select @mappingKey  = ( select MappingKey FROM ResourceMappingKeys where (Mapping_ID = @mappingID))

	/* retrieve corresponding map value type and value */
	select @mappingKey_Type, @mappingKey union all select MappingValue_Type, MappingValue FROM ResourceMappingValues where (Mapping_ID = @mappingID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Create Procedure GetResourceTypeStrings
@type int,
@target int

as 

select mapping_id, ResourceType_Value
from resourceMappingValues mv, ResourceMappingResourceTypes rt
where mapping_ID 
in ( select mapping_id from ResourceMappingKeys where MappingKey_type = @type and MappingKey = @target)
and MappingValue_Type = 7 
and mv.MappingValue = rt.ID;


select mapping_id,   mv.MappingValue, String_Value
from resourceMappingValues mv, ResourceMappingStrings rs
where mapping_ID 
in ( select mapping_id from ResourceMappingKeys where MappingKey_type = @type and MappingKey = @target)
and MappingValue_Type = 4 
and mv.MappingValue = rs.ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetResourceStringByID
	@ID int
 AS
	select String_Value from ResourceMappingStrings where ID = @ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

Create  PROCEDURE UpdateResourceMappingString
	@id int,
	@string nvarchar(2048)
 AS

	update ResourceMappingStrings set String_Value= @string where ID = @id
	select mapping_ID from ResourceMappingValues where MappingValue = @id and MappingValue_Type = 4

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetResourceTypeByID
	@ID int
 AS
	select ResourceType_Value from ResourceMappingResourceTypes where ID = @ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE GetResourceMappingIDs
 AS
	select Mapping_ID from ResourceMappingKeys
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE InsertResourceMappingKey
	@MappingKey_Type int,
	@MappingKey int
AS
      
        	insert into ResourceMappingKeys (MappingKey_Type, MappingKey)
			values (@MappingKey_Type, @MappingKey)
        	select ident_current('ResourceMappingKeys')
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 

GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE InsertResourceMappingValue
	@Mapping_ID int,
             @MappingValue_Type int,
	@MappingValue int
AS
       insert into ResourceMappingValues (Mapping_ID, MappingValue_Type, MappingValue)
		values (@Mapping_ID, @MappingValue_Type, @MappingValue)

      select @Mapping_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.DeleteREsourceMapping    Script Date: 10/4/2005 ******/

CREATE PROCEDURE DeleteResourceMapping
	@mapping_ID int
AS
BEGIN TRANSACTION

	delete from ResourceMappingValues
	where Mapping_ID = @mapping_ID
	if (@@error > 0)
		goto on_error

	delete from ResourceMappingKeys
	where Mapping_ID = @mapping_ID
	if (@@error > 0)
		goto on_error

	delete from Qualifiers
	where qualifier_reference_ID = @mapping_ID and 
	qualifier_type_ID = (select qualifier_type_ID from qualifier_Types 
				where description = 'Resource Mapping');
	if (@@error > 0)
		goto on_error

COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



Create Procedure GetMappingIdByKeyValue
@keyId int,
@keyType int,
@valueId int,
@valueType int

as 

select mapping_id from ResourceMappingValues
where mapping_id 
in ( select mapping_id from ResourceMappingKeys
	where MappingKey = @keyId and MappingKey_Type = @keyType )
AND MappingValue = @valueId AND MappingValue_Type = @valueType

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


Create PROCEDURE GetResourceMapIdsByValue
@type nvarchar(256),
@id int

AS

select distinct Mapping_ID from ResourceMappingVAlues
where mappingValue =@id and MappingValue_Type = (select type_id FROM ResourceMappingTypes where Type_Name =@type)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/* Registration Table Procedures */


CREATE PROCEDURE InsertRegistration
@couponId int,
@couponGuid varchar (50),
@registerGuid varchar (50),
@sourceGuid varchar (50),
@status int,
@descriptor NTEXT,
@email nvarchar (256)

AS

 insert into Registration (couponID, couponGuid,registerGuid,sourceGuid,status,descriptor,email)
	values (@couponId,@couponGuid, @registerGuid,@sourceGuid,@status,@descriptor,@email)
select ident_current('Register')

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SelectRegistrationRecord
@id int
AS
select record_id, couponID, couponGuid, registerGuid,sourceGuid,status,createTime,lastModTime,descriptor,email
from Registration
where record_id = @id

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE SelectRegistrations
AS
select record_id, couponId,couponGuid, registerGuid,sourceGuid,status,createTime,lastModTime,descriptor,email
from Registration
order by registerGuid

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SelectRegistration
@registerGuid varchar (50)
AS
select record_id,couponID,couponGuid, registerGuid,sourceGuid,status,createTime,lastModTime,descriptor,email
from Registration
where registerGuid = @registerGuid

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SelectRegistrationByStatus
@status int
AS
select record_id,couponID,couponGuid, registerGuid,sourceGuid,status,createTime,lastModTime,descriptor,email
from Registration
where status = @status
order by registerGuid

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SelectRegistrationByRange
@low int,
@high int
AS
select record_id,couponId,couponGuid,registerGuid,sourceGuid,status,createTime,lastModTime,descriptor,email
from Registration
where status BETWEEN @low and @high
order by registerGuid

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE updateRegistrationStatus
@id int,
@status int
AS

update registration set status=@status,lastModTime= getUtcdate()
where record_id=@id

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


