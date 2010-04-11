/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 * 
 * $Id$
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
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authentication;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
using iLabs.Ticketing;
using iLabs.UtilLib;

using iLabs.Proxies.USS;

using iLabs.Ticketing;
using iLabs.DataTypes.TicketingTypes;

namespace iLabs.ServiceBroker.iLabSB
{
    /// <summary>
    /// ssoAuth - Single Sign On Authorization, currently this is a work around since there are no supported 3rd party authorization services.
    /// Query properties: auth - authorization guid, key - optional key, usr - userName, grp - groupName, cid - clientGUID
    /// On initial call each query property is checked for,
    /// If the call is authorized try & retrieve the session either from existing session state of via the session cookie.
    /// If needed display page with login fields; process to create authorization and session cookies and capture users TZ.
    /// Depending on the properties specified and session state make  best-case selection of the action to take.
    /// If have user, group & client try & launch client
    /// if hae user & client try & resolve group, then launch client.
    /// If group &/or client can not be resolved go to appropriate page.
    /// </summary>
    /// 
    public partial class ssoAuth : System.Web.UI.Page
    {
        AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
        string supportMailAddress = ConfigurationSettings.AppSettings["supportMailAddress"];
        object uTZ = null;

        BrokerDB issuer = new BrokerDB();

        //DateTime startExecution;
        //long duration = -1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessages.Text = "";
                lblMessages.Visible = false;
                hdnAuthority.Value = Request.QueryString["auth"];
                hdnKey.Value = Request.QueryString["key"];
                hdnUser.Value = Request.QueryString["usr"];
                hdnGroup.Value = Request.QueryString["grp"];
                if (hdnGroup.Value != null && hdnGroup.Value.Length > 0)
                {
                    int g_id = AdministrativeAPI.GetGroupID(hdnGroup.Value);
                    if( g_id <= 0){
                        lblMessages.Text = "The specified group does not exist!";
                        lblMessages.Visible = true;
                        return;
                    }
                }
                hdnClient.Value = Request.QueryString["cid"];
                if (hdnClient.Value != null && hdnClient.Value.Length > 0)
                {
                     int c_id = AdministrativeAPI.GetLabClientID(hdnClient.Value);
                    if( c_id <= 0){
                        lblMessages.Text = "The specified client does not exist!";
                        lblMessages.Visible = true;
                        return;
                    }
                }
               
            }
            //Check if user specified and matches logged in user
            if (hdnUser.Value != null && hdnUser.Value.Length > 0)
            {
                // Check that the specified user & current user match
                if (Session["UserName"] != null && (Session["UserName"].ToString().Length > 0))
                {
                    if (hdnUser.Value.ToLower().CompareTo(Session["UserName"].ToString().ToLower()) != 0)
                    {
                        lblMessages.Visible = true;
                        lblMessages.Text = "You are currently logged in as a different user than the specified user. Please logout and login as " + hdnUser.Value;
                    }
                }
            }
           
            if (Session["UserName"] == null)
            {
                if (Request.IsAuthenticated)
                {
                    // Get Session info
                    // this needs work
                    SessionInfo sessionInfo = null;
                    divLogin.Visible = false;
                    HttpCookie cookie = Request.Cookies.Get(ConfigurationManager.AppSettings["isbAuthCookieName"]);
                    if (cookie != null)
                    {
                        long sessionid = Convert.ToInt64(cookie.Value);
                        sessionInfo = AdministrativeAPI.GetSessionInfo(sessionid);
                        if (hdnUser.Value != null && hdnUser.Value.Length > 0)
                        {
                            // Check that the specified user & current user match
                            if (sessionInfo.userName != null && (sessionInfo.userName.Length > 0))
                            {
                                if (hdnUser.Value.ToLower().CompareTo(sessionInfo.userName.ToLower()) != 0)
                                {
                                    lblMessages.Visible = true;
                                    lblMessages.Text = "You are currently logged in as a different user than the specified user. Please logout and login as " + hdnUser.Value;
                                    return;
                                }
                            }
                        }
                        Session["UserID"] = sessionInfo.userID;
                        Session["UserName"] = sessionInfo.userName;
                        Session["GroupName"] = sessionInfo.groupName;
                        Session["GroupID"] = sessionInfo.groupID;
                        Session["UserTZ"] = sessionInfo.tzOffset;

                    }
                }
                else
                { // Find out who you are, no authority means use local login
                    if (hdnAuthority.Value == null || hdnAuthority.Value.Length == 0)
                    {
                        // SB is Authority
                        divLogin.Visible = true;
                    }
                    else
                    {
                        // Use 3rd party Auth Service - not implmented
                        divLogin.Visible = true;
                    }
                }
            }
            else // We have a user Session
            {
                ResolveAction();
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
            //Page.ClientScript.RegisterStartupScript
        }
        #endregion

        private void logout()
        {
            AdministrativeAPI.SaveUserSessionEndTime(Convert.ToInt64(Session["SessionID"]));
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
        }

        /// <summary>
        /// This examines the specified parameters and current session state to resove the next action.
        /// This may only be reached after a user is Authenticated.
        /// </summary>
        private void  ResolveAction(){
                int user_ID = 0;
                int client_ID = 0;
                int group_ID = 0;
                string client_Guid = null;
                string group_Name = null;
                string user_Name = null;
                StringBuilder buf = new StringBuilder();
                Session["IsAdmin"] = false;
                Session["IsServiceAdmin"] = false;
                lblMessages.Visible = false;
                lblMessages.Text = "";

                if (hdnUser.Value != null && hdnUser.Value.Length > 0)
                {
                    // Check that the specified user & current user match
                    if (hdnUser.Value.ToLower().CompareTo(Session["UserName"].ToString().ToLower()) == 0)
                    {
                        user_Name = hdnUser.Value;
                        user_ID = AdministrativeAPI.GetUserID(user_Name);
                    }
                    else
                    {
                        //logout();
                        lblMessages.Visible = true;
                        lblMessages.Text = "You are not the user that was specified!";
                        return;
                    }
                }
                else // User is current user
                {
                    user_Name = Session["UserName"].ToString();
                    user_ID = Convert.ToInt32(Session["UserID"]);
                }

                //Get Client_ID
                if (hdnClient.Value != null && hdnClient.Value.Length > 0)
                {
                    client_ID = AdministrativeAPI.GetLabClientID(hdnClient.Value);
                    //Session["clientID"] = client_ID;
                }

                //{ // Note: The existing session client should not be concidered?
                
                if (hdnGroup.Value != null && hdnGroup.Value.Length > 0)
                {
                    group_Name = hdnGroup.Value;
                }
                
                // Check that the user & is a member of the group
                if (group_Name != null)
                {
                    int gid = AdministrativeAPI.GetGroupID(group_Name);
                    if (gid > 0)
                    {
                        if (AdministrativeAPI.IsAgentMember(user_ID, gid))
                        {
                            group_ID = gid;
                            //Session["GroupID"] = group_ID;
                            //Session["GroupName"] = group_Name;
                        }
                        else
                        {
                            // user is not a member of the group
                            group_ID = -1;
                            group_Name = null;
                            
                        }
                    }
                }

                // Session and parameters are parsed, do we have enough info to launch
                int[] clientGroupIDs = null;
                int[] userGroupIDs = null;

                // Try and resolve any unspecified parameters
                if (client_ID <= 0 && group_ID <= 0)
                {
                    userGroupIDs = AdministrativeAPI.ListGroupsForAgentRecursively(user_ID);
                    Group[] groups = AdministrativeAPI.GetGroups(userGroupIDs);
                    Dictionary<int, int[]> clientMap = new Dictionary<int, int[]>();
                    foreach (Group g in groups)
                    {
                        if ((g.groupType.CompareTo(GroupType.REGULAR) == 0) && (g.groupName.CompareTo("ROOT") != 0)
                            && (g.groupName.CompareTo("NewUserGroup") != 0) && (g.groupName.CompareTo("OrphanedUserGroup") != 0)
                             && (g.groupName.CompareTo("SuperUserGroup") != 0))
                        {
                            int[] clientIDs = AdministrativeUtilities.GetGroupLabClients(g.groupID);
                            if (clientIDs != null & clientIDs.Length > 0)
                            {
                                clientMap.Add(g.groupID, clientIDs);
                            }
                        }
                    }
                    if (clientMap.Count > 1) //more than one group with clients
                    {
                        modifyUserSession(group_ID, client_ID);
                        Response.Redirect(Global.FormatRegularURL(Request, "myGroups.aspx"),true);
                    }
                    if (clientMap.Count == 1) // get the group with clients
                    {
                        Dictionary<int, int[]>.Enumerator en = clientMap.GetEnumerator();
                        int gid = -1;
                        int[] clients = null;
                        while (en.MoveNext())
                        {
                            gid = en.Current.Key;
                            clients = en.Current.Value;
                        }
                        if (AdministrativeAPI.IsAgentMember(user_ID, gid))
                        {
                            group_ID = gid;
                            group_Name = AdministrativeAPI.GetGroupName(gid);
                            

                            if (clients == null || clients.Length > 1)
                            {
                               modifyUserSession(group_ID, client_ID);
                                Response.Redirect(Global.FormatRegularURL(Request, "myLabs.aspx"),true);
                            }
                            else
                            {
                                client_ID = clients[0];
                            }
                        }
                    }
                }
            
                else if (client_ID > 0 && group_ID <= 0)
                {
                    int gid = -1;
                    clientGroupIDs = AdministrativeUtilities.GetLabClientGroups(client_ID);
                    if (clientGroupIDs == null || clientGroupIDs.Length == 0)
                    {
                      modifyUserSession( group_ID, client_ID);
                        Response.Redirect(Global.FormatRegularURL(Request, "myGroups.aspx"),true);
                    }
                    else if (clientGroupIDs.Length == 1)
                    {
                        gid = clientGroupIDs[0];
                    }
                    else
                    {
                        userGroupIDs = AdministrativeAPI.ListGroupsForAgentRecursively(user_ID);
                        int count = 0;
                        foreach (int ci in clientGroupIDs)
                        {
                            foreach (int ui in userGroupIDs)
                            {
                                if (ci == ui)
                                {
                                    count++;
                                    gid = ui;
                                }
                            }
                        }
                        if (count != 1)
                        {
                            gid = -1;
                        }
                    }
                    if (gid > 0 && AdministrativeAPI.IsAgentMember(user_ID, gid))
                    {
                        group_ID = gid;
                       
                    }
                    else
                    {
                        modifyUserSession( group_ID, client_ID);
                    }
                }
                else if (client_ID <= 0 && group_ID > 0)
                {
                    int[] clients = AdministrativeUtilities.GetGroupLabClients(group_ID);
                    if (clients == null || clients.Length != 1)
                    {
                        modifyUserSession(group_ID, client_ID);
                        Response.Redirect(Global.FormatRegularURL(Request, "myLabs.aspx"),true);
                    }
                    else
                    {
                        client_ID = clients[0];
                    }
                }
                if (user_ID > 0 && group_ID > 0 && client_ID > 0)
                {
                    int gid = -1;
                    clientGroupIDs = AdministrativeUtilities.GetLabClientGroups(client_ID);
                    foreach (int g_id in clientGroupIDs)
                    {
                        if (g_id == group_ID)
                        {
                            gid = g_id;
                            break;
                        }
                    }
                    if (gid == -1)
                    {
                        buf.Append("The specified group does not have permission to to run the specified client!");
                        lblMessages.Visible = true;
                        lblMessages.Text = Utilities.FormatErrorMessage(buf.ToString());
                        return;
                    }
                    if (!AdministrativeAPI.IsAgentMember(user_ID, group_ID))
                    {
                        buf.Append("You do not have permission to to run the specified client!");
                        lblMessages.Visible = true;
                        lblMessages.Text = Utilities.FormatErrorMessage(buf.ToString());
                        return;
                    }

                    // is authorized ?
                    
                    modifyUserSession(group_ID, client_ID);
                    launchLab(user_ID, group_ID, client_ID);
                   
                }
            }


        protected void launchLab(int userID, int groupID, int clientID)
        {
            // Currently there is not a good solution for checking for an AllowExperiment ticket, will check the USS for reservation
            StringBuilder buf = new StringBuilder("~/myClient.aspx?auto=t");
          
            string userName = null;
            Coupon opCoupon = null;
            Ticket allowTicket = null;
            int effectiveGroupID = 0;
            if (Session["UserName"] != null && Session["UserName"].ToString().Length > 0)
            {
                userName = Session["UserName"].ToString();
            }
            else
            {
                userName = AdministrativeAPI.GetUserName(userID);
            }

              LabClient client = AdministrativeAPI.GetLabClient(clientID);
              if (client.clientID > 0) // It's a structure need to test for valid value
              {
                    DateTime start = DateTime.UtcNow;
                    long duration = 36000L; // default is ten hours
                    ProcessAgentInfo labServer = null;

                    if (client.labServerIDs.Length > 0)
                    {
                        labServer = issuer.GetProcessAgentInfo(client.labServerIDs[0]);
                    }
                    else
                    {
                        throw new Exception("The lab server is not specified for lab client " + client.clientName + " version: " + client.version);
                    }
                    // Find efective group
                    string effectiveGroupName = null;
                    effectiveGroupID = AuthorizationAPI.GetEffectiveGroupID(groupID, clientID,
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

                    //Check for Scheduling: 
                    if (client.needsScheduling)
                    {
                        int ussId = issuer.FindProcessAgentIdForClient(client.clientID, ProcessAgentType.SCHEDULING_SERVER);
                        if (ussId > 0)
                        {
                            ProcessAgent uss = issuer.GetProcessAgent(ussId);

                            int lssId = issuer.FindProcessAgentIdForAgent(client.labServerIDs[0], ProcessAgentType.LAB_SCHEDULING_SERVER);
                            ProcessAgent lss = issuer.GetProcessAgent(lssId);

                            // check for current reservation

                            //create a collection & redeemTicket
                            string redeemPayload = TicketLoadFactory.Instance().createRedeemReservationPayload(DateTime.UtcNow, DateTime.UtcNow);
                            opCoupon = issuer.CreateTicket(TicketTypes.REDEEM_RESERVATION, uss.agentGuid, ProcessAgentDB.ServiceGuid, 600, redeemPayload);

                            UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                            OperationAuthHeader op = new OperationAuthHeader();
                            op.coupon = opCoupon;
                            ussProxy.Url = uss.webServiceUrl;
                            ussProxy.OperationAuthHeaderValue = op;
                            Reservation reservation = ussProxy.RedeemReservation(ProcessAgentDB.ServiceGuid, userName, labServer.agentGuid, client.clientGuid);

                            if (reservation != null)
                            {
                                // create the allowExecution Ticket
                                start = reservation.Start;
                                duration = reservation.Duration;
                                string payload = TicketLoadFactory.Instance().createAllowExperimentExecutionPayload(
                                    start, duration, effectiveGroupName);
                                DateTime tmpTime = start.AddTicks(duration * TimeSpan.TicksPerSecond);
                                DateTime utcNow = DateTime.UtcNow;
                                long ticketDuration = (tmpTime.Ticks - utcNow.Ticks) / TimeSpan.TicksPerSecond;
                                allowTicket = issuer.AddTicket(opCoupon, TicketTypes.ALLOW_EXPERIMENT_EXECUTION,
                                        ProcessAgentDB.ServiceGuid, ProcessAgentDB.ServiceGuid, ticketDuration, payload);
                                // Append op coupon to url
                                buf.Append("&coupon_id=" + opCoupon.couponId);
                                buf.Append("&passkey=" + opCoupon.passkey);
                                buf.Append("&issuer_guid=" + opCoupon.issuerGuid);
                            }
                            //else
                            //{

                            //    string schedulingUrl = RecipeExecutor.Instance().ExecuteExerimentSchedulingRecipe(uss.agentGuid, lss.agentGuid, userName, groupName,
                            //        labServer.agentGuid, client.clientGuid, client.clientName, client.version,
                            //        Convert.ToInt64(ConfigurationSettings.AppSettings["scheduleSessionTicketDuration"]), Convert.ToInt32(Session["UserTZ"]));

                            //    schedulingUrl += "&sb_url=" + ProcessAgentDB.ServiceAgent.codeBaseUrl + "/myClient.aspx";
                            //    Response.Redirect(schedulingUrl, true);
                            //}
                        }
                        else{
                            // USS Not Found
                        }
                    } // End needsScheduling
                  //Response.Redirect(Global.FormatRegularURL(Request, "myClient.aspx"), true);
                  Response.Redirect(buf.ToString(), true);
                  } // End if valid client
              else{
                throw new Exception("The specified lab client could not be found");
              }
        }

        protected void launchLabXX(int userID, int groupID, int clientID)
        {
            // Currently there is not a good solution for checking for an AllowExperiment ticket, will check the USS for reservation
            Coupon allowExecutionCoupon = null;
            StringBuilder message = new StringBuilder("Message: clientID = " + clientID);
            LabClient client = AdministrativeAPI.GetLabClient(clientID);
            string userName = null;
            Coupon opCoupon = null;
            Ticket allowTicket = null;

            if (Session["UserID"] != null)
            {
                if (userID == Convert.ToInt32(Session["UserID"]))
                {
                    userName = Session["UserName"].ToString();
                }
                else
                {
                    userName = AdministrativeAPI.GetUserName(userID);
                    Session["UserID"] = userID;
                    Session["UserName"] = userName;
                }
                string groupName = AdministrativeAPI.GetGroupName(groupID);
                if (client.clientID > 0) // It's a structure need to test for valid value
                {
                    // create the RecipeExecutor


                    string redirectURL = null;
                    DateTime start = DateTime.UtcNow;
                    long duration = 36000L; // default is ten hours
                    ProcessAgentInfo labServer = null;

                    if (client.labServerIDs.Length > 0)
                    {
                        labServer = issuer.GetProcessAgentInfo(client.labServerIDs[0]);
                    }
                    else
                    {
                        throw new Exception("The lab server is not specified for lab client " + client.clientName + " version: " + client.version);
                    }

                    //Check for Scheduling: 
                    if (client.needsScheduling)
                    {
                        int ussId = issuer.FindProcessAgentIdForClient(client.clientID, ProcessAgentType.SCHEDULING_SERVER);
                        if (ussId > 0)
                        {
                            ProcessAgent uss = issuer.GetProcessAgent(ussId);


                            //// Find efective group
                            //string effectiveGroupName = null;
                            //int effectiveGroupID = AuthorizationAPI.GetEffectiveGroupID(groupID, clientID,
                            //    Qualifier.labClientQualifierTypeID, Function.useLabClientFunctionType);
                            //if (effectiveGroupID == groupID)
                            //{
                            //    if(Session["groupName"] != null){
                            //        effectiveGroupName = Session["groupName"].ToString(); 
                            //    }
                            //    else{
                            //        effectiveGroupName = AdministrativeAPI.GetGroupName(groupID);
                            //        Session["groupName"] = effectiveGroupName;
                            //    }
                            //}
                            //else if (effectiveGroupID > 0)
                            //{
                            //   effectiveGroupName = AdministrativeAPI.GetGroupName(effectiveGroupID );
                            //}

                            int lssId = issuer.FindProcessAgentIdForAgent(client.labServerIDs[0], ProcessAgentType.LAB_SCHEDULING_SERVER);
                            ProcessAgent lss = issuer.GetProcessAgent(lssId);


                            // check for current reservation
                            string redeemPayload = TicketLoadFactory.Instance().createRedeemReservationPayload(DateTime.UtcNow, DateTime.UtcNow);
                            opCoupon = issuer.CreateTicket(TicketTypes.REDEEM_RESERVATION, uss.agentGuid, ProcessAgentDB.ServiceGuid, 600, redeemPayload);
                            UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                            OperationAuthHeader op = new OperationAuthHeader();
                            op.coupon = opCoupon;
                            ussProxy.Url = uss.webServiceUrl;
                            ussProxy.OperationAuthHeaderValue = op;
                            Reservation reservation = ussProxy.RedeemReservation(ProcessAgentDB.ServiceGuid, userName, labServer.agentGuid, client.clientGuid);
                            if (reservation != null)
                            {
                                start = reservation.Start;
                                duration = reservation.Duration;
                                string payload = TicketLoadFactory.Instance().createAllowExperimentExecutionPayload(
                                    start, duration, groupName);
                                DateTime tmpTime = start.AddTicks(duration * TimeSpan.TicksPerSecond);
                                DateTime utcNow = DateTime.UtcNow;
                                long ticketDuration = (tmpTime.Ticks - utcNow.Ticks) / TimeSpan.TicksPerSecond;
                                allowTicket = issuer.AddTicket(opCoupon, TicketTypes.ALLOW_EXPERIMENT_EXECUTION,
                                        ProcessAgentDB.ServiceGuid, ProcessAgentDB.ServiceGuid, ticketDuration, payload);
                            }
                            else
                            {

                                string schedulingUrl = RecipeExecutor.Instance().ExecuteExerimentSchedulingRecipe(uss.agentGuid, lss.agentGuid, userName, groupName,
                                    labServer.agentGuid, client.clientGuid, client.clientName, client.version,
                                    Convert.ToInt64(ConfigurationSettings.AppSettings["scheduleSessionTicketDuration"]), Convert.ToInt32(Session["UserTZ"]));

                                schedulingUrl += "&sb_url=" + ProcessAgentDB.ServiceAgent.codeBaseUrl + "/myClient.aspx";
                                Response.Redirect(schedulingUrl, true);
                            }
                        }
                    }

                    if (client.clientType == LabClient.INTERACTIVE_HTML_REDIRECT)
                    {
                        if (client.IsReentrant) // check for an existing active experiment
                        {
                            long[] ids = InternalDataDB.RetrieveActiveExperimentIDs(userID, groupID, client.labServerIDs[0], client.clientID);
                            if (ids.Length > 0)
                            {
                                long[] coupIDs = InternalDataDB.RetrieveExperimentCouponIDs(ids[0]);
                                Coupon coupon = issuer.GetIssuedCoupon(coupIDs[0]);
                                // construct the redirect query
                                StringBuilder url = new StringBuilder(client.loaderScript.Trim());
                                if (url.ToString().IndexOf("?") == -1)
                                    url.Append('?');
                                else
                                    url.Append('&');
                                url.Append("coupon_id=" + coupon.couponId + "&passkey=" + coupon.passkey
                                    + "&issuer_guid=" + issuer.GetIssuerGuid());

                                // Add the return url to the redirect
                                url.Append("&sb_url=");
                                url.Append(ProcessAgentDB.ServiceAgent.codeBaseUrl +"/myClient.aspx");

                                // Now open the lab within the current Window/frame
                                Response.Redirect(url.ToString(), true);
                            }
                        }

                        //Check for Scheduling:
                        if (client.needsScheduling)
                        {
                            //The scheduling Ticket should exist and been parsed into the session
                            if (allowTicket == null)
                            {
                                throw new Exception(" Unable to confirm a reservation for this client.");
                            }

                        }
                        // execute the "experiment execution recipe
                        redirectURL = RecipeExecutor.Instance().ExecuteExperimentExecutionRecipe(labServer, client,
                        start, duration, Convert.ToInt32(Session["UserTZ"]), userID,
                        groupID, (string)Session["GroupName"]);

                        // Add the return url to the redirect
                        redirectURL += "&sb_url=" + ProcessAgentDB.ServiceAgent.codeBaseUrl +"/myClient.aspx";

                        // Now open the lab within the current Window/frame
                        Response.Redirect(redirectURL, true);

                    }
                    else if (client.clientType == LabClient.INTERACTIVE_APPLET)
                    {
                        // Note: Currently not supporting Interactive applets
                        // use the Loader script for Batch experiments
                        // This assumes that the client will request the experiment creation

                        Session["LoaderScript"] = client.loaderScript;
                        Session.Remove("RedirectURL");

                        string jScript = @"<script language='javascript'>parent.theapplet.location.href = '"
                            + "applet.aspx" + @"'</script>";
                        Page.RegisterStartupScript("ReloadFrame", jScript);
                    }

                    // Support for Batch 6.1 Lab Clients
                    else if (client.clientType == LabClient.BATCH_HTML_REDIRECT)
                    {
                        Session["ClientID"] = client.clientID;
                        AdministrativeAPI.SetSessionClient(Convert.ToInt64(Session["SessionID"]), client.clientID);
                        // use the Loader script for Batch experiments

                        //use ticketing & redirect to url in loader script

                        // [GeneralTicketing] retrieve static process agent corresponding to the first
                        // association lab server */


                        // New comments: The HTML Client is not a static process agent, so we don't search for that at the moment.
                        // Presumably when the interactive SB is merged with the batched, this should check for a static process agent.
                        // - CV, 7/22/05

                        Session.Remove("LoaderScript");

                        //payload includes username and effective group name & client id.
                        //ideally this should be encoded in xml  - CV, 7/27/2005
                        TicketLoadFactory factory = TicketLoadFactory.Instance();
                        string uName = (string)Session["UserName"];
                        string gName = (string)Session["GroupName"];

                        string sessionPayload = factory.createRedeemSessionPayload(userID,
                           groupID, clientID, uName, gName);
                        // SB is the redeemer, ticket type : session_identifcation, no expiration time, payload,SB as sponsor ID, redeemer(SB) coupon
                        Coupon coupon = issuer.CreateTicket(TicketTypes.REDEEM_SESSION, ProcessAgentDB.ServiceGuid,
                             ProcessAgentDB.ServiceGuid, -1, sessionPayload);

                        string jScript = @"<script language='javascript'> window.open ('" + client.loaderScript + "?couponID=" + coupon.couponId + "&passkey=" + coupon.passkey + "')</script>";
                        Page.RegisterStartupScript("HTML Client", jScript);
                    }
                }
                // use the Loader script for Batch experiments
                else if (client.clientType == LabClient.BATCH_APPLET)
                {
                    Session["ClientID"] = client.clientID;
                    AdministrativeAPI.SetSessionClient(Convert.ToInt64(Session["SessionID"]), client.clientID);
                    Session["LoaderScript"] = client.loaderScript;
                    Session.Remove("RedirectURL");

                    string jScript = @"<script language='javascript'>parent.theapplet.location.href = '"
                        + ProcessAgentDB.ServiceAgent.codeBaseUrl + @"/applet.aspx" + @"'</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "ReloadFrame", jScript);

                }
            }
            else
            {
                message.Append(" LabServer = null");
            }
            //lblDebug.Text = message.ToString();
            Utilities.WriteLog(message.ToString());
        }

        protected void btnLogIn_Click(object sender, System.EventArgs e)
        {
            if (txtUsername.Text.Equals("") || txtPassword.Text.Equals(""))
            {
                lblLoginErrorMessage.Text = "<div class=errormessage><p>Missing user ID and/or password field. </p></div>";
                lblLoginErrorMessage.Visible = true;
                return;
            }
            if (hdnUser.Value != null && hdnUser.Value.Length > 0)
            {
                // Check that the specified user & current user match
                if (hdnUser.Value.ToLower().CompareTo(txtUsername.Text.ToLower()) != 0)
                {
                    lblMessages.Visible = true;
                    lblMessages.Text = "You are currently trying to login  in as a different user than the specified user. Please login as " + hdnUser.Value;
                    return;
                }
            }
          

            int userID = -1;
            userID = wrapper.GetUserIDWrapper(txtUsername.Text);

            if (userID > 0)
            {
                User user = wrapper.GetUsersWrapper(new int[] { userID })[0];

                if (userID != -1 && user.lockAccount == true)
                {
                    lblLoginErrorMessage.Text = "<div class=errormessage><p>Account locked - Email " + supportMailAddress + ". </p></div>";
                    lblLoginErrorMessage.Visible = true;
                    return;
                }

                if (AuthenticationAPI.Authenticate(userID, txtPassword.Text))
                {
                    FormsAuthentication.SetAuthCookie(txtUsername.Text, false);
                    Session["UserID"] = userID;
                    Session["UserName"] = user.userName;
                    hdnUser.Value = user.userName;
                    Session["UserTZ"] = Request.Params["userTZ"];
                    uTZ = Request.Params["userTZ"];
                
                    Session["SessionID"] = AdministrativeAPI.InsertUserSession(userID, 0, Convert.ToInt32(Request.Params["userTZ"]), Session.SessionID.ToString()).ToString();
                    HttpCookie cookie = new HttpCookie(ConfigurationManager.AppSettings["isbAuthCookieName"], Session["SessionID"].ToString());
                    Response.AppendCookie(cookie);
                    divLogin.Visible = false;

                    ResolveAction();
                }
                else
                {
                    lblLoginErrorMessage.Text = "<div class=errormessage><p>Invalid user ID and/or password. </p></div>";
                    lblLoginErrorMessage.Visible = true;
                }
            }
            else
            {
                lblLoginErrorMessage.Text = "<div class=errormessage><p>Username does not exist. </p></div>";
                lblLoginErrorMessage.Visible = true;
            }
            //this.Page_Load(this, null);
        }

        protected void modifyUserSession(int group_ID, int client_ID)
        {
            if (group_ID > 0)
            {
                string group_Name = AdministrativeAPI.GetGroupName(group_ID); ;
                Session["GroupID"] = group_ID;
                Session["GroupName"] = group_Name;
            }
            else
            {
                Session.Remove("GroupName");
                Session.Remove("GroupID");
            }
            if (client_ID > 0)
            {
                 Session["ClientID"] = client_ID;
                 Session["ClientCount"] = 1;
            }
            else
            {
                Session.Remove("ClientID");
                Session.Remove("ClientCount");
            }
            AdministrativeAPI.ModifyUserSession(Convert.ToInt64(Session["Session_ID"]), group_ID, client_ID,
                Convert.ToInt32(Session["userTZ"]), Session.SessionID.ToString());
        }
    }
}
