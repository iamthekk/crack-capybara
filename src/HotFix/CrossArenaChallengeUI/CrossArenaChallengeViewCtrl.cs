using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Proto.CrossArena;
using UnityEngine;

namespace HotFix.CrossArenaChallengeUI
{
	public class CrossArenaChallengeViewCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
			this.Button_Refresh.Init();
			this.Button_Refresh.SetInfoText(Singleton<LanguageManager>.Instance.GetInfoByID("15003"));
			this.Button_Refresh.SetOnClick(new Action(this.OnForceRefreshList));
			this.TicketUI.Init();
			this.TicketUI.OnClick = new Action<CurrencyType>(this.InternalClickTicket);
			this.UpdateTicket(null, 0, null);
			this.Prefab_Opp.SetActive(false);
			this.Obj_NetLoading.SetActive(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.m_seqPool.Clear(false);
			this.TicketUI.OnClick = null;
			this.TicketUI.DeInit();
			this.Button_Refresh.DeInit();
			for (int i = 0; i < this.mUIList.Count; i++)
			{
				this.mUIList[i].DeInit();
			}
			this.mUIList.Clear();
			this.RTF_OppList.DestroyChildren();
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_CrossArena_RefreshChallengeList, new HandlerEvent(this.OnRefreshChallengeList));
			manager.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.UpdateTicket));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_CrossArena_RefreshChallengeList, new HandlerEvent(this.OnRefreshChallengeList));
			manager.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.UpdateTicket));
		}

		public void OnViewOpen(object data)
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			this.RefreshOppListAsEmpty();
			this.Obj_NetLoading.SetActive(true);
			this.RefreshRefreshCost();
			NetworkUtils.CrossArena.DoCrossArenaChallengeListRequest(false, delegate(bool result, CrossArenaChallengeListResponse resp)
			{
				if (this.Obj_NetLoading != null)
				{
					this.Obj_NetLoading.SetActive(false);
				}
			});
		}

		public void OnViewClose()
		{
			this.m_seqPool.Clear(false);
		}

		public void SetView(CrossArenaChallengeViewModule view)
		{
			this.mView = view;
		}

		public bool IsViewOpen()
		{
			return this.mView != null && this.mView.isActiveAndEnabled && base.gameObject != null && base.gameObject.activeSelf;
		}

		private void OnCloseThis()
		{
			if (this.mView != null)
			{
				GameApp.View.CloseView(this.mView.GetName(), null);
			}
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType type)
		{
			this.OnCloseThis();
		}

		private void OnClickShowPlayerInfo(long userid)
		{
			if (userid == 0L)
			{
				return;
			}
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(userid);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		private void UpdateTicket(object sender, int type, BaseEventArgs eventArgs)
		{
			uint newNum = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.CrossArena).NewNum;
			this.TicketUI.SetText(DxxTools.FormatNumber((long)((ulong)newNum)));
		}

		private void InternalClickTicket(CurrencyType currencyType)
		{
			CommonTicketBuyTipModule.OpenData openData = default(CommonTicketBuyTipModule.OpenData);
			openData.SetData(UserTicketKind.CrossArena);
			GameApp.View.OpenView(ViewName.CommonTicketBuyTipModule, openData, 1, null, null);
		}

		private void OnForceRefreshList()
		{
			int diamonds = GameApp.Data.GetDataModule(DataName.LoginDataModule).userCurrency.Diamonds;
			int refreshOppListCount = this.mDataModule.RefreshOppListCount;
			if (this.GetRefreshCost(refreshOppListCount) > diamonds)
			{
				GameApp.View.ShowItemNotEnoughTip(2, true);
				return;
			}
			this.RefreshOppListAsEmpty();
			this.Obj_NetLoading.SetActive(true);
			this.RefreshRefreshCost();
			NetworkUtils.CrossArena.DoCrossArenaChallengeListRequest(true, delegate(bool result, CrossArenaChallengeListResponse resp)
			{
				if (this.Obj_NetLoading != null)
				{
					this.Obj_NetLoading.SetActive(false);
				}
			});
		}

		private void OnRefreshChallengeList(object sender, int type, BaseEventArgs eventArgs)
		{
			this.Obj_NetLoading.SetActive(false);
			this.RefreshRefreshCost();
			this.mOppDataList.AddRange(this.mDataModule.OppList);
			this.RefreshOppList();
		}

		private void RefreshOppListAsEmpty()
		{
			this.mOppDataList.Clear();
			this.RefreshOppList();
		}

		private void RefreshOppList()
		{
			this.m_seqPool.Clear(false);
			float num = -10f;
			for (int i = 0; i < this.mOppDataList.Count; i++)
			{
				CrossArenaRankMember crossArenaRankMember = this.mOppDataList[i];
				CrossArenaChallengeOppItem crossArenaChallengeOppItem;
				if (i < this.mUIList.Count)
				{
					crossArenaChallengeOppItem = this.mUIList[i];
				}
				else
				{
					crossArenaChallengeOppItem = null;
				}
				if (crossArenaChallengeOppItem == null)
				{
					crossArenaChallengeOppItem = Object.Instantiate<GameObject>(this.Prefab_Opp, this.RTF_OppList).GetComponent<CrossArenaChallengeOppItem>();
					crossArenaChallengeOppItem.Init();
					if (i < this.mUIList.Count)
					{
						this.mUIList[i] = crossArenaChallengeOppItem;
					}
					else
					{
						this.mUIList.Add(crossArenaChallengeOppItem);
					}
				}
				crossArenaChallengeOppItem.rectTransform.anchoredPosition = new Vector2(-2000f, num);
				num -= crossArenaChallengeOppItem.rectTransform.sizeDelta.y + 10f;
				crossArenaChallengeOppItem.SetData(crossArenaRankMember);
				crossArenaChallengeOppItem.RefreshUI();
				crossArenaChallengeOppItem.OnShowPlayerInfo = new Action<long>(this.OnClickShowPlayerInfo);
				crossArenaChallengeOppItem.SetActive(true);
				DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), crossArenaChallengeOppItem.rectTransform, 0f, (float)i * 0.05f, 0.2f, 9);
			}
			for (int j = this.mOppDataList.Count; j < this.mUIList.Count; j++)
			{
				this.mUIList[j].SetActive(false);
			}
		}

		private void RefreshRefreshCost()
		{
			if (this.mDataModule.RefreshOppListCount < 0)
			{
				this.Button_Refresh.gameObject.SetActive(false);
				return;
			}
			int refreshCost = this.GetRefreshCost(this.mDataModule.RefreshOppListCount);
			if (refreshCost <= 0)
			{
				this.Button_Refresh.gameObject.SetActive(true);
				this.Button_Refresh.SetCountText(Singleton<LanguageManager>.Instance.GetInfoByID("101"), true);
				return;
			}
			this.Button_Refresh.SetItemIcon(2);
			this.Button_Refresh.SetCountText(refreshCost.ToString(), false);
			this.Button_Refresh.gameObject.SetActive(true);
		}

		private int GetRefreshCost(int refcount)
		{
			int[] crossArena_RefreshOppListCost = Singleton<GameConfig>.Instance.CrossArena_RefreshOppListCost;
			int num;
			if (refcount < 0)
			{
				num = 0;
			}
			else if (refcount >= crossArena_RefreshOppListCost.Length)
			{
				num = crossArena_RefreshOppListCost[crossArena_RefreshOppListCost.Length - 1];
			}
			else
			{
				num = crossArena_RefreshOppListCost[refcount];
			}
			return num;
		}

		public UIPopCommon PopCommon;

		public UIItemInfoButton Button_Refresh;

		public CurrencyUICtrl TicketUI;

		public RectTransform RTF_OppList;

		public GameObject Prefab_Opp;

		public GameObject Obj_NetLoading;

		private CrossArenaDataModule mDataModule;

		private CrossArenaChallengeViewModule mView;

		private List<CrossArenaRankMember> mOppDataList = new List<CrossArenaRankMember>();

		private List<CrossArenaChallengeOppItem> mUIList = new List<CrossArenaChallengeOppItem>();

		private SequencePool m_seqPool = new SequencePool();
	}
}
