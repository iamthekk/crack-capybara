using System;
using UnityEngine;

namespace Framework.Logic.Modules
{
	public class LanguageManager : Singleton<LanguageManager>
	{
		public string GetInfoByID(LanguageType languageType, int id)
		{
			return this.GetInfoByID(languageType, id.ToString());
		}

		public string GetInfoByID(LanguageType languageType, string id)
		{
			return GameApp.RunTime.GetInfoByID(languageType, id);
		}

		public string GetInfoByID(int id)
		{
			return this.GetInfoByID(this.m_data.GetCurrentLanguageType, id);
		}

		public string GetInfoByID(int id, params object[] args)
		{
			return string.Format(this.GetInfoByID(this.m_data.GetCurrentLanguageType, id), args);
		}

		public void CheckLanguage()
		{
			this.m_data = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			this.m_data.CheckLanguage();
		}

		public string GetCurrentLanguage()
		{
			return this.m_data.GetCurrentLanguageType.ToString();
		}

		private LanguageDataModule m_data;

		public SystemLanguage m_systemLanguage = 10;
	}
}
