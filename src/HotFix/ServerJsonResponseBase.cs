using System;
using System.Collections.Generic;

namespace HotFix
{
	public class ServerJsonResponseBase
	{
		public string msg;

		public int code;

		public ServerJsonResponseBase.Data data;

		[Serializable]
		public class Data
		{
			public uint maxServerId;

			public ServerJsonResponseBase.Data.FieldDescription fieldDescription;

			public List<ServerJsonResponseBase.Data.Users> users;

			[Serializable]
			public class FieldDescription
			{
				public string userInfo;
			}

			[Serializable]
			public class Users
			{
				public List<ServerJsonResponseBase.Data.Users.RoleInfo> roleInfo;

				[Serializable]
				public class RoleInfo
				{
					public string key;

					public string value;
				}
			}
		}
	}
}
