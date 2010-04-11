/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: requestGroup.aspx.cs,v 1.2 2006/08/11 14:26:15 pbailey Exp $
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Web.Mail;
using System.Configuration;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Summary description for requestgroup.
	/// </summary>
	public partial class requestGroup : System.Web.UI.Page
	{
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
		ArrayList nonRequestGroups = new ArrayList();
		ArrayList requestGroups = new ArrayList();
		ArrayList canRequestGroupIDs = new ArrayList();
		Group[] canRequestGroups;

		int userID;
		int[] userGroupIDs;
		int[] groupIDs;

		string supportMailAddress = ConfigurationSettings.AppSettings["supportMailAddress"];
		bool adminRequestGroup;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			userID = Convert.ToInt32(Session["UserID"]);

			if(ConfigurationSettings.AppSettings["adminRequestGroup"].Trim().ToLower() == "true")
				adminRequestGroup = true;
			else
				adminRequestGroup = false;
			
			// Reset error message
			lblResponse.Visible = false;

			// Initialize ArrayLists requestGroups, nonRequestGroups
			LoadGroupArrays();

			if(!IsPostBack)
			{

				// List groups user belongs to, and groups user wants to join, in the blue box on the right of the page.
				LoadBlueBox();

				// Load the repeater containing checkboxes next to groups the user may request membership in
				LoadRepeater();
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
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
		
		/// <summary>
		/// Initialize ArrayLists requestGroups, nonRequestGroups
		/// </summary>
		private void LoadGroupArrays()
		{

			// Gets the list of all groupIDs
			groupIDs = wrapper.ListGroupIDsWrapper();
			
			// Gets a list of all the groups a user belongs to
			userGroupIDs = wrapper.ListGroupsForAgentWrapper(userID);
			ArrayList userGroupList = new ArrayList(userGroupIDs);
			
			// Clear the list of groups that a user does not yet belong to
			canRequestGroupIDs.Clear();

			// Each group has a twin group that ends with the suffix "request".
			// This group is used to store users who have requested membership in a group,
			// pending administrator approval.
			// This is an ArrayList of those groups
			requestGroups.Clear();
			
			// This is an ArrayList of regualar groups, i.e. those that do not end with "request"
			nonRequestGroups.Clear();

			//since we already have the groups a user has access
			// if we use wrapper here, it will deny authentication
			Group[] gps = AdministrativeAPI.GetGroups(groupIDs);

			foreach(Group g in gps)
			{
				// If the user belongs to the group
				if (userGroupList.Contains(g.groupID))
				{
					if(g.groupType.CompareTo(GroupType.REQUEST)==0)
						requestGroups.Add(g);
					else
						if ((g.groupName.ToUpper()).CompareTo("ROOT")!=0)
						nonRequestGroups.Add(g);
				}
				else
				{
					// If user doesn't belong to group & if it is a request group
					// add to list of groups that they can request
					if(g.groupType.CompareTo(GroupType.REQUEST)==0)
					{
                        int origGroupID = AdministrativeAPI.GetAssociatedGroupID(g.groupID);
						//string origGroupName= AdministrativeAPI.GetGroups(new int[] {origGroupID})[0].groupName;
							
						if (!userGroupList.Contains(origGroupID))
						{	
							// Add the "request" group to the list of groups a user can be added to.
							// An Administrator will have to review the request and move the user
							// to the "real" group.
							// This is a setting that can be changed in web.config.
							if(adminRequestGroup)
							{
								canRequestGroupIDs.Add(g.groupID);
							}
							// Add the "real" group to the list of groups a user can be added to.
							else
							{
								canRequestGroupIDs.Add(origGroupID);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// List groups user belongs to, and groups user wants to join, in the blue box on the right of the page.
		/// </summary>
		private void LoadBlueBox()
		{
			//List Groups that user belongs to in blue box
			if ((nonRequestGroups!=null)&& (nonRequestGroups.Count>0))
			{
				lblGroups.Text = "";
				for (int i=0;i<nonRequestGroups.Count;i++)
				{
					lblGroups.Text+= ((Group)nonRequestGroups[i]).groupName;
					if (i != nonRequestGroups.Count-1)
						lblGroups.Text +=", ";
				}
			}
			else
			{
				lblGroups.Text = "No group";
			}

			//List Groups that user has requested to in blue box
			if ((requestGroups!=null)&& (requestGroups.Count>0))
			{
				lblRequestGroups.Text = "";
				for (int i=0;i<requestGroups.Count;i++)
				{
                    int origGroupID = AdministrativeAPI.GetAssociatedGroupID(((Group)requestGroups[i]).groupID);
                    string origGroupName = AdministrativeAPI.GetGroups(new int[] { origGroupID })[0].groupName;
					lblRequestGroups.Text+= origGroupName;
					if (i != requestGroups.Count-1)
						lblRequestGroups.Text +=", ";
				}
			}
			else
			{
				lblRequestGroups.Text = "No group";
			}
		}

		/// <summary>
		/// Load the repAvailableGroups repeater from the canRequestGroups ArrayList
		/// </summary>
		private void LoadRepeater()
		{
			if ((canRequestGroupIDs==null ) ||(canRequestGroupIDs.Count ==0))
			{
				lblNoGroups.Text = "<p>You cannot request membership in any more groups. Contact " + supportMailAddress + " if you wish to be added to any other group.</p>";
				lblNoGroups.Visible = true;
				repAvailableGroups.Visible = false;
				btnRequestMembership.Visible = false;
			}
			else
			{
				try
				{
					// have to bypass wrapper class here				
                    canRequestGroups = AdministrativeAPI.GetGroups(Utilities.ArrayListToIntArray(canRequestGroupIDs));
					for(int i=0; i<canRequestGroups.Length; i++)
					{
                        int origGroupID = AdministrativeAPI.GetAssociatedGroupID(((Group)canRequestGroups[i]).groupID);
                        string origGroupName = AdministrativeAPI.GetGroups(new int[] { origGroupID })[0].groupName;
						canRequestGroups[i].groupName = origGroupName;
					}
					repAvailableGroups.DataSource = canRequestGroups;
					repAvailableGroups.DataBind();
				
					int repCount =1;
					// To list all the labs belonging to a group
					foreach (Group g in canRequestGroups)
					{
						int[] lcIDsList = AdministrativeUtilities.GetGroupLabClients(g.groupID);

						LabClient[] lcList = wrapper.GetLabClientsWrapper(lcIDsList);

						Label lblGroupLabs = new Label();
						lblGroupLabs.Visible=true;
						lblGroupLabs.Text="<p>Associated Labs</p><ul>";

						for(int i=0;i<lcList.Length;i++)
						{
							lblGroupLabs.Text += "<li><strong class=lab>"+
								lcList[i].clientName+"</strong> - "+
								lcList[i].clientShortDescription+ "</li>";
						}
						lblGroupLabs.Text +="</ul>";

						repAvailableGroups.Controls.AddAt(repCount, lblGroupLabs);
						
						repCount += 3;
					}
				}
				catch (AccessDeniedException adex)
				{
					lblResponse.Visible = true;
					lblResponse.Text = Utilities.FormatErrorMessage(adex.Message);
				}
			}
		}

		
		protected void btnRequestMembership_Click(object sender, System.EventArgs e)
		{
			//Load Array of Groups that can be requested. This is needed to obtain the groupID, which is not 
			// stored in the repeater.
			// canRequestGroupIDs is created in LoadGroupArrays()
			// have to bypass wrapper class here
            canRequestGroups = AdministrativeAPI.GetGroups(Utilities.ArrayListToIntArray(canRequestGroupIDs));
			
			bool atLeastOneGroupSelected = false;
			int groupID;

			for (int i=0; i<repAvailableGroups.Items.Count; i++)
			{
				if(((CheckBox)repAvailableGroups.Items[i].Controls[1]).Checked == true)
				{
					atLeastOneGroupSelected = true;
					try
					{
						groupID = canRequestGroups[i].groupID;
						// have to bypass wrapper class here
                        AdministrativeAPI.AddMemberToGroup(Convert.ToInt32(Session["UserID"]), groupID);
						LoadGroupArrays();
						LoadRepeater();
						LoadBlueBox();
					}
				catch (Exception ex)
					{
						lblResponse.Visible = true;
						lblResponse.Text = Utilities.FormatErrorMessage("Cannot add member to Group! " + ex.Message);
					}
				}
			}

			// If no groups were selected - throw an error
			if(!atLeastOneGroupSelected)
			{
				lblResponse.Visible = true;
				lblResponse.Text = Utilities.FormatErrorMessage("No groups selected!");
				LoadRepeater();
				return;
			}
			
		}
	}
}
