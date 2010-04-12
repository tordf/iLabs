/* $Id: RecipeExecutor.cs,v 1.33 2007/12/26 05:27:26 pbailey Exp $ */
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

using iLabs.Core;
using iLabs.Proxies.ESS;
using iLabs.ServiceBroker;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Ticketing;
using System.Collections.Generic;



//using iLabs.Proxies.ESS;
//using iLabs.Proxies.ISB;

namespace iLabs.ServiceBroker
{
    public class RecipeExecutor
    {
        protected static RecipeExecutor instance;

        public static RecipeExecutor Instance()
        {
            if (instance == null)
                instance = new RecipeExecutor();

            return instance;
        }

        protected RecipeExecutor()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="labServer"></param>
        /// <param name="client"></param>
        /// <param name="startExecution"></param>
        /// <param name="duration"></param>
        /// <param name="userTZ"></param>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="groupName"></param>
        /// <returns>The redirect url where the user should be redirected, with the coupon appended to it</returns>
        public string ExecuteExperimentExecutionRecipe(ProcessAgentInfo labServer, LabClient client,
            DateTime startExecution, long duration, int userTZ, int userID, int groupID, string groupName)
        {

            BrokerDB brokerDB;
            Coupon coupon;
            // refactored out
            ExecuteExperimentExecutionRecipe(labServer, ref client, ref startExecution, duration, userTZ, userID, groupID, groupName, out brokerDB, out coupon);

            // construct the redirect query
            StringBuilder url = new StringBuilder(client.loaderScript.Trim());
            if (url.ToString().IndexOf("?") == -1)
                url.Append('?');
            else
                url.Append('&');
            url.Append("coupon_id=" + coupon.couponId + "&passkey=" + coupon.passkey
                + "&issuer_guid=" + brokerDB.GetIssuerGuid());            

            //return the appended url
            return url.ToString();
        }

        /// <summary>
        /// Modified ExecuteExperimentExecutionRecipe inorder to better support interactive applets. 
        /// </summary>
        /// <param name="labServer"></param>
        /// <param name="client"></param>
        /// <param name="startExecution"></param>
        /// <param name="duration"></param>
        /// <param name="userTZ"></param>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <param name="groupName"></param>
        /// <returns></returns> added silly bool should be changed
        public IDictionary<string, string> ExecuteExperimentExecutionRecipe(ProcessAgentInfo labServer, LabClient client,
            DateTime startExecution, long duration, int userTZ, int userID, int groupID, string groupName, bool silly)
        {
            BrokerDB brokerDB;
            Coupon coupon;
            // refactored out
            ExecuteExperimentExecutionRecipe(labServer, ref client, ref startExecution, duration, userTZ, userID, groupID, groupName, out brokerDB, out coupon);

            // 
            IDictionary<string,string> ht = new Dictionary<string,string>();
            ht["passkey"] = coupon.passkey;
            ht["coupon_id"] = coupon.couponId.ToString();
            ht["issuer_guid"] = brokerDB.GetIssuerGuid();
            //return the appended url
            return ht;
        }

        private static void ExecuteExperimentExecutionRecipe(ProcessAgentInfo labServer, ref LabClient client, ref DateTime startExecution, long duration, int userTZ, int userID, int groupID, string groupName, out BrokerDB brokerDB, out Coupon coupon)
        {
            int essId = 0;
            ProcessAgentInfo essAgent = null;

            long ticketDuration = 7200; //Default to 2 hours
            //   Add a 10 minutes to ESS ticket duration ( in seconds ) to extend beyond experiment expiration
            if (duration != -1)
            {
                //ticketDuration = duration + 60; // For testing only add a minute
                ticketDuration = duration + 600; // Add 10 minutes beyond the experiment end
            }
            else
            {
                ticketDuration = -1;
            }

            // Authorization wrapper
            AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();

            // create ticket issuer and payload factory
            brokerDB = new BrokerDB();
            TicketLoadFactory factory = TicketLoadFactory.Instance();

            if (client.needsESS)
            {
                essId = brokerDB.FindProcessAgentIdForClient(client.clientID, ProcessAgentType.EXPERIMENT_STORAGE_SERVER);

            }

            // 1. Create Coupon for ExperimentCollection
            coupon = brokerDB.CreateCoupon();

            //
            // 2. create ServiceBroker experiment record and get corresponding experiment id
            // This checks authorization.
            long experimentID = wrapper.CreateExperimentWrapper(StorageStatus.INITIALIZED,
                userID, groupID, labServer.agentId, client.clientID,
                essId, startExecution, duration);

            // Store a record of the Experiment Collection Coupon
            DataStorageAPI.InsertExperimentCoupon(experimentID, coupon.couponId);
            string essWebAddress = null;


            // If a ESS is specified Create the ESS Tickets, this should only happen if a resource is mapped
            if (essId > 0)
            {
                //3.A create ESS administer experiment ticket, Add 10 minutes to duration
                // This must be created before the ESS experiment records may be created
                essAgent = brokerDB.GetProcessAgentInfo(essId);
                if ((essAgent != null) && !essAgent.retired)
                {
                    brokerDB.AddTicket(coupon,
                           TicketTypes.ADMINISTER_EXPERIMENT, essAgent.AgentGuid, brokerDB.GetIssuerGuid(), ticketDuration, factory.createAdministerExperimentPayload(experimentID, essAgent.webServiceUrl));

                    //3.B create store record ticket
                    brokerDB.AddTicket(coupon,
                           TicketTypes.STORE_RECORDS, essAgent.agentGuid, labServer.agentGuid, ticketDuration, factory.StoreRecordsPayload(true, experimentID, essAgent.webServiceUrl));

                    //3.C create retrieve experiment ticket, retrieve Experiment Records never expires, unless experiment deleted
                    //    This should be changed to a long but finite period once eadExisting Expermint is in place.
                    brokerDB.AddTicket(coupon,
                           TicketTypes.RETRIEVE_RECORDS, essAgent.agentGuid, brokerDB.GetIssuerGuid(), -1, factory.RetrieveRecordsPayload(experimentID, essAgent.webServiceUrl));


                    // 3.D Create the ESS Experiment Records
                    ExperimentStorageProxy ess = new ExperimentStorageProxy();
                    ess.AgentAuthHeaderValue = new AgentAuthHeader();
                    ess.AgentAuthHeaderValue.coupon = essAgent.identOut;
                    ess.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                    ess.Url = essAgent.webServiceUrl;
                    essWebAddress = essAgent.webServiceUrl;

                    // Call the ESS to create the ESS Records and open the experiment
                    StorageStatus status = ess.OpenExperiment(experimentID, ticketDuration);
                    if (status != null)
                        DataStorageAPI.UpdateExperimentStatus(status);
                }
            }

            //
            // 4. create the execution ticket for the experiment
            //

            // 4.A create payload 
            string payload = factory.createExecuteExperimentPayload(essWebAddress, startExecution, duration,
                userTZ, groupName, brokerDB.GetIssuerGuid(), experimentID);


            // 4.B create experiment execution ticket.
            brokerDB.AddTicket(coupon,
                      TicketTypes.EXECUTE_EXPERIMENT, labServer.agentGuid, labServer.agentGuid, ticketDuration, payload);

            // 4.C Create sessionRedemption Ticket
            string sessionPayload = factory.createRedeemSessionPayload(userID, groupID, client.clientID);
            brokerDB.AddTicket(coupon,
                      TicketTypes.REDEEM_SESSION, brokerDB.GetIssuerGuid(), brokerDB.GetIssuerGuid(), ticketDuration, sessionPayload);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="groupName"></param>
        /// <param name="labClientName"></param>
        /// <param name="labClientVersion"></param>
        /// <returns>The redirect url where the user should be redirected, with the coupon appended to it</returns>
        public string ExecuteExerimentSchedulingRecipe(string ussGuid, string lssGuid, string username, string groupName, 
            string labServerGuid, string clientGuid, string labClientName, string labClientVersion, long duration, int userTZ)
        {
            try
            {
                BrokerDB issuer = new BrokerDB();
                TicketLoadFactory factory = TicketLoadFactory.Instance();

                string ticketType = TicketTypes.SCHEDULE_SESSION;
                
                //the uss is the redeemer of the scheduling ticket
                //string redeemerId = uss.agentGuid;

                //the SB is the sponsor of the scheduling ticket
                //string sponsorId = issuer.GetIssuerGuid();

             

                string payload1 = factory.createScheduleSessionPayload(username, groupName, issuer.GetIssuerGuid(),
                    labServerGuid, clientGuid, labClientName, labClientVersion, userTZ);
                string payload2 = factory.createRequestReservationPayload();

                Coupon schedulingCoupon = issuer.CreateTicket(TicketTypes.SCHEDULE_SESSION, ussGuid, 
                    issuer.GetIssuerGuid(), duration, payload1);
                issuer.AddTicket(schedulingCoupon, TicketTypes.REQUEST_RESERVATION, lssGuid, ussGuid, 
                    duration, payload2);

                //
                // construct the redirection url
                //
                string issuerGuid = schedulingCoupon.issuerGuid;
                string passkey = schedulingCoupon.passkey;
                string couponId = schedulingCoupon.couponId.ToString();

                // obtain the reservation URL from the admin URLs table
                string schedulingUrl = issuer.RetrieveAdminURL(ussGuid, TicketTypes.SCHEDULE_SESSION).Url;                
                schedulingUrl += "?coupon_id=" + couponId + "&issuer_guid=" + issuerGuid + "&passkey=" + passkey;

                return schedulingUrl;
            }
            catch
            {
                throw;
            }
        }
    }
}
