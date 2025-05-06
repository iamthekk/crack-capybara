using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class ChapterBattlePassDataModule : IDataModule
	{
		public ChapterBattlePassDto BattlePassDto { get; private set; }

		public ChapterActivity_Battlepass BattlePassConfig { get; private set; }

		public List<CommonFundUIData> uiDataList { get; private set; } = new List<CommonFundUIData>();

		public int Level { get; private set; }

		public int GetName()
		{
			return 164;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		public void UpdateData(ChapterBattlePassDto dto)
		{
			if (dto == null)
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterBattlePass_NoData, null);
				return;
			}
			ChapterBattlePassDto battlePassDto = this.BattlePassDto;
			int num = ((battlePassDto != null) ? battlePassDto.ConfigId : 0);
			this.BattlePassDto = dto;
			if (num != dto.ConfigId)
			{
				this.uiDataList.Clear();
				this.BattlePassConfig = GameApp.Table.GetManager().GetChapterActivity_Battlepass(dto.ConfigId);
				if (this.BattlePassConfig == null)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterBattlePass_NoData, null);
					return;
				}
				IList<ChapterActivity_BattlepassReward> allElements = GameApp.Table.GetManager().GetChapterActivity_BattlepassRewardModelInstance().GetAllElements();
				for (int i = 0; i < allElements.Count; i++)
				{
					ChapterActivity_BattlepassReward chapterActivity_BattlepassReward = allElements[i];
					if (chapterActivity_BattlepassReward.group == this.BattlePassConfig.group)
					{
						CommonFundUIData commonFundUIData = new CommonFundUIData();
						commonFundUIData.ConfigId = chapterActivity_BattlepassReward.id;
						commonFundUIData.Score = chapterActivity_BattlepassReward.score;
						commonFundUIData.IsLoopReward = chapterActivity_BattlepassReward.type == 1;
						commonFundUIData.LoopRewardLimit = this.BattlePassConfig.finalRewardLimit;
						commonFundUIData.FreeRewards = chapterActivity_BattlepassReward.freeReward.ToItemDataList();
						commonFundUIData.PayRewards = chapterActivity_BattlepassReward.payReward.ToItemDataList();
						this.uiDataList.Add(commonFundUIData);
					}
				}
				this.uiDataList.Sort(new Comparison<CommonFundUIData>(CommonFundUIData.Sort));
				for (int j = 0; j < this.uiDataList.Count; j++)
				{
					CommonFundUIData commonFundUIData2 = this.uiDataList[j];
					if (j > 0)
					{
						commonFundUIData2.PreviousScore = this.uiDataList[j - 1].Score;
					}
					else
					{
						commonFundUIData2.PreviousScore = 0;
					}
					if (j + 1 >= this.uiDataList.Count)
					{
						commonFundUIData2.NextScore = 0;
					}
					else
					{
						commonFundUIData2.NextScore = this.uiDataList[j + 1].Score;
					}
				}
				this.RefreshLevel();
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterBattlePass_DataChange, null);
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterBattlePass_RefreshData, null);
			RedPointController.Instance.ReCalc("Main.ChapterBattlePass", true);
		}

		public void UpdateScore(int score, long rowId)
		{
			if (this.BattlePassDto != null && this.BattlePassDto.RowId == rowId)
			{
				int score2 = this.BattlePassDto.Score;
				this.BattlePassDto.Score = score;
				this.RefreshLevel();
				EventArgsInt eventArgsInt = new EventArgsInt();
				eventArgsInt.SetData(score2);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterBattlePass_RefreshScore, eventArgsInt);
				RedPointController.Instance.ReCalc("Main.ChapterBattlePass", true);
			}
		}

		public void UpdateFinalRewardCount(int count)
		{
			if (this.BattlePassDto != null)
			{
				this.BattlePassDto.FinalRewardCount = count;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterBattlePass_RefreshData, null);
				RedPointController.Instance.ReCalc("Main.ChapterBattlePass", true);
			}
		}

		private void RefreshLevel()
		{
			if (this.BattlePassDto == null)
			{
				return;
			}
			int score = this.BattlePassDto.Score;
			for (int i = 0; i < this.uiDataList.Count; i++)
			{
				if (score < this.uiDataList[i].Score)
				{
					this.Level = i;
					return;
				}
				if (this.uiDataList[i].IsLoopReward)
				{
					this.Level = i;
					return;
				}
			}
		}

		public bool IsFunctionOpen()
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity, false);
		}

		public bool IsStart()
		{
			return this.BattlePassDto != null && this.BattlePassDto.StartTime <= DxxTools.Time.ServerTimestamp;
		}

		public bool IsEnd()
		{
			return this.BattlePassDto == null || DxxTools.Time.ServerTimestamp >= this.BattlePassDto.EndTime;
		}

		public bool IsInProgress()
		{
			return this.IsFunctionOpen() && this.IsStart() && !this.IsEnd();
		}

		public int GetFinalRewardLimit()
		{
			if (this.BattlePassDto == null)
			{
				return 0;
			}
			ChapterActivity_Battlepass chapterActivity_Battlepass = GameApp.Table.GetManager().GetChapterActivity_Battlepass(this.BattlePassDto.ConfigId);
			if (chapterActivity_Battlepass != null)
			{
				return chapterActivity_Battlepass.finalRewardLimit;
			}
			return 0;
		}

		public bool CanOpenFinalReward(bool isShowTip = true)
		{
			int finalRewardLimit = this.GetFinalRewardLimit();
			if (this.BattlePassDto == null || this.BattlePassDto.Buy <= 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("common_fund_no_buy"));
				return false;
			}
			if (this.BattlePassDto.FinalRewardCount == finalRewardLimit)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("common_fund_all_get"));
				return false;
			}
			int loopStartScore = CommonFundTools.GetLoopStartScore(this.uiDataList);
			CommonFundUIData finalLoopData = CommonFundTools.GetFinalLoopData(this.uiDataList);
			if (finalLoopData == null && this.BattlePassDto.Score <= loopStartScore)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("common_fund_no_score"));
				return false;
			}
			if (this.BattlePassDto.Score - loopStartScore - this.BattlePassDto.FinalRewardCount * finalLoopData.Score < finalLoopData.Score)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("common_fund_no_score"));
				return false;
			}
			return true;
		}

		public bool IsRedPoint()
		{
			if (this.BattlePassDto == null)
			{
				return false;
			}
			for (int i = 0; i < this.uiDataList.Count; i++)
			{
				CommonFundUIData commonFundUIData = this.uiDataList[i];
				if (!commonFundUIData.IsLoopReward && this.BattlePassDto.Score >= commonFundUIData.Score)
				{
					if (!this.BattlePassDto.FreeRewardList.Contains(commonFundUIData.ConfigId))
					{
						return true;
					}
					if (this.BattlePassDto.Buy > 0 && !this.BattlePassDto.PayRewardList.Contains(commonFundUIData.ConfigId))
					{
						return true;
					}
				}
			}
			int loopStartScore = CommonFundTools.GetLoopStartScore(this.uiDataList);
			CommonFundUIData finalLoopData = CommonFundTools.GetFinalLoopData(this.uiDataList);
			if (finalLoopData != null)
			{
				int num = 0;
				if (this.BattlePassDto.Score >= loopStartScore)
				{
					num = this.BattlePassDto.Score - loopStartScore - finalLoopData.Score * this.BattlePassDto.FinalRewardCount;
				}
				return this.BattlePassDto.Buy > 0 && this.BattlePassDto.FinalRewardCount < finalLoopData.LoopRewardLimit && num >= finalLoopData.Score;
			}
			return false;
		}

		public int GetJumpIndex()
		{
			if (this.BattlePassDto != null)
			{
				for (int i = 0; i < this.uiDataList.Count; i++)
				{
					CommonFundUIData commonFundUIData = this.uiDataList[i];
					bool flag = this.BattlePassDto.FreeRewardList.Contains(commonFundUIData.ConfigId);
					bool flag2 = this.BattlePassDto.PayRewardList.Contains(commonFundUIData.ConfigId);
					if (this.BattlePassDto.Buy > 0)
					{
						if (!flag || !flag2)
						{
							return i;
						}
					}
					else if (!flag)
					{
						return i;
					}
				}
			}
			return 0;
		}
	}
}
