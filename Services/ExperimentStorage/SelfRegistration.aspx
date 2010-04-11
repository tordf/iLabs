<%@ Page language="c#" Inherits="iLabs.ExpStorage.SelfRegistration" CodeFile="SelfRegistration.aspx.cs" EnableEventValidation="false" %>
<%@ Register Assembly="iLabControls" Namespace="iLabs.Controls" TagPrefix="iLab" %>



<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>selfRegistration</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<style type="text/css">@import url( css/main.css );
		</style>
	</head>
	<body>
	    <form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="innerwrapper">
				<iLab:RegisterSelf id="selfReg"  runat="server" AgentType="EXPERIMENT STORAGE SERVER" ></iLab:RegisterSelf>
			</div><!-- end innerwrapper div -->
			<br clear="all"/>
		</form>				
	</body>
</html>
