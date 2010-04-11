/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: manageLabClients.aspx.cs,v 1.41 2008/03/11 19:27:04 pbailey Exp $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.LSS;
using iLabs.Proxies.USS;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.iLabSB;
using iLabs.ServiceBroker.Mapping;
using iLabs.Ticketing;

using iLabs.UtilLib;
//using iLabs.Services;

namespace iLabs.ServiceBroker.admin
{
	/// <summary>
	/// Summary description for manageLabClients.
	/// </summary>
	public partial class manageLabClients : System.Web.UI.Page
    {
		AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

		int labClientID;
		int[] labClientIDs;
		LabClient[] labClients;

		int[] labServerIDs;
		ProcessAgentInfo[] labServers;

        protected BrokerDB ticketing;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            ticketing = new BrokerDB();
			if (Session["UserID"]==null)
				Response.Redirect("../login.aspx");

			//only superusers can view this page
			if (!Session["GroupName"].ToString().Equals(Group.SUPERUSER))
				Response.Redirect("../home.aspx");

			labClientIDs = wrapper.ListLabClientIDsWrapper();
			labClients = wrapper.GetLabClientsWrapper(labClientIDs);

			// Set the popup buttons' "CausesValidation" property.
			// For any button that is related to a popup, you have to set the CausesValidation
			// propery to false, otherwise
			// any RequiredFieldValidator control will cause the onclick event to be
			// comandeered by a routine in ASP.NET's WebValidationUI.js file.
			// The result would be that the custom onclick for the popup would not fire.

			btnEditList.CausesValidation = false;
			btnAddEditResources.CausesValidation = false;
			btnRemove.CausesValidation = false;

			// This button enables the popup to fire an event on the caller when the Save button is hit.
			btnRefresh.CausesValidation = false;

			// "Are you sure" javascript for Remove button
			btnRemove.Attributes.Add("onclick", "javascript:if(confirm('Are you sure you want to remove this Lab Client?')== false) return false;");
			
			// This is a hidden input tag. The associatedLabServers popup changes its value using a window.opener call in javascript,
			// then the GetPostBackEventReference fires the event associated with the btnRefresh button.
			// The result is that the LabServer repeater (repLabServers) is refreshed when the Save button is clicked
			// on the popup.
			hiddenPopupOnSave.Attributes.Add("onpropertychange", Page.GetPostBackEventReference(btnRefresh));

			if(!Page.IsPostBack )
			{
                Session.Remove("ClientUssMappingID");
                Session.Remove("ClientEssMappingID");
	
				// Load Lab Client dropdown
                ddlLabClient.Items.Add(new ListItem(" --- select Lab Client --- ", "0"));
				
				foreach (LabClient lc in labClients)
				{
                    
					ddlLabClient.Items.Add(new ListItem(lc.clientName,lc.clientID.ToString()));
			
				}
                //Put in availabe USS
                ListItem liHeaderUss = new ListItem("---Select User Side Scheduling Server---", "0");
                ddlAssociatedUSS.Items.Add(liHeaderUss);
                IntTag[] usses = ticketing.GetProcessAgentTagsByType(ProcessAgentType.SCHEDULING_SERVER,ProcessAgentDB.ServiceGuid);
                foreach (IntTag uss in usses)
                {
                    ListItem li = new ListItem(uss.tag, uss.id.ToString());
                    ddlAssociatedUSS.Items.Add(li);
                }
                //Put in availabe ESS
                ListItem liHeaderEss = new ListItem("---Select Experiment Storage Server---", "0");
                ddlAssociatedESS.Items.Add(liHeaderEss);
                IntTag[] esses = ticketing.GetProcessAgentTagsByType(ProcessAgentType.EXPERIMENT_STORAGE_SERVER, ProcessAgentDB.ServiceGuid);
                foreach (IntTag ess in esses)
                {
                    ListItem li = new ListItem(ess.tag, ess.id.ToString());
                    ddlAssociatedESS.Items.Add(li);
                }

                //Put in client types
                string[] clientTypes = InternalAdminDB.SelectLabClientTypes();

                foreach (string cType in clientTypes)
                {
                    ListItem li = new ListItem(cType);
                    ddlClientTypes.Items.Add(li);
                }

				// Disable the "Edit Lab Servers" button at first
				btnEditList.Visible = false;

                ddlAssociatedESS.Enabled = false;
                ddlAssociatedUSS.Enabled = false;

                btnRegisterUSS.Visible = false;
                btnRegisterESS.Visible = false;
                btnDissociateUSS.Visible = false;
                btnDissociateESS.Visible = false;

                // Reset error/confirmation message
                lblResponse.Text = "";
                lblResponse.Visible = false;

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
		/// Clears the Lab Client dropdown and reloads it from the array of LabClient objects
		/// </summary>
		private void InitializeDropDown()
		{
			labClientIDs = wrapper.ListLabClientIDsWrapper();
			labClients = wrapper.GetLabClientsWrapper(labClientIDs);
			
			ddlLabClient.Items.Clear();

			ddlLabClient.Items.Add(new ListItem(" --- select Lab Client --- ","0"));
			
			foreach (LabClient lc in labClients)
			{
				ddlLabClient.Items.Add(new ListItem(lc.clientName,lc.clientID.ToString()));
			
			}
		}

		/// <summary>
		/// This method clears the form fields.
		/// </summary>
		private void ClearFormFields()
		{
			txtLabClientName.Text = "";
			txtVersion.Text = "";
			txtShortDesc.Text = "";
			txtLongDesc.Text = "";
			txtContactFirstName.Text = "";
			txtContactLastName.Text = "";
			txtContactEmail.Text = "";
			txtDocURL.Text = "";
			txtNotes.Text = "";
			txtLoaderScript.Text = "";
            //Disable guid modification
            txtClientGuid.Text = "";
            txtClientGuid.ReadOnly = false;
            txtClientGuid.Enabled = true;
            btnGuid.Visible = true;
            cbxScheduling.Checked = false;
            cbxESS.Checked = false;
            cbxIsReentrant.Checked = false;

			//Clear the Lab Servers repeater
			repLabServers.DataSource = "";
			repLabServers.DataBind();

			//Clear the Client Info repeater
			repClientInfo.DataSource = "";
			repClientInfo.DataBind();

            ddlAssociatedESS.SelectedIndex = 0;
            ddlAssociatedESS.Enabled = false;
            btnRegisterESS.Visible = false;
            btnDissociateESS.Visible = false;

            ddlAssociatedUSS.SelectedIndex = 0;
            ddlAssociatedUSS.Enabled = false;
            btnRegisterUSS.Visible = false;
            btnDissociateUSS.Visible = false;
           

		}

        //Checks whether there are a USS and/or an ESS associated with the selected client
        private void CheckAssociatedResources(LabClient client)
        {
            int clientID = client.clientID;

            if (client.needsScheduling)
            {
                int ussId = 0;
                 ResourceMappingValue [] ussValues = new ResourceMappingValue[2];

                 ussValues[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.SCHEDULING_SERVER);
                 ussValues[1] = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                        TicketTypes.GetTicketType(TicketTypes.SCHEDULE_SESSION));
                List<ResourceMapping> ussList = ResourceMapManager.Find(
                    new ResourceMappingKey(ResourceMappingTypes.CLIENT,clientID),ussValues);
                if(ussList != null && ussList.Count > 0){
                    foreach(ResourceMapping rm in ussList){
                        for(int i=0; i<rm.values.Length;i++){
                            if(rm.values[i].Type == ResourceMappingTypes.PROCESS_AGENT){
                                ussId = (int) rm.values[i].Entry;
                                Session["ClientUssMappingID"] = rm.MappingID;
                                break;
                            }
                        }
                    }
                }
                if (ussId > 0)
                {
                    //ResourceMappingValue [] ussValues = new ResourceMappingValue[2];
                    //ussValues[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.SCHEDULING_SERVER);
                    ////valuesList.Add(new ResourceMappingValue(ResourceMappingTypes.PROCESS_AGENT, ussId));
                    //ussValues[1] = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                    //    TicketTypes.GetTicketType(TicketTypes.SCHEDULE_SESSION));
                    //ResourceMappingValue[] valuesForKey = (ResourceMappingValue[])valuesList.ToArray((new ResourceMappingValue()).GetType());

                    //foreach (DictionaryEntry entry in mappingsTable)
                    //{
                    //    if (ticketing.EqualMappingValues((ResourceMappingValue[])entry.Value, valuesForKey))
                    //        (int)entry.Key;
                    //}


                    //int[] id = { ticketing.GetProcessAgentID(uss.agentGuid) };
                    //IntTag[] ussTag = ticketing.GetProcessAgentTags(id);
                    btnRegisterUSS.Visible = false;
                    btnDissociateUSS.Visible = true;
                    ddlAssociatedUSS.SelectedValue = ussId.ToString();
                    ddlAssociatedUSS.Enabled = false;
                }

                else
                {
                    btnRegisterUSS.Visible = true && client.labServerIDs.Length > 0;
                    btnDissociateUSS.Visible = false;
                    ddlAssociatedUSS.Enabled = true && client.labServerIDs.Length > 0;
                    Session.Remove("ClientUssMappingID");
                }
            }
            else{
                btnRegisterUSS.Visible = false;
                btnDissociateUSS.Visible = false;
                ddlAssociatedUSS.Enabled = false;
                Session.Remove("ClientUssMappingID");

            }
            if(client.needsESS){
               int essId = 0;
                 ResourceMappingValue [] essValues = new ResourceMappingValue[2];

                 essValues[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.EXPERIMENT_STORAGE_SERVER);
                 essValues[1] = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                        TicketTypes.GetTicketType(TicketTypes.ADMINISTER_EXPERIMENT));
                List<ResourceMapping> essList = ResourceMapManager.Find(
                    new ResourceMappingKey(ResourceMappingTypes.CLIENT,clientID),essValues);
                if(essList != null && essList.Count > 0){
                    foreach(ResourceMapping rm in essList){
                        for(int j=0; j<rm.values.Length;j++){
                            if(rm.values[j].Type == ResourceMappingTypes.PROCESS_AGENT){
                                essId = (int) rm.values[j].Entry;
                                Session["ClientEssMappingID"] = rm.MappingID;
                                break;
                            }
                        }
                    }
                }

                if (essId > 0)
                {
                    //int[] id = { ticketing.GetProcessAgentID(ess.agentGuid) };
                    btnRegisterESS.Visible = false;
                    btnDissociateESS.Visible = true;
                    ddlAssociatedESS.SelectedValue = essId.ToString();
                    ddlAssociatedESS.Enabled = false;
                }

                else
                {
                    btnRegisterESS.Visible = true;
                    btnDissociateESS.Visible = false;
                    ddlAssociatedESS.Enabled = true;
                    Session.Remove("ClientEssMappingID");
                }
            }

            else
            {
            
                btnRegisterESS.Visible = false;
                btnDissociateESS.Visible = false;
                ddlAssociatedESS.Enabled = false;
                
                Session.Remove("ClientEssMappingID");

            }

        }


		/// <summary>
		/// This method loads the text fields on the form from an array of
		/// LabClient objects loaded from the database
		/// </summary>
		private void LoadFormFields()
		{
			ClearFormFields();
			// load values from the LabClient object whose array index matches
			// the dropdown (offset by 1, for the "Select" line at the top of the dropdown)
			LabClient lc = new LabClient();
			lc = labClients[ddlLabClient.SelectedIndex - 1];
            txtClientGuid.Text = lc.clientGuid;
            if (lc.clientGuid != null && lc.clientGuid.Length > 1)
            {
                txtClientGuid.ReadOnly = true;
                txtClientGuid.Enabled = false;
                btnGuid.Visible = false;
            }
            else
            {
                txtClientGuid.ReadOnly = false;
                txtClientGuid.Enabled = true;
                btnGuid.Visible = true;
            }
			txtLabClientName.Text = lc.clientName;
			txtVersion.Text = lc.version;
			txtShortDesc.Text = lc.clientShortDescription;
			txtLongDesc.Text = lc.clientLongDescription;
			txtContactFirstName.Text = lc.contactFirstName;
			txtContactLastName.Text = lc.contactLastName;
			txtContactEmail.Text = lc.contactEmail;
            cbxScheduling.Checked = lc.needsScheduling;
            cbxESS.Checked = lc.needsESS;
            cbxIsReentrant.Checked = lc.IsReentrant;
			
			ddlClientTypes.Items.Clear();
			string[] clientTypes = InternalAdminDB.SelectLabClientTypes();

			foreach (string cType in clientTypes)
			{
				ListItem li = new ListItem (cType);
				if (cType.Equals(lc.clientType))
					li.Selected=true;
				ddlClientTypes.Items.Add(li);
			}

			// the Documentation URL is in an array of ClientInfo objects,
			// which is itself part of the LabClient object

			// Commented this out in the ASPX page, since this information is 
			// duplicated in the ClientInfo repeater.
			// Also, there is no obvious way to handle multiple lines here.
			foreach(ClientInfo ci in lc.clientInfos)
			{
				if (ci.infoURLName.ToUpper().Equals("DOCUMENTATION"))
					txtDocURL.Text = ci.infoURL;
				break;
			}

			txtNotes.Text = lc.notes;
			txtLoaderScript.Text = lc.loaderScript;

            //Check if there is an associated USS and/or ESS => lab experiment needs scheduling and/or storage
            CheckAssociatedResources(lc);

			// Load Lab Server Repeater
			RefreshLabServerRepeater();

			// Load ClientInfo Repeater
			RefreshClientInfoRepeater();

			// Initialize and load the "Edit Associated Lab Servers" button's 
			// javascript onclick routine with the correct Lab Client ID in the querystring
			string assocPopupScript;
            assocPopupScript = "javascript:window.open('assocLabServersPopup.aspx?lc=";
            assocPopupScript += lc.clientID.ToString();
            assocPopupScript += "','managelabclients','scrollbars=yes,resizable=yes,width=700,height=400').focus()";
			btnEditList.Attributes.Remove("onClick");
			btnEditList.Attributes.Add("onClick", assocPopupScript);

			// Initialize and load the "Edit Associated Lab Servers" button's 
			// javascript onclick routine with the correct Lab Client ID in the querystring
			string infoPopupScript = "javascript:window.open('addInfoURLPopup.aspx?lc=";
            infoPopupScript += lc.clientID.ToString();
            infoPopupScript += "','manageclientinfo','scrollbars=yes,resizable=yes,width=800,height=600').focus()";
			btnAddEditResources.Attributes.Remove("onClick");
			btnAddEditResources.Attributes.Add("onClick", infoPopupScript);

            

            // Initialize and load the "Edit Associated groups" button's 
            // javascript onclick routine with the correct Lab Client ID in the querystring
            string groupsPopupScript = "javascript:window.open('manageLabGroups.aspx?lc=";
            groupsPopupScript += lc.clientID.ToString();
            groupsPopupScript += "','manageclientinfo','scrollbars=yes,resizable=yes,width=800,height=600').focus()";
            btnAssociateGroups.Attributes.Remove("onClick");
            btnAssociateGroups.Attributes.Add("onClick", groupsPopupScript);

		}

		protected void ddlLabClient_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            lblResponse.Visible = false;
            lblResponse.Text = "";

			if (ddlLabClient.SelectedIndex == 0)
			{
				// prepare for a new record
				ClearFormFields();

				// Disable the button that pops up the Associated Lab Server edit page
				btnEditList.Visible = false;			
			}
			else
			{
				// edit an existing record
				LoadFormFields();

				// Enable the button that pops up the Associated Lab Server edit page
				btnEditList.Visible = true;			

			}
		}

		/// <summary>
		/// Clears the form in preparation for a new record
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnNew_Click(object sender, System.EventArgs e)
		{
            lblResponse.Visible = false;
            lblResponse.Text = "";

			ddlLabClient.SelectedIndex = 0;
			ClearFormFields();
		}

		/// <summary>
		/// The Save Button method.
		/// The index of the Lab Client dropdown will be used to determine whether 
		/// this is a new (0) or an existing (>0) Lab Client.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnSaveChanges_Click(object sender, System.EventArgs e)
		{
            lblResponse.Visible = false;
            lblResponse.Text = "";

            if (txtVersion.Text == null || txtVersion.Text.Equals(""))
            {
                lblResponse.Text = Utilities.FormatWarningMessage("You must speecify a version for the client!");
                lblResponse.Visible = true;
                return;
            }
            if (txtClientGuid.Text == null || txtClientGuid.Text.Equals("") || txtClientGuid.Text.Length >50)
            {
                lblResponse.Text = Utilities.FormatWarningMessage("You must speecify a GUID for the client, the maximun number of characters is 50!");
                lblResponse.Visible = true;
                return;
            }
            if (txtLoaderScript.Text == null || txtLoaderScript.Text.Equals(""))
            {
                lblResponse.Text = Utilities.FormatWarningMessage("You must speecify a loader script for the client!");
                lblResponse.Visible = true;
                return;
            }
			int[] labServerIDs = new int[0];
			ClientInfo[] clientInfos;

			if ((txtDocURL.Text!=null)&&(txtDocURL.Text.Trim()!=""))
			{
				clientInfos = new ClientInfo[1];
				clientInfos[0].infoURL=txtDocURL.Text;
				clientInfos[0].infoURLName="Documentation";
				clientInfos[0].description="Link to documentation";
			}
			else
			{
				clientInfos = new ClientInfo[0];
			}

			///////////////////////////////////////////////////////////////
			/// ADD a new Lab Client                                     //
			///////////////////////////////////////////////////////////////
			if(ddlLabClient.SelectedIndex == 0)
			{
				// Add the Lab Client
				try
				{
                    // NEEDS TO BE CHANGED !!!!!!!!!!!!
					labClientID = wrapper.AddLabClientWrapper(txtClientGuid.Text, txtLabClientName.Text, txtVersion.Text, txtShortDesc.Text, 
                        txtLongDesc.Text, txtNotes.Text, txtLoaderScript.Text, ddlClientTypes.SelectedItem.Text, 
                        labServerIDs, txtContactEmail.Text, txtContactFirstName.Text, txtContactLastName.Text, 
                        cbxScheduling.Checked,cbxESS.Checked,cbxIsReentrant.Checked, clientInfos)	;
				}
				catch (AccessDeniedException ex)
				{
					lblResponse.Visible = true;
					lblResponse.Text = Utilities.FormatErrorMessage(ex.Message+". "+ex.GetBaseException());
					return;
				}

				// If successful...
				if (labClientID != -1)
				{
					lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatConfirmationMessage("Lab Client " + txtLabClientName.Text + " has been added.");

					
				}
				else // cannot create lab client
				{
					lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Cannot create Lab Client " + txtLabClientName.Text + ".");
				}

				// set dropdown to newly created Lab Client.
				InitializeDropDown();
				ddlLabClient.Items.FindByText(txtLabClientName.Text).Selected = true;
				// Prepare record for editing
				LoadFormFields();
                //Disable guid modification
                txtClientGuid.ReadOnly = true;
                txtClientGuid.Enabled = false;
				// Enable the button that pops up the Associated Lab Server edit page
				btnEditList.Visible = true;

			}
			else
			///////////////////////////////////////////////////////////////
			/// MODIFY an existing Lab Client                            //
			///////////////////////////////////////////////////////////////
			{
				// Save the index
				int savedSelectedIndex = ddlLabClient.SelectedIndex;
				
				// obtain information not edited in the text boxes from the array of LabClient objects
				labClientID = labClients[ddlLabClient.SelectedIndex - 1].clientID;
				labServerIDs = labClients[ddlLabClient.SelectedIndex -1].labServerIDs;
				if ((txtDocURL.Text!=null)&&(txtDocURL.Text.Trim()!=""))
				{
					ArrayList clientInfoList = new ArrayList(labClients[ddlLabClient.SelectedIndex-1].clientInfos);
					foreach (ClientInfo ci in clientInfoList)
					{
						if (ci.infoURLName.ToUpper().Equals("DOCUMENTATION"))
						{
							clientInfoList.Remove(ci);
							break;
						}
					}
					ClientInfo doc = new ClientInfo();
					doc.infoURL=txtDocURL.Text;
					doc.infoURLName="Documentation";
					doc.description="Link to documentation";
					clientInfoList.Add(doc);

					//Convert array list to array
					clientInfos = new ClientInfo[clientInfoList.Count];
					for (int i =0;i<clientInfoList.Count;i++)
						clientInfos[i]=(ClientInfo) clientInfoList[i];

				}
				else
					clientInfos = labClients[ddlLabClient.SelectedIndex -1].clientInfos;

				// Modify the Lab Client Record
				try
				{
                    LabClient lc = labClients[ddlLabClient.SelectedIndex -1];

                    wrapper.ModifyLabClientWrapper(labClientID, txtLabClientName.Text, txtVersion.Text, txtShortDesc.Text, 
                        txtLongDesc.Text, txtNotes.Text, txtLoaderScript.Text, ddlClientTypes.SelectedItem.Text, 
                        labServerIDs, txtContactEmail.Text, txtContactFirstName.Text, txtContactLastName.Text, 
                        cbxScheduling.Checked, cbxESS.Checked, cbxIsReentrant.Checked, clientInfos);

					// Reload the Lab Client dropdown
					InitializeDropDown();
					ddlLabClient.SelectedIndex = savedSelectedIndex;
                    LoadFormFields();
                    //Disable guid modification
                    txtClientGuid.ReadOnly = true;
                    txtClientGuid.Enabled = false;
					lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatConfirmationMessage("Lab Client " + txtLabClientName.Text + " has been modified.");
				}
				catch (Exception ex)
				{
					lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Lab Client " + txtLabClientName.Text + " could not be modified." + ex.GetBaseException());
				}
			}
	
		}

		/// <summary>
		/// Creates an ArrayList of LabServer objects.
		/// Binds the LabServer Repeater to this ArrayList.
		/// </summary>
		private void RefreshLabServerRepeater()
		{
			LabClient lc = new LabClient();
			lc = labClients[ddlLabClient.SelectedIndex - 1];

			ArrayList labServersList = new ArrayList();
			labServerIDs = lc.labServerIDs;
			labServers  = wrapper.GetProcessAgentInfosWrapper(labServerIDs);
			foreach(ProcessAgentInfo ls in labServers)
			{
                if(!ls.retired)
				    labServersList.Add(ls);
			}
			repLabServers.DataSource = labServersList;
			repLabServers.DataBind();
		
		}

		/// <summary>
		/// This is a hidden HTML button that is "clicked" by an event raised 
		/// by the closing of the associatedLabServers popup.
		/// It causes the Lab Servers repeater to be refreshed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnRefresh_ServerClick(object sender, System.EventArgs e)
		{
            lblResponse.Visible = false;
            lblResponse.Text = "";

			RefreshLabServerRepeater();
			RefreshClientInfoRepeater();
            LoadFormFields();
		}

		private void RefreshClientInfoRepeater()
		{
            //LoadFormFields();

			// refresh the array of LabClient objects from the database.
			// This insures that any changed ClientInfo arrays (one per LabClient Object)
			// are retrieved.
            labClientIDs = wrapper.ListLabClientIDsWrapper();
            labClients = wrapper.GetLabClientsWrapper(labClientIDs);

            LabClient lc = new LabClient();
            lc = labClients[ddlLabClient.SelectedIndex - 1];

            ArrayList clientInfosList = new ArrayList();

            foreach (ClientInfo ci in lc.clientInfos)
            {
                if (!ci.infoURLName.ToUpper().Equals("DOCUMENTATION"))
                    clientInfosList.Add(ci);
            }

            repClientInfo.DataSource = clientInfosList;
            repClientInfo.DataBind();
            //Disable guid modification
            txtClientGuid.ReadOnly = true;
            txtClientGuid.Enabled = false;
		}

		protected void btnAddEditResources_Click(object sender, System.EventArgs e)
		{
		
		}

		protected void btnRemove_Click(object sender, System.EventArgs e)
		{
            lblResponse.Visible = false;
            lblResponse.Text = "";

			if(ddlLabClient.SelectedIndex == 0)
			{
				lblResponse.Visible = true;
                lblResponse.Text = Utilities.FormatErrorMessage("Please select a lab client from dropdown list to delete");
				return;
			}
			else
			{
				labClientID = labClients[ddlLabClient.SelectedIndex - 1].clientID;
				try
				{
					wrapper.RemoveLabClientsWrapper(new int[] {labClientID});
					lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatConfirmationMessage("Lab Client '" + txtLabClientName.Text + "' has been deleted");
					InitializeDropDown();
					ClearFormFields();
				}
				catch (Exception ex)
				{
					lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Lab Client " + txtLabClientName.Text + "' cannot be deleted." + ex.GetBaseException());
				}

			}
		}

        protected void btnRegisterUSS_Click(object sender, EventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";

            StringBuilder message = new StringBuilder();
            try
            {
                if (ddlLabClient.SelectedIndex == 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Please save the Lab Client information before attempting to dissociate it from a resource");
                    return;
                }

                if (ddlAssociatedUSS.SelectedIndex == 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Please select a desired USS to be associated with the client.");
                    return;
                }

                LabClient lc = new LabClient();
                lc = labClients[ddlLabClient.SelectedIndex - 1];


                Object keyObj = int.Parse(ddlLabClient.SelectedValue);
                string keyType = ResourceMappingTypes.CLIENT;

                ArrayList valuesList = new ArrayList();
                Object valueObj = null;

                ResourceMappingValue value = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.SCHEDULING_SERVER);
                valuesList.Add(value);

                int ussId = int.Parse(ddlAssociatedUSS.SelectedValue);

                if (ussId > 0)
                {
                    value = new ResourceMappingValue(ResourceMappingTypes.PROCESS_AGENT, ussId);
                    valuesList.Add(value);

                    value = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                        TicketTypes.GetTicketType(TicketTypes.SCHEDULE_SESSION));
                    valuesList.Add(value);

                    ResourceMappingKey key = new ResourceMappingKey(keyType, keyObj);
                    ResourceMappingValue[] values = (ResourceMappingValue[])valuesList.ToArray((new ResourceMappingValue()).GetType());

                    // Add the mapping to the database & cache
                    ResourceMapping newMapping = ticketing.AddResourceMapping(key, values);

                    // add mapping to qualifier list
                    int qualifierType = Qualifier.resourceMappingQualifierTypeID;
                    string name = ticketing.ResourceMappingToString(newMapping);
                    int qualifierID = AuthorizationAPI.AddQualifier(newMapping.MappingID, qualifierType, name, Qualifier.ROOT);

                    // Moved to ManageLabGroups
                    //Give the Manager Group a Grant "MANAGE_USS_GROUP" on the created resource mapping
                    //int agentID = Convert.ToInt32(ddlAdminGroup.SelectedValue);
                    //string function = Function.manageUSSGroup;
                    //int grantID = wrapper.AddGrantWrapper(agentID, function, qualifierID);


                    Session["ClientUssMappingID"] = newMapping.MappingID;

                    btnRegisterUSS.Visible = false;
                    btnDissociateUSS.Visible = true;
                    ddlAssociatedUSS.Enabled = false;

                    
                    message.AppendLine("User-side Scheduling Server \"" + ddlAssociatedUSS.SelectedItem.Text + "\" succesfully "
                        + "associated with client \"" + ddlLabClient.SelectedItem.Text + "\".");
                   

                    //wrapper.ModifyLabClientWrapper(lc.clientID, lc.clientName, lc.version, lc.clientShortDescription,
                    //    lc.clientLongDescription, lc.notes, lc.loaderScript, lc.clientType,
                    //    lc.labServerIDs, lc.contactEmail, lc.contactFirstName, lc.contactLastName,
                    //    lc.needsScheduling, lc.needsESS, lc.IsReentrant, lc.clientInfos);

                    TicketLoadFactory factory = TicketLoadFactory.Instance();
                    ProcessAgentInfo uss = ticketing.GetProcessAgentInfo(ussId);
                    if (uss.retired)
                    {
                        throw new Exception("The specified USS is retired");
                    }
                    //this should be in a loop
                    for(int i = 0;i<lc.labServerIDs.Length;i++){
                       
                        if (lc.labServerIDs[i] > 0)
                        {
                            ProcessAgentInfo labServer = ticketing.GetProcessAgentInfo(lc.labServerIDs[i]);
                            if (labServer.retired)
                            {
                                throw new Exception("The lab server is retired");
                            }
                            int lssId = ticketing.FindProcessAgentIdForAgent(lc.labServerIDs[i], ProcessAgentType.LAB_SCHEDULING_SERVER);

                            if (lssId > 0)
                            {

                                ProcessAgentInfo lss = ticketing.GetProcessAgentInfo(lssId);
                                if (lss.retired)
                                {
                                    throw new Exception("The LSS is retired");
                                }
                                // The REVOKE_RESERVATION ticket
                                string revokePayload = factory.createRevokeReservationPayload();
                                Coupon ussCoupon = ticketing.CreateTicket(TicketTypes.REVOKE_RESERVATION, uss.agentGuid,
                                    lss.agentGuid, -1L, revokePayload);

                                // Is this in the domain or cross-domain
                                if (lss.domainGuid.Equals(ProcessAgentDB.ServiceGuid))
                                {
                                    // this domain
                                    //Add USS on LSS


                                    //string lssPayload = factory.createAdministerLSSPayload(Convert.ToInt32(Session["UserTZ"]));
                                    //long duration = 60; //seconds for the LSS to redeem the ticket of this coupon and Add LSS Info
                                    //ticketing.AddTicket(lssCoupon, TicketTypes.ADMINISTER_LSS, lss.agentGuid,
                                    //    ticketing.ServiceGuid(), duration, lssPayload);

                                    LabSchedulingProxy lssProxy = new LabSchedulingProxy();
                                    lssProxy.Url = lss.webServiceUrl;
                                    lssProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                                    lssProxy.AgentAuthHeaderValue.coupon = lss.identOut;
                                    lssProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                                    int ussAdded = lssProxy.AddUSSInfo(uss.agentGuid, uss.agentName, uss.webServiceUrl, ussCoupon);
                                    lssProxy.AddExperimentInfo(labServer.agentGuid, labServer.agentName, lc.clientGuid, lc.clientName, lc.version, lc.contactEmail);
                                }
                                else
                                {
                                    // cross-domain
                                    // send consumerInfo to remote SB
                                    int remoteSbId = ticketing.GetProcessAgentID(lss.domainGuid);
                                    message.AppendLine(RegistrationSupport.RegisterClientUSS(remoteSbId, null, lss.agentId, null, labServer.agentId,
                                        ussCoupon, uss.agentId, null, lc.clientID));

                                   
                                }
                                //ADD LSS on USS
                                string ussPayload = factory.createAdministerUSSPayload(Convert.ToInt32(Session["UserTZ"]));
                                
                                UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                                ussProxy.Url = uss.webServiceUrl;
                                ussProxy.AgentAuthHeaderValue = new AgentAuthHeader();
                                ussProxy.AgentAuthHeaderValue.coupon = uss.identOut;
                                ussProxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                                int lssAdded = ussProxy.AddLSSInfo(lss.agentGuid, lss.agentName, lss.webServiceUrl);

                                //Add Experiment Information on USS
                                int expInfoAdded = ussProxy.AddExperimentInfo(labServer.agentGuid, labServer.agentName,
                                    lc.clientGuid, lc.clientName, lc.version, lc.contactEmail, lss.agentGuid);

                                // Group Credentials MOVED TO THE MANAGE LAB GROUPS PAGE
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            lblResponse.Visible = true;
            lblResponse.Text = Utilities.FormatConfirmationMessage(message.ToString());
        }

        protected void btnRegisterESS_Click(object sender, EventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";

            try
            {
                if (ddlLabClient.SelectedIndex == 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Please save the Lab Client information before attempting to associate it with a resource");
                    return;
                }

                if (ddlAssociatedESS.SelectedIndex == 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Please select a desired ESS to be associated with the client.");
                    return;
                }                

                LabClient lc = new LabClient();
                lc = labClients[ddlLabClient.SelectedIndex - 1];

                Object keyObj = int.Parse(ddlLabClient.SelectedValue);
                string keyType = ResourceMappingTypes.CLIENT;

                ArrayList valuesList = new ArrayList();
                Object valueObj = null;

                ResourceMappingValue value = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.EXPERIMENT_STORAGE_SERVER);
                valuesList.Add(value);

                value = new ResourceMappingValue(ResourceMappingTypes.PROCESS_AGENT,
                    int.Parse(ddlAssociatedESS.SelectedValue));
                valuesList.Add(value);

                value = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                    TicketTypes.GetTicketType(TicketTypes.ADMINISTER_EXPERIMENT));
                valuesList.Add(value);

                ResourceMappingKey key = new ResourceMappingKey(keyType, keyObj);
                ResourceMappingValue[] values = (ResourceMappingValue[])valuesList.ToArray((new ResourceMappingValue()).GetType());
                ResourceMapping newMapping = ticketing.AddResourceMapping(key, values);



                // add mapping to qualifier list
                int qualifierType = Qualifier.resourceMappingQualifierTypeID;
                string name = ticketing.ResourceMappingToString(newMapping);
                int qualId = AuthorizationAPI.AddQualifier(newMapping.MappingID, qualifierType, name, Qualifier.ROOT);

                // No Grant required for ESS

                Session["ClientEssMappingID"] = newMapping.MappingID;

                btnRegisterESS.Visible = false;
                btnDissociateESS.Visible = true;
                ddlAssociatedESS.Enabled = false;

                lblResponse.Visible = true;
                lblResponse.Text = Utilities.FormatConfirmationMessage("Experiment Storage Server \"" + ddlAssociatedESS.SelectedItem.Text + "\" succesfully "
                    + "associated with client \"" + ddlLabClient.SelectedItem.Text + "\".");

                //wrapper.ModifyLabClientWrapper(lc.clientID, lc.clientName, lc.version, lc.clientShortDescription,
                //    lc.clientLongDescription, lc.notes, lc.loaderScript, lc.clientType,
                //    lc.labServerIDs, lc.contactEmail, lc.contactFirstName, lc.contactLastName,
                //    lc.needsScheduling, lc.needsESS, lc.IsReentrant, lc.clientInfos);

            }
            catch
            {
                throw;
            }
        }


        protected void btnDissociateUSS_Click(object sender, EventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";

            try
            {
                if (ddlLabClient.SelectedIndex == 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Please save the Lab Client information before attempting to dissociate it from a resource");
                    return;
                }

                if (ddlAssociatedUSS.SelectedIndex == 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Please select a desired USS to be dissociated from the client.");
                    return;
                }

                LabClient lc = new LabClient();
                lc = labClients[ddlLabClient.SelectedIndex - 1];

                ticketing.DeleteResourceMapping((int) Session["ClientUssMappingID"]);
                btnRegisterUSS.Visible = true;
                btnDissociateUSS.Visible = false;
                Session.Remove("ClientUssMappingID");

                lblResponse.Visible = true;
                lblResponse.Text = Utilities.FormatConfirmationMessage("User-side Scheduling Server \"" + ddlAssociatedUSS.SelectedItem.Text + "\" succesfully "
                    + "dissociated from client \"" + ddlLabClient.SelectedItem.Text + "\".");

                ddlAssociatedUSS.Enabled = true;
                ddlAssociatedUSS.SelectedIndex = 0;

            }
            catch
            {
                throw;
            }
        }
        protected void btnDissociateESS_Click(object sender, EventArgs e)
        {
            lblResponse.Visible = false;
            lblResponse.Text = "";

            try
            {
                if (ddlLabClient.SelectedIndex == 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Please save the Lab Client information before attempting to dissociate it from a resource");
                    return;
                }

                if (ddlAssociatedESS.SelectedIndex == 0 || ddlLabClient.SelectedIndex == 0)
                {
                    lblResponse.Visible = true;
                    lblResponse.Text = Utilities.FormatErrorMessage("Please select a desired ESS to be dissociated from the client.");
                    return;
                }

                LabClient lc = new LabClient();
                lc = labClients[ddlLabClient.SelectedIndex - 1];


                ticketing.DeleteResourceMapping((int) Session["ClientEssMappingID"]);
                btnRegisterESS.Visible = true;
                btnDissociateESS.Visible = false;
                Session.Remove("ClientEssMappingID");
                lblResponse.Visible = true;
                lblResponse.Text = Utilities.FormatConfirmationMessage("Experiment Storage Server \"" + ddlAssociatedESS.SelectedItem.Text + "\" succesfully "
                    + "dissociated from client \"" + ddlLabClient.SelectedItem.Text + "\".");

                ddlAssociatedESS.Enabled = true;
                ddlAssociatedESS.SelectedIndex = 0;
            }
            catch
            {
                throw;
            }
        }
        protected void btnAssociateGroups_Click(object sender, EventArgs e)
        {

        }
        protected void btnEditList_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuid_Click(object sender, System.EventArgs e)
        {
            Guid guid = System.Guid.NewGuid();
            txtClientGuid.Text = Utilities.MakeGuid();
            valGuid.Validate();
        }

        protected void checkGuid(object sender, ServerValidateEventArgs args)
        {
            if (args.Value.Length > 0 && args.Value.Length <= 50)
                args.IsValid = true;
            else
                args.IsValid = false;
        }

}
}
