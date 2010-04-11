/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id: myClientList.aspx.cs,v 1.2 2007/12/26 05:27:26 pbailey Exp $ */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;

namespace iLabs.ServiceBroker.iLabSB
{
	/// <summary>
	/// Summary description for myClientList.
	/// </summary>
	public partial class myClientList : System.Web.UI.Page
	{
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
		protected LabClient[] lcList = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(! IsPostBack)
			{
				if(Session["GroupName"] != null)
				{
					string groupName = Session["GroupName"].ToString();
					lblGroupNameTitle.Text = groupName;
					lblGroupNameSystemMessage.Text = groupName;
					lblGroupNameLabList.Text = groupName;
				}

			}
			// This doesn't work - is it possible to stick an int array in the session?
			//int[] lcIDList = (int[])(Session["LabClientList"]);

			//Temporarily getting the list again from using the Utilities class
			int[] lcIDList = AdministrativeUtilities.GetGroupLabClients (Convert.ToInt32(Session["GroupID"]));
			lcList = wrapper.GetLabClientsWrapper(lcIDList);
				
			repLabs.DataSource = lcList;
			repLabs.DataBind();

            List<SystemMessage> messagesList = new List<SystemMessage>();
            SystemMessage[] groupMessages = null;
            
            groupMessages = wrapper.GetSystemMessagesWrapper(SystemMessage.GROUP, Convert.ToInt32(Session["GroupID"]), 0, 0);
            if (groupMessages != null)
                messagesList.AddRange(groupMessages);

            if (messagesList != null && messagesList.Count > 0)
            {
                messagesList.Sort(SystemMessage.CompareDateDesc);
                //messagesList.Reverse();
                repSystemMessage.DataSource = messagesList;
                repSystemMessage.DataBind();
            }

            else
            {
                lblGroupNameSystemMessage.Text += "<p>No Messages at this time</p>";
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
			this.repLabs.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.repLabs_ItemCommand);

		}
		#endregion

		private void repLabs_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (Session["UserID"] ==null)
				Response.Redirect("login.aspx");
			else
			{
				if(e.CommandName=="SetLabClient")
				{
					// get the labClientID from the lcList.
					// The indexer of the List will match the index of the repeater
					// since the repeater was loaded from the List.
					int clientID = ((LabClient)lcList[e.Item.ItemIndex]).clientID;
				
					// Set the LabClient session value and redirect
					Session["ClientID"] = clientID;
                    AdministrativeAPI.SetSessionClient(Convert.ToInt64(Session["SessionID"]),clientID);
					Response.Redirect("myClient.aspx");
				}
			}
		}

		public void navLogout_Click(object sender, System.EventArgs e)
		{
			AdministrativeAPI.SaveUserSessionEndTime (Convert.ToInt64 (Session["SessionID"]));
			Session.RemoveAll();
			FormsAuthentication.SignOut ();
			Response.Redirect ("login.aspx");
		}
	}
}
