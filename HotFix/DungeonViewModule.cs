using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Common;
using Proto.Dungeon;
using Shop.Arena;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class DungeonViewModule : BaseViewModule
	{
		private uint currentDifficult
		{
			get
			{
				return this.dungeonDataModule.GetCurrentLevel(this.dungeonBase.id);
			}
		}

		private uint finishDifficult
		{
			get
			{
				return this.dungeonDataModule.GetFinishLevel(this.dungeonBase.id);
			}
		}

		public override void OnCreate(object data)
		{
			this.dungeonDataModule = GameApp.Data.GetDataModule(DataName.DungeonDataModule);
			this.ticketDataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			this.adDataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.tempBg.SetActiveSafe(false);
			this.adButtonCtrl.Init();
			this.singleRowRewardCtrl.Init();
			this.multipleRowRewardCtrl.Init();
			for (int i = 0; i < this.attrItemList.Count; i++)
			{
				this.attrItemList[i].Init();
			}
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			if (!(data is DungeonID))
			{
				return;
			}
			DungeonID dungeonID = (DungeonID)data;
			this.dungeonBase = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById((int)dungeonID);
			if (dungeonID == DungeonID.SwordIsland)
			{
				this.rootRT.sizeDelta = new Vector2(this.rootRT.sizeDelta.x, 1570f);
			}
			else
			{
				this.rootRT.sizeDelta = new Vector2(this.rootRT.sizeDelta.x, 1380f);
			}
			this.dungeonLevels = this.dungeonDataModule.GetAllLevel(this.dungeonBase.id);
			this.selectDifficult = this.dungeonDataModule.GetCurrentLevel(this.dungeonBase.id);
			this.maxDifficult = this.dungeonLevels[this.dungeonLevels.Count - 1].level;
			this.ticketKind = this.dungeonDataModule.GetTicketKind(this.dungeonBase.id);
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonPrevious.onClick.AddListener(new UnityAction(this.OnPrevious));
			this.buttonNext.onClick.AddListener(new UnityAction(this.OnNext));
			this.buttonSweep.onClick.AddListener(new UnityAction(this.OnSweep));
			this.buttonFight.onClick.AddListener(new UnityAction(this.OnFight));
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.dungeonBase.name);
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.dungeonBase.keyID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("未找到表id {0}", this.dungeonBase.keyID));
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(elementById.atlasID);
			this.imageTicket.SetImage(atlasPath, elementById.icon);
			this.RefreshInfo();
			this.LoadBg();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonPrevious.onClick.RemoveListener(new UnityAction(this.OnPrevious));
			this.buttonNext.onClick.RemoveListener(new UnityAction(this.OnNext));
			this.buttonSweep.onClick.RemoveListener(new UnityAction(this.OnSweep));
			this.buttonFight.onClick.RemoveListener(new UnityAction(this.OnFight));
			if (this.dungeonLevels != null)
			{
				this.dungeonLevels.Clear();
			}
		}

		public override void OnDelete()
		{
			this.adButtonCtrl.DeInit();
			this.singleRowRewardCtrl.DeInit();
			this.multipleRowRewardCtrl.DeInit();
			for (int i = 0; i < this.attrItemList.Count; i++)
			{
				this.attrItemList[i].DeInit();
			}
			this.bgDic.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private async Task LoadBg()
		{
			if (this.dungeonBase != null)
			{
				if (this.bgDic.ContainsKey(this.dungeonBase.id))
				{
					using (Dictionary<int, GameObject>.KeyCollection.Enumerator enumerator = this.bgDic.Keys.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							int num = enumerator.Current;
							if (num == this.dungeonBase.id)
							{
								this.bgDic[num].SetActiveSafe(true);
							}
							else
							{
								this.bgDic[num].SetActiveSafe(false);
							}
						}
						return;
					}
				}
				foreach (int num2 in this.bgDic.Keys)
				{
					this.bgDic[num2].SetActiveSafe(false);
				}
				string bgPath = DungeonViewModule.GetBgPath((DungeonID)this.dungeonBase.id);
				if (!string.IsNullOrEmpty(bgPath))
				{
					AsyncOperationHandle<GameObject> handle = GameApp.Resources.LoadAssetAsync<GameObject>(bgPath);
					await handle.Task;
					if (handle.Result != null)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(handle.Result);
						gameObject.SetParentNormal(this.bgParent, false);
						this.bgDic.Add(this.dungeonBase.id, gameObject);
					}
					handle = default(AsyncOperationHandle<GameObject>);
				}
			}
		}

		private void RefreshInfo()
		{
			UserTicket ticket = this.ticketDataModule.GetTicket(this.ticketKind);
			uint num = 0U;
			if (ticket != null)
			{
				this.textTicket.text = string.Format("{0}/{1}", ticket.NewNum, ticket.RevertLimit);
				num = ticket.NewNum;
			}
			else
			{
				this.textTicket.text = string.Format("0/{0}", this.dungeonBase.keyLimit);
			}
			int maxTimes = this.adDataModule.GetMaxTimes(this.dungeonBase.adId);
			int watchTimes = this.adDataModule.GetWatchTimes(this.dungeonBase.adId);
			if (num == 0U && watchTimes < maxTimes)
			{
				this.buttonsObj.SetActiveSafe(false);
				this.adButtonCtrl.gameObject.SetActiveSafe(true);
				this.adButtonCtrl.SetData(this.dungeonBase.keyID, 1, watchTimes, maxTimes, new Action(this.OnClickAd));
			}
			else
			{
				this.buttonsObj.SetActiveSafe(true);
				this.adButtonCtrl.gameObject.SetActiveSafe(false);
			}
			this.textDifficult.text = this.selectDifficult.ToString();
			if (this.dungeonBase.id == 3)
			{
				this.singleRowRewardCtrl.gameObject.SetActiveSafe(false);
				this.multipleRowRewardCtrl.gameObject.SetActiveSafe(true);
				Dungeon_DungeonLevel dungeonLevel = this.dungeonDataModule.GetDungeonLevel(this.dungeonBase.id, (int)this.selectDifficult);
				if (dungeonLevel != null)
				{
					this.multipleRowRewardCtrl.SetData(dungeonLevel.showRate, dungeonLevel.dropTimes);
				}
			}
			else
			{
				this.singleRowRewardCtrl.gameObject.SetActiveSafe(true);
				this.multipleRowRewardCtrl.gameObject.SetActiveSafe(false);
				List<ItemData> dungeonLevelRewards = this.dungeonDataModule.GetDungeonLevelRewards(this.dungeonBase.id, this.selectDifficult);
				this.singleRowRewardCtrl.SetData(dungeonLevelRewards);
			}
			this.buttonPrevious.gameObject.SetActiveSafe((ulong)this.selectDifficult > (ulong)((long)this.dungeonLevels[0].level));
			this.buttonNext.gameObject.SetActiveSafe(this.selectDifficult < this.currentDifficult);
			this.buttonSweep.gameObject.SetActiveSafe(this.currentDifficult > 1U);
			if (this.selectDifficult == this.currentDifficult && (ulong)this.finishDifficult != (ulong)((long)this.maxDifficult))
			{
				this.textSweep.text = Singleton<LanguageManager>.Instance.GetInfoByID("uidungeon_sweep_previous");
			}
			else
			{
				this.textSweep.text = Singleton<LanguageManager>.Instance.GetInfoByID("uidungeon_sweep");
			}
			Dungeon_DungeonLevel dungeonLevel2 = this.dungeonDataModule.GetDungeonLevel(this.dungeonBase.id, (int)this.selectDifficult);
			for (int i = 0; i < this.attrItemList.Count; i++)
			{
				if (i < dungeonLevel2.attrTips.Length)
				{
					this.attrItemList[i].SetData(dungeonLevel2.attrTips[i]);
					this.attrItemList[i].SetActive(true);
				}
				else
				{
					this.attrItemList[i].SetActive(false);
				}
			}
		}

		private void OnClickClose()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_DailyActivities_RefreshUI, null);
			GameApp.View.CloseView(ViewName.DungeonViewModule, null);
		}

		private void OnPrevious()
		{
			if ((ulong)this.selectDifficult > (ulong)((long)this.dungeonLevels[0].level))
			{
				this.selectDifficult -= 1U;
				this.RefreshInfo();
			}
		}

		private void OnNext()
		{
			if (this.selectDifficult < this.currentDifficult)
			{
				this.selectDifficult += 1U;
				this.RefreshInfo();
			}
		}

		private void OnSweep()
		{
			if (this.isFight)
			{
				return;
			}
			this.isFight = true;
			UserTicket ticket = this.ticketDataModule.GetTicket(this.ticketKind);
			if (ticket != null && ticket.NewNum > 0U)
			{
				uint num = this.selectDifficult;
				if (this.selectDifficult == this.currentDifficult && (ulong)this.finishDifficult != (ulong)((long)this.maxDifficult))
				{
					num = this.currentDifficult - 1U;
				}
				this.SendMsg(this.dungeonBase.id, (int)num, true);
				return;
			}
			this.isFight = false;
			this.ShowNotEnoughTip();
		}

		private void OnFight()
		{
			if (this.isFight)
			{
				return;
			}
			this.isFight = true;
			UserTicket ticket = this.ticketDataModule.GetTicket(this.ticketKind);
			if (ticket != null && ticket.NewNum > 0U)
			{
				this.SendMsg(this.dungeonBase.id, (int)this.selectDifficult, false);
				return;
			}
			this.isFight = false;
			this.ShowNotEnoughTip();
		}

		private void OnClickAd()
		{
			int maxTimes = this.adDataModule.GetMaxTimes(this.dungeonBase.adId);
			if (this.adDataModule.GetWatchTimes(this.dungeonBase.adId) < maxTimes)
			{
				AdBridge.PlayRewardVideo(this.dungeonBase.adId, delegate(bool isSuccess)
				{
					if (isSuccess)
					{
						NetworkUtils.Dungeon.DoDungeonAdGetItemRequest(this.dungeonBase.id, delegate(bool result, DungeonAdGetItemResponse resp)
						{
							if (result && resp != null && resp.CommonData != null && resp.CommonData.Reward != null && resp.CommonData.Reward.Count > 0)
							{
								DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
							}
							if (result)
							{
								GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(this.dungeonBase.adId), "REWARD ", "", resp.CommonData.Reward, null);
							}
							this.RefreshInfo();
						});
					}
				});
			}
		}

		private void SendMsg(int dungeonId, int levelId, bool isSweep)
		{
			NetworkUtils.Dungeon.DoStartDungeonRequest(dungeonId, levelId, isSweep, delegate(bool result, StartDungeonResponse response)
			{
				if (result && response != null)
				{
					if (isSweep)
					{
						if (response.CommonData.Reward != null && response.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(response.CommonData.Reward, null, true);
						}
						this.RefreshInfo();
						this.isFight = false;
						return;
					}
					this.DoBattle();
				}
			});
		}

		private void ShowNotEnoughTip()
		{
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uidungeon_tip");
			GameApp.View.ShowStringTip(infoByID);
		}

		private void DoBattle()
		{
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
					instance.SetData(DxxTools.UI.GetDungeonOpenData((DungeonID)this.dungeonBase.id));
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
					GameApp.View.CloseView(ViewName.DungeonViewModule, null);
					EventArgsGameDataEnter instance2 = Singleton<EventArgsGameDataEnter>.Instance;
					instance2.SetData(GameModel.Dungeon, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance2);
					GameApp.State.ActiveState(StateName.BattleDungeonState);
					this.isFight = false;
				});
			});
		}

		public static string GetBgPath(DungeonID id)
		{
			string text = string.Empty;
			switch (id)
			{
			case DungeonID.DragonsLair:
				text = string.Format("Assets/_Resources/Prefab/UI/Dungeon/{0}.prefab", "DragonsLairBG");
				break;
			case DungeonID.AstralTree:
				text = string.Format("Assets/_Resources/Prefab/UI/Dungeon/{0}.prefab", "AstralTreeBG");
				break;
			case DungeonID.SwordIsland:
				text = string.Format("Assets/_Resources/Prefab/UI/Dungeon/{0}.prefab", "SwordIslandBG");
				break;
			case DungeonID.DeepSeaRuins:
				text = string.Format("Assets/_Resources/Prefab/UI/Dungeon/{0}.prefab", "DeepSeaRuinsBG");
				break;
			}
			return text;
		}

		public RectTransform rootRT;

		public GameObject tempBg;

		public CustomText textTitle;

		public CustomButton buttonClose;

		public CustomButton buttonMask;

		public CustomButton buttonPrevious;

		public CustomButton buttonNext;

		public CustomText textDifficult;

		public CustomImage imageTicket;

		public CustomText textTicket;

		public CustomButton buttonSweep;

		public CustomButton buttonFight;

		public CustomText textSweep;

		public GameObject buttonsObj;

		public UIAdExchangeButton adButtonCtrl;

		public GameObject bgParent;

		public List<UIDungeonAttrTip> attrItemList;

		public UISingleRowRewardCtrl singleRowRewardCtrl;

		public UIMultipleRowRewardCtrl multipleRowRewardCtrl;

		private DungeonDataModule dungeonDataModule;

		private TicketDataModule ticketDataModule;

		private AdDataModule adDataModule;

		private Dungeon_DungeonBase dungeonBase;

		private List<Dungeon_DungeonLevel> dungeonLevels;

		private UserTicketKind ticketKind;

		private uint selectDifficult;

		private int maxDifficult;

		private const string Path = "Assets/_Resources/Prefab/UI/Dungeon/{0}.prefab";

		private bool isFight;

		private Dictionary<int, GameObject> bgDic = new Dictionary<int, GameObject>();
	}
}
