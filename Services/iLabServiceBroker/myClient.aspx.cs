/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id: myClient.aspx.cs,v 1.31 2008/03/17 21:22:06 pbailey Exp $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Xml;

using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.UtilLib;


using iLabs.Ticketing;

//using iLabs.Services;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Proxies.USS;

namespace iLabs.ServiceBroker.iLabSB
{
    /// <summary>
    /// iLab Client Launch Page.
    /// </summary>
    public partial class myClient : System.Web.UI.Page
    {
        //protected System.Web.UI.WebControls.Label lblDebug;
        protected System.Web.UI.HtmlControls.HtmlAnchor urlEmail;
        protected System.Web.UI.HtmlControls.HtmlAnchor urlDocumentation;

        protected LabClient lc;

        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

        public string couponId = null;
        public string passkey = null;
        public string issuerGuid = null;
        public string auto = null;

        BrokerDB issuer = new BrokerDB();

        DateTime startExecution;
        long duration = -1;
        bool autoLaunch = false;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            int groupID = 0;
            string groupName = null;
            lc = wrapper.GetLabClientsWrapper(new int[] { Convert.ToInt32(Session["ClientID"]) })[0];

            if (Session["GroupID"] != null && Session["GroupID"].ToString().Length > 0)
            {
                groupID = Convert.ToInt32(Session["GroupID"]);
            }
            if (Session["GroupName"] != null && Session["GroupName"].ToString().Length > 0)
            {
                groupName = Session["GroupName"].ToString();

                lblGroupNameTitle.Text = groupName;
                lblBackToLabs.Text = groupName;

                if (Convert.ToInt32(Session["ClientCount"]) == 1)
                    lblGroupNameSystemMessage.Text = "Messages for " + groupName;
                else
                    lblGroupNameSystemMessage.Text = "Messages for " + lc.clientName;
            }

            if (!IsPostBack)
            {
                auto = Request.QueryString["auto"];
                if (auto!= null && auto.Length > 0)
                {
                    if (auto.ToLower().Contains("t"))
                    {
                        autoLaunch = true;
                    }
                }
                if (lc.clientType == LabClient.INTERACTIVE_APPLET || lc.clientType == LabClient.INTERACTIVE_HTML_REDIRECT)
                {
                    // retrieve parameters from URL
                    couponId = Request.QueryString["coupon_id"];
                    passkey = Request.QueryString["passkey"];
                    issuerGuid = Request.QueryString["issuer_guid"];

                    if (lc.needsScheduling)
                    {
                        Coupon opCoupon = null;
                        if (couponId != null && passkey != null && issuerGuid != null)
                        {
                            opCoupon = new Coupon(issuerGuid, Int64.Parse(couponId), passkey);

                            // First check for an Allow Execution Ticket
                            Ticket allowExperimentExecutionTicket = issuer.RetrieveTicket(
                                opCoupon, TicketTypes.ALLOW_EXPERIMENT_EXECUTION);
                            if (allowExperimentExecutionTicket == null)
                            {
                                // Try for a reservation

                                int ussId = issuer.FindProcessAgentIdForClient(lc.clientID, ProcessAgentType.SCHEDULING_SERVER);
                                if (ussId > 0)
                                {
                                    ProcessAgent uss = issuer.GetProcessAgent(ussId);
                                    ProcessAgent ls = issuer.GetProcessAgent(lc.labServerIDs[0]);

                                    UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                                    OperationAuthHeader op = new OperationAuthHeader();
                                    op.coupon = opCoupon;
                                    ussProxy.Url = uss.webServiceUrl;
                                    ussProxy.OperationAuthHeaderValue = op;
                                    Reservation reservation = ussProxy.RedeemReservation(ProcessAgentDB.ServiceGuid, Session["UserName"].ToString(), ls.agentGuid, lc.clientGuid);

                                    if (reservation != null)
                                    {
                                        // Find efective group
                                        string effectiveGroupName = null;
                                        int effectiveGroupID = AuthorizationAPI.GetEffectiveGroupID(groupID, lc.clientID,
                                            Qualifier.labClientQualifierTypeID, Function.useLabClientFunctionType);
                                        if (effectiveGroupID == groupID)
                                        {
                                            if (Session["groupName"] != null)
                                            {
                                                effectiveGroupName = Session["groupName"].ToString();
                                            }
                                            else
                                            {
                                                effectiveGroupName = AdministrativeAPI.GetGroupName(groupID);
                                                Session["groupName"] = effectiveGroupName;
                                            }
                                        }
                                        else if (effectiveGroupID > 0)
                                        {
                                            effectiveGroupName = AdministrativeAPI.GetGroupName(effectiveGroupID);
                                        }
                                        // create the allowExecution Ticket
                                        DateTime start = reservation.Start;
                                        long duration = reservation.Duration;
                                        string payload = TicketLoadFactory.Instance().createAllowExperimentExecutionPayload(
                                            start, duration, effectiveGroupName);
                                        DateTime tmpTime = start.AddTicks(duration * TimeSpan.TicksPerSecond);
                                        DateTime utcNow = DateTime.UtcNow;
                                        long ticketDuration = (tmpTime.Ticks - utcNow.Ticks) / TimeSpan.TicksPerSecond;
                                        allowExperimentExecutionTicket = issuer.AddTicket(opCoupon, TicketTypes.ALLOW_EXPERIMENT_EXECUTION,
                                                ProcessAgentDB.ServiceGuid, ProcessAgentDB.ServiceGuid, ticketDuration, payload);
                                    }
                                }
                            }
                            if (allowExperimentExecutionTicket != null)
                            {
                                XmlDocument payload = new XmlDocument();
                                payload.LoadXml(allowExperimentExecutionTicket.payload);
                                startExecution = DateUtil.ParseUtc(payload.GetElementsByTagName("startExecution")[0].InnerText);
                                duration = Int64.Parse(payload.GetElementsByTagName("duration")[0].InnerText);

                                Session["StartExecution"] = DateUtil.ToUtcString(startExecution);
                                Session["Duration"] = duration;

                                //groupId = payload.GetElementsByTagName("groupID")[0].InnerText;

                                
                                // Display reenter button if experiment is reentrant & a current experiment exists
                                if (lc.IsReentrant)
                                {

                                    long[] ids = InternalDataDB.RetrieveActiveExperimentIDs(Convert.ToInt32(Session["UserID"]),
                                        Convert.ToInt32(Session["GroupID"]), lc.labServerIDs[0], lc.clientID);
                                    if (ids.Length > 0)
                                    {
                                        btnLaunchLab.Text = "Launch New Experiment";
                                        btnLaunchLab.Visible = true;
                                        pReenter.Visible = true;
                                        btnReenter.Visible = true;
                                        btnReenter.CommandArgument = ids[0].ToString();
                                    }
                                    else
                                    {

                                        pReenter.Visible = false;
                                        btnReenter.Visible = false;
                                        btnLaunchLab.Text = "Launch Lab";
                                        if (autoLaunch)
                                        {
                                            launchLabClient(lc.clientID);
                                        }
                                        else
                                        {
                                            btnLaunchLab.Visible = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (autoLaunch)
                                    {
                                        launchLabClient(lc.clientID);
                                    }
                                    else
                                    {
                                        btnLaunchLab.Visible = true;
                                    }
                                }
                            }
                            else
                            {
                                btnLaunchLab.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (autoLaunch)
                        {
                            launchLabClient(lc.clientID);
                        }
                        else
                        {
                            btnLaunchLab.Visible = true;
                        }
                    }
                }
                else if (lc.clientType == LabClient.BATCH_APPLET || lc.clientType == LabClient.BATCH_HTML_REDIRECT)
                {
                    if (autoLaunch)
                    {
                        launchLabClient(lc.clientID);
                    }
                    else
                    {
                        btnLaunchLab.Visible = true;
                    }
                }
            }





            btnSchedule.Visible = lc.needsScheduling;
            //Session["LoaderScript"] = lc.loaderScript;
            lblClientName.Text = lc.clientName;
            lblVersion.Text = lc.version;
            lblLongDescription.Text = lc.clientLongDescription;
            lblNotes.Text = lc.notes;
            string emailCmd = "mailto:" + lc.contactEmail;
            lblEmail.Text = "<a href=" + emailCmd + ">" + lc.contactEmail + "</a>";

            btnLaunchLab.Command += new CommandEventHandler(this.btnLaunchLab_Click);
            btnLaunchLab.CommandArgument = lc.clientID.ToString();

            int count = 0;

            if (lc.clientInfos != null)
            {
                foreach (ClientInfo ci in lc.clientInfos)
                {
                    if (ci.infoURLName.CompareTo("Documentation") != 0)
                    {
                        System.Web.UI.WebControls.Button b = new System.Web.UI.WebControls.Button();
                        b.Visible = true;
                        b.CssClass = "button";
                        b.Text = ci.infoURLName;
                        b.CommandArgument = ci.infoURL;
                        b.CommandName = ci.infoURLName;
                        b.ToolTip = ci.description;
                        b.Command += new CommandEventHandler(this.HelpButton_Click);
                        repClientInfos.Controls.AddAt(count, b);
                        repClientInfos.Controls.AddAt(count + 1, new LiteralControl("&nbsp;&nbsp;"));
                        count += 2;
                    }
                }
            }

            List<SystemMessage> messagesList = new List<SystemMessage>();
            SystemMessage[] groupMessages = null;
            if (Session["ClientCount"] != null && Convert.ToInt32(Session["ClientCount"]) == 1)
            {
                groupMessages = wrapper.GetSystemMessagesWrapper(SystemMessage.GROUP, Convert.ToInt32(Session["GroupID"]), 0, 0);
                if (groupMessages != null)
                    messagesList.AddRange(groupMessages);
            }

            foreach (int labServerID in lc.labServerIDs)
            {
                SystemMessage[] labMessages = wrapper.GetSystemMessagesWrapper(SystemMessage.LAB, 0, 0, labServerID);
                if (labMessages != null)
                    messagesList.AddRange(labMessages);
            }


            if (messagesList != null && messagesList.Count > 0)
            {
                messagesList.Sort(SystemMessage.CompareDateDesc);
                //messagesList.Reverse();
                repSystemMessage.DataSource = messagesList;
                repSystemMessage.DataBind();
            }

            else
            {

                lblGroupNameSystemMessage.Text += "</h3><p>No Messages at this time</p><h3>";
               
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


        private void urlEmail_Click(object sender, System.EventArgs e)
        {
            string email = wrapper.GetLabClientsWrapper(new int[] { Convert.ToInt32(Session["ClientID"]) })[0].contactEmail;
            Response.Redirect("emailTo:" + email);
        }

        protected void HelpButton_Click(object sender, CommandEventArgs e)
        {
            string jScript = @"<script language='javascript'> window.open ('" + e.CommandArgument.ToString() + "')</script>";
            Page.RegisterStartupScript("DocsPopup", jScript);
        }

        protected void urlDocumentation_Click(object sender, System.EventArgs e)
        {

            LabClient lc = wrapper.GetLabClientsWrapper(new int[] { Convert.ToInt32(Session["ClientID"]) })[0];
            string docURL = null;
            if (lc.clientInfos != null)
            {
                foreach (ClientInfo ci in lc.clientInfos)
                {
                    if (ci.infoURLName.CompareTo("Documentation") == 0)
                    {
                        docURL = ci.infoURL;
                        break;
                    }
                }
            }
            string jScript = @"<script language='javascript'> window.open ('" + docURL + "')</script>";
            Page.RegisterStartupScript("DocsPopup", jScript);
        }
        private void launchLabClient(int c_id)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("login.aspx");
            }

            StringBuilder message = new StringBuilder("Message: clientID = ");
            int[] labIds = new int[1];
            message.Append(btnLaunchLab.CommandArgument + " ");
            labIds[0] = c_id;
            LabClient[] clients = AdministrativeAPI.GetLabClients(labIds);
            if (clients.Length > 0)
            {
                if (clients[0].clientType == LabClient.INTERACTIVE_HTML_REDIRECT)
                {
                    // [GeneralTicketing] get lab servers metadata from lab server ids
                    ProcessAgentInfo[] labServers = issuer.GetProcessAgentInfos(clients[0].labServerIDs);
                    if (labServers != null)
                    {
                        message.Append(" LabServer count: " + labServers.Length);
                        if (labServers.Length > 0)
                        {
                            // execute the "experiment execution recipe
                            RecipeExecutor executor = RecipeExecutor.Instance();
                            string redirectURL = null;
                            DateTime start = DateTime.UtcNow;
                            long duration = -1L; // default is never timeout

                            //Check for Scheduling: 
                            //The scheduling Ticket should exist and been parsed into the session
                            if (lc.needsScheduling)
                            {
                                start = DateUtil.ParseUtc(Session["StartExecution"].ToString());
                                duration = Convert.ToInt64(Session["Duration"]);
                            }

                            redirectURL = executor.ExecuteExperimentExecutionRecipe(labServers[0], clients[0],
                            start, duration, Convert.ToInt32(Session["UserTZ"]), Convert.ToInt32(Session["UserID"]),
                            Convert.ToInt32(Session["GroupID"]), (string)Session["GroupName"]);

                            // Add the return url to the redirect
                            redirectURL += "&sb_url=" + Utilities.ExportUrlPath(Request.Url);

                            // Now open the lab within the current Window/frame
                            Response.Redirect(redirectURL, true);
                        }
                    }
                }
                else if (clients[0].clientType == LabClient.INTERACTIVE_APPLET)
                {
                    // Note: Currently not supporting Interactive applets
                    // use the Loader script for Batch experiments

                    Session["LoaderScript"] = clients[0].loaderScript;
                    Session.Remove("RedirectURL");

                    string jScript = @"<script language='javascript'>parent.theapplet.location.href = '"
                        + "applet.aspx" + @"'</script>";
                    Page.RegisterStartupScript("ReloadFrame", jScript);
                }

                // Support for Batch 6.1 Lab Clients
                else if (clients[0].clientType == LabClient.BATCH_HTML_REDIRECT)
                {
                    Session["ClientID"] = clients[0].clientID;
                    AdministrativeAPI.SetSessionClient(Convert.ToInt64(Session["SessionID"]), clients[0].clientID);
                    // use the Loader script for Batch experiments

                    //use ticketing & redirect to url in loader script

                    // [GeneralTicketing] retrieve static process agent corresponding to the first
                    // association lab server */


                    // New comments: The HTML Client is not a static process agent, so we don't search for that at the moment.
                    // Presumably when the interactive SB is merged with the batched, this should check for a static process agent.
                    // - CV, 7/22/05
                    {
                        Session.Remove("LoaderScript");

                        //payload includes username and effective group name & client id.
                        //ideally this should be encoded in xml  - CV, 7/27/2005
                        TicketLoadFactory factory = TicketLoadFactory.Instance();
                        string userName = (string)Session["UserName"];
                        string groupName = (string)Session["GroupName"];

                        string sessionPayload = factory.createRedeemSessionPayload(Convert.ToInt32(Session["UserID"]),
                           Convert.ToInt32(Session["GroupID"]), Convert.ToInt32(Session["ClientID"]), userName, groupName);
                        // SB is the redeemer, ticket type : session_identifcation, no expiration time, payload,SB as sponsor ID, redeemer(SB) coupon
                        Coupon coupon = issuer.CreateTicket(TicketTypes.REDEEM_SESSION, ProcessAgentDB.ServiceGuid,
                             ProcessAgentDB.ServiceGuid, -1, sessionPayload);

                        string jScript = @"<script language='javascript'> window.open ('" + lc.loaderScript + "?couponID=" + coupon.couponId + "&passkey=" + coupon.passkey + "')</script>";
                        Page.RegisterStartupScript("HTML Client", jScript);
                    }
                }
                // use the Loader script for Batch experiments
                else if (clients[0].clientType == LabClient.BATCH_APPLET)
                {
                    Session["ClientID"] = clients[0].clientID;
                    AdministrativeAPI.SetSessionClient(Convert.ToInt64(Session["SessionID"]), clients[0].clientID);
                    Session["LoaderScript"] = clients[0].loaderScript;
                    Session.Remove("RedirectURL");

                    string jScript = @"<script language='javascript'>parent.theapplet.location.href = '"
                        + ProcessAgentDB.ServiceAgent.codeBaseUrl + @"/applet.aspx" + @"'</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadFrame", jScript);
                    //Page.RegisterStartupScript("ReloadFrame", jScript);
                }
            }
            else
            {
                message.Append(" LabServer = null");
            }
            //lblDebug.Text = message.ToString();
            Utilities.WriteLog(message.ToString());
        }

        protected void btnLaunchLab_Click(object sender, System.EventArgs e)
        {
            launchLabClient(Convert.ToInt32(btnLaunchLab.CommandArgument));
        }

        private void reenterLabClient()
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("login.aspx");
            }
            BrokerDB brokerDB = new BrokerDB();
            StringBuilder message = new StringBuilder("Message: clientID = ");
            int[] labIds = new int[1];
            message.Append(btnReenter.CommandArgument + " ");
            long expid = Convert.ToInt64(btnReenter.CommandArgument);
            LabClient client = AdministrativeAPI.GetLabClient(Convert.ToInt32(Session["ClientID"]));
            if (client.clientID > 0)
            {
                if (client.clientType == LabClient.INTERACTIVE_HTML_REDIRECT)
                {
                    long[] coupIDs = InternalDataDB.RetrieveExperimentCouponIDs(expid);
                    Coupon coupon = brokerDB.GetIssuedCoupon(coupIDs[0]);
                    // construct the redirect query
                    StringBuilder url = new StringBuilder(client.loaderScript.Trim());
                    if (url.ToString().IndexOf("?") == -1)
                        url.Append('?');
                    else
                        url.Append('&');
                    url.Append("coupon_id=" + coupon.couponId + "&passkey=" + coupon.passkey
                        + "&issuer_guid=" + brokerDB.GetIssuerGuid());

                    // Add the return url to the redirect
                    url.Append("&sb_url=");
                    url.Append(Utilities.ExportUrlPath(Request.Url));

                    // Now open the lab within the current Window/frame
                    Response.Redirect(url.ToString(), true);
                }
            }
        }

        protected void btnReenter_Click(object sender, System.EventArgs e)
        {
            reenterLabClient();
        }

        private void doScheduling()
        {
            string username = Session["UserName"].ToString();
            string groupName = Session["GroupName"].ToString();
            string labClientName = lc.clientName;
            string labClientVersion = lc.version;

            ProcessAgent labServer = issuer.GetProcessAgent(lc.labServerIDs[0]);
            int ussId = issuer.FindProcessAgentIdForClient(lc.clientID, ProcessAgentType.SCHEDULING_SERVER);

            if (ussId > 0)
            {

                string ussGuid = issuer.GetProcessAgent(ussId).agentGuid;
                int lssId = issuer.FindProcessAgentIdForAgent(lc.labServerIDs[0], ProcessAgentType.LAB_SCHEDULING_SERVER);

                string lssGuid = issuer.GetProcessAgent(lssId).agentGuid;

                // Find efective group
                string effectiveGroupName = null;
                int currentGroup = Convert.ToInt32(Session["GroupID"]);
                int effectiveGroupID = AuthorizationAPI.GetEffectiveGroupID(currentGroup, lc.clientID,
                    Qualifier.labClientQualifierTypeID, Function.useLabClientFunctionType);
                if (effectiveGroupID == currentGroup)
                {
                    effectiveGroupName = Session["GroupName"].ToString();
                }
                else if (effectiveGroupID > 0)
                {
                    Group[] effGroup = AdministrativeAPI.GetGroups(new int[] { effectiveGroupID });
                    effectiveGroupName = effGroup[0].groupName;
                }

                //Default duration ????
                long duration = 36000;

                RecipeExecutor recipeExec = RecipeExecutor.Instance();
                string schedulingUrl = recipeExec.ExecuteExerimentSchedulingRecipe(ussGuid, lssGuid, username, effectiveGroupName,
                    labServer.agentGuid, lc.clientGuid, labClientName, labClientVersion,
                    Convert.ToInt64(ConfigurationSettings.AppSettings["scheduleSessionTicketDuration"]), Convert.ToInt32(Session["UserTZ"]));

                schedulingUrl += "&sb_url=" + Utilities.ExportUrlPath(Request.Url);
                Response.Redirect(schedulingUrl, false);
            }
        }

        protected void btnSchedule_Click(object sender, EventArgs e)
        {
            doScheduling();
        }

    }
}
