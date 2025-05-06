using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Common;
using Proto.Mining;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class MiningViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.miningDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			this.ticketDataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.adDataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.buttonAutoMining.onClick.AddListener(new UnityAction(this.OnClickAutoMining));
			this.buttonStopAuto.onClick.AddListener(new UnityAction(this.OnClickStopAuto));
			this.buttonBack.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonBonus.onClick.AddListener(new UnityAction(this.OnClickBonus));
			this.buttonAutoMining.gameObject.SetActiveSafe(true);
			this.buttonStopAuto.gameObject.SetActiveSafe(false);
			this.moduleCurrencyCtrl.Init();
			this.moduleCurrencyCtrl.SetStyle(EModuleId.Mining, new List<int> { 2, 1, 33 });
			for (int i = 0; i < this.miningGridCtrls.Count; i++)
			{
				UIMiningGridCtrl uiminingGridCtrl = this.miningGridCtrls[i];
				if (uiminingGridCtrl)
				{
					uiminingGridCtrl.Init();
				}
			}
			this.spinePickaxe.Init();
			this.spinePickaxe.gameObject.SetActiveSafe(false);
			this.clickMask.SetActiveSafe(false);
			this.nextFloorAni.Init();
			this.nextFloorAni.gameObject.SetActive(false);
			this.adButtonCtrl.Init();
			RedPointController.Instance.RegRecordChange("DailyActivity.ChallengeTag.Mining.Draw", new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
			this.helpButton.Init();
		}

		public override void OnOpen(object data)
		{
			this.miningDataModule.CacheTicket();
			this.isAutoMining = false;
			this.autoMiningCtrl = new AutoMiningController(this);
			this.RefreshInfo();
			this.RefreshGridMode();
			this.PlayOpenAni();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.refreshTimeObj.activeSelf)
			{
				long num = this.ticketDataModule.GetRecoverTimestamp(UserTicketKind.Mining) - DxxTools.Time.ServerTimestamp;
				if (num > 0L)
				{
					this.textTime.text = DxxTools.FormatTime(num);
				}
				else
				{
					this.textTime.text = "";
					this.refreshTimeObj.SetActiveSafe(false);
				}
			}
			if (Time.time - this.miningDataModule.cacheTime > 2f)
			{
				this.miningDataModule.CacheTicket();
			}
		}

		public override void OnClose()
		{
			this.autoMiningCtrl = null;
		}

		public override void OnDelete()
		{
			this.buttonAutoMining.onClick.RemoveListener(new UnityAction(this.OnClickAutoMining));
			this.buttonStopAuto.onClick.RemoveListener(new UnityAction(this.OnClickAutoMining));
			this.buttonBack.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonBonus.onClick.RemoveListener(new UnityAction(this.OnClickBonus));
			this.moduleCurrencyCtrl.DeInit();
			this.spinePickaxe.DeInit();
			this.nextFloorAni.DeInit();
			this.adButtonCtrl.DeInit();
			for (int i = 0; i < this.miningGridCtrls.Count; i++)
			{
				UIMiningGridCtrl uiminingGridCtrl = this.miningGridCtrls[i];
				if (uiminingGridCtrl)
				{
					uiminingGridCtrl.DeInit();
				}
			}
			this.miningGridCtrls.Clear();
			this.sequencePool.Clear(false);
			RedPointController.Instance.UnRegRecordChange("DailyActivity.ChallengeTag.Mining.Draw", new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
			this.helpButton.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Mining_PlayMining, new HandlerEvent(this.OnEventPlayMining));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Mining_GetAllReward, new HandlerEvent(this.OnEventGetAllReward));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Mining_NextFloor, new HandlerEvent(this.OnEventNextFloor));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Mining_Bomb, new HandlerEvent(this.OnEventMiningBomb));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Mining_Auto, new HandlerEvent(this.OnEventAutoMining));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Mining_RefreshInfo, new HandlerEvent(this.OnEventRefreshInfo));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Mining_ShowClickMask, new HandlerEvent(this.OnEventShowClickMask));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Mining_PlayMining, new HandlerEvent(this.OnEventPlayMining));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Mining_GetAllReward, new HandlerEvent(this.OnEventGetAllReward));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Mining_NextFloor, new HandlerEvent(this.OnEventNextFloor));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Mining_Bomb, new HandlerEvent(this.OnEventMiningBomb));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Mining_Auto, new HandlerEvent(this.OnEventAutoMining));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Mining_RefreshInfo, new HandlerEvent(this.OnEventRefreshInfo));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Mining_ShowClickMask, new HandlerEvent(this.OnEventShowClickMask));
		}

		private void RefreshInfo()
		{
			MiningInfoDto miningInfo = this.miningDataModule.MiningInfo;
			if (miningInfo == null)
			{
				return;
			}
			this.textCurrentFloor.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_floor", new object[] { miningInfo.Stage });
			this.bigRewardObj.SetActiveSafe(this.miningDataModule.IsHaveTreasure);
			this.refreshTimeObj.SetActiveSafe(!this.IsTicketMax());
			UserTicket ticket = this.ticketDataModule.GetTicket(UserTicketKind.Mining);
			if (ticket != null)
			{
				this.textTicket.text = string.Format("{0}/{1}", this.miningDataModule.cacheTicket.mVariable, ticket.RevertLimit);
			}
			else
			{
				this.textTicket.text = string.Format("0/{0}", GameConfig.Mining_Ticket_RecoverLimit);
			}
			int mining_Ticket_ID = GameConfig.Mining_Ticket_ID;
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(mining_Ticket_ID);
			if (item_Item != null)
			{
				string atlasPath = GameApp.Table.GetAtlasPath(item_Item.atlasID);
				this.imageTicket.SetImage(atlasPath, item_Item.icon);
			}
			int maxTimes = this.adDataModule.GetMaxTimes(12);
			int watchTimes = this.adDataModule.GetWatchTimes(12);
			if ((ticket == null || ticket.NewNum == 0U) && watchTimes < maxTimes)
			{
				this.ticketObj.SetActiveSafe(false);
				this.adButtonCtrl.gameObject.SetActiveSafe(true);
				this.adButtonCtrl.SetData(mining_Ticket_ID, GameConfig.Mining_AD_ItemNum, watchTimes, maxTimes, new Action(this.OnClickAd));
				return;
			}
			this.ticketObj.SetActiveSafe(true);
			this.adButtonCtrl.gameObject.SetActiveSafe(false);
		}

		private void RefreshGridMode()
		{
			if (this.miningDataModule.MiningInfo == null)
			{
				return;
			}
			MiningGrid gridType = this.miningDataModule.GetGridType();
			for (int i = 0; i < this.miningGridCtrls.Count; i++)
			{
				UIMiningGridCtrl uiminingGridCtrl = this.miningGridCtrls[i];
				if (uiminingGridCtrl)
				{
					if (uiminingGridCtrl.gridType == gridType)
					{
						this.currentCtrl = uiminingGridCtrl;
						uiminingGridCtrl.gameObject.SetActiveSafe(true);
						uiminingGridCtrl.SetData();
					}
					else
					{
						uiminingGridCtrl.gameObject.SetActiveSafe(false);
					}
				}
			}
		}

		private bool IsTicketMax()
		{
			UserTicket ticket = this.ticketDataModule.GetTicket(UserTicketKind.Mining);
			return ticket != null && ticket.NewNum >= ticket.RevertLimit;
		}

		private void PlayPickaxAnimation(Transform target, Action onFinish)
		{
			this.spinePickaxe.transform.position = target.position;
			float animationDuration = this.spinePickaxe.GetAnimationDuration("ChuiZi");
			this.spinePickaxe.gameObject.SetActiveSafe(true);
			this.spinePickaxe.PlayAnimation("ChuiZi", false);
			DelayCall.Instance.CallOnce((int)(animationDuration * 1000f), delegate
			{
				if (this.spinePickaxe != null)
				{
					this.spinePickaxe.gameObject.SetActiveSafe(false);
				}
				Action onFinish2 = onFinish;
				if (onFinish2 == null)
				{
					return;
				}
				onFinish2();
			});
		}

		private void PlayOpenAni()
		{
			this.titleRT.localScale = Vector3.one * 0.9f;
			this.centerRT.localScale = Vector3.one * 0.9f;
			this.bottomRT.anchoredPosition = new Vector2(this.bottomRT.anchoredPosition.x, -500f);
			this.autoBtnRT.anchoredPosition = new Vector2(500f, this.autoBtnRT.anchoredPosition.y);
			float num = 0.2f;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.titleRT, Vector3.one, num));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(this.centerRT, Vector3.one, num));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.bottomRT, 0f, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosX(this.autoBtnRT, 0f, num, false));
		}

		private void OnEventPlayMining(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshInfo();
			EventArgsMining eventArgsMining = eventArgs as EventArgsMining;
			if (eventArgsMining != null && eventArgsMining.gridItem != null)
			{
				this.PlayPickaxAnimation(eventArgsMining.gridItem.transform, null);
			}
		}

		private void OnEventGetAllReward(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.currentCtrl != null && this.miningDataModule.IsHaveTreasure)
			{
				this.currentCtrl.OpenTreasure();
			}
			this.RefreshInfo();
			this.RefreshGridMode();
		}

		private void OnEventNextFloor(object sender, int type, BaseEventArgs eventArgs)
		{
			GameApp.Sound.PlayClip(634, 1f);
			this.clickMask.SetActiveSafe(true);
			float animationDuration = this.nextFloorAni.GetAnimationDuration("Start");
			float animationDuration2 = this.nextFloorAni.GetAnimationDuration("End");
			this.nextFloorAni.gameObject.SetActiveSafe(true);
			this.nextFloorAni.PlayAnimation("Start", false);
			this.nextFloorAni.AddAnimation("Loop", true, 0f);
			DelayCall.Instance.CallOnce((int)(animationDuration * 1000f), delegate
			{
				this.RefreshInfo();
				this.RefreshGridMode();
				if (this.nextFloorAni)
				{
					this.nextFloorAni.PlayAnimation("End", false);
				}
			});
			DelayCall.Instance.CallOnce((int)((animationDuration + animationDuration2) * 1000f), delegate
			{
				if (this.nextFloorAni != null && this.nextFloorAni.gameObject != null)
				{
					this.nextFloorAni.gameObject.SetActiveSafe(false);
				}
				if (this.clickMask != null)
				{
					this.clickMask.SetActiveSafe(false);
				}
			});
		}

		private void OnEventMiningBomb(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsMiningBomb eventArgsMiningBomb = eventArgs as EventArgsMiningBomb;
			if (eventArgsMiningBomb != null && this.currentCtrl != null)
			{
				this.currentCtrl.OpenBomb(eventArgsMiningBomb.bombGrids);
			}
			this.RefreshInfo();
			this.RefreshGridMode();
		}

		private void OnEventAutoMining(object sender, int type, BaseEventArgs eventArgs)
		{
			this.StartAutoMining();
		}

		private void OnEventRefreshInfo(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshInfo();
		}

		private void OnTicketUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsTicketUpdate eventArgsTicketUpdate = eventArgs as EventArgsTicketUpdate;
			if (eventArgsTicketUpdate == null)
			{
				return;
			}
			if (eventArgsTicketUpdate.TicketKind == UserTicketKind.Mining)
			{
				this.RefreshInfo();
			}
		}

		private void OnEventShowClickMask(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsBool eventArgsBool = eventArgs as EventArgsBool;
			if (eventArgsBool != null)
			{
				this.clickMask.SetActiveSafe(eventArgsBool.Value);
			}
		}

		private void OnClickAutoMining()
		{
			if (GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsActivePrivilege(CardPrivilegeKind.AutoMining))
			{
				this.StartAutoMining();
				return;
			}
			GameApp.View.OpenView(ViewName.MiningBuyCardViewModule, null, 1, null, null);
		}

		private void OnClickStopAuto()
		{
			this.AutoEnd();
			if (this.autoMiningCtrl != null)
			{
				this.autoMiningCtrl.EndAutoMining();
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.MiningViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_CloseUI, null);
		}

		private void OnClickBonus()
		{
			GameApp.View.OpenView(ViewName.MiningBonusViewModule, null, 1, null, null);
		}

		private void OnClickAd()
		{
			int maxTimes = this.adDataModule.GetMaxTimes(12);
			if (this.adDataModule.GetWatchTimes(12) < maxTimes)
			{
				AdBridge.PlayRewardVideo(12, delegate(bool isSuccess)
				{
					if (isSuccess)
					{
						NetworkUtils.Mining.DoMiningAdGetItemRequest(delegate(bool result, MiningAdGetItemResponse resp)
						{
							if (result && resp != null && resp.CommonData != null && resp.CommonData.Reward != null)
							{
								if (resp.CommonData.Reward.Count > 0)
								{
									DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
								}
								GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(12), "REWARD ", "", resp.CommonData.Reward, null);
							}
							this.RefreshInfo();
						});
					}
				});
			}
		}

		private void StartAutoMining()
		{
			if (this.autoMiningCtrl != null)
			{
				this.isAutoMining = true;
				this.clickMask.SetActiveSafe(true);
				this.buttonAutoMining.gameObject.SetActiveSafe(false);
				this.buttonStopAuto.gameObject.SetActiveSafe(true);
				this.autoMiningCtrl.StartAutoMining(AutoMiningMode.Treasure);
			}
		}

		public async Task AutoMiningAni(List<GridDto> list)
		{
			this.RefreshInfo();
			if (this.currentCtrl != null)
			{
				bool flag = false;
				if (list.Count == 1 && list[0].Grade > 0)
				{
					UIMiningGridItem item = this.currentCtrl.GetGridItem(list[0]);
					if (item != null)
					{
						flag = true;
						this.PlayPickaxAnimation(item.transform, delegate
						{
							item.AutoMining(list[0]);
							this.RefreshGridMode();
						});
					}
				}
				if (!flag)
				{
					await this.currentCtrl.AutoMiningAni(list);
					this.RefreshGridMode();
				}
			}
		}

		public async Task AutoBombAni(int bombPos, List<GridDto> list)
		{
			if (this.currentCtrl != null)
			{
				await this.currentCtrl.AutoBombAni(bombPos, list);
			}
			this.RefreshInfo();
			this.RefreshGridMode();
		}

		public void AutoEnd()
		{
			this.isAutoMining = false;
			this.clickMask.SetActiveSafe(false);
			this.buttonAutoMining.gameObject.SetActiveSafe(true);
			this.buttonStopAuto.gameObject.SetActiveSafe(false);
		}

		private void OnRefreshRedPointChange(RedNodeListenData redData)
		{
			if (this.redNodeBonusCtrl == null)
			{
				return;
			}
			this.redNodeBonusCtrl.gameObject.SetActive(redData.m_count > 0);
		}

		[GameTestMethod("挖矿", "挖矿", "", 501)]
		private static void OpenMining()
		{
			GameApp.View.OpenView(ViewName.MiningViewModule, null, 1, null, null);
		}

		[Header("货币兰")]
		public ModuleCurrencyCtrl moduleCurrencyCtrl;

		public UIHelpButton helpButton;

		[Header("矿区")]
		public UISpineModelItem spinePickaxe;

		public GameObject clickMask;

		public List<UIMiningGridCtrl> miningGridCtrls;

		[Header("顶部信息")]
		public CustomText textCurrentFloor;

		public GameObject bigRewardObj;

		public CustomButton buttonAutoMining;

		public CustomButton buttonStopAuto;

		[Header("底部信息")]
		public GameObject refreshTimeObj;

		public CustomText textTime;

		public CustomText textTicket;

		public CustomImage imageTicket;

		public CustomButton buttonBack;

		public CustomButton buttonBonus;

		public RedNodeOneCtrl redNodeBonusCtrl;

		public GameObject ticketObj;

		public UIAdExchangeButton adButtonCtrl;

		[Header("过场动画")]
		public UISpineModelItem nextFloorAni;

		[Header("打开动画")]
		public RectTransform titleRT;

		public RectTransform centerRT;

		public RectTransform bottomRT;

		public RectTransform autoBtnRT;

		private MiningDataModule miningDataModule;

		private TicketDataModule ticketDataModule;

		private LoginDataModule loginDataModule;

		private AdDataModule adDataModule;

		private UIMiningGridCtrl currentCtrl;

		private AutoMiningController autoMiningCtrl;

		private bool isAutoMining;

		private SequencePool sequencePool = new SequencePool();
	}
}
