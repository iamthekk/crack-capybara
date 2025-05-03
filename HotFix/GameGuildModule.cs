using System;
using Dxx.Guild;
using Framework.DxxGuild;
using Framework.EventSystem;
using HotFix;
using Proto.Guild;
using UnityEngine;

public class GameGuildModule
{
	private GuildSDKManager SDK
	{
		get
		{
			return GuildSDKManager.Instance;
		}
	}

	public bool Init(GuildConfig config)
	{
		if (config == null)
		{
			HLog.LogError("未找到公会初始化配置文件!");
			return false;
		}
		this.GuildObject = new GameObject("Guild");
		Object.DontDestroyOnLoad(this.GuildObject);
		this.NetworkProxy = new GuildNetworkProxy_Common();
		GuildSDKManager.Init(new GuildInitConfig
		{
			Config = config
		});
		GuildSDKManager.Instance.Permission.SetPermissionRule(new GuildPermissionSet());
		this.InitForGame();
		this.NetworkProxy.StartProxy();
		return true;
	}

	public void Destroy()
	{
		this.DestroyForGame();
		if (this.GuildObject != null)
		{
			Object.Destroy(this.GuildObject);
			this.GuildObject = null;
		}
		if (this.NetworkProxy != null)
		{
			this.NetworkProxy.StopProxy();
		}
		GuildSDKManager.DeInit();
	}

	public GameObject CreateSubGameObject(string name)
	{
		if (this.GuildObject == null)
		{
			return null;
		}
		Transform transform = this.GuildObject.transform.Find(name);
		if (transform == null)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.transform.SetParent(this.GuildObject.transform, false);
			return gameObject;
		}
		return transform.gameObject;
	}

	public void OnUserLogin()
	{
	}

	public bool InitForGame()
	{
		this.RegGuildGameModules();
		GuildProxy.GameEvent.AddReceiver(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnUserInfoChange));
		return true;
	}

	public void DestroyForGame()
	{
		this.UnRegGuildGameModules();
		GuildProxy.GameEvent.RemoveReceiver(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnUserInfoChange));
	}

	private void OnUserInfoChange(object sender, int type, BaseEventArgs eventArgs)
	{
		if (this.SDK.GuildInfo.HasGuild)
		{
			GuildNetUtil.Guild.DoRequest_GetGuildDetailInfo(this.SDK.GuildInfo.GuildID, delegate(bool result, GuildGetDetailResponse resp)
			{
				if (result)
				{
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_RefreshHallMember);
				}
			}, false);
		}
	}

	private void RegGuildGameModules()
	{
		this.SDK.RegModule(1001, new GuildUIDataModule());
	}

	private void UnRegGuildGameModules()
	{
		this.SDK.UnRegModule(1001, null);
	}

	public GameObject GuildObject;

	public GuildNetworkProxy_Common NetworkProxy;
}
