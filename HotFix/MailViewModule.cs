using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Habby.Mail.Data;
using LocalModels.Bean;
using Proto.Common;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class MailViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.MailViewModule;
		}

		public override void OnCreate(object data)
		{
			this.m_mailDataModule = GameApp.Data.GetDataModule(DataName.MailDataModule);
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as MailViewModule.OpenData;
			this.m_popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.m_netLoading.SetActive(true);
			this.m_unHave.SetActive(false);
			this.m_scroll.gameObject.SetActive(false);
			this.m_scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.GetMailListInfo();
			this.m_awardAllBt.onClick.AddListener(new UnityAction(this.OnClickAwardAllBt));
			this.m_deleteAllBt.onClick.AddListener(new UnityAction(this.OnClickDeleteAllBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.m_openData != null)
			{
				Action onCloseCallback = this.m_openData.onCloseCallback;
				if (onCloseCallback != null)
				{
					onCloseCallback();
				}
				this.m_openData = null;
			}
			this.m_popCommon.OnClick = null;
			this.m_awardAllBt.onClick.RemoveAllListeners();
			this.m_deleteAllBt.onClick.RemoveAllListeners();
			this.m_sequencePool.Clear(false);
		}

		public override void OnDelete()
		{
			foreach (KeyValuePair<int, MailItemNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
			this.m_datas.Clear();
			this.m_mailDataModule = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_MailData_RefreshUI, new HandlerEvent(this.OnEventRefreshUI));
			manager.RegisterEvent(LocalMessageName.CC_MailData_GetListData, new HandlerEvent(this.OnEventRefreshUI));
			manager.RegisterEvent(LocalMessageName.CC_MailData_ReadMail, new HandlerEvent(this.OnEventRefreshUI));
			manager.RegisterEvent(LocalMessageName.CC_MailData_DeleteData, new HandlerEvent(this.OnEventRefreshUI));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_RefreshUI, new HandlerEvent(this.OnEventRefreshUI));
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_GetListData, new HandlerEvent(this.OnEventRefreshUI));
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_ReadMail, new HandlerEvent(this.OnEventRefreshUI));
			manager.UnRegisterEvent(LocalMessageName.CC_MailData_DeleteData, new HandlerEvent(this.OnEventRefreshUI));
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBt();
			}
		}

		private void OnClickCloseBt()
		{
			if (this.m_openData != null)
			{
				MoreExtensionViewModule.TryBackOpenView(this.m_openData.srcViewName);
			}
			GameApp.View.CloseView(this.GetName(), null);
		}

		private void OnClickAwardAllBt()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				MailData mailData = this.m_datas[i];
				if (mailData != null && !mailData.isReward && mailData.rewards != null && mailData.rewards.Length != 0)
				{
					list.Add(mailData.mailId);
				}
			}
			if (list.Count <= 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("4144"));
				return;
			}
			GameApp.Mail.GetManager().Control.SendGetMailReward(list.ToArray(), delegate(List<MailRewardObject> resps, CommonData commonData)
			{
				for (int j = 0; j < resps.Count; j++)
				{
					MailRewardObject mailRewardObject = resps[j];
					if (mailRewardObject != null)
					{
						EventMailReceiveAwards instance = Singleton<EventMailReceiveAwards>.Instance;
						instance.SetData(mailRewardObject);
						GameApp.Event.DispatchNow(this, 226, instance);
					}
				}
				DxxTools.UI.OpenRewardCommon(commonData.Reward, delegate
				{
					GameApp.Event.DispatchNow(this, 229, null);
				}, true);
				if (commonData != null && commonData.Reward != null)
				{
					bool flag = false;
					for (int k = 0; k < commonData.Reward.Count; k++)
					{
						RewardDto rewardDto = commonData.Reward[k];
						if (rewardDto != null)
						{
							Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)rewardDto.ConfigId);
							if (elementById != null && elementById.itemType == 46)
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_ShopInfo_DataPull, null);
					}
				}
				GameApp.Data.GetDataModule(DataName.LoginDataModule).EmailSetUserInfo(commonData.UserInfoDto);
				RedPointController.Instance.ReCalc("Main.Mail", true);
				GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(999999), null, null, null, null, resps);
				GameApp.SDK.Analyze.Track_Mail(resps);
			}, true);
		}

		private void OnClickDeleteAllBt()
		{
			List<string> idList = new List<string>();
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				MailData mailData = this.m_datas[i];
				if (mailData.IsReadShow())
				{
					idList.Add(mailData.mailId);
				}
			}
			if (idList.Count == 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("9403"));
				return;
			}
			GameApp.Mail.GetManager().Control.SendDeleteMails(idList.ToArray(), delegate(MailsDeleteResponse resp)
			{
				EventMailDelete instance = Singleton<EventMailDelete>.Instance;
				instance.SetData(idList);
				GameApp.Event.DispatchNow(this, 227, instance);
			}, true);
		}

		private void OnEventRefreshUI(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshUI();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			MailData mailData = this.m_datas[index];
			if (mailData == null)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("Node");
			MailItemNode component;
			this.m_nodes.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<MailItemNode>();
				component.Init();
				this.m_nodes[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			component.RefreshData(mailData, index);
			return loopListViewItem;
		}

		public void GetMailListInfo()
		{
			GameApp.Mail.GetManager().Control.SendGetMailList(delegate(List<MailData> list)
			{
				EventMailList instance = Singleton<EventMailList>.Instance;
				instance.SetData(list);
				GameApp.Event.DispatchNow(this, 225, instance);
			}, false);
		}

		public void OnRefreshUI()
		{
			this.m_datas = this.m_mailDataModule.GetMailDatas().Values.ToList<MailData>();
			IOrderedEnumerable<MailData> orderedEnumerable = from dto in this.m_datas
				orderby dto.IsReadShow(), dto.GetEffectiveAt() descending
				select dto;
			this.m_datas = orderedEnumerable.ToList<MailData>();
			bool activeSelf = this.m_netLoading.activeSelf;
			this.m_netLoading.SetActive(false);
			bool flag = this.m_datas.Count > 0;
			this.m_unHave.SetActive(!flag);
			this.m_scroll.gameObject.SetActive(flag);
			if (!flag)
			{
				return;
			}
			this.m_scroll.SetListItemCount(this.m_datas.Count, true);
			this.m_scroll.RefreshAllShowItems();
			if (activeSelf && flag)
			{
				this.PlayScale();
			}
		}

		private void PlayScale()
		{
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_sequencePool.Get(), this.m_scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		public UIPopCommon m_popCommon;

		public GameObject m_netLoading;

		public GameObject m_unHave;

		public LoopListView2 m_scroll;

		public CustomButton m_awardAllBt;

		public CustomButton m_deleteAllBt;

		private SequencePool m_sequencePool = new SequencePool();

		private Dictionary<int, MailItemNode> m_nodes = new Dictionary<int, MailItemNode>();

		private List<MailData> m_datas = new List<MailData>();

		private MailDataModule m_mailDataModule;

		private MailViewModule.OpenData m_openData;

		public class OpenData
		{
			public ViewName srcViewName;

			public Action onCloseCallback;
		}
	}
}
