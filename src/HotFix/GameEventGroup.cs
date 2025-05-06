using System;
using Server;

namespace HotFix
{
	public class GameEventGroup
	{
		public void Init(GameEventData data, int iStage, Action iEndAction)
		{
			this.currentData = data;
			this.currentData.SetRoot(true);
			this.stage = iStage;
			this.endAction = iEndAction;
			this.currentEvent = this.GetEvent(this.currentData);
			this.xRandom = new XRandom(data.poolData.randomSeed);
		}

		public void DeInit()
		{
			if (this.currentEvent != null)
			{
				this.currentEvent.DeInit();
				this.currentEvent = null;
			}
		}

		public XRandom GetRandom()
		{
			return this.xRandom;
		}

		public void SetCurrentData(GameEventData data)
		{
			this.currentData = data;
		}

		public void PushEvent(GameEventPushType pushType, object param)
		{
			if (this.currentEvent != null)
			{
				this.currentEvent.DoEvent(pushType, param);
			}
		}

		protected void OnEventEnd()
		{
			if (this.currentEvent != null && this.currentEvent.IsEnd)
			{
				this.currentEvent.DeInit();
				if (this.currentData == null)
				{
					Action action = this.endAction;
					if (action == null)
					{
						return;
					}
					action();
					return;
				}
				else
				{
					this.currentEvent = this.GetEvent(this.currentData);
				}
			}
		}

		public void HangUp()
		{
			if (this.currentEvent != null)
			{
				this.currentEvent.HangUp();
			}
		}

		public void ResumeHangUp()
		{
			if (this.currentEvent != null)
			{
				this.currentEvent.ResumeHangUp();
			}
		}

		public bool IsCurrentEventDone()
		{
			return this.currentEvent == null || this.currentEvent.isDone;
		}

		private GameEventBase GetEvent(GameEventData data)
		{
			if (data == null)
			{
				HLog.LogError("GameEventGroup.GetEvent: data is null");
				return null;
			}
			GameEventNodeType nodeType = data.GetNodeType();
			GameEventBase gameEventBase;
			if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Sweep)
			{
				gameEventBase = this.CreateSweepEvent(nodeType);
			}
			else
			{
				gameEventBase = this.CreateChapterEvent(nodeType);
			}
			if (gameEventBase != null)
			{
				gameEventBase.Init(data, this.stage, new Action(this.OnEventEnd), this);
			}
			return gameEventBase;
		}

		private GameEventBase CreateChapterEvent(GameEventNodeType type)
		{
			GameEventBase gameEventBase = null;
			switch (type)
			{
			case GameEventNodeType.Normal:
				return new ChapterEventNormal();
			case GameEventNodeType.Battle:
				return new ChapterEventBattle();
			case GameEventNodeType.Shop:
				return new ChapterEventShop();
			case GameEventNodeType.Skill:
				return new ChapterEventGetSkill();
			case GameEventNodeType.Box:
				return new ChapterEventBox();
			case GameEventNodeType.Surprise:
				return new ChapterEventSurprise();
			case GameEventNodeType.TalentSkill:
				return new ChapterEventTalentSkill();
			case GameEventNodeType.Fishing:
				return new ChapterEventFishing();
			case GameEventNodeType.NpcBattle:
				return new ChapterEventNpcBattle();
			case GameEventNodeType.FishingGame:
				return new ChapterEventFishingChapter();
			case GameEventNodeType.MiniGame:
				return new ChapterEventMiniGame();
			case GameEventNodeType.WaveBattle:
				return new ChapterEventWaveBattle();
			case GameEventNodeType.RoundSkill:
				return new ChapterEventRoundSkill();
			}
			HLog.LogError(string.Format("GameEventGroup.GetEvent: event type error, type={0}", type));
			return gameEventBase;
		}

		private GameEventBase CreateSweepEvent(GameEventNodeType type)
		{
			GameEventBase gameEventBase = null;
			switch (type)
			{
			case GameEventNodeType.Normal:
				return new SweepEventNormal();
			case GameEventNodeType.Select:
			case GameEventNodeType.IF:
			case GameEventNodeType.Shop:
				break;
			case GameEventNodeType.Battle:
				return new SweepEventBattle();
			case GameEventNodeType.Skill:
				return new SweepEventGetSkill();
			case GameEventNodeType.Box:
				return new SweepEventBox();
			case GameEventNodeType.Surprise:
				return new SweepEventSurprise();
			default:
				switch (type)
				{
				case GameEventNodeType.NpcBattle:
					return new SweepEventNpcBattle();
				case GameEventNodeType.MiniGame:
					return new SweepEventMiniGame();
				case GameEventNodeType.WaveBattle:
					return new SweepEventWaveBattle();
				case GameEventNodeType.RoundSkill:
					return new SweepEventRoundSkill();
				}
				break;
			}
			HLog.LogError(string.Format("GameEventGroup.GetEvent: event type error, type={0}", type));
			return gameEventBase;
		}

		private int stage;

		private GameEventData currentData;

		private GameEventBase currentEvent;

		private Action endAction;

		private bool isActivityDone;

		private XRandom xRandom;
	}
}
