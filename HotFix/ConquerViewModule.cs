using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.Conquer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class ConquerViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_ticketDataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			this.m_conquerDataModule = GameApp.Data.GetDataModule(DataName.ConquerDataModule);
			this.m_currencyAP.CurrencyType = CurrencyType.AP;
			this.m_currencyAP.Init();
			this.m_playerNode.Init();
			this.m_lordNode.Init();
			this.m_slaveNodePrefab.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as ConquerViewModule.OpenData;
			this.OnRefreshAPCurrency();
			if (this.m_openData != null)
			{
				EventArgsSetConquerData instance = Singleton<EventArgsSetConquerData>.Instance;
				instance.SetData(this.m_openData.m_targetUserID, this.m_openData.m_targetNick);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ConquerData_SetData, instance);
			}
			string text = (string.IsNullOrEmpty(this.m_conquerDataModule.TargetNick) ? DxxTools.GetDefaultNick(this.m_conquerDataModule.TargetUserID) : this.m_conquerDataModule.TargetNick);
			if (this.m_conquerDataModule.IsUser)
			{
				this.m_userBg.SetActive(true);
				this.m_otherBg.SetActive(false);
				this.m_userTitleTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6601, new object[] { text });
			}
			else
			{
				this.m_userBg.SetActive(false);
				this.m_otherBg.SetActive(true);
				this.m_otherTitleTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6601, new object[] { text });
			}
			this.m_content.SetActive(false);
			this.m_netLoading.SetActive(true);
			if (this.m_openData != null)
			{
				NetworkUtils.Conquer.DoConquerListRequest(this.m_openData.m_targetUserID, delegate(bool isOk, ConquerListResponse rep)
				{
					if (!isOk)
					{
						return;
					}
					EventArgsSetConquerResponseData instance2 = Singleton<EventArgsSetConquerResponseData>.Instance;
					instance2.SetData(rep);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_ConquerData_SetResponseData, instance2);
					if (!GameApp.View.IsOpened(ViewName.ConquerViewModule))
					{
						return;
					}
					this.OnRefresh();
				});
			}
			else
			{
				this.OnRefresh();
			}
			this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			this.m_maskBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_closeBt.onClick.RemoveAllListeners();
			this.m_maskBt.onClick.RemoveAllListeners();
			this.m_sequencePool.Clear(false);
			this.OnDestroySlaveNodes();
		}

		public override void OnDelete()
		{
			this.m_currencyAP.DeInit();
			if (this.m_playerNode != null)
			{
				this.m_playerNode.DeInit();
			}
			if (this.m_lordNode != null)
			{
				this.m_lordNode.DeInit();
			}
			this.m_loginDataModule = null;
			this.m_conquerDataModule = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_AP_Update, new HandlerEvent(this.OnEventRefreshAPCurrency));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Battle, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Revolt, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Loot, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Pardon, new HandlerEvent(this.OnEventRefreshPardonUI));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_AP_Update, new HandlerEvent(this.OnEventRefreshAPCurrency));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Battle, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Revolt, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Loot, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Pardon, new HandlerEvent(this.OnEventRefreshPardonUI));
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.ConquerViewModule, null);
		}

		private void OnEventRefreshAPCurrency(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshAPCurrency();
		}

		private void OnEventRefreshUI(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefresh();
		}

		private void OnEventRefreshPardonUI(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsConquerTypeData eventArgsConquerTypeData = eventargs as EventArgsConquerTypeData;
			if (eventArgsConquerTypeData == null)
			{
				return;
			}
			if (this.m_conquerDataModule.Data.UserId == eventArgsConquerTypeData.m_targetUserID)
			{
				this.OnRefresh();
				return;
			}
			ConquerSlaveNode conquerSlaveNode = null;
			foreach (KeyValuePair<int, ConquerSlaveNode> keyValuePair in this.m_slaveNodes)
			{
				if (!(keyValuePair.Value == null) && keyValuePair.Value.m_data.UserId == eventArgsConquerTypeData.m_targetUserID)
				{
					conquerSlaveNode = keyValuePair.Value;
					break;
				}
			}
			if (conquerSlaveNode == null)
			{
				return;
			}
			Sequence sequence = this.m_sequencePool.Get();
			Vector2 sizeDelta = conquerSlaveNode.rectTransform.sizeDelta;
			Vector2 vector;
			vector..ctor(sizeDelta.x, 0f);
			Vector3 vector2;
			vector2..ctor(0f, 0f, 1f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOSizeDelta(conquerSlaveNode.rectTransform, vector, 0.25f, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(conquerSlaveNode.m_root, vector2, 0.25f));
			TweenSettingsExtensions.OnUpdate<Sequence>(sequence, delegate
			{
				if (this.m_scrollConent == null)
				{
					return;
				}
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_scrollConent);
				LayoutRebuilder.MarkLayoutForRebuild(this.m_scrollConent);
			});
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
			{
				if (!GameApp.View.IsOpened(ViewName.ConquerViewModule))
				{
					return;
				}
				this.OnRefresh();
			});
		}

		private void OnRefresh()
		{
			if (this.m_content != null)
			{
				this.m_content.SetActive(true);
			}
			if (this.m_netLoading != null)
			{
				this.m_netLoading.SetActive(false);
			}
			if (this.m_playerNode != null)
			{
				this.m_playerNode.RefreshUI(this.m_conquerDataModule.Data, this.m_loginDataModule.userId, this.m_conquerDataModule.IsUser);
			}
			if (this.m_lordNode != null)
			{
				this.m_lordNode.RefreshUI(this.m_conquerDataModule.Data, this.m_loginDataModule.userId, this.m_conquerDataModule.IsUser);
			}
			if (this.m_conquerDataModule.Data.Slaves == null || this.m_conquerDataModule.Data.Slaves.Count <= 0)
			{
				this.m_unSlave.gameObject.SetActive(true);
				this.m_scrollRect.gameObject.SetActive(false);
				return;
			}
			this.m_unSlave.gameObject.SetActive(false);
			this.m_scrollRect.gameObject.SetActive(true);
			this.OnDestroySlaveNodes();
			this.CreateSlaveNodes();
		}

		private void OnDestroySlaveNodes()
		{
			foreach (KeyValuePair<int, ConquerSlaveNode> keyValuePair in this.m_slaveNodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_slaveNodes.Clear();
		}

		private void CreateSlaveNodes()
		{
			for (int i = 0; i < this.m_conquerDataModule.Data.Slaves.Count; i++)
			{
				ConquerUserDto conquerUserDto = this.m_conquerDataModule.Data.Slaves[i];
				if (conquerUserDto != null)
				{
					ConquerSlaveNode conquerSlaveNode = Object.Instantiate<ConquerSlaveNode>(this.m_slaveNodePrefab);
					conquerSlaveNode.transform.SetParentNormal(this.m_scrollConent, false);
					conquerSlaveNode.SetActive(true);
					conquerSlaveNode.Init();
					conquerSlaveNode.RefreshUI(conquerUserDto, this.m_loginDataModule.userId, this.m_conquerDataModule.IsUser);
					this.m_slaveNodes[conquerSlaveNode.GetObjectInstanceID()] = conquerSlaveNode;
				}
			}
		}

		private void OnRefreshAPCurrency()
		{
			if (this.m_currencyAP == null)
			{
				return;
			}
			this.m_currencyAP.SetText(string.Format("{0}/{1}", this.m_ticketDataModule.GetTicketCount(UserTicketKind.UserLife), this.m_loginDataModule.UserLiftMaxValue));
		}

		public CustomButton m_maskBt;

		public CustomButton m_closeBt;

		public CurrencyUICtrl m_currencyAP;

		public GameObject m_userBg;

		public CustomText m_userTitleTxt;

		public GameObject m_otherBg;

		public CustomText m_otherTitleTxt;

		public GameObject m_content;

		public GameObject m_netLoading;

		public ConquerPlayerNode m_playerNode;

		public ConquerLordNode m_lordNode;

		public ScrollRect m_scrollRect;

		public RectTransform m_scrollConent;

		public ConquerSlaveNode m_slaveNodePrefab;

		public GameObject m_unSlave;

		private SequencePool m_sequencePool = new SequencePool();

		private Dictionary<int, ConquerSlaveNode> m_slaveNodes = new Dictionary<int, ConquerSlaveNode>();

		public ConquerViewModule.OpenData m_openData;

		private LoginDataModule m_loginDataModule;

		private TicketDataModule m_ticketDataModule;

		private ConquerDataModule m_conquerDataModule;

		public class OpenData
		{
			public long m_targetUserID;

			public string m_targetNick;
		}
	}
}
