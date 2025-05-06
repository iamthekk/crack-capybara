using System;
using System.Collections;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Modules;
using Google.Protobuf;
using UnityEngine;

namespace Framework.NetWork
{
	public class NetWorkManager : MonoBehaviour
	{
		public void SetNetworkManager(INetWorkManager netWorkManager)
		{
			this.m_networkManager = netWorkManager;
			if (this.m_networkManager == null)
			{
				return;
			}
			this.m_networkManager.SetData(this.Url, 1, this.m_netWorkUsingType);
		}

		public string Url
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_url))
				{
					string text = "SwitchPreServer";
					if (GameApp.SDK.GetCloudDataValue<bool>(text, false))
					{
						this.m_url = this.m_preURL;
					}
					else
					{
						this.m_url = ((this.m_netWorkUsingType == NetWorkUsingType.Release) ? this.m_releaseURL : this.m_debugURL);
					}
				}
				return this.m_url;
			}
		}

		public string SocketHost
		{
			get
			{
				if (GameApp.SDK.GetCloudDataValue<bool>("SwitchPreServer", false))
				{
					return this.m_preSocketNetUrl;
				}
				NetWorkUsingType netWorkUsingType = this.m_netWorkUsingType;
				if (netWorkUsingType == NetWorkUsingType.Debug)
				{
					return this.m_debugSocketNetUrl;
				}
				if (netWorkUsingType != NetWorkUsingType.Release)
				{
					throw new ArgumentOutOfRangeException();
				}
				return this.m_releaseSocketNetUrl;
			}
		}

		public int Version
		{
			get
			{
				return 1;
			}
		}

		public bool IsNetConnect
		{
			get
			{
				return this.m_networkManager != null && this.m_networkManager.IsNetConnect();
			}
		}

		public void SetTransID(ulong transid)
		{
			this.m_transId = transid;
		}

		public void Send(IMessage message, Action<IMessage> callBack, bool isShowMask, bool isCache, string key, bool isShowError = true)
		{
			if (this.m_networkManager == null)
			{
				HLog.LogError("m_networkManager == null");
				return;
			}
			if (message == null)
			{
				return;
			}
			if (!Singleton<ProtocolRateLimiterManager>.Instance.Record(message.GetType()))
			{
				EventArgTips instance = Singleton<EventArgTips>.Instance;
				instance.LanguageTips = "Common_RequestTimesLimit";
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_Framework_ShowTips, instance);
				return;
			}
			NetWorkSendData netWorkSendData = new NetWorkSendData();
			netWorkSendData.Clear();
			netWorkSendData.m_message = message;
			netWorkSendData.m_callBack = callBack;
			netWorkSendData.m_isShowMask = isShowMask;
			netWorkSendData.m_isCache = isCache;
			netWorkSendData.m_key = key;
			netWorkSendData.m_isShowError = isShowError;
			IEnumerator enumerator = this.m_networkManager.SendWebRequest(netWorkSendData);
			base.StartCoroutine(enumerator);
		}

		public void HandleCommonData(IMessage msg)
		{
			if (this.m_networkManager != null)
			{
				this.m_networkManager.HandleCommonData(msg);
			}
		}

		[SerializeField]
		private string m_debugURL = "https://test.advrpg.com/";

		[SerializeField]
		private string m_preURL = "https://pre.advrpg.com/";

		[SerializeField]
		private string m_releaseURL = "https://prod.advrpg.com/";

		[SerializeField]
		private string m_debugSocketNetUrl = "dev-im.advrpg.com";

		[SerializeField]
		private string m_preSocketNetUrl = "pre-im.advrpg.com";

		[SerializeField]
		private string m_releaseSocketNetUrl = "prod-im.advrpg.com";

		public NetWorkUsingType m_netWorkUsingType;

		[SerializeField]
		[Label]
		private string m_url = string.Empty;

		public string m_account = "test001";

		public string m_deviceID = "";

		public string m_account2 = "";

		public string m_userID = "";

		private const int m_version = 1;

		public ulong m_transId = 1UL;

		public uint m_serverID;

		public uint m_abVersion;

		private INetWorkManager m_networkManager;
	}
}
