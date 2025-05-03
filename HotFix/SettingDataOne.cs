using System;
using Framework.Logic;

namespace HotFix
{
	public class SettingDataOne
	{
		public SettingDataOne(SettingDataName dataName)
		{
			this.MSettingDataName = dataName;
			this.m_saveKey = string.Format("Save_Setting_{0}", this.MSettingDataName.ToString());
			this.m_isOpen = Utility.PlayerPrefs.GetInt(this.m_saveKey, 1) == 1;
		}

		public void SetValue(bool isOpen)
		{
			this.m_isOpen = isOpen;
			this.Save();
		}

		public bool GetValue()
		{
			return this.m_isOpen;
		}

		private void Save()
		{
			Utility.PlayerPrefs.SetInt(this.m_saveKey, this.m_isOpen ? 1 : 0);
		}

		private const string KeyNameFormat = "Save_Setting_{0}";

		public SettingDataName MSettingDataName;

		private string m_saveKey;

		public bool m_isOpen;
	}
}
