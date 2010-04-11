<%@ Register TagPrefix="uc1" TagName="NavBar" Src="NavBar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="banner" Src="banner.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="footer.ascx" %>
<%@ Page language="c#" Inherits="iLabs.Scheduling.LabSide.RevokeReservation" CodeFile="RevokeReservation.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>RevokeReservation</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="C#" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		
		<style type="text/css">@import url( css/main.css ); 
		</style>
	</head>
	<body>
	<div id="outerwrapper">
		<uc1:Banner id="Banner1" runat="server"></uc1:Banner>
		<uc1:NavBar id="NavBar1" runat="server"></uc1:NavBar>
		<br clear="all">
		<div id="innerwrapper">
		<div id="pageintro">
						<h1>Revoke Reservation
						</h1>
						<p><asp:label id="lblDescription" runat="server"></asp:label>
						</p>
						<!-- Administer Groups Error message here: <div class="errormessage"><p>Error message here.</p></div> End error message -->
						<p><asp:label id="lblErrorMessage" Runat="server" EnableViewState="False" Visible="False"></asp:label></p>
					</div>
					
					
					<div id="pagecontent">	
		<div class="simpleform"><form id="Form1" method="post" runat="server">
		<table>
		<tr>
		<th>
		<label for="startTime">Start Time</label>
		</th>
		<td>
		<a href="javascript:;" onclick="window.open('datePickerPopup.aspx?date=start','cal','width=250,height=225,left=270,top=180')">
	    <img src="calendar.gif" border="0" class="normal" /></a><asp:TextBox ID="txtStartDate" runat="server" Width="74px"></asp:TextBox>
           
		</td>
		<td style="width: 41px">
		<asp:DropDownList cssClass="i18n" ID="ddlStartHour" runat="server">
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
        </asp:DropDownList>
		</td>
		<td  style="width:25px">
		 <asp:TextBox ID="txtStartMin" runat="server" Width="24px" >00</asp:TextBox>
		</td>
		<td>
		<asp:DropDownList cssClass="i18n" ID="ddlStartAM" runat="server"  >
            <asp:ListItem>AM</asp:ListItem>
            <asp:ListItem>PM</asp:ListItem>
        </asp:DropDownList>
		</td>
		</tr>
		
		
		<tr>
		<th>
		<label for="endTime">End Time</label>
		</th>
		<td>
		<a href="javascript:;" onclick="window.open('datePickerPopup.aspx?date=end','cal','width=250,height=225,left=270,top=180')">
											<img src="calendar.gif" border="0" class="normal" /></a><asp:TextBox ID="txtEndDate" runat="server" Width="74px"></asp:TextBox>
           
		</td>
		<td style="width: 41px">
		<asp:DropDownList cssClass="i18n" ID="ddlEndHour" runat="server">
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
        </asp:DropDownList>
		</td>
		<td  style="width:25px">
		 <asp:TextBox ID="txtEndMin" runat="server" Width="25px" >00</asp:TextBox>
		</td>
		<td>
		<asp:DropDownList cssClass="i18n" ID="ddlEndAM" runat="server"  >
            <asp:ListItem>AM</asp:ListItem>
            <asp:ListItem>PM</asp:ListItem>
        </asp:DropDownList>
		</td>
		</tr>
		<tr>
	     <th> </th>
	     <td colspan="4">
	     <asp:button id="btnRevoke" runat="server" Text="Revoke Reservation" CssClass="button" onclick="btnRevoke_Click" ></asp:button>
	    	</td>
		</tr>
		</table>
			</form>
		</div>
		</div>
		</div>
		<uc1:footer id="Footer1" runat="server"></uc1:footer>
		</div>
					
	</body>
</html>
	
	
	
	
	
	
