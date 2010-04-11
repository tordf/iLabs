using System;
using System.Data;
using System.Data.Common;

namespace iLabs.ServiceBroker.Authorization
{
	public sealed class AuthCache
	{
		private static DataSet _GrantSet;
		private static DataSet _QualifierSet; 
		private static DataSet _QualifierHierarchySet;
		private static DataSet _AgentHierarchySet;
		private static DataSet _AgentsSet;

		private AuthCache()
		{
		}

		public static DataSet GrantSet
		{
			get
			{
				return _GrantSet;
			}
			set
			{
				_GrantSet = value;
			}
		}

		public static DataSet QualifierSet
		{
			get
			{
				return _QualifierSet;
			}
			set
			{
				_QualifierSet = value;
			}
		}
		public static DataSet QualifierHierarchySet
		{
			get
			{
				return _QualifierHierarchySet;
			}
			set
			{
				_QualifierHierarchySet = value;
			}
		}
		public static DataSet AgentHierarchySet
		{
			get
			{
				return _AgentHierarchySet;
			}
			set
			{
				_AgentHierarchySet = value;
			}
		}
		public static DataSet AgentsSet
		{
			get
			{
				return _AgentsSet;
			}
			set
			{
				_AgentsSet = value;
			}
		}
	}
}