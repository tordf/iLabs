/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: manageLabGroups.aspx.cs,v 1.16 2008/05/09 21:04:16 pbailey Exp $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.PAgent;
using iLabs.Proxies.LSS;
using iLabs.Proxies.USS;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Mapping;
using iLabs.Ticketing;
using iLabs.UtilLib;



namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for manageLabs.
	/// </summary>
	public partial class manageLabGroups : System.Web.UI.Page
	{
        protected ArrayList adminGroupUserList = new ArrayList();

		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        BrokerDB issuer = new BrokerDB();

        LabClient[] labClients;
        LabClient theClient;
        int labClientID;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

			//only superusers can view this page
			if (!Session["GroupName"].ToString().Equals(Group.SUPERUSER))
				Response.Redirect("../home.aspx");

			
            //RefreshAdminUserGroupsRepeater();


            if (!IsPostBack)
            {
                // Error Message
                lblResponse.Visible = false;
                InitializeDropDown();
                if (Request.Params["lc"] != null && Request.Params["lc"].Length > 0)
                {
                    labClientID = int.Parse(Request.Params["lc"]);
                    if (labClientID > 0)
                    {
                        LabClient[] clients = wrapper.GetLabClientsWrapper(new int[] { labClientID });
                        if (clients.Length > 0)
                        {
                            theClient = clients[0];
                        }

                        ddlLabClient.SelectedValue = labClientID.ToString();
                        ddlLabClient.Enabled = false;
                        // Now that the Lab Client is known the associated/available Group
                        // ListBoxes can be loaded, along with the Lab Server Repeater
                        LoadListBoxes();
                        RefreshLabServerRepeater();
                        RefreshUssAndEssRepeaters();
                        RefreshAdminUserGroupsRepeater();

                        //btnSaveChanges.Enabled = true;
                    }
                }

                // Save Button
                //btnSaveChanges.Enabled = false;

                //RefreshAdminUserGroupsRepeater();


            }
            else
            {
                int id = int.Parse(ddlLabClient.SelectedValue);
                if (id > 0)
                {
                    LabClient[] clients = wrapper.GetLabClientsWrapper(new int[] { id });
                    if (clients.Length > 0 && clients[0].clientID > 0)
                    {
                        theClient = clients[0];
                    }
                }
            }

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

            this.repAdminUserGroups.ItemDataBound += new RepeaterItemEventHandler(this.repAdminUserGroups_ItemBound);
            this.repAdminUserGroups.ItemCommand += new RepeaterCommandEventHandler(this.repAdminUserGroups_ItemCommand);

		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			//this.ibtnAdd.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnAdd_Click);
			//this.ibtnRemove.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnRemove_Click);


		}
		#endregion
		/// <summary>
		/// Write Available and Associated Group List Boxes
		/// </summary>
		private void LoadListBoxes()
		{
			// Clear listboxes

            ddlAdminGroup.Items.Clear();
            //ddlLabServer.Items.Clear();
            ddlUserGroup.Items.Clear();
            if (theClient.needsScheduling)
            {
                ddlAdminGroup.Enabled = true;
                //Put in availabe admin groups
                ListItem liHeaderAdminGroup = new ListItem("---Select Management Group---", "-1");
                ddlAdminGroup.Items.Add(liHeaderAdminGroup);
                int[] admgroupIDs = AdministrativeAPI.ListAdminGroupIDs();
                Group[] gps = AdministrativeAPI.GetGroups(admgroupIDs);
                foreach (Group group in gps)
                {
                    ListItem li = new ListItem(group.groupName, group.groupID.ToString());
                    ddlAdminGroup.Items.Add(li);
                }
            }
            else
            {
                ddlAdminGroup.Enabled = false;
            }
            ListItem liHeaderUserGroup = new ListItem("---Select User Group---", "-1");
            ddlUserGroup.Items.Add(liHeaderUserGroup);

			// Get All Groups
			int[] groupIDs = wrapper.ListGroupIDsWrapper();
			Group[] availGroups = wrapper.GetGroupsWrapper(groupIDs);
			
			// Get Associated Groups (i.e. associated to a specified Lab Client)
			int[] assocGroupIDs = AdministrativeUtilities.GetLabClientGroups(Convert.ToInt32(ddlLabClient.SelectedValue));
			ArrayList assocGroupList = new ArrayList(assocGroupIDs);


			foreach(Group availGroup in availGroups)
			{
				string gName = availGroup.groupName;
				//Dont populate Root, SuperUser, NewUser and OrphanedUser groups
				if ((availGroup.groupID>0)&&(!gName.Equals(Group.ROOT))
					&&(!gName.Equals(Group.SUPERUSER))&&(!gName.Equals(Group.NEWUSERGROUP))
					&&(!gName.Equals(Group.ORPHANEDGROUP)))
				{
					// Don't write Groups that are already associated with the client
					// to the Available List box
					if(assocGroupList.Contains(availGroup.groupID))
					{
						//Write to Associated Listbox if already associated.
						//lbxAssociated.Items.Add(new ListItem(availGroup.groupName,availGroup.groupID.ToString()));
                        
					}
					else
					{
						//Write to available Listbox
						//lbxAvailable.Items.Add(new ListItem(availGroup.groupName, availGroup.groupID.ToString()));
                        ddlUserGroup.Items.Add(new ListItem(availGroup.groupName, availGroup.groupID.ToString()));
					}
				}
			}

		} // END private void LoadListBoxes()

		/// <summary>
		/// Clears the Lab Client dropdown and reloads it from the array of LabClient objects
		/// </summary>
		private void InitializeDropDown()
		{

			int[] labClientIDs = wrapper.ListLabClientIDsWrapper();
			labClients = wrapper.GetLabClientsWrapper(labClientIDs);
			
			ddlLabClient.Items.Clear();

			ddlLabClient.Items.Add(new ListItem(" --- select Lab Client --- ","-1"));
			
			foreach (LabClient lc in labClients)
			{
				ListItem li = new ListItem(lc.clientName,lc.clientID.ToString());
				ddlLabClient.Items.Add(li);
			}
              

		}

		/// <summary>
		/// Creates an ArrayList of LabServer objects.
		/// Binds the LabServer Repeater to this ArrayList.
		/// </summary>
		private void RefreshLabServerRepeater()
		{
            //LabClient lc = new LabClient();
            //lc = wrapper.GetLabClientsWrapper(new int[] {Convert.ToInt32(ddlLabClient.SelectedValue)})[0];
            ////lc = labClients[ddlLabClient.SelectedIndex - 1];

			ArrayList labServersList = new ArrayList();
            //ddlLabServer.Items.Clear();
            repLabServers.DataSource = null;
            repLabServers.DataBind();
            if (theClient.clientID >0)
            {
                if (theClient.labServerIDs != null && theClient.labServerIDs.Length > 0)
                {
                    ProcessAgentInfo[] labServers = wrapper.GetProcessAgentInfosWrapper(theClient.labServerIDs);
                    if (labServers.Length > 0)
                    {
                        //if (labServers.Length > 1)
                        //{
                        //    ddlLabServer.Items.Add(new ListItem("-- Select Lab Server --", "0"));
                        //}
                        foreach (ProcessAgentInfo ls in labServers)
                        {
                            if(!ls.retired)
                                labServersList.Add(ls);
                        }
                        repLabServers.DataSource = labServersList;
                        repLabServers.DataBind();
                    }
                }
            }		
		}

        private void RefreshUssAndEssRepeaters()
        {
            //int clientID = Convert.ToInt32(ddlLabClient.SelectedValue);
            repUSS.DataSource = "";
            if (theClient.needsScheduling)
            {
                int ussId = issuer.FindProcessAgentIdForClient(theClient.clientID, ProcessAgentType.SCHEDULING_SERVER);
                if (ussId > 0)
                {
                    ArrayList ussList = new ArrayList();
                    ussList.Add(issuer.GetProcessAgentInfo(ussId)); ;
                    repUSS.DataSource = ussList;
                    repUSS.DataBind();
                }
            } 
            
            repUSS.DataBind();
            repESS.DataSource = "";
            if (theClient.needsESS)
            {
                int essId = issuer.FindProcessAgentIdForClient(theClient.clientID, ProcessAgentType.EXPERIMENT_STORAGE_SERVER);
                if (essId > 0)
                {
                    ArrayList essList = new ArrayList();
                    essList.Add(issuer.GetProcessAgentInfo(essId)); ;

                    repESS.DataSource = essList;
                }
            }   
            repESS.DataBind();
            
        }

        private void RefreshAdminUserGroupsRepeater()
        {
            try
            {
                repAdminUserGroups.DataSource = null;
                //ArrayList temp = adminGroupUserList;
                adminGroupUserList = GetGroupManagerUserMaps();
                repAdminUserGroups.DataSource = adminGroupUserList;
                repAdminUserGroups.DataBind();

            }
            catch (Exception ex)
            {
                lblResponse.Text = Utilities.FormatErrorMessage("Cannot list Group Manager User Map. " + ex.GetBaseException());
                lblResponse.Visible = true;
            }
        }

        /// <summary>
        /// method to get all maps between Manage and User Groups
        /// </summary>
        /// <returns></returns>
        private ArrayList GetGroupManagerUserMaps()
        {
            ArrayList mapList = new ArrayList();
            if (theClient.clientID > 0)
            {
                // Only get the authorizing groups
                int[] assocGroupIDs = AdministrativeUtilities.GetLabClientGroups(theClient.clientID,false);
                Group[] assocGroups = wrapper.GetGroupsWrapper(assocGroupIDs);

               // if (theClient.needsScheduling)
               // {
                    int ussId = issuer.FindProcessAgentIdForClient(theClient.clientID, ProcessAgentType.SCHEDULING_SERVER);

                    if (ussId > 0) // HasScheduling 
                    {
                        ArrayList ussList = new ArrayList();
                        ussList.Add(issuer.GetProcessAgentInfo(ussId)); ;

                        repUSS.DataSource = ussList;
                        repUSS.DataBind();
                    


                    ResourceMappingValue[] values = new ResourceMappingValue[2];
                    values[0] = new ResourceMappingValue(ResourceMappingTypes.CLIENT, theClient.clientID);
                    values[1] = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE, TicketTypes.GetTicketType(TicketTypes.MANAGE_USS_GROUP));
                    //values[1] = new ResourceMappingValue(ResourceMappingTypes.PROCESS_AGENT, ussId);
                    
                    
                    foreach (Group g in assocGroups)
                    {
                        ResourceMappingKey rmKey = new ResourceMappingKey(ResourceMappingTypes.GROUP, g.groupID);
                        List<int> rmIds = ResourceMapManager.FindMapIds(rmKey, values);
                        if (rmIds != null)
                        {
                            foreach (int rm in rmIds)
                            {
                                int qualId = AuthorizationAPI.GetQualifierID(rm, Qualifier.resourceMappingQualifierTypeID);
                                int[] grantIDs = AuthorizationAPI.FindGrants(-1, Function.manageUSSGroup, qualId);
                                if (grantIDs.Length > 0)
                                {
                                    Grant[] grants = AuthorizationAPI.GetGrants(grantIDs);
                                    foreach (Grant grant in grants)
                                    {
                                        Group[] managerGroups = AdministrativeAPI.GetGroups(new int[] { grant.agentID });
                                        foreach (Group mg in managerGroups)
                                        {
                                            mapList.Add(new GroupManagerUserMap(mg.groupName, g.groupName, grant.grantID, rm));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else // No scheduling
                {
                    foreach (Group g in assocGroups)
                    {
                        mapList.Add(new GroupManagerUserMap("", g.groupName, 0, 0));
                    }
               }
            }
            //return the array list of "ManagerGroup" -> "UserGroup"
            return mapList;
        }

        protected void ddlLabClient_SelectedIndexChanged(object sender, System.EventArgs e)
        {
         
            int id = int.Parse(ddlLabClient.SelectedValue);
            if (id > 0)
            {
                LabClient[] clients = wrapper.GetLabClientsWrapper(new int[] { id });
                if (clients.Length > 0 && clients[0].clientID > 0)
                {
                    theClient = clients[0];
                }
                else
                {
                    theClient = new LabClient();
                    repLabServers.DataSource = "";
                    repLabServers.DataBind();

                    repESS.DataSource = "";
                    repESS.DataBind();

                    repUSS.DataSource = "";
                    repUSS.DataBind();
                }

                // Now that the Lab Client is known the associated/available Group
                // ListBoxes can be loaded, along with the Lab Server Repeater
                LoadListBoxes();
                RefreshLabServerRepeater();
                RefreshUssAndEssRepeaters();
                RefreshAdminUserGroupsRepeater();

                //btnSaveChanges.Enabled = true;
            }
            else
            {
                //lbxAssociated.Items.Clear();
                //lbxAvailable.Items.Clear();
                repLabServers.DataSource = "";
                repLabServers.DataBind();

                repESS.DataSource = "";
                repESS.DataBind();

                repUSS.DataSource = "";
                repUSS.DataBind();

                //btnSaveChanges.Enabled = false;
            }
        }

        private void repAdminUserGroups_ItemBound(Object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                GroupManagerUserMap gm = (GroupManagerUserMap)e.Item.DataItem;
                Label lbl = (Label)e.Item.FindControl("lblUserGroup");
                
                    lbl.Text = gm.UserGroupName;
                    object obj = e.Item.FindControl("tdRemove");
                HtmlTableCell tc = (HtmlTableCell)e.Item.FindControl("tdRemove");
                obj = e.Item.FindControl("trManagement");
                HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trManagement");
                if(gm.ManagerGroupName !=null && gm.ManagerGroupName.Length >0){
                    tc.RowSpan = 2;
                    lbl = (Label)e.Item.FindControl("lblManageGroup");
                    lbl.Text = gm.ManagerGroupName;
                    tr.Visible = true;

                }
                else{
                    tc.RowSpan = 1;
                    tr.Visible = false;
                }   
                Button curBtn = (Button)e.Item.FindControl("btnRemove");
                
                curBtn.CommandArgument = gm.ToCSV(); 
            }

        }

      

        ///// <summary>
        ///// Save Button.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnSaveChanges_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        //Get the labClient ID from the dropdown
        //        int lcID = Convert.ToInt32(ddlLabClient.SelectedValue);
        //        //Get lab client information
        //        LabClient[] clients = wrapper.GetLabClientsWrapper(new int[]{lcID});

			
        //        /*Update Lab Client grants*/

        //        //Get qualifier for labclient
        //        int lcQualifierID = AuthorizationAPI.GetQualifierID(lcID,Qualifier.labClientQualifierTypeID);
        //        //Get all "uselabclient" grants for this labclient
        //        int[] lcGrantIDs = wrapper.FindGrantsWrapper(-1,Function.useLabClientFunctionType,lcQualifierID);
        //        Grant[] lcGrants = wrapper.GetGrantsWrapper(lcGrantIDs);
        //        //Get list of agents that can use labclient
        //        ArrayList lcAgents = new ArrayList();
        //        foreach (Grant g in lcGrants)
        //            lcAgents.Add(g.agentID);

        //        foreach (ListItem li in lbxAssociated.Items)
        //        {
        //            int groupID = Convert.ToInt32(li.Value);
        //            //if that agent doesn't already have the uselabclient permission
        //            if (!lcAgents.Contains(groupID))
        //                //add lab client grant
        //                wrapper.AddGrantWrapper(groupID, Function.useLabClientFunctionType, lcQualifierID);
        //            else
        //                //otherwise just remove it from the set of agents that we need to keep track of
        //                lcAgents.Remove(groupID);
        //        }

        //        //Need to delete the grant for remaining agents (since they've been removed by user!)
        //        ArrayList grantsToBeRemoved = new ArrayList();
        //        foreach (Grant g in lcGrants)
        //            if (lcAgents.Contains(g.agentID))
        //                grantsToBeRemoved.Add(g.grantID);
			

        //        /*Update Lab Servers grant*/

        //        //Update grants for each of the associated lab servers
        //        foreach (int lsID in clients[0].labServerIDs)
        //        {
        //            //Get qualifier for labserver
        //            int lsQualifierID = AuthorizationAPI.GetQualifierID(lsID,Qualifier.labServerQualifierTypeID);
        //            //Get all "uselabserver" grants for this labserver
        //            int[] lsGrantIDs = wrapper.FindGrantsWrapper(-1,Function.useLabServerFunctionType,lsQualifierID);
        //            Grant[] lsGrants = wrapper.GetGrantsWrapper(lsGrantIDs);
        //            //Get list of agents that can use labserver
        //            ArrayList lsAgents = new ArrayList();
        //            foreach (Grant g in lsGrants)
        //                lsAgents.Add(g.agentID);

        //            foreach (ListItem li in lbxAssociated.Items)
        //            {
        //                int groupID = Convert.ToInt32(li.Value);
        //                //if that agent doesn't already have the uselabclient permission
        //                if (!lsAgents.Contains(groupID))
        //                    //add lab server grant
        //                    wrapper.AddGrantWrapper(groupID, Function.useLabServerFunctionType, lsQualifierID);
        //                else
        //                    //otherwise just remove it from the set of agents that we need to keep track of
        //                    lsAgents.Remove(groupID);
        //            }

        //            //Need to delete the grant for remaining agents (since they've been removed by user!)
        //            foreach (Grant g in lsGrants)
        //                if (lsAgents.Contains(g.agentID))
        //                    grantsToBeRemoved.Add(g.grantID);
        //        }

        //        //Delete All Grants that neeed to be removed
        //        wrapper.RemoveGrantsWrapper(Utilities.ArrayListToIntArray(grantsToBeRemoved));
        //        LoadListBoxes();

        //        lblResponse.Visible = true;
        //        lblResponse.Text = Utilities.FormatConfirmationMessage("Lab client (& corresponding lab servers) successfully updated. ");
        //    }
        //    catch (Exception ex)
        //    {
        //        lblResponse.Visible = true;
        //        lblResponse.Text = Utilities.FormatErrorMessage("Cannot update Lab Client. " + ex.GetBaseException());
        //    }
		
        //} // Save Button
       
        protected void repAdminUserGroups_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //RefreshAdminUserGroupsRepeater();
            //adminGroupUserList = GetGroupManagerUserMaps();
            int labServerID = 0;
            try{
                if (e.CommandName.Equals("Remove"))
                {
                    //example:issuer.DeleteAdminURL(Int32.Parse(e.CommandArgument.ToString()))
                    GroupManagerUserMap gm = GroupManagerUserMap.Parse(e.CommandArgument.ToString());
                    int userGroupID = wrapper.GetGroupIDWrapper(gm.UserGroupName);
                    string clientFunction = Function.useLabClientFunctionType;
                    string labFunction = Function.useLabServerFunctionType;

                    if (theClient.clientID > 0)
                    {
                        //If there is a USS Manager Group
                        if(gm.GrantID > 0 && gm.ResourceMappingID >0){
                            wrapper.RemoveGrantsWrapper(new int[] { gm.GrantID });
                            //delete Resource Mapping [User Group] <-> [USS]
                            issuer.DeleteResourceMapping(gm.ResourceMappingID);
                        } // End is a USS Manager Group

                        //delete the Grant [User Group] -> USE LAB CLIENT -> lab client
                        int clientQualifierID = AuthorizationAPI.GetQualifierID(theClient.clientID, Qualifier.labClientQualifierTypeID);
                        if (clientQualifierID > 0)
                        {
                            int [] clientGrants = wrapper.FindGrantsWrapper(userGroupID, clientFunction, clientQualifierID);
                            if((clientGrants != null) && (clientGrants.Length >0) && (clientGrants[0] > 0))
                                wrapper.RemoveGrantsWrapper(new int[] { clientGrants[0] });

                            if ((theClient.labServerIDs != null) && (theClient.labServerIDs.Length > 0)
                                && (theClient.labServerIDs[0] > 0))
                            {
                                labServerID = theClient.labServerIDs[0];
                                int remainingClients = AdministrativeAPI.CountServerClients(userGroupID, labServerID);
                                if (remainingClients == 0)
                                {
                                    //delete the Grant [User Group] -> USE LAB SERVER -> lab Server
                                    int labQualifierID = AuthorizationAPI.GetQualifierID(labServerID, Qualifier.labServerQualifierTypeID);
                                    if (labQualifierID > 0)
                                    {
                                        int [] labGrants = (wrapper.FindGrantsWrapper(userGroupID, labFunction, labQualifierID));
                                        if((labGrants != null) && (labGrants.Length >0) && (labGrants[0] > 0))
                                       
                                            wrapper.RemoveGrantsWrapper(new int[] { labGrants[0] });
                                    }
                                }
                            }
                        }         
                    }
                    RefreshAdminUserGroupsRepeater();
                    LoadListBoxes();
                } 
            }
            catch (Exception exc)
            {
                lblResponse.Visible = true;
                lblResponse.Text = Utilities.FormatErrorMessage("Cannot Remove User Group. " + exc.GetBaseException());
                Utilities.WriteLog(exc.Message);
            }
        }


        protected void repAdminUserGroups_ItemCommand_ORIG(object source, RepeaterCommandEventArgs e)
        {
            //RefreshAdminUserGroupsRepeater();
            adminGroupUserList = GetGroupManagerUserMaps();
            int labServerID = 0;
            try
            {
                if (e.CommandName.Equals("Remove"))
                {
                    //example:issuer.DeleteAdminURL(Int32.Parse(e.CommandArgument.ToString()))
                    string userGroupName = e.CommandArgument.ToString();

                    int userGroupID = wrapper.GetGroupIDWrapper(userGroupName);
                    string clientFunction = Function.useLabClientFunctionType;
                    string labFunction = Function.useLabServerFunctionType;

                    if (theClient.clientID > 0)
                    {
                        //if (theClient.needsScheduling)
                        //{
                        string managerGroupName = null;
                        int mapGrantID = 0, mapResourceMappingID = 0;
                        GroupManagerUserMap groupMap = new GroupManagerUserMap();

                        for (int i = 0; i < adminGroupUserList.Count; i++)
                        {
                            groupMap = (GroupManagerUserMap)adminGroupUserList[i];
                            if (groupMap.UserGroupName.Equals(userGroupName))
                            {
                                managerGroupName = groupMap.ManagerGroupName;
                                mapGrantID = groupMap.GrantID;
                                mapResourceMappingID = groupMap.ResourceMappingID;
                                break;
                            }
                        }



                        //delete Grant [Manager Group] -> [MANAGE_USS_GROUP] -> [Rsrc Mapping{[User Group] <-> [USS]}]
                        if (mapGrantID > 0)
                            wrapper.RemoveGrantsWrapper(new int[] { mapGrantID });

                        //delete Resource Mapping [User Group] <-> [USS]
                        if (mapResourceMappingID > 0)
                            issuer.DeleteResourceMapping(issuer.GetResourceMapping(mapResourceMappingID));
                        //} End needs scheduling

                        //delete the Grant [User Group] -> USE LAB CLIENT -> lab client
                        int clientQualifierID = AuthorizationAPI.GetQualifierID(theClient.clientID, Qualifier.labClientQualifierTypeID);
                        if (clientQualifierID > 0)
                        {
                            int[] clientGrants = wrapper.FindGrantsWrapper(userGroupID, clientFunction, clientQualifierID);
                            if ((clientGrants != null) && (clientGrants.Length > 0) && (clientGrants[0] > 0))
                                wrapper.RemoveGrantsWrapper(new int[] { clientGrants[0] });

                            if ((theClient.labServerIDs != null) && (theClient.labServerIDs.Length > 0)
                                && (theClient.labServerIDs[0] > 0))
                            {
                                labServerID = theClient.labServerIDs[0];
                                int remainingClients = AdministrativeAPI.CountServerClients(userGroupID, labServerID);
                                if (remainingClients == 0)
                                {
                                    //delete the Grant [User Group] -> USE LAB SERVER -> lab Server
                                    int labQualifierID = AuthorizationAPI.GetQualifierID(labServerID, Qualifier.labServerQualifierTypeID);
                                    if (labQualifierID > 0)
                                    {
                                        int[] labGrants = (wrapper.FindGrantsWrapper(userGroupID, labFunction, labQualifierID));
                                        if ((labGrants != null) && (labGrants.Length > 0) && (labGrants[0] > 0))

                                            wrapper.RemoveGrantsWrapper(new int[] { labGrants[0] });
                                    }
                                }
                            }
                        }

                    }
                    RefreshAdminUserGroupsRepeater();
                    LoadListBoxes();
                }
            }
            catch (Exception exc)
            {
                Utilities.WriteLog(exc.Message);
            }
        }

        // Error recovery must be added to this method
        protected void btnSaveChange_Click(object sender, EventArgs e)
        {
            int id = int.Parse(ddlLabClient.SelectedValue);
            if (id > 0)
            {
                LabClient[] clients = wrapper.GetLabClientsWrapper(new int[] { id });
                if (clients.Length > 0 && clients[0].clientID > 0)
                {
                    theClient = clients[0];
                }
            }
            // objects used in try block may be required for recovery if  errors
            bool noError = true;
            ProcessAgentInfo labServer = null;
            ProcessAgentInfo uss = null;
            ProcessAgentInfo lss = null;
            long rmUss = 0;

            lblResponse.Visible = false;
            lblResponse.Text = "";
            try
            {
                if (ddlUserGroup.SelectedIndex <= 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatWarningMessage("Please select a user group from the corresponding drop-down list.");
                    return;
                }
                int groupID = Convert.ToInt32(ddlUserGroup.SelectedValue);
                if (groupID <= 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatWarningMessage("The user group is invalid.");
                    return;
                }
                //Get the user Group Name to be displayed in the repeater
                Group[] userGroup = wrapper.GetGroupsWrapper(new int[] { groupID });
                string userGroupName = userGroup[0].GroupName;

                if (theClient.labServerIDs != null && theClient.labServerIDs.Length > 0)
                {
                    labServer = issuer.GetProcessAgentInfo(theClient.labServerIDs[0]);
                }
                if (labServer == null)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatWarningMessage("Lab Client should first be associated with a Lab Server");
                    return;
                }

                if (theClient.needsScheduling)
                {
                    if (ddlAdminGroup.SelectedIndex <= 0 || ddlUserGroup.SelectedIndex <= 0)
                    {
                        lblResponse.Visible = true;
                        lblResponse.Text = Utilities.FormatWarningMessage("Please select a user and management group from the corresponding drop-down lists.");
                        return;
                    }
                    int manageID = Convert.ToInt32(ddlAdminGroup.SelectedValue);
                    if (manageID <= 0)
                    {
                        lblResponse.Visible = true;
                        lblResponse.Text = Utilities.FormatWarningMessage("The management group is invalid.");
                        return;
                    }


                    int ussId = issuer.FindProcessAgentIdForClient(theClient.clientID, ProcessAgentType.SCHEDULING_SERVER);
                    if (ussId <= 0)
                    {
                        lblResponse.Visible = true;
                        lblResponse.Text = Utilities.FormatWarningMessage("Lab Client should first be associated with a USS");
                        return;
                    }
                    int lssId = issuer.FindProcessAgentIdForAgent(theClient.labServerIDs[0], ProcessAgentType.LAB_SCHEDULING_SERVER);
                    if (lssId <= 0)
                    {
                        lblResponse.Visible = true;
                        lblResponse.Text = Utilities.FormatWarningMessage("Lab Server should first be associated with an LSS");
                        return;
                    }

                    else
                    {

                        uss = issuer.GetProcessAgentInfo(ussId);
                        if (uss.retired)
                        {
                            throw new Exception("The USS is retired");
                        }
                        lss = issuer.GetProcessAgentInfo(lssId);
                        if (lss.retired)
                        {
                            throw new Exception("The LSS is retired");
                        }


                        //Object keyObj = groupID;
                        //string keyType = ResourceMappingTypes.GROUP;
                        ResourceMappingKey key = new ResourceMappingKey(ResourceMappingTypes.GROUP, groupID);
                        ResourceMappingValue[] values = new ResourceMappingValue[3];
                        values[0] = new ResourceMappingValue(ResourceMappingTypes.CLIENT, theClient.clientID);
                        values[1] = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                            TicketTypes.GetTicketType(TicketTypes.MANAGE_USS_GROUP));
                        values[2] = new ResourceMappingValue(ResourceMappingTypes.GROUP,manageID);
                        
                       
                        //values[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.SCHEDULING_SERVER);
                        //values[1] = new ResourceMappingValue(ResourceMappingTypes.PROCESS_AGENT, ussId);
                        


                        ResourceMapping newMapping = issuer.AddResourceMapping(key, values);
                        rmUss = newMapping.MappingID;

                        // add mapping to qualifier list
                        int qualifierType = Qualifier.resourceMappingQualifierTypeID;
                        string name = issuer.ResourceMappingToString(newMapping);
                        int qualifierID = AuthorizationAPI.AddQualifier(newMapping.MappingID, qualifierType, name, Qualifier.ROOT);

                        //Give the Manager Group a Grant "MANAGE_USS_GROUP" on the created resource mapping                        
                        string function = Function.manageUSSGroup;
                        int grantID = wrapper.AddGrantWrapper(manageID, function, qualifierID);

                        //Get the management Group Name to be displayed in the repeater
                        Group[] adminGroup = wrapper.GetGroupsWrapper(new int[] { manageID });
                        string adminGroupName = adminGroup[0].GroupName;

                        //Create the Map between user and management group
                        GroupManagerUserMap groupMap = new GroupManagerUserMap(adminGroupName, userGroupName, grantID, newMapping.MappingID);

                        TicketLoadFactory factory = TicketLoadFactory.Instance();
                        //long duration = 60;
                        // Get this SB's domain Guid
                        string domainGuid = ProcessAgentDB.ServiceGuid;
                        string sbName = ProcessAgentDB.ServiceAgent.agentName;

                        //Add Credential set on the USS

                        //string ussPayload = factory.createAdministerUSSPayload(Convert.ToInt32(Session["userTZ"]));
                        //Coupon ussCoupon = issuer.CreateTicket(TicketTypes.ADMINISTER_USS, uss.agentGuid,
                        //    issuer.GetIssuerGuid(), duration, ussPayload);
                        UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                        ussProxy.Url = uss.webServiceUrl;
                        ussProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                        ussProxy.AgentAuthHeaderValue.coupon = uss.identOut;
                        ussProxy.AgentAuthHeaderValue.agentGuid = domainGuid;

                        ussProxy.AddCredentialSet(domainGuid, sbName, userGroupName);

                        // Check for existing RevokeReservation ticket redeemer = USS, sponsor = LSS
                        Coupon[] revokeCoupons = issuer.RetrieveIssuedTicketCoupon(
                            TicketTypes.REVOKE_RESERVATION, uss.agentGuid, lss.agentGuid);
                        Coupon revokeCoupon = null;
                        if (revokeCoupons != null && revokeCoupons.Length > 0)
                        {
                            revokeCoupon = revokeCoupons[0];
                        }
                        else
                        {
                            // Create RevokeReservation ticket
                            revokeCoupon = issuer.CreateTicket(TicketTypes.REVOKE_RESERVATION,
                                 uss.agentGuid, lss.agentGuid, -1L, factory.createRevokeReservationPayload());
                        }

                        //Add Credential set on the LSS

                        // check if this domain
                        if (ProcessAgentDB.ServiceGuid.Equals(lss.domainGuid))
                        {
                            LabSchedulingProxy lssProxy = new LabSchedulingProxy();
                            lssProxy.Url = lss.webServiceUrl;
                            lssProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                            lssProxy.AgentAuthHeaderValue.coupon = lss.identOut;
                            lssProxy.AgentAuthHeaderValue.agentGuid = domainGuid;
                            // Add the USS to the LSS, this may be called multiple times with duplicate data
                            lssProxy.AddUSSInfo(uss.AgentGuid, uss.agentName, uss.webServiceUrl, revokeCoupon);
                            int credentialSetAdded = lssProxy.AddCredentialSet(domainGuid,
                                sbName, userGroupName, uss.agentGuid);
                        }
                        else
                        { // Cross-Domain Registration needed
                            ProcessAgentInfo remoteSB = issuer.GetProcessAgentInfo(lss.domainGuid);
                            if(remoteSB.retired){
                                throw new Exception("The remote service broker is retired");
                            }
                            ResourceDescriptorFactory resourceFactory = ResourceDescriptorFactory.Instance();
                            string ussDescriptor = resourceFactory.CreateProcessAgentDescriptor(ussId);
                            string lssDescriptor = resourceFactory.CreateProcessAgentDescriptor(lssId);
                            string groupDescriptor = resourceFactory.CreateGroupCredentialDescriptor(domainGuid, sbName, userGroupName, uss.agentGuid, lss.agentGuid);

                            ServiceDescription[] info = new ServiceDescription[3];
                            info[0] = new ServiceDescription(null, revokeCoupon, ussDescriptor);
                            info[1] = new ServiceDescription(null, null, lssDescriptor);
                            info[2] = new ServiceDescription(null, null, groupDescriptor);

                            ProcessAgentProxy sbProxy = new ProcessAgentProxy();
                            sbProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                            sbProxy.AgentAuthHeaderValue.agentGuid = domainGuid;
                            sbProxy.AgentAuthHeaderValue.coupon = remoteSB.identOut;
                            sbProxy.Url = remoteSB.webServiceUrl;
                            sbProxy.Register(Utilities.MakeGuid(), info);
                        }
                    }
                }

                /*Update Lab Client grants*/

                //Get qualifier for labclient
                int lcQualifierID = AuthorizationAPI.GetQualifierID(theClient.clientID, Qualifier.labClientQualifierTypeID);
                //Get all "uselabclient" grants for this labclient
                int[] lcGrantIDs = wrapper.FindGrantsWrapper(-1, Function.useLabClientFunctionType, lcQualifierID);
                Grant[] lcGrants = wrapper.GetGrantsWrapper(lcGrantIDs);
                //Get list of agents that can use labclient
                ArrayList lcAgents = new ArrayList();
                foreach (Grant g in lcGrants)
                    lcAgents.Add(g.agentID);

                //if that agent doesn't already have the uselabclient permission
                if (!lcAgents.Contains(groupID))
                    //add lab client grant
                    wrapper.AddGrantWrapper(groupID, Function.useLabClientFunctionType, lcQualifierID);



                /*Update Lab Servers grant*/

                //Update grants for each of the associated lab servers
                foreach (int lsID in theClient.labServerIDs)
                {
                    //Get qualifier for labserver
                    int lsQualifierID = AuthorizationAPI.GetQualifierID(lsID, Qualifier.labServerQualifierTypeID);
                    //Get all "uselabserver" grants for this labserver
                    int[] lsGrantIDs = wrapper.FindGrantsWrapper(-1, Function.useLabServerFunctionType, lsQualifierID);
                    Grant[] lsGrants = wrapper.GetGrantsWrapper(lsGrantIDs);
                    //Get list of agents that can use labserver
                    ArrayList lsAgents = new ArrayList();
                    foreach (Grant g in lsGrants)
                        lsAgents.Add(g.agentID);

                    //int groupID = Convert.ToInt32(ddlUserGroup.SelectedValue);

                    //if that agent doesn't already have the uselabclient permission
                    if (!lsAgents.Contains(groupID))
                        //add lab server grant
                        wrapper.AddGrantWrapper(groupID, Function.useLabServerFunctionType, lsQualifierID);
                }

                //Refresh the repeater
                RefreshAdminUserGroupsRepeater();
                LoadListBoxes();

                lblResponse.Visible = true;
                lblResponse.Text = Utilities.FormatConfirmationMessage("Lab client (& corresponding lab servers) successfully updated. ");
            }
            catch (Exception ex)
            {
                lblResponse.Visible = true;
                lblResponse.Text = Utilities.FormatErrorMessage("Cannot update Lab Client. " + ex.GetBaseException());
            }
        }

} // public class manageLabGroups

} // namespace iLabs.ServiceBroker.admin
