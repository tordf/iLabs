<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Page language="c#" CodeFile="register.aspx.cs" AutoEventWireup="false" Inherits="iLabs.ServiceBroker.iLabSB.register" EnableEventValidation="true" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>MIT iLab Service Broker - Register</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id: register.aspx,v 1.2 2007/10/18 18:02:12 pbailey Exp $ -->
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" Content="C#" />
		<meta name="vs_defaultClientScript" content="JavaScript" />
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:userNav id="UserNav1" runat="server"></uc1:userNav>
				<br clear="all" />
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Register</h1>
						<p>Fill out the form below to register for an iLab account. You will be be emailed 
							a confirmation.</p>
						<!-- Errormessage should appear here:-->
						<!--div class="errormessage"-->
						<asp:ValidationSummary ID="ValSummmary" runat="server" ShowSummary="true" DisplayMode="List"
						HeaderText="Please correct the following erors." />
						<asp:Label Runat="server" id="lblResponse" Visible="False"></asp:Label>
						<!--/div--> <!--End error message -->
					</div> <!-- end pageintro div -->
					<div id="pagecontent">
						<!--div id="actionbox-right"-->
						<!--<h3>More Information</h3>
							<p class="message">You can <A href="#">view available labs here</A>.
							</p>
							<p class="message">You can <A href="#">view available groups here</A>.
							</p>-->
						<!--/div-->
						<div class="simpleform"><form id="register" name="register" method="post" action="">
								<table>
									<tr>
										<th>
											<label for="username">Username</label></th>
										<td><asp:textbox id="txtUsername" Runat="server"></asp:textbox>
											<!--input type="text" name="username" id="username"--></td>
									</tr>
									<tr>
										<th>
											<label for="firstname">First Name </label>
										</th>
										<td><asp:textbox id="txtFirstName" Runat="server"></asp:textbox>
											<!--input type="text" name="firstname" id="firstname"--></td>
									</tr>
									<tr>
										<th>
											<label for="lastname">Last Name </label>
										</th>
										<td><asp:textbox id="txtLastName" Runat="server"></asp:textbox>
											<!--input type="text" name="lastname" id="lastname"--></td>
									</tr>
									<tr>
										<th>
											<label for="email">Email</label></th>
										<td><asp:textbox id="txtEmail" Runat="server"></asp:textbox>
										<asp:RegularExpressionValidator ID = valEmail runat="server"
										ControlToValidate="txtEmail" ValidationExpression=".*@.*\..*"
										ErrorMessage="* The email address is not in the correct format!"
										Display="dynamic">*</asp:RegularExpressionValidator>
											<!--input type="text" name="email" id="email"-->
										</td>
									</tr>
									<tr>
										<th style="HEIGHT: 7px">
											<label for="affiliation">Affiliation</label></th>
										<td style="HEIGHT: 7px">
											<% if(ConfigurationSettings.AppSettings["useAffiliationDDL"].Equals("true")){ %>
											<asp:dropdownlist CssClass="i18n" id="ddlAffiliation" Runat="server"></asp:dropdownlist>
											<% }else{ %>
											<asp:TextBox id="txtAffiliation" Runat="server"></asp:TextBox>
											<% } %>
										</td>
									</tr>
									<tr id="trowRequestGroup" runat="server">
										<th>
											<label for="group">Requested Group </label>
										</th>
										<td><asp:dropdownlist CssClass="i18n" id="ddlGroup" Runat="server"></asp:dropdownlist>
										</td>
									</tr>
									<tr>
										<th>
											<label for="password">Password</label></th>
										<td><asp:textbox id="txtPassword" Runat="server" TextMode="Password"></asp:textbox>
											<!--input type="password" name="password" id="password"--></td>
									</tr>
									<tr>
										<th>
											<label for="passwordconfirm">Confirm Password </label>
										</th>
										<td><asp:textbox id="txtConfirmPassword" Runat="server" TextMode="Password"></asp:textbox>
											<!--input type="password" name="passwordconfirm" id="passwordconfirm"--></td>
									</tr>
									<tr>
										<th>
											<label for="purpose">Purpose for requesting account </label>
										</th>
										<td><asp:textbox id="txtReason" Runat="server" TextMode="MultiLine" Columns="30" Rows="6"></asp:textbox>
											<!--textarea name="purpose" cols="30" rows="6" id="purpose"></textarea--></td>
									</tr>
									<tr>
										<th colSpan="2">
											<asp:button id="btnSubmit" Runat="server" CssClass="buttonright" Text="Submit"></asp:button>
											<!--input type="submit" name="Submit" value="Submit" class="buttonright"--></th></tr>
								</table>
							</form>
						</div>
					</div>
					<br clear="all"/> <!-- end pagecontent div --></div> <!-- end innerwrapper div -->
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
		</form>
	</body>
</html>
