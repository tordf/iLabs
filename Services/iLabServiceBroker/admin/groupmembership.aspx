<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.groupMembership" CodeFile="groupMembership.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>MIT iLab Service Broker - Group Membership</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id: groupmembership.aspx,v 1.3 2007/01/29 22:19:38 pbailey Exp $ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner><uc1:adminnav id="AdminNav1" runat="server"></uc1:adminnav><br clear="all"/>
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Group Membership</h1>
						<p>Add or remove group memberships by selecting at least one user or group and at 
							least one new target group to contain the selected user or group.
						</p>
						<!-- Group Membership Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
						<asp:label id="lblResponse" Visible="False" Runat="server"></asp:label>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div id="messagebox-right">
							<h4>Instructions</h4>
							<p>Select the user/group you want to transfer from "Users and Groups" box. Select 
								the group you want to move it under from the "Target Groups" box. Click the 
								Copy/Move button."
							</p>
						</div>
						<div class="simpleform">
							<label for="searchfor">Search by username or groupname</label><br/>
							<asp:textbox id="txtSearchfor" Runat="server"></asp:textbox><asp:button id="btnGo" Runat="server" CssClass="button" Text="Go"></asp:button>
							<!--input name="searchfor" type="text" id="searchfor" size="20"--> <!--input name="Submit" type="submit" class="button" value="Go"-->
						</div>
						<div class="simpleform">
							<div>&nbsp;</div>
							<div>&nbsp;</div>
							<div>&nbsp;</div>
							<table>
								<tr>
									<th class="top">
										<label for="usersandgroups">Users and Groups </label>
									</th>
									<th>
										&nbsp;
									</th>
									<th class="top">
										<label for="targetgroups">Target Groups </label>
									</th>
								</tr>
								<tr>
									<td style="WIDTH: 300px">
										<!--Check to see if default style can be set from the css -->
										<asp:TreeView   id="agentsTreeView" runat="server" cssClass="treeView" SelectedNodeStyle-ForeColor="White" SelectedNodeStyle-Font-Bold="true"   SelectedNodeStyle-BackColor="BlueViolet" ForeColor="black" style="font-family:verdana,arial,helvetica;font-size:10px"></asp:TreeView>
									</td>
									<td class="buttonstyle"><asp:ImageButton ID="ibtnCopyTo" Runat="server" Width="74" Height="22" CssClass="buttonstyle" ImageUrl="../img/copy-btn.gif"
											AlternateText="Copy To"></asp:ImageButton><!--img src="../img/copy-btn.gif" alt="Copy to" width="74" height="22" class="buttonstyle"--><br/>
										<asp:ImageButton ID="ibtnMoveTo" Runat="server" Width="74" Height="22" CssClass="buttonstyle" ImageUrl="../img/move-btn.gif"
											AlternateText="Move to"></asp:ImageButton><!--img src="../img/move-btn.gif" alt="Move to" width="74" height="22" class="buttonstyle"--><br/>
										<asp:ImageButton ID="ibtnRemove" Runat="server" Width="74" Height="22" CssClass="buttonstyle" ImageUrl="../img/remove-btn.gif"
											AlternateText="Remove"></asp:ImageButton><!--img src="../img/remove-btn.gif" alt="Remove" width="74" height="22" class="buttonstyle"-->
									</td>
									<td style="WIDTH: 310px">
										<!-- Check to see if default style can be set from the css -->
										<asp:TreeView id="groupsTreeView" runat="server"  cssClass="treeView" SelectedNodeStyle-ForeColor="White" SelectedNodeStyle-Font-Bold="true"   SelectedNodeStyle-BackColor=BlueViolet ForeColor="black" style="font-family:verdana,arial,helvetica;font-size:10px"></asp:TreeView>
										<div></div>
									</td>
								</tr>
							</table>
						</div>
					</div>
					<br clear="all"/>
					<!-- end pagecontent div -->
				</div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</html>
