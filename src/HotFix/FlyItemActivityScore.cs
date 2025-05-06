using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class FlyItemActivityScore : BaseFlyItem
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public override void SetData(object param)
		{
			if (param == null)
			{
				return;
			}
			ulong num = (ulong)param;
			ChapterBattlePassDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule);
			ChapterActivityWheelDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
			if (dataModule.BattlePassDto != null && dataModule.BattlePassDto.RowId == (long)num)
			{
				ChapterActivity_Battlepass chapterActivity_Battlepass = GameApp.Table.GetManager().GetChapterActivity_Battlepass(dataModule.BattlePassDto.ConfigId);
				if (chapterActivity_Battlepass != null)
				{
					string atlasPath = GameApp.Table.GetAtlasPath(chapterActivity_Battlepass.atlasID);
					this.m_icon.SetImage(atlasPath, chapterActivity_Battlepass.itemIcon);
					return;
				}
			}
			else
			{
				if (dataModule2.IsActivityOpen((long)num))
				{
					string atlasPath2 = GameApp.Table.GetAtlasPath(dataModule2.ScoreAtlas);
					this.m_icon.SetImage(atlasPath2, dataModule2.ScoreIcon);
					return;
				}
				ChapterActivityData activityData = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).GetActivityData(num);
				if (activityData != null)
				{
					string atlasPath3 = GameApp.Table.GetAtlasPath(activityData.ScoreAtlasId);
					this.m_icon.SetImage(atlasPath3, activityData.ScoreIcon);
				}
			}
		}
	}
}
