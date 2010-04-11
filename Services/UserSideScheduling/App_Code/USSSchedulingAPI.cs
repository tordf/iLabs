using System;
using iLabs.DataTypes.SchedulingTypes;

namespace iLabs.Scheduling.UserSide
{
    /// <remarks/>
  
    public class ReservationInfo
    {

        private int reservationIdField;

        private string userNameField;

        private int credentialSetIdField;

        private System.DateTime startTimeField;

        private System.DateTime endTimeField;

        private int experimentInfoIdField;

        /// <remarks/>
        public int reservationId
        {
            get
            {
                return this.reservationIdField;
            }
            set
            {
                this.reservationIdField = value;
            }
        }

        /// <remarks/>
        public string userName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }

        /// <remarks/>
        public int credentialSetId
        {
            get
            {
                return this.credentialSetIdField;
            }
            set
            {
                this.credentialSetIdField = value;
            }
        }

        /// <remarks/>
        public System.DateTime startTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime endTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }

        /// <remarks/>
        public int experimentInfoId
        {
            get
            {
                return this.experimentInfoIdField;
            }
            set
            {
                this.experimentInfoIdField = value;
            }
        }
    }

    /// <summary>
    /// a structure which hold lab side scheduling server information
    /// </summary>
    public struct LSSInfo
    {
        /// <summary>
        /// the ID of the lab side scheduling server information
        /// </summary>
        public int lssInfoId;
        /// <summary>
        /// the GUID of the lab side scheduling server 
        /// </summary>
        public string lssGuid;
        /// <summary>
        /// the name of the lab side scheduling server
        /// </summary>
        public string lssName;
        /// <summary>
        /// teh url of the lab side scheduling server
        /// </summary>
        public string lssUrl;

    }

    /// <summary>
    /// a structure which holds USS Credential Set
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public struct UssCredentialSet
    {
        /// <summary>
        /// the ID of the Credential Set
        /// </summary>
        public int credentialSetId;
        /// <summary>
        /// the GUID of the Service Broker whose domain the group belongs to
        /// </summary>
        public string serviceBrokerGuid;
        /// <summary>
        /// the name of the service broker whose domain the group belongs to
        /// </summary>
        public string serviceBrokerName;
        /// <summary>
        /// the name of the group
        /// </summary>
        public string groupName;
    }

    /// <summary>
    /// a structure which holds user side scheduling policy
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public struct USSPolicy
    {
        /// <summary>
        /// the ID of the USSPolicy
        /// </summary>
        public int ussPolicyId;
        /// <summary>
        /// the ID of the information of the experiment that the policy employes to
        /// </summary>
        public int experimentInfoId;
        /// <summary>
        /// the rule 
        /// </summary>
        public string rule;
        /// <summary>
        /// the ID of the credential set, user with which should obey the rule.
        /// </summary>
        public int credentialSetId;

    }
    /// <summary>
    /// a structuer which holds experiment inforamtion
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public struct UssExperimentInfo
    {
        /// <summary>
        /// the ID of the experiment information
        /// </summary>
        public int experimentInfoId;
        /// <summary>
        /// the GUID of the lab server which issues the experiment 
        /// </summary>
        public string labServerGuid;
        /// <summary>
        /// the name of the lab server which issues the experiment
        /// </summary>
        public string labServerName;
        /// <summary>
        /// the GUID of the lab client 
        /// </summary>
        public string labClientGuid;
        /// <summary>
        /// the version of the client through which the experiment can be executed
        /// </summary>
        public string labClientVersion;
        /// <summary>
        /// the name of the client through which the experiment can be executed
        /// </summary>
        public string labClientName;
        /// <summary>
        /// the name of the experiment's provider
        /// </summary>
        public string providerName;
        /// <summary>
        /// the GUID of the lab side scheculing server through which the lab server can be accessed
        /// </summary>
        public string lssGuid;

    }

	/// <summary>
	/// Summary description for USSSchedulingAPI.
	/// </summary>
	/// 
	public class USSSchedulingAPI
	{


		public USSSchedulingAPI()
		{
			//
			// TODO: Add constructor logic here
			//
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
            return DBManager.AddUSSPolicy(groupName, serviceBrokerGuid, experimentInfoId, rule);

        }
        /// <summary>
        /// delete the user side scheduling policies specified by the ussPolicyIds
        /// </summary>
        /// <param name="ussPolicyIds"></param>
        /// <returns></returns>the IDs of all UssPolicies not successfully removed

        public static int[] RemoveUSSPolicy(int[] ussPolicyIds)
        {
            return DBManager.RemoveUSSPolicy(ussPolicyIds);
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
            return DBManager.ModifyUSSPolicy(ussPolicyId, experimentInfoId, rule, credentialSetId);
        }
        /// <summary>
        /// enumerates all IDs of the User side Scheduling Policies 
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="serviceBrokerID"></param>
        /// <returns></returns>an array of ints containing the IDs of all the USSPolicies
        public static int[] ListUSSPolicyIDs()
        {
            return DBManager.ListUSSPolicyIDs();
        }
        /// <summary>
        /// returns an array of the immutable USSPolicy objects that correspond to the supplied ussPolicyIds
        /// </summary>
        /// <param name="ussPolicyIds"></param>
        /// <returns></returns>
        public static USSPolicy[] GetUSSPolicies(int[] ussPolicyIds)
        {
            return DBManager.GetUSSPolicies(ussPolicyIds);
        }
        /// <summary>
        /// enumerates all IDs of the User side Scheduling Policies applying for the users with a particular credential set identified by the combination of groupName and serviceBrokerID
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the USSPolicies applying for the users with a particular credential set
        public static int[] ListUSSPolicyIDsByGroup(string groupName, string serviceBrokerGuid)
        {
            return DBManager.ListUSSPolicyIDsByGroup(groupName, serviceBrokerGuid);
        }
        /// <summary>
        /// enumerates the ID of the User side Scheduling Policies applying for the users with a particular credential set identified by the combination of groupName and serviceBrokerID to execute a particular experiment
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the USSPolicies applying for the users with a particular credential set to execute a particular experiment
        public static int[] ListUSSPolicyIDsByGroupAndExperiment(string groupName, string serviceBrokerGuid, string labClientName, string labClientVersion)
        {
            return DBManager.ListUSSPolicyIDsByGroupAndExperiment(groupName, serviceBrokerGuid, labClientName, labClientVersion);
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
        /// <param name="labServerGuid"></param>
        /// <param name="clientGuid"
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>the unique id which identifies the reservation added by the user,>0 successfully added, ==-1 otherwise
        public static int AddReservation(string userName, string serviceBrokerGuid, string groupName, 
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            return DBManager.AddReservation(userName, serviceBrokerGuid, groupName,
                labServerGuid, clientGuid, startTime, endTime);
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
            return DBManager.AddReservation(userName, serviceBrokerGuid, groupName, experimentInfoId, startTime, endTime);
        }
        /// <summary>
        /// delete the reservations specified by the reservationIDs
        /// </summary>
        /// <param name="reservationIDs"></param>
        /// <returns></returns>an array of ints containing the IDs of all reservation not successfully removed
        public static int[] RemoveReservation(int[] reservationIDs)
        {
            return DBManager.RemoveReservation(reservationIDs);
        }
        /// <summary>
        /// updates the data fields for the reservation specified by the reservationID
        /// </summary>
        /// <param name="reservationID"></param>
        /// <param name="experimentInfoId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>ture if updated successfully, false otherwise
        public static bool ModifyReservation(int reservationID, int experimentInfoId, DateTime startTime, DateTime endTime)
        {
            return DBManager.ModifyReservation(reservationID, experimentInfoId, startTime, endTime);

        }
        /// <summary>
        /// enumerates all IDs of the reservations on a particular made by a particular user identified by the combination of userName and serviceBrokerID
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the reservation made by the specified user

        public static int[] ListReservationIDsByUser(string userName, string serviceBrokerGuid, int experimentInfoId)
        {
            return DBManager.ListReservationIDsByUser(userName, serviceBrokerGuid, experimentInfoId);
        }
        /// <summary>
        /// enumerates all IDs of the reservations with a credential sets identified by the combination of groupName and serviceBrokerID
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the reservations for the specified lab server made by the specified user
        public static int[] ListReservationIDsByGroup(string groupName, string serviceBrokerGuid)
        {
            return DBManager.ListReservationIDsByGroup(groupName, serviceBrokerGuid);

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
            return DBManager.ListReservationIDsByLabServer(labServerGuid, startTime, endTime);

        }
        /// <summary>
        /// returns an array of the immutable reservation objects that correspond to the supplied reservation IDs
        /// </summary>
        /// <param name="reservationIDs"></param>
        /// <returns></returns>
        public static ReservationInfo[] GetReservations(int[] reservationIDs)
        {
            return DBManager.GetReservations(reservationIDs);
        }

        /// <summary>
        /// returns an array of the immutable reservation objects that correspond to the supplied reservation IDs
        /// </summary>
        /// <param name="sbGuid"></param>
        /// <param name="userName"></param>
        /// <param name="groupName"></param>
        /// <param name="lsGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static ReservationInfo[] GetReservations(string sbGuid, string userName, string groupName,
            string lsGuid, string clientGuid, DateTime start, DateTime end)
        {
            return DBManager.GetReservations(sbGuid, userName, groupName,lsGuid, clientGuid, start, end);
        }
        /// <summary>
        /// to select reservation accorrding to given criterion
        /// </summary>
        public static ReservationInfo[] GetReservations(string userName, int experimentInfoId, int credentialSetId, DateTime timeAfter, DateTime timeBefore)
        {
            return DBManager.GetReservations(userName, experimentInfoId, credentialSetId, timeAfter, timeBefore);
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
        /// <param name="labClientGuid"></param>
        /// <param name="labClientName"></param>
        /// <param name="labClientVersion"></param>
		/// <param name="providerName"></param>
        /// <param name="lssGuid"></param>
		/// <returns></returns>the unique ID which identifies the experiment added,>0 successfully added,==-1 otherwise
        public static int AddExperimentInfo(string labServerGuid, string labServerName, string labClientGuid, string labClientName, string labClientVersion, string providerName, string lssGuid)
		{
            int[] experimentInfoIds = ListExperimentInfoIDs();
            UssExperimentInfo[] experimentInfo = GetExperimentInfos(experimentInfoIds);

            foreach (UssExperimentInfo info in experimentInfo)
            {
                if (info.labClientGuid .Equals(labClientGuid))
                {
                    return -1;
                }
            }

            return DBManager.AddExperimentInfo(labServerGuid, labServerName, labClientGuid, labClientName, labClientVersion, providerName, lssGuid);
		}
		/// <summary>
		/// delete the experiment information specified by the experimentInfoIds
		/// </summary>
		/// <param name="experimentInfoIds"></param>
		/// <returns></returns>an array of ints containing the IDs of all experiments not successfully removed
		public static int[] RemoveExperimentInfo(int[] experimentInfoIds)
		{
			return DBManager.RemoveExperimentInfo(experimentInfoIds);
		}
         /// <summary>
        ///  enumerates the GUID of LSS which is in charge of a particular experiment specified by labClientName and labClientVersion
        /// </summary>
        /// <param name="labClientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>
        public static string ListLSSIDbyExperiment(string clientGuid, string labServerGuid)
        {
            return DBManager.ListLssIdByExperiment(clientGuid, labServerGuid);
        }
		/// <summary>
		///Enumerates IDs of all the experimentInfos 
		/// </summary>
		/// <returns></returns> An array of ints containing the IDs of all the experimentInfos 
		public static int[] ListExperimentInfoIDs()
		{
			return DBManager.ListExperimentInfoIDs();
		}
		/// <summary>
		/// Returns an array of the immutable ExperimentInfo objects that correspond to the supplied experimentInfo IDs. 
		/// </summary>
		/// <param name="experimentInfoIds"></param>
		/// <returns></returns>
		public static UssExperimentInfo[] GetExperimentInfos(int[] experimentInfoIds)
		{
			return DBManager.GetExperimentInfos(experimentInfoIds);
		}
		/// <summary>
		/// enumerates the ID of the information of a particular experiment specified by labClientName and labClientVersion
		/// </summary>
		/// <param name="labServerGuid"></param>
		/// <param name="clientGuid"></param>
		/// <returns></returns>the ID of the information of a particular experiment, -1 if such a experiment info can not be retrieved
		public static int ListExperimentInfoIDByExperiment(string labServerGuid, string clientGuid)
		{
		    return DBManager.ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
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
        public static int ModifyExperimentInfo(int experimentInfoId, string labServerGuid, string labClientGuid, string labServerName, string labClientName, string labClientVersion, string providerName, string lssGuid)
		{
            return DBManager.ModifyExperimentInfo(experimentInfoId, labServerGuid, labServerName, labClientGuid, labClientName, labClientVersion, providerName, lssGuid);
		}
		/// <summary>
		/// enumerates the URL of LSS which is in charge of a particular experiment specified by labClientName and labClientVersion
		/// </summary>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <returns>the url of the requested LSS, null if such a experiment info can not be retrieved</returns>
		public static string ListLSSURLbyExperiment(string clientGuid,string labServerGuid)
		{
			return DBManager.ListLssUrlByExperiment(clientGuid, labServerGuid);
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
            int[] lssInfoIds = ListLSSInfoIDs();
            LSSInfo[] lssInfo = GetLSSInfos(lssInfoIds);

            //Checks if the LSS to be added already exists
            foreach (LSSInfo info in lssInfo)
            {
                if (lssGuid.Equals(info.lssGuid))
                {
                    return -1;
                }
            }


            return DBManager.AddLSSInfo(lssGuid, lssName, lssUrl);
		}
		/// <summary>
		/// Updates the data fields for the LSSInfo specified by the lssInfoId; note lssInfoId may not be changed
		/// </summary>
		/// <param name="lssInfoId"></param>
        /// <param name="lssGuid"></param>
		/// <param name="lssName"></param>
		/// <param name="lssUrl"></param>
		/// <returns></returns>true if lssInfo was successfully modified, ==false otherwise
		public static int ModifyLSSInfo(int lssInfoId, string lssGuid,string lssName, string lssUrl)
		{
            return DBManager.ModifyLSSInfo(lssInfoId, lssGuid, lssName, lssUrl);
		}
		/// <summary>
		/// delete the information of lab side scheduling servers identified by lssInfoIds
		/// </summary>
		/// <param name="lssInfoIds"></param>
		/// <returns></returns>An array of ints containing the IDs of all LSS whose informations not successfully removed, i.e., those for which the operation failed. 
		public static int[] RemoveLSSInfo(int[] lssInfoIds)
		{
			return DBManager.RemoveLSSInfo(lssInfoIds);			
		}
        /// <summary>
        /// delete the information of lab side scheduling server identified by GUID
        /// </summary>
        /// <param name="lssInfoIds"></param>
        /// <returns></returns>the number of records deleted.
        public static int RemoveLSSInfoByGuid(string lssGuid)
        {
            return DBManager.RemoveLSSInfoByGuid(lssGuid);
        }
		/// <summary>
		/// Enumerates IDs of all the lssInfos 
		/// </summary>
		/// <returns></returns>An array of ints containing the IDs of all the lssInfos 
		public static int[] ListLSSInfoIDs()
		{
			return DBManager.ListLSSInfoIDs();
		}
		/// <summary>
		/// Returns an array of the immutable LSSInfo objects that correspond to the supplied lssInfo IDs. 
		/// </summary>
		/// <param name="lssInfoIds"></param>
		/// <returns></returns>
		public static LSSInfo[] GetLSSInfos(int[] lssInfoIds)
		{
			return DBManager.GetLSSInfos(lssInfoIds);
		}

        /// <summary>
        /// Returns an LSSInfo object that correspond to the supplied lss Guid. 
        /// </summary>
        /// <param name="lssInfoIds"></param>
        /// <returns>The specified LSSInfo or a LSSinfo with ID == 0</returns>
        public static LSSInfo GetLSSInfo(string lssGuid)
        {
            return DBManager.GetLSSInfo(lssGuid);
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
            int[] credentialSetIds = ListCredentialSetIds();
            UssCredentialSet[] credentialSets = GetCredentialSets(credentialSetIds);

            foreach (UssCredentialSet set in credentialSets)
            {
                if (set.serviceBrokerGuid.Equals(serviceBrokerGuid) && set.serviceBrokerName.Equals(serviceBrokerName)
                    && set.groupName.Equals(groupName))
                {
                    return -1;
                }
            }
            return DBManager.AddCredentialSet(serviceBrokerGuid, serviceBrokerName, groupName);
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
            return DBManager.ModifyCredentialSet(credentialSetId, groupName, serviceBrokerGuid, serviceBrokerName);
             
		}
		/// <summary>
		/// delete the credential sets specified by the credentialSetIds
		/// </summary>
		/// <param name="credentialSetIds"></param>
		/// <returns></returns>an array of ints containing the IDs of all credentail sets not successfully removed
		public static int[] RemoveCredentialSets(int[] credentialSetIds)
		{
			return DBManager.RemoveCredentialSets(credentialSetIds);
		}
		/// <summary>
		///  Remove a credential set
		/// </summary>
        /// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
		/// <returns></returns>true, the credentialset is removed successfully, false otherwise
        public static int RemoveCredentialSet(string serviceBrokerGuid, string serviceBrokerName, string groupName)
		{
            return DBManager.RemoveCredentialSet(serviceBrokerGuid, serviceBrokerName, groupName);
		}
		/// <summary>
		/// Enumerates IDs of all the credential sets 
		/// </summary>
		/// <returns></returns>An array of ints containing the IDs of all the credential sets 
		public static int[] ListCredentialSetIds()
		{
			return DBManager.ListCredentialSetIds();
		}
		/// <summary>
		/// Returns an array of the immutable credential set objects that correspond to the supplied credential set IDs
		/// </summary>
		/// <param name="credentialSetIds"></param>
		/// <returns></returns>
		public static UssCredentialSet[] GetCredentialSets(int[] credentialSetIds)
		{
			return DBManager.GetCredentialSets(credentialSetIds);
		}

        /// <summary>
        /// Get the credential set ID of a particular group
        /// </summary>
        /// <param name="serviceBrokerID"></param>
        /// <param name="groupName"></param>
        /// <param name="ussID"></param>
        /// <returns></returns>the unique ID which identifies the credential set added.>0 was successfully added,-1 otherwise
        public static int GetCredentialSetID(string serviceBrokerGuid, string groupName)
        {
            int credentialSetID = DBManager.GetCredentialSetID(serviceBrokerGuid, groupName);

            return credentialSetID;
        }
		/// <summary>
		/// remove all the reservation for certain lab server being covered by the revocation time 
		/// </summary>
        /// <param name="labServerGuid"></param>
		/// <param name="startTime"></param>the local time of USS
		/// <param name="endTime"></param>the local time of USS
		/// <returns></returns>true if all the reservations have been removed successfully
        public static bool RevokeReservation(string serviceBrokerGuid, string groupName,
            string labServerGuid, string labClientGuid,
            DateTime startTime, DateTime endTime, string message)
		{
			try
			{   
				bool i = false;
				DateTime startTimeUTC = startTime.ToUniversalTime();
				DateTime endTimeUTC = endTime.ToUniversalTime();
                int[] resIDs = ListReservationIDsByLabServer(labServerGuid, startTimeUTC, endTimeUTC);
				int[] unRemovedResIDs = RemoveReservation(resIDs);
				if (unRemovedResIDs.Length == 0)
				{
					i =true;
				}
				return i;
			}
			catch(Exception ex)
			{
				throw new Exception("Exception thrown in RevokeReservation",ex);
			}
		}
        /// <summary>
        /// Returns an existing ReservationInfo or null, indicating whether it is the right time for a particular user to execute a particular experiment
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>true if it is the right time for a particular user to execute a particular experiment
        public static ReservationInfo RedeemReservation(String userName, String serviceBrokerGuid, String clientGuid, String labServerGuid)
        {
            return RedeemReservation(userName,serviceBrokerGuid,clientGuid,labServerGuid,DateTime.UtcNow);
         
        }

		/// <summary>
		/// Returns an existing ReservationInfo or null, indicating whether it is the right time for a particular user to execute a particular experiment
		/// </summary>
		/// <param name="userName"></param>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="labServerGuid"></param>
		/// <returns></returns>true if it is the right time for a particular user to execute a particular experiment
        public static ReservationInfo RedeemReservation(String userName, String serviceBrokerGuid, 
            String clientGuid, String labServerGuid,DateTime targetTime)
		{
			ReservationInfo redeemedRes = null;
			try
			{
                int i = DBManager.ListReservationIDByUser(userName, serviceBrokerGuid, clientGuid, labServerGuid, targetTime);
				if (i >0)
				{
                    redeemedRes = DBManager.GetReservations(new int[] { i })[0];
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Exception thrown in RedeemReservation",ex);
			}

            return redeemedRes;		 

		}
        /// <summary>
        /// Return the time span the user should wait till the start time of the reservation
        /// </summary>
        /// <param name="reservationID"></param>the reservation ID to be checked
        /// <returns></returns>
        public static TimeSpan GetReservationWaitTime(int reservationID)
        {
            ReservationInfo redeemedRes = new ReservationInfo();
            TimeSpan ts = new TimeSpan();
            try
            {
             redeemedRes = DBManager.GetReservations(new int[] { reservationID })[0];
             DateTime startTime = redeemedRes.startTime.ToLocalTime();
             ts = startTime.Subtract(DateTime.Now);
             }
            
            catch (Exception ex)
            {
                throw new Exception("Exception thrown in RedeemReservation", ex);
            }

            return ts;

        }

	}
}
