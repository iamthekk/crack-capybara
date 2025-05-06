using System;
using System.Collections.Generic;
using Framework.Logic;
using Server;

namespace HotFix
{
	public class NodeAttParam : NodeParamBase
	{
		public GameEventAttType attType { get; private set; }

		public double baseCount { get; private set; }

		public ChapterDropSource dropSource { get; private set; }

		public int rate { get; private set; }

		public override NodeKind GetNodeKind()
		{
			return NodeKind.EventAtt;
		}

		public override double FinalCount
		{
			get
			{
				return (this.baseCount + (double)this.AddAttribute()) * (double)this.rate;
			}
		}

		public NodeAttParam(GameEventAttType attType, double baseCount, ChapterDropSource dropSource, int rate = 1)
		{
			this.attType = attType;
			this.baseCount = baseCount;
			this.dropSource = dropSource;
			this.rate = ((rate < 1) ? 1 : rate);
		}

		public void AddNum(float add)
		{
			this.baseCount += (double)add;
		}

		public void SetNum(double newNum)
		{
			this.baseCount = newNum;
		}

		public static string GetAttName(GameEventAttType type, float num)
		{
			string text = "";
			switch (type)
			{
			case GameEventAttType.AttackPercent:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_16");
				break;
			case GameEventAttType.RecoverHpRate:
			case GameEventAttType.CampHpRate:
			{
				string text2 = "%";
				if (num >= 0f)
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_149", new object[] { Utility.Math.Abs(num).ToString() + text2 });
				}
				else
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_148", new object[] { Utility.Math.Abs(num).ToString() + text2 });
				}
				break;
			}
			case GameEventAttType.HPMaxPercent:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_18");
				break;
			case GameEventAttType.Exp:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_20");
				break;
			case GameEventAttType.Food:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_19");
				break;
			case GameEventAttType.DefencePercent:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_37");
				break;
			case GameEventAttType.Chips:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_chips");
				break;
			}
			return text;
		}

		public static string GetAttName_TGA(GameEventAttType type, float num)
		{
			string text = "";
			switch (type)
			{
			case GameEventAttType.AttackPercent:
				text = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_16");
				break;
			case GameEventAttType.RecoverHpRate:
			case GameEventAttType.CampHpRate:
			{
				string text2 = "%";
				if (num >= 0f)
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_149", new object[] { Utility.Math.Abs(num).ToString() + text2 });
				}
				else
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_148", new object[] { Utility.Math.Abs(num).ToString() + text2 });
				}
				break;
			}
			case GameEventAttType.HPMaxPercent:
				text = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_18");
				break;
			case GameEventAttType.Exp:
				text = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_20");
				break;
			case GameEventAttType.Food:
				text = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_19");
				break;
			case GameEventAttType.DefencePercent:
				text = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_37");
				break;
			case GameEventAttType.Chips:
				text = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_chips");
				break;
			}
			return text;
		}

		public bool IsRate()
		{
			return this.attType == GameEventAttType.AttackPercent || this.attType == GameEventAttType.DefencePercent || this.attType == GameEventAttType.HPMaxPercent || this.attType == GameEventAttType.RecoverHpRate || this.attType == GameEventAttType.CampHpRate;
		}

		public NodeAttParam Clone()
		{
			return new NodeAttParam(this.attType, this.baseCount, this.dropSource, this.rate);
		}

		public static List<AttributeTypeData> GetAttParamList(List<NodeAttParam> paramList)
		{
			List<AttributeTypeData> list = new List<AttributeTypeData>();
			Dictionary<GameEventAttType, double> dictionary = NodeAttParam.MergerSameAtt(paramList);
			foreach (GameEventAttType gameEventAttType in dictionary.Keys)
			{
				GameEventAttType gameEventAttType2 = gameEventAttType;
				float num = (float)Math.Round(dictionary[gameEventAttType], 1, MidpointRounding.AwayFromZero);
				if (num != 0f)
				{
					AttributeTypeData attributeTypeData = new AttributeTypeData();
					attributeTypeData.m_attrType = gameEventAttType2;
					attributeTypeData.num = num;
					if (num >= 0f)
					{
						attributeTypeData.m_value = "<color=#D3F24E>";
					}
					else
					{
						attributeTypeData.m_value = "<color=#FF604D>";
					}
					AttributeTypeData attributeTypeData2 = attributeTypeData;
					attributeTypeData2.m_value += NodeAttParam.GetAttName(gameEventAttType2, num);
					AttributeTypeData attributeTypeData3 = attributeTypeData;
					attributeTypeData3.m_tgaValue += NodeAttParam.GetAttName_TGA(gameEventAttType2, num);
					string text = string.Empty;
					if (gameEventAttType2 == GameEventAttType.AttackPercent || gameEventAttType2 == GameEventAttType.HPMaxPercent || gameEventAttType2 == GameEventAttType.RecoverHpRate || gameEventAttType2 == GameEventAttType.DefencePercent || gameEventAttType2 == GameEventAttType.CampHpRate)
					{
						text = "%";
					}
					if (gameEventAttType2 == GameEventAttType.RecoverHpRate || gameEventAttType2 == GameEventAttType.CampHpRate)
					{
						text = "";
					}
					else
					{
						AttributeTypeData attributeTypeData4 = attributeTypeData;
						attributeTypeData4.m_value += ((num >= 0f) ? string.Format("+{0}", num) : string.Format("{0}", num));
						AttributeTypeData attributeTypeData5 = attributeTypeData;
						attributeTypeData5.m_tgaValue += ((num >= 0f) ? string.Format("+{0}", num) : string.Format("{0}", num));
					}
					AttributeTypeData attributeTypeData6 = attributeTypeData;
					attributeTypeData6.m_value += text;
					AttributeTypeData attributeTypeData7 = attributeTypeData;
					attributeTypeData7.m_tgaValue += text;
					AttributeTypeData attributeTypeData8 = attributeTypeData;
					attributeTypeData8.m_value += "</color>";
					list.Add(attributeTypeData);
				}
			}
			return list;
		}

		public static AttributeTypeData GetAttParam(NodeAttParam param)
		{
			GameEventAttType attType = param.attType;
			float num = (float)Math.Round(param.FinalCount, 1, MidpointRounding.AwayFromZero);
			AttributeTypeData attributeTypeData = new AttributeTypeData();
			attributeTypeData.m_attrType = attType;
			attributeTypeData.num = num;
			if (num >= 0f)
			{
				attributeTypeData.m_value = "<color=#D3F24E>";
			}
			else
			{
				attributeTypeData.m_value = "<color=#FF604D>";
			}
			AttributeTypeData attributeTypeData2 = attributeTypeData;
			attributeTypeData2.m_value += NodeAttParam.GetAttName(attType, num);
			AttributeTypeData attributeTypeData3 = attributeTypeData;
			attributeTypeData3.m_tgaValue += NodeAttParam.GetAttName_TGA(attType, num);
			string text = string.Empty;
			if (attType == GameEventAttType.AttackPercent || attType == GameEventAttType.HPMaxPercent || attType == GameEventAttType.RecoverHpRate || attType == GameEventAttType.DefencePercent || attType == GameEventAttType.CampHpRate)
			{
				text = "%";
			}
			if (attType == GameEventAttType.RecoverHpRate || attType == GameEventAttType.CampHpRate)
			{
				text = "";
			}
			else
			{
				AttributeTypeData attributeTypeData4 = attributeTypeData;
				attributeTypeData4.m_value += ((num >= 0f) ? string.Format("+{0}", num) : string.Format("{0}", num));
				AttributeTypeData attributeTypeData5 = attributeTypeData;
				attributeTypeData5.m_tgaValue += ((num >= 0f) ? string.Format("+{0}", num) : string.Format("{0}", num));
			}
			AttributeTypeData attributeTypeData6 = attributeTypeData;
			attributeTypeData6.m_value += text;
			AttributeTypeData attributeTypeData7 = attributeTypeData;
			attributeTypeData7.m_tgaValue += text;
			AttributeTypeData attributeTypeData8 = attributeTypeData;
			attributeTypeData8.m_value += "</color>";
			return attributeTypeData;
		}

		private static Dictionary<GameEventAttType, double> MergerSameAtt(List<NodeAttParam> list)
		{
			Dictionary<GameEventAttType, double> dictionary = new Dictionary<GameEventAttType, double>();
			for (int i = 0; i < list.Count; i++)
			{
				GameEventAttType attType = list[i].attType;
				double finalCount = list[i].FinalCount;
				if (dictionary.ContainsKey(attType))
				{
					Dictionary<GameEventAttType, double> dictionary2 = dictionary;
					GameEventAttType gameEventAttType = attType;
					dictionary2[gameEventAttType] += finalCount;
				}
				else
				{
					dictionary.Add(attType, finalCount);
				}
			}
			return dictionary;
		}

		private float AddAttribute()
		{
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			if (playerData != null)
			{
				GameEventAttType attType = this.attType;
				if (attType == GameEventAttType.Exp)
				{
					FP expAddRate = playerData.AttributeData.ExpAddRate;
					return (float)(this.baseCount * expAddRate).AsLong();
				}
				if (attType == GameEventAttType.CampHpRate)
				{
					return playerData.AttributeData.RecoveryRate.AsFloat() * 100f;
				}
			}
			return 0f;
		}

		public static string GetUIShowAttributeInfo(GameEventAttType attType, long attParam)
		{
			string attributeName = NodeAttParam.GetAttributeName(attType);
			string text = string.Format("{0}+{1}%", attributeName, attParam);
			if (attType == GameEventAttType.RecoverHpRate)
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_149", new object[] { attParam.ToString() + "%" });
			}
			return text;
		}

		public static string GetAttributeName(GameEventAttType attType)
		{
			string text = "";
			switch (attType)
			{
			case GameEventAttType.AttackPercent:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_16");
				break;
			case GameEventAttType.RecoverHpRate:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_RestoreHP");
				break;
			case GameEventAttType.HPMaxPercent:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_18");
				break;
			case GameEventAttType.DefencePercent:
				text = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_37");
				break;
			}
			return text;
		}
	}
}
