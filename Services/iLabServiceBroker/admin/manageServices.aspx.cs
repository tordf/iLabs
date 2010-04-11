/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: manageServices.aspx.cs,v 1.45 2008/05/07 22:52:32 pbailey Exp $
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
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Mapping;
using iLabs.ServiceBroker;
using iLabs.Ticketing;

//using iLabs.Services;
using iLabs.ServiceBroker;
using iLabs.DataTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;

namespace iLabs.ServiceBroker.admin
{
    /// <summary>
    /// Allows an Administrator to add, modify, or delete a Lab Server database record
    /// </summary>
    public partial class manageServices : System.Web.UI.Page
    {
        protected bool isNew = false;
        protected bool hasAdminGroups = true;
        protected bool hasManageLSSGroups = false;
        protected BrokerDB brokerDB = new BrokerDB();
        protected AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

        //243,239,229 - light green
        //174,155,138 - brown
        private Color disabled = Color.FromArgb(243, 239, 229);
        private Color enabled = Color.White;

        //private int currMappingID = -1;


        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("../login.aspx");
            
            if (!IsPostBack)
            {
                txtServiceGUID.ReadOnly = true;
                txtServiceGUID.BackColor = disabled;
                txtServiceType.ReadOnly = true;
                txtServiceType.BackColor = disabled;
                txtApplicationURL.ReadOnly = true;
                txtApplicationURL.BackColor = disabled;
                txtDomainServer.ReadOnly = true;
                txtDomainServer.BackColor = disabled;
                // Get list of services
                InitializeDropDown();
                //Put in availabe admin groups
                IntializeAdminGroupList();
                IntializeManageLSSList();
                SetInputMode(false);
                lblAdminGroup.Visible = true;
                ddlAdminGroup.Enabled = true;
                ddlAdminGroup.BackColor = enabled;
                ddlAdminGroup.Visible = true;
                trAdminGroup.Visible = false;
                trAssociate.Visible = false;
                trManage.Visible = false;
                //lblAssociate.Visible = false;
                //ddlLSS.Visible = false;
                //btnAssociateLSS.Visible = false;
                Session.Remove("LS_LSSmapID");
                Session.Remove("DomainGuid");
               
            }
            //else // Do any stuff that needs to be done on Postback PageOpen calls
            //{
                
            //}
/*
            // Get System Messages on all OpenPage calls
            ArrayList messagesList = new ArrayList();
            SystemMessage[] messages = wrapper.GetSystemMessagesWrapper("System", 0, 0);
            foreach (SystemMessage message in messages)
            {
                messagesList.Add(message);
            }

            messagesList.Sort(new DateComparer());
            messagesList.Reverse();

            if (messagesList.Count == 0)
            {
                SystemMessage noMessage = new SystemMessage();
                noMessage.messageBody = "No Messages at this time";
                noMessage.lastModified = DateTime.Now;
                messagesList.Add(noMessage);
            }

            repSystemMessage.DataSource = messagesList;
            repSystemMessage.DataBind();
            */
           

            // Reset error/confirmation message
            lblErrorMessage.Visible = false;

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
            brokerDB = new BrokerDB();

        }
        #endregion


        private void IntializeAdminGroupList()
        {
            ddlAdminGroup.Items.Clear();
            ListItem liHeaderAdminGroup = new ListItem("---No Admin Group---", "-1");
            ddlAdminGroup.Items.Add(liHeaderAdminGroup);

            int[] groupIDs = AdministrativeAPI.ListAdminGroupIDs();
            if (groupIDs.Length > 0)
            {
                hasAdminGroups = true;
                Group[] gps = AdministrativeAPI.GetGroups(groupIDs);
                foreach (Group group in gps)
                {
                    ListItem li = new ListItem(group.groupName, group.groupID.ToString());
                    ddlAdminGroup.Items.Add(li);
                }
            }
            else
            {
                hasAdminGroups = false;
            }
        }

        private void IntializeManageLSSList()
        {
            ddlManageLSS.Items.Clear();
            ListItem liHeaderAdminGroup = new ListItem("---No Management Group---", "-1");
            ddlManageLSS.Items.Add(liHeaderAdminGroup);

            int[] groupIDs = AdministrativeAPI.ListAdminGroupIDs();
            if (groupIDs.Length > 0)
            {
                hasManageLSSGroups = true;
                Group[] gps = AdministrativeAPI.GetGroups(groupIDs);
                foreach (Group group in gps)
                {
                    ListItem li = new ListItem(group.groupName, group.groupID.ToString());
                    ddlManageLSS.Items.Add(li);
                }
            }
            else
            {
                hasManageLSSGroups = false;
            }
        }


        private int CheckAssociatedLSSForLS(int lsId)
        {
            int lssId = 0;
            ddlLSS.ClearSelection();
            ddlLSS.SelectedIndex = 0;
            ddlLSS.BackColor = enabled;
            ddlLSS.Enabled = true;
            ddlManageLSS.ClearSelection();
            ddlManageLSS.SelectedIndex = 0;
            ddlManageLSS.BackColor = enabled;
            ddlManageLSS.Enabled = true;
            ResourceMappingValue[] values = new ResourceMappingValue[2];
            values[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.LAB_SCHEDULING_SERVER);
            values[1] = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                TicketTypes.GetTicketType(TicketTypes.MANAGE_LAB));

            List<ResourceMapping> maps = ResourceMapManager.Find(new ResourceMappingKey(ResourceMappingTypes.PROCESS_AGENT, lsId), values);
            if (maps != null && maps.Count > 0)
            {
                foreach (ResourceMapping rm in maps)
                {
                    for (int i = 0; i < rm.values.Length; i++)
                    {
                        if (rm.values[i].Type == ResourceMappingTypes.PROCESS_AGENT)
                        {
                            lssId = (int)rm.values[i].Entry;
                            ddlLSS.ClearSelection();
                            ddlLSS.SelectedValue = lssId.ToString();
                            ddlLSS.Enabled = false;
                            ddlLSS.BackColor = disabled;
                            btnAssociateLSS.Text = "Disassociate";
                            btnAssociateLSS.CommandName = "disassociate";
                            Session["LS_LSSmapID"] = rm.MappingID;


                            // Find any manageLab grants
                            int qualID = AuthorizationAPI.GetQualifierID(rm.MappingID, Qualifier.resourceMappingQualifierTypeID);
                            Grant[] grants = AuthorizationAPI.GetGrants(AuthorizationAPI.FindGrants(-1, Function.manageLAB, qualID));
                            foreach (Grant g in grants)
                            {
                                if (ddlManageLSS.Items.FindByValue(g.agentID.ToString()) != null)
                                {
                                    ddlManageLSS.ClearSelection();
                                    ddlManageLSS.Items.FindByValue(g.agentID.ToString()).Selected = true;
                                    ddlManageLSS.Enabled = false;
                                    ddlManageLSS.BackColor = disabled;
                                    break;
                                }

                            }
                            break;
                        }
                    }
                }
            }
            
            else
            {
                ddlLSS.Enabled = true;
                ddlLSS.SelectedIndex = 0;
                ddlLSS.BackColor = enabled;
                Session.Remove("LS_LSSmapID");
                btnAssociateLSS.Text = "Associate";
                btnAssociateLSS.CommandName = "associate";
                ddlManageLSS.Enabled = true;
            }

            return lssId;

        }

        /// <summary>
        /// Clears the Lab Server dropdown and reloads it from the array of Service objects
        /// </summary>
        private void InitializeDropDown()
        {
            //ServiceIDs = wrapper.ListServiceIDsWrapper();
            IntTag[] tags = brokerDB.GetProcessAgentTagsWithType();

            ddlService.Items.Clear();

            ddlService.Items.Add(new ListItem(" ---------- select Service ---------- ", "0"));
            // Do not load the "Unknown Lab Server" record into the dropdown. Hence we start at 1, not 0
            foreach (IntTag t in tags)
            {

                ddlService.Items.Add(new ListItem(t.tag, t.id.ToString()));
            }

            //Put in availabe LSS
            ddlLSS.Items.Clear();
            ListItem liHeaderLss = new ListItem("---Select Lab Side Scheduling Server---", "-1");
            ddlLSS.Items.Add(liHeaderLss);
            IntTag[] lsses = brokerDB.GetProcessAgentTagsByType(ProcessAgentType.LAB_SCHEDULING_SERVER);
            foreach (IntTag lss in lsses)
            {
                ListItem li = new ListItem(lss.tag, lss.id.ToString());
                ddlLSS.Items.Add(li);
            }
        }

        /// <summary>
        /// The GUID and Incoming Passkey fields cannot be edited in an existing record,
        /// but they must be specified for a new record.
        /// This method resets the ReadOnly state and background colors of these fields.
        /// </summary>
        /// <param name="readOnlySwitch">true if displaying a service, false if not</param>
        private void SetInputMode(bool isDisplay)
        {
            txtBatchHelp.Visible = false;
            btnRegister.Text = "Install Domain Credentitials";
            txtWebServiceURL.ReadOnly = isDisplay;
            txtWebServiceURL.BackColor = isDisplay ? disabled : enabled;
            txtServiceGUID.ReadOnly = isDisplay;
            txtServiceGUID.BackColor = isDisplay ? disabled : enabled;
            trPasskey.Visible = !isDisplay;
            txtOutPassKey.ReadOnly = isDisplay;
            txtOutPassKey.BackColor = isDisplay ? disabled : enabled;
            txtApplicationURL.ReadOnly = isDisplay ? true : false;
            txtApplicationURL.BackColor = isDisplay ? disabled : enabled;
            txtServiceName.ReadOnly = !isDisplay;
            txtServiceName.BackColor = !isDisplay ? disabled : enabled;
            txtServiceDescription.ReadOnly = !isDisplay;
            txtServiceDescription.BackColor = !isDisplay ? disabled : enabled;
            txtContactEmail.ReadOnly = !isDisplay;
            txtContactEmail.BackColor = !isDisplay ? disabled : enabled;
           
            txtInfoURL.ReadOnly = !isDisplay ? true : false;
            txtInfoURL.BackColor = !isDisplay ? disabled : enabled;
            trBatchIn.Visible= false;
            trBatchOut.Visible = false;
            
            //lblAdminGroup.Visible = isDisplay;
            //ddlAdminGroup.Visible = isDisplay;
            trAdminGroup.Visible = isDisplay;
            btnRegister.Visible = !isDisplay;
            btnAdminURLs.Visible = isDisplay;
            btnSaveChanges.Visible = isDisplay;
        }

        /// <summary>
        /// The GUID and Incoming Passkey fields cannot be edited in an existing record,
        /// but they must be specified for a new record.
        /// This method resets the ReadOnly state and background colors of these fields.
        /// </summary>
        /// <param name="isDisplay">true if displaying a service, false if not</param>
        private void SetBatchInputMode(bool isDisplay)
        {
            txtBatchHelp.Visible = true;
            btnRegister.Text = "Register LabServer";
            txtWebServiceURL.ReadOnly = false;
            txtWebServiceURL.BackColor = enabled;
            txtServiceGUID.ReadOnly = isDisplay;
            txtServiceGUID.BackColor = isDisplay ? disabled : enabled;
            trPasskey.Visible = false;
            //txtOutPassKey.ReadOnly = isDisplay;
            //txtOutPassKey.BackColor = isDisplay ? disabled : enabled;
            txtApplicationURL.ReadOnly = false;
            txtApplicationURL.BackColor = enabled;
            txtServiceName.ReadOnly = isDisplay;
            txtServiceName.BackColor = isDisplay ? disabled : enabled;
            txtServiceType.Text = ProcessAgentType.BATCH_LAB_SERVER;
            txtServiceType.ReadOnly = true;
            txtServiceType.BackColor = disabled;
            txtServiceDescription.ReadOnly = false; //  !isDisplay;
            txtServiceDescription.BackColor = enabled; // !isDisplay ? disabled : enabled;
            txtContactEmail.ReadOnly = false; // !isDisplay;
            txtContactEmail.BackColor = enabled; // !isDisplay ? disabled : enabled;

            txtInfoURL.ReadOnly = false; // !isDisplay ? true : false;
            txtInfoURL.BackColor = enabled; // !isDisplay ? disabled : enabled;
            trAdminGroup.Visible = false;
            trAssociate.Visible = false;
            trManage.Visible = false;
            trBatchIn.Visible= true;
            txtBatchPassIn.ReadOnly = false;
            txtBatchPassIn.BackColor = enabled;
            trBatchOut.Visible= true;
            txtBatchPassOut.ReadOnly = false;
            txtBatchPassOut.BackColor = enabled;
            btnRegister.Visible = !isDisplay;
            btnAdminURLs.Visible = false;
            btnSaveChanges.Visible = isDisplay;
        }

        /// <summary>
        /// This method clears the form fields.
        /// </summary>
        private void ClearFormFields()
        {
            txtServiceName.Text = "";
            txtServiceGUID.Text = "";

            txtServiceType.Text = "";
            SetInputMode(false);
            txtWebServiceURL.Text = "";
            txtServiceDescription.Text = "";
            txtApplicationURL.Text = "";
            txtDomainServer.Text = "";
            txtInfoURL.Text = "";
            txtContactEmail.Text = "";
            txtBugEmail.Text = "";
            txtLocation.Text = "";
            txtOutPassKey.Text = "";
            txtBatchPassIn.Text = "";
            txtBatchPassOut.Text = "";
            ddlAdminGroup.ClearSelection();
            trAdminGroup.Visible = false;
            trAssociate.Visible = false;
            trManage.Visible = false;
            ddlLSS.ClearSelection();
            //ddlLSS.Visible = false;
            //lblAssociate.Visible = false;
           
            //btnAssociateLSS.Visible = false;
           
        }

        /// <summary>
        /// This method fires when the Service dropdown changes.
        /// If the index is greater than zero, the specified ProcessAgent will be looked up
        /// and its values will be used to populate the text fields on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlService_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cbxDoBatch.Checked = false;
            int id = Convert.ToInt32(ddlService.SelectedValue);
            if (id == 0)
            // prepare for a new record
            {
                ClearFormFields();
                //if(cbxDoBatch.Checked){
                //    SetBatchInputMode(false);
                //}
                //else{
                //    SetInputMode(false);
                //}
                ClearButtonScripts();
            }
            else
            //retrieve an existing record
            {

                displayService(id);
            }
        }

        protected void displayService(int agentId)
        {
            ProcessAgentInfo agent = brokerDB.GetProcessAgentInfo(agentId);
           
            if (agent != null)
            {
                if (agent.retired)
                {
                    ClearFormFields();
                    throw new Exception("The requested service is retired");
                }
                txtServiceName.Text = agent.agentName;
                txtServiceType.Text = ProcessAgentType.ToTypeName(agent.agentType);
                
                txtServiceGUID.Text = agent.agentGuid;
                txtWebServiceURL.Text = agent.webServiceUrl;
                txtApplicationURL.Text = agent.codeBaseUrl;
                if (agent.domainGuid != null)
                {
                    if (agent.domainGuid.Equals(ProcessAgentDB.ServiceGuid))
                    {
                        txtDomainServer.Text = ProcessAgentDB.ServiceAgent.agentName;
                    }
                    else
                    {
                   
                        ProcessAgent remote = brokerDB.GetProcessAgent(agent.domainGuid);
                        txtDomainServer.Text = remote.agentName;
                    }
                }
                Session["DomainGuid"] = agent.domainGuid;

                SystemSupport ss = brokerDB.RetrieveSystemSupport(agentId);
                if(ss != null){
                    txtServiceDescription.Text = ss.description;
                    txtInfoURL.Text = ss.infoUrl;
                    txtContactEmail.Text = ss.contactEmail;
                    txtBugEmail.Text = ss.bugEmail;
                    txtLocation.Text = ss.location;
                }
                else{
                    txtServiceDescription.Text = "";
                    txtInfoURL.Text = "";
                    txtContactEmail.Text = "";
                    txtBugEmail.Text = "";
                    txtLocation.Text = "";
                }


                //Hashtable resources = brokerDB.GetResourceStringTags(agentId, ResourceMappingTypes.PROCESS_AGENT);
                //if (resources != null)
                //{
                //    if (resources.ContainsKey("Description"))
                //        txtServiceDescription.Text = ((IntTag)resources["Description"]).tag;
                //    else
                //        txtServiceDescription.Text = "";

                //    if (resources.ContainsKey("Info URL"))
                //        txtInfoURL.Text = ((IntTag)resources["Info URL"]).tag;
                //    else
                //        txtInfoURL.Text = "";

                //    if (resources.ContainsKey("Contact Email"))
                //        txtContactEmail.Text = ((IntTag)resources["Contact Email"]).tag;
                //    else
                //        txtContactEmail.Text = "";
                //}
                //else
                //{
                //    txtServiceDescription.Text = "";
                //    txtInfoURL.Text = "";
                //    txtContactEmail.Text = "";
                //}
                if (agent.agentType.Equals(ProcessAgentType.AgentType.BATCH_LAB_SERVER))
                {
                    cbxDoBatch.Checked = true;
                   
                    SetBatchInputMode(true);
                    if (agent.identIn != null)
                    {
                        txtBatchPassIn.Text = agent.identIn.passkey;
                    }
                    else
                    {
                        txtBatchPassIn.Text = "";
                    }
                    if (agent.identOut != null)
                    {
                        txtBatchPassOut.Text = agent.identOut.passkey;
                    }
                    else
                    {
                        txtBatchPassOut.Text = "";
                    }
                }
                else
                {
                    SetInputMode(true);
                    if (!agent.agentType.Equals(ProcessAgentType.AgentType.SERVICE_BROKER)
                    && !agent.agentType.Equals(ProcessAgentType.AgentType.REMOTE_SERVICE_BROKER)
                    && !agent.agentType.Equals(ProcessAgentType.AgentType.EXPERIMENT_STORAGE_SERVER)
                    && agent.domainGuid.Equals(ProcessAgentDB.ServiceGuid))
                    {
                        // Admin Group List

                        trAdminGroup.Visible = true;

                        string warningMessage = null;

                        int qualifierTypeID = Qualifier.ToTypeID(ProcessAgentType.ToTypeName(agent.agentType));
                        int qualifierID = AuthorizationAPI.GetQualifierID(agentId, qualifierTypeID);

                        string grantFunction = GetGrantFunction(txtServiceType.Text);

                        // Get all grants for the function and qualifier ( theService )
                        //Ideally, there should only be one group that has the "Administer" function on a particular Process Agent
                        //for now, take the first element of the array
                        int[] grantIDs = wrapper.FindGrantsWrapper(-1, grantFunction, qualifierID);
                        if (grantIDs.Length > 0)
                        {
                            Grant[] grants = wrapper.GetGrantsWrapper(grantIDs);
                            int adminGroupID = grants[0].agentID;
                            if (adminGroupID > 0)
                            {
                                ddlAdminGroup.SelectedValue = adminGroupID.ToString();
                            }
                            else
                            {
                                if (ddlAdminGroup.Items.Count > 0)
                                    ddlAdminGroup.SelectedIndex = 0;
                                else ddlAdminGroup.ClearSelection();
                            }

                            if (grantIDs.Length > 1)
                            {
                                warningMessage = "NOTE: It seems that several Admin Groups are associated with this "
                                    + "process agent. Returning the first one.";
                                lblErrorMessage.Visible = true;
                                Utilities.FormatErrorMessage(warningMessage);
                            }
                        }
                        else
                        {
                            if (ddlAdminGroup.Items.Count > 0)
                                ddlAdminGroup.SelectedIndex = 0;
                            else ddlAdminGroup.ClearSelection();
                        }
                    }

                    else
                    {
                        trAdminGroup.Visible = false;
                        //ddlAdminGroup.SelectedIndex = 0;
                        //ddlAdminGroup.Enabled = false;
                        //ddlAdminGroup.BackColor = disabled;
                        //ddlAdminGroup.Visible = false;
                        //lblAdminGroup.Visible = false;
                    }
                    if (agent.agentType.Equals(ProcessAgentType.AgentType.LAB_SERVER) && agent.domainGuid.Equals(ProcessAgentDB.ServiceGuid))
                    {
                        trAssociate.Visible = true;
                        trManage.Visible = true;
                        CheckAssociatedLSSForLS(agentId);

                    }
                    else
                    {
                        trAssociate.Visible = false;
                        trManage.Visible = false;
                    }
                }

            }
            SetBtnScripts(agent.agentGuid,ProcessAgentType.ToTypeName(agent.agentType));
        }

        protected void btnNew_Click(object sender, System.EventArgs e)
        {
            ddlService.SelectedIndex = 0;
            cbxDoBatch.Checked = false;
            ClearFormFields();
            SetInputMode(false);
        }

        protected void modifySystemSupport(int agentId)
        {
            brokerDB.SaveSystemSupport(agentId, txtContactEmail.Text, txtBugEmail.Text, txtInfoURL.Text,
                txtServiceDescription.Text, txtLocation.Text);
        }

        /// <summary>
        /// The Save Button method. If the GUID field is not set to ReadOnly, this method
        /// will assume that a new record is being created. Otherwise, it will assume that
        /// an existing record is being edited.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveChanges_Click(object sender, System.EventArgs e)
        {
            int agentID = -1;
            ///////////////////////////////////////////////////////////////
            /// ADD a new Service                                     //
            ///////////////////////////////////////////////////////////////
            if (txtServiceGUID.ReadOnly == false) // add new record
            {

               
                // Add the iLab Service
                try
                {
                    //Coupon inCoupon = brokerDB.CreateCoupon();
                    //Coupon outCoupon = brokerDB.CreateCoupon();
                    //agentID = brokerDB.InsertProcessAgent(txtServiceGUID.Text, txtServiceName.Text,txtServiceDescription.Text, txtWebServiceURL.Text, "", txtInfoURL.Text, txtContactEmail.Text, 0f,inCoupon,outCoupon);
                }
                catch (AccessDeniedException ex)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(ex.Message);
                    return;
                }

                // If successful...
                if (agentID != -1)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("iLabs Service " + txtServiceName.Text + " has been added.");

                    // if there is an outgoing passkey, attempt to add it
                    if (txtOutPassKey.Text != "")
                    {
                        try
                        {
                            ; //wrapper.RegisterOutgoingServerPasskeyWrapper(AgentID, txtOutPassKey.Text);
                        }
                        catch
                        {
                            lblErrorMessage.Visible = true;
                            lblErrorMessage.Text += Utilities.FormatErrorMessage(" BUT Cannot add outgoing passkey");
                            return;
                        }
                        // [GeneralbrokerDB] Generate aggregate record in StaticProcessAgent table 
                        // from associated Service and LabClient record(s)
                        bool success = false;
                        //success = this.CreateStaticProcessAgent(AgentID);

                        // if the creation of the process agent and the installation of the domain credentials is not successful, return
                        if (!success)
                        {
                            lblErrorMessage.Visible = true;
                            lblErrorMessage.Visible = true;
                            lblErrorMessage.Text = "CreateStaticProcessAgent: ";
                            return;
                        }
                    }

                }
                else // cannot create service
                {
                    lblErrorMessage.Visible = true;
                    Utilities.FormatErrorMessage(lblErrorMessage.Text = "Cannot create Service: " + txtServiceName.Text + ".");
                }

                // set dropdown to newly created Lab Server.
                InitializeDropDown();
                ddlService.Items.FindByText(txtServiceName.Text).Selected = true;

                SetInputMode(true);

            }
            else // txtServiceGUID.ReadOnly == false // if ReadOnly is true, modify existing record
            {
                ///////////////////////////////////////////////////////////////
                /// MODIFY an existing Service                            //
                ///////////////////////////////////////////////////////////////

                // Save the index
                int savedSelectedIndex = ddlService.SelectedIndex;

                int agentId = Convert.ToInt32(ddlService.SelectedValue);
                try
                {
                    string dGuid = null;
                    if (Session["DomainGuid"] != null)
                    {
                        dGuid = Session["DomainGuid"].ToString();
                    }
                   
                    // Modify the ProcessAgent record
                    wrapper.ModifyProcessAgentWrapper(txtServiceGUID.Text, txtServiceName.Text, txtServiceType.Text,
                       dGuid,txtApplicationURL.Text, txtWebServiceURL.Text);

                    // Need to add processing for SystemSupport, replaces esourceMapping for these fields
                    modifySystemSupport(agentId);

                    // Modify coupons if needed
                    if (txtServiceType.Text.Equals(ProcessAgentType.BATCH_LAB_SERVER))
                    {
                        // Modify coupons if needed
                        ProcessAgentInfo info = brokerDB.GetProcessAgentInfo(agentId);
                        if (info.retired)
                        {
                            throw new Exception("The Batch Lab Server is retired");
                        }
                        // Modify coupons if needed
                        Coupon inCoupon = null;
                        Coupon outCoupon = null;
                        long inID = 0;
                        long outID = 0;
                        bool changeIn = false;
                        bool changeOut = false;
                        if (info.identIn == null)
                        {
                            if ((txtBatchPassIn.Text != "") && (txtBatchPassIn.Text.Length > 0))
                            {
                                // Create the issued coupon
                                inCoupon = brokerDB.CreateCoupon(txtBatchPassIn.Text);
                                // Insert the coupon
                                brokerDB.InsertCoupon(inCoupon);
                                inID = inCoupon.couponId;
                                changeIn = true;
                            }
                        }
                        else
                        {
                            if ((txtBatchPassIn.Text == null) || (txtBatchPassIn.Text.Length < 1))
                            {
                                changeIn = true;
                            }
                            else if (info.identIn.passkey.CompareTo(txtBatchPassIn.Text) != 0)
                            {
                                // Create the issued coupon
                                inCoupon = brokerDB.CreateCoupon(txtBatchPassIn.Text);
                                // Insert the coupon
                                brokerDB.InsertCoupon(inCoupon);
                                inID = inCoupon.couponId;
                                changeIn = true;
                            }
                            else
                            {
                                inID = info.identIn.couponId;
                            }
                        }
                        if (info.identOut == null)
                        {
                            if ((txtBatchPassOut.Text != "") && (txtBatchPassOut.Text.Length > 0))
                            {
                                // Create the issued coupon
                                outCoupon = brokerDB.CreateCoupon(txtBatchPassOut.Text);
                                // Insert the coupon
                                brokerDB.InsertCoupon(outCoupon);
                                outID = outCoupon.couponId;
                                changeOut = true;
                            }
                        }
                        else
                        {
                            if ((txtBatchPassOut.Text == null) || (txtBatchPassOut.Text.Length < 1))
                            {
                                changeOut = true;
                            }
                            else if (info.identOut.passkey.CompareTo(txtBatchPassOut.Text) != 0)
                            {
                                // Create the issued coupon
                                outCoupon = brokerDB.CreateCoupon(txtBatchPassOut.Text);
                                // Insert the coupon
                                brokerDB.InsertCoupon(outCoupon);
                                outID = outCoupon.couponId;
                                changeOut = true;
                            }
                        }
                        
                        if (changeIn ||changeOut)
                        {
                            if ((inID == 0) && (outID == 0))
                            {
                                brokerDB.SetIdentCoupons(info.agentGuid, 0, 0, null);
                            }
                            else
                            {
                                brokerDB.SetIdentCoupons(info.agentGuid, inID, outID, ProcessAgentDB.ServiceGuid);
                            }
                        }
                    }
                    // test for administered Service
                    else if (!txtServiceType.Text.Equals(ProcessAgentType.SERVICE_BROKER) && !txtServiceType.Text.Equals(ProcessAgentType.REMOTE_SERVICE_BROKER))
                    {
                        // Get the current Administer grants
                        int qualifierTypeID = Qualifier.ToTypeID(txtServiceType.Text);
                        int qualifierID = AuthorizationAPI.GetQualifierID(agentId, qualifierTypeID);
                        string grantFunction = GetGrantFunction(txtServiceType.Text);

                        // Get all grants for the function and qualifier ( theService )
                        //Ideally, there should only be one group that has the "Administer" function on a particular Process Agent
                        //for now, take the first element of the array
                        int[] grantIDs = wrapper.FindGrantsWrapper(-1, grantFunction, qualifierID);

                        if (ddlAdminGroup.SelectedIndex > 0)
                        {
                            int groupID = Convert.ToInt32(ddlAdminGroup.SelectedValue);
                            bool found = false;
                            List<int> removeGrants = new List<int>();
                            if (grantIDs != null && grantIDs.Length > 0)
                            {
                                Grant[] grants = wrapper.GetGrantsWrapper(grantIDs);
                                foreach (Grant g in grants)
                                {
                                    if (g.agentID == groupID)
                                    {
                                        found = true;
                                    }
                                    else
                                    {
                                        removeGrants.Add(g.grantID);
                                    }
                                }
                            }
                            if (!found)
                            {
                                wrapper.AddGrantWrapper(groupID, grantFunction, qualifierID);
                            }
                            if (removeGrants.Count > 0)
                            {
                                wrapper.RemoveGrantsWrapper(removeGrants.ToArray());
                            }

                        }
                        else
                        {
                            if (grantIDs != null && grantIDs.Length > 0)
                            {
                                wrapper.RemoveGrantsWrapper(grantIDs);
                            }
                        }
                        if (txtServiceType.Text.Equals(ProcessAgentType.LAB_SERVER))
                        {
                            trAssociate.Visible = true;
                            trManage.Visible = true;
                            CheckAssociatedLSSForLS(agentId);
                        }
                        else
                        {
                            trAssociate.Visible = false;
                            trManage.Visible = false;
                        }
                    }
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Service " + txtServiceName.Text + " has been modified.");

                    // Reload the Lab Server dropdown
                    InitializeDropDown();
                    ddlService.SelectedIndex = savedSelectedIndex;
                }
                catch(Exception ex)
                {
                    Utilities.WriteLog("Modify Service: " + ex.Message);
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("Process Agent " + txtServiceName.Text + " cannot be modified.");
                    return;
                }

            }
            if(agentID > 0){
                displayService(agentID);
            }
             

        }

        /// <summary>
        /// returns an Administrative grant function corresponding to a Process Agent type, 
        /// these values are limited to the requirements of this page.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        private string GetGrantFunction(string serviceType)
        {
            string grantFuntion = null;

            if (serviceType.Equals(ProcessAgentType.SCHEDULING_SERVER))
                grantFuntion = TicketTypes.ADMINISTER_USS;

            else if (serviceType.Equals(ProcessAgentType.LAB_SCHEDULING_SERVER))
                grantFuntion = TicketTypes.ADMINISTER_LSS;

            else if (serviceType.Equals(ProcessAgentType.EXPERIMENT_STORAGE_SERVER))
                grantFuntion = TicketTypes.ADMINISTER_ESS;

            else if (serviceType.Equals(ProcessAgentType.LAB_SERVER))
                grantFuntion = TicketTypes.ADMINISTER_LS;

            return grantFuntion;
        }



        /// <summary>
        /// Generate New Incoming Passkey Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegister_Click(object sender, System.EventArgs e)
        {
            int agentID = 0;
            ProcessAgent service = ProcessAgentDB.ServiceAgent;
            if (service == null)
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = Utilities.FormatErrorMessage("This service broker has not been configured. You must perform selfRegistration before any other services are registered.");
                return;
            }
            

            if (txtServiceType.Text.Equals(ProcessAgentType.BATCH_LAB_SERVER))
            {
                bool hasError = false;
                StringBuilder errorMess = new StringBuilder();
                errorMess.AppendLine("Error: Registering a Batch LabServer you are missing the following fields:");

                if (txtWebServiceURL.Text == null || txtWebServiceURL.Text.Length == 0)
                {
                    hasError = true;
                    errorMess.Append('\t' + "Web Service URL");
                }
                if (txtServiceGUID.Text == null || txtServiceGUID.Text.Length == 0 || txtServiceGUID.Text.Length > 35)
                {
                    hasError = true;
                    errorMess.Append('\t' + "Service GUID: Batch LabServer Guids must be between 1 and 35 characters");
                }
                //if (txtBatchPassIn.Text == null || txtBatchPassIn.Text.Length == 0)
                //{
                //    hasError = true;
                //    errorMess.Append('\t' + "Passcode From LabServer");
                //}
                //if (txtBatchPassOut.Text == null || txtBatchPassOut.Text.Length == 0)
                //{
                //    hasError = true;
                //    errorMess.Append('\t' + "Passcode To LabServer");
                //}
                if (txtApplicationURL.Text == null || txtApplicationURL.Text.Length == 0)
                {
                    hasError = true;
                    errorMess.Append('\t' + "Codebase URL");
                }
                if (txtServiceName.Text == null || txtServiceName.Text.Length == 0)
                {
                    hasError = true;
                    errorMess.Append('\t' + "Service Name");
                }
                if (hasError)
                {
                    errorMess.AppendLine("");
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage(errorMess.ToString());
                    return;
                }
                // Missing fields check passed start to create the BatchLabServer records,
                // hopefully the data is correct,
                // Since InstallDomainCredentials is not supported by the 6.1 Batch labs we must
                // only do one side of the operation.
                // Batch labs have a one to one relationship with service brokers, 
                // this means it must be a member of this domain 
                int tstID = brokerDB.GetProcessAgentID(txtServiceGUID.Text);
                if (tstID > 0)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("A service with the specified Guid has already been registered.");
                    return;
                }
                Coupon inCoupon = null;
                if (txtBatchPassIn.Text != null && txtBatchPassIn.Text.Length > 0)
                {
                    inCoupon = brokerDB.CreateCoupon(txtBatchPassIn.Text);
                }
                Coupon outCoupon = null;
                if (txtBatchPassOut.Text != null && txtBatchPassOut.Text.Length > 0)
                {
                    outCoupon = brokerDB.CreateCoupon(txtBatchPassOut.Text);
                }
                agentID = wrapper.AddProcessAgentWrapper(txtServiceGUID.Text, txtServiceName.Text,
                    ProcessAgentType.BATCH_LAB_SERVER, ProcessAgentDB.ServiceGuid, txtApplicationURL.Text,
                    txtWebServiceURL.Text, inCoupon, outCoupon);

                modifySystemSupport(agentID);
                if (agentID > 0)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Relationship with the service has been created and saved.");
                }
                ClearButtonScripts();

            }

            else
            { // Interactive service
                if ((txtWebServiceURL.Text == "") || (txtOutPassKey.Text == ""))
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("Please specify a Web Service URL and initial passkey before registering the service.");
                    return;
                }

                else
                {

                    try
                    {
                        Coupon inIdentCoupon = brokerDB.CreateCoupon();
                        Coupon outIdentCoupon = brokerDB.CreateCoupon();

                        ProcessAgent agent = null;

                        ProcessAgentProxy proxy = new ProcessAgentProxy();
                        InitAuthHeader data = new InitAuthHeader();
                        proxy.Url = txtWebServiceURL.Text.Trim();

                        data.initPasskey = txtOutPassKey.Text.Trim();
                        proxy.InitAuthHeaderValue = data;

                        agent = proxy.InstallDomainCredentials(service, inIdentCoupon, outIdentCoupon);
                        if (agent != null)
                        {
                            if (agent.type.Equals(ProcessAgentType.SERVICE_BROKER))
                                agent.type = ProcessAgentType.REMOTE_SERVICE_BROKER;
                            agentID = wrapper.AddProcessAgentWrapper(agent.agentGuid, agent.agentName, agent.type,
                                agent.domainGuid, agent.codeBaseUrl, agent.webServiceUrl,
                                outIdentCoupon, inIdentCoupon);

                            // Create the default AdminUrls
                            switch (agent.type)
                            {
                                case ProcessAgentType.EXPERIMENT_STORAGE_SERVER:
                                    //brokerDB.InsertAdminURL(agentId, agent.codeBaseUrl + "/administer.aspx", TicketTypes.ADMINISTER_ESS);
                                    break;
                                case ProcessAgentType.LAB_SCHEDULING_SERVER:
                                    brokerDB.InsertAdminURL(agentID, agent.codeBaseUrl + "/administer.aspx", TicketTypes.ADMINISTER_LSS);
                                    brokerDB.InsertAdminURL(agentID, agent.codeBaseUrl + "/manage.aspx", TicketTypes.MANAGE_LAB);
                                    break;
                                case ProcessAgentType.LAB_SERVER:
                                    brokerDB.InsertAdminURL(agentID, agent.codeBaseUrl + "/administer.aspx", TicketTypes.ADMINISTER_LS);
                                    brokerDB.InsertAdminURL(agentID, agent.codeBaseUrl + "/administer.aspx", TicketTypes.MANAGE_LAB);
                                    break;
                                case ProcessAgentType.SCHEDULING_SERVER:
                                    brokerDB.InsertAdminURL(agentID, agent.codeBaseUrl + "/administer.aspx", TicketTypes.ADMINISTER_USS);
                                    brokerDB.InsertAdminURL(agentID, agent.codeBaseUrl + "/manage.aspx", TicketTypes.MANAGE_USS_GROUP);
                                    brokerDB.InsertAdminURL(agentID, agent.codeBaseUrl + "/Reservation.aspx", TicketTypes.SCHEDULE_SESSION);
                                    break;
                                case ProcessAgentType.REMOTE_SERVICE_BROKER:
                                default:
                                    break;
                            }
                            SystemSupport mySS = brokerDB.RetrieveSystemSupport(ProcessAgentDB.ServiceGuid);
                            if (mySS != null)
                            {
                                ServiceDescription[] values = new ServiceDescription[1];
                                values[0] = new ServiceDescription(mySS.ToXML(), null, "requestSystemSupport");
                                proxy.AgentAuthHeaderValue = new AgentAuthHeader();
                                proxy.AgentAuthHeaderValue.coupon = inIdentCoupon;
                                proxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                                proxy.Register(Utilities.MakeGuid(), values);
                            }
                            lblErrorMessage.Visible = true;
                            lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Relationship with the service has been created and saved.");

                            // set the script of the "Admin URL button"
                            SetBtnScripts(agent.agentGuid,agent.type);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrorMessage.Visible = true;
                        //lblErrorMessage.Text = Utilities.FormatErrorMessage("InstallDomainCredentials Error: " + Utilities.DumpException(ex));
                        lblErrorMessage.Text = Utilities.FormatErrorMessage("InstallDomainCredentials Error: " + ex.Message);
                    }
                }
            }
            InitializeDropDown();
            if (agentID > 0)
            {
                ddlService.Items.FindByValue(agentID.ToString()).Selected = true;
                //SetInputMode(true);
                displayService(agentID);
            }
            else
            {
                if (ddlService.Items.Count > 0)
                {
                    ddlService.Items[0].Selected = true;
                }
            }
        }

     

        protected void ClearButtonScripts()
        {
            btnAdminURLs.Attributes.Add("onClick", "");
        }

        protected void SetBtnScripts(string agentGuid, string type)
        {

            if (type == ProcessAgentType.SERVICE_BROKER
                || type == ProcessAgentType.REMOTE_SERVICE_BROKER)
            {
                btnAdminURLs.Attributes.Add("onClick", "");
                //btnRsrcMappings.Attributes.Add("onClick", "");
                return;
            }

            string script1 = "javascript:window.open('AddAdminURLPopup.aspx?paguid=" + agentGuid + "','adminurls','scrollbars=yes,resizable=yes,width=700,height=500')";
            btnAdminURLs.Attributes.Add("onClick", script1);

            //string script2 = "javascript:window.open('AddResourceMappingPopup.aspx?paguid=" + agentGuid + "','rsrcmappings','scrollbars=yes,resizable=yes,width=700,height=500')";
            //btnRsrcMappings.Attributes.Add("onClick", script2);
        }
      

        protected void btnAssociateLSS_Click(object sender, EventArgs e)
        {
            if(btnAssociateLSS.CommandName.CompareTo("associate") == 0){
           
            try
            {
                if (ddlLSS.SelectedIndex == 0)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("Please select a desired LSS to be associated with the lab server.");
                    return;
                }
                if (ddlManageLSS.SelectedIndex == 0)
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = Utilities.FormatErrorMessage("Please select a group to manage the Lab Server on the lab scheduling server.");
                    return;
                }
                int lsID = Int32.Parse(ddlService.SelectedValue);
                int lssID = Int32.Parse(ddlLSS.SelectedValue);
                int manageGroupID = Int32.Parse(ddlManageLSS.SelectedValue);
                brokerDB.AssociateLSS(lsID,lssID );
                try
                {
                    // This has been moved to ManageServices
                    // This should be only for LS/LSS in the domain
                    // Add LabServer LSS ManageLab Grant 

                    ResourceMappingKey key = new ResourceMappingKey(ResourceMappingTypes.PROCESS_AGENT, lsID);
                    ResourceMappingValue[] values = new ResourceMappingValue[3];
                    values[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.LAB_SCHEDULING_SERVER);
                    values[1] = new ResourceMappingValue(ResourceMappingTypes.PROCESS_AGENT, lssID);
                    values[2] = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                        TicketTypes.GetTicketType(TicketTypes.MANAGE_LAB));
                    List<int> mapIDS = ResourceMapManager.FindMapIds(key, values);
                    if (mapIDS.Count > 0)
                    {
                        
                        int labQualifierID = AuthorizationAPI.GetQualifierID(mapIDS[0], Qualifier.resourceMappingQualifierTypeID);
                        int labGrantID = wrapper.AddGrantWrapper(manageGroupID, Function.manageLAB, labQualifierID);
                    }

                }
                catch(Exception ex)
                {
                    Utilities.WriteLog(ex.Message);
                }
                btnAssociateLSS.Text = "Disassociate";
                btnAssociateLSS.CommandName = "disassociate";
                ddlLSS.Enabled = false;
                ddlLSS.BackColor = disabled;
                ddlManageLSS.Enabled = false;
                ddlManageLSS.BackColor = disabled;
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Lab-side Scheduling Server \"" + ddlLSS.SelectedItem.Text + "\" succesfully "
                    + "associated with lab server \"" + ddlService.SelectedItem.Text + "\".");

            }
            catch
            {
                throw;
            }
            }
            else if(btnAssociateLSS.CommandName.CompareTo("disassociate") == 0){
                try
            {
                brokerDB.DeleteResourceMapping( (int) Session["LS_LSSmapID"]);
                Session.Remove("LS_LSSmapID");

                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = Utilities.FormatConfirmationMessage("Lab-side Scheduling Server \"" + ddlLSS.SelectedItem.Text + "\" succesfully "
                    + "dissociated from lab server \"" + ddlService.SelectedItem.Text + "\".");

                ddlLSS.Enabled = true;
                ddlLSS.BackColor = enabled;
                ddlLSS.SelectedIndex = 0;
                ddlManageLSS.SelectedIndex = 0;
                ddlManageLSS.Enabled = true;
                ddlManageLSS.BackColor = enabled;
                btnAssociateLSS.CommandName = "associate";
                btnAssociateLSS.Text = "Associate";
            }

            catch
            {
                throw;
            }
            }
        }

        protected void cbxDoBatch_Changed(object sender, EventArgs e)
        {
            ddlService.SelectedIndex = 0;
            if (cbxDoBatch.Checked)
            {
                ClearFormFields();
                SetBatchInputMode(false);
                trBatchIn.Visible = true;
                trBatchOut.Visible = true;
                txtBatchHelp.Visible = true;
            }
            else
            {
                ClearFormFields();
                SetInputMode(false);
                trBatchIn.Visible = false;
                trBatchOut.Visible = false;
                 txtBatchHelp.Visible = false;
            }
            

        }

    }
}
