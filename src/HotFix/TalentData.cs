using System;
using System.Collections.Generic;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class TalentData
	{
		public int TalentStage { get; private set; } = -1;

		public int TalentExp { get; private set; }

		public int TotalExp
		{
			get
			{
				return this.prevTalentTotalExp + this.TotalExp;
			}
		}

		public Dictionary<string, TalentAttributeLevelUpData> AttributeMap { get; private set; } = new Dictionary<string, TalentAttributeLevelUpData>();

		public void Init(int stage, int exp)
		{
			this.TalentStage = stage;
			this.TalentExp = exp;
			TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(this.TalentStage);
			this.prevTalentTotalExp = elementById.exp;
			List<MergeAttributeData> mergeAttributeData = elementById.attributeGroup.GetMergeAttributeData();
			for (int i = 0; i < mergeAttributeData.Count; i++)
			{
				TalentAttributeLevelUpData talentAttributeLevelUpData = new TalentAttributeLevelUpData(this.TalentStage, mergeAttributeData[i], elementById.levelLimit, elementById.levelupCost);
				this.AttributeMap[talentAttributeLevelUpData.talentAttributeKey] = talentAttributeLevelUpData;
			}
		}

		public void UpdateExp(int exp)
		{
			this.TalentExp = exp;
		}

		public void UpdateTalentAttributeLevel(MapField<string, uint> map)
		{
			foreach (KeyValuePair<string, uint> keyValuePair in map)
			{
				this.UpdateTalentAttributeLevel(keyValuePair.Key, (int)keyValuePair.Value);
			}
		}

		public void UpdateTalentAttributeLevel(string attributeKey, int value)
		{
			if (this.AttributeMap.ContainsKey(attributeKey))
			{
				this.AttributeMap[attributeKey].UpdateLevel(value);
				return;
			}
			HLog.LogError("server step attribute:[" + attributeKey + "] level up data is not sync with client");
		}

		public void Fix2FullLevel(int exp)
		{
			TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(this.TalentStage);
			foreach (KeyValuePair<string, TalentAttributeLevelUpData> keyValuePair in this.AttributeMap)
			{
				keyValuePair.Value.UpdateLevel(elementById.levelLimit);
			}
			this.TalentExp = exp;
		}

		private int prevTalentTotalExp;
	}
}
