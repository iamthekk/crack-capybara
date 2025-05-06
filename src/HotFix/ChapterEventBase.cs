using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using XNode.GameEvent;

namespace HotFix
{
	public abstract class ChapterEventBase : GameEventBase
	{
		public sealed override void Init(GameEventData data, int stage, Action endAction, GameEventGroup group)
		{
			this.currentData = data;
			this.stage = stage;
			this.endAction = endAction;
			this.group = group;
			this.CreateFunctions(data);
			if (data.IsShowButton())
			{
				this.buttonDatas = data.GetButtons();
				for (int i = 0; i < this.buttonDatas.Count; i++)
				{
					this.CreateFunctions(this.buttonDatas[i]);
				}
			}
			this.OnInit();
		}

		protected void MergerAttribute(List<NodeAttParam> attParams)
		{
			Singleton<GameEventController>.Instance.MergerAttribute(attParams);
		}

		protected sealed override async Task OnClickButton(int buttonIndex)
		{
			if (!(this is ChapterEventRoundSkill) && (!(this is ChapterEventNormal) || !(this as ChapterEventNormal).IsMoveToNpcAutoClick))
			{
				GameApp.SDK.Analyze.Track_StagetClickTest(null);
				GameTGATools.Ins.SetStageButtonClickIndex(buttonIndex);
				GameTGATools.Ins.OnStageClickButton();
			}
			GameEventDataSelect btn = ((buttonIndex < this.buttonDatas.Count) ? this.buttonDatas[buttonIndex] : null);
			if (btn != null)
			{
				await this.DoFunctionImmediately(btn);
			}
			this.nextData = this.currentData.GetNext(buttonIndex);
			if (this.nextData != null)
			{
				if (btn != null)
				{
					this.nextData.SetFatherFunctions(btn.GetMyChildTriggerFunctions());
				}
				else
				{
					this.nextData.SetFatherFunctions(this.currentData.GetMyChildTriggerFunctions());
				}
				base.isDone = true;
				base.SetNext();
			}
			else
			{
				if (btn != null)
				{
					await this.TriggerMyFunction(btn);
				}
				else
				{
					await this.TriggerMyFunction(this.currentData);
				}
				base.isDone = true;
			}
		}

		protected void CreateFunctions(GameEventData funcData)
		{
			if (funcData == null)
			{
				return;
			}
			List<GameEventData> functionDatas = funcData.GetFunctionDatas();
			for (int i = 0; i < functionDatas.Count; i++)
			{
				GameEventDataFunction gameEventDataFunction = functionDatas[i] as GameEventDataFunction;
				if (gameEventDataFunction != null)
				{
					GameEventFunctionBase gameEventFunctionBase = this.CreateFunction(gameEventDataFunction);
					if (gameEventFunctionBase != null)
					{
						gameEventFunctionBase.Create();
						funcData.AddFunction(gameEventFunctionBase);
					}
				}
			}
		}

		protected async Task DoFunctionImmediately(GameEventData doData)
		{
			if (doData != null)
			{
				List<GameEventFunctionBase> list = doData.GetUndoMyFunctions(GameEventFunctionBase.FunctionDoType.Immediately);
				for (int i = 0; i < list.Count; i++)
				{
					await list[i].DoFunction();
				}
			}
		}

		protected async Task TriggerMyFunction(GameEventData doData)
		{
			if (doData != null)
			{
				List<GameEventFunctionBase> list = doData.GetUndoMyFunctions(GameEventFunctionBase.FunctionDoType.ChildTrigger);
				for (int i = 0; i < list.Count; i++)
				{
					await list[i].DoFunction();
				}
				base.SetNext();
			}
		}

		protected async Task TriggerFatherFunction(GameEventData doData)
		{
			if (doData != null)
			{
				List<GameEventFunctionBase> list = doData.GetUndoFatherFunctions();
				for (int i = 0; i < list.Count; i++)
				{
					await list[i].DoFunction();
				}
			}
		}

		private GameEventFunctionBase CreateFunction(GameEventDataFunction funcData)
		{
			GameEventFunctionBase gameEventFunctionBase = null;
			EventFunction eventFunctionType = funcData.eventFunctionType;
			switch (eventFunctionType)
			{
			case 1:
				gameEventFunctionBase = new GameEventFunctionAddEventPoint(funcData);
				break;
			case 2:
				gameEventFunctionBase = new GameEventFunctionDoEventPoint(funcData);
				break;
			case 3:
				gameEventFunctionBase = new GameEventFunctionPlayerAnimation(funcData);
				break;
			case 4:
				gameEventFunctionBase = new GameEventFunctionChangeMap(funcData);
				break;
			case 5:
				gameEventFunctionBase = new GameEventFunctionAddMonsterGroup(funcData);
				break;
			case 6:
				gameEventFunctionBase = new GameEventFunctionMonsterGroupLeave(funcData);
				break;
			case 7:
				gameEventFunctionBase = new GameEventFunctionAddFollowNpc(funcData);
				break;
			case 8:
				gameEventFunctionBase = new GameEventFunctionEmoticons(funcData);
				break;
			case 9:
			case 18:
				break;
			case 10:
				gameEventFunctionBase = new GameEventFunctionPassingNpc(funcData);
				break;
			case 11:
				gameEventFunctionBase = new GameEventFunctionLostRandomSkill(funcData);
				break;
			case 12:
				gameEventFunctionBase = new GameEventFunctionLostTagSkills(funcData);
				break;
			case 13:
				gameEventFunctionBase = new GameEventFunctionGetSkill(funcData);
				break;
			case 14:
				gameEventFunctionBase = new GameEventFunctionRandomSkill(funcData);
				break;
			case 15:
				gameEventFunctionBase = new GameEventFunctionChangeAttr(funcData);
				break;
			case 16:
				gameEventFunctionBase = new GameEventFunctionGetItem(funcData);
				break;
			case 17:
				gameEventFunctionBase = new GameEventFunctionLevelUp(funcData);
				break;
			case 19:
				gameEventFunctionBase = new GameEventFunctionLostIDSkill(funcData);
				break;
			case 20:
				gameEventFunctionBase = new GameEventFunctionLostAllFood(funcData);
				break;
			case 21:
				gameEventFunctionBase = new GameEventFunctionRandomBox(funcData);
				break;
			case 22:
				gameEventFunctionBase = new GameEventFunctionFixBox(funcData);
				break;
			case 23:
				gameEventFunctionBase = new GameEventFunctionRandomSurprise(funcData);
				break;
			case 24:
				gameEventFunctionBase = new GameEventFunctionFixSurprise(funcData);
				break;
			case 25:
				gameEventFunctionBase = new GameEventFunctionGetItemServer(funcData);
				break;
			default:
				if (eventFunctionType == 101)
				{
					gameEventFunctionBase = new GameEventFunctionFishingAllEnd(funcData);
				}
				break;
			}
			return gameEventFunctionBase;
		}
	}
}
