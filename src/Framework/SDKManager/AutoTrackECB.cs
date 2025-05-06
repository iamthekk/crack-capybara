using System;
using System.Collections.Generic;
using ThinkingData.Analytics;

namespace Framework.SDKManager
{
	public class AutoTrackECB : TDAutoTrackEventHandler
	{
		public Dictionary<string, object> GetAutoTrackEventProperties(int type, Dictionary<string, object> properties)
		{
			foreach (KeyValuePair<string, object> keyValuePair in properties)
			{
				if (keyValuePair.Key.Equals("#start_reason"))
				{
					GameApp.DeepLink.UpdateDeepLink((string)keyValuePair.Value);
				}
			}
			return new Dictionary<string, object> { 
			{
				"AutoTrackEventProperty",
				DateTime.Today
			} };
		}
	}
}
