using System;
using System.Collections.Generic;

namespace Framework.SDKManager
{
	internal struct TGATrackData
	{
		public TGATrackData(string key, Dictionary<string, object> dic)
		{
			this.key = key;
			this.dic = dic;
		}

		public string key;

		public Dictionary<string, object> dic;
	}
}
