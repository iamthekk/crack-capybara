using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;

namespace HotFix
{
	public class SettingDataModule : IDataModule
	{
		public int GetName()
		{
			return 102;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		public void InitValue()
		{
			this.m_dic = new Dictionary<SettingDataName, SettingDataOne>();
			this.m_dic.Add(SettingDataName.Background, new SettingDataOne(SettingDataName.Background));
			this.m_dic.Add(SettingDataName.SoundEffect, new SettingDataOne(SettingDataName.SoundEffect));
			this.m_dic.Add(SettingDataName.Vibration, new SettingDataOne(SettingDataName.Vibration));
			GameApp.Sound.SetSoundEffectOpen(this.GetValue(SettingDataName.SoundEffect));
			GameApp.Sound.SetBackgroundVolume((float)(this.GetValue(SettingDataName.Background) ? 0 : (-80)));
			DeviceVibration.SetOpen(this.GetValue(SettingDataName.Vibration));
		}

		public void SetValue(SettingDataName dataName, bool value)
		{
			SettingDataOne settingDataOne;
			if (!this.m_dic.TryGetValue(dataName, out settingDataOne))
			{
				HLog.LogError(string.Format("SettingDataModule SetValue Error:{0}", dataName));
				return;
			}
			settingDataOne.SetValue(value);
		}

		public bool GetValue(SettingDataName dataName)
		{
			SettingDataOne settingDataOne;
			if (!this.m_dic.TryGetValue(dataName, out settingDataOne))
			{
				HLog.LogError(string.Format("SettingDataModule GetValue Error:{0}", dataName));
				return true;
			}
			return settingDataOne.GetValue();
		}

		private Dictionary<SettingDataName, SettingDataOne> m_dic;
	}
}
