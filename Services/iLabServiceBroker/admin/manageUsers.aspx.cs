/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: manageUsers.aspx.cs,v 1.2 2006/08/11 14:26:21 pbailey Exp $
 */
using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web .Security ;
using System.Text.RegularExpressions;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;
using iLabs.UtilLib;


namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for manageUser.
	/// </summary>
	public partial class manageUser : System.Web.UI.Page
	{
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");
			
			btnSaveChanges.CssClass="button";
			btnSaveChanges.Enabled=true;
			
			btnRemove.Attributes.Add("onclick", "javascript:if(confirm('This will remove the user from the groups and subgroups it belongs to. Are you sure you want to remove this user?')== false) return false;");

			if (!Session["GroupName"].ToString().Equals(ServiceBroker.Administration.Group.SUPERUSER))
			{
				txtPassword.Enabled = false;
				txtConfirmPassword.Enabled = false;
				txtPassword.BackColor = Color.Silver;
				txtConfirmPassword.BackColor = Color.Silver;
			}

			if(!IsPostBack)
			{
				/* Check the Web.Config for the Affilitation setting. 
				 * Affiliation can either be set up as a drop down list or a text box.
				 * This is specified in the Web.Config file
				 */
				if(ConfigurationSettings.AppSettings["useAffiliationDDL"].Equals("true"))
				{
					String afList = ConfigurationSettings.AppSettings["affiliationOptions"];
					char [] delimiter = {','};
					String [] options =afList.Split(delimiter,100);
					for(int i =0;i< options.Length;i++)
					{
						ddlAffiliation.Items.Add(options[i]);
					}
					if(options.Length > 0)
					{
						ddlAffiliation.Items[0].Selected = false;
					}
				}
				else
				{
					// Setup default affiliation
				}

				BuildUserListBox();
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

		/* 
		 * Builds the Select Users List box. 
		 * By default, the box gets filled with all the users in the database
		 */
		private void BuildUserListBox()
		{
			lbxSelectUser.Items .Clear ();
			
			try
			{
				int[] userIDs = wrapper.ListUserIDsWrapper ();
				User[] users = wrapper.GetUsersWrapper(userIDs);
				foreach (User user in users) 
				{
					ListItem userItem = new ListItem();
					userItem.Text = user.lastName+", "+user.firstName+" - "+user.userName;
					userItem.Value = user.userID.ToString();
					lbxSelectUser.Items .Add(userItem);
				}
			}
			catch(Exception ex)
			{
				string msg = "Exception: Cannot list userNames. "+ex.Message+". "+ex.GetBaseException();
				lblResponse.Text = Utilities.FormatErrorMessage(msg);
				lblResponse.Visible = true;
			}
		}

		/* 
		 * Builds the Select Users List using a specified array of users. 
		 * This is used to return the results of a search
		 */
		private void BuildUserListBox(User[] users)
		{
			lbxSelectUser.Items .Clear ();
			
			foreach (User user in users) 
			{
				ListItem userItem = new ListItem();
				userItem.Text = user.lastName+", "+user.firstName+" - "+user.userName;
				userItem.Value = user.userID.ToString();
				lbxSelectUser.Items .Add(userItem);
			}
		}

		/* 
		 * Builds the Select Users List using a specified list of users. 
		 * This is used to return the results of a search
		 */
		private void BuildUserListBox(ArrayList users)
		{
			lbxSelectUser.Items .Clear ();
			
			foreach (User user in users) 
			{
				ListItem userItem = new ListItem();
				userItem.Text = user.lastName+", "+user.firstName+" - "+user.userName;
				userItem.Value = user.userID.ToString();
				lbxSelectUser.Items .Add(userItem);
			}
		}

		private void ResetState()
		{
			if(ConfigurationSettings.AppSettings["useAffiliationDDL"].Equals("true"))
			{
				ddlAffiliation.ClearSelection ();
			}
			else
			{
				txtAffiliation.Text = "";
			}
			txtEmail.Text = "";
			txtLastName.Text = "";
			txtFirstName.Text = "";
			txtUsername.Text = "";
			txtUsername.Enabled=true;
			lblGroups.Text="";
			lblRequestGroups.Text="";
			cbxLockAccount.Checked = false;
			btnSaveChanges.CssClass="button";
			btnSaveChanges.Enabled = true;

		}

		private void DisplayUserInfo(User user)
		{
			ResetState();

			txtUsername.Text = user.userName;
			txtUsername.Enabled = false;
			txtFirstName.Text = user.firstName;
			txtLastName.Text = user.lastName;
			txtEmail.Text = user.email;

			/* Note if you change your drop down list options after launching the SB, 
					 * make sure your old affiliation options exist, or change them in the database.
					 * For e.g. if you change MIT Student to Student, make sure that the affiliation of 
					 * all the users is changed from "MIT Student" to "Student in the database.
					 * Otherwise the next step will throw an exception.*/
			if(ConfigurationSettings.AppSettings["useAffiliationDDL"].Equals("true"))
			{
				ddlAffiliation.Items .FindByText (user.affiliation).Selected = true;
			}
			else
			{
				txtAffiliation.Text = user.affiliation;
			}

			cbxLockAccount.Checked = user.lockAccount ;

			try
			{
				//Get explicit groups the user belongs to
				ArrayList nonRequestGroups = new ArrayList();
				ArrayList requestGroups = new ArrayList();
				int[] gpIDs = wrapper.ListGroupsForAgentWrapper (user.userID );
				ServiceBroker.Administration.Group[] gps=wrapper.GetGroupsWrapper(gpIDs);
				foreach(ServiceBroker.Administration.Group g in gps)
				{
					if (g.groupName.EndsWith("request"))
						requestGroups.Add(g);
					else 
						if(!g.groupName.Equals("NewUserGroup"))
						nonRequestGroups.Add(g);	
				}

				string groupsDisplay = "'"+user.userName + "'"+" is a member of the following groups:" + "<p>";
				if ((nonRequestGroups!=null)&& (nonRequestGroups.Count>0))
				{
					foreach (ServiceBroker.Administration.Group g in nonRequestGroups)
					{
						groupsDisplay += " &nbsp;&nbsp;-&nbsp;&nbsp;"+ g.groupName+ "<br>";
					}
				}
				else
				{
					groupsDisplay += " &nbsp;&nbsp;-&nbsp;&nbsp;no group <br>";
				}
					
				lblGroups.Text = groupsDisplay;

				string requestGroupsDisplay = "'"+user.userName + "'"+" has requested membership in the following groups:" + "<p>";
				if ((requestGroups!=null)&& (requestGroups.Count>0))
				{
					foreach (Administration.Group g in requestGroups)
					{
						int origGroupID = AdministrativeAPI.GetAssociatedGroupID(g.groupID);
						string origGroupName = AdministrativeAPI.GetGroups(new int[] {origGroupID})[0].groupName;
						requestGroupsDisplay += " &nbsp;&nbsp;-&nbsp;&nbsp;"+ origGroupName+ "<br>";
					}
				}
				else
				{
					requestGroupsDisplay += " &nbsp;&nbsp;-&nbsp;&nbsp;no group <br>";
				}
				lblRequestGroups.Text=requestGroupsDisplay;

				if (!Session["GroupName"].ToString().Equals(ServiceBroker.Administration.Group.SUPERUSER))
				{
					btnSaveChanges.Enabled=false;
					btnSaveChanges.CssClass="buttongray";
				}
			}

			catch (Exception ex)
			{
				string msg = "Exception: Trouble accessing page. "+ex.Message+". "+ex.GetBaseException();
				lblResponse.Text = Utilities.FormatErrorMessage(msg);
				lblResponse.Visible = true;
			}
		}

		private bool WildCardMatch(string searchString, string compareString )
		{
			Regex regex = new Regex(searchString,RegexOptions.IgnoreCase);

			return regex.IsMatch(compareString);
		}
	
		protected void lbxSelectUser_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lbxSelectUser.SelectedIndex < 0)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("You must select a user!");
				lblResponse.Visible = true;
			}
			else
			{
				try
				{
					User[] user = wrapper.GetUsersWrapper (new int[] {Int32.Parse(lbxSelectUser.SelectedValue)});
					DisplayUserInfo(user[0]);
				}
				catch(Exception ex)
				{
					string msg = "Exception: Cannot retrieve user's information. " +ex.Message+". "+ex.GetBaseException();
					lblResponse.Text = Utilities.FormatErrorMessage (msg);
					lblResponse.Visible = true;
				}
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			//only superusers can view this page
			if (!Session["GroupName"].ToString().Equals(ServiceBroker.Administration.Group.SUPERUSER))
				btnSaveChanges.Enabled=false;
				btnSaveChanges.CssClass="buttongray";

			// If an option is not selected in the "Search by" drop down list
			if(ddlSearchBy.SelectedIndex<1)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Select a search criterion.");
				lblResponse.Visible = true; 
			}
			else
			{
				//if blank entry
				if(txtSearchBy.Text == "")
				{
					lblResponse.Text = Utilities.FormatErrorMessage("Enter the text you want to search for.");
					lblResponse.Visible = true; 
				}
				else
				{
					ArrayList foundUsers = new ArrayList();
					string option = ddlSearchBy.SelectedItem.Text;
						
					int[] userIDs = wrapper.ListUserIDsWrapper ();
					User[] users = wrapper.GetUsersWrapper(userIDs);

					switch (option)
					{
						case "Username":
						{
							foreach(User u in users)
							{
								if (WildCardMatch(txtSearchBy.Text,u.userName))
									//if (! foundUsers.Contains(u))
									foundUsers.Add(u);
							}
						}
							break;
						case "First Name":
						{
							foreach(User u in users)
							{
								if (WildCardMatch(txtSearchBy.Text,u.firstName))
									foundUsers.Add(u);
							}
						}
							break;
						case "Last Name":
						{
							foreach(User u in users)
							{
								if (WildCardMatch(txtSearchBy.Text,u.lastName))
									foundUsers.Add(u);
							}
						}
							break;
						case "Group":
						{
							//Get a list of the groups from the database
							int[] groupIDs = wrapper.ListGroupIDsWrapper ();
							ServiceBroker.Administration.Group[] groups = wrapper.GetGroupsWrapper(groupIDs);

							//Find the relevant groups using the wild card search
							ArrayList foundGroups = new ArrayList();
							foreach(ServiceBroker.Administration.Group g in groups)
							{
								if (WildCardMatch(txtSearchBy.Text,g.groupName))
									foundGroups.Add(g);
							}
							
							//if the group exists in the database
							ArrayList foundUserIDs = new ArrayList();
							if (foundGroups.Count>0)
							{
								foreach (ServiceBroker.Administration.Group foundg in foundGroups)
								{
									//Get the list of users in the group
									int[] userIDsForGroup=wrapper.ListUserIDsInGroupRecursivelyWrapper(foundg.groupID);

									//Put this in the foundUserID ArrayList
									foreach (int userID in userIDsForGroup)
										if (! foundUserIDs.Contains(userID))
										foundUserIDs.Add(userID);
								}
							
								//if the group contains users
								if (foundUserIDs.Count > 0)
								{
									User[] userArray=wrapper.GetUsersWrapper(Utilities.ArrayListToIntArray(foundUserIDs));
									BuildUserListBox(userArray);
								}
								else //no users exist in the group
								{
									string msg = "No users exist in group "+txtSearchBy.Text+ "." ;
									lblResponse.Text = Utilities.FormatErrorMessage(msg);
									lblResponse.Visible=true;
								}
							}
							else //groupID < 0, group doesn't exist
							{
								string msg = "The group "+txtSearchBy.Text+" does not exist.";
								lblResponse.Text = Utilities.FormatErrorMessage(msg);
								lblResponse.Visible = true; 
							}
						}
							break;
					} //end switch

					if (option.CompareTo("Group")!=0)
					{
						if (foundUsers.Count>0)
						{
							// if only one record found
							if (foundUsers.Count==1)
							{
								/* Need to rebuild the listbox, incase of multiple searches.
								 * The results of a search are displayed in the list box 
								 * & hence if one needs to do a 2nd search, the list has to be rebuilt.
								 */
								BuildUserListBox();
								User foundUser = (User) foundUsers[0];
								lbxSelectUser.Items.FindByValue(foundUser.userID.ToString()).Selected = true;
								DisplayUserInfo(foundUser);	
							}
							else
							{
								BuildUserListBox(foundUsers);
							}
						}
						else // no users found
						{
							string msg = "The user "+txtSearchBy.Text+" does not exist.";
							lblResponse.Text = Utilities.FormatErrorMessage(msg);
							lblResponse.Visible = true; 
						}
					}// end if option !group
				}
			}
		}
	
		protected void btnSaveChanges_Click(object sender, System.EventArgs e)
		{
			//Error checking for empty fields
			if(txtUsername.Text.CompareTo("")==0 )
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Enter a Username.");
				lblResponse.Visible=true;
				return;
			}

			if(txtFirstName.Text.CompareTo("")==0 )
			{
				lblResponse.Text = Utilities.FormatErrorMessage("You must enter the user's first name.");
				lblResponse.Visible=true;
				return;
			}

			if(txtLastName.Text.CompareTo("")==0 )
			{
				lblResponse.Text = Utilities.FormatErrorMessage("You must enter the user's last name.");
				lblResponse.Visible=true;
				return;
			}

			if(txtEmail.Text.CompareTo("")==0 )
			{
				lblResponse.Text = Utilities.FormatErrorMessage("You must enter the user's email.");
				lblResponse.Visible=true;
				return;
			}

			String strAffiliation = "";
			if(ConfigurationSettings.AppSettings["useAffiliationDDL"].Equals("true"))
			{
				if (ddlAffiliation.SelectedIndex > 0)
				{
					strAffiliation = ddlAffiliation.SelectedItem.Text;
				}
			}
			else
			{
				strAffiliation = txtAffiliation.Text;
			}
			
			if ((strAffiliation==null)||(strAffiliation.CompareTo("")==0))
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Please select an affiliation group.");
				lblResponse.Visible=true;
				return;
			}
		
			//If all the error checks are cleared
			
				//if adding a new user
			if(txtUsername.Enabled)
			{
				try
				{
					// the database will also throw an exception if the agentName exists
					// since username must be unique across both users and groups.
					// this is just another check to throw a meaningful exception
					if (wrapper.GetUserIDWrapper(txtUsername.Text)>0) // then the user already exists in database
					{
						string msg = "The username '"+txtUsername.Text+"' already exists. Choose another username.";
						lblResponse.Text = Utilities.FormatErrorMessage(msg);
						lblResponse.Visible=true;
						return;
					}
					else
					{
						if (Session["GroupName"].Equals(ServiceBroker.Administration.Group.SUPERUSER))
						{
							//Password checks
							if(txtPassword.Text.CompareTo("")==0 )
							{
								lblResponse.Text = Utilities.FormatErrorMessage("You must enter a password.");
								lblResponse.Visible=true;
								return;
							}
			
							if(txtConfirmPassword.Text.CompareTo("")==0 )
							{
								lblResponse.Text = Utilities.FormatErrorMessage("Retype the password in the 'Confirm Password' field.");
								lblResponse.Visible=true;
								return;
							}

							if(txtPassword.Text != txtConfirmPassword.Text )
							{
								lblResponse.Text = Utilities.FormatErrorMessage("Password fields do not match. Retype password.");
								lblResponse.Visible=true;
								return;
							}
						}

						//Add User
						int userID= wrapper.AddUserWrapper(txtUsername.Text, txtUsername.Text, AuthenticationType.NativeAuthentication,
							txtFirstName.Text, txtLastName.Text, txtEmail.Text,strAffiliation, "No reason specified - User added through Administrative interface",
							"", wrapper.GetGroupIDWrapper(ServiceBroker.Administration.Group.NEWUSERGROUP), cbxLockAccount.Checked);

						//Set Password - Can only change password if you're superuser
						if (Session["GroupName"].Equals(ServiceBroker.Administration.Group.SUPERUSER))
						{
							wrapper.SetNativePasswordWrapper (wrapper.GetUserIDWrapper(txtUsername.Text) , txtPassword.Text );
						}

						string msg = "The record for "+txtUsername.Text + " has been created successfully.";
						lblResponse.Text= Utilities.FormatConfirmationMessage(msg);
						lblResponse.Visible=true;

						txtUsername.Enabled=false;
						BuildUserListBox();
						//select the recently added user in the list box
						lbxSelectUser.Items.FindByValue(userID.ToString());
					}
				}
				catch (Exception ex)
				{
					string msg = "Exception: Cannot add '"+txtUsername.Text +"'. "+ex.Message +". "+ex.GetBaseException()+".";
					lblResponse.Text= Utilities.FormatErrorMessage(msg);
					lblResponse.Visible=true;
				}
			}
			else // if updating an old user
			{
				try
				{
					//Update user information
					wrapper.ModifyUserWrapper (wrapper.GetUserIDWrapper(txtUsername.Text) ,txtUsername.Text , txtUsername.Text , AuthenticationType.NativeAuthentication, txtFirstName.Text ,txtLastName.Text , txtEmail.Text , strAffiliation, "","", cbxLockAccount.Checked );

					//Update password information only if the old password has not been changed
					if(txtPassword.Text.CompareTo("")!=0 )
					{
						if(txtConfirmPassword.Text.CompareTo("")==0 )
						{
							lblResponse.Text = Utilities.FormatErrorMessage("Retype the password in the 'Confirm Password' field.");
							lblResponse.Visible=true;
							return;
						}

						if(txtPassword.Text != txtConfirmPassword.Text )
						{
							lblResponse.Text = Utilities.FormatErrorMessage("Password fields do not match. Retype password.");
							lblResponse.Visible=true;
							return;
						}

						//Update password
						wrapper.SetNativePasswordWrapper (wrapper.GetUserIDWrapper(txtUsername.Text) , txtPassword.Text );
					}
									
					string msg = "The record for '"+txtUsername.Text + "' has been updated.";
					lblResponse.Text= Utilities.FormatConfirmationMessage(msg);
					lblResponse.Visible=true;
				}
				catch(Exception ex)
				{
					string msg = "Exception: Cannot update '"+txtUsername.Text +"'. "+ex.Message +". "+ex.GetBaseException()+".";
					lblResponse.Text= Utilities.FormatErrorMessage(msg);
					lblResponse.Visible=true;
				}
			}
		}

		protected void btnRemove_Click(object sender, System.EventArgs e)
		{
			if (lbxSelectUser.SelectedIndex<0)
			{
				lblResponse.Text = Utilities.FormatErrorMessage("Select a user to be deleted.");
				lblResponse.Visible=true;
				return;
			}
			try
			{
				if(wrapper.RemoveUsersWrapper (new int[] {wrapper.GetUserIDWrapper(txtUsername.Text) }).Length > 0)
				{
					string msg = "'" + txtUsername.Text + "' was not deleted.";
					lblResponse.Text = Utilities.FormatErrorMessage(msg);
					lblResponse.Visible=true;
				}
				else
				{
					string msg = "'"+txtUsername.Text + "' has been deleted.";
					lblResponse.Text = Utilities.FormatConfirmationMessage(msg);
					lblResponse.Visible=true;
				}
			}
			catch (Exception ex)
			{
				string msg = "Exception: Cannot delete '" + txtUsername.Text + "'. "+ex.Message+". "+ex.GetBaseException();
				lblResponse.Text = Utilities.FormatErrorMessage(msg);
				lblResponse.Visible=true;
			}
			finally
			{
				ResetState();
				txtSearchBy.Text = "";
				BuildUserListBox();
			}
		}

		protected void btnNew_Click(object sender, System.EventArgs e)
		{
			ResetState();
			txtSearchBy.Text = "";
			BuildUserListBox();
		}
	}
	
}


	

