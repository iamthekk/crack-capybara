using System;
using System.Collections.Generic;

namespace Server
{
	public class BattleReportData_ChangeAttribute : BaseBattleReportData
	{
		public int TargetInstanceID { get; private set; }

		public Dictionary<string, BattleReportData_AttributeData> Attributes { get; private set; } = new Dictionary<string, BattleReportData_AttributeData>();

		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.ChangeAttributes;
			}
		}

		public BattleReportData_ChangeAttribute SetData(int targetInstanceId)
		{
			this.TargetInstanceID = targetInstanceId;
			return this;
		}

		public BattleReportData_ChangeAttribute AddData(BattleReportData_AttributeData data)
		{
			if (data == null)
			{
				return this;
			}
			BattleReportData_AttributeData battleReportData_AttributeData;
			if (this.Attributes.TryGetValue(data.attributeKey, out battleReportData_AttributeData))
			{
				this.Attributes[data.attributeKey] = data;
			}
			else
			{
				this.Attributes.Add(data.attributeKey, data);
			}
			return this;
		}

		public override string ToString()
		{
			string text = string.Format("[attributes count:{0}]:\n", this.Attributes.Count);
			foreach (KeyValuePair<string, BattleReportData_AttributeData> keyValuePair in this.Attributes)
			{
				text = text + string.Format("{0} curValue:{1} changeValue:{2}", keyValuePair.Key, keyValuePair.Value.curValue, keyValuePair.Value.changeValue) + "\n";
			}
			return text;
		}
	}
}
