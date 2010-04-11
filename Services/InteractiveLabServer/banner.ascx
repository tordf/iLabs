<%@ Control Language="c#" Inherits="iLabs.LabServer.LabView.banner" CodeFile="banner.ascx.cs" %>
<div id="topbanner" class="banner"><asp:Image id="imgBanner" ImageURL="~/images/MITiCampus_Logo_White.gif" width="50" height="36" borderwidth="0" Runat="server"
		Visible="false"></asp:Image>LabVIEW 8.6 Lab Server
	<div class="info">
		<ul>
			<li><asp:label id="lblUserNameBanner" Visible="False" Runat="server"></asp:label></li>
			<li><asp:label id="lblGroupNameBanner" Visible="False" Runat="server"></asp:label></li>
		</ul>
	</div>
</div>
