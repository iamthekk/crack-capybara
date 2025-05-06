using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Chapter;
using UnityEngine.Events;

namespace HotFix
{
	public class ChapterBattlePassViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterBattlePass_DataChange, new HandlerEvent(this.OnEventDataChange));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterBattlePass_NoData, new HandlerEvent(this.OnEventNoData));
			this.fundCtrl.Init();
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as ChapterBattlePassViewModule.OpenData;
			if (this.openData == null || this.dataModule.BattlePassDto == null)
			{
				return;
			}
			this.battlePassCfg = GameApp.Table.GetManager().GetChapterActivity_Battlepass(this.dataModule.BattlePassDto.ConfigId);
			if (this.battlePassCfg == null)
			{
				HLog.LogError(string.Format("[ChapterActivity_Battlepass] not found id={0}", this.dataModule.BattlePassDto.ConfigId));
				return;
			}
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.fundCtrl.SetButton(true, this.openData.isShowGotoBtn, new Action(this.OnClickClose), new Action(this.OnClickGoto), "chapter_battlepass_goto");
			this.fundCtrl.SetPurchaseButton(true, this.battlePassCfg.purchaseId, new Action<bool>(this.OnBuySuccess), null, null);
			this.RefreshStyle();
			this.Refresh();
			int jumpIndex = this.dataModule.GetJumpIndex();
			this.fundCtrl.SetJumpItem(jumpIndex, 50f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.dataModule == null || this.dataModule.BattlePassDto == null)
			{
				return;
			}
			if (DxxTools.Time.ServerTimestamp > this.dataModule.BattlePassDto.EndTime)
			{
				this.fundCtrl.SetTime(Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_end"));
				return;
			}
			this.fundCtrl.SetTime(Singleton<LanguageManager>.Instance.GetTime(this.dataModule.BattlePassDto.EndTime - DxxTools.Time.ServerTimestamp));
		}

		public override void OnClose()
		{
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
		}

		public override void OnDelete()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterBattlePass_DataChange, new HandlerEvent(this.OnEventDataChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterBattlePass_NoData, new HandlerEvent(this.OnEventNoData));
			this.fundCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void Refresh()
		{
			this.fundCtrl.SetData(this.dataModule.uiDataList, this.battlePassCfg.atlasID, this.battlePassCfg.itemIcon, "ui_chapter_activity_tip", new Action<int, int>(this.OnGetReward), new Action(this.OnGetLoopReward));
			this.RefreshStatus();
		}

		private Task RefreshStyle()
		{
			ChapterBattlePassViewModule.<RefreshStyle>d__14 <RefreshStyle>d__;
			<RefreshStyle>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshStyle>d__.<>4__this = this;
			<RefreshStyle>d__.<>1__state = -1;
			<RefreshStyle>d__.<>t__builder.Start<ChapterBattlePassViewModule.<RefreshStyle>d__14>(ref <RefreshStyle>d__);
			return <RefreshStyle>d__.<>t__builder.Task;
		}

		private void RefreshStatus()
		{
			if (this.dataModule.BattlePassDto != null)
			{
				this.fundCtrl.SetStatus(this.dataModule.BattlePassDto.Score, this.dataModule.BattlePassDto.FreeRewardList.ToList<int>(), this.dataModule.BattlePassDto.PayRewardList.ToList<int>(), this.dataModule.BattlePassDto.FinalRewardCount, this.dataModule.BattlePassDto.Buy > 0);
				this.fundCtrl.Refresh();
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.ChapterBattlePassViewModule, null);
		}

		private void OnClickGoto()
		{
			this.OnClickClose();
			int previousChapterID = GameApp.Data.GetDataModule(DataName.ChapterDataModule).GetPreviousChapterID();
			if (previousChapterID <= 0)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("ui_sweep_tips_lock");
				EventArgsString eventArgsString = new EventArgsString();
				eventArgsString.SetData(infoByID);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, eventArgsString);
				return;
			}
			ChapterSweepViewModule.OpenData openData = new ChapterSweepViewModule.OpenData();
			openData.chapterId = previousChapterID;
			openData.isRecord = false;
			GameApp.View.OpenView(ViewName.ChapterSweepViewModule, openData, 1, null, null);
		}

		private void OnGetReward(int id, int isPay)
		{
			List<int> list = new List<int> { id };
			List<int> preFreeReward = new List<int>();
			if (this.dataModule.BattlePassDto.FreeRewardList != null)
			{
				foreach (int num in this.dataModule.BattlePassDto.FreeRewardList)
				{
					if (!preFreeReward.Contains(num))
					{
						preFreeReward.Add(num);
					}
				}
			}
			List<int> prePayReward = new List<int>();
			if (this.dataModule.BattlePassDto.PayRewardList != null)
			{
				foreach (int num2 in this.dataModule.BattlePassDto.PayRewardList)
				{
					if (!prePayReward.Contains(num2))
					{
						prePayReward.Add(num2);
					}
				}
			}
			NetworkUtils.Chapter.DoGetChapterBattlePassRewardRequest(list, delegate(bool isOk, GetChapterBattlePassRewardResponse resp)
			{
				if (isOk)
				{
					DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
					this.RefreshStatus();
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(2, this.battlePassCfg.name);
					int level = this.dataModule.Level;
					List<int> list2 = new List<int>();
					if (this.dataModule.BattlePassDto.FreeRewardList != null)
					{
						foreach (int num3 in this.dataModule.BattlePassDto.FreeRewardList)
						{
							if (!list2.Contains(num3))
							{
								list2.Add(num3);
							}
						}
					}
					foreach (int num4 in preFreeReward)
					{
						if (list2.Contains(num4))
						{
							list2.Remove(num4);
						}
					}
					List<int> list3 = new List<int>();
					if (this.dataModule.BattlePassDto.PayRewardList != null)
					{
						foreach (int num5 in this.dataModule.BattlePassDto.PayRewardList)
						{
							if (!list3.Contains(num5))
							{
								list3.Add(num5);
							}
						}
					}
					foreach (int num6 in prePayReward)
					{
						if (list3.Contains(num6))
						{
							list3.Remove(num6);
						}
					}
					List<int> list4 = new List<int>();
					List<int> list5 = new List<int>();
					for (int i = 0; i < this.dataModule.uiDataList.Count; i++)
					{
						CommonFundUIData commonFundUIData = this.dataModule.uiDataList[i];
						if (list2.Contains(commonFundUIData.ConfigId))
						{
							list4.Add(i + 1);
						}
						if (list3.Contains(commonFundUIData.ConfigId))
						{
							list5.Add(i + 1);
						}
					}
					GameApp.SDK.Analyze.Track_BattlePassGet_ChapterActivity(infoByID, level, resp.CommonData.Reward, this.battlePassCfg.purchaseId, list4, list5, null);
				}
			});
		}

		private void OnGetLoopReward()
		{
			if (this.dataModule.CanOpenFinalReward(true))
			{
				int preFinalRewardCount = this.dataModule.BattlePassDto.FinalRewardCount;
				NetworkUtils.Chapter.DoChapterBattlePassOpenBoxRequest(delegate(bool isOk, ChapterBattlePassOpenBoxResponse resp)
				{
					if (isOk)
					{
						this.RefreshStatus();
						BoxUpgradeViewModule.OpenData openData = new BoxUpgradeViewModule.OpenData();
						openData.initGrade = resp.InitGrade;
						openData.upgradeLimit = resp.UpgradeInfo.Count;
						openData.upgradeProgress = resp.UpgradeInfo.ToList<int>();
						openData.showRewards = resp.CommonData.Reward;
						GameApp.View.OpenView(ViewName.BoxUpgradeViewModule, openData, 1, null, null);
						string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(2, this.battlePassCfg.name);
						int level = this.dataModule.Level;
						int num = this.dataModule.BattlePassDto.FinalRewardCount;
						num -= preFinalRewardCount;
						int configId = this.dataModule.uiDataList.Find((CommonFundUIData a) => a.IsLoopReward).ConfigId;
						List<int> list = new List<int>();
						for (int i = 0; i < num; i++)
						{
							list.Add(configId);
						}
						GameApp.SDK.Analyze.Track_BattlePassGet_ChapterActivity(infoByID, level, resp.CommonData.Reward, this.battlePassCfg.purchaseId, null, null, list);
					}
				});
			}
		}

		private void OnEventDataChange(object sender, int type, BaseEventArgs eventargs)
		{
			this.Refresh();
		}

		private void OnEventNoData(object sender, int type, BaseEventArgs eventargs)
		{
			if (GameApp.View.IsOpened(ViewName.ChapterBattlePassViewModule))
			{
				this.OnClickClose();
			}
		}

		private void OnBuySuccess(bool obj)
		{
			this.Refresh();
		}

		[GameTestMethod("章节通行证", "增加活动积分", "", 411)]
		private static void AddChapterActivityScore()
		{
			ChapterBattlePassDataModule chapterBattlePassDataModule = GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule);
			if (chapterBattlePassDataModule.BattlePassDto != null)
			{
				chapterBattlePassDataModule.UpdateScore(chapterBattlePassDataModule.BattlePassDto.Score + 300, chapterBattlePassDataModule.BattlePassDto.RowId);
			}
		}

		public CustomButton buttonMask;

		public CommonFundCtrl fundCtrl;

		private ChapterBattlePassViewModule.OpenData openData;

		private ChapterBattlePassDataModule dataModule;

		private ChapterActivity_Battlepass battlePassCfg;

		public class OpenData
		{
			public int chapterId;

			public bool isShowGotoBtn;
		}
	}
}
