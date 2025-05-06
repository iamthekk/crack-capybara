using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Common;
using Proto.Tower;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class RogueDungeonViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			this.ticketDataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			this.copyRewardItem.SetActiveSafe(false);
			this.copyMonsterItem.SetActiveSafe(false);
			this.uiItemInfoButton.Init();
			this.uiItemInfoButton.SetItemIcon(34);
			this.uiItemInfoButton.SetOnClick(new Action(this.OnClickChallenge));
			this.uiItemInfoButton.SetInfoText(Singleton<LanguageManager>.Instance.GetInfoByID("uitower_challenge"));
			this.buttonBack.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonRank.onClick.AddListener(new UnityAction(this.OnClickRank));
			List<int> list = new List<int>();
			list.Add(2);
			list.Add(1);
			list.Add(34);
			this.currencyCtrl.Init();
			this.currencyCtrl.SetStyle(EModuleId.RogueDungeon, list);
			this.helpButton.Init();
		}

		public override void OnOpen(object data)
		{
			RogueDungeon_rogueDungeon rogueDungeon_rogueDungeon = GameApp.Table.GetManager().GetRogueDungeon_rogueDungeon((int)this.mDataModule.CurrentFloorID);
			if (rogueDungeon_rogueDungeon == null)
			{
				this.rewardObj.SetActiveSafe(false);
				this.monsterObj.SetActiveSafe(false);
				this.uiItemInfoButton.gameObject.SetActiveSafe(false);
				this.textFloor.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiroguedungeon_pass_all");
				return;
			}
			this.rewardObj.SetActiveSafe(true);
			this.monsterObj.SetActiveSafe(true);
			this.uiItemInfoButton.gameObject.SetActiveSafe(true);
			this.textFloor.text = Singleton<LanguageManager>.Instance.GetInfoByID(rogueDungeon_rogueDungeon.name);
			List<PropData> list;
			if (LoginDataModule.IsTestB())
			{
				list = rogueDungeon_rogueDungeon.firstRewardB.ToPropDataList();
			}
			else
			{
				list = rogueDungeon_rogueDungeon.firstReward.ToPropDataList();
			}
			for (int i = 0; i < list.Count; i++)
			{
				UIItem uiitem;
				if (i < this.itemList.Count)
				{
					uiitem = this.itemList[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyRewardItem);
					gameObject.SetParentNormal(this.rewardParent, false);
					uiitem = gameObject.GetComponent<UIItem>();
					this.itemList.Add(uiitem);
					uiitem.Init();
				}
				uiitem.gameObject.SetActiveSafe(true);
				uiitem.SetData(list[i]);
				uiitem.OnRefresh();
			}
			for (int j = 0; j < rogueDungeon_rogueDungeon.showMonster.Length; j++)
			{
				List<long> listLong = rogueDungeon_rogueDungeon.showMonster[j].GetListLong(',');
				if (listLong.Count < 2)
				{
					return;
				}
				int num = (int)listLong[0];
				long num2 = listLong[1];
				UIShowMonsterItem uishowMonsterItem;
				if (j < this.monsterItems.Count)
				{
					uishowMonsterItem = this.monsterItems[j];
				}
				else
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.copyMonsterItem);
					gameObject2.SetParentNormal(this.monsterParent, false);
					uishowMonsterItem = gameObject2.GetComponent<UIShowMonsterItem>();
					this.monsterItems.Add(uishowMonsterItem);
					uishowMonsterItem.Init();
				}
				uishowMonsterItem.gameObject.SetActiveSafe(true);
				uishowMonsterItem.SetData(num, num2);
			}
			this.RefreshInfo();
			this.PlayOpenAni();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonBack.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonRank.onClick.RemoveListener(new UnityAction(this.OnClickRank));
			this.currencyCtrl.DeInit();
			for (int i = 0; i < this.itemList.Count; i++)
			{
				UIItem uiitem = this.itemList[i];
				if (uiitem)
				{
					uiitem.DeInit();
				}
			}
			this.itemList.Clear();
			for (int j = 0; j < this.monsterItems.Count; j++)
			{
				UIShowMonsterItem uishowMonsterItem = this.monsterItems[j];
				if (uishowMonsterItem)
				{
					uishowMonsterItem.DeInit();
				}
			}
			this.monsterItems.Clear();
			this.sequencePool.Clear(false);
			this.helpButton.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
		}

		private void RefreshInfo()
		{
			UserTicket ticket = this.ticketDataModule.GetTicket(UserTicketKind.RogueDungeon);
			string text = ((((ticket != null) ? ticket.NewNum : 0U) > 0U) ? "<color=#FFFFFF>1</color>" : "<color=#FF0000>1</color>");
			this.uiItemInfoButton.SetCountText(text, false);
		}

		private void OnClickChallenge()
		{
			if (this.isClick)
			{
				return;
			}
			this.isClick = true;
			UserTicket ticket = this.ticketDataModule.GetTicket(UserTicketKind.RogueDungeon);
			if (ticket != null && ticket.NewNum > 0U)
			{
				NetworkUtils.RogueDungeon.DoEnterChallengeRequest(delegate(bool result, HellEnterBattleResponse response)
				{
					if (result)
					{
						this.DoBattle();
						return;
					}
					this.isClick = false;
				});
				return;
			}
			this.isClick = false;
			GameApp.View.ShowItemNotEnoughTip(UserTicketKind.RogueDungeon.GetHashCode(), true);
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.RogueDungeonViewModule, null);
		}

		private void OnClickRank()
		{
			GameApp.View.OpenView(ViewName.RogueDungeonRankViewModule, null, 1, null, null);
		}

		private void DoBattle()
		{
			EventArgsAddItemTipData eventArgsAddItemTipData = new EventArgsAddItemTipData();
			eventArgsAddItemTipData.SetDataCount(34, -1, this.uiItemInfoButton.transform.position);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddItemTipNode, eventArgsAddItemTipData);
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					this.isClick = false;
					this.RefreshInfo();
					EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
					instance.SetData(DxxTools.UI.GetRogueDungeonOpenData());
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
					this.OnClickClose();
					EventArgsGameDataEnter instance2 = Singleton<EventArgsGameDataEnter>.Instance;
					instance2.SetData(GameModel.RogueDungeon, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance2);
					GameApp.State.ActiveState(StateName.BattleRogueDungeonState);
				});
			});
		}

		private void PlayOpenAni()
		{
			this.bgRT.localScale = Vector3.one * 0.9f;
			this.bottomRT.anchoredPosition = new Vector2(this.bottomRT.anchoredPosition.x, -500f);
			float num = 0.2f;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.bgRT, Vector3.one, num));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.bottomRT, 0f, num, false));
		}

		private void OnTicketUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshInfo();
		}

		[GameTestMethod("地牢", "地牢", "", 502)]
		private static void OpenRogueDungeon()
		{
			GameApp.View.OpenView(ViewName.RogueDungeonViewModule, null, 1, null, null);
		}

		public CustomText textFloor;

		public GameObject rewardParent;

		public GameObject copyRewardItem;

		public GameObject monsterParent;

		public GameObject copyMonsterItem;

		public CustomButton buttonBack;

		public CustomButton buttonRank;

		public UIItemInfoButton uiItemInfoButton;

		public ModuleCurrencyCtrl currencyCtrl;

		public GameObject rewardObj;

		public GameObject monsterObj;

		public UIHelpButton helpButton;

		[Header("打开动画")]
		public RectTransform bgRT;

		public RectTransform bottomRT;

		private List<UIItem> itemList = new List<UIItem>();

		private List<UIShowMonsterItem> monsterItems = new List<UIShowMonsterItem>();

		private RogueDungeonDataModule mDataModule;

		private TicketDataModule ticketDataModule;

		private SequencePool sequencePool = new SequencePool();

		private bool isClick;
	}
}
