/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id: Administration.cs,v 1.26 2008/03/17 16:21:49 pbailey Exp $ */

using System;
using System.Collections;
using System.Data;
using System.Text;

using iLabs.Core;
using iLabs.ServiceBroker;
//using iLabs.ServiceBroker.DataStorage;
using iLabs.ServiceBroker.Authorization;
using iLabs.ServiceBroker.Internal;
//using iLabs.Services;

using iLabs.DataTypes;
using iLabs.DataTypes.ProcessAgentTypes;
using iLabs.DataTypes.TicketingTypes;
using iLabs.UtilLib;

using iLabs.Ticketing;
using iLabs.TicketIssuer;
using iLabs.ServiceBroker;

namespace iLabs.ServiceBroker.Administration
{

	/// <summary>
	/// The User struct possesses a number of fixed fields, the 
    /// common denominator of all possible user descriptions. 
    /// But it will also possess a field that implements an XML-based 
    /// extension mechanism. This field should conform to an XML schema 
    /// registered with the Service Broker, and implementation specific 
    /// elements and attributes that can be coded in the XML Extension String.
	/// </summary>
	public struct User
	{
		/// <summary>
		/// The integer ID of the User.
		/// </summary>
		public int userID;

		/// <summary>
		/// The string user name (this would be analagous to a kerberos name).
		/// </summary>
		public string userName;

		/// <summary>
		/// The user's first name.
		/// </summary>
		public string firstName;

		/// <summary>
		/// The user's last name.
		/// </summary>
		public string lastName;

		/// <summary>
        /// The user's email address.
		/// </summary>
		public string email;

		/// <summary>
		/// The user's affiliation; usually, but not necessarily, their university.
		/// </summary>
		public string affiliation;

		/// <summary>
		/// The text supplied by a user to explain his or her request for an account.
		/// </summary>
		public string reason;

		/// <summary>
		/// Date and time the user's account was first registered.
		/// </summary>
		public DateTime registrationDate;

		/// <summary>
		/// A boolean which is set to true if the user's account is locked.
		/// </summary>
		public bool lockAccount;

		/// <summary>
		/// A field that implements an XML-based extension mechanism. 
		/// This field should conform to an XML schema registered with the Service Broker, 
        /// and implementation specific elements and attributes that can be coded in the XML Extension String.
		/// </summary>
		public string xmlExtension;

	}


	/// <summary>
	/// Structure containing information pertaining to a Lab Client.
	/// </summary>
	public struct LabClient
	{
        /// <summary>
        /// A string limited to 50 characters used for identification across domains, 
        /// do not modifiy except at creation.
        /// </summary>
        public string clientGuid;

		/// <summary>
		/// The integer ID of the Lab Client.
		/// </summary>
		public int clientID;

		/// <summary>
		/// A name for the client application meaningful to humans; 
        /// it is not required to be unique on the Service Broker instance. 
		/// </summary>
		public string clientName;

		/// <summary>
		/// The version number of the client software, versions change 
        /// when the client interface changes. 
		/// </summary>
		public string version;

		/// <summary>
		/// An array of ClientInfo structures containing (potentially) multiple 
        /// instances of information associated with this clientID. Information 
        /// such as the client name and the URL of an information page are maintained.
		/// </summary>
		/// <seealso cref="ClientInfo">ClientInfo Structure</seealso>
		public ClientInfo[] clientInfos;

		/// <summary>
		/// A brief description of the client suitable for listing in a GUI. 
		/// </summary>
		public string clientShortDescription;

		/// <summary>
		/// A longer more descriptive text explaining the purpose of the client. 
		/// </summary>
		public string clientLongDescription;

		/// <summary>
		/// An arbitrary piece of text that will be visible to students using the client.
		/// </summary>
		public string notes;

		/// <summary>
		/// An HTML fragment that will be embedded on a Service Broker generated page 
        /// executed by the user’s web browser to launch the client. 
		/// </summary>
		public string loaderScript;

		/// <summary>
		/// The type of lab client. Current values are: 
		/// 1-Applet Client; 2-HTML Redirect Client. 
		/// These are extensible by adding records to the Client_Types table in 
        /// the database. In the current batched experiment implementation, 
        /// the authorization mechanism differs according to the type of client.
		/// </summary>
		public string clientType;

		/// <summary>
		/// An array of labServerIDs specifying the ordered list of lab servers 
        /// accessed by this client. 
		/// </summary>
		public int[] labServerIDs;

		/// <summary>
		/// The first name of the party responsible for the maintenance of the client.  
		/// </summary>
		public string contactFirstName;
		/// <summary>
		/// The last name of the party responsible for the maintenance of the client. 
		/// </summary>
		public string contactLastName;
		
		/// <summary>
        /// The email address of the party responsible for the maintenance of the client. 
		/// </summary>
		public string contactEmail;

        /// <summary>
        /// True if the client requires an ESS.
        /// </summary>
        public bool needsESS;

        /// <summary>
        /// True if the client needs to be scheduled.
        /// </summary>
        public bool needsScheduling;

        /// <summary>
        /// True if the client may be re-entered. This implies that a new 
        /// experimentID will not be created for the client. 
        /// </summary>
        public bool IsReentrant;

		/// <summary>
		/// Property for the Client Name.
		/// This is needed in order for a Repeater object to display the value in the clientName field. 
		/// </summary>
		public string ClientName
		{
			get{ return clientName; } 
		}

		/// <summary>
		/// Property for the Client Short Description.
		/// This is needed in order for a Repeater object to display 
        /// the value in the clientShortDescription field. 
		/// </summary>
		public string ClientShortDescription
		{
			get{ return clientShortDescription; } 
		}

        /// <summary>
        /// Named constant for the Client type.
        /// </summary>
        public const string BATCH_APPLET = "Batch Applet";

        /// <summary>
        /// Named constant for the Client type.
        /// </summary>
        public const string BATCH_HTML_REDIRECT = "Batch Redirect";

        /// <summary>
        /// Named constant for the Client type.
        /// </summary>
        public const string INTERACTIVE_APPLET = "Interactive Applet";

        /// <summary>
        /// Named constant for the Client type.
        /// </summary>
        public const string INTERACTIVE_HTML_REDIRECT = "Interactive Redirect";

        /// <summary>
        /// Named constant for the Client type.
        /// </summary>
        public const string WEB_SERVICE_REDIRECT = "Web Service Redirect";
	}

	/// <summary>
	/// A structure containing information associated with a particular Lab Client.
	/// </summary>
	public struct ClientInfo
	{
		/// <summary>
		/// The integer ID of the ClientInfo record.
		/// </summary>
		public int clientInfoID;

		/// <summary>
		/// The URL of an info page for this Client.
		/// </summary>
		public string infoURL;

		/// <summary>
		/// The name associated with the URL specified in infoURL.
		/// </summary>
		public string infoURLName;

		/// <summary>
		/// The description associated with the URL specified in infoURL. 
		/// </summary>
		public string description;

		/// <summary>
		/// A numeric value pertaining to the order in which this particular ClientInfo record should be displayed in a GUI.
		/// This value is a unique entry in a collection of other ClientInfo records that belong to the same Lab Client.
		/// </summary>
		public int displayOrder;

		/// <summary>
		/// Property for the Info URL. 
		/// This is needed in order for a Repeater object to display the value in the infoURL field. 
		/// </summary>
		public string InfoURL
		{
			get{ return infoURL; } 
		}

		/// <summary>
		/// Property for the Info URL Name. 
		/// This is needed in order for a Repeater object to display the value in the infoURLName field. 
		/// </summary>
		public string InfoURLName
		{
			get{ return infoURLName; } 
		}
		
		/// <summary>
		/// Property for the Description. 
		/// This is needed in order for a Repeater object to display the value in the description field. 
		/// </summary>
		public string Description
		{
			get{ return description; } 
		}
	}

   public class ExperimentAdminInfo
    {
        public long experimentID;
        public int userID;
        public int groupID;         
        public int agentID;
         public int clientID;
         public int essID;
         public int status;
         public int recordCount;
         public long duration;
         public DateTime startTime;
         public DateTime creationTime;
         public DateTime closeTime;
         public string annotation;
     }


	/// <summary>
	/// A structure containing information pertaining to a particular group.
	/// </summary>
	public struct Group
	{
		/// <summary>
		/// The integer ID of the group. Must be unique in a namespace shared with User instances.
		/// </summary>
		public int groupID;

		/// <summary>
		/// A string to be used in administrative listings to identify the group on the Service Broker. Must be unique in a namespace shared with User instances.
		/// </summary>
		public string groupName;

		/// <summary>
		/// A brief description of the Group suitable for display in a table or a GUI. 
		/// </summary>
		public string description;

		/// <summary>
		/// The email contact for group administration.
		/// </summary>
		public string email;

		/// <summary>
		/// The type of group. Current values are: 
		/// 1-Regular Group; 2-Request Group; 3-Course Staff Group. 
		/// These are extensible by adding records to the Group_Types table in the database.
		/// In the current batched experiment implementation, the authorization mechanism differs according to the type of group.
		/// </summary>
		public string groupType;

		/// <summary>
		/// Property for the Group ID. 
		/// This is needed in order for a Repeater object to display the value in the groupID field. 
		/// </summary>
		public int GroupID
		{
			get{ return groupID; } 
		}

		/// <summary>
		/// Property for the Group Name. 
		/// This is needed in order for a Repeater object to display the value in the groupName field. 
		/// </summary>
		public string GroupName
		{
			get{ return groupName; } 
		}

		/// <summary>
		/// Property for the Description. 
		/// This is needed in order for a Repeater object to display the value in the description field. 
		/// </summary>
		public string Description
		{
			get{ return description; } 
		}

        /// <summary>
        /// Property for the Group Name. 
        /// This is needed in order for a Repeater object to display the value in the groupName field. 
        /// </summary>
        public string GroupType
        {
            get { return groupType; }
        }

		/// <summary>
		/// 
		/// </summary>
		public const string UNKNOWN = "Unknown Group";
		//public const int UNKNOWN_ID = 0;
		/// <summary>
		/// Named constant for the Root of a group hierarchy.
		/// </summary>
		public const string ROOT = "ROOT";
		//public const int ROOT_ID = 1;
		/// <summary>
		/// Named constant for the Orphaned User Group in the group hierarchy. 
		/// This is for groups which are children of parents that have been deleted. 
		/// </summary>
		public const string ORPHANEDGROUP = "OrphanedUserGroup";
		//public const int ORPHANEDGROUP_ID = 3;
		/// <summary>
		/// Named constant for the New User Group. 
		/// This is a special group into which all new users are assigned. 
		/// It is a temporary holding place that the user occupies while waiting for an administrator to place them into their actual group or groups.
		/// </summary>
		public const string NEWUSERGROUP = "NewUserGroup";
		//public const int NEWUSERGROUP_ID = 2;
		/// <summary>
		/// Named constant for the Super User Group. 
		/// This is the group for Administrators who posess all system privileges.
		/// </summary>
		public const string SUPERUSER = "SuperUserGroup";
		//public const int SUPERUSER_ID = 4;

	}

    public class GroupInfo
    {
        public int id = -1;
        public string name;
        public string description;
        public string email;
        public int request = -1;
        public int collectionNode = -1;
        public int admin = -1;
    }

    /// <summary>
    /// A class to hold session recovery data.
    /// </summary>
    public class SessionInfo
    {
        /// <summary>
        /// The long integer Session ID.
        /// </summary>
        public long sessionID;

        /// <summary>
        /// ID of the User whose session is represented by the UserSession object.
        /// </summary>
        public int userID;

        /// <summary>
        /// Current Effective Group of the current UserSession.
        /// </summary>
        public int groupID;

        /// <summary>
        /// ClientID of the current UserSession, may be null.
        /// </summary>
        public int clientID;

        /// <summary>
        /// tzOffset offset from GMT in minutes.
        /// </summary>
        public int tzOffset;

        /// <summary>
        /// The users name
        /// </summary>
        public string userName;
        /// <summary>
        /// The current group
        /// </summary>
        public string groupName;


    }
  
	
	/// <summary>
	/// A structure containing information pertaining to the User Session.
    /// Primary use is for log-in reporting.
	/// </summary>
	public struct UserSession
	{
		/// <summary>
		/// The long integer Session ID.
		/// </summary>
		public long sessionID;

        /// <summary>
        /// ID of the User whose session is represented by the UserSession object.
        /// </summary>
        public int userID;

        /// <summary>
        /// Current Effective Group of the current UserSession.
        /// </summary>
        public int groupID;

        /// <summary>
        /// ClientID of the current UserSession, may be null.
        /// </summary>
        public int clientID;

        /// <summary>
        /// tzOffset offset from GMT in minutes.
        /// </summary>
        public int tzOffset;

		/// <summary>
		/// Date/Time the session started.
		/// </summary>
		public DateTime sessionStartTime;

		/// <summary>
		/// Date/Time the session ended.
		/// </summary>
		public DateTime sessionEndTime;

		/// <summary>
		/// The Session Key.
		/// </summary>
		public string sessionKey;
      

		
	}

	/// <summary>
	/// A structure which holds System Messages.
	/// </summary>
	public struct SystemMessage
	{
		/// <summary>
		/// The integer ID of the message.
		/// </summary>
		public int messageID;

		/// <summary>
		/// The type of the message. Current values are: 
		/// 1-System; 2-Group; and 3-System. 
		/// These are extensible by adding records to the Message_Types table in the database.
		/// </summary>
		public string messageType; //Changed to is_global in the database. Is_Global = 1, refers to a system message.

		/// <summary>
		/// true if the message is to be displayed.
		/// </summary>
		public bool toBeDisplayed;
        /// <summary>
        /// Client that should display this message.
        /// </summary>
        public int clientID;
		/// <summary>
		/// Effective Group of the user who created this message.
		/// </summary>
		public int groupID;

		/// <summary>
		/// ID of the ProcessAgent which this message refers to.
		/// </summary>
		public int agentID;

		/// <summary>
		/// Date/Time of the last modification of the message.
		/// </summary>
		public DateTime lastModified;

		/// <summary>
		/// Body of the message.
		/// </summary>
		public string messageBody;

		/// <summary>
		/// Title of the message.
		/// </summary>
		public string messageTitle;

		/// <summary>
		/// Property for the Message Body.
		/// This is needed in order for a Repeater object to display the value in the messageBody field. 
		/// </summary>
		public string MessageBody
		{
			get{ return messageBody; } 
		}

		/// <summary>
		/// Property for Last Modified. 
		/// This is needed in order for a Repeater object to display the value in the lastModified field. 
		/// </summary>
		public DateTime LastModified
		{
			get{ return lastModified; } 
		}

		/// <summary>
		/// Named constant for the System message type.
		/// </summary>
		public const string SYSTEM = "System";

		/// <summary>
		/// Named constant for the Group message type.
		/// </summary>
		public const string GROUP = "Group";

		/// <summary>
		/// Named constant for the Lab message type.
		/// </summary>
		public const string LAB = "Lab";

        public static int CompareDateAsc(SystemMessage m1, SystemMessage m2)
        {
            return m1.lastModified.CompareTo(m2.lastModified);
        }
        public static int CompareDateDesc(SystemMessage m1, SystemMessage m2)
        {
            return m2.lastModified.CompareTo(m1.lastModified);
        }
	}

    public class DateComparer : IComparer
    {
        public int Compare(object o1, object o2)
        {
            DateTime date1 = ((SystemMessage)o1).lastModified;
            DateTime date2 = ((SystemMessage)o2).lastModified;

            return date1.CompareTo(date2);
        }
    }

	public class AdministrativeAPI
	{
		public AdministrativeAPI()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        /***** 
         * ProcessAgent Administrative methods actual DB calls should be in either TicketIssuerDB or InternalDBAdmin API.
         *****/

        /// <summary>
        /// Registers the service identified by Name with the Service Broker.
        /// The method is responsible for creating the Qualifiers used to access the service.
        /// May be used for both domain & non-domain services ( ProcessAgents ).
        /// After this call users may be assigned grants to the service.
        /// </summary>
        /// <param name="guid">The GUID by which lab server identifies itself in web service calls.</param>
        /// <param name="name">A name for the service meaningful to humans; it is not required to be unique on the Service Broker instance.</param>       
        /// <param name="type"></param>
        /// <param name="issuerGuid">The service's domain ServiceBroker Guid</param>
        /// <param name="codeBaseURL">The URL for the lab server's web root directory.</param>
        /// <param name="serviceUrl">The fully qualified WebService page</param>
        /// <param name="inCoupon">Optional, not specified for a Service outside of the domain</param>
        /// <param name="outCoupon">Optional, not specified for a Service outside of the domain</param>
        /// <returns>The unique ID which identifies the ProcessAgent internally to the Service Broker.</returns>
        public static int AddProcessAgent(string guid, string name, string type, string domainGuid,
            string codeBaseUrl, string serviceUrl, Coupon inCoupon, Coupon outCoupon)
        {
            int agentID = -1;

            // if an exception is thrown, return false, otherwise true
            try
            {
                TicketIssuerDB ticketing = new TicketIssuerDB();
                //Insert the lab server into the database
                agentID = ticketing.InsertProcessAgent(guid, name, type, domainGuid, codeBaseUrl, serviceUrl,
                    inCoupon, outCoupon);
                string typeUpper = type.ToUpper();
                try
                {
                    int qualifierType = 0;
                    //Add the processAgent to the Qualifiers & Qualifier_Hierarchy table
                    switch (typeUpper)
                    {
                        case ProcessAgentType.SERVICE_BROKER:
                        case ProcessAgentType.BATCH_SERVICE_BROKER:
                        case ProcessAgentType.REMOTE_SERVICE_BROKER:
                            qualifierType = Qualifier.serviceBrokerQualifierTypeID;
                            break;
                        case ProcessAgentType.LAB_SERVER:
                        case ProcessAgentType.BATCH_LAB_SERVER:
                            qualifierType = Qualifier.labServerQualifierTypeID;
                            break;
                        case ProcessAgentType.EXPERIMENT_STORAGE_SERVER:
                            qualifierType = Qualifier.storageServerQualifierTypeID;
                            break;
                        case ProcessAgentType.LAB_SCHEDULING_SERVER:
                            qualifierType = Qualifier.labSchedulingQualifierTypeID;
                            break;
                        case ProcessAgentType.SCHEDULING_SERVER:
                            qualifierType = Qualifier.userSchedulingQualifierTypeID;
                            break;
                        default:
                            break;
                    }

                    Authorization.AuthorizationAPI.AddQualifier(agentID, qualifierType, name, Qualifier.ROOT);
                }
                catch (Exception ex)
                {
                    // rollback lab server insertion
                    InternalAdminDB.DeleteProcessAgents(new int[] { agentID });

                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return agentID;
        }

        /// <summary>
        /// Unregisters the lab servers identified by labServerIDs with the service Broker. 
        /// After this call users can no longer submit experiments to the designated lab servers.
        /// </summary>
        /// <param name="labServerIDs">An array of IDs identifying the lab servers to be unregistered.</param>
        /// <returns>An array of labServerIDs for the lab servers that were not successfully removed, i.e. those for which the operation failed.</returns>
        public static int[] RemoveProcessAgents(int[] agentIDs)
        {
            try
            {
                //return removed labServers from Lab_server table
                int[] lsIDs = InternalAdminDB.DeleteProcessAgents(agentIDs);

                return lsIDs;
            }
            catch (Exception ex)
            {
                throw;
            }

            //Note: The qualifiers pertaining to the lab server are automatically
            //deleted in the 'DeleteLabServer' database method. This preserves consistency
            //and gets rid of unnecessary rollback mechanisms that would otherwise have
            //to be implemented. - CV, 4/29/05
        }

	
		///*********************** Services **************************///


		/// <summary>
		/// Returns an array of the immutable LabServer objects for the registered lab servers whoes IDs are supplied in labServerIDs.
		/// </summary>
		/// <param name="labServerIDs">The IDs identifying the registered lab servers whose information is to be retrieved.</param>
		/// <returns>An array of LabServer objects describing the registered lab servers specified in labServerIDs; if the nth labServer ID does not correspond to a valid lab server, the nth entry in the return array will be null.</returns>
		/// <seealso cref="LabServer">LabServer Class</seealso>
		public static ProcessAgent[] GetLabServers()
		{
            
			return new ProcessAgentDB().GetProcessAgentsByType(ProcessAgentType.LAB_SERVER);
		}

		/// <summary>
		/// Returns the integer ID of the lab server given the labserver GUID
		/// </summary>
		public static int GetLabServerID(string labServerGUID)
		{
			return new ProcessAgentDB().GetProcessAgentID(labServerGUID);
		}

		/// <summary>
		/// Returns the integer ID of the lab server given the experiment ID
		/// </summary>
		/// <param name="experimentID">A unique integer which identifies an experiment in the database</param>
		/// <returns>The integer LabServerID for the Lab Server on which the experiment ran.</returns>
		public static int GetLabServerID(long experimentID)
		{
            return InternalAdminDB.SelectLabServerID(experimentID);
		}


		///*********************** LAB SERVERS **************************///
        /*
                /// <summary>
                /// Registers the lab server identified by labServerName with the Service Broker.
                /// After this call users can submit experiments to the designated Lab Server.
                /// </summary>
                /// <param name="labServerName">A name for the lab server meaningful to humans; it is not required to be unique on the Service Broker instance.</param>
                /// <param name="labServerGUID">The GUID by which lab server identifies itself in web service calls.</param>
                /// <param name="webServiceURL">The URL for the lab server's web service interface.</param>
                /// <param name="labServerDescription">A brief description of the lab suitable for listing in a GUI.</param>
                /// <param name="labInfoURL">The URL of a lab information page, often hosted by the lab server.</param>
                /// <param name="contactEmail">The email address of the party responsible for the maintenance of the lab server.</param>
                /// <param name="contactFirstName">The first name of the party responsible for the maintenance of the lab server.</param>
                /// <param name="contactLastName">The last name of the party responsible for the maintenance of the lab server.</param>
                /// <returns>The unique ID which identifies the lab server internally to the Service Broker.</returns>
                public static int AddLabServer (string labServerName, string labServerGUID, string webServiceURL, string labServerDescription, string labInfoURL, string contactEmail, string contactFirstName, string contactLastName)
                {
                    LabServer ls = new LabServer ();
                    ls.labServerName = labServerName;
                    ls.labServerGUID = labServerGUID;
                    ls.webServiceURL = webServiceURL;
                    ls.labServerDescription = labServerDescription;
                    ls.labInfoURL = labInfoURL;
                    ls.contactEmail = contactEmail;
                    ls.contactFirstName = contactFirstName;
                    ls.contactLastName = contactLastName;
			
                    // if an exception is thrown, return false, otherwise true
                    try 
                    {
                        //Insert the lab server into the database
                        ls.labServerID = InternalAdminDB.InsertLabServer (ls);

                        try
                        {
                            //Add the labserver to the Qualifiers & Qualifier_Hierarchy table
                            Authorization.Authorization .AddQualifier (ls.labServerID, Authorization.Qualifier .labServerQualifierTypeID , ls.labServerName, Qualifier.ROOT);
                        }
                        catch (Exception ex)
                        {
                            // rollback lab server insertion
                            InternalAdminDB.DeleteLabServers(new int[]{ls.labServerID});

                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    return ls.labServerID;
                }

                /// <summary>
                /// Unregisters the lab servers identified by labServerIDs with the service Broker. 
                /// After this call users can no longer submit experiments to the designated lab servers.
                /// </summary>
                /// <param name="labServerIDs">An array of IDs identifying the lab servers to be unregistered.</param>
                /// <returns>An array of labServerIDs for the lab servers that were not successfully removed, i.e. those for which the operation failed.</returns>
                public static int[] RemoveLabServers( int[] labServerIDs)
                {
                    try
                    {
                        //return removed labServers from Lab_server table
                        int[] lsIDs = InternalAdminDB.DeleteLabServers (labServerIDs);

                        return lsIDs;
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    //Note: The qualifiers pertaining to the lab server are automatically
                    //deleted in the 'DeleteLabServer' database method. This preserves consistency
                    //and gets rid of unnecessary rollback mechanisms that would otherwise have
                    //to be implemented. - CV, 4/29/05
                }

                /// <summary>
                /// Modifies the information pertaining to the lab server registered under labServerID.
                /// </summary>
                /// <param name="labServerID">The ID of the lab server whose information will be modified.</param>
                /// <param name="labServerName">A name for the lab server meaningful to humans; it is not required to be unique on the Service Broker instance, but probably should be; if NULL, then the previous value will not be changed.</param>
                /// <param name="labServerGUID">The GUID by which lab server indentifies itself in web service calls.</param>
                /// <param name="webServiceURL">The new URL for the lab server's web service interface; if NULL, then the previous value will not be changed.</param>
                /// <param name="labServerDescription">A brief description of the lab suitable for listing in a GUI; if NULL, then the previous value will not be changed.</param>
                /// <param name="labInfoURL">The URL of the lab information page, often hosted by the lab server; if NULL, the the previous value will not be changed.</param>
                /// <param name="contactEmail">The email consumerInfo of the party responsible for the maintenance of the lab server; if NULL, then the previous value will not be changed.</param>
                /// <param name="contactFirstName">The first name of the party responsible for the maintenance of the lab server; if NULL, then the previous value will not be changed.</param>
                /// <param name="contactLastName">The last name of the party responsible for the maintenance of the lab server; if NULL, then the previous value will not be changed.</param>
                public static void ModifyLabServer (int labServerID, string labServerName, string labServerGUID, string webServiceURL, string labServerDescription, string labInfoURL, string contactEmail, string contactFirstName, string contactLastName)
                {
                    LabServer ls = new LabServer ();
                    ls.labServerID = labServerID;
                    ls.webServiceURL = webServiceURL;
                    ls.labServerGUID=labServerGUID;
                    ls.labServerDescription = labServerDescription;
                    ls.labInfoURL = labInfoURL;
                    ls.labServerName = labServerName;
                    ls.contactEmail = contactEmail;
                    ls.contactFirstName = contactFirstName;
                    ls.contactLastName = contactLastName;
                    try 
                    {
                        //Modify lab server database call
                        InternalAdminDB.UpdateLabServer (ls);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
        */
        /// <summary>
        /// Lists the IDs of all lab servers registered with the Service Broker.
        /// </summary>
        /// <returns>An array of LabServerIDs for all registered lab servers.</returns>
        public static int[] ListLabServerIDs()
        {
            return new ProcessAgentDB().GetProcessAgentIDsByType((int) ProcessAgentType.AgentType.LAB_SERVER);
        }
/*
		/// <summary>
		/// Generates and installs a new passkey that will be used to authenticate web service calls made by the remote server identified by labServerID.
		/// The passkey returned must be conveyed out of band to the administrator of the remote server.
		/// </summary>
		/// <param name="labServerID">The ID of the (lab) server for which the passkey will be generated.</param>
		/// <returns>The passkey authenticating the remote server.</returns>
		/// <seealso cref="GetIncomingServerPasskey">GetIncomingServerPasskey</seealso>
		public static string GenerateIncomingServerPasskey(int labServerID)
		{
			// Random rand = new Random ();  // dynamic seeds
			// string key = rand.Next ().ToString ();

			// Changed from random number to a GUID - CF 1/21/2005
			string key = Guid.NewGuid().ToString("N");  // with no "-" in Guid string 

			try
			{
				InternalAdminDB.UpdateLSIncomingPasskey (labServerID, key);
			}
			catch (Exception ex)
			{
				throw;
			}
			return key;
		}

		/// <summary>
		/// Retrieves a previously generated passkey to authenticate web service calls made by the remote server identified by labServerID.
		/// </summary>
		/// <param name="labServerID">The ID of the (lab) server for which the passkey will be retrieved.</param>
		/// <returns>The passkey authenticating the remote server.</returns>
		/// <seealso cref="GenerateIncomingServerPasskey">GenerateIncomingServerPasskey</seealso>
		public static string GetIncomingServerPasskey(int labServerID)
		{
			string incomingKey = "";
			try
			{
				incomingKey = InternalAdminDB.selectSBIncomingPasskey (labServerID);
			}
			catch(Exception ex)
			{	
				throw;
			}
			return incomingKey;
		}

		/// <summary>
		/// Installs a per lab server passkey that authenticates this service broker.
		/// </summary>
		/// <param name="labServerID">The ID of the lab server for which the passkey will be installed.</param>
		/// <param name="passkey">The passkey or token that this Service Broker must present on each web service call to gain access to the lab server.</param>
		public static void RegisterOutgoingServerPasskey(int labServerID, string passkey)
		{
			try 
			{
				InternalAdminDB.UpdateLSOutgoingPasskey (labServerID, passkey);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieve a previously registered passkey that this service broker must present on each web service call to gain access to the lab server identified by labServerID.
		/// </summary>
		/// <param name="labServerID">The ID of the (lab) server for which the passkey will be retrieved.</param>
		/// <returns>The passkey for the remote server.</returns>
		public static string GetOutgoingServerPasskey(int labServerID)
		{
			string outgoingKey = "";
			try
			{
				outgoingKey = InternalAdminDB.selectSBOutgoingPasskey (labServerID);
			}
			catch(Exception ex)
			{	
				throw;
			}
			return outgoingKey;
		}
*/

		///*********************** LAB CLIENTS **************************///
		

		/// <summary>
		/// Registers the client identified by ClientName with the Service Broker; after this call users can request the loading of this client to compose and execute experiments on the associated lab servers.
		/// </summary>
        /// <param name="guid">A string limited to 50 characters used for identification across domains, do not modifiy except at creation.</param>
		/// <param name="clientName">A name for the client application meaningful to humans; it is not required to be unique on the Service Broker.</param>
		/// <param name="version">The version number of the client software; each new version must receive a new clientID.</param>
		/// <param name="notes">An arbitrary piece of text that will be visible to students using the client.</param>
		/// <param name="loaderScript">An HTML fragment that will be embedded on a Service Broker generated page executed by the user's web browser to launch the client.</param>
		/// <param name="clientShortDescription">A brief description of the client suitable for listing in a GUI.</param>
		/// <param name="clientLongDescription">A longer more descriptive text explaining the purpose of the client.</param>
		/// <param name="clientType">A string identifying the client type.</param>
		/// <param name="labServerIDs">An array of labServerIDs specifying the ordered list of lab servers accessed by this client.</param>
        /// <param name="contactEmail">The email address of the party responsible for the maintenance of the client.</param>
		/// <param name="contactFirstName">The first name of the party responsible for the maintenance of the client.</param>
		/// <param name="contactLastName">The last name of the party responsible for the maintenance of the client.</param>
		///<param name="clientInfos">An array of ClientInfo structures containing multiple instances of information associated with this clientID. Information such as the client name and the URL of an information page are maintained. It this value is NULL, the previous value will not be changed.</param>
		/// <returns>The unique clientID which identifies the client software internally to the Service Broker. >0 was successfully registered; -1 otherwise</returns>
        public static int AddLabClient(string guid, string clientName, string version, string clientShortDescription, string clientLongDescription, string notes, string loaderScript, string clientType, int[] labServerIDs, string contactEmail, string contactFirstName, string contactLastName, bool needsScheduling, bool needsESS, bool isReentrant, ClientInfo[] clientInfos)
		{
			LabClient lc = new LabClient ();
            lc.clientGuid = guid;
			lc.clientName = clientName;
			lc.version = version;
			lc.notes=notes;
			lc.clientShortDescription=clientShortDescription;
			lc.clientLongDescription=clientLongDescription;
			lc.loaderScript=loaderScript;
			lc.labServerIDs = labServerIDs;
			lc.contactEmail = contactEmail;
			lc.contactFirstName = contactFirstName;
			lc.contactLastName = contactLastName;
            lc.needsScheduling = needsScheduling;
            lc.needsESS = needsESS;
            lc.IsReentrant = isReentrant;
			lc.clientInfos=clientInfos;
			lc.clientType= clientType;

			try
			{
				//Insert the lab client into the database
				lc.clientID = InternalAdminDB.InsertLabClient (lc);

				try
				{
					//Add the lab client to the Qualifiers & Qualifiers Hierarchy Table
					Authorization.AuthorizationAPI .AddQualifier (lc.clientID, Authorization.Qualifier .labClientQualifierTypeID, lc.clientName, Qualifier.ROOT);
				}
				catch (Exception ex)
				{
					// rollback lab client insertion
					InternalAdminDB.DeleteLabClients(new int[]{lc.clientID});

					throw;
				}
			}			
			catch (Exception ex)
			{
				throw;
			}

			return lc.clientID;
		}

		/// <summary>
		/// Unregisters the lab clients identified by labClientIDs with the service broker; after this call users can no longer load the specified clients.
		/// </summary>
		/// <param name="labClientIDs">The IDs identifying the lab clients to be unregistered.</param>
		/// <returns>An array of LabClientIDs for the lab clients that were not successfully removed, i.e. those for which the operation failed.</returns>
		public static int[] RemoveLabClients (int[] labClientIDs)
		{
			try
			{
				// delete from the lab clients table
				int[] lcIDs = InternalAdminDB.DeleteLabClients (labClientIDs);
				return lcIDs;
			}
			catch (Exception ex)
			{
				throw;
			}

			//Note: The qualifiers pertaining to the lab server are automatically
			//deleted in the 'DeleteLabServer' database method. This preserves consistency
			//and gets rid of unnecessary rollback mechanisms that would otherwise have
			//to be implemented. - CV, 4/29/05
		}

		/// <summary>
		/// Modifies the information pertaining to the client registered under the ID clientID.
		/// </summary>
		/// <param name="clientID">The unique ID which identifies the lab client whose information will be removed.</param>
		/// <param name="clientName">A name for the client application meaningful to humans; it is not required to be unique on the Service Broker instance, but probably should be; if NULL, the the previous value will not be changed.</param>
		/// <param name="version">The version number of the client software; if NULL, the previous value will not be changed.</param>
		/// <param name="clientShortDescription">A brief description of the client suitable for listing in a GUI; if NULL, then the previous value will not be changed.</param>
		/// <param name="clientLongDescription">A longer more descriptive text explaining the purpose of the client; if NULL, then the previous value will not be changed.</param>
		/// <param name="notes">An arbitrary piece of text that will be visible to students using the client; if NULL, then the previous value will not be changed.</param>
		/// <param name="loaderScript">An HTML fragment that will be enbedded on the Service Broker generated page executed by the user's web browser to launch the client; if  NULL, then the previous value will not be changed.</param>
		/// <param name="clientType">A string identifying the client type.</param>
		/// <param name="labServerIDs">An array of labServerIDs specifying the ordered list of lab servers accessed by this client; if NULL, then the previous value will not be changed.</param>
        /// <param name="contactEmail">The email address of the party responsible for the maintenance of the client; if NULL, the previous value will not be changed.</param>
		/// <param name="contactFirstName">The first name of the party responsible for the maintenance of the client; if NULL, the previous value will not be changed.</param>
		/// <param name="contactLastName">The last name of the party responsible for the maintenance of the client; if NULL, the previous value will not be changed.</param>
		/// <param name="clientInfos">An array of ClientInfo structures containing multiple instances of information associated with this clientID. Information such as the client name and the URL of the information page are maintained. If this value is Null, the previous value will not be changed.</param>
        public static void ModifyLabClient(int clientID, string clientName, string version, string clientShortDescription, string clientLongDescription, string notes, string loaderScript, string clientType, int[] labServerIDs, string contactEmail, string contactFirstName, string contactLastName, bool needsScheduling, bool needsESS, bool isReentrant, ClientInfo[] clientInfos)
		{
			LabClient lc = new LabClient ();
			lc.clientID = clientID;
			lc.clientName = clientName;
			lc.version = version;
			lc.clientShortDescription = clientShortDescription;
			lc.clientLongDescription = clientLongDescription;
			lc.notes = notes;
		    lc.loaderScript = loaderScript;
			lc.clientType = clientType;
			lc.labServerIDs = labServerIDs;
			lc.contactEmail = contactEmail;
			lc.contactFirstName = contactFirstName;
			lc.contactLastName = contactLastName;
            lc.needsScheduling = needsScheduling;
            lc.needsESS = needsESS;
            lc.IsReentrant = isReentrant;
			lc.clientInfos=clientInfos;

			try
			{
				InternalAdminDB.UpdateLabClient (lc);
			}			
			catch(Exception e)
			{
				throw;
			}
		}

		/// <summary>
		/// Lists the IDs of all lab clients registered with the service broker.
		/// </summary>
		/// <returns>An array of LabClientIDs for all registered lab clients.</returns>
		public static int[] ListLabClientIDs()
		{
			return InternalAdminDB.SelectLabClientIDs ();
		}

        /// <summary>
        /// Returns a labClient.
        /// </summary>
        /// <param name="labClientIDs">The IDs identifying the registered lab clients whose information is to be retrieved.</param>
        /// <returns>An array of LabClient objects describing the registered lab clients specified in labClientIDs; if the nth LabClient ID does not correspond to a valid lab client, the nth entry in the return array will be null.</returns>
        public static LabClient GetLabClient(int labClientID)
        {
            return InternalAdminDB.SelectLabClient(labClientID);
        }

        /// <summary>
        /// Returns a labClient.
        /// </summary>
        /// <param name="labClientIDs">The IDs identifying the registered lab clients whose information is to be retrieved.</param>
        /// <returns>An array of LabClient objects describing the registered lab clients specified in labClientIDs; if the nth LabClient ID does not correspond to a valid lab client, the nth entry in the return array will be null.</returns>
        public static LabClient GetLabClient(string clientGuid)
        {
            return InternalAdminDB.SelectLabClient(clientGuid);
        }
		/// <summary>
		/// Returns an array of the immutable labClient objects for the registered lab clients whose IDs are supplied in labClientIDs.
		/// </summary>
		/// <param name="labClientIDs">The IDs identifying the registered lab clients whose information is to be retrieved.</param>
		/// <returns>An array of LabClient objects describing the registered lab clients specified in labClientIDs; if the nth LabClient ID does not correspond to a valid lab client, the nth entry in the return array will be null.</returns>
		public static LabClient[] GetLabClients(int[] labClientIDs)
		{
			return InternalAdminDB.SelectLabClients (labClientIDs);
		}

        /// <summary>
        /// Returns the immutable labClient object for the specified Guid.
        /// </summary>
        /// <param name="clientGuid">The guid identifying the registered lab client whose information is to be retrieved.</param>
        /// <returns>A LabClient object describing the registered lab client specified; if the labClient GUID does not correspond to a valid lab client, null</returns>
        public static int GetLabClientID(string clientGuid)
        {
            return InternalAdminDB.SelectLabClientId(clientGuid);
        }

     public static string GetLabClientName(int id)
        {
            return InternalAdminDB.GetLabClientName(id);
        }

        public static string GetLabClientGUID(int id)
        {
            return InternalAdminDB.GetLabClientGUID(id);
        }
        public static int CountServerClients(int groupID, int labServerID){
            return InternalAdminDB.CountServerClients(groupID, labServerID);
    }

        ///*********************** CLIENT ITEMS **************************///


        /// <summary>
        /// Saves the value of a client data item for the specified client and user.
        /// </summary>
        /// <param name="clientID">The ID of the client implementation.</param>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="itemName">The name under which the data item is to be saved.</param>
        /// <param name="itemValue">The value to be saved.</param>
        public static void SaveClientItemValue(int clientID, int userID, string itemName, string itemValue)
        {
            try
            {
                InternalAdminDB.SaveClientItem(clientID, userID, itemName, itemValue);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the value of the specified client data items for the designed client and user.
        /// </summary>
        /// <param name="clientID">The name of the client implementation.</param>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="itemNames">An array of item names for the data items to be retrieved.</param>
        /// <returns>The current values of the data items specified in itemNames (and in the same order); if an item named in itemNames is not recognized, a null will be returned at that position in the array.</returns>
        public static string[] GetClientItemValue(int clientID, int userID, string[] itemNames)
        {
            string[] itemValues = new string[itemNames.Length];

            for (int i = 0; i < itemNames.Length; i++)
            {
                itemValues[i] = InternalAdminDB.SelectClientItemValue(clientID, userID, itemNames[i]);
            }
            return itemValues;
        }

        /// <summary>
        /// Deletes client data items and their values for the specified client and user.
        /// </summary>
        /// <param name="clientID">The ID of the client implementation.</param>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="itemNames">The array of names of items to be removed.</param>
        /// <returns>An array of item names that does not exist for the specified client and user.</returns>
        public static string[] RemoveClientItems(int clientID, int userID, string[] itemNames)
        {
            ArrayList aList = new ArrayList();
            for (int i = 0; i < itemNames.Length; i++)
            {
                try
                {
                    InternalAdminDB.DeleteClientItem(clientID, userID, itemNames[i]);
                }
                catch
                {
                    //aList.Add(itemNames[i]);
                    throw;
                }
            }
            string[] unRemovedItemNames = Utilities.ArrayListToStringArray(aList);

            return unRemovedItemNames;
        }

        /// <summary>
        /// List the names of all cient data items for the given client and user.
        /// </summary>
        /// <param name="clientID">The ID of the client implementation.</param>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>The array of string item names for the given client and user.</returns>
        public static string[] ListClientItems(int clientID, int userID)
        {
            return InternalAdminDB.SelectClientItems(clientID, userID);
        }


		///*********************** USERS **************************///
		
		/// <summary>
		/// Registers a new user with the supplied information; makes the user a member of the group specified by initialGroupID.
		/// </summary>
		/// <param name="userName">A string to be used in administrative listings to identify the user on the Service Broker. Must be unique in a namespace shared with Group instance.</param>
		/// <param name="principalString">The string the user presents as his or her identifier to the mechanism used to authenticate the user.</param>
		/// <param name="authenticationType">Determines the authentication module, native or supported external services, used to authenticate this user.</param>
		/// <param name="firstName">The first name of the user.</param>
		/// <param name="lastName">The last name of the user.</param>
        /// <param name="email">The email address of the user.</param>
		/// <param name="affiliation">The affiliation of the user.</param>
		/// <param name="reason">The text supplied by the user to explain his or her request for an account.</param>
		/// <param name="xmlExtension">The XML extension string containing further information about the user.</param>
		/// <param name="initialGroupID">The Group to which this User will be initially assigned.</param>
		/// <param name="lockAccount">Indicates whether the user to be created will have his or her account locked.</param>
		/// <returns>The unique userID which identifies the user internally to the Service Broker.>0 if the user was unknown and was successfully registered;==-1 otherwise. In order for 
		/// this to happen, all the following must succeed:
		/// 1. userID must not be an element of the current User and Group namespace 
		/// 2. authenticationType must designate a supported type of authentication 
		/// 3. principalID must be unique within the specified authentication type 
		/// 4. xmlExtension must be successfully validated against the current User XML Extension schema specified by the SetUserXMLExtensionSchema method 
		/// 5. initialGroupID must designate a Group registered with the Service Broker</returns>
		public static int AddUser (string userName, string principalString, string authenticationType, string firstName, string lastName, string email, string affiliation,string reason, string xmlExtension, int initialGroupID, bool lockAccount)
		{
			User user = new User ();
			user.userName = userName;
			user.firstName = firstName;
			user.lastName = lastName;
			user.email = email;
			user.reason=reason;
			user.affiliation = affiliation;
			user.xmlExtension = xmlExtension;
			user.lockAccount = lockAccount;
			
			try
			{
				//Inserts agents too. See InternalDB method for details.

				// Add user to the database
				user.userID = InternalAdminDB.InsertUser (user, principalString, authenticationType, initialGroupID);
			}
			catch (Exception ex)
			{
				throw;
			}

			return user.userID;

		}
		
		/// <summary>
		/// Unregisters the users specified by userIDs.
		/// </summary>
		/// <param name="userIDs">An array of user IDs specifying the users to be removed.</param>
		/// <returns>An array containing the userIDs of all Users not successfully unregistered, i.e., those for whom the operation failed.</returns>
		/// <remarks>
		/// Comments from InternalAdminDB.DeleteUsers (userIDs):
		///	* IMPORTANT ! - The database is currently not set to Cascade delete for Agents and Users.
		///	* Hence the stored procedure implements the following functionality:
		///	* 1. When a user (specified by userID) is to be deleted, the agent is first deleted.
		///	* 2. This cascade deletes the records in the Agent Hierarchy and Grants tables (which has to be manually done in the code if cascade delete doesn't work).
		///	* 3. Then the User is deleted from the Users Table
		///	* 4. This cascade deletes the entries in the Principals table (which has to be manually done in the code if cascade delete doesn't work).
		///	* 5. This cascade deletes the entries in the Experiment_Information, Client_Items and User_sessions tables
		///
		/// </remarks>
		public static int[] RemoveUsers (int[] userIDs)
		{
			int[] unRemovedUsers = InternalAdminDB.DeleteUsers (userIDs);
			return unRemovedUsers;
		}

		/// <summary>
		/// Updates the data fields for the User specified by UserID; note that userID may not be changed.
		/// </summary>
		/// <param name="userID">The user ID whose data is being changed.</param>
		/// <param name="userName">A string to be used in administrative listings to identify the user on the Service Broker; If NULL, then the previous value will not be changed.</param>
		/// <param name="principalString">The string the user presents as his or her identifier when authenticating him or herself; if NULL, then the previous value will not be changed.</param>
		/// <param name="authenticationType">Determines the authentication module, native or supported external services, used to authenticate this user; if NULL, the previous value will not be changed.</param>
		/// <param name="firstName">The new contents of the firstName field; if NULL, then the previous value will not be changed.</param>
		/// <param name="lastName">The new contens of the lastName field; if NULL, then the previous value will not be changed.</param>
		/// <param name="email">The new contents of the email field; if NULL, then the previous value will not be changed.</param>
		/// <param name="affiliation">The new contents of the affiliation field; if NULL, then the previous value will not be changed.</param>
		/// <param name="reason">The text supplied by the user to explain his or her request for an account; If NULL, then the previous value will not be changed.</param>
		/// <param name="xmlExtension">The new contents of the xmlExtension field; if NULL, then the previous value will not be changed.</param>
		/// <param name="lockAccount">Indicates whether the user to be modified has locked accout.</param>
		/// <remarks>The user's registrationDate cannot be modified.
		/// A "No record exists exception" is thrown if no record exists in the database to be modified.
		/// </remarks>
		public static void ModifyUser (int userID,string userName, string principalString, string authenticationType, string firstName, string lastName, string email, string affiliation, string reason,string xmlExtension, bool lockAccount)
		{
			User user = new User ();
			user.userID = userID;
			user.userName = userName;
			user.firstName = firstName;
			user.lastName = lastName;
			user.reason = reason;
			user.email = email;
			user.affiliation = affiliation;
			user.xmlExtension = xmlExtension;
			user.lockAccount = lockAccount;

			try
			{
				// modified by charu on 5/22/04, since principal id should not be passed here. refer method for more information.
				InternalAdminDB.UpdateUser (user, principalString, authenticationType);
			}
			catch(Exception e)
			{
				throw;
			}
		}
		
		/// <summary>
		/// Enumerates all users registered with the Service Broker.
		/// </summary>
		/// <returns>An array of ints containing the IDs of all known Users registered with the Service Broker.</returns>
		public static int[] ListUserIDs()
		{
			return InternalAdminDB.SelectUserIDs ();
		}

		/// <summary>
		/// Enumerates all orphaned users registered with the service broker; an orphaned user is a user that no longer belongs to a Group.
		/// </summary>
		/// <returns>An array containing the userIDs of all known orphaned Users registered with the Service Broker.</returns>
		/// <remarks>Currently implemented as belong to "OrphanedUserGroup" group.</remarks>
		public static int[] ListOrphanedUserIDs()
		{
			return InternalAdminDB.SelectOrphanedUserIDs ();
		}

		/// <summary>
		/// Returns an array of the immutable user objects that correspond to the supplied user IDs.
		/// </summary>
		/// <param name="userIDs">The IDs identifying the users whose information is being requested.</param>
		/// <returns>An array of the immutable User objects describing the specified users; if the nth user ID does not correspond to a valid user, the nth entry in the return array will be null.</returns>
		public static User[] GetUsers(int[] userIDs)
		{
			User[] users = InternalAdminDB.SelectUsers (userIDs);
			return users;
		}

		/// <summary>
		/// Returns the integer userID that corresponds to the supplied username.
		/// </summary>
		/// <param name="userName">The userName identifying the user whose user ID is requested.</param>
		/// <returns>The integer userID of the requested user.</returns>
		public static int GetUserID (string userName)
		{
			return InternalAdminDB.SelectUserID(userName);
		}

        /// <summary>
        /// Returns the integer userID that corresponds to the supplied username.
        /// </summary>
        /// <param name="userName">The userName identifying the user whose user ID is requested.</param>
        /// <returns>The integer userID of the requested user.</returns>
        public static string GetUserName(int userID)
        {
            return InternalAdminDB.SelectUserName(userID);
        }


		///*********************** GROUPS **************************///
		

		/// <summary>
		/// Creates the group GroupName in the Service Broker user/group namespace.
		/// </summary>
		/// <param name="groupName">A string to be used in adminstrative listings to identify the group on the Service Broker. Must be unique in a namespace shared with User instances.</param>
		/// <param name="parentGroupID">The ID of the new group’s parent group. If less than or equal to 0, the new group is created as a top level group parented off the root of the group hierarchy.</param>
		/// <param name="description">A brief description of the Group suitable for display in a table or a GUI.</param>
		/// <param name="email">The contact email for group administration.</param>
		/// <returns>The unique groupID which identifies this group internally to the Service Broker.>0 if the group was unknown and was successfully registered; ==-1 otherwise.</returns>
		/// <remarks>The method will first create the group in the agents table & 
		/// then create an entry in the groups table (using the agentID).
		/// Then it will create a group qualifier and an experiment collection qualifier associated with the group
		/// </remarks>

		public static int AddGroup (string groupName, int parentGroupID, string description, string email, string groupType, int associatedGroupID)
		{
			Group g = new Group ();
			g.groupName = groupName;
			g.description = description;
			g.email=email;
			g.groupType=groupType;

			int RootGroupID = GetGroupID (Group.ROOT);
			int parentQualifierID = Qualifier.ROOT;
			int parentECQualifierID = Qualifier.ROOT;
			
			// If the parent group ID is <=0 then the default parent group is ROOT (ROOT groupID = 1)
			if (parentGroupID <=0)
				parentGroupID = RootGroupID;

			// If the parent Group is not ROOT then get its qualifierIDs (otherwise the parentQualifier is just Root)
			if (parentGroupID != RootGroupID)
			{
				parentQualifierID = Authorization.AuthorizationAPI.GetQualifierID(parentGroupID, Qualifier.groupQualifierTypeID);

				//Request groups are added to new user group (which doesn't have an experiment collection)
				int NewUserGroupID = GetGroupID (Group.NEWUSERGROUP);
				if (parentGroupID != NewUserGroupID)
				{
					parentECQualifierID = Authorization.AuthorizationAPI.GetQualifierID(parentGroupID,Qualifier.experimentCollectionQualifierTypeID);
				}
			}

			try 
			{
                // Add group to database

				//Inserts agents too. See InternalDB method for details.
				g.groupID  = InternalAdminDB.InsertGroup(g,parentGroupID, associatedGroupID);
				
				try
				{
					// add into Qualifier table as well as Q-H table
					if (parentQualifierID >0)
						Authorization.AuthorizationAPI .AddQualifier (g.groupID, Authorization.Qualifier .groupQualifierTypeID , g.groupName, parentQualifierID);
                    // Only insert experiment collection Qualifier for regular groups
					if (parentECQualifierID >0 && (groupType.CompareTo(GroupType.REGULAR) == 0))
						Authorization.AuthorizationAPI.AddQualifier(g.groupID,Authorization.Qualifier.experimentCollectionQualifierTypeID,g.groupName+ " Experiment Collection",parentECQualifierID);
				}
				catch (Exception ex)
				{
					// rollback group insertion
					InternalAdminDB.DeleteGroups(new int[]{g.groupID});

					throw;
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			return g.groupID;
		}

		/// <summary>
		/// Destroys the groups identified by groupIDs but does nothing to the group members except remove their membership in the destroyed group.
		/// Notes: Removing a group will usually have effects on other parts of the user management system. In the current Service Broker implementation, removing a group triggers a cascading delete on all records of experiments that were executed with that effective group. Deleting a group DOES NOT delete users, however, even if the users do not belong to another group. When a user belongs to only one group and that group is deleted, the user is moved into the “Orphaned Users Group? The intention here is to provide business logic support for end of semester cleanup (removing experiment records) while still allowing student accounts to be preserved across semesters.
		/// </summary>
		/// <param name="groupIDs">The IDs of the Groups to be destroyed.</param>
		/// <returns>An array of group IDs for the Groups that were not successfully destroyed, i.e. those for which the operation failed.</returns>
		/// <remarks>
		/// Comments from InternalAdminDB.DeleteGroups (groupIDs):
		/// 	
		///			 * IMPORTANT ! - The database if currently not set to Cascade delete for Agents and Groups.
		///			 * Hence the stored procedure implements the following functionality:
		///			 * 1. When a group (specified by groupID) is to be deleted, the agent is first deleted.
		///			 * 2. This cascade deletes the records in the Agent Hierarchy and Grants tables (which has to be manually done in the code if cascade delete doesn't work)
		///			 * 3. Then the Group is deleted from the Groups Table
		///	         * 4. This cascade deletes the entries in the Experiment_Information, System_Messages and User_sessions tables
		///	         * 5. Finally the corresponding group entries are deleted from the Qualifiers Table (and hence Qualifier_Hierarchy by cascade delete)
		///	         * 6. The Experiment_Collection qualifier of the group is also deleted from the Qualifiers Table
		///
		///IMPORTANT! When a group is deleted all the experiments that were 
		///run as part of the group are also deleted!
		///
		///NOTE: CURRENTLY THERE IS NO ROLLBACK MECHANISM IMPLEMENTED IN THIS METHOD BETWEEN THE 3 MAIN STEPS:
		/// RemoveMembers, DeleteExperiments, DeleteGroups since it's very hard to do the rollback. This will hopefully become a little
		/// more efficient when DeleteExperiments can be done in 1 call in the ESSAPI - CV, 5/17/2005
		/// 
		/// </remarks>
		public static int[] RemoveGroups(int[] groupIDs)
		{
			try
			{
				//First call RemoveMembersFromGroup to remove all the group's children
				
				// Notes from RemoveMembersFromGroup
				//		* 1. When a user is to be removed from a group, he/she is either
				//			- Moved into orphaned users group (if they are part of only 1 group,
				//				 from which they're being removed) OR,
				//			- Just removed from the group (relationship severed in agent hierarchy table)
				//				if they're part of multiple groups
				//				
				//		* 2. When a subgroup is to be removed from a group it is either,
				//			- Moved to ROOT (if it is part of only one group from which it's being removed).
				//				Consequently, the corresponding group qualifier is also moved under the 
				//				Qualifier root & the corresponding experiment collection qualifier 
				//				is also moved under Qualifier root, OR
				//			- Just removed from the group (relationship severed in agent hierarchy table)
				//				if they're part of multiple groups. The corresponding group and
				//				experiment collection qualifier relationships are also removed.

				for (int i= 0; i< groupIDs.Length; i++)
				{
					int[] unRemovedMemberIDs = RemoveMembersFromGroup(ListMemberIDsInGroup(groupIDs[i]), groupIDs[i]);
				}

				//Delete all experiments related to a group using the ESS API.
                long[] groupExperimentIDs = InternalDataDB.SelectGroupExperimentIDs(groupIDs);
                InternalDataDB.DeleteExperiments(groupExperimentIDs);

				//Deleting a group automatically removes the group qualifier
				//and the associated experiment collection qualifier from the qualifiers table
				//This also removes all associated experiment information records, system messages and user session records
				int[] unDeletedGroupIDs = InternalAdminDB.DeleteGroups (groupIDs);

				return unDeletedGroupIDs;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Modifies the description of the group groupID in the Service Broker User/Group namespace.
		/// </summary>
		/// <param name="groupID">The ID of the group whose description will be modified.</param>
		/// <param name="groupName">A string to be used in administrative listings to identify the group on the Service Broker; If NULL, then the previous value will not be changed.</param>
		/// <param name="description">The new brief description of the Group suitable for display in a table or a GUI.</param>
		/// <param name="email">The new email contact for Group administration; if NULL, then the previous value will not be changed.</param>
		public static void ModifyGroup(int groupID,string groupName, string description, string email)
		{
			Group g = new Group ();
			g.groupID = groupID;
			g.groupName = groupName;
			g.description = description;
			g.email = email;

			try 
			{
				InternalAdminDB.UpdateGroup (g);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Lists the IDs of all groups registered with the Service Broker.
		/// </summary>
		/// <returns>An array of all registered group IDs.</returns>
		public static int[] ListGroupIDs()
		{
			return InternalAdminDB.SelectGroupIDs();
		}
        /// <summary>
        /// Lists the IDs of all admin groups registered with the Service Broker.
        /// </summary>
        /// <returns>An array of all admin registered group IDs.</returns>
        public static int[] ListAdminGroupIDs()
        {
            return InternalAdminDB.SelectAdminGroupIDs();
        }

		/// <summary>
		/// Returns an array of the immutable Group objects for the registered groups whose IDs are supplied in groupIDs.
		/// </summary>
		/// <param name="groupIDs">The IDs identifying the registered groups whose information is to be retrieved.</param>
		/// <returns>An array of Group objects describing the groups specified in groupIDs; if the nth group ID does not correspond to a valid group, the nth entry in the return array will be null.</returns>
		/// <seealso cref="Group">Group Class</seealso>
		public static Group[] GetGroups(int[] groupIDs)
		{
			return InternalAdminDB.SelectGroups (groupIDs);
		}

		/// <summary>
		/// Obtains a GroupID, given the group name.
		/// </summary>
		/// <param name="groupName">The name of the group whose ID is to be obtained.</param>
		/// <returns>The integer ID of the group.</returns>
		public static int GetGroupID (string groupName)
		{
			return InternalAdminDB.SelectGroupID(groupName);
		}

        /// <summary>
        /// Obtains a GroupName, given the group ID.
        /// </summary>
        /// <param name="groupID">The ID of the group whose name is to be obtained.</param>
        /// <returns>Name of the group, or null if not found</returns>
        public static string GetGroupName(int groupID)
        {
            return InternalAdminDB.SelectGroupName(groupID);
        }
		/// <summary>
		/// Obtains the associated GroupID, given the group ID. A TA Group or Request 
		/// group is associated with another group which it administers / is the request group of.
		/// </summary>
		/// <param name="groupID">The ID of the group whose associated group ID is to be obtained.</param>
		/// <returns>The integer ID of the associated group.</returns>
		public static int GetAssociatedGroupID (int groupID)
		{
			return InternalAdminDB.SelectAssociatedGroupID(groupID);
		}


		///*********************** GROUP MEMBERSHIP **************************///


		/// <summary>
		/// Add a user or subgroup to the specified group
		/// </summary>
		/// <param name="memberID">The ID of the user or subgroup to be added.</param>
		/// <param name="groupID">The ID of the Group to which the user or subgroup is to be added.</param>
		/// <returns>true if groupID is a registered Group, if memberID identifies a previously registered user or group, if the user or subgroup is not already a member of the specified Group, and if the user or subgroup was successfully added to the Group; false otherwise.</returns>
		/// <remarks>
		/// Notes from InternalAdminDB:
		/// * The stored procedure implements the following functionality:
		/// * 1. When a user is to be added a group, he/she is
		///		- Removed into orphaned users group (if they exist there), AND
		///		- Added to the group in the agent hierarchy
		///	* 2. When a subgroup is to be added to a group it is,
		///		- The qualifier corresponding to the subgroup is added under the qualifier corresponding to the parent group
		///		- If the subgroup has an experiment collection node, it is moved under the experiment collection node of the parent(if one exists)
		///			except if the parent is ROOT, in which case the subgroup experiment collection node is added under Qualifier ROOT
		///		- Added to the group in the agent hierarchy
		/// </remarks>
		public static bool AddMemberToGroup(int memberID, int groupID)
		{
			if( IsAgentMember(groupID, memberID))    // prevents cycles in the AgentHierarchy
				return false;
			else
			{
				bool inserted = false;

				try 
				{
					//Add member to group in database
					inserted = InternalAdminDB.InsertMemberInGroup (memberID, groupID);
				}
				catch (Exception ex)
				{
					throw;
				}
				return inserted;
			}
		}

        /// </summary>
        /// <param name="memberID">The ID of the user or subgroup to be added.</param>
        /// <param name="groupID">The ID of the Group to which the user or subgroup is to be added.</param>
        /// <returns>true if groupID is a registered Group, if memberID identifies a previously registered user or group, if the user or subgroup is not already a member of the specified Group, and if the user or subgroup was successfully added to the Group; false otherwise.</returns>
        /// <remarks>
        /// Notes from InternalAdminDB:
        /// * The stored procedure implements the following functionality:
        /// * 1. When a user is to be added a group, he/she is
        ///		- Removed into orphaned users group (if they exist there), AND
        ///		- Added to the group in the agent hierarchy
        ///	* 2. When a subgroup is to be added to a group it is,
        ///		- The qualifier corresponding to the subgroup is added under the qualifier corresponding to the parent group
        ///		- If the subgroup has an experiment collection node, it is moved under the experiment collection node of the parent(if one exists)
        ///			except if the parent is ROOT, in which case the subgroup experiment collection node is added under Qualifier ROOT
        ///		- Added to the group in the agent hierarchy
        /// </remarks>
        public static bool MoveMemberToGroup(int memberID, int fromID, int toID)
        {
            if (IsAgentMember(toID, memberID))    // prevents cycles in the AgentHierarchy
                return false;
            else
            {
                bool inserted = false;

                try
                {
                    //Add member to group in database
                    inserted = InternalAdminDB.MoveMemberToGroup(memberID, fromID, toID);
                }
                catch
                {
                    throw;
                }
                return inserted;
            }
        }

		/// <summary>
		/// Removes user and subgroups from the specified group.
		/// </summary>
		/// <param name="memberIDs">The IDs of the users and subgroups to be removed from the group.</param>
		/// <param name="groupID">The ID of the Group from which the members are to be removed.</param>
		/// <returns>An array of the members who were not successfully removed from the group, i.e., the ones for which the operation failed.</returns>
		/// <remarks>
		/// Returns an array of the members who were not successfully removed from the group, i.e. that ones for which the operation failed.
		/// If memberID is a user 
		///   1. if it has multiple parents, delete the memberID-groupID relationship only in agentHierarchy; 
		///   2. if it has only one parent, i.e. groupID, then remove the memberID-groupID relationship and set its parent a member to "OrphanedUserGroup"; 
		/// If memberID is a group 
		///   1. if it has multiple parents, delete the memberID-groupID relationship only in agentHierarchy; 
		///   2. if it has only one parent, i.e. groupID, get its lists of childen and call recurssion , the last step is to remove the memberID-groupID relationship; 
		///   
		///   
		///   Notes from InternalAdminDBAPI:
		///   Deleting a member from the group
		///
		///		* IMPORTANT ! 
		///		* The stored procedure implements the following functionality:
		///		* 1. When a user is to be removed from a group, he/she is either
		///			- Moved into orphaned users group (if they are part of only 1 group,
		///				 from which they're being removed) OR,
		///			- Just removed from the group (relationship severed in agent hierarchy table)
		///				if they're part of multiple groups
		///				
		///		* 2. When a subgroup is to be removed from a group it is either,
		///			- Moved to ROOT (if it is part of only one group from which it's being removed).
		///				Consequently, the corresponding group qualifier is also moved under the 
		///				Qualifier root and the corresponding experiment collection qualifier 
		///				is also moved under Qualifier root, OR
		///			- Just removed from the group (relationship severed in agent hierarchy table)
		///				if they're part of multiple groups. The corresponding group and
		///				experiment collection qualifier relationships are also removed.
		///
		///   </remarks>
		public static int[] RemoveMembersFromGroup(int[] memberIDs, int groupID)
		{
			ArrayList aList = new ArrayList ();

			try
			{
				for (int i=0; i<memberIDs.Length ; i++)
				{
					bool deleted = InternalAdminDB.DeleteMemberFromGroup (memberIDs[i], groupID);

					if (!deleted)
						aList.Add(memberIDs[i]);
				}

				int[] unremovedMembers = Utilities.ArrayListToIntArray(aList);
			
				return unremovedMembers;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns an array of user IDs for all users in the specified group.
		/// </summary>
		/// <param name="groupID">The ID of the Group whose users are to be listed.</param>
		/// <returns>An array containing the user IDs of all users belonging to the Group.</returns>
		public static int[] ListUserIDsInGroup(int groupID)
		{
			ArrayList arrayList = new ArrayList ();
			try
			{
				//Lists all members of a group as agent structs
				Agent[] agent = InternalAdminDB.SelectMembersInGroup (groupID);

				for (int i =0; i < agent.Length ; i++)
				{
					//If an Agent is a User
					if(agent[i].type == Agent.userType )
					{
						//Add user to list if it doesn't exist already
						if(!arrayList.Contains (agent[i].id ))
							arrayList.Add(agent[i].id );
					}
				}
			}
			catch(Exception ex)
			{
				throw;
			}

			int[] userIDs = Utilities.ArrayListToIntArray(arrayList);

			return userIDs;

		}

		/// <summary>
		/// Returns an array of user IDs for all users in the specified group or one of the recursively contained subgroups.
		/// </summary>
		/// <param name="groupID">The ID of the Group whose members are to be listed.</param>
		/// <returns>An array containing the user IDs of all members of the Group.</returns>
		public static int[] ListUserIDsInGroupRecursively(int groupID)
		{
			ArrayList arrayList = new ArrayList ();
			try
			{
				//Lists all members of a group as agent structs
				Agent[] agent = InternalAdminDB.SelectMembersInGroup (groupID);

				for (int i =0; i < agent.Length ; i++)
				{
					//If an Agent is a User
					if(agent[i].type == Agent.userType )
					{
						//Add user to list if it doesn't exist already
						if(!arrayList.Contains (agent[i].id ))
							arrayList.Add(agent[i].id );
					}
					else //If Agent is a group
					{
						//Get the members of the group
						int[] list = ListUserIDsInGroupRecursively(agent[i].id );

						//add them to the list
						for(int j =0; j < list.Length ; j++)
						{
							if(!arrayList.Contains (list[j]))
								arrayList.Add(list[j]);
						}

					}
				}
			}
			catch(Exception ex)
			{
				throw;
			}

			int[] userIDs = Utilities.ArrayListToIntArray(arrayList);

			return userIDs;

		}

		/// <summary>
		/// Returns an array of group IDs for all subgroups of the specified group
		/// </summary>
		/// <param name="groupID">The ID of the Group whose subgroups are to be listed.</param>
		/// <returns>An array containing the group IDs of all subgroup of the Group.</returns>
		public static int[] ListSubgroupIDs(int groupID)
		{
			ArrayList arrayList = new ArrayList ();
			try
			{
				//Lists all subgroups of a group as agent structs
				Agent[] agent = InternalAdminDB.SelectMembersInGroup (groupID);

				for (int i =0; i < agent.Length ; i++)
				{
					//If an Agent is a Group
					if(agent[i].type == Agent.groupType)
					{
						//Add group to list if it doesn't exist already
						if(!arrayList.Contains (agent[i].id ))
							arrayList.Add(agent[i].id );
					}
				}
			}
			catch(Exception ex)
			{
				throw;
			}

			int[] groupIDs = Utilities.ArrayListToIntArray(arrayList);

			return groupIDs;

		}

		/// <summary>
		/// Returns an array of group IDs for all subgroups of the specified group or one of the recursively contained subgroups.
		/// </summary>
		/// <param name="groupID">The ID of the Group whose subgroups are to be listed.</param>
		/// <returns>An array containing the group IDs of all subgroup of the Group.</returns>
		public static int[] ListSubgroupIDsRecursively(int groupID)
		{
			ArrayList arrayList = new ArrayList ();
			try
			{
				//Lists all members of a group as agent structs
				Agent[] agent = InternalAdminDB.SelectMembersInGroup (groupID);

				for (int i =0; i < agent.Length ; i++)
				{
					//If an Agent is a group
					if(agent[i].type == Agent.groupType	 )
					{
						//Add group to list if it doesn't exist already
						if(!arrayList.Contains (agent[i].id ))
							arrayList.Add(agent[i].id );
					}
					//Get the members of the group
					int[] list = ListSubgroupIDsRecursively(agent[i].id );

					//add them to the list
					for(int j =0; j < list.Length ; j++)
					{
						if(!arrayList.Contains (list[j]))
							arrayList.Add(list[j]);
					}
				}
			}
			catch(Exception ex)
			{
				throw;
			}

			int[] groupIDs = Utilities.ArrayListToIntArray(arrayList);

			return groupIDs;

		}

		/// <summary>
		/// Lists the member IDs of all members of the specified group.
		/// </summary>
		/// <param name="groupID">The ID of the Group whose members are to be listed.</param>
		/// <returns>An array containing the memberIDs of all members of the Group. A subgroup is listed by the subgroup ID, not by enumerating all members of the subgroup.</returns>
		public static int[] ListMemberIDsInGroup(int groupID)
		{
			Agent[] agent = InternalAdminDB.SelectMembersInGroup (groupID);
			int [] memberIDs = new int [agent.Length ];
			for (int i =0; i < agent.Length ; i++)
			{
				memberIDs[i] = agent[i].id  ;
			}
			return memberIDs;
		}


		/// <summary>
		/// Recursively checks whether the specified agent is a member of the specified group. 
		/// </summary>
		/// <param name="agentID">The ID of the agent whose membership in the enclosing group is to be checked; note that the agent may be a single user (represented by a userID) or a subgroup of the target group to satisfy the query.</param>
		/// <param name="groupID">The ID of the Group in which the agent's membership is being checked.</param>
		/// <returns>true if and only if the specified agent is a member or subgroup or the target group.</returns>
		public static bool IsAgentMember(int agentID, int groupID)
		{
			int[] groupIDs = ListGroupsForAgentRecursively(agentID);

			for(int i=0; i< groupIDs.Length ; i++)
			{
				if(groupIDs[i].Equals (groupID))
					return true;
			}
			return false;
		}

        /// <summary>
        /// Lists the IDs of all Groups of which the specified agent (group or user) is an explicit member. 
        /// An explicit member is one directly added to the specified group by the adduser() or addmemberToGroup() methods.
        /// </summary>
        /// <param name="agentID">The ID of the agent whose group membership is being enumerated.</param>
        /// <returns>An int array containing the group IDs of all Groups to which the user directly belongs.</returns>
        public static int[] ListNonRequestGroupsForAgent(int agentID)
        {
            int[] groups = InternalAuthorizationDB.ListAgentParents(agentID);
            return groups;
        }

		
		/// <summary>
		/// Lists the IDs of all Groups of which the specified agent (group or user) is an explicit member. 
		/// An explicit member is one directly added to the specified group by the adduser() or addmemberToGroup() methods.
		/// </summary>
		/// <param name="agentID">The ID of the agent whose group membership is being enumerated.</param>
		/// <returns>An int array containing the group IDs of all Groups to which the user directly belongs.</returns>
		public static int[] ListGroupsForAgent(int agentID)
		{
            int[] groups = InternalAuthorizationDB.ListAgentParents(agentID);
			return groups;
		}

		/// <summary>
		/// Lists the IDs of all groups of which the specified agent (group or user) is a member. 
		/// This includes any superGroups of groups to which the agent was explicityly added as a member.
		/// </summary>
		/// <param name="agentID">The ID of the agent whose group membership is being enumerated.</param>
		/// <returns>An array containing the group IDs of all Groups and supergroups to which the user belongs.</returns>
		public static int[] ListGroupsForAgentRecursively(int agentID)
		{
			Hashtable myHT = new  Hashtable ();
			int[] gps = ListGroupsForAgent(agentID);

			for(int i=0; i<gps.Length ; i++)
			{
				if(!myHT.Contains (gps[i]))
				{
					myHT.Add(gps[i],"");
				}
			}
			
			for(int i=0; i<gps.Length ; i++)
			{
				int[] groups = ListGroupsForAgentRecursively(gps[i]);
				
				for(int j=0; j<groups.Length ; j++)
				{
					if(!myHT.Contains (	groups[j]))
					{
						myHT.Add(groups[j],"");
					}
				}
			}

			int[] list = new int[myHT.Count ];
			int count =0;
			IDictionaryEnumerator myEnumerator = myHT.GetEnumerator ();
			while ( myEnumerator.MoveNext() )
			{	
				list[count] = (int) myEnumerator.Key ;
				count++;
			}

			return list;
		}


		///*********************** NOTIFICATION **************************///


		/// <summary>
		/// Emails the text message to all Users whose user IDs appear in userIDs.
		/// </summary>
		/// <param name="userIDs">An array of user IDs specifying the recipient list.</param>
		/// <param name="message">The text to be emailed to each user.</param>
		/// <returns>An array of the users who were not successfully notified, i.e., the ones for which the operation failed.</returns>
		/// <remarks>This is not implemented, as we need to specify an SMTP/Mail server to do this!!</remarks>
		public static int[] NotifyUsers(int[] userIDs, string message)
		{
			User[] users = GetUsers(userIDs);
			return null;
		}



		///*********************** SYSTEM MESSAGES **************************///


		/// <summary>
		/// Adds a message for a target group, lab server, or system as a whole.
		/// </summary>
		/// <param name="messageType">A string identifying the message type.</param>
		/// <param name="toBeDisplayed">true if the message is to be displayed; false otherwise.</param>
		/// <param name="groupID">
		/// -1 if message is not targeted at a particular group. 
		/// If greater than 0, this message is targeted to users who have selected the group identified by groupID as their effective group.
		/// Note this argument has precedence over labServerID. If this argument and labServerID are both -1, the message is intended as a general system message visible to all users.
		/// </param>
		/// <param name="labServerID">
		/// -1 if message is not targeted at a particular Lab Server. 
		/// If greater than 0, this message is targeted to users who have selected as their effective group, a group that has access to the lab server specified by this ID.
		/// If this argument and groupID are both -1, the message is intended as a general system message visible to all users.
		/// </param>
		/// <param name="messageBody">The text of the message to be displayed.</param>
		/// <param name="messageTitle">The title of the message to be displayed.</param>
		/// <returns></returns>
        public static int AddSystemMessage(string messageType, bool toBeDisplayed, int groupID, int clientID, int agentID, string messageBody, string messageTitle)
		{
			SystemMessage sm = new SystemMessage();
			sm.messageType=messageType;
			sm.toBeDisplayed = toBeDisplayed;
			sm.groupID = groupID;
            sm.clientID = clientID;
			sm.agentID = agentID;
			sm.messageBody = messageBody;
			sm.messageTitle = messageTitle;

			return InternalAdminDB.InsertSystemMessage(sm);
		}

		/// <summary>
		/// Deletes the messages specified by messageIDs.
		/// </summary>
		/// <param name="messageIDs">An array of message IDs specifying the messages to be removed.</param>
		/// <returns>An array containing the IDs of all messages not successfully removed, i.e., those for whom the operation failed.</returns>
		public static int[] RemoveSystemMessages(int[] messageIDs)
		{
			return InternalAdminDB.DeleteSystemMessages(messageIDs);
		}

		/// <summary>
		/// Modifies a message specified by messageID.
		/// </summary>
		/// <param name="messageID">Identifies the message to be modified.</param>
		/// <param name="messageType">A string identifying the message type; if NULL, then the previous value will not be changed.</param>
		/// <param name="toBeDisplayed">true if the message is to be displayed; false otherwise.</param>
		/// <param name="groupID">-1 if message is not targeted at a particular group; if >0, this message is targeted to users who have selected the group identified by this groupID as their effective group. Note this argument has precedence over labServerID. If this argument and labServerID are both ==-1, then the message is intended as a general system message visible to all users.</param>
		/// <param name="labServerID">-1 if message is not targeted at users of a particular lab server; if >0, this message is targeted at users who have selected as their effective group, a group that has access to the lab server specified by this ID. If this argument and groupID are both ==-1, then the message is intended as a general system message visible to all users.</param>
		/// <param name="messageBody">The text of the message to be displayed; if NULL, then the previous value will not be changed.</param>
		/// <param name="messageTitle">The title of the message to be displayed; if NULL, then the previous value will not be changed.</param>
		public static void ModifySystemMessage(int messageID, string messageType, bool toBeDisplayed, int groupID, int clientID, int agentID, string messageBody, string messageTitle)
		{
			SystemMessage sm = new SystemMessage();
			sm.messageID = messageID;
			sm.messageType=messageType;
			sm.toBeDisplayed = toBeDisplayed;
			sm.groupID = groupID;
			sm.clientID = clientID;
            sm.agentID = agentID;
			sm.messageBody = messageBody;
			sm.messageTitle = messageTitle;

			InternalAdminDB.UpdateSystemMessage(sm);
		}

		/// <summary>
		/// Retrieve system messages specified by a combination of messageType and groupID or labServerID.
		/// </summary>
		/// <param name="messageType">A string identifying the requested message type.</param>
		/// <param name="groupID">If >0, messages for the group identified by this groupID are retrieved. If ==-1, then the groupID is not used as a selection criterion. Note this argument has precedence over labServerID; that is, if both groupID and labServerID are both >0, then labServerID will be ignored as a selction criterion. If groupID and labServerID are both ==-1, then only general system messages visible to all users will be retrieved.</param>
		/// <param name="labServerID">If >0, messages for the labServer identified by this labServerID are retrieved. If ==-1, then the labServerID is not used as a selection criterion. Note groupID has precedence over this argument; that is, if both groupID and labServerID are both >0, then labServerID will be ignored as a selection criterion. If groupID and labServerID are both ==-1, then only general system messages visible to all users will be retrieved.</param>
		/// <returns>An SystemMessage array of the requested system messages.</returns>
		/// <seealso cref="SystemMessage">SystemMessage Class</seealso>
		public static SystemMessage[] GetSystemMessages(string messageType, int groupID, int clientID, int agentID)
		{
			return InternalAdminDB.SelectSystemMessages(messageType, groupID, clientID, agentID);
		}
		

		///*********************** USER SESSIONS **************************///


		/// <summary>
		/// Inserts a user session record.
		/// </summary>
		/// <param name="userID">The ID of the User.</param>
		/// <param name="effectiveGroupID">The User's current Effective Group.</param>
		/// <param name="sessionKey">The User's current Session Key.</param>
		/// <returns>A database generated session ID.</returns>
		public static long InsertUserSession(int userID, int effectiveGroupID, int tzOffset,string sessionKey)
		{			
			UserSession us = new UserSession();
			us.userID = userID;
			us.groupID = effectiveGroupID;
			us.sessionKey = sessionKey;
            us.tzOffset = tzOffset;

			return InternalAdminDB.InsertUserSession (us);
		}

        public static bool ModifyUserSession(long sessionID, int effectiveGroupID, int clientID, int tzOffset,string sessionKey)
        {
            return InternalAdminDB.ModifyUserSession(sessionID,effectiveGroupID,clientID,tzOffset,sessionKey);
        }

        public static bool SetSessionGroup(long sessionID, int groupID)
        {
            return InternalAdminDB.SetSessionGroup(sessionID, groupID);        
        }
        public static bool SetSessionClient(long sessionID, int clientID)
        {
            return InternalAdminDB.SetSessionClient(sessionID, clientID);
        }

        public static bool SetSessionKey(long sessionID, string key)
        {
            return InternalAdminDB.SetSessionKey(sessionID, key);
        }


		/// <summary>
		/// Updates the session end time in the user's session record. returns the user's session end time.
		/// </summary>
		/// <param name="sessionID">The user's session ID.</param>
		/// <returns>The user's session end time for the session referenced by sessionID.</returns>
		public static DateTime SaveUserSessionEndTime(long sessionID)
		{
			return InternalAdminDB.SaveUserSessionEndTime (sessionID);
		}
        
        /// <summary>
        /// Obtain the user's sessions given the Session IDs.
        /// </summary>
        /// <param name="sessionIDs">The user's session IDs</param>
        /// <returns>An array of UserSession objects, referenced by an input array of sessionIDs.</returns>
        /// <seealso cref="UserSession">UserSession Class</seealso>
        public static UserSession[] GetUserSessions(long[] sessionIDs)
        {
            return InternalAdminDB.SelectUserSessions(sessionIDs);
        }

        /// <summary>
        /// Retrieve user session info given the Session ID.
        /// </summary>
        /// <param name="sessionID">The user's session ID</param>
        /// <returns>A DataTable with .</returns>
        /// <seealso cref="UserSession">UserSession Class</seealso>
        public static SessionInfo GetSessionInfo(long sessionID)
        {
            return InternalAdminDB.SelectSessionInfo(sessionID);
        }
        public static SessionInfo GetSessionInfo(Coupon coupon)
        {
            SessionInfo sessionInfo = null;
            TicketIssuerDB ticketIssuer = new TicketIssuerDB();
            Ticket sessionTicket = ticketIssuer.RetrieveIssuedTicket(coupon, TicketTypes.REDEEM_SESSION, ProcessAgentDB.ServiceGuid);
            if (sessionTicket != null)
            {
                if (sessionTicket.IsExpired())
                {
                    throw new AccessDeniedException("The ticket has expired.");
                }
                // Get Session info	
                XmlQueryDoc expDoc = new XmlQueryDoc(sessionTicket.payload);
                sessionInfo = new SessionInfo();
                string STR_RedeemSessionPayload = "RedeemSessionPayload/";
                string userID = expDoc.Query(STR_RedeemSessionPayload + "userID");
                sessionInfo.userID = Convert.ToInt32(userID);
                string groupID = expDoc.Query(STR_RedeemSessionPayload + "groupID");
                sessionInfo.groupID = Convert.ToInt32(groupID);
                string clientID = expDoc.Query(STR_RedeemSessionPayload + "clientID");
                sessionInfo.clientID = Convert.ToInt32(clientID);
                sessionInfo.userName = expDoc.Query(STR_RedeemSessionPayload + "userName");
                sessionInfo.groupName = expDoc.Query(STR_RedeemSessionPayload + "groupName");
            }
            return sessionInfo;
        }

		/// <summary>
		/// Obtain all sessions of a given User.
		/// </summary>
		/// <param name="userID">The user's ID. If less than 0, matches all user IDs.</param>
		/// <param name="groupID">The ID of the user's effective group. If less than 0, matches all the user's groups.</param>
		/// <param name="timeAfter">The start of the time search interval. If null, there is no start time for the match.</param>
		/// <param name="timeBefore">The end of the time search interval. If null, there is no end time for the match.</param>
		/// <returns>An array of UserSession objects that matches the arguments.</returns>
		/// <seealso cref="UserSession">UserSession Class</seealso>
		public static UserSession[] GetUserSessions(int userID, int groupID, DateTime timeAfter, DateTime timeBefore)
		{
			return InternalAdminDB.SelectUserSessions (userID, groupID, timeAfter, timeBefore);
		}

        //**************************** Resource Mappings **********************************************/
        //public static ResourceMapping AddResourceMapping(ResourceMappingKey key, ResourceMappingValue[] values)
        //{
        //    BrokerDB issuer = new BrokerDB();
        //    ResourceMapping mapping = issuer.AddResourceMapping(key, values);

        //    // add mapping to qualifier list
        //    int qualifierType = Qualifier.resourceMappingQualifierTypeID;
        //    string name = Administration.GetMappingString(mapping, issuer);
            
        //    Authorization.Authorization.AddQualifier(mapping.MappingID, qualifierType, name, Qualifier.ROOT);

        //    return mapping;
        //}

        //public static void DeleteResourceMapping(ResourceMapping mapping)
        //{
        //    InternalAdminDB.DeleteResourceMapping(mapping);
        //}

        //protected static string GetMappingEntryString(ResourceMappingEntry entry, TicketIssuerDB ticketIssuer)
        //{

        //    Object o = entry.Entry;

        //    if (entry == null)
        //        return "Entry not found";

        //    if (entry.Type.Equals(ResourceMappingTypes.PROCESS_AGENT))
        //    {
        //        int [] pas = new int[] {(int)o};
        //        IntTag[] tags = ticketIssuer.GetProcessAgentTagsWithType(pas);
        //        if (tags != null && tags.Length > 0)
        //            return tags[0].tag;
        //        else
        //            return "Process Agent not found";
        //    }

        //    else if (entry.Type.Equals(ResourceMappingTypes.CLIENT))
        //    {
        //        int[] labClientIDs = Administration.ListLabClientIDs();
        //        LabClient[] labClients = Administration.GetLabClients(labClientIDs);
        //        for (int i = 0; i < labClientIDs.Length; i++)
        //            if (labClientIDs[i] == (int)o)
        //                return labClients[i].ClientName;
        //        return "Client not found";
        //    }

        //    else if (entry.Type.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
        //    {
        //        return GetMappingString((ResourceMapping)o, ticketIssuer);
        //    }

        //    else if (entry.Type.Equals(ResourceMappingTypes.STRING))
        //        return (string)o;

        //    else if (entry.Type.Equals(ResourceMappingTypes.RESOURCE_TYPE))
        //        return (string)o;

        //    else if (entry.Type.Equals(ResourceMappingTypes.TICKET_TYPE))
        //        return ((TicketType)o).shortDescription;

        //    else if (entry.Type.Equals(ResourceMappingTypes.GROUP))
        //    {
        //        int[] groupIDs = Administration.ListGroupIDs();
        //        Group[] groups = Administration.GetGroups(groupIDs);
        //        for (int i = 0; i < groupIDs.Length; i++)
        //            if (groupIDs[i] == (int)o)
        //                return groups[i].GroupName;
        //    }

        //    return "";
        //}

        //protected static string GetMappingString(ResourceMapping mapping, TicketIssuerDB issuer)
        //{
        //    StringBuilder s = new StringBuilder();
        //    s.Append(mapping.MappingID);
        //    s.Append( " ");
        //    if(mapping.key.TypeName != ResourceMappingTypes.PROCESS_AGENT)
        //    s.Append(mapping.key.TypeName + ":");
        //    s.Append(GetMappingEntryString(mapping.key, issuer) + "->");
         
        //    // print all values except last
        //    for (int i = 0; i < mapping.values.Length; i++)
        //    {
        //        ResourceMappingValue value = mapping.values[i];
        //        if (value.Type.Equals(ResourceMappingTypes.RESOURCE_MAPPING))
        //            s.Append( "(" + value.TypeName + ":" + ((ResourceMapping)value.Entry).MappingID + "), ");
        //        else
        //            s.Append( GetMappingEntryString(value, issuer));
        //        if (i < mapping.values.Length)
        //            s.Append(", ");
        //    }
        //    return s.ToString();
        //}

 
 


	}
}
