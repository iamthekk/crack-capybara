using System;
using System.Collections.Generic;
using Framework;

namespace HotFix
{
	public class NodeScoreParam : NodeParamBase
	{
		public ulong activityRowId { get; private set; }

		public ChapterDropSource source { get; private set; }

		public int score { get; private set; }

		public int atlasId { get; private set; }

		public string scoreIcon { get; private set; }

		public string scoreNameId { get; private set; }

		public int activityId { get; private set; }

		public ChapterActivityKind activityKind { get; private set; }

		public override double FinalCount
		{
			get
			{
				int num = 1;
				if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Sweep)
				{
					num = GameApp.Data.GetDataModule(DataName.ChapterSweepDataModule).SweepRate;
					if (num < 1)
					{
						num = 1;
					}
				}
				return (double)(this.score * num);
			}
		}

		public override NodeKind GetNodeKind()
		{
			return NodeKind.ActivityScore;
		}

		public NodeScoreParam(ulong rowId, ChapterDropSource source, int score)
		{
			this.activityRowId = rowId;
			this.source = source;
			this.score = score;
			ChapterActivityData activityData = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).GetActivityData(rowId);
			if (activityData != null)
			{
				this.activityId = (int)activityData.ActivityId;
				this.atlasId = activityData.ScoreAtlasId;
				this.scoreIcon = activityData.ScoreIcon;
				this.scoreNameId = activityData.ScoreNameId;
				this.activityKind = activityData.Kind;
				return;
			}
			ChapterBattlePassDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule);
			if (dataModule.BattlePassDto != null && dataModule.BattlePassConfig != null && dataModule.BattlePassDto.RowId == (long)rowId && dataModule.IsInProgress())
			{
				this.activityId = dataModule.BattlePassConfig.id;
				this.atlasId = dataModule.BattlePassConfig.atlasID;
				this.scoreIcon = dataModule.BattlePassConfig.itemIcon;
				this.scoreNameId = dataModule.BattlePassConfig.itemNameId;
				this.activityKind = ChapterActivityKind.BattlePass;
				return;
			}
			ChapterActivityWheelDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
			if (dataModule2.IsActivityOpen((long)rowId))
			{
				this.activityId = dataModule2.WheelInfo.ActiveId;
				this.atlasId = dataModule2.ScoreAtlas;
				this.scoreIcon = dataModule2.ScoreIcon;
				this.scoreNameId = dataModule2.ScoreNameId;
				this.activityKind = ChapterActivityKind.Wheel;
			}
		}

		public static List<ActivityScoreTypeData> GetScoreParamList(List<NodeScoreParam> items)
		{
			List<ActivityScoreTypeData> list = new List<ActivityScoreTypeData>();
			for (int i = 0; i < items.Count; i++)
			{
				NodeScoreParam nodeScoreParam = items[i];
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(nodeScoreParam.scoreNameId);
				string text = string.Format("+{0}", nodeScoreParam.FinalCount);
				ActivityScoreTypeData activityScoreTypeData = new ActivityScoreTypeData();
				activityScoreTypeData.atlas = nodeScoreParam.atlasId;
				activityScoreTypeData.icon = nodeScoreParam.scoreIcon;
				activityScoreTypeData.m_value = (activityScoreTypeData.m_value = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_153", new object[] { "<color=#91EEF6>" + infoByID + text + "</color>" }));
				activityScoreTypeData.m_tgaValue = (activityScoreTypeData.m_tgaValue = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_153", new object[] { infoByID + text }));
				list.Add(activityScoreTypeData);
			}
			return list;
		}

		public FlyItemOtherType GetFlyType()
		{
			switch (this.activityKind)
			{
			case ChapterActivityKind.Rank:
				return FlyItemOtherType.ActivityScoreRank;
			case ChapterActivityKind.BattlePass:
				return FlyItemOtherType.ActivityScoreNormal;
			case ChapterActivityKind.Wheel:
				return FlyItemOtherType.ActivityScoreWheel;
			default:
				return FlyItemOtherType.Null;
			}
		}
	}
}
