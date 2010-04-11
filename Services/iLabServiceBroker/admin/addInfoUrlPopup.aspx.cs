/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id: addInfoUrlPopup.aspx.cs,v 1.2 2006/10/23 21:13:42 pbailey Exp $ */

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
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;


namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for addInfoUrlPopup.
	/// </summary>
	public partial class addInfoUrlPopup : System.Web.UI.Page
	{
		//The error message div tab

		
		int labClientID;
		int[] labClientIDs;
		LabClient[] labClients;
		ClientInfo[] clientInfos;
		ArrayList clientInfosList = new ArrayList();
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

			labClientID = int.Parse(Request.Params["lc"]);
			labClientIDs = new int[1];
			labClientIDs[0] = labClientID;
			labClients = wrapper.GetLabClientsWrapper(labClientIDs);

			clientInfos = labClients[0].clientInfos;
			if (clientInfos.Length == 0)
			{
				clientInfos = new ClientInfo[1];
			}

			if(!IsPostBack)
			{
				RefreshClientInfoRepeater();
				ClearFormFields();
				LoadListBox();
			}
			
			// Current Lab Client name
			lblLabClient.Text = labClients[0].clientName;
			
			// Error Message
			divErrorMessage.Visible = false;

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
			this.ibtnMoveUp.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnMoveUp_Click);
			this.ibtnMoveDown.Click += new System.Web.UI.ImageClickEventHandler(this.ibtnMoveDown_Click);
			this.repClientInfo.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.repClientInfo_ItemDataBound);
			this.repClientInfo.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.repClientInfo_ItemCommand);

		}
		#endregion

		private void RefreshClientInfoRepeater()
		{
			
			// refresh the array of LabClient objects from the database.
			// This insures that any changed ClientInfo arrays (one per LabClient Object)
			// are retrieved.
			labClientIDs = new int[1];
			labClientIDs[0] = labClientID;
			labClients = wrapper.GetLabClientsWrapper(labClientIDs);

			LabClient lc = new LabClient();
			lc = labClients[0];

			clientInfosList.Clear();

			foreach(ClientInfo ci in lc.clientInfos)
			{
				if (!ci.infoURLName.ToUpper().Equals("DOCUMENTATION"))
					clientInfosList.Add(ci);	
			}

			repClientInfo.DataSource = clientInfosList;
			repClientInfo.DataBind();

			// update public clientInfos object
			clientInfos = lc.clientInfos;
			if (clientInfos.Length == 0)
			{
				clientInfos = new ClientInfo[1];
			}
		}

		/// <summary>
		/// ItemCommand event for the ClientInfo Repeater.
		/// Fires when the Edit or Remove buttons are clicked.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void repClientInfo_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			//int clientInfoID = ((ClientInfo)(e.Item.DataItem)).clientInfoID;
			int clientInfoID = Convert.ToInt32(e.CommandArgument);
			int index=0;
			for (index=0;index<=clientInfos.Length;index++)
				if ((clientInfos[index].clientInfoID==clientInfoID))
					break;
			if(e.CommandName == "Edit")
			{
				LoadFormFields(index);
			}
			else if(e.CommandName == "Remove")
			{
				RemoveClientInfo(index);
			}
		}

		/// <summary>
		/// Loads the fields in the Info Edit Box with information
		/// from the selected record, to prepare for editing.
		/// </summary>
		private void LoadFormFields(int index)
		{
			txtDesc.Text = clientInfos[index].description;
			txtUrl.Text = clientInfos[index].infoURL;
			txtInfoname.Text = clientInfos[index].infoURLName;
			hiddenClientInfoIndex.Value = index.ToString();
		}

		/// <summary>
		/// Clears the fields in the Info Edit Box on the right
		/// to prepare for the creation of a new record.
		/// </summary>
		private void ClearFormFields()
		{
			txtInfoname.Text = "";
			txtUrl.Text = "";
			txtDesc.Text = "";
			// keeps track of current clientInfos index. New record should
			// have and index that is one higher than the highest current index.
			// Array Length property is not zero-based, hence equality here.
			hiddenClientInfoIndex.Value = clientInfos.Length.ToString();
		}

		protected void btnNew_Click(object sender, System.EventArgs e)
		{
			ClearFormFields();
		}

		protected void btnSaveInfoChanges_Click(object sender, System.EventArgs e)
		{
			int index;
			index = int.Parse(hiddenClientInfoIndex.Value);
			
		// Set the correct element of the clientInfos array to the values from the text boxes
			if (index == clientInfos.Length)
			{
				// new addition
				// create a new ClientInfo object
				ClientInfo newClientInfo = new ClientInfo();
				newClientInfo.description = txtDesc.Text;
				newClientInfo.infoURL = txtUrl.Text;
				newClientInfo.infoURLName = txtInfoname.Text;

				ClientInfo[] enlargedClientInfos;
				enlargedClientInfos = new ClientInfo[index +1];
				for (int i=0; i<index; i++)
				{
					enlargedClientInfos[i] = clientInfos[i];
				}
				// Add the new ClientInfo object
				enlargedClientInfos[index] = newClientInfo;

				// Set the clientInfos object to the new expanded object
				clientInfos = enlargedClientInfos;
			}
			else
			{
				// modify existing clientInfo
				clientInfos[index].description = txtDesc.Text;
				clientInfos[index].infoURL = txtUrl.Text;
				clientInfos[index].infoURLName = txtInfoname.Text;
			}

			//Save the LabClient object with the updated clientInfos array
			UpdateLabClientInfo(clientInfos);
		
		}

		private void RemoveClientInfo(int index)
		{
			ClientInfo[] shrunkClientInfos;
			int offset;
			offset = 0;
			shrunkClientInfos = new ClientInfo[clientInfos.Length -1];
			for (int i=0; i < shrunkClientInfos.Length; i++)
			{
				if (i == index)
				{
					offset += 1;
				}
				shrunkClientInfos[i] = clientInfos[i + offset];
			}

			// Set the clientInfos object to the new smaller object
			clientInfos = shrunkClientInfos;

			//Save the LabClient object with the updated clientInfos array
			UpdateLabClientInfo(shrunkClientInfos);

		}

		/// <summary>
		/// Databound event for the repClientInfo Repeater.
		/// You have to initialize any javascript for the buttons in the 
		/// Databound event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void repClientInfo_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
			{
				Button remBtn = (Button) e.Item.FindControl("btnRemove");
				// "Are you sure" javascript for Remove button
				string script = "javascript:if(confirm('Are you sure you want to remove this Client Resource?')== false) return false;";
				remBtn.Attributes.Add("onClick",script);
				remBtn.CommandArgument = ((ClientInfo)e.Item.DataItem).clientInfoID.ToString();

				//Adding comman arguments to edit button
				Button editBtn = (Button) e.Item.FindControl("btnEdit");
				editBtn.CommandArgument = ((ClientInfo)e.Item.DataItem).clientInfoID.ToString();
			}

		}
		
		private void LoadListBox()
		{
			lbxChangeOrder.Items.Clear();
			for (int i=0; i< clientInfos.Length; i++)
			{
				if ((!(clientInfos[i].infoURLName==null))&&(!clientInfos[i].infoURLName.ToUpper().Equals("DOCUMENTATION")))
				lbxChangeOrder.Items.Add(new ListItem(clientInfos[i].infoURLName+" - "+clientInfos[i].description, i.ToString()));
			}
		}

		/// <summary>
		/// Moves an item in the ListBox up one position
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ibtnMoveUp_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			int i;
			if ( (i = lbxChangeOrder.SelectedIndex) >0 ) 
			{
				ListItem li1 = lbxChangeOrder.Items[i];
				ListItem li2 = lbxChangeOrder.Items[i-1];
				lbxChangeOrder.Items.Remove(li1);
				lbxChangeOrder.Items.Remove(li2);
				lbxChangeOrder.Items.Insert(i-1,li1);
				lbxChangeOrder.Items.Insert(i,li2);
			}
		}

		/// <summary>
		/// Moves an item in the ListBox down one position.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ibtnMoveDown_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			int i=lbxChangeOrder.SelectedIndex;
			if (i >= 0 && i < lbxChangeOrder.Items.Count-1) 
			{
				ListItem li1=lbxChangeOrder.Items[i];
				ListItem li2=lbxChangeOrder.Items[i+1];
				lbxChangeOrder.Items.Remove(li1);
				lbxChangeOrder.Items.Remove(li2);
				lbxChangeOrder.Items.Insert(i,li1);
				lbxChangeOrder.Items.Insert(i,li2);
			}
		}

		/// <summary>
		/// Saves the LabClient with the ClienInfo array rewritten into a new order.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSaveOrderChanges_Click(object sender, System.EventArgs e)
		{
			ClientInfo[] newClientInfos = new ClientInfo[clientInfos.Length];
			for (int i=0; i < newClientInfos.Length; i++)
			{
				if (i<lbxChangeOrder.Items.Count)
				//Take the index out of the value in the list box, where it has been preserved.
				newClientInfos[i] = clientInfos[int.Parse(lbxChangeOrder.Items[i].Value)];
				else
					newClientInfos[i]=clientInfos[i];
			}

			//Save the LabClient object with the updated clientInfos array
			UpdateLabClientInfo(newClientInfos);		
		}

		/// <summary>
		/// This is the main database update routine.
		/// In order to update the ClientInfo structure in the Lab Client, it is necessary to 
		/// update the entire lab client.
		/// </summary>
		/// <param name="clientInfos"></param>
		private void UpdateLabClientInfo(ClientInfo[] clientInfos)
		{
			try
			{
                wrapper.ModifyLabClientWrapper(labClientID, labClients[0].clientName, labClients[0].version, 
                    labClients[0].clientShortDescription, labClients[0].clientLongDescription, labClients[0].notes, 
                    labClients[0].loaderScript, labClients[0].clientType, labClients[0].labServerIDs,
                    labClients[0].contactEmail, labClients[0].contactFirstName, labClients[0].contactLastName,
                    labClients[0].needsScheduling, labClients[0].needsESS, labClients[0].IsReentrant, clientInfos);
				// Create the javascript which will cause a page refresh event to fire on the popup's parent page
				string jScript;
				jScript = "<script language=javascript> window.opener.Form1.hiddenPopupOnSave.value='1';";
				// jScript += "window.close();";
				jScript += "</script>";
				Page.RegisterClientScriptBlock("postbackScript", jScript);
			}
			catch (Exception ex)
			{
				divErrorMessage.Visible = true;
				lblResponse.Visible = true;
				lblResponse.Text = "Cannot update Lab Client. " + ex.Message;
			}

			RefreshClientInfoRepeater();
			LoadListBox();
		}

	}
}
