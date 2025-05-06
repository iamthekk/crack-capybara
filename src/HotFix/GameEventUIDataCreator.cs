using System;
using System.Collections.Generic;

namespace HotFix
{
	public static class GameEventUIDataCreator
	{
		public static GameEventUIData Create(int eventResId, int stage, bool isRoot, string languageId, object[] languageParams, List<NodeAttParam> paramList = null, List<NodeItemParam> itemList = null, List<NodeSkillParam> skillList = null, List<string> infoList = null, List<NodeScoreParam> scoreList = null)
		{
			GameEventUIData gameEventUIData = new GameEventUIData(eventResId, stage, isRoot, languageId, languageParams);
			gameEventUIData.SetAttParam(paramList);
			gameEventUIData.SetItemParam(itemList);
			gameEventUIData.SetSkillParam(skillList);
			gameEventUIData.SetInfParam(infoList);
			gameEventUIData.SetScoreParam(scoreList);
			Singleton<EventRecordController>.Instance.CacheUI(gameEventUIData);
			return gameEventUIData;
		}
	}
}
