<%@ Page language="c#" Inherits="iLabs.LabServer.LabView.labExperiments" CodeFile="labExperiments.aspx.cs" EnableEventValidation="false" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="userNav" Src="userNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RunLab</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">@import url( css/main.css );
		</style>
	</HEAD>
	<body>
		<form method="post" runat="server">
			<a name="top"></a>
			<div id="outerwrapper"><uc1:banner id="Banner1" runat="server"></uc1:banner>
			<uc1:userNav id="UserNav1" runat="server"></uc1:userNav><br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1><asp:label id="lblTitle" Runat="server">Configure Lab Experiments</asp:label></h1>
						<asp:label id="lblDescription" Runat="server"></asp:label>
						<p><asp:label id="lblErrorMessage" Runat="server" Visible="False"></asp:label></p>
					    <div id="Div1"  runat="server">
					        <asp:CustomValidator ID="valGuid" ControlToValidate="txtClientGuid" OnServerValidate="checkGuid" 
                            Text="A Guid must be unique and no longer than 50 characters" runat="server"/>
					    </div>
					</div>
					<!-- end pageintro div -->
					<div id="pagecontent">						
						<!-- Content goes here -->
						<p><asp:HyperLink id="lnkBackSB" Text="Back to InteractiveSB" runat="server" ></asp:HyperLink></p>	
						<div class="simpleform">
						    <form id="appInfo" action="" method="post" name="appInfo">
									<table style="WIDTH: 564px; HEIGHT: 460px" cellSpacing="0" cellPadding="5" border="0">
										<TBODY>
											<tr>
												<th style="width: 480px">
													<label for="appName">Lab Application</label></th>
												<td style="width: 484px"><asp:dropdownlist cssClass="i18n" id="ddlApplications" Runat="server" Width="360px" onselectedindexchanged="ddlApplications_SelectedIndexChanged" AutoPostBack="True"></asp:dropdownlist></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="title">Title</label></th>
												<td style="width: 484px"><asp:textbox id="txtTitle" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
									            <th>
										            <label for="ClientGuid">Client GUID</label></th>
									            <td><asp:textbox id="txtClientGuid" Runat="server" Width="260px"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnGuid" runat="server" Text="Make Guid" OnClick="btnGuid_Click" /></td>
								            </tr>
								            <tr runat="server">
												<th style="width: 480px">
													<label for="version">Version</label></th>
												<td style="width: 484px"><asp:textbox id="txtVersion" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr id="Tr1" runat="server">
												<th style="width: 480px">
													<label for="version">Revision</label></th>
												<td style="width: 484px"><asp:textbox id="txtRev" Runat="server" Width="360px"></asp:textbox></td>
											</tr><tr runat="server">
												<th style="width: 480px">
													<label for="applicationKey">Application Key</label></th>
												<td style="width: 484px"><asp:textbox id="txtAppKey" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="applicationPath">Path</label></th>
												<td style="width: 484px"><asp:textbox id="txtApplicationPath" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="application">Application</label></th>
												<td style="width: 484px"><asp:textbox id="txtApplication" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="webPageUrl">Web Page URL</label></th>
												<td style="width: 484px"><asp:textbox id="txtPageUrl" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="width">Application URL</label></th>
												<td style="width: 484px"><asp:textbox id="txtURL" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="width">Width</label></th>
												<td style="width: 484px"><asp:textbox id="txtWidth" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="height">Height</label></th>
												<td style="width: 484px"><asp:textbox id="txtHeigth" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="dataSources">Data Sources </label></th>
												<td style="width: 484px"><asp:textbox id="txtDataSources" Runat="server" Width="360px"></asp:textbox></td>
											</tr>		
											<tr>
												<th style="width: 480px">
													<label for="server">Server</label></th>
												<td style="width: 484px"><asp:textbox id="txtServer" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="port">Port</label></th>
												<td style="width: 484px"><asp:textbox id="txtPort" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="contactemail">Contact Email </label></th>
												<td style="width: 484px"><asp:textbox id="txtContactEmail" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="description">Description</label></th>
												<td style="width: 484px"><asp:textbox id="txtDescription" Runat="server" Columns="20" Rows="5" TextMode="MultiLine"
														Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="infoUrl">Info URL </label></th>
												<td style="width: 484px"><asp:textbox id="txtInfoUrl" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th style="width: 480px">
													<label for="comment">Comment</label></th>
												<td style="width: 484px"><asp:textbox id="txtComment" Runat="server" Width="360px"></asp:textbox></td>
											</tr> 
											<tr>
												<th style="width: 480px">
													<label for="port">Extra Data</label></th>
												<td style="width: 484px"><asp:textbox id="txtExtra" Runat="server" Width="360px"></asp:textbox></td>
											</tr>
											<tr>
												<th colSpan="2">
												    <asp:button id="btnSaveChanges" Runat="server" CssClass="button" Text="Save Changes" onclick="btnSaveChanges_Click"></asp:button>
                                                    &nbsp;&nbsp;<asp:button id="btnNew" Runat="server" CssClass="button" Text="Clear" onclick="btnNew_Click"></asp:button>
                                                     &nbsp;&nbsp;<asp:button id="btnDelete" Runat="server" CssClass="button" Text="Delete" onclick="btnDelete_Click"></asp:button>
                                                 </th>
                                            </tr>
											<tr>
												<th colSpan="2">
													</th></tr>
										</TBODY>
									</table>
								</form>
							</div>
					</div>
					<br clear="all">
					<!-- end pagecontent div --></div>
				<!-- end innerwrapper div --><uc1:footer id="Footer1" runat="server"></uc1:footer></div>
		</form>
	</body>
</HTML>
