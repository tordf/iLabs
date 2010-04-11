/*
 * Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
 * Please see license.txt in top level directory for full license.
 */

/* $Id: Authentication.cs,v 1.3 2008/03/14 16:04:42 pbailey Exp $ */

using System;
using System.Web.Security;

using iLabs.ServiceBroker.Internal;

namespace iLabs.ServiceBroker.Authentication
{
	public class AuthenticationType
	{
		public const string NativeAuthentication = "Native";
		public const string Kerberos = "Kerberos_MIT";
	}


	/// <summary>
	/// Summary description for Authentication.
	/// </summary>
	public class AuthenticationAPI
	{
		public AuthenticationAPI()
		{
		}

		/// <summary>
		/// performs whatever actions including GUI interaction to identify and authenticate the user who has initiated the current session
		/// </summary>
		/// <param name="userID">the ID of user</param>
		/// <param name="password">the user's password</param>
		/// <returns>true if the user has been authenticated; false otherwise</returns>
		public static bool Authenticate (int userID, string password)
		{
			string hashedDBPassword = InternalAuthenticationDB.ReturnNativePassword (userID);
			string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
			if(hashedPassword == hashedDBPassword )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/*		
		public static string Authenticate (string type)
		{
			// how to do this?
		}

		public static bool CreateNativePrincipal (string principalID)
		{

		}

		public static string[] RemoveNativePrincipals (string[] principalIDs)
		{
		
		}

		public static string[] ListNativePrincipals()
		{

		}
*/
		/// <summary>
		/// Set the password of the specified native principal
		/// </summary>
		/// <param name="userID">the ID of the native principal whose password is to be changed</param>
		/// <param name="password">the new password</param>
		/// <returns>true if the change was successful; false if the new password was of inappropriate form or the native userID unknown.</returns>
		public static bool SetNativePassword (int userID, string password)
		{
			string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
			try
			{
				InternalAuthenticationDB.SaveNativePassword (userID, hashedPassword);

			}
			catch
			{
				return false;
			}
			return true;

		}

	
	}
}
