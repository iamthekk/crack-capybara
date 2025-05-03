using System;
using System.Collections.Generic;

namespace HotFix
{
	public class ServerPullResponseData : ServerJsonResponseBase
	{
		public List<ServerPullResponseData.CRoleInfo> GetCRoleInfoList()
		{
			List<ServerPullResponseData.CRoleInfo> list = new List<ServerPullResponseData.CRoleInfo>();
			if (this.data != null && this.data.users != null && this.data.users.Count > 0)
			{
				for (int i = 0; i < this.data.users.Count; i++)
				{
					List<ServerJsonResponseBase.Data.Users.RoleInfo> roleInfo = this.data.users[i].roleInfo;
					if (roleInfo != null && roleInfo.Count > 0)
					{
						ServerPullResponseData.CRoleInfo croleInfo = new ServerPullResponseData.CRoleInfo();
						list.Add(croleInfo);
						for (int j = 0; j < roleInfo.Count; j++)
						{
							ServerJsonResponseBase.Data.Users.RoleInfo roleInfo2 = roleInfo[j];
							string key = roleInfo2.key;
							uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
							if (num <= 2083069342U)
							{
								if (num != 301557727U)
								{
									if (num != 1347729273U)
									{
										if (num == 2083069342U)
										{
											if (key == "deviceId")
											{
												croleInfo.deviceId = roleInfo2.value;
											}
										}
									}
									else if (key == "accountId2")
									{
										croleInfo.accountId2 = roleInfo2.value;
									}
								}
								else if (key == "userId")
								{
									croleInfo.userId = long.Parse(roleInfo2.value);
								}
							}
							else if (num <= 2145047283U)
							{
								if (num != 2100805631U)
								{
									if (num == 2145047283U)
									{
										if (key == "logintime")
										{
											long.TryParse(roleInfo2.value, out croleInfo.logintime);
										}
									}
								}
								else if (key == "serverId")
								{
									croleInfo.serverId = uint.Parse(roleInfo2.value);
								}
							}
							else if (num != 3518925660U)
							{
								if (num == 4274207345U)
								{
									if (key == "accountId")
									{
										croleInfo.accountId = roleInfo2.value;
									}
								}
							}
							else if (key == "createTime")
							{
								long.TryParse(roleInfo2.value, out croleInfo.createTime);
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				list.Sort(delegate(ServerPullResponseData.CRoleInfo a, ServerPullResponseData.CRoleInfo b)
				{
					int num2 = b.logintime.CompareTo(a.logintime);
					if (num2.Equals(0))
					{
						num2 = b.serverId.CompareTo(a.serverId);
					}
					return num2;
				});
			}
			return list;
		}

		public class CRoleInfo
		{
			public long userId;

			public uint serverId;

			public string deviceId;

			public string accountId;

			public string accountId2;

			public long createTime;

			public long logintime;
		}
	}
}
