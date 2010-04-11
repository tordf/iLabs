using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;

using iLabs.Core;
using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.StorageTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.Ticketing;
using iLabs.TicketIssuer;
using iLabs.UtilLib;

using iLabs.ServiceBroker.Administration;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker.Internal;
using iLabs.ServiceBroker.Mapping;
using iLabs.Proxies.PAgent;
using iLabs.Proxies.ESS;

namespace iLabs.ServiceBroker
{
    /// <summary>
    /// Interface for the DB Layer class
    /// </summary>
    public class BrokerDB : TicketIssuerDB
    {
        

        //protected static ResourceMapping[] resourceMappings;

        public BrokerDB()
        {
        }

        public ProcessAgentInfo GetExperimentESS(long experimentID)
        {
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("getEssInfoForExperiment", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter(myCommand,"@experimentID", experimentID, DbType.Int64));
            ProcessAgentInfo ess = null;
            try
            {
                myConnection.Open();
                DbDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    ess = readAgentInfo(myReader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown reading ESSinfo", ex);
            }
            finally
            {
                myConnection.Close();
            }
            return ess;
        }

        public Coupon GetEssOpCoupon(long experimentId, string ticketType,
             long duration, string essGuid)
        {
            Coupon opCoupon = null;
            long[] couponIDs = DataStorageAPI.RetrieveExperimentCouponIDs(experimentId);
            if (couponIDs != null && couponIDs.Length >= 0)
            {   // An experiment ticket collection exists, try and find an active
                // Retrieve_Records ticket
                for (int i = 0; i < couponIDs.Length; i++)
                {
                    Coupon tmpCoupon = GetIssuedCoupon(couponIDs[i]);
                    Ticket ticket = RetrieveTicket(tmpCoupon, ticketType);
                    if (ticket != null && !ticket.IsExpired()&& (ticket.SecondsToExpire() > duration))
                    {
                        if (ticket.redeemerGuid.CompareTo(essGuid) == 0)
                        {
                            opCoupon = tmpCoupon;
                            break;
                        }
                    }
                }
            }
            return opCoupon;
        }

        public Experiment RetrieveExperiment(long experimentID, int userID, int groupID)
        {
            int roles = 0;
            Experiment experiment = null;
            AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
            roles = wrapper.GetExperimentAuthorizationWrapper(experimentID, userID, groupID);

            if ((roles | ExperimentAccess.READ) == ExperimentAccess.READ)
            {
                experiment = new Experiment();
                experiment.experimentId = experimentID;
                experiment.issuerGuid = ProcessAgentDB.ServiceGuid;
                ProcessAgentInfo ess = GetExperimentESS(experimentID);
                if (ess != null)
                {
                    ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                    Coupon opCoupon = GetEssOpCoupon(experimentID, TicketTypes.RETRIEVE_RECORDS, 60, ess.agentGuid);
                    if (opCoupon == null)
                    {
                        string payload = TicketLoadFactory.Instance().RetrieveRecordsPayload(experimentID, ess.webServiceUrl);
                        opCoupon = CreateTicket(TicketTypes.RETRIEVE_RECORDS, ess.agentGuid, ProcessAgentDB.ServiceGuid,
                            60, payload);
                    }
                    essProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                    essProxy.OperationAuthHeaderValue.coupon = opCoupon;
                    essProxy.Url = ess.webServiceUrl;
                    experiment.records = essProxy.GetRecords(experimentID, null);
                }

            }
            else
            {
                throw new AccessDeniedException("You do not have permission to read this experiment");
            }

            return experiment;
        }

        public ExperimentRecord[] RetrieveExperimentRecords(long experimentID, int userID, int groupID, Criterion[] criteria)
        {
            int roles = 0;
            ExperimentRecord[] records = null;
            AuthorizationWrapperClass wrapper = new AuthorizationWrapperClass();
            roles = wrapper.GetExperimentAuthorizationWrapper(experimentID, userID, groupID);

            if ((roles | ExperimentAccess.READ) == ExperimentAccess.READ)
            {
                records = RetrieveExperimentRecords(experimentID, criteria);

            }
            else
            {
                throw new AccessDeniedException("You do not have permission to read this experiment");
            }

            return records;
        }

        public ExperimentRecord[] RetrieveExperimentRecords(long experimentID, Criterion[] criteria)
        {
            ExperimentRecord[] records = null;
            ProcessAgentInfo ess = GetExperimentESS(experimentID);
            if (ess != null)
            {
                ExperimentStorageProxy essProxy = new ExperimentStorageProxy();
                Coupon opCoupon = GetEssOpCoupon(experimentID, TicketTypes.RETRIEVE_RECORDS, 60, ess.agentGuid);
                if (opCoupon == null)
                {
                    string payload = TicketLoadFactory.Instance().RetrieveRecordsPayload(experimentID, ess.webServiceUrl);
                    opCoupon = CreateTicket(TicketTypes.RETRIEVE_RECORDS, ess.agentGuid, ProcessAgentDB.ServiceGuid,
                        60, payload);
                }
                essProxy.OperationAuthHeaderValue = new OperationAuthHeader();
                essProxy.OperationAuthHeaderValue.coupon = opCoupon;
                essProxy.Url = ess.webServiceUrl;
                records = essProxy.GetRecords(experimentID, criteria);
            }
            return records;
        }


        //public Coupon CreateExperimentTicketCollection(ProcessAgentInfo ess, int userId, int groupId, int roles, long duration){
        //    Coupon coupon = CreateCoupon();
           
        //        TicketLoadFactory factory = TicketLoadFactory.Instance();
        //        string payload = null;
        //        if (ticketType.CompareTo(TicketTypes.ADMINISTER_EXPERIMENT) == 0)
        //        {
        //            payload = factory.createAdministerESSPayload();
        //        }
        //        if (ticketType.CompareTo(TicketTypes.RETRIEVE_RECORDS) == 0)
        //        {
        //            payload = factory.RetrieveRecordsPayload(experimentId, webServiceUrl);
        //        }
        //        if (ticketType.CompareTo(TicketTypes.STORE_RECORDS) == 0)
        //        {
        //            payload = factory.StoreRecordsPayload(true, experimentId, webServiceUrl);
        //        }
        //        // Create a ticket to read records
        //        opCoupon = CreateTicket(ticketType, agentGuid, ProcessAgentDB.ServiceGuid, duration, payload);
        //        DataStorage.InsertExperimentCoupon(experimentId, opCoupon.couponId);
            
        //    return opCoupon;
        //}

        /** Admin URL **/

        public int InsertAdminURL(DbConnection connection, int id, string url, string ticketType)
        {
            try
            {
                if (!TicketTypes.TicketTypeExists(ticketType))
                    throw new Exception("\"" + ticketType + "\" is not a valid ticket type.");

                // command executes the "InsertAdminURL" stored procedure
                DbCommand cmd = FactoryDB.CreateCommand("InsertAdminURL", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameter
                // 1. type
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@processAgentID", id, DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@adminURL", url,DbType.String, 512));
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@ticketType", ticketType, DbType.AnsiString, 100));
                

                // execute the command
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
        }

        public int InsertAdminURL(int id, string url, string ticketType)
        {
            if (!TicketTypes.TicketTypeExists(ticketType))
                throw new Exception("\"" + ticketType + "\" is not a valid ticket type.");
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                connection.Open();
                return InsertAdminURL(connection, id, url, ticketType);
            }

            finally
            {
                connection.Close();
            }
        }

        public void DeleteAdminURL(int Id)
        {

            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                connection.Open();
                DeleteAdminURL(connection, Id);
            }
            finally
            {
                connection.Close();
            }
        }


        public int DeleteAdminURL(DbConnection connection, int Id)
        {
            try
            {

                // command executes the "DeleteAdminURLbyID" stored procedure
                DbCommand cmd = FactoryDB.CreateCommand("DeleteAdminURLbyID", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameter
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@adminURLID",Id, DbType.Int32));
                
                // execute the command
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
        }


        public int DeleteAdminURL(DbConnection connection, int processAgentId, string url, string ticketType)
        {
            try
            {
                if (!TicketTypes.TicketTypeExists(ticketType))
                    throw new Exception("\"" + ticketType + "\" is not a valid ticket type.");

                // command executes the "InsertAdminURL" stored procedure
                DbCommand cmd = FactoryDB.CreateCommand("DeleteAdminURL", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameter
                // 1. type
               cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@processAgentID",processAgentId,DbType.Int32));
               cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@adminURL", url, DbType.String, 512));
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@ticketType", ticketType, DbType.AnsiString, 100));
                

                // execute the command
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
        }

        public void DeleteAdminURL(ProcessAgentInfo processAgentInfo, string url, string ticketType)
        {
            if (!TicketTypes.TicketTypeExists(ticketType))
                throw new Exception("\"" + ticketType + "\" is not a valid ticket type.");
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                connection.Open();
                DeleteAdminURL(connection, processAgentInfo.AgentId, url, ticketType);
            }

            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
        }

        public void DeleteAdminURL(AdminUrl adminURL)
        {
            if (!TicketTypes.TicketTypeExists(adminURL.TicketType.name))
                throw new Exception("\"" + adminURL.TicketType.name + "\" is not a valid ticket type.");
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                connection.Open();
                DeleteAdminURL(connection, adminURL.ProcessAgentId, adminURL.Url, adminURL.TicketType.name);
            }

            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
        }

        public int ModifyAdminUrls(int agentID, string oldCodebase, string newCodebase)
        {

            int status = 0;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();
            // command executes the "InsertAdminURL" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("ModifyAdminUrlCodebase", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameter
            // 1. type
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@id", agentID, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@old", oldCodebase,DbType.String, 512));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@new",newCodebase,DbType.String, 512));
            
            try{
             // execute the command
                connection.Open();
                object obj = cmd.ExecuteScalar();
                if(obj != null)
                    status = Convert.ToInt32(obj);
            }
             catch (Exception ex)
             {
                 throw;
             }
             finally
             {
                 connection.Close();
             }
            return status;
        }

        public int ModifyClientScripts(int clientID, string oldCodebase, string newCodebase)
        {
             int status = 0;
             // create sql connection
             DbConnection connection = FactoryDB.GetConnection();
            
             DbCommand cmd = FactoryDB.CreateCommand("GetLoaderScript", connection);
             cmd.CommandType = CommandType.StoredProcedure;

             // populate parameter
             // 1. type
             cmd.Parameters.Add(FactoryDB.CreateParameter(cmd, "@id", clientID,DbType.Int32));
            
             int count = -1;
             try
             {
                 // execute the command
                 connection.Open();
                 string loaderScript = Convert.ToString(cmd.ExecuteScalar());
                 if (loaderScript.Contains(oldCodebase))
                 {
                     loaderScript = loaderScript.Replace(oldCodebase, newCodebase);

                     DbCommand cmd2 = FactoryDB.CreateCommand("SetLoaderScript", connection);
                     cmd2.CommandType = CommandType.StoredProcedure;
                     cmd2.Parameters.Add(FactoryDB.CreateParameter(cmd2,"@id", clientID, DbType.Int32));
                     cmd2.Parameters.Add(FactoryDB.CreateParameter(cmd2, "@script", loaderScript, DbType.String,2000));
                     
                     count = cmd2.ExecuteNonQuery();
                 }
             }
             catch (Exception ex)
             {
                 throw;
             }
             finally
             {
                 connection.Close();
             }
             return status;
         }

        public int ModifyResourceInfoURL(int agentID, string oldCodebase, string newCodebase)
        {
            int status = 0;
            Hashtable resources = GetResourceStringTags(agentID, ResourceMappingTypes.PROCESS_AGENT);
            if (resources != null) // Check the current resources
            {
                IntTag resourceTag = null;
               
                if (resources.ContainsKey("Info URL"))
                {
                    resourceTag = (IntTag)resources["Info URL"];
                    string value = resourceTag.tag.Replace(oldCodebase, newCodebase); 
                    status = UpdateResourceMappingString(resourceTag.id, value);
                }  
            }
            return status;
        }



        public AdminUrl[] RetrieveAdminURLs(int processAgentID)
        {
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                connection.Open();
                return RetrieveAdminURLs(connection, processAgentID);
            }

            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }

        }

        public AdminUrl RetrieveAdminURL(int processAgentID, string function)
        {
            AdminUrl[] adminUrls = RetrieveAdminURLs(processAgentID);
            for (int i = 0; i < adminUrls.Length; i++)
            {
                if (adminUrls[i].TicketType.name.CompareTo(function) == 0)
                    return adminUrls[i];

            }
            return null;
        }

        public AdminUrl RetrieveAdminURL(string processAgentGuid, string function)
        {
            int id = GetProcessAgentID(processAgentGuid);
            if( id > 0)
                return RetrieveAdminURL(id, function);
            else 
                return null;
        }

        public AdminUrl[] RetrieveAdminURLs(DbConnection connection, int processAgentID)
        {
            // create sql command
            // command executes the "RetrieveAdminURLs" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrieveAdminURLs", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
           cmd.Parameters.Add(FactoryDB.CreateParameter(cmd, "@processAgentID", processAgentID, DbType.Int32));
           

            // read the result
            ArrayList list = new ArrayList();
            DbDataReader dataReader = cmd.ExecuteReader();
            try
            {
                while (dataReader.Read())
                {
                    int id = (int)dataReader.GetInt32(0);
                    processAgentID = (int)dataReader.GetInt32(1);
                    string url = dataReader.GetString(2).Trim();
                    string ticketType = dataReader.GetString(3);

                    list.Add(new AdminUrl(id, processAgentID, url, ticketType));
                }
                dataReader.Close();
            }
            catch (DbException e)
            {
                Console.WriteLine(e);
                throw;
            }

            finally
            {
                // close the sql connection
                connection.Close();
            }

            AdminUrl dummy = new AdminUrl();
            AdminUrl[] urls = (AdminUrl[])list.ToArray(dummy.GetType());
            return urls;
        }

        public override int ModifyDomainCredentials(string originalGuid, ProcessAgent agent,Coupon inCoupon, Coupon outCoupon, string extra)
        {
            int status = 0;
            ProcessAgentInfo paiOld = GetProcessAgentInfo(agent.agentGuid);
            try
            {
                status = base.ModifyDomainCredentials(originalGuid, agent, inCoupon, outCoupon,extra);
            }
            catch (Exception ex)
            {
                throw new Exception("ISB: ", ex);
            }
            if (paiOld != null)
            {
                if (agent.codeBaseUrl.CompareTo(paiOld.codeBaseUrl) != 0)
                {
                    ModifyAdminUrls(paiOld.agentId, paiOld.codeBaseUrl, agent.codeBaseUrl);
                    ModifyResourceInfoURL(paiOld.agentId, paiOld.codeBaseUrl, agent.codeBaseUrl);
                    if (paiOld.agentType == ProcessAgentType.AgentType.LAB_SERVER)
                    {
                        ModifyClientScripts(paiOld.agentId, paiOld.codeBaseUrl, agent.codeBaseUrl);
                    }
                }
            }
            //Notify all ProcessAgents about the change
            ProcessAgentInfo[] domainServices = GetProcessAgentInfos();
            ProcessAgentProxy proxy = null;
            foreach (ProcessAgentInfo pi in domainServices)
            {
                // Do not send if retired this service or the service being modified since this is
                if (!pi.retired && (pi.agentGuid.CompareTo(ProcessAgentDB.ServiceGuid) != 0) 
                    && (pi.agentGuid.CompareTo(agent.agentGuid) != 0))
                {
                    proxy = new ProcessAgentProxy();
                    proxy.AgentAuthHeaderValue = new AgentAuthHeader();
                    proxy.AgentAuthHeaderValue.coupon = pi.identOut;
                    proxy.Url = pi.webServiceUrl;

                    proxy.ModifyDomainCredentials(originalGuid, agent, extra, inCoupon, outCoupon);
                }
            }
            return status;
        }

        public override int ModifyProcessAgent(string originalGuid, ProcessAgent agent, string extra)
        {
            int status = 0;
            ProcessAgentInfo paiOld = GetProcessAgentInfo(originalGuid);

            if (paiOld != null)
            {

                try
                {
                    status = UpdateProcessAgent(paiOld.agentId, agent.agentGuid, agent.agentName, agent.type,
                        agent.domainGuid, agent.codeBaseUrl, agent.webServiceUrl);
                }
                catch (Exception ex)
                {
                    throw new Exception("ISB: ", ex);
                }

                if (agent.codeBaseUrl.CompareTo(paiOld.codeBaseUrl) != 0)
                {
                    status += ModifyAdminUrls(paiOld.agentId, paiOld.codeBaseUrl, agent.codeBaseUrl);
                    status += ModifyResourceInfoURL(paiOld.agentId, paiOld.codeBaseUrl, agent.codeBaseUrl);
                    if (paiOld.agentType == ProcessAgentType.AgentType.LAB_SERVER)
                    {
                        status += ModifyClientScripts(paiOld.agentId, paiOld.codeBaseUrl, agent.codeBaseUrl);
                    }
                }
                if(agent.agentName.CompareTo(paiOld.agentName) !=0){
                    // need to update Qualifier Names
                    AuthorizationAPI.ModifyQualifierName(Qualifier.ToTypeID(agent.type),paiOld.agentId,agent.agentName);
                    int[] resourceMapIds = GetResourceMappingIdsByValue(ResourceMappingTypes.PROCESS_AGENT,paiOld.agentId);
                    foreach(int id in resourceMapIds){
                        ResourceMapping map =ResourceMapManager.GetMap(id);
                        AuthorizationAPI.ModifyQualifierName(Qualifier.resourceMappingQualifierTypeID,id,ResourceMappingToString(map));
                    }
                }
                //Notify all ProcessAgents about the change
                ProcessAgentInfo[] domainServices = GetProcessAgentInfos();
                ProcessAgentProxy proxy = null;
                foreach (ProcessAgentInfo pi in domainServices)
                {
                    // Do not send if retired this service or the service being modified since this is
                    if (!pi.retired && (pi.agentType != ProcessAgentType.AgentType.BATCH_LAB_SERVER)
                        && (pi.agentGuid.CompareTo(ProcessAgentDB.ServiceGuid) != 0)
                        && (pi.agentGuid.CompareTo(agent.agentGuid) != 0))
                    {
                        proxy = new ProcessAgentProxy();
                        proxy.AgentAuthHeaderValue = new AgentAuthHeader();
                        proxy.AgentAuthHeaderValue.agentGuid = ProcessAgentDB.ServiceGuid;
                        proxy.AgentAuthHeaderValue.coupon = pi.identOut;
                        proxy.Url = pi.webServiceUrl;
                        try
                        {
                            status += proxy.ModifyProcessAgent(originalGuid, agent, extra);
                        }
                        catch (Exception ex)
                        {
                            Exception ex2 = new Exception("ModifyProcessAgent: " + pi.webServiceUrl, ex);
                            throw ex2;
                        }
                    }
                }
            }

            return status;
        }


        /* START OF RESOURCE MAPPING */
     
        public IntTag[] GetAdminServiceTags(int groupID)
        {
            ArrayList list = new ArrayList();
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                DbCommand cmd = FactoryDB.CreateCommand("GetAdminServiceTags", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                   // populate stored procedure parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupId", groupID,DbType.Int32));
                

                // read the result
                connection.Open();
                DbDataReader dataReader = cmd.ExecuteReader();
           
                while (dataReader.Read())
                {
                    int id = dataReader.GetInt32(0);
                    string name = dataReader.GetString(1);
                    list.Add(new IntTag(id,name));
                }
                dataReader.Close();
            }
            catch (DbException e)
            {
                Console.WriteLine(e);
                throw;
            }

            finally
            {
                // close the sql connection
                connection.Close();
            }

            IntTag dummy = new IntTag();
            IntTag[] tags = (IntTag[])list.ToArray(dummy.GetType());
            return tags;
        }


        public IntTag[] GetAdminProcessAgentTags(int groupID)
        {
            ArrayList list = new ArrayList();
            DbConnection connection = FactoryDB.GetConnection();
            try
            {
                DbCommand cmd = FactoryDB.CreateCommand("GetAdminProcessAgentTags", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                   // populate stored procedure parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupID", groupID, DbType.Int32));

                // read the result
                connection.Open();
                DbDataReader dataReader = cmd.ExecuteReader();
           
                while (dataReader.Read())
                {
                    int id = dataReader.GetInt32(0);
                    string name = dataReader.GetString(1);
                    list.Add(new IntTag(id,name));
                }
                dataReader.Close();
            }
            catch (DbException e)
            {
                Console.WriteLine(e);
                throw;
            }

            finally
            {
                // close the sql connection
                connection.Close();
            }

            IntTag dummy = new IntTag();
            IntTag[] tags = (IntTag[])list.ToArray(dummy.GetType());
            return tags;
        }

        public Grant[] GetProcessAgentAdminGrants(int agentID, int groupID)
        {
            ArrayList list = new ArrayList();
            DbConnection myConnection = FactoryDB.GetConnection();
            DbCommand myCommand = FactoryDB.CreateCommand("GetProcessAgentAdminGrants", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add(FactoryDB.CreateParameter(myCommand, "@agentID", agentID, DbType.Int32));
            myCommand.Parameters.Add(FactoryDB.CreateParameter(myCommand, "@groupID", groupID, DbType.Int32));


            try
            {
                myConnection.Open();
                DbDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    Grant grant = new Grant();
                    grant.agentID = reader.GetInt32(0);
                    grant.function = reader.GetString(1);
                    grant.grantID = reader.GetInt32(2);
                    grant.qualifierID = reader.GetInt32(3);
                    list.Add(grant);
                }
                reader.Close();
            }
            catch
            {
            }
            finally
            {
                myConnection.Close();
            }
            Grant dummy = new Grant();
            Grant[] grants = (Grant[])list.ToArray(dummy.GetType());
            return grants;

        }

     
	
       public ResourceMapping AddResourceMapping(string keyType, object key, string[] valueTypes, object[] values){
           DbConnection connection = FactoryDB.GetConnection();
           ResourceMapping mapping = null;
           try
           {
               connection.Open();
               mapping = InsertResourceMapping(connection, keyType, key, valueTypes, values);
               if (mapping != null)
               {
                  
                   // add the new resource mapping to the static resource mappings array
                   ResourceMapManager.Add(mapping);
                   

               }


           }
           catch (DbException sqlEx)
           {
           }
           finally
           {
               connection.Close();
           }

           return mapping;

       }

        protected ResourceMapping InsertResourceMapping(DbConnection connection, string keyType, object key, string[] valueTypes, object[] values)
        {
            if (valueTypes == null || values == null)
                throw new ArgumentException("Arguments cannot be null", "valueTypes and values");

            if (valueTypes.Length != values.Length)
                throw new ArgumentException ("Parameter Arrays \"valueTypes\" and \"values\" should be of the same length");

            ResourceMappingKey mappingKey = new ResourceMappingKey(keyType, key);
            // insert key into database
         
            try
            {
                DbCommand cmd = FactoryDB.CreateCommand("InsertResourceMappingKey", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Get the key type id
                int keyTypeID = ResourceMappingTypes.GetResourceMappingTypeID(keyType);
                if (keyTypeID == -1)
                    throw new ArgumentException("Value for key type is invalid");

                int keyID = -1;

                // if the key is a string, add the string to the strings table
                if (keyType.Equals(ResourceMappingTypes.STRING))
                {
                    DbCommand cmd2 = FactoryDB.CreateCommand("AddResourceMappingString", connection);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    // populate parameters
                    cmd2.Parameters.Add(FactoryDB.CreateParameter(cmd2,"@string_Value", key, DbType.String,2048));

                    keyID = Convert.ToInt32(cmd2.ExecuteScalar());
                }

                // if the key is a Resource Type, add the string to the ResourceTypes table
                else if (keyType.Equals(ResourceMappingTypes.RESOURCE_TYPE))
                {
                    DbCommand cmd2 = FactoryDB.CreateCommand("AddResourceMappingResourceType", connection);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.Add(FactoryDB.CreateParameter(cmd2,"@resourceType_Value", key, DbType.String,256));

                    keyID = Convert.ToInt32(cmd2.ExecuteScalar());
                }

                else
                    keyID = ResourceMappingEntry.GetId(key);

                if (keyID == -1)
                    throw new ArgumentException("Value for key is invalid");

                // populate stored procedure parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@MappingKey_Type",keyTypeID, DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@MappingKey", keyID,DbType.Int32));
                
                // execute the command
                int mappingID = Convert.ToInt32(cmd.ExecuteScalar());

                //
                // insert mapping values
                //
                ResourceMappingValue[] mappingValues = new ResourceMappingValue[values.Length];
                for (int i = 0; i < mappingValues.Length; i++)
                {
                    mappingValues[i] = new ResourceMappingValue(valueTypes[i], values[i]);

                    // Get the value type id
                    int valueTypeID = ResourceMappingTypes.GetResourceMappingTypeID(valueTypes[i]);
                    if (valueTypeID == -1)
                        throw new ArgumentException("Value for value type \"" + i + "\" is invalid");

                    int valueID = -1;

                    // if the value is a string, add the string to the strings table
                    if (valueTypes[i].Equals(ResourceMappingTypes.STRING))
                    {
                        DbCommand cmd2 = FactoryDB.CreateCommand("AddResourceMappingString", connection);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        // populate parameters
                        cmd2.Parameters.Add(FactoryDB.CreateParameter(cmd2,"@string_Value", (string)values[i],DbType.String,2048));
                    
                        valueID = Convert.ToInt32(cmd2.ExecuteScalar());
                    }

                    // if the key is a Resource Type, add the string to the ResourceTypes table
                    else if (valueTypes[i].Equals(ResourceMappingTypes.RESOURCE_TYPE))
                    {
                        DbCommand cmd2 = FactoryDB.CreateCommand("AddResourceMappingResourceType", connection);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.Add(FactoryDB.CreateParameter(cmd2,"@resourceType_Value",(string)values[i],DbType.String,256));
                        valueID = Convert.ToInt32(cmd2.ExecuteScalar());
                    }

                    else
                        valueID = ResourceMappingEntry.GetId(values[i]);

                    if (valueID == -1)
                        throw new ArgumentException("Value \"" + i + "\" is invalid");

                    cmd = FactoryDB.CreateCommand("InsertResourceMappingValue", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Clear();

                    // populate stored procedure parameters
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@Mapping_ID", mappingID,DbType.Int32));
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@MappingValue_Type", valueTypeID,DbType.Int32));
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@MappingValue", valueID, DbType.Int32));

                    // execute the command
                    mappingID = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // create new mapping object
                ResourceMapping mapping = new ResourceMapping(mappingID, mappingKey, mappingValues);


         
                return mapping;
            }

            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
        }

        public ResourceMapping AddResourceMapping(ResourceMappingKey key, ResourceMappingValue[] values)
        {
            string[] valueTypes = new string[values.Length];
            object[] valueObjs = new object[values.Length];

            for (int i = 0; i < valueTypes.Length; i++)
            {
                valueTypes[i] = values[i].type;
                valueObjs[i] = values[i].entry;
            }

            return AddResourceMapping(key.type, key.entry, valueTypes, valueObjs);
        }


         /// <summary>
        /// 
        /// </summary>
        /// <param name="mapping">Resource Mapping to be deleted</param>
        /// <returns><code>true</code> if the mapping has been deleted successfully</returns>
        public bool DeleteResourceMapping(ResourceMapping mapping)
        {
            return DeleteResourceMapping(mapping.MappingID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapping">Resource Mapping to be deleted</param>
        /// <returns><code>true</code> if the mapping has been deleted successfully</returns>
        public bool DeleteResourceMapping(int mappingId)
        {
            bool status = false;
            DbConnection connection = FactoryDB.GetConnection();

            try
            {

                // Check for any Qualifiers created for this RM
                int qualifierId = AuthorizationAPI.GetQualifierID(mappingId, Qualifier.resourceMappingQualifierTypeID);
                if (qualifierId > 0)
                {
                    // Any grant associated with this qualifier are removed via a cascading delete
                    AuthorizationAPI.RemoveQualifiers(new int[] { qualifierId });
                }

                DbCommand cmd = FactoryDB.CreateCommand("DeleteResourceMapping", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@mapping_ID", mappingId,DbType.Int32));
               
                connection.Open();
                cmd.ExecuteNonQuery();

                // Remove the Deleted ResourceMapping
                ResourceMapManager.Remove(mappingId);
                status = true;

                // update resource mappings array
                // find index of deleted mapping
                //int i = 0;
                //while (i < resourceMappings.Length)
                //{
                //    if (resourceMappings[i].MappingID == mappingId)
                //        break;
                //    i++;
                //}
                //// delete the mapping that ws found from the array
                //ResourceMapping[] temp = new ResourceMapping[resourceMappings.Length - 1];
                //Array.Copy(resourceMappings, 0, temp, 0, i);
                //Array.Copy(resourceMappings, i + 1, temp, i, resourceMappings.Length - i - 1);
                //resourceMappings = temp;

            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }

            return status;
        }

        public bool DeleteResourceMapping(string keyType, int keyId, string valueType, int valueId)
        {
            return DeleteResourceMapping(ResourceMappingTypes.GetResourceMappingTypeID(keyType), keyId, 
                ResourceMappingTypes.GetResourceMappingTypeID( valueType), valueId);
    }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapping">Resource Mapping to be deleted</param>
        /// <returns><code>true</code> if the mapping has been deleted successfully</returns>
        public bool DeleteResourceMapping(int keyTypeId, int keyId, int valueTypeId, int valueId)
        {
            bool status = false;
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                int mapId = -1;
                DbCommand cmd = FactoryDB.CreateCommand("GetMappingIdByKeyValue", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@keyID", keyId,DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@keyType", keyTypeId, DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@valueID", valueId,DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@valueType", valueTypeId,DbType.Int32));
                
                connection.Open();
                mapId  = (int)cmd.ExecuteScalar();
                if (mapId > 0)
                {
                    status = DeleteResourceMapping(mapId);
                }
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
            return status;
        }

        public ResourceMappingKey GetResourceMappingKey(int mappingID)
        {
            ResourceMapping mapping = ResourceMapManager.GetMap(mappingID);
            if(mapping != null)
                return mapping.key;
            else 
                return null;
            
        }

        public ResourceMapping GetResourceMapping(int mappingID)
        {
            return ResourceMapManager.GetMap(mappingID);
        }

        public int[] GetResourceMappingIdsByValue(string type, int value)
        {
            List<int> list = new List<int>();
            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("GetResourceMapIDsByValue", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@type", type,DbType.AnsiString, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@id", value, DbType.Int32));
           
            // execute the command
            try
            {
                DbDataReader dataReader = null;
                connection.Open();
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    if (!DBNull.Value.Equals(dataReader.GetValue(0)))
                        list.Add(dataReader.GetInt32(0));
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return list.ToArray();
        }

        /// <summary>
        /// Reads a resource mapping from the database
        /// </summary>
        /// <param name="mappingID">id of the resource mapping</param>
        /// <returns>ResourceMapping object</returns>
        public ResourceMapping ReadResourceMapping(int mappingID)
        {
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                connection.Open();
                ResourceMapping mapping = ReadResourceMapping(mappingID, connection);

                return mapping;
            }

            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
        }
      
        public ResourceMapping ReadResourceMapping(int mappingID, DbConnection connection)
        {
            DbDataReader dataReader = null;
            try
            {
                DbCommand cmd = FactoryDB.CreateCommand("GetResourceMappingByID", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@mappingID", mappingID, DbType.Int32));
                
                // execute the command
                dataReader = cmd.ExecuteReader();

                // first row is (key_type, key)
                dataReader.Read();
                int keyTypeID = -1, keyID = -1;
                if (!DBNull.Value.Equals(dataReader.GetValue(0)))
                    keyTypeID = dataReader.GetInt32(0);
                if (!DBNull.Value.Equals(dataReader.GetValue(1)))
                    keyID = dataReader.GetInt32(1);
                ResourceMappingKey key = (ResourceMappingKey)CompleteResourceMappingKeyRead(connection, keyTypeID, keyID, true);

                // subsequent rows are (value_type, value)
                ResourceMappingValue value = null;
                ArrayList valuesList = new ArrayList();
                while (dataReader.Read())
                {
                    int valueTypeID = -1, valueID = -1;
                    if (!DBNull.Value.Equals(dataReader.GetValue(0)))
                        valueTypeID = dataReader.GetInt32(0);
                    if (!DBNull.Value.Equals(dataReader.GetValue(1)))
                        valueID = dataReader.GetInt32(1);
                    value = (ResourceMappingValue)CompleteResourceMappingKeyRead(connection, valueTypeID, valueID, false);
                    valuesList.Add(value);
                }
                //if (valuesList.Count > 0)
                //{
                    ResourceMappingValue[] values = (ResourceMappingValue[])valuesList.ToArray(value.GetType());

                    // construct resource mapping
                    return new ResourceMapping(mappingID, key, values);
                //}
                //else
                //{
                //    return null;
                //}
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
            finally{
                dataReader.Close();
            }
        }

        /// <summary>
        /// Return all resource mappings in the database.
        /// If resource mappings array is not null, return it.
        /// Otherwise read resource mappings list from database and return it.
        /// </summary>
        /// <returns></returns>
        //public ResourceMapping[] GetResourceMappings()
        //{          
        //    // check if resourcemappings have been initialized before, in which case they do not need to be re-read from the DB
        //    if (resourceMappings != null)
        //        return resourceMappings;

        //    // otherwise, read mappings from the database    
        //    List<ResourceMapping> mappingList = null;
        //    mappingList = RetrieveResourceMapping();
           
        //    if (mappingList != null)
        //        resourceMappings = mappingList.ToArray();

        //    else
        //        resourceMappings = new ResourceMapping[0];
        //    return resourceMappings;
        //}

        public List<ResourceMapping> RetrieveResourceMapping()
        {
            // Read mappings from the database            
            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("GetResourceMappingIDs", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            ResourceMapping mapping = null;
            List<ResourceMapping> mappingList = new List<ResourceMapping>();
            connection.Open();
            // execute the command
            DbDataReader dataReader = null;
            dataReader = cmd.ExecuteReader();

            // read mapping ID's
            ArrayList mappingIDs = new ArrayList();
            while (dataReader.Read())
            {
                int mappingID = -1;
                if (!DBNull.Value.Equals(dataReader.GetValue(0)))
                {
                    mappingID = dataReader.GetInt32(0);
                    mappingIDs.Add(mappingID);
                }
            }

            dataReader.Close();
            connection.Close();


            // read mappings
            int ii = 0;
            int[] ids = (int[])mappingIDs.ToArray(ii.GetType());
            for (int i = 0; i < ids.Length; i++)
            {
                mapping = ReadResourceMapping(ids[i]);
                mappingList.Add(mapping);
            }
            if (mappingList.Count > 0)
            {
                return mappingList;
            }
            else
                return null;
        }
 

        /// <summary>
        /// Complete reading a resource mapping given the mapping id as well an entry id. The entry is could be a key or a value.
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="entryID"></param>
        /// <param name="isKey">Determines whether the method should return a key or a value</param>
        /// <returns></returns>
        private ResourceMappingEntry CompleteResourceMappingKeyRead(DbConnection connection, int typeID, int entryID, bool isKey)
        {
            // get the entry type
            string type = ResourceMappingTypes.GetResourceMappingType(typeID);            

            // construct the entry object based on the entry type
            object entry = null;
            if (type.Equals(ResourceMappingTypes.PROCESS_AGENT))
                // read from process agent table
                entry = entryID;
            else if (type.Equals(ResourceMappingTypes.CLIENT))
                // copy client ID
                entry = entryID;
            else if (type.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
                // copy resource mapping ID
                entry = entryID;
            else if (type.Equals(ResourceMappingTypes.STRING))
                // read string from string table
                entry = GetResourceMappingString(entryID);

            else if (type.Equals(ResourceMappingTypes.RESOURCE_TYPE))
                // read string from Resource Types table
                entry = GetResourceMappingResourceType(entryID);

            else if (type.Equals(ResourceMappingTypes.TICKET_TYPE))
                // read the ticket type from the tyckettypes class
                entry = TicketTypes.GetTicketType(entryID);
            else if (type.Equals(ResourceMappingTypes.GROUP))
                // copy group id
                entry = entryID;

            if (isKey)
                return new ResourceMappingKey(type, entry);
            else
                return new ResourceMappingValue(type, entry);
        }

        /// <summary>
        /// Read a resource mapping string from the database
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public String GetResourceMappingString(int strID)
        {
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                DbCommand cmd = FactoryDB.CreateCommand("GetResourceStringByID", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@ID", strID, DbType.Int32));

                connection.Open();
                return Convert.ToString(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }

            finally
            {
                connection.Close();
            }
        }

        public int AddResourceMappingString(string s)
        {
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                DbCommand cmd = FactoryDB.CreateCommand("AddResourceMappingString", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@string_Value", s, DbType.String,2048));
                
                connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());

            }

            finally
            {
                connection.Close();
            }

        }

        public int UpdateResourceMappingString(int id, string s)
        {
            DbConnection connection = FactoryDB.GetConnection();
            int mappingID = 0;
            try
            {
                DbCommand cmd = FactoryDB.CreateCommand("UpdateResourceMappingString", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@id", id,DbType.Int32));
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@string", s, DbType.String,2048));
               
                connection.Open();
                mappingID = Convert.ToInt32(cmd.ExecuteScalar());
                if (mappingID > 0)
                {
                    ResourceMapping rm = ReadResourceMapping(mappingID, connection);
                    ResourceMapManager.Update(rm);
                }
            }

            finally
            {
                connection.Close();
            }
            return mappingID;

        }

        public int AddResourceMappingResourceType(string s)
        {
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                DbCommand cmd = FactoryDB.CreateCommand("AddResourceMappingResourceType", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@resourceType_Value", s,DbType.String,256));
            
                connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());

            }

            finally
            {
                connection.Close();
            }

        }

        public String GetResourceMappingResourceType(int resourceTypeID)
        {
            DbConnection connection = FactoryDB.GetConnection();

            try
            {
                DbCommand cmd = FactoryDB.CreateCommand("GetResourceTypeByID", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // populate parameters
                cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@ID", resourceTypeID, DbType.Int32));
               
                connection.Open();
                return Convert.ToString(cmd.ExecuteScalar());
            }
            catch (DbException e)
            {
                writeEx(e);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        //Returns a hashtable of (mappingID, ResourceMappingValue[]) pairs
        //public Hashtable GetResourceMappingsForKey(object searchKey, string type)
        //{
        //    ResourceMappingKey key = null;

        //    if (type.Equals(ResourceMappingTypes.CLIENT))
        //    {
        //        key = new ResourceMappingKey(type, (int)searchKey);
        //    }
        //    else if (type.Equals(ResourceMappingTypes.PROCESS_AGENT))
        //    {
        //        key = new ResourceMappingKey(type, (int)searchKey);
        //    }
        //    else if (type.Equals(ResourceMappingTypes.TICKET_TYPE))
        //    {
        //        key = new ResourceMappingKey(type, (TicketType)searchKey);
        //    }
        //    else if (type.Equals(ResourceMappingTypes.GROUP))
        //    {  
        //        key = new ResourceMappingKey(type, (int)searchKey);
        //    }

           
        //    List<ResourceMapping> list = ResourceMapManager.Get(key);
        //    if (list != null && list.Count > 0)
        //    {
        //        Hashtable mappingsTable = new Hashtable();
        //        foreach (ResourceMapping rm in list)
        //        {
        //            mappingsTable.Add(rm.MappingID, rm);
        //        }
        //        return mappingsTable;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //Gets the resource mapping values as a 2D array from a mappings HashTable
        public ResourceMappingValue[][] GetResourceMappingValues(Hashtable mappingsTable)
        {
            if (mappingsTable == null || mappingsTable.Count == 0)
                return null;

            ResourceMappingValue[][] values = new ResourceMappingValue[mappingsTable.Count][];
            int i = 0;
            foreach(DictionaryEntry entry in mappingsTable)
            {
                values[i++] = ((ResourceMapping)entry.Value).values;
               
            }
            return values;  
        }

        public Hashtable GetResourceStringTags(ResourceMappingKey key)
        {
            return GetResourceStringTags((int) key.Entry, key.Type);
        }

        public Hashtable GetResourceStringTags(int target, string rmType)
        {
            
            return GetResourceStringTags(target, ResourceMappingTypes.GetResourceMappingTypeID(rmType));
        }

        public Hashtable GetResourceStringTags(int target,int type){
            Hashtable resources = null;
            Hashtable results = null;

            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = FactoryDB.CreateCommand("GetResourceTypeStrings", connection);   
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
          
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@type", type, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@target", target,DbType.Int32));
            
            try
            {
                connection.Open();
                DbDataReader reader = cmd.ExecuteReader();
                if(reader.HasRows){
                    resources = new Hashtable();
                    int mid = 0;                    
                    while(reader.Read()){
                        mid = reader.GetInt32(0);
                        resources.Add(mid,reader.GetString(1));
                    }
                    if (reader.NextResult())
                    {
                        results = new Hashtable();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            if (resources.ContainsKey(id))
                            {
                                IntTag tag = new IntTag();
                                tag.id = reader.GetInt32(1);
                                tag.tag = reader.GetString(2);
                                results.Add(resources[id], tag);
                            }
                        }
                    }          
                }
                reader.Close();
            }
                 
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                throw;
            }
            finally{
                connection.Close();
                
            }
            if(results != null && results.Count > 0)
                return results;
            else
                return null;
        }


        /// <summary>
        /// Find a ResourceMapping entry, given a matrix of values
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public object FindResourceEntry(ResourceMappingValue searchValue, ResourceMappingValue[][] values, string dataType)
        {
            if (values == null || values.Length == 0 || searchValue == null)
                return null;

            object target = null;
            bool found = false;
            int row = 0;

            //the number of "set of values" (array of values) associated with this client
            int numSetOfValues = values.GetLength(0);

            for (row = 0; row < numSetOfValues && !found; row++)
            {
                int numValues = values[row].Length;
                for (int column = 0; column < numValues; column++)
                {
                    if (values[row][column].Equals(searchValue))
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found)
            {
                ResourceMappingValue[] mappingValue = values[row - 1];
                for (int i = 0; i < mappingValue.Length; i++)
                {
                    if (mappingValue[i].Type.Equals(dataType))
                    {
                        target = mappingValue[i].Entry;
                        break;
                    }
                }
            }
            return target;
        }

        public int AssociateLSS(int lsId, int lssId)
        {
            Object keyObj = lsId;
            string keyType = ResourceMappingTypes.PROCESS_AGENT;

            ArrayList valuesList = new ArrayList();
            Object valueObj = null;

            ResourceMappingValue value = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE,
                ProcessAgentType.LAB_SCHEDULING_SERVER);
            valuesList.Add(value);

            value = new ResourceMappingValue(ResourceMappingTypes.PROCESS_AGENT,
                lssId);
            valuesList.Add(value);

            value = new ResourceMappingValue(ResourceMappingTypes.TICKET_TYPE,
                TicketTypes.GetTicketType(TicketTypes.MANAGE_LAB));
            valuesList.Add(value);

            ResourceMappingKey key = new ResourceMappingKey(keyType, keyObj);
            ResourceMappingValue[] values = (ResourceMappingValue[])valuesList.ToArray((new ResourceMappingValue()).GetType());
            ResourceMapping newMapping = AddResourceMapping(key, values);

            // add mapping to qualifier list
            int qualifierType = Qualifier.resourceMappingQualifierTypeID;
            string name = ResourceMappingToString(newMapping);
            int qualifierID = AuthorizationAPI.AddQualifier(newMapping.MappingID, qualifierType, name, Qualifier.ROOT);

            // Should a grant be created here

            return qualifierID;   
        }

        public int FindProcessAgentIdForAgent(int keyId, string type)
        {
            int result = -1;
            ResourceMappingKey key = new ResourceMappingKey(ResourceMappingTypes.PROCESS_AGENT, keyId);
            ResourceMappingValue [] search = new ResourceMappingValue[1];
            search[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, type);
            List<ResourceMapping> found = ResourceMapManager.Find(key, search);
            if (found != null && found.Count > 0)
            {
                foreach (ResourceMapping rm in found)
                {
                    for (int i = 0; i < rm.values.Length; i++)
                    {
                        if (rm.values[i].type.Equals(ResourceMappingTypes.PROCESS_AGENT))
                        {
                            result = (int)rm.values[i].entry;
                            break;
                        }
                    }
                }

            }
            
            //Hashtable mappingsTable = GetResourceMappingsForKey(keyId, ResourceMappingTypes.PROCESS_AGENT);
            //if (mappingsTable != null)
            //{
            //    ResourceMappingValue[][] values = GetResourceMappingValues(mappingsTable);
            //    result = FindProcessAgentIdForLS(keyId, values,
            //        ProcessAgentType.LAB_SCHEDULING_SERVER);
            //}
            return result;
        }
          
        /// <summary>
        /// Find a Process Agent (an LSS) associated with a particular LS, given a matrix of values
        /// </summary>
        /// <param name="lsId"></param>
        /// <param name="values"></param>
        /// <param name="processAgentType"></param>
        /// <returns></returns>
        //public int FindProcessAgentIdForLS(int lsId, ResourceMappingValue[][] values,
        //    string processAgentType)
        //{
        //    int paId = 0;
        //    if (values == null || values.Length == 0 || lsId == 0)
        //        return paId;

        //    ResourceMappingValue searchValue = null;
            

        //    if (processAgentType.Equals(ProcessAgentType.LAB_SCHEDULING_SERVER))
        //        searchValue = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.LAB_SCHEDULING_SERVER);

        //    bool foundProcessAgent = false;
        //    int row = 0;

        //    //the number of "set of values" (array of values) associated with this client
        //    int numSetOfValues = values.GetLength(0);

        //    for (row = 0; row < numSetOfValues && !foundProcessAgent; row++)
        //    {
        //        int numValues = values[row].Length;
        //        for (int column = 0; column < numValues; column++)
        //        {
        //            if (values[row][column].Equals(searchValue))
        //            {
        //                foundProcessAgent = true;
        //                break;
        //            }
        //        }
        //    }

        //    if (foundProcessAgent)
        //    {

        //        ResourceMappingValue[] associatedPA = values[row - 1];
        //        for (int i = 0; i < associatedPA.Length; i++)
        //        {
        //            if (associatedPA[i].Type.Equals(ResourceMappingTypes.PROCESS_AGENT))
        //            {
        //                paId = (int)associatedPA[i].Entry;
        //                break;
        //            }
        //        }
        //    }

        //    return paId;
        //}


        /// <summary>
        /// Finds an USS or ESS associated with a particular client, given an matrix of values
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="values"></param>
        /// <param name="processAgentType"></param>
        /// <returns></returns>
        public int FindProcessAgentIdForClient(int clientID, string processAgentType)
        {
            int paId = 0;

            ResourceMappingKey key = new ResourceMappingKey(ResourceMappingTypes.CLIENT, clientID);
            ResourceMappingValue[] searchValue = new ResourceMappingValue[1];
           

            if (processAgentType.Equals(ProcessAgentType.SCHEDULING_SERVER))
                searchValue[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.SCHEDULING_SERVER);
            else if (processAgentType.Equals(ProcessAgentType.EXPERIMENT_STORAGE_SERVER))
                searchValue[0] = new ResourceMappingValue(ResourceMappingTypes.RESOURCE_TYPE, ProcessAgentType.EXPERIMENT_STORAGE_SERVER);
            List<ResourceMapping> found = ResourceMapManager.Find(key, searchValue);

            if (found != null && found.Count > 0)
            {
                foreach (ResourceMapping rm in found)
                {
                    for (int i = 0; i < rm.values.Length; i++)
                    {
                        if (rm.values[i].type.Equals(ResourceMappingTypes.PROCESS_AGENT))
                        {
                            paId = (int)rm.values[i].entry;
                            break;
                        }
                    }
                }

            }



            //bool foundProcessAgent = false;
            //int row = 0;

            ////the number of "set of values" (array of values) associated with this client
            //int numSetOfValues = values.GetLength(0);

            //for (row = 0; row < numSetOfValues && !foundProcessAgent; row++)
            //{
            //    int numValues = values[row].Length;
            //    for (int column = 0; column < numValues; column++)
            //    {
            //        if (values[row][column].Equals(searchValue))
            //        {
            //            foundProcessAgent = true;
            //            break;
            //        }
            //    }
            //}

            //if (foundProcessAgent)
            //{
                
            //    ResourceMappingValue[] associatedPA = values[row - 1];
            //    for (int i = 0; i < associatedPA.Length; i++)
            //    {
            //        if (associatedPA[i].Type.Equals(ResourceMappingTypes.PROCESS_AGENT))
            //        {
            //            paId = (int)associatedPA[i].Entry;
            //            break;
            //        }
            //    }
            //}
            
            return paId;

                
        }
        
        /// <summary>
        /// Checks if an array of Resource Mapping values is Equal to another one
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public bool EqualMappingValues(ResourceMappingValue[] v1, ResourceMappingValue[] v2)
        {
            int num1Values = v1.GetLength(0);
            int num2Values = v2.GetLength(0);

            if (num1Values != num2Values)
                return false;

            bool areNotEqual = false;
            bool areEqual = false;

            for (int i = 0; i < num1Values; i++)
            {
                if (!v1[i].Equals(v2[i]))
                {
                    areNotEqual = true;
                    break;
                }
            }

            //for (int i = 0; i < num1Values; i++)
            //{
            //    areEqual = false;

            //    for (int j = 0; j < num2Values; j++)
            //    {
            //        if (v1[i].Equals(v1[j]))
            //        {
            //            areEqual = true;
            //            break;
            //        }
            //    }

            //    if (areEqual == false)
            //        break;
            //}

            //return (areEqual);

            return (!areNotEqual);
        }

        // THis is not supported, an attempt to re-do resources using int's instead of strings
        //public int InsertResourceMap(int keyType, int keyValue,
        //    int type0, object value0, int type1, object value1, int type2, object value2)
        //{
        //    int id = -1;
        //    DbConnection connection  = CreateConnection();
        //      // command executes the "InsertResourceMap" stored procedure
        //        DbCommand cmd = FactoryDB.CreateCommand("InsertResourceMap", connection);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        DbParameter paramType = null;
        //        DbParameter paramValue = null;

        //        // Need to do this in pairs 
        //        paramType = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@keyType", DbType.Int);
        //        paramValue = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@keyValue", DbType.Int);
        //        paramType.Value = keyType; 
        //        paramValue.Value = keyValue;
        //        paramType = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@Type0", DbType.Int);
        //        paramValue = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@value0", DbType.Int);
        //        paramType.Value = type0;
        //        paramValue.Value = value0;
            
        //    paramType = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@type1", DbType.Int);
        //    paramValue = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@value1", DbType.Int);
        //    if(ResourceMap.IsResourceMapType(type1)){
        //        paramType.Value = type1;   
        //        paramValue.Value = value1;
        //    }
        //    else{
        //        paramType.Value = DBNull.Value;   
        //        paramValue.Value =  DBNull.Value;
        //    }
        //    paramType = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@type2", DbType.Int);
        //    paramValue = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@value2", DbType.Int);
        //   if(ResourceMap.IsResourceMapType(type2)){
        //        paramType.Value = type2;   
        //        paramValue.Value = value2;
        //    }
        //    else{
        //        paramType.Value = DBNull.Value;   
        //        paramValue.Value =  DBNull.Value;
        //    }
        //    try
        //    {
        //        // execute the command
        //        id = Convert.ToInt32(cmd.ExecuteScalar());
        //    }
        //    catch (DbException e)
        //    {
        //        writeEx(e);
        //        throw;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return id;
        //}

           public string ResourceMappingToString(ResourceMapping mapping)
        {
            StringBuilder s = new StringBuilder();
            //s.Append(mapping.MappingID + " ");
             s.Append(GetMappingEntryString(mapping.key,true) + "-> ");

            //if (mapping.values.Length > 1)
            //    s.Append("(");

            // print all values except last
             for (int i = 0; i < mapping.values.Length; i++)
             {
                 if (i > 0 && i < mapping.values.Length)
                     s.Append(", ");
                 s.Append(GetMappingEntryString(mapping.values[i],true));

             }

            //// print last value
            //if (mapping.values[mapping.values.Length - 1].Type.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
            //    s.Append("(" + mapping.values[mapping.values.Length - 1] + ":" + mapping.values[mapping.values.Length - 1].Entry + ")");
            //else
            //    s.Append("(" + mapping.values[mapping.values.Length - 1].TypeName + ":" + GetMappingEntryString(mapping.values[mapping.values.Length - 1]) + ")");

            //if (mapping.values.Length > 1)
            //    s.Append(")");
            return s.ToString();
        }



        public string GetMappingEntryString(ResourceMappingEntry entry, bool showType)
        {
            StringBuilder buf = new StringBuilder();
            Object o = entry.Entry;

            if (entry == null)
            {
                buf.Append("Entry is null, NOT FOUND");
            }
            else if (entry.Type.Equals(ResourceMappingTypes.PROCESS_AGENT))
            {
                string name = null;
                //if (showType)
                //    name = GetProcessAgentNameWithType((int)o);
                //else
                    name = GetProcessAgentName((int)o);
                if (name != null)
                    buf.Append(name);
                else
                    buf.Append("Process Agent not found");
            }
            else if (entry.Type.Equals(ResourceMappingTypes.CLIENT))
            {
                LabClient[] labClients = AdministrativeAPI.GetLabClients(new int[] { (int)o });
                if (labClients.Length == 1)
                {
                    if (showType)
                        buf.Append("Client: ");
                    buf.Append(labClients[0].ClientName);

                }
                else
                {
                    buf.Append("Client not found");
                }
            }

            else if (entry.Type.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
            {
                if (showType)
                    buf.Append("RM: ");
                buf.Append(ResourceMappingToString(GetResourceMapping((int)o)));
            }
            else if (entry.Type.Equals(ResourceMappingTypes.STRING))
            {
                if (showType)
                    buf.Append("String: ");
                buf.Append((string)o);
            }
            else if (entry.Type.Equals(ResourceMappingTypes.RESOURCE_TYPE))
            {
                if (showType)
                    buf.Append("RT: ");
                string type = (string)o;
                if(type.Equals(ProcessAgentType.EXPERIMENT_STORAGE_SERVER))
                    buf.Append("ESS");
                else if(type.Equals(ProcessAgentType.LAB_SCHEDULING_SERVER))
                     buf.Append("LSS");
                 else if(type.Equals(ProcessAgentType.SCHEDULING_SERVER))
                     buf.Append("USS");
                else if(type.Equals(ProcessAgentType.LAB_SERVER))
                     buf.Append("LS");
                else{
                    buf.Append(type);
                }
            }
            else if (entry.Type.Equals(ResourceMappingTypes.TICKET_TYPE))
            {
                if (showType)
                    buf.Append("TT: ");
                buf.Append(((TicketType)o).shortDescription);
            }
            else if (entry.Type.Equals(ResourceMappingTypes.GROUP))
            {

                Group[] groups = AdministrativeAPI.GetGroups(new int[] { (int)o });
                if (groups.Length == 1)
                {

                    if (showType)
                        buf.Append("Group: ");
                    buf.Append(groups[0].GroupName);
                }
                else
                {
                    buf.Append("Group not found");
                }
            }
            if (buf.Length == 0)
            {
                buf.Append("Entry not Found");
            }
            return buf.ToString(); ;
        }


        //public string GetMappingString(ResourceMapping mapping)
        //{
        //    String s = mapping.MappingID.ToString();
        //    s += " (" + mapping.key.TypeName + ":" + GetMappingEntryString(mapping.key) + ")";
        //    s += "-->";

        //    if (mapping.values.Length > 1)
        //        s += "(";

        //    // print all values except last
        //    for (int i = 0; i < mapping.values.Length - 1; i++)
        //    {
        //        ResourceMappingValue value = mapping.values[i];
        //        if (value.Type.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
        //            s += "(" + value.TypeName + ":" + value.Entry + "), ";
        //        else
        //            s += "(" + value.TypeName + ":" + GetMappingEntryString(value) + "), ";
        //    }

        //    // print last value
        //    if (mapping.values[mapping.values.Length - 1].Type.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
        //        s += "(" + mapping.values[mapping.values.Length - 1] + ":" + mapping.values[mapping.values.Length - 1].Entry + ")";
        //    else
        //        s += "(" + mapping.values[mapping.values.Length - 1].TypeName + ":" + GetMappingEntryString(mapping.values[mapping.values.Length - 1]) + ")";

        //    if (mapping.values.Length > 1)
        //        s += ")";
        //    return s;
        //}
/*
        public int InsertRegisterRecord(int couponId, string couponGuid, string registerGuid,
            string sourceGuid, int status, string email, string descriptor)
        {
            DbConnection connection = CreateConnection();
            DbCommand cmd = FactoryDB.CreateCommand("InsertRegistration", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            DbParameter idParam = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@couponId", DbType.Int );
            idParam.Value = couponId;
            DbParameter couponGuidParam = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@couponGuid", DbType.AnsiString,50 );
            couponGuidParam.Value = couponGuid;
            DbParameter registerGuidParam = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@registerGuid", DbType.Varchar,50);
            registerGuidParam.Value = registerGuid;
            DbParameter sourceParam = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@sourceGuid", DbType.Varchar,50);
            sourceParam.Value = sourceGuid;
            DbParameter statusParam = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@status", DbType.Int );
            statusParam.Value = status;
            DbParameter emailParam = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@email", DbType.AnsiString,256 );
            emailParam.Value = email;
            DbParameter descriptorParam = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@descriptor", DbType.Text );
            descriptorParam.Value = descriptor;
            try
            {
            
                return Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception e)
            {
                Utilities.WriteLog(e.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        public string[] SelectRegisterGuids()
        {
            DbConnection connection = CreateConnection();
            DbCommand cmd = FactoryDB.CreateCommand("InsertRegistration", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            DbParameter Param = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@", DbType.AnsiString);
            Param.Value = (string)s;
            try
            {

                DbDataReader dataReader = null;
                dataReader = cmd.ExecuteReader();

            }
            catch (Exception e)
            {
                Utilities.WriteLog(e.Message);
            }
            finally
            {
                connection.Close();
            }


        }

        protected RegisterRecord readRegisterRecord(DbDataReader reader){
            RegisterRecord record = new RegisterRecord();
            
            record.recordId = reader.GetInt32(0);
            if(!reader.IsDBNull(1))
                record.couponId = reader.GetInt32(1);
            if(!reader.IsDBNull(2))
                record.couponGuid = reader.GetString(2);
            if(!reader.IsDBNull(3))
                record.registerGuid = reader.GetString(3);
            if(!reader.IsDBNull(4))
                record.sourceGuid = reader.GetString(4);
            if(!reader.IsDBNull(5))
                record.status = reader.GetInt32(5);
           record.create =  DateUtil.SpecifyUTC(reader.GetDateTime(6));
             record.lastModified = DateUtil.SpecifyUTC(reader.GetDateTime(7));
             if(!reader.IsDBNull(8))
                record.descriptor = reader.GetString(8);
             if(!reader.IsDBNull(9))
                record.email = reader.GetString(9);
            return record;
        }

        public RegisterRecord SelectRegisterRecord(int id)
        {
            RegisterRecord record = null;
            DbConnection connection = CreateConnection();
            DbCommand cmd = FactoryDB.CreateCommand("SelectRegistrationRecord", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            DbParameter Param = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@id", DbType.Int);
            Param.Value = id;
            try
            {
                DbDataReader dataReader = null;
                dataReader = cmd.ExecuteReader();
                while(dataReader.Read()){
                    record = readRegisterRecord(dataReader);


            }
            catch (Exception e)
            {
                Utilities.WriteLog(e.Message);
            }
            finally
            {
                connection.Close();
            }


        }

        public RegisterRecord[] SelectRegister(string registerGuid)
        {
            DbConnection connection = CreateConnection();
            DbCommand cmd = FactoryDB.CreateCommand("InsertRegistration", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            DbParameter Param = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@", DbType.AnsiString);
            Param.Value = (string)s;
            try
            {

                return Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception e)
            {
                Utilities.WriteLog(e.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        public RegisterRecord[] SelectRegisterByStatus(int status)
        {
            DbConnection connection = CreateConnection();
            DbCommand cmd = FactoryDB.CreateCommand("InsertRegistration", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            DbParameter Param = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@", DbType.AnsiString);
            Param.Value = (string)s;
            try
            {

                return Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception e)
            {
                Utilities.WriteLog(e.Message);
            }
            finally
            {
                connection.Close();
            }

        }
        public RegisterRecord[] SelectRegisterByStatus(int low, int high)
        {
            DbConnection connection = CreateConnection();
            DbCommand cmd = FactoryDB.CreateCommand("InsertRegistration", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            DbParameter Param = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@", DbType.AnsiString);
            Param.Value = (string)s;
            try
            {

                return Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception e)
            {
                Utilities.WriteLog(e.Message);
            }
            finally
            {
                connection.Close();
            }


        }

        public int SetRegisterStatus(int id, int status)
        {
            DbConnection connection = CreateConnection();
            DbCommand cmd = FactoryDB.CreateCommand("UpdateRegistrationStatus", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate parameters
            DbParameter idParam = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@id", DbType.Int);
            idParam.Value = id;
            DbParameter statusParam = cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@status", DbType.Int);
            statusParam.Value = status;
            try
            {

                return cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Utilities.WriteLog(e.Message);
            }

            finally
            {
                connection.Close();
            }


        }
*/

        
    }
    //public class RegisterRecord
    //{
    //    public int recordId;
    //    public int status;
    //    public int couponId;
    //    public DateTime create;
    //    public DateTime lastModified;
    //    public string couponGuid;
    //    public string registerGuid;
    //    public string sourceGuid;
    //    public string descriptor;
    //    public string email;
    //}
        
}
