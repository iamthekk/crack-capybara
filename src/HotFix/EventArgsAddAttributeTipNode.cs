using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsAddAttributeTipNode : BaseEventArgs
	{
		public void SetData(List<KeyValuePair<string, long>> param)
		{
			if (param == null || param.Count <= 0)
			{
				return;
			}
			this.data = param;
		}

		public void AddData(string attributeKey, long value)
		{
			this.data.Add(new KeyValuePair<string, long>(attributeKey, value));
		}

		public override void Clear()
		{
			this.data.Clear();
		}

		public List<KeyValuePair<string, long>> data = new List<KeyValuePair<string, long>>();
	}
}
