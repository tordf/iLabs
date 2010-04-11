<%@ Register TagPrefix="uc1" TagName="footer" Src="../footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="adminNav" Src="adminNav.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="../banner.ascx" %>
<%@ Page language="c#" Inherits="iLabs.ServiceBroker.admin.manageLabClients" validateRequest="false" CodeFile="manageLabClients.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>MIT iLab Service Broker - Manage Lab Clients</title> 
		<!-- 
Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
Please see license.txt in top level directory for full license. 
-->
		<!-- $Id: managelabclients.aspx,v 1.16 2007/06/27 20:20:48 pbailey Exp $ -->
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
		<style type="text/css">@import url( ../css/main.css ); 
		</style>
		<script language="JavaScript" type="text/JavaScript">
	<!--

	function MM_openBrWindow(theURL,winName,features) { //v2.0
	 window.open(theURL,winName,features);
	}
	//-->
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<a name="top"></a><input id="hiddenPopupOnSave" type="hidden" name="hiddenPopupOnSave" runat="server" />
			<div id="outerwrapper">
				<uc1:banner id="Banner1" runat="server"></uc1:banner>
				<uc1:adminNav id="AdminNav1" runat="server"></uc1:adminNav>
				<br clear="all">
				<div id="innerwrapper">
					<div id="pageintro">
						<h1>Manage Lab Clients
						</h1>
						<p>Add, remove or modify a lab client below.
						</p>
						<p><asp:label id="lblResponse" Runat="server" Visible="False"></asp:label></p>
						<div id="Div1"  runat="server">
					<asp:CustomValidator ID="valGuid" ControlToValidate="txtClientGuid" OnServerValidate="checkGuid" 
                    Text="A Guid must be unique and no longer than 50 characters" runat="server"/>
					
					</div>
					</div><!-- end pageintro div -->
					<div id="pagecontent">
						<div id="actionbox-right" style="WIDTH: 224px; HEIGHT: 124px">
							<h3>Associated Lab Servers
							</h3>
							<ol>
								<asp:repeater id="repLabServers" Runat="server">
									<ItemTemplate>
										<li>
											<strong>Name:</strong>
											<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "AgentName"))%>
											<br>
											<strong>Desc:</strong>
											<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "ServiceURL"))%>
										</li>
									</ItemTemplate>
								</asp:repeater></ol>
							<asp:button id="btnEditList" Runat="server" Text="Edit List" CssClass="buttonright" CausesValidation="False" OnClick="btnEditList_Click"></asp:button><button id="btnRefresh" style="VISIBILITY: hidden" type="button" runat="server" onserverclick="btnRefresh_ServerClick"></button>
						</div>
						<div class="simpleform">
							<table cellSpacing="5" cellPadding="0" border="0">
								<tr>
									<th>
										<label for="labclient">Lab Client</label></th>
									<td style="width: 501px"><asp:dropdownlist CssClass="i18n" id="ddlLabClient" Runat="server" Width="496px" AutoPostBack="True" onselectedindexchanged="ddlLabClient_SelectedIndexChanged"></asp:dropdownlist></td>
								</tr>
								<tr>
									<th>
										<label for="labclientname">Lab Client Name</label></th>
									<td style="width: 501px"><asp:textbox id="txtLabClientName" Runat="server" Width="496px"></asp:textbox><asp:requiredfieldvalidator id="rfvLabClientName" Runat="server" ControlToValidate="txtLabClientName" ErrorMessage="You must enter the Lab Client Name"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<th>
										<label for="ClientGuid">Client GUID</label></th>
									<td style="width: 501px"><asp:textbox id="txtClientGuid" Runat="server" Width="384px"></asp:textbox>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnGuid" runat="server" Text="Make Guid" OnClick="btnGuid_Click" /></td>
								</tr>
								<tr>
									<th>
										<label for="version">Version</label></th>
									<td style="width: 501px"><asp:textbox id="txtVersion" Runat="server" Width="496px"></asp:textbox></td>
								</tr>
								<tr>
									<th>
										<label for="shordesc">Short Description </label>
									</th>
									<td style="width: 501px"><asp:textbox id="txtShortDesc" Runat="server" Width="496px"></asp:textbox></td>
								</tr>
								<tr>
									<th>
										<label for="longdesc">Long Description</label></th>
									<td style="width: 501px"><asp:textbox id="txtLongDesc" Runat="server" Width="496px" TextMode="MultiLine" Rows="3" Columns="20"></asp:textbox></td>
								</tr>
								<tr>
									<th>
										<label for="contactfirstname">Contact First Name</label></th>
									<td style="width: 501px"><asp:textbox id="txtContactFirstName" Runat="server" Width="496px"></asp:textbox></td>
								</tr>
								<tr>
									<th>
										<label for="contactlastname">Contact Last Name</label></th>
									<td style="width: 501px"><asp:textbox id="txtContactLastName" Runat="server" Width="496px"></asp:textbox></td>
								</tr>
								<tr>
									<th>
										<label for="contactemail">Contact Email</label></th>
									<td style="width: 501px"><asp:textbox id="txtContactEmail" Runat="server" Width="496px"></asp:textbox></td>
								</tr>
								<tr>
									<th>
										<label for="docurl">Documentation URL</label></th>
									<td style="width: 501px"><asp:textbox id="txtDocURL" Runat="server" Width="496px" TextMode="MultiLine"></asp:textbox></td>
								</tr>
								<tr>
									<th>
										<label for="notes">Notes</label></th>
									<td style="width: 501px"><asp:textbox id="txtNotes" Runat="server" Width="496px"></asp:textbox></td>
								</tr>
								<tr>
									<th>
										<label for="notes">Client Type</label></th>
									<td style="width: 501px"><asp:DropDownList CssClass="i18n" id="ddlClientTypes" Runat="server" Width="496px"></asp:DropDownList></td>
								</tr>
								<tr>
									<th>
										<label for="loaderscript">Loader Script</label></th>
									<td style="width: 501px"><asp:textbox id="txtLoaderScript" Runat="server" Width="496px" TextMode="MultiLine" Rows="5"></asp:textbox></td>
								</tr>
								<tr id="trNeedsESS">
									<th>
										<label for="needsExperimentStorage">Needs&nbsp;ESS</label></th>
									<td style="height: 26px; width: 501px;"><asp:checkBox id="cbxESS" Runat="server" Width="24px"></asp:checkBox>&nbsp;<asp:DropDownList CssClass="i18n" id="ddlAssociatedESS" Runat="server" Width="344px"></asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnRegisterESS" CssClass="button" runat="server" Text="Associate" OnClick="btnRegisterESS_Click" />
                                        <asp:Button ID="btnDissociateESS" CssClass="button" runat="server" Text="Dissociate" OnClick="btnDissociateESS_Click" />
                                        &nbsp; &nbsp; &nbsp;
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
                                    </td>
								</tr>
								<tr id="trNeedsUSS">
									<th>
										<label for="needsSchduling">Needs&nbsp;Scheduling</label></th>
									<td style="height: 26px; width: 501px;"><asp:checkbox id="cbxScheduling" Runat="server" Width="24px"></asp:checkbox>&nbsp;<asp:DropDownList CssClass="i18n" id="ddlAssociatedUSS" Runat="server" Width="344px"></asp:DropDownList>
									    &nbsp;&nbsp;&nbsp;
									    <asp:Button ID="btnRegisterUSS" runat="server" CssClass="button" OnClick="btnRegisterUSS_Click" Text="Associate" />
                                        <asp:Button ID="btnDissociateUSS" runat="server" CssClass="button" OnClick="btnDissociateUSS_Click" Text="Dissociate" />
                                         &nbsp; &nbsp; &nbsp;
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;
                                        </td>
								</tr>
								<tr id="trIsReentrant" runat="server" visible="true">
									<th style="height: 26px">
										<label for="isReentrant">Is Reentrant</label></th>
									<td style="width: 501px"><asp:checkbox id="cbxIsReentrant" Runat="server" Width="24px"></asp:checkbox></td>
								</tr>
								
								<tr>
									<th class="colspan" colSpan="2">
										Additional Lab Client Links
									</th>
								</tr>
								<tr>
									<td class="colspan" colSpan="2">
										<div class="unit">
											<table cellSpacing="0" cellPadding="0" border="0">
												<tr>
													<th class="top">
														Name
													</th>
													<th class="top">
														URL
													</th>
													<th class="top">
														URL Description</th></tr>
												<!-- TR in this nested table will repeat for every resource there is -->
												<asp:repeater id="repClientInfo" Runat="server">
													<ItemTemplate>
														<tr>
															<td><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "InfoURLName"))%></td>
															<td><A href='<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "InfoURL"))%>'target="_blank"><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "infoURL"))%></A></td>
															<td><%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Description"))%></td>
														</tr>
													</ItemTemplate>
												</asp:repeater>
											</table>
										</div>
									</td>
								</tr>
								<tr>
									<th colSpan="2">
                                        <asp:Button ID="btnAssociateGroups" runat="server" CssClass="button" Text="Associate Groups" />
										<asp:button id="btnAddEditResources" Runat="server" Text="Add/Edit Links" CssClass="button"
											Width="173px" onclick="btnAddEditResources_Click"></asp:button></th></tr>
								<tr>
									<th colSpan="2">
										<asp:button id="btnSaveChanges" Runat="server" Text="Save Changes" CssClass="button" onclick="btnSaveChanges_Click"></asp:button>
										<asp:button id="btnRemove" Runat="server" CssClass="button" Text="Remove" onclick="btnRemove_Click"></asp:button>
										<asp:button id="btnNew" Runat="server" Text="New" CssClass="button" onclick="btnNew_Click"></asp:button></th></tr>
							</table>
						</div>
						<br clear="all" />
						<!-- end pagecontent div -->
					</div>
					<!-- end innerwrapper div -->
				</div>
				<uc1:footer id="Footer1" runat="server"></uc1:footer>
			</div>
		</form>
	</body>
</html>
