using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;
using Proto.Chapter;
using Server;

namespace HotFix
{
	public abstract class GameEventBase
	{
		public bool isDone { get; protected set; }

		public bool IsEnd
		{
			get
			{
				return this.isEnd;
			}
		}

		public virtual void Init(GameEventData data, int stage, Action endAction, GameEventGroup group)
		{
			this.currentData = data;
			this.stage = stage;
			this.endAction = endAction;
			this.group = group;
			this.isDone = false;
			this.OnInit();
		}

		public virtual void DeInit()
		{
			this.buttonDatas.Clear();
			this.OnDeInit();
		}

		public abstract void OnInit();

		public abstract void OnDeInit();

		public abstract Task OnEvent(GameEventPushType pushType, object param);

		protected abstract void ShowUI();

		public async void DoEvent(GameEventPushType pushType, object param)
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			GameTGATools.Ins.StageClickTempEventSizeType = this.currentData.poolData.eventSizeType;
			GameTGATools.Ins.StageClickTempEventID = this.currentData.poolData.tableId;
			GameTGATools.Ins.StageClickTempDay = Singleton<GameEventController>.Instance.GetCurrentStage();
			GameTGATools.Ins.StageClickTempExp = playerData.Exp.mVariable;
			GameTGATools.Ins.StageClickTempLevel = playerData.ExpLevel.mVariable;
			GameTGATools.Ins.StageClickTempATK = GameTGATools.NumberOffset(playerData.Attack.GetValue(), 10);
			GameTGATools.Ins.StageClickTempDEF = GameTGATools.NumberOffset(playerData.Defence.GetValue(), 10);
			GameTGATools.Ins.StageClickTempHP = GameTGATools.NumberOffset(playerData.CurrentHp.GetValue(), 10);
			GameTGATools.Ins.StageClickTempHP_MAX = GameTGATools.NumberOffset(playerData.HpMax.GetValue(), 10);
			GameTGATools.Ins.SetStageClickTempSelectedSkillList(playerData.GetPlayerSkillBuildList());
			if (dataModule.CurrentChapter.BigBonus > 0)
			{
				int num = Singleton<GameEventController>.Instance.GetEventSizeTypeNum(EventSizeType.BigWin) - Singleton<GameEventController>.Instance.PlayerData.PlayBigBonusCount * dataModule.CurrentChapter.BigBonus;
				num = ((num < 0) ? 0 : num);
				GameTGATools.Ins.StageClickTempEpicProgress = num;
			}
			if (dataModule.CurrentChapter.MinorBonus > 0)
			{
				int num2 = Singleton<GameEventController>.Instance.GetEventSizeTypeNum(EventSizeType.MinorWin) - Singleton<GameEventController>.Instance.PlayerData.PlayMinorBonusCount * dataModule.CurrentChapter.MinorBonus;
				num2 = ((num2 < 0) ? 0 : num2);
				GameTGATools.Ins.StageClickTempNormalProgress = num2;
			}
			await this.OnEvent(pushType, param);
			if (pushType != GameEventPushType.ClickButton)
			{
				if (pushType != GameEventPushType.ClickScroll)
				{
					if (pushType == GameEventPushType.UIAniFinish)
					{
						GameEventData next = this.currentData.GetNext(0);
						if (this.currentData.poolData.IsActivity && next == null)
						{
							long num3 = 0L;
							long num4 = 0L;
							List<ulong> list = new List<ulong>();
							ChapterActivityDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
							ChapterBattlePassDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule);
							ChapterActivityWheelDataModule dataModule4 = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
							for (int i = 0; i < this.currentData.poolData.actRowIdArr.Length; i++)
							{
								ulong num5 = this.currentData.poolData.actRowIdArr[i];
								ChapterActivityData activityData = dataModule2.GetActivityData(num5);
								if (activityData != null && activityData.IsInProgress())
								{
									list.Add(activityData.RowId);
								}
								if (dataModule3.IsInProgress() && num5 == (ulong)dataModule3.BattlePassDto.RowId)
								{
									num3 = (long)num5;
								}
								if (dataModule4.IsActivityOpen((long)num5))
								{
									num4 = (long)num5;
								}
							}
							if (list.Count > 0)
							{
								this.HangUp();
								NetworkUtils.Chapter.DoChapterActRewardRequest(this.stage, list, delegate(bool result, ChapterActRewardResponse resp)
								{
									if (!result && resp != null && resp.Code > -100 && Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Sweep)
									{
										this.ResumeHangUp();
									}
									this.CheckFinish();
								});
							}
							if (num3 > 0L)
							{
								NetworkUtils.Chapter.DoChapterBattlePassScoreRequest(this.stage, num3, null);
							}
							if (num4 > 0L)
							{
								NetworkUtils.Chapter.DoChapterWheelScoreRequest(this.stage, num4, null);
							}
							if ((num3 > 0L || num4 > 0L) && list.Count <= 0)
							{
								this.CheckFinish();
							}
						}
						else
						{
							this.CheckFinish();
						}
					}
				}
				else if (this.isDone)
				{
					if (!this.isMoveToNpc)
					{
						if (!this.OnClickScroll())
						{
							GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShakeButton, null);
						}
					}
				}
			}
			else if (this.isDone)
			{
				if (!this.isMoveToNpc)
				{
					await this.OnClickButton((int)param);
				}
			}
		}

		protected virtual void CheckFinish()
		{
		}

		public void SetNext()
		{
			this.group.SetCurrentData(this.nextData);
			this.End();
		}

		protected void End()
		{
			this.isEnd = true;
			Action action = this.endAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		protected virtual Task OnClickButton(int buttonIndex)
		{
			this.isDone = true;
			this.nextData = this.currentData.GetNext(buttonIndex);
			this.SetNext();
			return Task.CompletedTask;
		}

		protected bool OnClickScroll()
		{
			if (this.currentData.IsShowButton())
			{
				return false;
			}
			this.OnClickButton(0);
			return true;
		}

		public virtual void HangUp()
		{
			this.isHangUp = true;
		}

		public virtual void ResumeHangUp()
		{
			this.isHangUp = false;
		}

		public List<GameEventDataSelect> GetButtonDatas()
		{
			return this.buttonDatas;
		}

		public List<int> GetPositionMonsterIds(int tableId)
		{
			List<int> list = new List<int>();
			MonsterCfg_monsterCfg monsterCfg_monsterCfg = GameApp.Table.GetManager().GetMonsterCfg_monsterCfg(tableId);
			if (monsterCfg_monsterCfg == null)
			{
				return list;
			}
			if (monsterCfg_monsterCfg.pos1 > 0)
			{
				list.Add(monsterCfg_monsterCfg.pos1);
			}
			if (monsterCfg_monsterCfg.pos2 > 0)
			{
				list.Add(monsterCfg_monsterCfg.pos2);
			}
			if (monsterCfg_monsterCfg.pos3 > 0)
			{
				list.Add(monsterCfg_monsterCfg.pos3);
			}
			if (monsterCfg_monsterCfg.pos4 > 0)
			{
				list.Add(monsterCfg_monsterCfg.pos4);
			}
			if (monsterCfg_monsterCfg.pos5 > 0)
			{
				list.Add(monsterCfg_monsterCfg.pos5);
			}
			return list;
		}

		protected int RandomDrop(XRandom xRandom, int[] arr)
		{
			int num = 0;
			int num2 = 0;
			if (arr != null)
			{
				if (arr.Length != 0)
				{
					num = arr[0];
				}
				if (arr.Length > 1)
				{
					num2 = arr[1];
				}
				else
				{
					num2 = num;
				}
			}
			return xRandom.Range(num, num2 + 1);
		}

		protected void MarkDone()
		{
			this.isDone = true;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_UIShowButton, null);
		}

		protected int stage;

		protected GameEventData currentData;

		protected GameEventData nextData;

		protected Action endAction;

		protected GameEventGroup group;

		private bool isEnd;

		protected bool isHangUp;

		protected List<GameEventDataSelect> buttonDatas = new List<GameEventDataSelect>();

		protected bool isMoveToNpc;
	}
}
