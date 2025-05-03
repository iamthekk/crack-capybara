using System;
using Framework.NetWork;
using UnityEngine;

namespace Framework.SocketNet
{
	public class SocketNetConfig : ScriptableObject
	{
		public string GetHost()
		{
			if (GameApp.NetWork.m_netWorkUsingType == NetWorkUsingType.Release)
			{
				return this.Host;
			}
			return this.DebugHost;
		}

		public uint GetPort()
		{
			if (GameApp.NetWork.m_netWorkUsingType == NetWorkUsingType.Release)
			{
				return this.Port;
			}
			return this.DebugPort;
		}

		public static SocketNetConfig GetConfig()
		{
			SocketNetConfig socketNetConfig = Resources.Load<SocketNetConfig>("socket_config");
			if (socketNetConfig == null)
			{
				HLog.LogError("[Socket]未找到Socket初始化配置文件：参考目录：Assets/Resources/socket_config.asset");
				return null;
			}
			return socketNetConfig;
		}

		public const string Config_Resources_PATH = "socket_config";

		public const string Config_PATH = "Assets/Resources/socket_config.asset";

		public string Host = "framework-socket.gorillasvc.com";

		public uint Port = 80U;

		public string DebugHost = "framework-socket.gorillasvc.com";

		public uint DebugPort = 80U;
	}
}
