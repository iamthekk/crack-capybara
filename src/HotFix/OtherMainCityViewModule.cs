using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.User;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class OtherMainCityViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_otherMainCityDataModule = GameApp.Data.GetDataModule(DataName.OtherMainCityDataModule);
			this.m_playerGroup.Init();
			this.m_nodes[MainCityName.MainCity] = this.m_otherMainCity;
			this.m_nodes[MainCityName.Gold] = this.m_gold;
			this.m_nodes[MainCityName.Box] = this.m_box;
			this.m_nodes[MainCityName.Guild] = this.m_guild;
			this.m_nodes[MainCityName.Shop] = this.m_shop;
			foreach (KeyValuePair<MainCityName, BaseOtherMainCityNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.Init();
					keyValuePair.Value.SetActive(false);
				}
			}
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as OtherMainCityViewModule.OpenData;
			if (this.m_animator != null)
			{
				this.m_animator.SetTrigger("Idle");
			}
			this.m_backBt.onClick.AddListener(new UnityAction(this.OnClickBackBt));
			this.m_netLoading.SetActive(true);
			if (this.m_openData != null)
			{
				NetworkUtils.User.DoUserGetCityInfoRequest(this.m_openData.m_userID, delegate(bool isOk, UserGetCityInfoResponse rep)
				{
					if (!isOk)
					{
						return;
					}
					EventArgsSetOtherMainCityData instance = Singleton<EventArgsSetOtherMainCityData>.Instance;
					instance.SetData(this.m_openData.m_userID, rep);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_OtherMainCityData_SetData, instance);
					this.OnRefreshUI(this.m_otherMainCityDataModule.Data);
					if (this.m_animator != null)
					{
						this.m_animator.SetTrigger("Play");
					}
				});
			}
			else
			{
				this.OnRefreshUI(this.m_otherMainCityDataModule.Data);
				if (this.m_animator != null)
				{
					this.m_animator.SetTrigger("Play");
				}
			}
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Battle, new HandlerEvent(this.OnEventConquerRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Revolt, new HandlerEvent(this.OnEventConquerRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Loot, new HandlerEvent(this.OnEventConquerRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Pardon, new HandlerEvent(this.OnEventConquerRefreshUI));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Battle, new HandlerEvent(this.OnEventConquerRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Revolt, new HandlerEvent(this.OnEventConquerRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Loot, new HandlerEvent(this.OnEventConquerRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Pardon, new HandlerEvent(this.OnEventConquerRefreshUI));
			this.m_backBt.onClick.RemoveAllListeners();
			foreach (KeyValuePair<MainCityName, BaseOtherMainCityNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.OnHide();
				}
			}
		}

		public override void OnDelete()
		{
			foreach (KeyValuePair<MainCityName, BaseOtherMainCityNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
			this.m_playerGroup.DeInit();
			this.m_otherMainCityDataModule = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickBackBt()
		{
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				LoadingMainViewModule loadingViewModule = GameApp.View.GetViewModule(ViewName.LoadingMainViewModule);
				loadingViewModule.PlayShow(delegate
				{
					GameApp.View.CloseView(ViewName.OtherMainCityViewModule, null);
					loadingViewModule.PlayHide(delegate
					{
						GameApp.View.CloseView(ViewName.LoadingMainViewModule, null);
					});
				});
			});
		}

		private void OnRefreshUI(UserGetCityInfoResponse rep)
		{
			if (this.m_netLoading != null)
			{
				this.m_netLoading.SetActive(false);
			}
			foreach (KeyValuePair<MainCityName, BaseOtherMainCityNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.SetActive(true);
					keyValuePair.Value.SetLock(false);
					keyValuePair.Value.SetCityInfoResponse(rep);
					keyValuePair.Value.OnShow();
				}
			}
			if (this.m_playerGroup != null)
			{
				this.m_playerGroup.RefreshUI(rep);
			}
		}

		private void OnEventConquerRefreshUI(object sender, int type, BaseEventArgs eventargs)
		{
			BaseOtherMainCityNode baseOtherMainCityNode;
			if (this.m_nodes.TryGetValue(MainCityName.MainCity, out baseOtherMainCityNode))
			{
				(baseOtherMainCityNode as OtherMainCity_MainCityNode).OnRefreshUI();
			}
		}

		public OtherMainCityViewModule.OpenData m_openData;

		private Dictionary<MainCityName, BaseOtherMainCityNode> m_nodes = new Dictionary<MainCityName, BaseOtherMainCityNode>();

		public OtherMainCityPlayerGroup m_playerGroup;

		public GameObject m_netLoading;

		public CustomButton m_backBt;

		public Animator m_animator;

		public OtherMainCityDataModule m_otherMainCityDataModule;

		public OtherMainCity_MainCityNode m_otherMainCity;

		public OtherMainCity_DefaultNode m_gold;

		public OtherMainCity_DefaultNode m_box;

		public OtherMainCity_DefaultNode m_guild;

		public OtherMainCity_DefaultNode m_shop;

		public class OpenData
		{
			public long m_userID;
		}
	}
}
