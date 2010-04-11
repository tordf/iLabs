<%@ Control Language="c#" Inherits="iLabs.ServiceBroker.admin.adminNav" CodeFile="adminNav.ascx.cs" %>
<div id="navbar"><div id="nav">
		<ul class="navlist" id="ulAdminNavList" runat="server">
			<li>
				<A class="first" href="../home.aspx">Home</A>
			<li>
				<A id="aMyGroups" runat="server" href="../myGroups.aspx">My Groups</A>
			<li>
				<A id="aMyLabs" runat="server" href="../myLabs.aspx">My Labs</A>
			<li>
				<A id="aManageServices" runat="server" href="manageServices.aspx">Services 
					&amp; Clients</A>
			<li>
				<A id="aManageUsers" href="manageUsers.aspx" runat="server">Users &amp; Groups</A>
			<li>
				<A id="aGrants" runat="server" href="grants.aspx">Grants</A>
			<li>
				<A id="aMappings" runat="server" href="adminResourceMappings.aspx">Resource Mappings</A>
			<li>
				<A id="aExperimentRecords" runat="server" href="experimentRecords.aspx">Records</A>
			<li>
				<A id="aMessages" class="last" runat="server" href="messages.aspx">Messages</A></li>
		</ul>
	</div>
	<!-- end nav div -->
	<div id="nav2">
		<ul class="navlist2">
			<li><a id="aHelp" runat="server">Help</a></li>
			<li><asp:LinkButton ID="lbtnLogout" Runat="server" onclick="lbtnLogout_Click">Log out</asp:LinkButton></li>
		</ul>
	</div>
	<!-- end nav2 div -->
	<div id="nav3">
		<ul id="ulNav3Labs" runat="server" class="navlist3">
			<!-- Lab Servers, Lab Clients, and Labs navigation -->
			<li>
				<A id="aNav3ServiceBrokerInfo" runat="server" href="SelfRegistration.aspx">Self Registration</A></li>
			<li>
				<A id="aNav3ManageServices" runat="server" href="manageServices.aspx">Manage Process Agents</A></li>
			<li>
				<A id="aNav3ManageLabClients" runat="server" href="manageLabClients.aspx">Manage Lab Clients</A></li>
			<li>
				<A id="aNav3ManageLabs" runat="server" href="Registration.aspx">Cross-domain Registration</A>
			</li>
		</ul>
		<ul id="ulNav3UsersGroups" runat="server" class="navlist3">
			<!-- Users and Groups navigation -->
			<li>
				<A id="aNav3ManageUsers" runat="server" href="manageUsers.aspx">Manage Users</A></li>
			<li>
				<A id="aNav3AdministerGroups" runat="server" href="administerGroups.aspx">Administer 
					Groups</A></li>
			<li>
				<A id="aNav3GroupMembership" runat="server" href="groupMembership.aspx">Group 
					Membership</A>
			</li>
		</ul>
		<ul id="ulNav3Records" runat="server" class="navlist3">
			<li>
				<a id="aNav3SBinfo" runat="server" href="sbStats.aspx">Service Broker Information</a></li>
			<li>
				<a id="aNav3Reports" runat="server" href="sbReport.aspx">Reports</a></li>
			<li>
				<A id="aNav3ExperimentRecords" runat="server" href="experimentRecords.aspx">Experiment 
					Records</A></li>
			<li>
				<A id="aNav3LoginRecords" runat="server" href="loginRecords.aspx">Log-in Records</A>
			</li>
		</ul>
	</div> <!-- end nav3 div -->
</div> <!-- end navbar -->
