/* $Id: LabScheduling.asmx.cs,v 1.9 2007/06/27 22:45:02 pbailey Exp $ */
using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.Ticketing;
using iLabs.UtilLib;
using iLabs.Web;


namespace iLabs.Scheduling.LabSide
{
	/// <summary>
	/// Summary description for SchedulingService.
	/// </summary>
    [XmlType(Namespace = "http://ilab.mit.edu/iLabs/Type")]
    [WebServiceBinding(Name = "IProcessAgent", Namespace = "http://ilab.mit.edu/iLabs/Services"),
    WebServiceBinding(Name = "ILSS", Namespace = "http://ilab.mit.edu/iLabs/Services"),
    WebService(Name = "LabSideScheduling", Namespace = "http://ilab.mit.edu/iLabs/Services")]
	public class LabScheduling : WS_ILabCore
	{
    
 
		public LabScheduling()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}
        
        public OperationAuthHeader opHeader = new OperationAuthHeader();
        
		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

        /// <summary>
        /// Modifies the information related to the specified service the service's Guid must exist and the typ of service may not be modified,
        /// in and out coupons may be changed.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="inIdentCoupon"></param>
        /// <param name="outIdentCoupon"></param>
        /// <returns></returns>
        [WebMethod,
        SoapDocumentMethod(Binding = "IProcessAgent"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public override int ModifyDomainCredentials(string originalGuid, ProcessAgent agent, string extra, 
            Coupon inCoupon, Coupon outCoupon)
        {
            int status = 0;
            if (dbTicketing.AuthenticateAgentHeader(agentAuthHeader))
            {
                DBManager dbManager = new DBManager();
                try
                {
                    status = dbManager.ModifyDomainCredentials(originalGuid, agent, inCoupon, outCoupon, extra);
                }
                catch (Exception ex)
                {
                    throw new Exception("USS: ", ex);
                }


            }
            return status;

        }

        /// <summary>
        /// Informs this processAgent that it should modify all references to a specific processAent. 
        /// This is used to propagate modifications, The agentGuid must remain the same.
        /// </summary>
        /// <param name="domainGuid">The guid of the services domain ServiceBroker</param>
        /// <param name="serviceGuid">The guid of the service</param>
        /// <param name="state">The retired state to be set</param>
        /// <returns>A status value, negative values indicate errors, zero indicates unknown service, positive indicates level of success.</returns>
        [WebMethod,
        SoapDocumentMethod(Binding = "IProcessAgent"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public virtual int ModifyProcessAgent(string originalGuid, ProcessAgent agent, string extra)
        {
            int status = 0;
            if (dbTicketing.AuthenticateAgentHeader(agentAuthHeader))
            {
                DBManager dbManager = new DBManager();
                status = dbManager.ModifyProcessAgent(originalGuid, agent, extra);
            }
            return status;
        }
     
		
		/// <summary>
		/// retrieve available time periods(local time of LSS) overlaps with a time chrunk for a particular group and particular experiment,
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
		/// <param name="startTime"></param>the local time of LSS
		/// <param name="endTime"></param>the local time of LSS
		/// <returns></returns>return an array of time periods (local time), each of the time periods is longer than the experiment's minimum time 
        [WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public TimePeriod[] RetrieveAvailableTimePeriods(string serviceBrokerGuid, 
            string groupName, string ussGuid,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
		{
            Coupon opCoupon = new Coupon();
            opCoupon.couponId = opHeader.coupon.couponId;
            opCoupon.passkey = opHeader.coupon.passkey;
            opCoupon.issuerGuid = opHeader.coupon.issuerGuid;
            try
            {
                Ticket retrievedTicket = dbTicketing.RetrieveAndVerify(opCoupon, TicketTypes.REQUEST_RESERVATION);
                TimePeriod[] array = LSSSchedulingAPI.RetrieveAvailableTimePeriods(serviceBrokerGuid, groupName, ussGuid,
                    labServerGuid, clientGuid, startTime, endTime);
                return array;
            }
            catch
            {
                throw;
            }
		}

 
		/// <summary>
		/// Returns an Boolean indicating whether a particular reservation from a USS is confirmed and added to the database in LSS successfully. If it fails, exception will be throw out indicating
		///	the reason for rejection.
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>the notification whether the reservation is confirmed. If not, notification will give a reason
        [WebMethod(Description = "Returns an Boolean indicating whether a particular reservation from a USS is confirmed and added to the database in LSS successfully."
        + "If it fails, a exception will be throw indicating the reason for rejection.")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public string ConfirmReservation(string serviceBrokerGuid, string groupName, 
            string ussGuid, string labServerGuid, string clientGuid, 
            DateTime startTime, DateTime endTime)
		{
            string confirm = null;
            Coupon opCoupon = new Coupon();
            opCoupon.couponId = opHeader.coupon.couponId;
            opCoupon.passkey = opHeader.coupon.passkey;
            opCoupon.issuerGuid = opHeader.coupon.issuerGuid;
            try
            {
                Ticket retrievedTicket = dbTicketing.RetrieveAndVerify(opCoupon, TicketTypes.REQUEST_RESERVATION);
                confirm = LSSSchedulingAPI.ConfirmReservation(serviceBrokerGuid, groupName, ussGuid,
                    labServerGuid, clientGuid, startTime, endTime);
                return confirm;
            }
            catch
            {
                throw;
            }
		}

        /// <summary>
        /// Update the reservation status
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="startTime">UTC start time</param>
        /// <param name="endTime">UTC end time</param>
        /// <returns>true updated successfully, false otherwise</returns>
        [WebMethod(Description = "Used to update reservation status on LSS")]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public bool RedeemReservation(string serviceBrokerGuid, string groupName,
            string ussGuid, string labServerGuid, string clientGuid,
            DateTime startTime, DateTime endTime)
        {
            return false;
        }

		/// <summary>
		/// remove the reservation information
		/// </summary>
		/// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>true remove successfully, false otherwise
        [WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("opHeader", Direction = SoapHeaderDirection.In)]
        public int RemoveReservation(string serviceBrokerGuid, string groupName,
            string ussGuid, string labServerGuid, string clientGuid, 
            DateTime startTime, DateTime endTime)
		{
            bool removed = false;
            Coupon opCoupon = new Coupon();
            opCoupon.couponId = opHeader.coupon.couponId;
            opCoupon.passkey = opHeader.coupon.passkey;
            opCoupon.issuerGuid = opHeader.coupon.issuerGuid;
            
            try
            {
                // Ticket retrievedTicket = ticketRetrieval.RetrieveAndVerify(opCoupon, type, "LAB SCHEDULING SERVER");

                Ticket retrievedTicket = dbTicketing.RetrieveAndVerify(opCoupon, TicketTypes.REQUEST_RESERVATION);

                DateTime startTimeUTC = startTime.ToUniversalTime();
                DateTime endTimeUTC = endTime.ToUniversalTime();
                 removed = LSSSchedulingAPI.RemoveReservationInfo(serviceBrokerGuid, groupName, ussGuid,
                    labServerGuid, clientGuid, startTimeUTC, endTimeUTC);
                
            }
            catch
            {
                throw;
            }
            return removed ? 1 : 0;
		}

		/// <summary>
		/// Add information of a particular user side scheduling server identified by ussGuid. 
        /// This may be called several times with the same USS Info, due to the nature 
        /// of Scheduling this is not an error. If a revokeTicket already exists for the
        /// USS use the existing TicketCoupon.
		/// </summary>
        /// <param name="ussGuid"></param>
		/// <param name="ussName"></param>
		/// <param name="ussUrl"></param>
        /// <param name="coupon"></param>
        /// <returns>true if the USSInfo is added successfully or is already in the database, false otherwise</returns>
		[WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public int AddUSSInfo(string ussGuid, string ussName, string ussUrl, Coupon coupon)
		{
            int status = 0;
            try
            {
                if (dbTicketing.AuthenticateAgentHeader(agentAuthHeader))
                {
                    int ussId = LSSSchedulingAPI.ListUSSInfoID(ussGuid);
                    if (ussId > 0)
                    {
                        USSInfo[] info = LSSSchedulingAPI.GetUSSInfos(new int[] { ussId });
                        if(info != null && info.Length > 0){
                            if(info[0].ussGuid.Equals(ussGuid) // && info[0].ussUrl.Equals(ussUrl) 
                                && info[0].domainGuid.Equals(coupon.issuerGuid)){
                                if(info[0].couponId != coupon.couponId){
                                    // A new revokeTicket coupon has been created,
                                    // Add it to the database & update USSinfo
                                    if (!dbTicketing.AuthenticateCoupon(coupon))
                                        dbTicketing.InsertCoupon(coupon);
                                    status = LSSSchedulingAPI.ModifyUSSInfo(ussId, ussGuid,ussName,ussUrl,
                                        coupon.couponId,coupon.issuerGuid);
                                }
                                
                            }
                        }

                    }
                    if(status <= 0)
                    {
                        if( !dbTicketing.AuthenticateCoupon(coupon))
                            dbTicketing.InsertCoupon(coupon);
                        int uID = LSSSchedulingAPI.AddUSSInfo(ussGuid, ussName, ussUrl, coupon.couponId, coupon.issuerGuid);
                        if (uID > 0)
                            status = 1;
                    }

                }
            }
            catch
            {
                throw;
            }
            return status;
		}

        [WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public int ModifyUSSInfo(string ussGuid, string ussName, string ussUrl, Coupon coupon)
        {
            int status = 0;
           
                if (dbTicketing.AuthenticateAgentHeader(agentAuthHeader))
                {
                    int id = DBManager.ListUSSInfoID(ussGuid);
                    if(id > 0)
            status = DBManager.ModifyUSSInfo(id,ussGuid, ussName, ussUrl, coupon.couponId, coupon.issuerGuid);
                }
            return status;
        }

        [WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public int RemoveUSSInfo(string ussGuid, string ussName, string ussUrl, Coupon coupon)
        {
            int status = 0;

            if (dbTicketing.AuthenticateAgentHeader(agentAuthHeader))
            {
            }
            return status;
        }

		/// <summary>
		/// Add a credential set of a particular group, may be called multiple times with same data. 
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
		/// <returns></returns>true if the CredentialSet is added successfully, or already exists, false otherwise
		[WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public int AddCredentialSet(string serviceBrokerGuid, string serviceBrokerName, 
            string groupName, string ussGuid)
		{
            int status = 0;
            if (dbTicketing.AuthenticateAgentHeader(agentAuthHeader))
            {
                int test = LSSSchedulingAPI.GetCredentialSetID(serviceBrokerGuid, groupName, ussGuid);
                if (test > 0)
                {
                    status = 1;
                }
                else
                {
                    int cID = LSSSchedulingAPI.AddCredentialSet(serviceBrokerGuid, serviceBrokerName, groupName, ussGuid);
                    if (cID != -1)
                        status = 1;
                }
            }
			return status;
		}

        [WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public int ModifyCredentialSet(string serviceBrokerGuid, string serviceBrokerName,
            string groupName, string ussGuid)
        {
            if (dbTicketing.AuthenticateAgentHeader(agentAuthHeader))
            {
                int id = DBManager.GetCredentialSetID(serviceBrokerGuid, groupName, ussGuid);
                return LSSSchedulingAPI.ModifyCredentialSet(id, serviceBrokerGuid, serviceBrokerName, groupName, ussGuid);
            }
            else
                return 0;
        }

        /// <summary>
        /// remove a credential set of a particular group
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <returns></returns>true if the CredentialSet is removed successfully, false otherwise
        [WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
       SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public int RemoveCredentialSet(string serviceBrokerGuid, string serviceBrokerName,
            string groupName, string ussGuid)
        {
            if (dbTicketing.AuthenticateAgentHeader(agentAuthHeader))
            {
                return LSSSchedulingAPI.RemoveCredentialSet(serviceBrokerGuid, serviceBrokerName, groupName, ussGuid);
            }
            else 
                return 0;
        }
		
          /// <summary>
        /// add information of a particular experiment
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="labServerName"></param>
        /// <param name="labClientGuid"></param>
        /// <param name="labClientVersion"></param>
        /// <param name="labClientName"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>true, the experimentInfo is added 
        /// successfully, false otherwise
        [WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public int AddExperimentInfo(string labServerGuid, string labServerName,
            string clientGuid, string clientName, string clientVersion,
            string providerName)
        {
            if (dbTicketing.AuthenticateAgentHeader(agentAuthHeader))
            {
                int id = LSSSchedulingAPI.AddExperimentInfo(labServerGuid, labServerName, clientGuid, clientName,
                    clientVersion, providerName,null, 0, 0, 0, 0);
                int ok = LSSSchedulingAPI.CheckForLSResource(labServerGuid, labServerName);
                return (id > 0)? 1:0;
            }
            else
                return 0;
        }

        [WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public int ModifyExperimentInfo(string labServerGuid, string labServerName,
            string clientGuid, string clientName, string clientVersion,
            string providerName)
        {
            int status = 0;
            int id = DBManager.ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
            if (id > 0)
            {
                LssExperimentInfo[] exps = DBManager.GetExperimentInfos(new int[] { id });
                if (exps != null && exps.Length > 0)
                {
                    if (DBManager.ModifyExperimentInfo(id, labServerGuid, labServerName, clientGuid, clientName,
                        clientVersion, providerName, null,
                        exps[0].prepareTime, exps[0].recoverTime, exps[0].minimumTime, exps[0].earlyArriveTime))
                        status++;
                }
            }
            return status;
        }

        /// <summary>
        /// Remove a particular experiment
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="labServerName"></param>
        /// <param name="clientGuid"></param>
        /// <param name="clientVersion"></param>
        /// <param name="clientName"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>true, the experimentInfo is removed 
        /// successfully, false otherwise
        [WebMethod]
        [SoapDocumentMethod(Binding = "ILSS"),
        SoapHeader("agentAuthHeader", Direction = SoapHeaderDirection.In)]
        public int RemoveExperimentInfo(string labServerGuid, string labServerName,
            string clientGuid, string clientName, string clientVersion,
            string providerName)
        {
            int status = 0;
            return status;
        }

 

    /*    
        /// <summary>
        /// Modifies the information related to the specified service the service's Guid must exist and the typ of service may not be modified,
        /// in and out coupons may be changed.
        /// </summary>
        /// <param name="newCredentials"></param>
       /// <returns></returns>
        protected override int modifyDomainCredentials(string originalGuid, ProcessAgent agent,
            Coupon inCoupon,Coupon outCoupon, string extra)
        {
            int status = 0;
            try
            {
               status = modifyProcessAgent(originalGuid, agent,extra);
            }
            catch (Exception ex)
            {
                throw new Exception("LSS: ", ex);
            }
            if (agent.type == ProcessAgentType.SERVICE_BROKER || agent.type == ProcessAgentType.REMOTE_SERVICE_BROKER)
            {
                //Check for SB names in credential sets
                DBManager.ModifyCredentialSetServiceBroker(agent.agentGuid, agent.agentName);
            }
            if (agent.type == ProcessAgentType.LAB_SERVER)
            {
                // Labserver Names in Experiment info's
                // Labserver Names in LS_Resource
                DBManager.ModifyExperimentLabServer(originalGuid,agent.agentGuid, agent.agentName);
            }
            if (agent.type == ProcessAgentType.SCHEDULING_SERVER)
            {
                // USS path, & name in USS_Info
                int ussId = DBManager.ListUSSInfoID(agent.agentGuid);
                if (ussId > 0)
                {
                    if (inCoupon == null)
                    {
                        dbTicketing.InsertCoupon(inCoupon);
                    }
                   status = DBManager.ModifyUSSInfo(ussId, agent.agentGuid,agent.agentName, agent.webServiceUrl,
                        inCoupon.couponId, inCoupon.issuerGuid);
                }
            }
            //return dbTicketing.GetProcessAgent(credentials.agent.agentGuid);
            return status;

        }
        
        */
  
	}
}
