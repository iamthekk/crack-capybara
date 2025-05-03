using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	[CreateAssetMenu]
	public class GameAppConfigData : ScriptableObject
	{
		private void OnEnable()
		{
			if (this.m_gameAppConfigInfoDic == null)
			{
				this.m_gameAppConfigInfoDic = new Dictionary<string, GameAppConfigInfo>(this.m_gameAppConfigInfoList.Count);
				for (int i = 0; i < this.m_gameAppConfigInfoList.Count; i++)
				{
					GameAppConfigInfo gameAppConfigInfo = this.m_gameAppConfigInfoList[i];
					this.m_gameAppConfigInfoDic.Add(gameAppConfigInfo.name, gameAppConfigInfo);
				}
			}
		}

		private void OnDestroy()
		{
			if (this.m_gameAppConfigInfoDic != null)
			{
				this.m_gameAppConfigInfoDic.Clear();
			}
			this.m_gameAppConfigInfoDic = null;
		}

		public string GetConfigInfo(string name)
		{
			GameAppConfigInfo gameAppConfigInfo;
			this.m_gameAppConfigInfoDic.TryGetValue(name, out gameAppConfigInfo);
			if (gameAppConfigInfo == null)
			{
				return string.Empty;
			}
			return gameAppConfigInfo.info;
		}

		public List<GameAppConfigInfo> m_gameAppConfigInfoList = new List<GameAppConfigInfo>();

		public Dictionary<string, GameAppConfigInfo> m_gameAppConfigInfoDic;
	}
}
