using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	[CreateAssetMenu]
	public class HotfixConfigData : ScriptableObject
	{
		private void OnEnable()
		{
			if (this.infoDic == null)
			{
				this.infoDic = new Dictionary<string, HotfixConfigInfo>(this.infoList.Count);
				for (int i = 0; i < this.infoList.Count; i++)
				{
					HotfixConfigInfo hotfixConfigInfo = this.infoList[i];
					this.infoDic.Add(hotfixConfigInfo.name, hotfixConfigInfo);
				}
			}
		}

		private void OnDestroy()
		{
			if (this.infoDic != null)
			{
				this.infoDic.Clear();
			}
			this.infoDic = null;
		}

		public string GetConfigInfo(string infoName)
		{
			HotfixConfigInfo hotfixConfigInfo;
			this.infoDic.TryGetValue(infoName, out hotfixConfigInfo);
			if (hotfixConfigInfo == null)
			{
				return string.Empty;
			}
			return hotfixConfigInfo.info;
		}

		public List<HotfixConfigInfo> infoList = new List<HotfixConfigInfo>();

		public Dictionary<string, HotfixConfigInfo> infoDic;
	}
}
