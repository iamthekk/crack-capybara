using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
	public static class BattleLogHelper
	{
		public static bool IsLog { get; }

		public static void SetEnableLog(bool isLog)
		{
		}

		public static void LogCardData(CardData cardData, string prefix = "")
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogAttributeDataList(List<MergeAttributeData> attributeDataList, string prefix = "")
		{
			if (!BattleLogHelper.IsLog)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("{0} Attribute.Count:{1} {{", prefix, attributeDataList.Count));
			for (int i = 0; i < attributeDataList.Count; i++)
			{
				MergeAttributeData mergeAttributeData = attributeDataList[i];
				stringBuilder.Append(string.Format("{0}:{1}", mergeAttributeData.Header, mergeAttributeData.Value));
			}
			stringBuilder.Append("}");
		}

		public static void LogCreateDataMembers(List<OutMemberData> outMemberDatas, string prefix = "")
		{
			if (!BattleLogHelper.IsLog)
			{
				return;
			}
		}

		public static void LogSMemberAttributes(SMemberData memberData, string prefix = "")
		{
			if (!BattleLogHelper.IsLog)
			{
				return;
			}
			CardData cardData = memberData.cardData;
			MemberAttributeData attribute = memberData.attribute;
		}

		public static void DebugReportByRound(int round, List<BaseBattleReportData> m_datas)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_GameStart(BattleReportData_GameStart report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_RoundStart(BattleReportData_RoundStart report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_RoundEnd(BattleReportData_RoundEnd report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_Move(BattleReportData_Move report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_PlaySkill(BattleReportData_PlaySkill report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_PlaySkillComplete(BattleReportData_PlaySkillComplete report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_CreateBullet(BattleReportData_CreateBullet report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_Hurt(BattleReportData_Hurt report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_ChangeAttribute(BattleReportData_ChangeAttribute report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_WaveChange(BattleReportData_WaveChange report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}

		public static void LogReport_LegacySkillSummonDisplay(BattleReportData_LegacySkillSummonDisplay report)
		{
			bool isLog = BattleLogHelper.IsLog;
		}
	}
}
