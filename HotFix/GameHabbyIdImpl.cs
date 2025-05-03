using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Modules;
using HotFix;
using Proto.User;
using UnityEngine;

public class GameHabbyIdImpl : IHabbyIdIntegration
{
	public string GameId
	{
		get
		{
			return "Capybara";
		}
	}

	public static event Action OnStateChanged;

	public string GetGameAccountId()
	{
		return GameApp.Data.GetDataModule(DataName.LoginDataModule).accountId.ToString();
	}

	public string GetGameUserId()
	{
		return GameApp.Data.GetDataModule(DataName.LoginDataModule).userId.ToString();
	}

	public void TrackEvent(string eventName, Dictionary<string, object> eventProps)
	{
		bool flag = false;
		LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
		if (dataModule != null)
		{
			if (!dataModule.habbyMailBind)
			{
				flag = true;
			}
			else
			{
				if (eventName.Equals("cancel_subscribe"))
				{
					flag = true;
				}
				if (eventName.Equals("overseas_habby_register") && eventProps.ContainsKey("step") && eventProps["step"].ToString().Equals("connect_successful_show"))
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			GameApp.SDK.Analyze.Track_HabbyIDBind(eventName, eventProps);
		}
	}

	public void ShowLoading(bool show)
	{
		GameApp.View.ShowNetLoading(show);
	}

	public void OnStateChange()
	{
		Action onStateChanged = GameHabbyIdImpl.OnStateChanged;
		if (onStateChanged == null)
		{
			return;
		}
		onStateChanged();
	}

	public void LoginHabbyId(string authCode, string email, LoginMode mode, Action<bool, int> callback)
	{
		if (mode == 1)
		{
			callback(true, 0);
			return;
		}
		if (mode == null || mode == 2)
		{
			LanguageDataModule dataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			NetworkUtils.User.bindHabbyId(email, authCode, GameApp.SDK.Analyze.GetDeviceID(), GameApp.SDK.Analyze.GetDistinctID(), dataModule.GetLanguageAbbr(dataModule.GetCurrentLanguageType), delegate(bool res, UserHabbyMailBindResponse resp)
			{
				Debug.Log("LoginHabbyId: " + resp.Code.ToString());
				if (resp != null && resp.Code == 0)
				{
					GameApp.Data.GetDataModule(DataName.LoginDataModule).SetHabbyID(true, resp.HabbyId);
					callback(true, 0);
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_HabbyIdData, null);
					return;
				}
				callback(false, (resp == null) ? 1 : resp.Code);
			});
		}
	}

	public void LogoutHabbyId(Action<bool, int> callback)
	{
	}
}
