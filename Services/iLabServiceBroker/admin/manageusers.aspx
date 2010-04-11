<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.manageUser" CodeFile="manageUsers.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
	<HEAD>
		<title>MIT iLab Service Broker - Manage Users</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id: manageusers.aspx,v 1.1.1.1 2006/02/07 22:10:58 pbailey Exp $ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner><uc1:adminnav id="AdminNav1" runat="server"></uc1:adminnav><br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Manage Users
						</h1>
						<p>Add or modify a User below.</p>
						<!-- Errormessage should appear here:-->
						<!--div class="errormessage"--><asp:label id="lblResponse" EnableViewState="False" Visible="False" Runat="server"></asp:label>
						<!-- Manage Users Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message --></div>
					<!-- end pageintro div -->
					<div id="pagecontent">
						<div id="itemdisplay">
							<h4>Selected User</h4>
							<div class="message">
								<p><asp:label id="lblGroups" Runat="server"></asp:label></p>
								<p><asp:label id="lblRequestGroups" Runat="server"></asp:label></p>
							</div>
							<div class="simpleform">
								<table>
									<tr>
										<th>
											<label for="username">Username</label></th>
										<td><asp:textbox id="txtUsername" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="firstname">First Name </label>
										</th>
										<td><asp:textbox id="txtFirstName" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="lastname">Last Name</label></th>
										<td><asp:textbox id="txtLastName" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="email">Email</label></th>
										<td><asp:textbox id="txtEmail" Runat="server"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="affiliation">Affiliation</label></th>
										<td>
											<% if(ConfigurationSettings.AppSettings["useAffiliationDDL"].Equals("true")){ %>
											<asp:dropdownlist CssClass="i18n" id="ddlAffiliation" Runat="server" Width="171px"></asp:dropdownlist>
											<!--select name="affiliation" id="affiliation">
												<option value="0" selected>-- Make selection --</option>
												<option>item 1</option>
												<option>item 2</option>
											</select-->
											<% }else{ %>
											<asp:textbox id="txtAffiliation" Runat="server"></asp:textbox>
											<% } %>
										</td>
									</tr>
									<tr>
										<th>
											<label for="pword">Password</label></th>
										<td><asp:textbox id="txtPassword" Runat="server" Width="152px" TextMode="Password"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											<label for="confirmpword">Confirm Password </label>
										</th>
										&nbsp;
										<td><asp:textbox id="txtConfirmPassword" Runat="server" Width="152px" TextMode="Password"></asp:textbox></td>
									</tr>
									<tr>
										<th>
											&nbsp;</th>
										<td><asp:checkbox id="cbxLockAccount" Runat="server"></asp:checkbox><label for="lock">Lock 
												Account</label></td>
									</tr>
									<tr>
										<th colSpan="2">
											<asp:button id="btnSaveChanges" Runat="server" Text="Save Changes" onclick="btnSaveChanges_Click"></asp:button><asp:button id="btnRemove" Runat="server" Text="Remove" CssClass="button" onclick="btnRemove_Click"></asp:button><asp:button id="btnNew" Runat="server" Text="New" CssClass="button" onclick="btnNew_Click"></asp:button></th></tr>
								</table>
							</div>
						</div>
						<div class="simpleform"><label for="searchby">Search by</label><asp:dropdownlist CssClass="i18n" id="ddlSearchBy" Runat="server" Width="200">
								<asp:ListItem Value="-- select one --">-- select one --</asp:ListItem>
								<asp:ListItem Value="Username">Username</asp:ListItem>
								<asp:ListItem Value="Last Name">Last Name</asp:ListItem>
								<asp:ListItem Value="First Name">First Name</asp:ListItem>
								<asp:ListItem Value="Group">Group</asp:ListItem>
							</asp:dropdownlist>
							<br>
							<asp:textbox id="txtSearchBy" Runat="server"></asp:textbox><asp:button id="btnSearch" Runat="server" Text="Go" CssClass="button" onclick="btnSearch_Click"></asp:button></div>
						<div>&nbsp;</div>
						<div class="simpleform"><label for="selectauser">Select a User (Last Name, First Name - 
								Username)</label><br>
							<asp:listbox cssClass="i18n" id="lbxSelectUser" Runat="server" Width="256px" AutoPostBack="True" Rows="15" onselectedindexchanged="lbxSelectUser_SelectedIndexChanged"></asp:listbox></div>
					</div>
					<br clear="all">
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
		<DIV></DIV>
	</body>
</HTML>
