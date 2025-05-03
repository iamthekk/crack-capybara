using System;

namespace Server
{
	public class BattleReportData_AttributeData
	{
		public BattleReportData_AttributeData(string attributeKey, FP curValue, FP changeValue, string param = null)
		{
			this.attributeKey = attributeKey;
			this.curValue = curValue;
			this.changeValue = changeValue;
			this.param = param;
		}

		public string attributeKey;

		public FP changeValue;

		public FP curValue;

		public string param;
	}
}
