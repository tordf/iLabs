<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.addInfoUrlPopup" CodeFile="addInfoUrlPopup.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>addInfoUrlPopup</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id: addinfourlpopup.aspx,v 1.1.1.1 2006/02/07 22:10:58 pbailey Exp $ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../css/main.css" rel="stylesheet" type="text/css">
		<link href="../css/popup.css" rel="stylesheet" type="text/css">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="wrapper">
				<div id="pageintro">
					<h1>Additional Lab Client Resources for
						<br>
						<asp:Label ID="lblLabClient" Runat="server"></asp:Label>
					</h1>
					<p>Add, edit, reorder, and remove additional lab client resources below.
					</p>
					<div class="errormessage" id="divErrorMessage" runat="server">
						<p><asp:label id="lblResponse" Runat="server"></asp:label></p>
					</div>
				</div>
				<div id="pagecontent">
					<div id="sidedisplay">
						<h4>Add/Edit Resource</h4>
						<div class="simpleform">
							<table cellSpacing="0" cellPadding="0" border="0">
								<tr>
									<th>
										<label for="infoname">Info Name</label></th>
									<td style="WIDTH: 301px"><asp:textbox id="txtInfoname" Runat="server" Width="232px"></asp:textbox></td>
								</tr>
								<tr>
									<th>
										<label for="url">Link URL</label></th>
									<td style="WIDTH: 301px"><asp:textbox id="txtUrl" Runat="server" Width="232px"></asp:textbox>
									<td style="WIDTH: 89px"></td>
								</tr>
								<tr>
									<th>
										<label for="desc">Link
											<br>
											Description</label></th>
									<td style="WIDTH: 301px"><asp:textbox id="txtDesc" Runat="server" TextMode="MultiLine" Columns="20" Rows="3" Width="232px"></asp:textbox></td>
								</tr>
								<tr>
									<th style="WIDTH: 371px" colSpan="2">
										<asp:button id="btnSaveInfoChanges" Runat="server" Text="Save Changes" CssClass="button" onclick="btnSaveInfoChanges_Click"></asp:button>
										<asp:button id="btnNew" Runat="server" Text="New" CssClass="button" onclick="btnNew_Click"></asp:button>
									</th>
								</tr>
							</table>
							<input id="hiddenClientInfoIndex" style="VISIBILITY: hidden" runat="server" NAME="hiddenClientInfoIndex">
						</div>
						<!-- End simpleform div -->
						<h4>Reorder List</h4>
						<div class="simpleform">
							<table cellSpacing="0" cellPadding="0" border="0">
								<tr>
									<th style="WIDTH: 258px" colSpan="2">
										<label for="changeorder">Select an item to change its order</label></th></tr>
								<tr>
									<td style="WIDTH: 177px"><asp:listbox cssClass="i18n" id="lbxChangeOrder" Runat="server" Width="224px"></asp:listbox></td>
									<td class="buttonstyle" style="WIDTH: 131px"><asp:imagebutton id="ibtnMoveUp" Runat="server" CssClass="buttonstyle" Width="43" ImageUrl="../img/up-btn.gif"
											Height="22"></asp:imagebutton><br>
										<asp:imagebutton id="ibtnMoveDown" Runat="server" CssClass="buttonstyle" Width="57" ImageUrl="../img/down-btn.gif"
											Height="22"></asp:imagebutton></td>
								</tr>
								<tr>
									<th style="WIDTH: 258px" colSpan="2">
										<asp:button id="btnSaveOrderChanges" CssClass="buttonright" Text="Save Changes" Runat="server" onclick="btnSaveOrderChanges_Click"></asp:button></th></tr>
							</table>
						</div>
					</div>
					<!-- End simpleform Div --></div>
				<!-- End sidedisplay Div -->
				<h2>Additional Lab Client Resources List</h2>
				<div class="unit">
					<table cellSpacing="0" cellPadding="0" border="0">
						<asp:repeater id="repClientInfo" Runat="server">
							<ItemTemplate>
								<tr>
									<th>
										Desc
									</th>
									<td width="200"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Description"))%>
									</td>
									<td rowSpan="2">
										<asp:button id="btnEdit" CssClass="button" Text="Edit" CommandName="Edit" Runat="server"></asp:button>
										<asp:button id="btnRemove" CssClass="button" Text="Remove" CommandName="Remove" Runat="server"></asp:button></td>
								</tr>
								<tr>
									<th>
										Link</th>
									<td width="200"><A href='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "InfoURL"))%>'target="_blank"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "infoURLName"))%></A>
									</td>
								</tr>
							</ItemTemplate>
						</asp:repeater>
					</table>
				</div>
			</div>
			<DIV></DIV>
			<!-- End Page Content Div -->
			<DIV></DIV>
			<!-- End Wrapper Div--></form>
	</body>
</HTML>
