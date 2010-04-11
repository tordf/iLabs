using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.Mail;

using iLabs.DataTypes;
using iLabs.DataTypes.SchedulingTypes;
using iLabs.DataTypes.SoapHeaderTypes;
using iLabs.Core;
using iLabs.UtilLib;

using iLabs.Proxies.USS;

namespace iLabs.Scheduling.LabSide
{


    #region LSS API Classes
    /*
    /// <summary>
    /// a structure which holds time block
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class TimeBlockInfo
    {
        /// <summary>
        /// the ID of the time block
        /// </summary>
        public int timeBlockId;
        /// <summary>
        /// the resourceID , the LabServer and resource the time block was assigned to. May not be needed see recurrenceID.
        /// </summary>
        public int resourceId;
        /// <summary>
        /// the start time of the time block, in UTC
        /// </summary>
        public DateTime startTime;
        /// <summary>
        /// the end time of the time block, in UTC
        /// </summary>
        public DateTime endTime;
        /// <summary>
        /// the GUID of the lab server that the time block belongs to. May not be needed see recurrenceID.
        /// </summary>
        public String labServerGuid;
        /// <summary>
        /// the ID of the recurrence that the time block belongs to
        /// </summary>
        public int recurrenceID;
    }
*/

    /// <summary>
    /// a structure which holds experiment information
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class LssExperimentInfo
    {
        /// <summary>
        /// the ID of the experiment information
        /// </summary>
        public int experimentInfoId;
        /// <summary>
        /// the GUID of the lab Client 
        /// </summary>
        public string labClientGuid;
        /// <summary>
        /// the GUID of the lab server which provide the experiment
        /// </summary>
        public string labServerGuid;
        /// <summary>
        /// the Name of the lab server which provide the experiment
        /// </summary>
        public string labServerName;
        /// <summary>
        /// the name of the lab client through which the experiment can be executed
        /// </summary>
        public string labClientName;
        /// <summary>
        /// the version of the lab client through which the experiment can be executed
        /// </summary>
        public string labClientVersion;
        /// <summary>
        /// the name of the provider of the experiment
        /// </summary>
        public string providerName;
        public string contactEmail;
    
        /// <summary>
        /// the start up time needed for the execution of the experiment
        /// </summary>
        public int prepareTime;
        /// <summary>
        /// the cool down time needed after the execution of the experiment
        /// </summary>
        public int recoverTime;
        /// <summary>
        /// the experiment's minimum exection time
        /// </summary>
        public int minimumTime;
        /// <summary>
        /// the maxium time the user is allowed to arrive before the excution time of the experiment 
        /// </summary>
        public int earlyArriveTime;
    }
    /// <summary>
    /// a structure which holds Lab server side policy
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class LSSPolicy
    {
        /// <summary>
        /// the ID of the lab side scheduling server policy
        /// </summary>
        public int lssPolicyId;
        /// <summary>
        /// the ID of the credential set, the goup with which should obey this policy
        /// </summary>
        public int credentialSetId;
        /// <summary>
        /// the rule
        /// </summary>
        public string rule;
        /// <summary>
        /// the ID of the information of the experiment which the policy is applied to
        /// </summary>
        public int experimentInfoId;
    }

    public class LSResource{
        public int resourceID;
        public string labServerGuid;
        public string labServerName;
        public string description;
    }
    /// <summary>
    /// a structure which holds a permitted experiment for a time block
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class PermittedExperiment
    {
        /// <summary>
        /// the ID of the permission
        /// </summary>
        public int permittedExperimentId;
        /// <summary>
        /// the ID of the informaiton of the experiment which is permitted to be executed 
        /// </summary>
        public int experimentInfoId;
        /// <summary>
        /// the ID of the recurrence whose permission is given to the experiment
        /// </summary>
        public int recurrenceId;
    }

    /// <summary>
    /// a structure which holds user side scheduling information
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class USSInfo
    {
        /// <summary>
        /// the ID of the user side scheduling server information
        /// </summary>
        public int ussInfoId;
        public long couponId;
        public string domainGuid;
        /// <summary>
        /// the GUID of the user side scheduling server
        /// </summary>
        public string ussGuid;
        /// <summary>
        /// the name of the user side scheduling server
        /// </summary>
        public string ussName;
        /// <summary>
        ///  the URL of the user side scheduling server
        /// </summary>
        public string ussUrl;
        
    }

    /// <summary>
    /// a structure which holds credential set
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class LssCredentialSet
    {
        /// <summary>
        /// the ID of the credential set
        /// </summary>
        public int credentialSetId;
        /// <summary>
        /// the GUID of the service broker whose domain the group belongs to
        /// </summary>
        public string serviceBrokerGuid;
        /// <summary>
        /// the Name of the service broker whose domain the group belongs to
        /// </summary>
        public string serviceBrokerName;
        /// <summary>
        /// the Name of the goup with this credential set
        /// </summary>
        public string groupName;
        /// <summary>
        /// the GUID of the user side scheduling server one which the group registered   
        /// </summary>
        public string ussGuid;
    }
    /// <summary>
    /// a structure which holds reservation information
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://ilab.mit.edu/iLabs/type")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ilab.mit.edu/iLabs/type", IsNullable = false)]
    public class ReservationInfo : ITimeBlock
    {
        /// <summary>
        /// the ID of the reservation information
        /// </summary>
        public int reservationInfoId;
        /// <summary>
        /// the ID of the resource which is reserved for execution
        /// </summary>
        public int resourceId;
        public int experimentInfoId;
        /// <summary>
        /// the ID of the credentialSet. the user from the group with this credential set made the reservation
        /// </summary>
        public int credentialSetId;
        /// <summary>
        /// the start time of the reservation
        /// </summary>
        public DateTime startTime;
        /// <summary>
        /// the end time of the reservation
        /// </summary>
        public DateTime endTime;
        public int statusCode;

        public int CompareTo(object that)
        {
            return CompareTo((ITimeBlock)that);
        }

        public int CompareTo(ITimeBlock b)
        {
            int status = 0;
            if (Start > b.Start)
            {
                status = 1;
            }
            else if (Start < b.Start)
            {
                status = -1;
            }
            else if (Duration > b.Duration)
            {
                status = 1;
            }
            else if (Duration < b.Duration)
            {
                status = -1;
            }
            return status;
        }

        public DateTime Start
        {
            get
            {
                return startTime;
            }
        }

        public DateTime End
        {
            get
            {
                return endTime;
            }
        }
        public int Duration
        {
            get
            {
                return (int)(endTime - startTime).TotalSeconds;
            }
        }
        public bool Intersects(ITimeBlock target)
        {
            return (this.Start < target.End && this.End > target.Start);
        }

        public TimeBlock Intersection(ITimeBlock target)
        {
            if (Intersects(target))
            {
                DateTime start = (Start > target.Start) ? Start : target.Start;
                DateTime end = (End < target.End) ? End : target.End;
                return new TimeBlock(start, end);
            }
            else
                return null;
        }
       
    }

#endregion

	/// <summary>
	/// Summary description for LSSSchedulingAPI.
	/// </summary>
	public class LSSSchedulingAPI
	{
		public LSSSchedulingAPI()
		{
			//
			// TODO: Add constructor logic here
			//
        }

        #region ExperimentInfo Methods
        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR ExperimentInfo
			 * !------------------------------------------------------------------------------!
			 */
		/// <summary>
		/// add information of particular experiment
		/// </summary>
        /// <param name="labClientGuid"></param>
		/// <param name="labServerGuid"></param>
		/// <param name="labServerName"></param>
		/// <param name="labClientVersion"></param>
		/// <param name="labClientName"></param>
		/// <param name="providerName"></param>
        /// <param name="contactEmail"></param>
		/// <param name="quantum"></param>
		/// <param name="prepareTime"></param>
		/// <param name="recoverTime"></param>
		/// <param name="minimumTime"></param>
		/// <param name="earlyArriveTime"></param>
		/// <returns></returns>the unique ID which identifies the experiment information added, >0 was successfully added, ==-1 otherwise
        public static int AddExperimentInfo( string labServerGuid, string labServerName, string labClientGuid,
            string labClientName, string labClientVersion, string providerName, string contactEmail,
            int prepareTime, int recoverTime, int minimumTime, int earlyArriveTime)
		{
            int experimentInfoID = DBManager.AddExperimentInfo(labServerGuid, labServerName, labClientGuid, labClientName, labClientVersion,
                providerName, contactEmail, prepareTime, recoverTime, minimumTime, earlyArriveTime);
				return experimentInfoID;		
		}
		/// <summary>
		/// delete the experiment information specified by the experimentInfoIDs
		/// </summary>
		/// <param name="experimentInfoIDs"></param>
		/// <returns></returns>an array of ints containing the IDs of all experiment information not successfully removed
		public static int[] RemoveExperimentInfo(int[] experimentInfoIDs)
		{
			int[] uIDs=DBManager.RemoveExperimentInfo(experimentInfoIDs);
			return uIDs;
		}
		/// <summary>
		/// Return an array of the immutable USSInfo objects thta correspond to the supplied USS information IDs
		/// </summary>
		/// <param name="experimentInfoIDs"></param>
		/// <returns></returns>
		public static LssExperimentInfo[] GetExperimentInfos(int[] experimentInfoIDs)
		{
			LssExperimentInfo[] experimentInfos=DBManager.GetExperimentInfos(experimentInfoIDs);
			return experimentInfos;
		}
		/// <summary>
		/// update the data fields for the experiment information specified by the experimentInfoID
		/// </summary>
		/// <param name="experimentInfoID"></param>
		/// <param name="labServerGuid"></param>
		/// <param name="labServerName"></param>
        /// <param name="labClientName"></param>
        /// <param name="labClientVersion"></param>
		/// <param name="providerName"></para
		/// <param name="prepareTime"></param>
		/// <param name="recoverTime"></param>
		/// <param name="minimumTime"></param>
		/// <param name="earlyArriveTime"></param>
		/// <returns></returns>true if modified successfully, falso otherwise
        public static bool ModifyExperimentInfo(int experimentInfoID, string labServerGuid, string labServerName,
             string labClientGuid, string labClientName, string labClientVersion, string providerName, 
            string contactEmail, int prepareTime, int recoverTime, int minimumTime, int earlyArriveTime)
		{
            bool i = DBManager.ModifyExperimentInfo(experimentInfoID, labServerGuid, labServerName, labClientGuid, labClientName, labClientVersion,
                providerName, contactEmail,prepareTime, recoverTime, minimumTime, earlyArriveTime);
		    return i;
		}

        /// <summary>
        /// get the labserver name according to the labserver ID
        /// </summary>
        /// <param name="labServer/// <param name="labClientName"></param>"></param>
        /// <returns></returns>
        public static string RetrieveLabServerName(string labServerGuid)
        {
            string labServerName = DBManager.RetrieveLabServerName(labServerGuid);
            return labServerName;
        }
		
		/// <summary>
		/// enumerates IDs of the information of all the experiments belonging to certain lab server identified by the labserverID
		/// </summary>
		/// <param name="labServerGuid"></param>
		/// <returns></returns>an array of ints containing the IDs of the information of all the experiments belonging to specified lab server
		public static int[] ListExperimentInfoIDsByLabServer(string labServerGuid)
		{
			int[] eIDs=DBManager.ListExperimentInfoIDsByLabServer(labServerGuid);
			return eIDs;
		}
		/// <summary>
		/// retrieve the ids of all the experiment information
		/// </summary>
		/// <returns></returns>an array of ints containing the IDs of the information of all the experiments 
		public static int[] ListExperimentInfoIDs()
		{
			int[] eIDs=DBManager.ListExperimentInfoIDs();
			return eIDs;
		}
		/// <summary>
		/// enumerates the ID of the information of a particular experiment specified by labClientName and labClientVersion
		/// </summary>
		/// <param name="labClientName"></param>
		/// <param name="labClientVersion"></param>
		/// <returns></returns>the ID of the information of a particular experiment, -1 if such a experiment info can not be retrieved
		public static int ListExperimentInfoIDByExperiment(string labServerGuid, string clientGuid)
		{
			int i=DBManager.ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
			return i;
        }
        #endregion

        #region LS_Resource Methods

        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR LS Resource
			 * !------------------------------------------------------------------------------!
			 */

        public static int CheckForLSResource(string guid, string name)
        {
            return DBManager.CheckForLSResource(guid, name);
        }
        public static LSResource GetLSResource(int id)
        {
            return DBManager.GetLSResource(id);
        }

        public static LSResource GetLSResource(string guid)
        {
            return DBManager.GetLSResource(guid);
        }

        public static IntTag[] GetLSResourceTags()
        {
            return DBManager.GetLSResourceTags();
        }

        public static IntTag[] GetLSResourceTags(string guid)
        {
            return DBManager.GetLSResourceTags(guid);
        }

        #endregion

        #region TimeBlock Methods
        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR Time Block
			 * !------------------------------------------------------------------------------!
			 */
        /*
		/// <summary>
		/// add a time block in which users with a particular credential set are allowed to access a particular lab server
		/// </summary>
		/// <param name="labServerGuid"></param>
		/// <param name="credentialSetID"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>the uniqueID which identifies the time block added, >0 was successfully added; ==-1 otherwise
		public static int AddTimeBlock(string labServerGuid,int resourceID, DateTime startTime, DateTime endTime, int recurrenceID)
		{
			int i=DBManager.AddTimeBlock(labServerGuid, resourceID, startTime, endTime, recurrenceID);
			return i;
		}
		
		/// <summary>
		/// delete the time blocks specified by the timeBlockIDs
		/// </summary>
		/// <param name="timeBlockIDs"></param>
		/// <returns></returns>an array of ints containning the IDs of all time blocks not successfully removed
		public static int[] RemoveTimeBlock(int[] timeBlockIDs)
		{
			int[] tbIDs=DBManager.RemoveTimeBlock(timeBlockIDs);
			return tbIDs;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="timeBlockID"></param>
		/// <param name="labServerGuid"></param>
		/// <param name="credentialSetID"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns> true if updated sucessfully, fals otherwise
		public static bool ModifyTimeBlock(int timeBlockID, string labServerGuid,int resourceID, DateTime startTime, DateTime endTime)
		{
			bool i=DBManager.ModifyTimeBlock(timeBlockID, labServerGuid, resourceID, startTime,  endTime);
            return i;
		}
		/*
		/// <summary>
		/// enumerates all IDs of the time blocks belonging to a particular lab server identified by the labserverID
		/// </summary>
		/// <param name="labServerGuid"></param>
		/// <returns></returns>an array of ints containing the IDs of all the time blocks of specified lab server
		public static int[] ListTimeBlockIDsByLabServer(string labServerGuid)
		{
			int[] tbIDs=DBManager.ListTimeBlockIDsByLabServer(labServerGuid);
			return tbIDs;
		}
         * */
        /*
		/// <summary>
		/// enumerates all IDs of the time blocks during which the members of a particular group identified by the credentialSetID are allowed to access a particular lab server identified by the labServerID
		/// </summary>
		/// <param name="labServerGuid"></param>
		/// <param name="credentialSetID"></param>
		/// <returns></returns>an array of ints containing the IDs of all the time blocks during which the members of a particular grou[ are allowed to access a particular lab server
		public static int[] ListTimeBlockIDsByGroup(string labServerGuid,int credentialSetID)
		{
			int[] tbIDs=DBManager.ListTimeBlockIDsByGroup(labServerGuid,credentialSetID);
			return tbIDs;
		}
        
		/// <summary>
		/// list the IDs of a particular lab server's time blocks which are assigned to particular group in the time chunk defined by the start time and end time
		/// </summary>
		/// <param name="serviceBrokerGuid"></param>
		/// <param name="groupName"></param>
		/// <param name="ussGuid"></param>
		/// <param name="labServerGuid"></param>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>
		public static int[] ListTimeBlockIDsByTimeChunk(string serviceBrokerGuid,string groupName, string ussGuid,
            string clientGuid, string labServerGuid, DateTime startTime, DateTime endTime)
		{
			int[] tbIDs=DBManager.ListTimeBlockIDsByTimeChunk(serviceBrokerGuid, groupName, ussGuid, labServerGuid, clientGuid, startTime, endTime);
			return tbIDs;
		}
		/// Enumerates the IDs of the information of all the time blocks 
		/// </summary>
		/// <returns></returns>the array  of ints contains the IDs of the information of all the time blocks
		public static int[] ListTimeBlockIDs()
		{
			int[] tbIDs=DBManager.ListTimeBlockIDs();
			return tbIDs;
		}
		/// <summary>
		/// Returns an array of the immutable TimeBlockInfo objects that correspond ot the supplied time block IDs
		/// </summary>
		/// <param name="timeBlockIDs"></param>
		/// <returns></returns>an array of immutable objects describing the specified time blocks
		public static TimeBlock[] GetTimeBlocks(int[] timeBlockIDs)
		{
			TimeBlock[] tbs=DBManager.GetTimeBlocks(timeBlockIDs);
			return tbs;
		}
*/
        #endregion

        #region CredentialSet

        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR CredentialSet
			 * !------------------------------------------------------------------------------!
			 */
		/// <summary>
		/// add a credential set of a particular group
		/// </summary>
		/// <param name="serviceBrokerID"></param>
		/// <param name="groupName"></param>
		/// <param name="ussID"></param>
		/// <returns></returns>the unique ID which identifies the credential set added.>0 was successfully added,-1 otherwise
		public static int AddCredentialSet(string serviceBrokerGuid,string serviceBrokerName, string groupName, string ussGuid)
		{
            int[] credentialSetIDs = LSSSchedulingAPI.ListCredentialSetIDs();
            LssCredentialSet[] credentialSets = LSSSchedulingAPI.GetCredentialSets(credentialSetIDs);

            foreach (LssCredentialSet set in credentialSets)
            {
                if (set.serviceBrokerGuid.Equals(serviceBrokerGuid) && set.serviceBrokerName.Equals(serviceBrokerName) &&
                    set.groupName.Equals(groupName) && set.ussGuid.Equals(ussGuid))
                {
                    return -1;
                }
                    
            }

			int cID=DBManager.AddCredentialSet(serviceBrokerGuid,serviceBrokerName, groupName, ussGuid);	
			return cID;
		}
		/// <summary>
		/// Updates the data fields for the credential set specified by the credentialSetID; note credentialSetID may not be changed 
		/// </summary>
		/// <param name="credentialSetID"></param>
		/// <param name="serviceBrokerGuid"></param>
		/// <param name="serviceBrokerName"></param>
		/// <param name="groupName"></param>
		/// <param name="ussGuid"></param>
		/// <returns></returns>if modified successfully, false otherwise
		public static int ModifyCredentialSet(int credentialSetID, string serviceBrokerGuid, string serviceBrokerName, string groupName, string ussGuid)
		{
			int i=DBManager.ModifyCredentialSet(credentialSetID, serviceBrokerGuid,serviceBrokerName, groupName,  ussGuid);
			return i;
			   
		}
		/// <summary>
		///  remove a credential set specified by the credentialsetsIDS
		/// </summary>
		/// <param name="credentialSetIDs"></param>
		/// <returns></returns>An array of ints containing the IDs of all credential sets not successfully removed, i.e., those for which the operation failed. 
		public static int[] RemoveCredentialSets(int[] credentialSetIDs)
		{
		     int[] cIDs=DBManager.RemoveCredentialSets(credentialSetIDs);
			return cIDs;
		}
		/// <summary>
		/// Enumerates the IDs of the information of all the credential set 
		/// </summary>
		/// <returns></returns>the array  of ints contains the IDs of all the credential set
		public static int[] ListCredentialSetIDs()
		{
			int[] cIDs=DBManager.ListCredentialSetIDs();
			return cIDs;
			
		}
          /// <summary>
        /// remove a credential set of a particular group
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <returns></returns>true, the credentialset is removed successfully, false otherwise
        public static int RemoveCredentialSet(string serviceBrokerGuid, string serviceBrokerName, string groupName, string ussGuid)
        {
            return DBManager.RemoveCredentialSet(serviceBrokerGuid, serviceBrokerName, groupName, ussGuid);
        }
		/// <summary>
		/// Returns an array of the immutable Credential objects that correspond to the supplied credentialSet IDs. 
		/// </summary>
		/// <param name="credentialSetIDs"></param>
		/// <returns></returns>An array of immutable objects describing the specified Credential Set information; if the nth credentialSetID does not correspond to a valid experiment scheduling property, the nth entry in the return array will be null.
		public static LssCredentialSet[] GetCredentialSets(int[] credentialSetIDs)
		{
			LssCredentialSet[] credentialSets=DBManager.GetCredentialSets(credentialSetIDs);
			return credentialSets;
		}
        /// <summary>
        /// Get the credential set ID of a particular group
        /// </summary>
        /// <param name="serviceBrokerID"></param>
        /// <param name="groupName"></param>
        /// <param name="ussID"></param>
        /// <returns></returns>the unique ID which identifies the credential set added.>0 was successfully added,-1 otherwise
        public static int GetCredentialSetID(string serviceBrokerGuid, string groupName, string ussGuid)
        {
            int credentialSetID = DBManager.GetCredentialSetID(serviceBrokerGuid, groupName, ussGuid);
           
            return credentialSetID;
        }

#endregion

        #region PermittedCredentialSet Methods
        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR Permited CredentialSet
			 * !------------------------------------------------------------------------------!
			 */
        /// <summary>
        /// add permission of a particular experiment being executed in a particular time block
        /// </summary>
        /// <param name="experimentInfoID"></param>
        /// <param name="timeBlockID"></param>
        /// <returns></returns>the unique ID which identifies the permission added. >0 was successfully added;==-1 otherwise
        public static int AddPermittedCredentialSet(int credentialSetID, int recurrenceID)
        {
            int i = DBManager.AddPermittedCredentialSet(credentialSetID, recurrenceID);
            return i;
        }

        /// <summary>
        /// delete permission of  a particular CredentialSet in a particular time block
        /// </summary>
        /// <param name="permittedExperimentIDs"></param>
        /// <returns></returns>an array of ints containing the IDs of all permissions not successfully removed
        public static int[] RemovePermittedCredentialSets(int[] permittedCredentialSetIDs, int recurrenceID)
        {
            int[] ids = DBManager.RemovePermittedCredentialSets(permittedCredentialSetIDs, recurrenceID);
            return ids;
        }
        /*
                /// <summary>
                /// enumerates the IDs of the information of the permitted experiments for a particular time block identified by the timeBlockID
                /// </summary>
                /// <param name="timeBlockID"></param>
                /// <returns></returns>an array of ints containing the IDs of the information of the permitted experiments for a particular time block identified by the timeBlockID
                public static int[] ListPermittedCredentialSetIDsByTimeBlock(int timeBlockID)
                {
                    int[] ids = DBManager.ListCredentialSetIDsByTimeBlock(timeBlockID);
                    return ids;
                }
      
                /// <summary>
                /// returns an array of the immutable PermittedExperiment objects that correspond to the supplied permittedExperimentIDs
                /// </summary>
                /// <param name="permittedExperimentIDs"></param>
                /// <returns></returns>an array of immutable objects describing the specified PermittedExperiments
                public static LssCredentialSet[] GetPermittedCredentialSets(int[] permittedCredentialSetIDs)
                {
                    LssCredentialSet[] exps = DBManager.GetPermittedCredentialSets(permittedCredentialSetIDs);
                    return exps;
                }
                /// <summary>
                /// retrieve unique ID of the PerimmttiedExperiment which represents the permission of executing a particular experiment in a particular time block
                /// </summary>
                /// <param name="experimentInfoID"></param>
                /// <param name="timeBlockID"></param>
                /// <returns></returns>-1 if the permission can not be retrieved
                public static int ListPermittedCredentialSetID(int credentialSetID, int timeBlockID)
                {
                    int i = DBManager.ListCredentialSetID(credentialSetID, timeBlockID);
                    return i;
                }
                /// <summary>
                /// retrieve unique ID of the PerimmttiedExperiment which represents the permission of executing a particular experiment in a particular recurrence
                /// </summary>
                /// <param name="experimentInfoID"></param>
                /// <param name="recurrenceID"></param>
                /// <returns></returns>-1 if the permission can not be retrieved
                public static int ListPermittedCredentialSetIDByRecur(int credentialSetID, int recurrenceID)
                {
                    int i = DBManager.ListPermittedCredentialSetIDByRecur(credentialSetID, recurrenceID);
                    return i;
                }
                 * */

        /// <summary>
        /// check whether the permission to a particular time block has been given to a CredentialSet 
        /// </summary>
        /// <param name="experimentInfoID"></param>
        /// <param name="timeBlockID"></param>
        /// <returns></returns>true the the permission exists. false other wise
        public static bool CheckGroupPermission(int credentialSetID, int timeBlockID)
        {
            return DBManager.IsPermittedCredentialSet(credentialSetID, timeBlockID);

        }
        /// <summary>
        /// enumerates the IDs of information of the permitted experiments for a particular recurrence identified by the recurrenceID
        /// </summary>
        /// <param name="recurrenceID"></param>
        /// <returns></returns>an array of ints containing the IDs of the information of the permitted groups for a particular recurrence identified by the recurrenceID
        public static int[] ListCredentialSetIDsByRecurrence(int recurrenceID)
        {
            int[] ids = DBManager.ListCredentialSetIDsByRecurrence(recurrenceID);
            return ids;
        }

        #endregion

        #region PermittedExperiment Methods

        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR Permited Experiment
			 * !------------------------------------------------------------------------------!
			 */
		/// <summary>
		/// add permission of a particular experiment being executed in a particular time block
		/// </summary>
		/// <param name="experimentInfoID"></param>
		/// <param name="timeBlockID"></param>
		/// <returns></returns>the unique ID which identifies the permission added. >0 was successfully added;==-1 otherwise
		public static int AddPermittedExperiment(int experimentInfoID, int recurrenceID)
		{
			int i=DBManager.AddPermittedExperiment(experimentInfoID, recurrenceID);
			return i;
		}
		
		/// <summary>
		/// delete permission of  a particular experiment being executed in a particular time block
		/// </summary>
		/// <param name="permittedExperimentIDs"></param>
		/// <returns></returns>an array of ints containing the IDs of all permissions not successfully removed
		public static int[] RemovePermittedExperiments(int[] permittedExperimentIDs, int recurrenceID)
		{
            int[] ids = DBManager.RemovePermittedExperiments(permittedExperimentIDs, recurrenceID);
			return ids;
		}
		/*
		/// <summary>
		/// enumerates the IDs of the information of the permitted experiments for a particular time block identified by the timeBlockID
		/// </summary>
		/// <param name="timeBlockID"></param>
		/// <returns></returns>an array of ints containing the IDs of the information of the permitted experiments for a particular time block identified by the timeBlockID
		public static int[] ListPermittedExperimentInfoIDsByTimeBlock(int timeBlockID)
		{
			int[] ids=DBManager.ListPermittedExperimentInfoIDsByTimeBlock(timeBlockID);
			return ids;
		}
         * */

		/// <summary>
		/// returns an array of the immutable PermittedExperiment objects that correspond to the supplied permittedExperimentIDs
		/// </summary>
		/// <param name="permittedExperimentIDs"></param>
		/// <returns></returns>an array of immutable objects describing the specified PermittedExperiments
		public static PermittedExperiment[] GetPermittedExperiments(int[] permittedExperimentIDs)
		{
			PermittedExperiment[] exps=DBManager.GetPermittedExperiments(permittedExperimentIDs);
			return exps;
		}
        /*
		/// <summary>
		/// retrieve unique ID of the PerimmttiedExperiment which represents the permission of executing a particular experiment in a particular time block
		/// </summary>
		/// <param name="experimentInfoID"></param>
		/// <param name="timeBlockID"></param>
		/// <returns></returns>-1 if the permission can not be retrieved
		public static int ListPermittedExperimentID(int experimentInfoID, int timeBlockID)
		{
			int i = DBManager.ListPermittedExperimentID(experimentInfoID, timeBlockID);
			return i;
		}
         * */

         /// <summary>
        /// retrieve unique ID of the PerimmttiedExperiment which represents the permission of executing a particular experiment in a particular recurrence
        /// </summary>
        /// <param name="experimentInfoID"></param>
        /// <param name="recurrenceID"></param>
        /// <returns></returns>-1 if the permission can not be retrieved
        public static int ListPermittedExperimentIDByRecur(int experimentInfoID, int recurrenceID)
        {
            int i = DBManager.ListPermittedExperimentIDByRecur(experimentInfoID, recurrenceID);
            return i;
        }
/*
		/// <summary>
		/// check whether the permission a particular time block has been given to a particular experiment  
		/// </summary>
		/// <param name="experimentInfoID"></param>
		/// <param name="timeBlockID"></param>
		/// <returns></returns>true the the permission exists. false other wise
		public static bool CheckPermission(int experimentInfoID, int timeBlockID)
		{
			bool i;

			if (DBManager.ListPermittedExperimentID(experimentInfoID, timeBlockID)==-1)
			{
				i=false;
			}
			else
			{
				i=true;
			}
			return i;

        }
*/
        #endregion

        /// <summary>
        /// enumerates the IDs of information of the permitted experiments for a particular recurrence identified by the recurrenceID
        /// </summary>
        /// <param name="recurrenceID"></param>
        /// <returns></returns>an array of ints containing the IDs of the information of the permitted experiments for a particular recurrence identified by the recurrenceID
        public static int[] ListExperimentInfoIDsByRecurrence(int recurrenceID)
        {
            int[] ids = DBManager.ListExperimentInfoIDsByRecurrence(recurrenceID);
            return ids;
        }

        public TimePeriod[] GetTimePeriods(string serviceBrokerGUID, string groupName, string ussGUID,
        string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            DBManager.GetAvailableTimePeriods(serviceBrokerGUID, groupName, ussGUID,
            labServerGuid, clientGuid, startTime, endTime);
            return null;
        }

        /// <summary>
/// retrieve available time periods(local time of LSS) overlaps with a time chrunk for a particular group and particular experiment, so that we get the a serials available time periods
/// which is the minumum available time periods set covering the time chunk
/// </summary>
/// <param name="serviceBrokeGUID"></param>
/// <param name="groupName"></param>
/// <param name="ussGUID"></param>
/// <param name="labServerGuid"></param>
/// <param name="clientGuid"></param>
/// <param name="startTime"></param>the local time of LSS
/// <param name="endTime"></param>the local time of LSS
/// <returns></returns>return an array of time periods, each of the time periods is longer than the experiment's minimum time 
        public static TimePeriod[] RetrieveAvailableTimePeriods(string serviceBrokerGUID, string groupName, string ussGUID,
             string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            List<TimePeriod> timePeriods = null;
            TimeBlock[] returnBlocks = null;
            int minTime = 1;
            int quantumMax = 1;
            int resourceID = 0;
            LssExperimentInfo exInfo = null;
            try
            {
                LssExperimentInfo[] exInfos = null;
                //get the experiment configuration
                int id = ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
                if(id >0)
                    exInfos = DBManager.GetExperimentInfos(new int[] { id });
                if (exInfos != null && exInfos.Length > 0)
                    exInfo = exInfos[0];

                //LssExperimentInfo exInfo = DBManager.GetExperimentInfo(serviceBrokerGUID, groupName, clientGuid,
                //    labServerGuid, startTime, endTime);
               
                Recurrence[] recurrences = DBManager.GetRecurrences(serviceBrokerGUID, groupName,
                    labServerGuid, clientGuid, startTime, endTime);
                if (exInfo == null || recurrences == null)
                {
                    return null;
                }
                minTime = (exInfo.prepareTime + exInfo.minimumTime + exInfo.recoverTime) * 60;

                List<TimeBlock> recurBlocks = new List<TimeBlock>();
                foreach (Recurrence rec in recurrences)
                {
                    if (resourceID == 0)
                    {
                        resourceID = rec.resourceId;
                    }
                    else if (resourceID != rec.resourceId)
                    {
                        throw new Exception(" multiple resources returned from get Recurrences");
                    }

                    TimeBlock[] recurTbs = rec.GetTimeBlocks(startTime.AddMinutes(-exInfo.prepareTime), endTime.AddMinutes(exInfo.recoverTime));

                    if (recurTbs != null)
                    {
                        quantumMax = Math.Max(quantumMax, rec.quantum);
                        recurBlocks.AddRange(recurTbs);
                    }
                }
                
                TimeBlock[] reservationTbs = DBManager.ListReservationTimeBlocks(resourceID, startTime.AddMinutes(-exInfo.prepareTime),
                        endTime.AddMinutes(exInfo.recoverTime));
                if (reservationTbs != null)
                {
                    returnBlocks = TimeBlock.Remaining(recurBlocks.ToArray(), reservationTbs);
                    returnBlocks = TimeBlock.Concatenate(returnBlocks);
                }
                else
                {
                    returnBlocks = recurBlocks.ToArray(); ;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("exception thrown in retrieving available time periods" + ex.Message);
            }
            if (returnBlocks != null)
            {
                timePeriods = new List<TimePeriod>();
                foreach (TimeBlock t in returnBlocks)
                {
                    if (t.duration >= minTime)
                    {
                        TimePeriod tp = new TimePeriod(t.startTime.AddMinutes(exInfo.prepareTime), t.End.AddMinutes(-exInfo.recoverTime));
                        tp.quantum = quantumMax;
                        timePeriods.Add(tp);
                    }
                }
                timePeriods.Sort();
            }
            if (timePeriods != null && timePeriods.Count > 0)
                return timePeriods.ToArray();
            else
                return null;
        }
        /*
             /// <summary>
             /// 
             /// </summary>
             /// <param name="serviceBrokerGUID"></param>
             /// <param name="groupName"></param>
             /// <param name="ussGUID"></param>
             /// <param name="labClientName"></param>
             /// <param name="labClientVersion"></param>
             /// <param name="startTime"></param>
             /// <param name="endTime"></param>
             /// <returns></returns>
             public static TimeBlock[] RetrieveAvailableTimeBlocks(string serviceBrokerGUID,string groupName, string ussGUID,
                 string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
             {
                 try
                 {
                     DateTime startTimeUTC = startTime.ToUniversalTime();
                     DateTime endTimeUTC = endTime.ToUniversalTime();
                     //get the experiment configuration
                     int eID = DBManager.ListExperimentInfoIDByExperiment(clientGuid, labServerGuid);
                     if (eID < 0)
                     {
                         throw new Exception("exception throw in retrieving available time periods, the experiment can not be found");
                     }
                     LssExperimentInfo exInfo = GetExperimentInfos(new int[] { eID })[0];
                     int preTime = exInfo.prepareTime;
                     int recTime = exInfo.recoverTime;
                     int minimumTime = exInfo.minimumTime;
                     string labServerGUID = exInfo.labServerGuid;
                     //get the timeblocks which are the minimum time blocks set covering the time chunk for the lab server where the experiment is executed
                     int[] tIDs = ListTimeBlockIDsByTimeChunk(serviceBrokerGUID, groupName, ussGUID, clientGuid, labServerGUID, startTimeUTC, endTimeUTC);
                     //get the IDs timeblocks in the time chunk which has been given the permission to the experiment for particular group
                     ArrayList arrayListTIDs = new ArrayList();
                     for (int i = 0; i < tIDs.Length; i++)
                     {
                    
                         if (CheckPermission(eID, tIDs[i]))
                         {
                             arrayListTIDs.Add(tIDs[i]);
                         }
                     }

                     int[] availTIDs = Utilities.ArrayListToIntArray(arrayListTIDs);
                     //get the  time blocks which are the minimum time blocks se-t covering the time chunk, and has been given the permission to the experiment for particular group
                     TimeBlock[] availTBs = DBManager.GetTimeBlocks(availTIDs);
                     return availTBs;
                 }
                 catch (Exception ex)
                 {

                     throw new Exception("exception throw in retrieving available time blocks" + ex.Message); 

                 }	

                 }

     /// <summary>
     /// given a time period defined by the start time and the end tiime, return the time slots defined by the quatum of the experiment during this time period
     /// </summary>
     /// <param name="labClientName"></param>
     /// <param name="labClientVersion"></param>
     /// <param name="startTime"></param>
     /// <param name="endTime"></param>
     /// <returns></returns>
             public static TimePeriod[] RetrieveTimeSlots(string serviceBrokerGuid, string groupName, 
                 string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
             {
                 try
                 {
                     //get the experiment configuration
                     int eID=DBManager.ListExperimentInfoIDByExperiment(clientGuid,labServerGuid);
                     if(eID < 0)
                     {
                         throw new Exception("exception throw in retrieving available time periods, the experiment can not be found"); 
                     }
                     LssExperimentInfo exInfo=GetExperimentInfos(new int[] {eID})[0];
                     //int preTime=exInfo.prepareTime;
                     //int recTime=exInfo.recoverTime;
                     int quatum = exInfo.quantum;
                     if (quatum <= 0)
                     {
                         throw new Exception("RetrieveTimeSlots: quantum must be greater than zero");
                     }
                     //DateTime expStartTime = startTime.AddMinutes(preTime);
                     //DateTime expEndTime = endTime.AddMinutes(-recTime);
                     DateTime expStartTime = startTime;
                     DateTime expEndTime = endTime;
                     ArrayList tslots = new ArrayList();
                     DateTime t = expStartTime;
                     DateTime t1 = expEndTime.AddMinutes(-quatum);
                     while(t <= t1)
                     {   
                         TimePeriod ts = new TimePeriod();
                         ts.startTime = t;
                         ts.endTime = t.AddMinutes(quatum);
                         tslots.Add(ts);
                         t = t.AddMinutes(quatum);	
                     }
                     TimePeriod[] tsArray = new TimePeriod[tslots.Count];
                     for(int i=0; i< tslots.Count;i++)
                     {
                         tsArray[i] = (TimePeriod)tslots[i];
                     }
                     return tsArray;
                 }
                 catch
                 {
                     throw;
                 }
             }
         * */

        #region ReservationInfo Methods
        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR ReservationInfo
			 * !------------------------------------------------------------------------------!
			 */
        /// <summary>
        /// add reservation information
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="labClientName"></param>
        /// <param name="labClientVersion"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>the unique ID identifying the reservation information added, >0 successfully added, -1 otherwise
        public static int AddReservationInfo(string serviceBrokerGuid, string groupName, string ussGuid,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime, int status)
        {
            return DBManager.AddReservationInfo(serviceBrokerGuid, groupName, ussGuid, labServerGuid, clientGuid, startTime, endTime, status);
        }
        /// <summary>
        /// add reservationInfo
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="credentialSetID"></param>
        /// <param name="experimentInfoID"></param>
        /// <returns></returns>the unique ID identifying the reservation information added, >0 successfully added, -1 otherwise
        public static int AddReservationInfo(DateTime startTime, DateTime endTime, int credentialSetID, int experimentInfoId, int resourceID, int status)
        {
            return DBManager.AddReservationInfo(startTime, endTime, credentialSetID, experimentInfoId, resourceID, status);
        }


        /// <summary>
        /// delete the reservation information specified by the reservationInfoIDs
        /// </summary>
        /// <param name="reservationInfoIDs"></param>
        /// <returns></returns>an array of ints containning the IDs of all reservation information not successfully removed
        public static int[] RemoveReservationInfoByIDs(int[] reservationInfoIDs)
        {
            int[] rIDs = DBManager.RemoveReservationInfoByIDs(reservationInfoIDs);
            return rIDs;
        }

        public static int RevokeReservations(int[] ids)
        {
            int count = 0;
            ReservationInfo[] info = DBManager.GetReservationInfos(ids);
            if (info != null & info.Length > 0)
            {
                foreach (ReservationInfo ri in info)
                {
                    count += RevokeReservation(ri);
                }
            }
            return count;
        }
        public static int RevokeReservation(ReservationInfo ri)
        {
            int count = 0;
            LssCredentialSet[] sets = DBManager.GetCredentialSets(new int[] { ri.credentialSetId });
            LssExperimentInfo[] exps = DBManager.GetExperimentInfos(new int[] { ri.experimentInfoId });
            if (sets != null && sets.Length > 0 && exps != null && exps.Length > 0)
            {

                USSInfo uss = DBManager.GetUSSInfo(sets[0].ussGuid);
                if (uss != null)
                {
                    ProcessAgentDB paDB = new ProcessAgentDB();
                    UserSchedulingProxy ussProxy = new UserSchedulingProxy();
                    OperationAuthHeader header = new OperationAuthHeader();
                    header.coupon = paDB.GetCoupon(uss.couponId, uss.domainGuid);
                    ussProxy.OperationAuthHeaderValue = header;
                    ussProxy.Url = uss.ussUrl;

                    int num = ussProxy.RevokeReservation(sets[0].serviceBrokerGuid, sets[0].groupName,
                        exps[0].labServerGuid, exps[0].labClientGuid, ri.Start, ri.End, "The reservation time assigned to this reservation is being removed");
                    if (num > 0)
                    {
                        LSSSchedulingAPI.RemoveReservationInfoByIDs(new int[] { ri.reservationInfoId });
                        count += num;
                    }
                }
             }
               
            return count;
        }


        /// <summary>
        /// remove the reservation information
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="labClientName"></param>
        /// <param name="labClientVersion"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>true remove successfully, false otherwise
        public static bool RemoveReservationInfo(string serviceBrokerGuid, string groupName, string ussGuid,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            bool removed = DBManager.RemoveReservationInfo(serviceBrokerGuid, groupName, ussGuid,
                labServerGuid, clientGuid, startTime, endTime);
            if (removed)
            {
                int eID = DBManager.ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
                LssExperimentInfo exInfo = GetExperimentInfos(new int[] { eID })[0];
                if (exInfo.contactEmail != null && exInfo.contactEmail.Length > 0)
                {
                    // Send a mail Message
                    StringBuilder message = new StringBuilder();

                    MailMessage mail = new MailMessage();
                    mail.To = exInfo.contactEmail;
                    mail.From = ConfigurationSettings.AppSettings["genericFromMailAddress"];
                    mail.Subject = "Removed Reservation: " + exInfo.labClientName;

                    message.Append("A reservation for " + exInfo.labClientName + " version: " + exInfo.labClientVersion);
                    message.AppendLine(" on " + exInfo.labServerName + " has been made.");
                    message.AppendLine("\tGroup: " + groupName + " ServiceBroker: " + serviceBrokerGuid);
                    message.Append("\tFrom: " + DateUtil.ToUserTime(startTime, CultureInfo.CurrentCulture, DateUtil.LocalTzOffset));
                    message.AppendLine("\t\tTo: " + DateUtil.ToUserTime(endTime, CultureInfo.CurrentCulture, DateUtil.LocalTzOffset));
                    message.AppendLine("\tDateFormat: " + CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " \tUTC Offset: " + (DateUtil.LocalTzOffset / 60.0));
                    message.AppendLine();
                    mail.Body = message.ToString();
                    SmtpMail.SmtpServer = "127.0.0.1";

                    try
                    {
                        SmtpMail.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        // Report detailed SMTP Errors
                        StringBuilder smtpErrorMsg = new StringBuilder();
                        smtpErrorMsg.Append("Exception: " + ex.Message);
                        //check the InnerException
                        if (ex.InnerException != null)
                        {
                            smtpErrorMsg.Append("<br>Inner Exceptions:");
                            while (ex.InnerException != null)
                            {
                                smtpErrorMsg.Append("<br>" + ex.InnerException.Message);
                                ex = ex.InnerException;
                            }
                            Utilities.WriteLog(smtpErrorMsg.ToString());
                        }
                    }
                }
            }
            
            return removed;
        }

        /// <summary>
        /// enumerates all IDs of the reservations made to a particular experiment identified by the experimentInfoID
        /// </summary>
        /// <param name="experimentInfoID"></param>
        /// <returns>an array of ints containing the IDs of all the reservation information made to the specified experiment</returns>
        public static int[] ListReservationInfoIDsByExperiment(int experimentInfoID)
        {
            int[] rIDs = DBManager.ListReservationInfoIDsByExperiment(experimentInfoID);
            return rIDs;
        }
        /// <summary>
        /// enumerates all IDs of the reservations made to a particular experiment from a particular group 
        /// between the start time and the end time
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="labClientName"></param>
        /// <param name="labClientVersion"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int[] ListReservationInfoIDs(string serviceBrokerGuid, string groupName, string ussGuid,
            string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            int[] rIDs = DBManager.ListReservationInfoIDs(serviceBrokerGuid, groupName, ussGuid,
                labServerGuid, clientGuid, startTime, endTime);
            return rIDs;
        }
        /// <summary>
        /// retrieve reservation made to a particular labserver during a given time chunk.
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int[] ListReservationInfoIDsByLabServer(string labServerGuid, DateTime startTime, DateTime endTime)
        {
            int[] rIDs = DBManager.ListReservationInfoIDsByLabServer(labServerGuid, startTime, endTime);
            return rIDs;
        }

        /// <summary>
        /// to select reservation Infos accorrding to given criterion
        /// </summary>
        public static ReservationInfo[] SelectReservationInfo(string labServerGuid, int experimentInfoID,
            int credentialSetID, DateTime timeAfter, DateTime timeBefore)
        {
            ReservationInfo[] reservationInfos = DBManager.SelectReservationInfo(labServerGuid, experimentInfoID,
                credentialSetID, timeAfter, timeBefore);
            return reservationInfos;
        }

        /// <summary>
        /// returns an array of the immutable ReservationInfo objects that correspond to the supplied reservationInfoIDs
        /// </summary>
        /// <param name="reservationInfoIDs"></param>
        /// <returns></returns>an array ofimmutable objects describing the specified reservations

        public static ReservationInfo[] GetReservationInfos(int[] reservationInfoIDs)
        {
            ReservationInfo[] reservationInfos = DBManager.GetReservationInfos(reservationInfoIDs);
            return reservationInfos;
        }
        public static IntTag[] ListReservations(string labServerGuid, DateTime start, DateTime end, CultureInfo culture, int userTZ)
        {
            return DBManager.ListReservations(labServerGuid, start, end, culture, userTZ);
        }
        public static IntTag[] ListReservations(int expId, int credId, DateTime start, DateTime end, CultureInfo culture, int userTZ)
        {
            return DBManager.ListReservations(expId, credId, start, end, culture, userTZ);
        }
        /// <summary>
        /// Returns an Boolean indicating whether a particular reservation from a USS is confirmed and added to the database in LSS successfully. If it fails, exception will be throw out indicating
        ///	the reason for rejection.
        /// </summary>
        /// <param name="serviceBrokerGuid"></param>
        /// <param name="groupName"></param>
        /// <param name="ussGuid"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="clientGuid"></param>
        /// <param name="startTime"></param>the local time of LSS
        /// <param name="endTime"></param>the local time of LSS
        /// <returns></returns>the notification whether the reservation is confirmed. If not, notification will give a reason
        public static string ConfirmReservation(string serviceBrokerGuid, string groupName, string ussGuid,
             string labServerGuid, string clientGuid, DateTime startTime, DateTime endTime)
        {
            string notification = null;
            try
            {

                //get the experiment configuration

                int eID = DBManager.ListExperimentInfoIDByExperiment(labServerGuid, clientGuid);
                LssExperimentInfo exInfo = GetExperimentInfos(new int[] { eID })[0];
                int preTime = exInfo.prepareTime;
                int recTime = exInfo.recoverTime;
                int minTime = exInfo.minimumTime;
                //check whether the reservation is executable
                if (((TimeSpan)endTime.Subtract(startTime)).TotalMinutes < minTime)
                {
                    notification = "The reservation time is less than the minimum time the experiment required";
                    return notification;
                }
                //the start time for the experiment equipment
                DateTime expStartTime = startTime.AddMinutes(-preTime);
                //the end time for the experiment equipment
                DateTime expEndTime = endTime.AddMinutes(recTime);
                Recurrence[] recurrences = DBManager.GetRecurrences(serviceBrokerGuid, groupName,
                    labServerGuid, clientGuid, startTime, endTime);
                if ((recurrences == null) || (recurrences.Length == 0))
                {
                    notification = "This experiment is not available to your group during this time.";
                    return notification;
                }
                else if (recurrences.Length > 1)
                {
                    notification = "More than one recurrence was found for this request. Please choose another time.";
                    return notification;
                }

                else
                {
                    TimeBlock[] reserved = DBManager.ListReservationTimeBlocks(recurrences[0].resourceId, expStartTime, expEndTime);
                    bool ok = true;
                    if ((reserved != null) && (reserved.Length > 0))
                    {

                        // Need to process the reservations to make suggestions
                        notification = "This reservation time is not available now, please adjust your reservation time.";
                        TimeBlock check = new TimeBlock(expStartTime, expEndTime);
                        foreach (TimeBlock tb in reserved)
                        {
                            TimeBlock intersection = check.Intersection(tb);
                            if (intersection != null)
                            {
                                TimeSpan span = TimeSpan.FromSeconds(intersection.Duration);
                                if(intersection.Start < expEndTime)
                                    notification += @"<br />Adjust the time by starting " + span.TotalMinutes +
                                        " minutes earlier, or make the duration shorter by " + span.TotalMinutes + " minutes.";
                                if (intersection.End > expStartTime)
                                    notification += @"<br />Adjust the time by starting " + span.TotalMinutes +
                                        " minutes later.";

                            }
                           
                        }
                        return notification;
                    }
                    //add the reservation to to reservationInfo table
                    int status = AddReservationInfo(serviceBrokerGuid, groupName, ussGuid, labServerGuid, clientGuid, startTime.ToUniversalTime(), endTime.ToUniversalTime(), 0);
                    if (status > 0)
                    {
                        notification = "The reservation is confirmed successfully";
                        if (exInfo.contactEmail != null && exInfo.contactEmail.Length > 0)
                        {
                            // Send a mail Message
                            StringBuilder message = new StringBuilder();

                            MailMessage mail = new MailMessage();
                            mail.To = exInfo.contactEmail;
                            mail.From = ConfigurationSettings.AppSettings["genericFromMailAddress"];
                            mail.Subject = "New Reservation: " + exInfo.labClientName;

                            message.Append("A new reservation for " + exInfo.labClientName + " version: " + exInfo.labClientVersion);
                            message.AppendLine(" on " + exInfo.labServerName + " has been made.");
                            message.AppendLine("\tGroup: " + groupName + " ServiceBroker: " + serviceBrokerGuid);
                            message.Append("\tFrom: " + DateUtil.ToUserTime(startTime, CultureInfo.CurrentCulture, DateUtil.LocalTzOffset));
                            message.AppendLine("\t\tTo: " + DateUtil.ToUserTime(endTime, CultureInfo.CurrentCulture, DateUtil.LocalTzOffset));
                            message.AppendLine("\tDateFormat: " + CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " \tUTC Offset: " + (DateUtil.LocalTzOffset / 60.0));
                            message.AppendLine();
                            mail.Body = message.ToString();
                            SmtpMail.SmtpServer = "127.0.0.1";

                            try
                            {
                                SmtpMail.Send(mail);
                            }
                            catch (Exception ex)
                            {
                                // Report detailed SMTP Errors
                                StringBuilder smtpErrorMsg = new StringBuilder();
                                smtpErrorMsg.Append("Exception: " + ex.Message);
                                //check the InnerException
                                if (ex.InnerException != null)
                                {
                                    smtpErrorMsg.Append("<br>Inner Exceptions:");
                                    while (ex.InnerException != null)
                                    {
                                        smtpErrorMsg.Append("<br>" + ex.InnerException.Message);
                                        ex = ex.InnerException;
                                    }
                                    Utilities.WriteLog(smtpErrorMsg.ToString());
                                }
                            }
                        }
                    }
                    else
                        notification = "Error: AddReservation status = " + status;
                    return notification;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return notification;
        }

        #endregion

        #region Recurrence Methods
        /* !------------------------------------------------------------------------------!
         *							CALLS FOR Recurrence
         * !------------------------------------------------------------------------------!
         */

        public static int AddRecurrence(Recurrence recur)
        {
            return AddRecurrence(recur.startDate, recur.numDays, (int) recur.recurrenceType,
                recur.startOffset, recur.endOffset, recur.quantum, recur.resourceId, recur.dayMask);
        }
        /// <summary>
        /// add recurrence
        /// </summary>
        /// <param name="recurrenceStartDate"></param>
        /// <param name="recurrenceEndDate"></param>
        /// <param name="recurrenceType"></param>
        /// <param name="recurrenceStartTime"></param>
        /// <param name="recurrenceDuration"></param>
        /// <param name="labServerGuid"></param>
        /// <param name="resourceID"></param>
        /// <returns></returns>the uniqueID which identifies the recurrence added, >0 was successfully added; ==-1 otherwise
        public static int AddRecurrence(DateTime recurrenceStartDate, int numDays, 
            int recurrenceType, TimeSpan startOffset, TimeSpan endOffset, int quantum,
            int resourceID,byte dayMask)
        {
            int recurrenceID = DBManager.AddRecurrence(recurrenceStartDate, numDays, recurrenceType,
                startOffset, endOffset, quantum, resourceID, dayMask);
            return recurrenceID;
        }

        /// <summary>
        /// delete the recurrence specified by the recurrenceIDs
        /// </summary>
        /// <param name="recurrenceIDs"></param>
        /// <returns></returns>status indicating the number of reservations deleted or an error
        public static int RemoveRecurrence(int recurrenceID)
        {
            int status = 0;
            int count = 0;
            int errors = 0;
            Recurrence[] recurs = DBManager.GetRecurrences(new int[] { recurrenceID });
            if (recurs != null && recurs.Length >0){
                DateTime now = DateTime.UtcNow;
                if(recurs[0].End > now){
                    int[] reservationIds = DBManager.ListReservationInfoIDsByLabResource(recurs[0].resourceId, now, recurs[0].End);
                    if (reservationIds != null && reservationIds.Length > 0)
                    {
                        TimeBlock[] remaining = recurs[0].GetTimeBlocks(now, recurs[0].End);
                        ReservationInfo[] infos = DBManager.GetReservationInfos(reservationIds);
                        foreach (TimeBlock tb in remaining)
                        {
                            foreach (ReservationInfo ri in infos)
                            {
                                if (tb.Intersects(ri))
                                {
                                    count += LSSSchedulingAPI.RevokeReservation(ri);
                                }
                            }
                        }
                    }
                }
                status = DBManager.RemoveRecurrence(recurrenceID);
            }
            return status;
        }

        /// <summary>
        /// Returns an array of the immutable Recurrence objects that correspond ot the supplied Recurrence IDs
        /// </summary>
        /// <param name="timeBlockIDs"></param>
        /// <returns></returns>an array of immutable objects describing the specified Recurrence
        public static Recurrence[] GetRecurrence(int[] recurrenceIDs)
        {
            Recurrence[] recur = DBManager.GetRecurrences(recurrenceIDs);
            return recur;
         
        }
        /// Enumerates the IDs of the information of all the Recurrence 
        /// </summary>
        /// <returns></returns>the array  of ints contains the IDs of the information of all the Recurrence
        public static int[] ListRecurrenceIDs()
        {
            int[] recurIDs = DBManager.ListRecurrenceIDs();
            return recurIDs;
        }
 /*         /// <summary>
        /// enumerates all IDs of the recurrences belonging to a particular lab server identified by the labserverID
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the time blocks of specified lab server
        public static int[] ListRecurrenceIDsByLabServer(string labServerGuid)
        {
            int[] recurIDs = DBManager.ListRecurrenceIDsByLabServer(labServerGuid);
            return recurIDs;
        }

        /// <summary>
        /// enumerates all IDs of the recurrences belonging to a particular lab server identified by the labserverID
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the time blocks of specified lab server
        public static int[] ListRecurrenceIDsByLabServer(DateTime start, DateTime end, string labServerGuid)
        {
            int[] recurIDs = DBManager.ListRecurrenceIDsByLabServer(start, end, labServerGuid);
            return recurIDs;
        }
*/
        /// <summary>
        /// enumerates all IDs of the recurrences belonging to a particular lab server identified by the labserverID
        /// </summary>
        /// <param name="labServerGuid"></param>
        /// <returns></returns>an array of ints containing the IDs of all the time blocks of specified lab server
        public static int[] ListRecurrenceIDsByResourceID(DateTime start, DateTime end, int resourceID)
        {
            int[] recurIDs = DBManager.ListRecurrenceIDsByResourceID(start, end, resourceID);
            return recurIDs;
        }
        #endregion

        #region USSInfo Methods

        /* !------------------------------------------------------------------------------!
			 *							CALLS FOR USSInfo
			 * !------------------------------------------------------------------------------!
			 */
        /// <summary>
        /// Add information of a particular user side scheduling server identified by ussID 
        /// </summary>
        /// <param name="ussGuid"></param>
        /// <param name="ussName"></param>
        /// <param name="ussURL"></param>
        /// <param name="couponID">ID of the RevokeReservation ticket Coupon, coupon should be in the database</param>
        /// <param name="issuerGuid">Issuer of the RevokeReservation ticket Coupon</param>
        /// <returns></returns>The unique ID which identifies the experiment information added. >0 was successfully added; ==-1 otherwise   
        public static int AddUSSInfo(string ussGuid, string ussName, string ussURL, long couponID, string issuerGuid)
        {
            int[] ussInfoIDs = ListUSSInfoIDs();
            USSInfo[] ussInfo = GetUSSInfos(ussInfoIDs);

            foreach (USSInfo info in ussInfo)
            {
                if (ussGuid.Equals(info.ussGuid))
                    return -1;
            }

            int uID = DBManager.AddUSSInfo(ussGuid, ussName, ussURL, couponID, issuerGuid);
            return uID;
        }

        /// <summary>
        /// Updates the data fields for the USS information specified by the ussInfoID; note ussInfoID may not be changed 
        /// </summary>
        /// <param name="ussInfoID"></param>
        /// <param name="ussID"></param>
        /// <param name="ussName"></param>
        /// <param name="ussURL"></param>
        /// <returns></returns>true if modified successfully, false otherwise
        public static int ModifyUSSInfo(int ussInfoID, string ussGUID, string ussName, string ussURL, long couponID, string issuerGuid)
        {
            int i = DBManager.ModifyUSSInfo(ussInfoID, ussGUID, ussName, ussURL, couponID, issuerGuid);
            return i;

        }

        /// <summary>
        /// Delete the uss information specified by the ussInfoIDs. 
        /// </summary>
        /// <param name="ussInfoIDs"></param>
        /// <returns></returns>An array of USS information IDs specifying the USS information to be removed
        public static int[] RemoveUSSInfo(int[] ussInfoIDs)
        {
            int[] uIDs = DBManager.RemoveUSSInfo(ussInfoIDs);
            return uIDs;
        }

        /// <summary>
        /// Enumerates the IDs of the information of all the USS 
        /// </summary>
        /// <returns></returns>the array  of ints contains the IDs of the information of all the USS
        public static int[] ListUSSInfoIDs()
        {
            int[] uIDs = DBManager.ListUSSInfoIDs();
            return uIDs;

        }
        /// <summary>
        /// Enumerates the ID of the information of a particular USS specified by ussID
        /// </summary>
        /// <param name="ussGuid"></param>
        /// <returns></returns>the ID of the information of a particular USS 
        public static int ListUSSInfoID(string ussGuid)
        {
            int i = DBManager.ListUSSInfoID(ussGuid);
            return i;

        }

        /// <summary>
        /// Returns an array of the immutable USSInfo objects that correspond to the supplied USS information IDs. 
        /// </summary>
        /// <param name="ussInfoIDs"></param>
        /// <returns></returns>An array of immutable objects describing the specified USS information; if the nth ussInfoID does not correspond to a valid experiment scheduling property, the nth entry in the return array will be null
        public static USSInfo[] GetUSSInfos(int[] ussInfoIDs)
        {
            USSInfo[] ussInfos = DBManager.GetUSSInfos(ussInfoIDs);
            return ussInfos;

        }

        #endregion
    }

}
