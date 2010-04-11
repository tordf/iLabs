<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.iLabSB.myClient" CodeFile="myClient.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - My Client</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id: myClient.aspx,v 1.6 2008/03/17 21:22:06 pbailey Exp $ -->
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<body>
		<form method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>My Labs
						</h1>
						<h2>Group:
							<asp:label id="lblGroupNameTitle" Runat="server"></asp:label></h2>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div id="messagebox-right">
							<h3><asp:label id="lblGroupNameSystemMessage" Runat="server"></asp:label></h3>
							<asp:repeater id="repSystemMessage" runat="server">
								<ItemTemplate>
									<p class="message">
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "MessageBody")) %>
									</p>
									<p class="date">Date Posted:
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "LastModified")) %>
									</p>
								</ItemTemplate>
							</asp:repeater></div>
						<!-- Div id "singlelab" is displayed if only one client is available. Otherwise, div class "group" is displayed, which has a list of available labs. -->
						<div class="singlelab-left">
							<h3>Lab Client:
								<asp:label id="lblClientName" Runat="server"></asp:label>
							</h3>
							<p><strong>Version:</strong>
								<asp:label id="lblVersion" Runat="server"></asp:label></p>
							<p><strong>Description: </strong>
								<asp:label id="lblLongDescription" Runat="server"></asp:label></p>
							<p>
								<asp:label id="lblNotes" Runat="server"></asp:label></p>
							<p><asp:Label ID="lblDocURL" Runat="server"></asp:Label>
                            </p>
							<p><strong>Contact Email:</strong>
								<asp:label id="lblEmail" Runat="server"></asp:label>
							</p>
							<p>
							    <asp:button id="btnLaunchLab" Runat="server" CssClass="button" Text="Launch Lab" onClick="btnLaunchLab_Click" Visible="false" Width="171px" ></asp:button>
							</p>
							<p id="pReenter" runat="server">
							    <asp:button id="btnReenter" Runat="server" CssClass="button" Text="Re-enter Experiment" onClick="btnReenter_Click" Visible="false" Width="171px" ></asp:button>
							</p>
                            <p id="pSchedule" runat="server">
							    <asp:button id="btnSchedule" Runat="server" CssClass="button" Text="Schedule/Redeem Session" onclick="btnSchedule_Click" Visible="false" Width="170px"></asp:button>&nbsp;
                            </p>
							<asp:repeater id="repClientInfos" runat="server"></asp:repeater></div>
						<!-- This is only displayed if the user came from My Groups -->
						<% if(Session["ClientCount"] != null){if (Convert.ToInt32(Session["ClientCount"])>1) { %>
						<p><a href="myClientList.aspx">« Back to Labs in
								'<asp:Label ID="lblBackToLabs" Runat="server"></asp:Label>'
							</a>
						</p>
						<%}}%>
					</div>
					<br clear="all" />
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
			
		</form>
	</body>
</html>
