using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

using iLabs.Core;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;

namespace iLabs.Scheduling.UserSide
{
	/// <summary>
	/// Summary description for DBManager.
	/// </summary>
	public class DBManager : ProcessAgentDB
	{
		

		public DBManager()
		{
		}

        /// <summary>
        /// Modifies the information related to the specified service the service's Guid must exist and the typ of service may not be modified,
        /// in and out coupons may be changed.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="inIdentCoupon"></param>
        /// <param name="outIdentCoupon"></param>
        /// <returns></returns>
        public override int ModifyDomainCredentials(string originalGuid, ProcessAgent agent,
            Coupon inCoupon, Coupon outCoupon, string extra)
        {
            int status = 0;
            try
            {
                status = base.ModifyDomainCredentials(originalGuid, agent, inCoupon, outCoupon, extra);
            }
            catch (Exception ex)
            {
                throw new Exception("USS: ", ex);
            }

            if (agent.type == ProcessAgentType.SERVICE_BROKER || agent.type == ProcessAgentType.REMOTE_SERVICE_BROKER)
            {
                //Check for SB names in credential sets
                ModifyCredentialSetServiceBroker(originalGuid, agent.agentGuid, agent.agentName);
            }
            if (agent.type == ProcessAgentType.LAB_SERVER)
            {
                // Labserver Names in Experiment info's
                ModifyExperimentLabServer(agent.agentGuid, agent.agentName);
            }
            if (agent.type == ProcessAgentType.LAB_SCHEDULING_SERVER)
            {
                // LSS path, & name in LSS_Info
                DBManager.ModifyLSSInfo(agent.agentGuid, agent.agentName, agent.webServiceUrl);
            }
            return status;

        }

        /// <summary>
        /// Modifies the information related to the specified service the service's Guid must exist and the typ of service may not be modified,
        /// in and out coupons may be changed.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="inIdentCoupon"></param>
        /// <param name="outIdentCoupon"></param>
        /// <returns></returns>
        public override int ModifyProcessAgent(string originalGuid, ProcessAgent agent, string extra)
        {
            int status = 0;
            try
            {
                status = base.ModifyProcessAgent(originalGuid, agent, extra);
            }
            catch (Exception ex)
            {
                throw new Exception("USS: ", ex);
            }

            if (agent.type == ProcessAgentType.SERVICE_BROKER || agent.type == ProcessAgentType.REMOTE_SERVICE_BROKER)
            {
                //Check for SB names in credential sets
                ModifyCredentialSetServiceBroker(originalGuid, agent.agentGuid, agent.agentName);
            }
            if (agent.type == ProcessAgentType.LAB_SERVER)
            {
                // Labserver Names in Experiment info's
                ModifyExperimentLabServer(agent.agentGuid, agent.agentName);
            }
            if (agent.type == ProcessAgentType.LAB_SCHEDULING_SERVER)
            {
                // LSS path, & name in LSS_Info
                ModifyLSSInfo(agent.agentGuid, agent.agentName, agent.webServiceUrl);
            }
            return status;

        }

 

        /* !------------------------------------------------------------------------------!
         *							CALLS FOR USSPolicy
         * !------------------------------------------------------------------------------!
         */
        /// <summary>
        /// add user side scheduling policy that governs whether a reservation request to execute an experiment at a certain time will be accept from a student with a particular credential set
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="experimentInfoId"></param>
        /// <param name="rule"></param>
        /// <returns></returns>the index ID of the USSPolicy added,>0 successfully added, ==-1 otherwise
        public static int AddUSSPolicy(string groupName, string serviceBrokerGuid, int experimentInfoId, string rule)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "addUSSPolicy" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("AddUSSPolicy", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupName", groupName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experimentInfoId", experimentInfoId, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@rule", rule, DbType.AnsiString, 2048));
            
            int i = -1;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    i = Int32.Parse(ob.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add USSPolicy", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;

        }
        /// <summary>
        /// delete the user side scheduling policies specified by the ussPolicyIds
        /// </summary>
        /// <param name="ussPolicyIds"></param>
        /// <returns></returns>the IDs of all UssPolicies not successfully removed

        public static int[] RemoveUSSPolicy(int[] ussPolicyIds)
        {
            ArrayList arrayList = new ArrayList();
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "deleteUSSPolicy" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("DeleteUSSPolicy", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@ussPolicyId", null, DbType.Int32));
            // execute the command
            try
            {
                connection.Open();
                //populate the parameters
                foreach (int ussPolicyId in ussPolicyIds)
                {
                    cmd.Parameters["@ussPolicyId"].Value = ussPolicyId;
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        arrayList.Add(ussPolicyId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in remove usspolicies", ex);
            }
            finally
            {
                connection.Close();
            }
            int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
            return uIDs;
        }
        /// <summary>
        /// update the data fields for the user side scheduling policy specified by the ussPolicyId
        /// </summary>
        /// <param name="ussPolicyId"></param>
        /// <param name="experimentInfoId"></param>
        /// <param name="rule"></param>
        /// <returns></returns>if updated succesfully, return ture, otherwise, return false

        public static bool ModifyUSSPolicy(int ussPolicyId, int experimentInfoId, string rule, int credentialSetId)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "modifyUSSPolicy" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ModifyUSSPolicy", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@ussPolicyId", ussPolicyId, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experimentInfoID", experimentInfoId, DbType.Int32));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@rule", rule, DbType.AnsiString, 2048));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@credentialSetId", credentialSetId, DbType.Int32));
           
            bool i = false;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                int m = 0;
                if (ob != null && ob != System.DBNull.Value)
                {
                    m = Int32.Parse(ob.ToString());
                }
                if (m != 0)
                {
                    i = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add ModifyUSSPolicy", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;

        }

        /// <summary>
        /// enumerates all IDs of the User side Scheduling Policies 
        /// </summary>
        /// <returns></returns>an array of ints containing the IDs of all the USSPolicies
        public static int[] ListUSSPolicyIDs()
        {
            int[] ussPolicyIds;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveUSSPolicyIDsByGroup" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrieveUSSPolicyIDs", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the ussPolicyIds retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["USS_Policy_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["USS_Policy_ID"]));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve USSPolicyIDs", ex);
            }
            finally
            {
                connection.Close();
            }
            ussPolicyIds = Utilities.ArrayListToIntArray(arrayList);
            return ussPolicyIds;
        }
        /// <summary>
        /// enumerates all IDs of the User side Scheduling Policies applying for the users with a particular credential set identified by the combination of groupName and serviceBrokerID
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the USSPolicies applying for the users with a particular credential set
        public static int[] ListUSSPolicyIDsByGroup(string groupName, string serviceBrokerGuid)
        {
            int[] ussPolicyIds;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveUSSPolicyIDsByGroup" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrieveUSSPolicyIDsByGroup", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupName", groupName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString, 50));

            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the ussPolicyIds retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["USS_Policy_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["USS_Policy_ID"]));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve USSPolicyIDs by group", ex);
            }
            finally
            {
                connection.Close();
            }
            ussPolicyIds = Utilities.ArrayListToIntArray(arrayList);
            return ussPolicyIds;
        }

        /// <summary>
        /// enumerates the ID of the User side Scheduling Policies applying for the users with a particular credential set identified by the combination of groupName and serviceBrokerID to execute a particular experiment
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the USSPolicies applying for the users with a particular credential set to execute a particular experiment
        public static int[] ListUSSPolicyIDsByGroupAndExperiment(string groupName, string serviceBrokerGuid, string clientGuid, string labServerGuid)
        {
            int[] ussPolicyIds;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveUSSPolicyIDsByGroupandExp" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrieveUSSPolicyIDsByGroupandExp", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupName", groupName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@clientGuid", clientGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerGuid", labServerGuid, DbType.AnsiString, 50));
            
            // execute the command
            DbDataReader dataReader = null;
            ArrayList arrayList = new ArrayList();
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                //store the ussPolicyIds retrieved in arraylist

                while (dataReader.Read())
                {
                    if (dataReader["USS_Policy_ID"] != System.DBNull.Value)
                        arrayList.Add(Convert.ToInt32(dataReader["USS_Policy_ID"]));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve USSPolicyIDs by group and experiment", ex);
            }
            finally
            {
                connection.Close();
            }
            ussPolicyIds = Utilities.ArrayListToIntArray(arrayList);
            return ussPolicyIds;
        }
        /// <summary>
        /// returns an array of the immutable USSPolicy objects that correspond to the supplied ussPolicyIds
        /// </summary>
        /// <param name="ussPolicyIds"></param>
        /// <returns></returns>
        public static USSPolicy[] GetUSSPolicies(int[] ussPolicyIds)
        {
            USSPolicy[] ussPolicies = new USSPolicy[ussPolicyIds.Length];
            for (int i = 0; i < ussPolicyIds.Length; i++)
            {
                ussPolicies[i] = new USSPolicy();
            }
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveUSSPolicyByID" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrieveUSSPolicyByID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@ussPolicyId", null, DbType.Int32));
            //execute the command
            try
            {
                connection.Open();
                for (int i = 0; i < ussPolicyIds.Length; i++)
                {
                    // populate the parameters
                    cmd.Parameters["@ussPolicyId"].Value = ussPolicyIds[i];
                    DbDataReader dataReader = null;
                    dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        ussPolicies[i].ussPolicyId = ussPolicyIds[i];
                        if (dataReader[0] != System.DBNull.Value)
                            ussPolicies[i].experimentInfoId = (int)dataReader.GetInt32(0);
                        if (dataReader[1] != System.DBNull.Value)
                            ussPolicies[i].rule = dataReader.GetString(1);
                        if (dataReader[2] != System.DBNull.Value)
                            ussPolicies[i].credentialSetId = (int)dataReader.GetInt32(2);
                    }
                    dataReader.Close();
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in get usspolicies", ex);
            }
            finally
            {
                connection.Close();
            }
            return ussPolicies;
        }
	    	/* !------------------------------------------------------------------------------!
			 *							CALLS FOR ReservationInfo
			 * !------------------------------------------------------------------------------!
			 */
        /// <summary>
		/// add reservation by user
		/// </summary>
		/// <param name="userName"></param>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
		/// <param name="experimentInfoId"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>the unique id which identifies the reservation added by the user,>0 successfully added, ==-1 otherwise
        public static int AddReservation(string userName, string serviceBrokerGuid, string groupName,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            int experId = DBManager.ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
            if (experId > 0)
            {
                return DBManager.AddReservation(userName, serviceBrokerGuid, groupName,
                   experId, startTime, endTime);
            }
            else return 0;
        }
		/// <summary>
		/// add reservation by user
		/// </summary>
		/// <param name="userName"></param>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
		/// <param name="experimentInfoId"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>the unique id which identifies the reservation added by the user,>0 successfully added, ==-1 otherwise
        public static int AddReservation(string userName, string serviceBrokerGuid, string groupName, int experimentInfoId, DateTime startTime, DateTime endTime)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "AddReservation" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("AddReservation",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@userName", userName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupName", groupName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experimentInfoId", experimentInfoId, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@startTime", startTime, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@endTime", endTime, DbType.DateTime));
			
			int i=-1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add ReservationInfo",ex);
			}
			finally
			{
				connection.Close();
			}	
			return i;		
		}
		/// <summary>
		/// delete the reservations specified by the reservationIDs
		/// </summary>
		/// <param name="reservationIDs"></param>
		/// <returns></returns>an array of ints containing the IDs of all reservation not successfully removed
		public static int[] RemoveReservation(int[] reservationIDs)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteReservation" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("DeleteReservation",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@reservationID", null, DbType.Int32));
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int reservationID in reservationIDs)
				{
					cmd.Parameters["@reservationID"].Value = reservationID;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(reservationID);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove reservations",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}
/// <summary>
/// updates the data fields for the reservation specified by the reservationID
/// </summary>
/// <param name="reservationID"></param>
/// <param name="experimentInfoId"></param>
/// <param name="startTime"></param>
/// <param name="endTime"></param>
/// <returns></returns>ture if updated successfully, false otherwise
		public static bool ModifyReservation(int reservationID,int experimentInfoId,DateTime startTime,DateTime endTime)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "ModifyReservation" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("ModifyReservation",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@reservationID", reservationID, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experimentInfoId", experimentInfoId, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@startTime", startTime, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@endTime", endTime, DbType.DateTime));
			
			bool i=false;
			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
				int m=0;
                if (ob != null && ob != System.DBNull.Value)
				{
					 m = Int32.Parse(ob.ToString());
				}
				if (m!=0)
				{
					i=true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add ModifyReservation",ex);
			}
			finally
			{
				connection.Close();
			}		
			return i;
             
		}
		/// <summary>
		/// enumerates all IDs of the reservations made on a particular experiment by a particular user identified by the combination of userName and serviceBrokerID
		/// </summary>
		/// <param name="userName"></param>
        /// <param name="serviceBrokerGuid"></param>
		/// <returns></returns>an array of ints containing the IDs of all the reservation made by the specified user

        public static int[] ListReservationIDsByUser(string userName, string serviceBrokerGuid, int experimentInfoId)
		{
			int[] reservationIDs;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveReservationIDsByUser" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveReservationIDsByUser", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@userName", userName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experimentInfoId", experimentInfoId, DbType.Int32));
			
			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the reservationIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Reservation_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Reservation_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve reservationIDs by user",ex);
			}
			finally
			{
				connection.Close();
			}
			reservationIDs=Utilities.ArrayListToIntArray(arrayList);
			return reservationIDs;
		}
		/// <summary>
		/// enumerates all IDs of the reservations with a credential sets identified by the combination of groupName and serviceBrokerID
		/// </summary>
		/// <param name="groupName"></param>
        /// <param name="serviceBrokerGuid"></param>
		/// <returns></returns>an array of ints containing the IDs of all the reservations for the specified lab server made by the specified user
        public static int[] ListReservationIDsByGroup(string groupName, string serviceBrokerGuid)
		{
			int[] reservationIDs;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveReservationIDsByGroup" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveReservationIDsByGroup", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupName", groupName, DbType.String,256));			
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString,50));
           
			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the reservationIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Reservation_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Reservation_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve reservationIDs by group",ex);
			}
			finally
			{
				connection.Close();
			}
			reservationIDs=Utilities.ArrayListToIntArray(arrayList);
			return reservationIDs;
		}

		/// <summary>
		/// enumerates all IDs of the reservations made on the particular lab server during a paticular time period
		/// </summary>
        /// <param name="labServerGuid"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>/* An array of ints containing the IDs of the reservations made on the particular lab server during a particular time period 
        public static int[] ListReservationIDsByLabServer(string labServerGuid, DateTime startTime, DateTime endTime)
		{
			int[] reservationIDs;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveReservationIDsByLabServer" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveReservationIDsByLabServer", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerGUID", labServerGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@startTime", startTime, DbType.DateTime));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@endTime", endTime, DbType.DateTime));
			
			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the reservationIDs retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Reservation_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Reservation_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve reservationIDs by group",ex);
			}
			finally
			{
				connection.Close();
			}
			reservationIDs=Utilities.ArrayListToIntArray(arrayList);
			return reservationIDs;
			
		}
        /// <summary>
        /// retrieve the ID of the reservation on a particular experiment for a particular user at a specific time 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="labClientName"></param>
        /// <param name="labClientVersion"></param>
        /// <param name="time"></param>
        /// <returns></returns>the ID of the reservation if the requested reservation does exist, -1 otherwise
        public static int ListReservationIDByUser(string userName, string serviceBrokerGuid, string clientGuid, string labServerGuid, DateTime time)
		{
			int i= 0;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveReservationIDByUser" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveReservationIDByUser", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@userName", userName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGuid", serviceBrokerGuid,DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@clientGuid", clientGuid, DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerGuid", labServerGuid, DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@timeUTC", time, DbType.DateTime));
			
			// execute the command
			try 
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve reservationIDs by group",ex);
			}
			finally
			{
				connection.Close();
			}
			
			return i;
			
		}


		/// <summary>
		/// returns an array of the immutable reservation objects that correspond to the supplied reservation IDs
		/// </summary>
		/// <param name="reservationIDs"></param>
		/// <returns></returns>
		public static ReservationInfo[] GetReservations(int[] reservationIDs)
		{
			ReservationInfo[] reservations=new ReservationInfo[reservationIDs.Length];
			for(int i=0; i<reservationIDs.Length;i++)
			{
				reservations[i]=new  ReservationInfo();
			}
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveReservationByID" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveReservationByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@reservationID", null, DbType.Int32));
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0;i<reservationIDs.Length;i++)
				{
					// populate the parameters
					cmd.Parameters["@reservationID"].Value = reservationIDs[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
					{	
						reservations[i].reservationId=reservationIDs[i];
						if(dataReader[0] != System.DBNull.Value )
							reservations[i].userName=dataReader.GetString(0);
						if(dataReader[1] != System.DBNull.Value )
							reservations[i].startTime= DateUtil.SpecifyUTC(dataReader.GetDateTime(1));
						if(dataReader[2] != System.DBNull.Value )
                            reservations[i].endTime = DateUtil.SpecifyUTC(dataReader.GetDateTime(2));
						if(dataReader[3] != System.DBNull.Value )
							reservations[i].credentialSetId=(int)dataReader.GetInt32(3);
						if(dataReader[4] != System.DBNull.Value )
							reservations[i].experimentInfoId=(int)dataReader.GetInt32(4);
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get reservations",ex);
			}
			finally
			{
				connection.Close();
			}	
			return reservations;
		}

        public static ReservationInfo[] GetReservations(string sbGuid, string userName, string groupName,
            string lsGuid, string clientGuid, DateTime start, DateTime end)
        {
            int experimentId = DBManager.ListExperimentInfoIDByExperiment(lsGuid, clientGuid);
            int credId = -1;
            if(groupName != null && groupName.Length >0)
                credId = DBManager.GetCredentialSetID(sbGuid, groupName);
            return DBManager.GetReservations(userName, experimentId, credId, start, end);
        }

		/// <summary>
		/// to select reservation accorrding to given criterion
		/// </summary>
        public static ReservationInfo[] GetReservations(string userName, int experimentInfoId, int credentialSetId, DateTime start, DateTime end)
        {
            List<ReservationInfo> reservations = new List<ReservationInfo>();
            int action = 0;
            if (experimentInfoId > 0)
                action |= 1;
            if (credentialSetId > 0)
                action |= 2;
            if (userName != null && userName.Length > 0)
                action |= 4;

            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();
            DbCommand cmd = null;
            DbParameter expParam = null;
            DbParameter credParam = null;
            DbParameter userParam = null;
            // create sql command
            switch (action)
            {
                case 0:
                    cmd = FactoryDB.CreateCommand("RetrieveReservations", connection);
                    break;
                case 1:
                    cmd = FactoryDB.CreateCommand("RetrieveReservationsByExperiment", connection);
                    break;
                case 2:
                    cmd = FactoryDB.CreateCommand("RetrieveReservationsByGroup", connection);
                    break;
                case 3:
                    cmd = FactoryDB.CreateCommand("RetrieveReservationsByGroupAndExperiment", connection);
                    break;
                case 4:
                    cmd = FactoryDB.CreateCommand("RetrieveReservationsByUser", connection);
                    break;
                case 5:
                    throw new Exception("Wrong combination of arguments in selectReservations 5");
                    // NOT Implemented cmd = FactoryDB.CreateCommand("RetrieveReservationsByExperiment", connection);
                    break;
                case 6:
                    cmd = FactoryDB.CreateCommand("RetrieveReservationsByUserAndGroup", connection);
                    break;
                case 7:
                    cmd = FactoryDB.CreateCommand("RetrieveReservationsByAll", connection);
                    break;
                default:
                    throw new Exception("Wrong combination of arguments in selectReservations");
                    break;
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@start", start, DbType.DateTime));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@end", end, DbType.DateTime));
            switch (action)
            {
                case 0:
                    break;
                case 1:
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experInfoID", experimentInfoId, DbType.Int32));
                    break;
                case 2:
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@credSetID", credentialSetId, DbType.Int32));
                    break;
                case 3:
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experInfoID", experimentInfoId, DbType.Int32));
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@credSetID", credentialSetId, DbType.Int32));
                    break;
                case 4:
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd, "@userName", userName, DbType.String, 256));
                    break;
                case 5:
                    break;
                case 6:
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@credSetID", credentialSetId,DbType.Int32));
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd, "@userName", userName, DbType.String, 256));
                    break;
                case 7:
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experInfoID", experimentInfoId, DbType.Int32));
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@credSetID", credentialSetId, DbType.Int32));
                    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@userName", userName, DbType.String, 256));
                    break;
                default:
                    break;
            }
            DbDataReader dataReader = null;
            try
            {
                connection.Open();
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    ReservationInfo reservation = new ReservationInfo();
                    reservation.reservationId = dataReader.GetInt32(0); ;
                    if (dataReader[1] != System.DBNull.Value)
                        reservation.userName = dataReader.GetString(1);
                    if (dataReader[2] != System.DBNull.Value)
                        reservation.startTime = DateUtil.SpecifyUTC(dataReader.GetDateTime(2));
                    if (dataReader[3] != System.DBNull.Value)
                        reservation.endTime = DateUtil.SpecifyUTC(dataReader.GetDateTime(3));
                    if (dataReader[4] != System.DBNull.Value)
                        reservation.credentialSetId = (int)dataReader.GetInt32(4);
                    if (dataReader[5] != System.DBNull.Value)
                        reservation.experimentInfoId = (int)dataReader.GetInt32(5);
                    reservations.Add(reservation);
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
            return reservations.ToArray();
        }

		    /* !------------------------------------------------------------------------------!
			 *							CALLS FOR Experiment Information
			 * !------------------------------------------------------------------------------!
			 * */
		/// <summary>
		/// add information of a particular experiment
		/// </summary>
        /// <param name="labServerGuid"></param>
		/// <param name="labServerName"></param>
		/// <param name="labClientVersion"></param>
		/// <param name="labClientName"></param>
		/// <param name="providerName"></param>
        /// <param name="lssGuid"></param>
		/// <returns></returns>the unique ID which identifies the experiment added,>0 successfully added,==-1 otherwise
        public static int AddExperimentInfo(string labServerGuid, string labServerName,
            string labClientGuid, string labClientName, string labClientVersion, string providerName, string lssGuid)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "AddExperimentInfo" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("AddExperimentInfo",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labClientGUID", labClientGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerGUID", labServerGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerName", labServerName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labClientVersion", labClientVersion, DbType.String,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labClientName", labClientName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@providerName", providerName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssGUID", lssGuid, DbType.AnsiString,50));
           
			int i=-1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add Experiment Information",ex);
			}
			finally
			{
				connection.Close();
			}	
			return i;		
		}
/// <summary>
/// delete the experiment information specified by the experimentInfoIds
/// </summary>
/// <param name="experimentInfoIds"></param>
/// <returns></returns>an array of ints containing the IDs of all experiments not successfully removed
		public static int[] RemoveExperimentInfo(int[] experimentInfoIds)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteExperimentInfo" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("DeleteExperimentInfo",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experimentInfoId", null, DbType.Int32));
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int experimentInfoId in experimentInfoIds)
				{
					cmd.Parameters["@experimentInfoId"].Value = experimentInfoId;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(experimentInfoId);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove experiment infomation",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}
		/// <summary>
		///Enumerates IDs of all the experimentInfos 
		/// </summary>
		/// <returns></returns> An array of ints containing the IDs of all the experimentInfos 
		public static int[] ListExperimentInfoIDs()
		{
			int[] experimentInfoIds;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveExperimentIDs" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveExperimentIDs", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the experimentInfoIds retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Experiment_Info_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Experiment_Info_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve experimentInfoIds",ex);
			}
			finally
			{
				connection.Close();
			}
			experimentInfoIds=Utilities.ArrayListToIntArray(arrayList);
			return experimentInfoIds;
		}
		/// <summary>
		/// Returns an array of the immutable ExperimentInfo objects that correspond to the supplied experimentInfo IDs. 
		/// </summary>
		/// <param name="experimentInfoIds"></param>
		/// <returns></returns>
		public static UssExperimentInfo[] GetExperimentInfos(int[] experimentInfoIds)
		{
			UssExperimentInfo[] experimentInfos=new UssExperimentInfo[experimentInfoIds.Length];
			for(int i=0; i<experimentInfoIds.Length;i++)
			{
				experimentInfos[i]=new  UssExperimentInfo();
			}
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveExperimentInfoByID" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveExperimentInfoByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experimentInfoId", null, DbType.Int32));
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0;i<experimentInfoIds.Length;i++)
				{
					// populate the parameters
					cmd.Parameters["@experimentInfoId"].Value = experimentInfoIds[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
					{	
						experimentInfos[i].experimentInfoId=experimentInfoIds[i];
                        if (dataReader[0] != System.DBNull.Value)
                            experimentInfos[i].labClientGuid = dataReader.GetString(0);
						if(dataReader[1] != System.DBNull.Value )
							experimentInfos[i].labServerGuid=dataReader.GetString(1);
						if(dataReader[2] != System.DBNull.Value )
							experimentInfos[i].labServerName=dataReader.GetString(2);
						if(dataReader[3] != System.DBNull.Value )
							experimentInfos[i].labClientVersion=dataReader.GetString(3);
						if(dataReader[4] != System.DBNull.Value )
							experimentInfos[i].labClientName=dataReader.GetString(4);
						if(dataReader[5] != System.DBNull.Value )
							experimentInfos[i].providerName=dataReader.GetString(5);
						if(dataReader[6] != System.DBNull.Value )
							experimentInfos[i].lssGuid=dataReader.GetString(6);
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get experimentInfo",ex);
			}
			finally
			{
				connection.Close();
			}	
			return experimentInfos;
		}
		/// <summary>
		/// enumerates the ID of the information of a particular experiment specified by labClientName and labClientVersion
		/// </summary>
		/// <param name="labClientName"></param>
		/// <param name="labClientVersion"></param>
		/// <returns></returns>the ID of the information of a particular experiment, -1 if such a experiment info can not be retrieved
		public static int ListExperimentInfoIDByExperiment(string labServerGuid, string clientGuid)
		{
			int experimentInfoId=-1;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveExperimentInfoIDByExperiment" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveExperimentInfoIDByExperiment", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@clientGuid", clientGuid, DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerGuid", labServerGuid, DbType.AnsiString,50));
			
			// execute the command
			
			try 
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					experimentInfoId=Int32.Parse(ob.ToString());
				}
				
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve experimentInfoIds by experiment",ex);
			}
			finally
			{
				connection.Close();
			}
			return experimentInfoId;
		}
		/// <summary>
		/// enumerates the URL of LSS which is in charge of a particular experiment specified by labClientName and labClientVersion
		/// </summary>
		/// <param name="labClientName"></param>
		/// <param name="labClientVersion"></param>
		/// <returns></returns>the url of the requested LSS, null if such a experiment info can not be retrieved
		public static string ListLssUrlByExperiment(string clientGuid,string labServerGuid)
		{
			string url = null;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveLSSURLbyExperiment" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveLSSURLbyExperiment", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@clientGuid", clientGuid, DbType.AnsiString,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerGuid", labServerGuid, DbType.AnsiString,50));
			
			// execute the command
			
			try 
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					url = ob.ToString();
				}
				
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve LSS url by experiment",ex);
			}
			finally
			{
				connection.Close();
			}
			return url;
		}
        /// <summary>
        ///  enumerates the GUID of LSS which is in charge of a particular experiment specified by labClientName and labClientVersion
        /// </summary>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>
        public static string ListLssIdByExperiment(string clientGuid, string labServerGuid)
        {
            string lssID = null;
            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            // command executes the "RetrieveLSSURLbyExperiment" stored procedure
            DbCommand cmd = FactoryDB.CreateCommand("RetrieveLSSIDbyExperiment", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            // populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@clientGuid", clientGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerGuid", labServerGuid, DbType.AnsiString, 50));

            // execute the command

            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    lssID = ob.ToString();
                }

            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in retrieve LSS url by experiment", ex);
            }
            finally
            {
                connection.Close();
            }
            return lssID;
        }
/// <summary>
/// modify the experimentInfo
/// </summary>
/// <param name="experimentInfoId"></param>
/// <param name="labServerGuid"></param>
/// <param name="labServerName"></param>
/// <param name="labClientVersion"></param>
/// <param name="labClientName"></param>
/// <param name="providerName"></param>
        /// <param name="lssGuid"></param>
/// <returns></returns>true modified successfully, false otherwise
        public static int ModifyExperimentInfo(int experimentInfoId, string labServerGuid, string labServerName, 
            string labClientGuid, string labClientName, string labClientVersion, string providerName, string lssGuid)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "ModifyExperimentInfo" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("ModifyExperimentInfo",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@experimentInfoId", experimentInfoId,DbType.Int32));
		    cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labClientGUID", labClientGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerGUID", labServerGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerName", labServerName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labClientVersion", labClientVersion, DbType.String,50));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labClientName", labClientName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@providerName", providerName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssGUID", lssGuid, DbType.AnsiString,50));
            
			int i=0;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
		
                if (ob != null && ob != System.DBNull.Value)
				{
					i = Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add ModifyExperimentInfo",ex);
			}
			finally
			{
				connection.Close();
			}		
			return i;
             
		}

        /// <summary>
        /// modify the experimentInfo
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="labServerName"></param>
        /// <param name="labClientVersion"></param>
        /// <param name="labClientName"></param>
        /// <param name="providerName"></param>
        /// <param name="lssGuid"></param>
        /// <returns></returns>true modified successfully, false otherwise
        public static int ModifyExperimentInfo(string labServerGuid, string labServerName,
            string labClientGuid, string labClientName, string labClientVersion, string providerName, string lssGuid)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "ModifyExperimentInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ModifyExperimentInfoByGuid", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labClientGUID", labClientGuid,DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerGUID", labServerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labServerName", labServerName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labClientVersion", labClientVersion, DbType.String, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@labClientName", labClientName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@providerName", providerName, DbType.String, 25));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssGUID", lssGuid, DbType.AnsiString, 50));
            
            int i = 0;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                
                if (ob != null && ob != System.DBNull.Value)
                {
                   i = Int32.Parse(ob.ToString());
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add ModifyExperimentInfo", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;

        }


        /// <summary>
        /// modify the LabServer name
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="labServerName"></param>
        /// <returns></returns>true modified successfully, false otherwise
        public static int ModifyExperimentLabServer(string labServerGuid, string labServerName)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "ModifyExperimentInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ModifyExperimentLabServer", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters

            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd, "@labServerGUID", labServerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd, "@labServerName", labServerName, DbType.String, 256));
            
            int count = -1;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
               
                if (ob != null && ob != System.DBNull.Value)
                {
                   count = Convert.ToInt32(ob);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add ModifyExperimentLabServer", ex);
            }
            finally
            {
                connection.Close();
            }
            return count;

        }
        
		/* !------------------------------------------------------------------------------!
			 *							CALLS FOR LSSInfo
			 * !------------------------------------------------------------------------------!
			 * */
		/// <summary>
		/// add information of a particular lab side scheduling server identified by lssID
		/// </summary>
        /// <param name="lssGuid"></param>
		/// <param name="lssUrl"></param>
		/// <returns></returns>the unique ID which identifies the LSSInfo added,>0 successfully added, ==-1 otherwise
        public static int AddLSSInfo(string lssGuid, string lssName, string lssUrl)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "AddLSSInfo" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("AddLSSInfo",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssGUID", lssGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssName", lssName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssURL", lssUrl, DbType.String,512));
			
			int i=-1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add LSS Information",ex);
			}
			finally
			{
				connection.Close();
			}	
			return i;		
		}

        /// <summary>
        /// Updates the data fields for the LSSInfo specified by the lssInfoId; note lssInfoId may not be changed
        /// </summary>
        /// <param name="lssInfoId"></param>
        /// <param name="lssGuid"></param>
        /// <param name="lssName"></param>
        /// <param name="lssUrl"></param>
        /// <returns></returns>true if lssInfo was successfully modified, ==false otherwise
        public static int ModifyLSSInfo(int lssInfoId, string lssGuid, string lssName, string lssUrl)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "ModifyLSSInfo" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("ModifyLssInfo",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssInfoId", lssInfoId, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssGUID", lssGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssName", lssName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssURL", lssUrl, DbType.String,512));
			
            int count = -1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
			
                if (ob != null && ob != System.DBNull.Value)
				{
					count = Convert.ToInt32(ob);
				}
			
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add ModifyLSSInfo",ex);
			}
			finally
			{
				connection.Close();
			}		
			return count;
             
		}
        /// <summary>
        /// Updates the data fields for the LSSInfo specified by the lssInfoId; note lssInfoId may not be changed
        /// </summary>
        /// <param name="lssInfoId"></param>
        /// <param name="lssGuid"></param>
        /// <param name="lssName"></param>
        /// <param name="lssUrl"></param>
        /// <returns></returns>true if lssInfo was successfully modified, ==false otherwise
        public static int ModifyLSSInfo(string lssGuid, string lssName, string lssUrl)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "ModifyLSSInfo" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ModifyLssInfoByGuid", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssGUID", lssGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssName", lssName, DbType.String, 256));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssURL", lssUrl, DbType.String, 512));
            
            int count = -1;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    count = Convert.ToInt32(ob);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add ModifyLSSInfo", ex);
            }
            finally
            {
                connection.Close();
            }
            return count;

        }

		/// <summary>
		/// delete the information of lab side scheduling servers identified by lssInfoIds
		/// </summary>
		/// <param name="lssInfoIds"></param>
		/// <returns></returns>An array of ints containing the IDs of all LSS whose informations not successfully removed, i.e., those for which the operation failed. 
		public static int[] RemoveLSSInfo(int[] lssInfoIds)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeletLSSInfo" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("DeletLSSInfo",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssInfoId", null, DbType.Int32));
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int lssInfoId in lssInfoIds)
				{
					cmd.Parameters["@lssInfoId"].Value = lssInfoId;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(lssInfoId);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove LSS infomation",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}

        /// <summary>
        /// delete the information of lab side scheduling servers identified by lssInfoIds
        /// </summary>
        /// <param name="lssInfoIds"></param>
        /// <returns></returns>An array of ints containing the IDs of all LSS whose informations not successfully removed, i.e., those for which the operation failed. 
        public static int RemoveLSSInfoByGuid(string lssGuid)
        {
            int status;
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "DeletLSSInfoByGuid" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("DeletLSSInfoByGuid", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@guid", lssGuid, DbType.AnsiString, 50));
            
            // execute the command
            try
            {
                connection.Open();
               status = cmd.ExecuteNonQuery();
               
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in remove LSS infomation", ex);
            }
            finally
            {
                connection.Close();
            }
            
            return status;
        }
		/// <summary>
		/// Enumerates IDs of all the lssInfos 
		/// </summary>
		/// <returns></returns>An array of ints containing the IDs of all the lssInfos 
		public static int[] ListLSSInfoIDs()
		{
			int[] lssInfoIds;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveRetrieveLssInfoIDs" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveLssInfoIDs", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the lssInfoIds retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["LSS_Info_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["LSS_Info_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve lssInfoIds",ex);
			}
			finally
			{
				connection.Close();
			}
			lssInfoIds=Utilities.ArrayListToIntArray(arrayList);
			return lssInfoIds;
		}
		/// <summary>
		/// Returns an array of the immutable LSSInfo objects that correspond to the supplied lssInfo IDs. 
		/// </summary>
		/// <param name="lssInfoIds"></param>
		/// <returns></returns>
		public static LSSInfo[] GetLSSInfos(int[] lssInfoIds)
		{
			LSSInfo[] lssInfos=new LSSInfo[lssInfoIds.Length];
			for(int i=0; i<lssInfoIds.Length;i++)
			{
				lssInfos[i]=new LSSInfo();
			}
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveLSSInfoByID" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveLSSInfoByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssInfoId", null, DbType.Int32));
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0;i<lssInfoIds.Length;i++)
				{
					// populate the parameters
					cmd.Parameters["@lssInfoId"].Value = lssInfoIds[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
					{	
						lssInfos[i].lssInfoId=lssInfoIds[i];
						if(dataReader[0] != System.DBNull.Value )
							lssInfos[i].lssGuid=dataReader.GetString(0);
						if(dataReader[1] != System.DBNull.Value )
							lssInfos[i].lssName=dataReader.GetString(1);
						if(dataReader[2] != System.DBNull.Value )
							lssInfos[i].lssUrl=dataReader.GetString(2);
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get lssInfo",ex);
			}
			finally
			{
				connection.Close();
			}	
			return lssInfos;
		}

        /// <summary>
        /// Returns an array of the immutable LSSInfo objects that correspond to the supplied lssInfo IDs. 
        /// </summary>
        /// <param name="lssInfoIds"></param>
        /// <returns></returns>
        public static LSSInfo GetLSSInfo(string lssGuid)
        {
            LSSInfo lssInfo = new LSSInfo();

            // create sql connection
            DbConnection connection = FactoryDB.GetConnection();

            // create sql command
            DbCommand cmd = FactoryDB.CreateCommand("RetrieveLSSInfoByGUID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@lssGuid", lssGuid, DbType.AnsiString, 50));
            
            //execute the command
            try
            {
                connection.Open();
                DbDataReader dataReader = null;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    lssInfo.lssInfoId = dataReader.GetInt32(0);
                    if (dataReader[1] != System.DBNull.Value)
                        lssInfo.lssGuid = dataReader.GetString(1);
                    if (dataReader[2] != System.DBNull.Value)
                        lssInfo.lssName = dataReader.GetString(2);
                    if (dataReader[3] != System.DBNull.Value)
                        lssInfo.lssUrl = dataReader.GetString(3);
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                throw new Exception("Exception thrown in get lssInfo", ex);
            }
            finally
            {
                connection.Close();
            }
            return lssInfo;
        }


		/* !------------------------------------------------------------------------------!
			 *							CALLS FOR Credential Set
			 * !------------------------------------------------------------------------------!
			 * */
		/// <summary>
		///  add a credential set
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
		/// <returns></returns>return the unique ID identifying the credential set added,>0 successfully added, ==-1 otherwise
        public static int AddCredentialSet(string serviceBrokerGuid, string serviceBrokerName, string groupName)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "AddCredentialSet" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("AddCredentialSet",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerName", serviceBrokerName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupName", groupName, DbType.String,256));
		
			int i=-1;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
				{
					i= Int32.Parse(ob.ToString());
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add credential set",ex);
			}
			finally
			{
				connection.Close();
			}	
			return i;		
		}
		/// <summary>
		/// Updates the data fields for the credential set specified by the credentialSetId; note credentialSetId may not be changed
		/// </summary>
		/// <param name="credentialSetId"></param>
		/// <param name="groupName"></param>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="serviceBrokerName"></param>
		/// <returns></returns>true if reservation was successfully modified, ==false otherwise
        public static bool ModifyCredentialSet(int credentialSetId, string groupName, string serviceBrokerGuid, string serviceBrokerName)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "ModifyCredentialSet" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("ModifyCredentialSet",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@credentialSetId", credentialSetId, DbType.Int32));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupName", groupName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerName", serviceBrokerName, DbType.String,256));
			
			bool i=false;

			// execute the command
			try
			{
				connection.Open();
				Object ob=cmd.ExecuteScalar();
				int m=0;
                if (ob != null && ob != System.DBNull.Value)
				{
					m = Int32.Parse(ob.ToString());
				}
				if (m!=0)
				{
					i=true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in add ModifyCredentialSet",ex);
			}
			finally
			{
				connection.Close();
			}		
			return i;
             
		}
        /// <summary>
        /// Updates the data fields for the credential set specified by the credentialSetId; note credentialSetId may not be changed
        /// </summary>
        /// <param name="credentialSetId"></param>
        /// <param name="groupName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="serviceBrokerName"></param>
        /// <returns></returns>true if reservation was successfully modified, ==false otherwise
        public static int ModifyCredentialSetServiceBroker(string originalGuid, string serviceBrokerGuid, string serviceBrokerName)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "ModifyCredentialSet" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("ModifyCredentialSetServiceBroker", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@originalGUID", originalGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerName", serviceBrokerName, DbType.String, 256));
           
            int count = -1;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
            
                if (ob != null && ob != System.DBNull.Value)
                {
                    count = Convert.ToInt32(ob);
                }
             
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in add ModifyCredentialSet", ex);
            }
            finally
            {
                connection.Close();
            }
            return count;

        }
		/// <summary>
		/// delete the credential sets specified by the credentialSetIds
		/// </summary>
		/// <param name="credentialSetIds"></param>
		/// <returns></returns>an array of ints containing the IDs of all credentail sets not successfully removed
		public static int[] RemoveCredentialSets(int[] credentialSetIds)
		{
			ArrayList arrayList=new ArrayList();
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteCredentialSet" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("DeleteCredentialSetByID",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@credentialSetId", null, DbType.Int32));
			
			// execute the command
			try
			{
				connection.Open();
				//populate the parameters
				foreach(int credentialSetId in credentialSetIds)
				{			
					cmd.Parameters["@credentialSetId"].Value = credentialSetId;
					if (cmd.ExecuteNonQuery()==0)
					{
						arrayList.Add(credentialSetId);
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove credential set",ex);
			}
			finally
			{
				connection.Close();
			}	
			int[] uIDs = Utilities.ArrayListToIntArray(arrayList);
			return uIDs;			
		}
		/// <summary>
		///  Remove a credential set
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
		/// <returns></returns>true, the credentialset is removed successfully, false otherwise
        public static int RemoveCredentialSet(string serviceBrokerGuid, string serviceBrokerName, string groupName)
		{
			//create a connection
			DbConnection connection= FactoryDB.GetConnection();
			//create a command
			//command executes the "DeleteCredentialSet" store procedure
			DbCommand cmd=FactoryDB.CreateCommand("DeleteCredentialSet",connection);
			cmd.CommandType=CommandType.StoredProcedure;
			//populate the parameters
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString,50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerName", serviceBrokerName, DbType.String,256));
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupName", groupName, DbType.String,256));
			
			int removed = 0;

			// execute the command
			try
			{
				connection.Open();
                removed = cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw new Exception("Exception thrown in remove credential set",ex);
			}
			finally
			{
				connection.Close();
			}	
			return removed;		
		}
		
/// <summary>
/// Enumerates IDs of all the credential sets 
/// </summary>
/// <returns></returns>An array of ints containing the IDs of all the credential sets 
		public static int[] ListCredentialSetIds()
		{
			int[] credentialSetIds;
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveCredentialSetIDs" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveCredentialSetIDs", connection);
			cmd.CommandType = CommandType.StoredProcedure;

			// execute the command
			DbDataReader dataReader = null;
			ArrayList arrayList=new ArrayList();
			try 
			{
				connection.Open();
				dataReader = cmd.ExecuteReader ();
				//store the credentialSetIds retrieved in arraylist
				
				while(dataReader.Read ())
				{	
					if(dataReader["Credential_Set_ID"] != System.DBNull.Value )
						arrayList.Add(Convert.ToInt32(dataReader["Credential_Set_ID"]));
				}
				dataReader.Close();
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in retrieve credentialSetIds",ex);
			}
			finally
			{
				connection.Close();
			}
			credentialSetIds=Utilities.ArrayListToIntArray(arrayList);
			return credentialSetIds;
		}
/// <summary>
/// Returns an array of the immutable credential set objects that correspond to the supplied credential set IDs
/// </summary>
/// <param name="credentialSetIds"></param>
/// <returns></returns>
		public static UssCredentialSet[] GetCredentialSets(int[] credentialSetIds)
		{
			UssCredentialSet[] credentialSets=new UssCredentialSet[credentialSetIds.Length];
			for(int i=0; i<credentialSetIds.Length;i++)
			{
				credentialSets[i]=new UssCredentialSet();
			}
			// create sql connection
			DbConnection connection = FactoryDB.GetConnection();

			// create sql command
			// command executes the "RetrieveCredentialSetByID" stored procedure
			DbCommand cmd = FactoryDB.CreateCommand("RetrieveCredentialSetByID", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@credentialSetID", null, DbType.Int32));
			//execute the command
			try 
			{
				connection.Open();
				for(int i=0;i<credentialSetIds.Length;i++)
				{
					// populate the parameters
					cmd.Parameters["@credentialSetID"].Value = credentialSetIds[i];	
					DbDataReader dataReader = null;
					dataReader=cmd.ExecuteReader();
					while(dataReader.Read())
					{	
						credentialSets[i].credentialSetId=credentialSetIds[i];
						if(dataReader[0] != System.DBNull.Value )
							credentialSets[i].groupName=dataReader.GetString(0);
						if(dataReader[1] != System.DBNull.Value )
							credentialSets[i].serviceBrokerGuid=dataReader.GetString(1);
						if(dataReader[2] != System.DBNull.Value )
							credentialSets[i].serviceBrokerName=dataReader.GetString(2);
					}
					dataReader.Close();
				}	
			} 

			catch (Exception ex)
			{
				throw new Exception("Exception thrown in get credential set",ex);
			}
			finally
			{
				connection.Close();
			}	
			return credentialSets;
		}

        /// <summary>
        /// Get a credential set ID of a particular group
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <returns></returns>the unique ID which identifies the credential set, 0 otherwise
        public static int GetCredentialSetID(string serviceBrokerGuid, string groupName)
        {
            //create a connection
            DbConnection connection = FactoryDB.GetConnection();
            //create a command
            //command executes the "AddCredentialSet" store procedure
            DbCommand cmd = FactoryDB.CreateCommand("GetCredentialSetID", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            //populate the parameters
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@serviceBrokerGUID", serviceBrokerGuid, DbType.AnsiString, 50));
            cmd.Parameters.Add(FactoryDB.CreateParameter(cmd,"@groupName", groupName, DbType.String, 256));
                       
            int i = 0;

            // execute the command
            try
            {
                connection.Open();
                Object ob = cmd.ExecuteScalar();
                if (ob != null && ob != System.DBNull.Value)
                {
                    i = Convert.ToInt32(ob);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in GetCredentialSetID", ex);
            }
            finally
            {
                connection.Close();
            }
            return i;
        }
		}
	}

