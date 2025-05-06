using System;
using HabbySDK.Llc;
using UnityEngine;

namespace Framework.SocketNet
{
	public class SocketNet_HabbyImAdapter : ISocketNet
	{
		public bool Init()
		{
			return LlcMgr.Instance.Init(GameApp.NetWork.SocketHost);
		}

		public void DeInit()
		{
			if (LlcMgr.Instance != null)
			{
				LlcMgr.Instance.Deinit();
			}
		}

		public bool Connected
		{
			get
			{
				return LlcMgr.Instance.IsConnected;
			}
		}

		public void CheckReconnect(string whyneed)
		{
			this.Log(string.Format("CheckReconnect({0}) 检测重新连接... Connected ? {1}", whyneed, this.Connected));
			if (!this.Connected)
			{
				this.Init();
			}
		}

		public bool LoginSuccess
		{
			get
			{
				return LlcMgr.Instance.IsConnected;
			}
		}

		public void SetSocketGroup(int kind, string groupName)
		{
			ISocketGameProxyBase socketGameProxyBase = this.mGameProxy;
			if (socketGameProxyBase == null)
			{
				return;
			}
			socketGameProxyBase.SetSocketGroup(kind, groupName);
		}

		public void SetSocketGameHandler(ISocketGameProxyBase proxy)
		{
			this.SetSocketMessageHandler(proxy);
		}

		public void SetTokenAfterLogin(long userid, string token)
		{
			if (this.mUserID != userid || string.IsNullOrEmpty(this.mToken) || !this.Connected)
			{
				this.mUserID = userid;
				this.mToken = token;
				LlcMgr.Instance.Authenticate(this.mUserID.ToString(), this.mToken);
				return;
			}
			if (this.mToken == token)
			{
				this.Log("Set socket token is same, no need reconnect!");
				return;
			}
			this.mToken = token;
			LlcMgr.Instance.Authenticate(userid.ToString(), this.mToken);
		}

		public ISocketGameProxyBase GameProxy
		{
			get
			{
				return this.mGameProxy;
			}
		}

		public void SetSocketMessageHandler(ISocketGameProxyBase proxy)
		{
			this.DeleteGameProxy();
			this.mGameProxy = proxy;
			if (this.mGameProxy != null)
			{
				this.mGameProxy.OnInit();
			}
		}

		private void DeleteGameProxy()
		{
			if (this.mGameProxy != null)
			{
				this.mGameProxy.OnUnInit();
			}
			this.mGameProxy = null;
		}

		private void Log(string message)
		{
			Debug.Log("[Socket]" + message);
		}

		private void LogWarning(string message)
		{
			Debug.LogWarning("[Socket]" + message);
		}

		private void LogError(string message)
		{
			Debug.LogError("[Socket]" + message);
		}

		private ISocketGameProxyBase mGameProxy;

		private long mUserID;

		private string mToken;
	}
}
