using System;
using UnityEngine;

namespace Framework.GameAppConfig
{
	public class GameAppConfigManager : MonoBehaviour
	{
		public bool GetBool(string key)
		{
			return !(this.m_gameAppConfigData == null) && string.Equals(this.m_gameAppConfigData.GetConfigInfo(key), "True");
		}

		public string GetString(string key)
		{
			if (this.m_gameAppConfigData == null)
			{
				return string.Empty;
			}
			return this.m_gameAppConfigData.GetConfigInfo(key);
		}

		[SerializeField]
		private GameAppConfigData m_gameAppConfigData;
	}
}
