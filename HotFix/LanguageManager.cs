using System;
using Framework;
using Framework.Logic.Modules;
using LocalModels.Bean;

namespace HotFix
{
	public class LanguageManager : Singleton<LanguageManager>
	{
		protected LanguageDataModule LanguageData
		{
			get
			{
				if (this.m_data == null)
				{
					this.m_data = GameApp.Data.GetDataModule<LanguageDataModule>(1);
				}
				return this.m_data;
			}
		}

		public LanguageType GameLanguage
		{
			get
			{
				LanguageDataModule languageData = this.LanguageData;
				if (languageData == null)
				{
					return 0;
				}
				return languageData.GetCurrentLanguageType;
			}
		}

		public string GetInfoByID_LogError(int id)
		{
			string infoByID = this.GetInfoByID(id);
			if (!string.IsNullOrEmpty(infoByID))
			{
				HLog.LogError(string.Format("尝试寻找废弃的多语言表ID:{0},如果看到这个log，请找张宏", id));
			}
			return infoByID;
		}

		public string GetInfoByID_LogError(int id, params object[] args)
		{
			string infoByID = this.GetInfoByID(id, args);
			if (!string.IsNullOrEmpty(infoByID))
			{
				HLog.LogError(string.Format("尝试寻找废弃的多语言表ID:{0},如果看到这个log，请找张宏", id));
			}
			return infoByID;
		}

		public string GetInfoByID_LogError(LanguageType languageType, int id)
		{
			string infoByID = this.GetInfoByID(languageType, id);
			if (!string.IsNullOrEmpty(infoByID))
			{
				HLog.LogError(string.Format("尝试寻找废弃的多语言表ID:{0},如果看到这个log，请找张宏", id));
			}
			return infoByID;
		}

		private string GetInfoByID(LanguageType languageType, int id)
		{
			return this.GetInfoByID(languageType, id.ToString());
		}

		private string GetInfoByID(int id)
		{
			return this.GetInfoByID(this.LanguageData.GetCurrentLanguageType, id);
		}

		private string GetInfoByID(int id, params object[] args)
		{
			return string.Format(this.GetInfoByID(this.LanguageData.GetCurrentLanguageType, id), args);
		}

		public string GetInfoByID(LanguageType languageType, string idStr)
		{
			string text = string.Empty;
			try
			{
				if (languageType == 2)
				{
					LanguageCN_languagetable elementById = GameApp.Table.GetManager().GetLanguageCN_languagetableModelInstance().GetElementById(idStr);
					if (elementById != null)
					{
						text = this.GetInfoByID(elementById, languageType);
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					LanguageRaft_languagetable elementById2 = GameApp.Table.GetManager().GetLanguageRaft_languagetableModelInstance().GetElementById(idStr);
					if (elementById2 != null)
					{
						text = this.GetInfoByID(elementById2, languageType);
					}
					int num;
					if (string.IsNullOrEmpty(text) && int.TryParse(idStr, out num) && num > 0)
					{
						Language_languagetable elementById3 = GameApp.Table.GetManager().GetLanguage_languagetableModelInstance().GetElementById(num);
						if (elementById3 != null)
						{
							text = this.GetInfoByID(elementById3, languageType);
							HLog.LogError(string.Format("有需要迁移的多语言表ID  {0},如果看到这个log，请找亚源挪一下", num));
						}
					}
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				throw;
			}
			return text;
		}

		public string GetInfoByID(LanguageCN_languagetable table, LanguageType languageType)
		{
			string text = string.Empty;
			try
			{
				text = table.chinesesimplified;
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				throw;
			}
			return text;
		}

		public string GetInfoByID(Language_languagetable table, LanguageType languageType)
		{
			string text = string.Empty;
			try
			{
				switch (languageType)
				{
				case 0:
					text = table.english;
					goto IL_0069;
				case 1:
					break;
				case 2:
					text = table.chinesesimplified;
					goto IL_0069;
				case 3:
					text = table.chinesetraditional;
					goto IL_0069;
				case 4:
					text = table.japanese;
					goto IL_0069;
				default:
					if (languageType == 11)
					{
						text = table.korean;
						goto IL_0069;
					}
					if (languageType == 12)
					{
						text = table.vietnamese;
						goto IL_0069;
					}
					break;
				}
				text = table.english;
				IL_0069:;
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				throw;
			}
			return text;
		}

		public string GetInfoByID(LanguageRaft_languagetable table, LanguageType languageType)
		{
			string text = string.Empty;
			try
			{
				switch (languageType)
				{
				case 0:
					text = table.english;
					goto IL_00E2;
				case 1:
					text = table.spanish;
					goto IL_00E2;
				case 2:
					text = table.chinesesimplified;
					goto IL_00E2;
				case 3:
					text = table.chinesetraditional;
					goto IL_00E2;
				case 4:
					text = table.japanese;
					goto IL_00E2;
				case 5:
					text = table.french;
					goto IL_00E2;
				case 6:
					text = table.german;
					goto IL_00E2;
				case 7:
					text = table.italian;
					goto IL_00E2;
				case 9:
					text = table.russian;
					goto IL_00E2;
				case 10:
					text = table.arabic;
					goto IL_00E2;
				case 11:
					text = table.korean;
					goto IL_00E2;
				case 12:
					text = table.vietnamese;
					goto IL_00E2;
				case 13:
					text = table.thai;
					goto IL_00E2;
				case 14:
					text = table.Indonesia;
					goto IL_00E2;
				case 15:
					text = table.portuguese;
					goto IL_00E2;
				}
				text = table.english;
				IL_00E2:;
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				throw;
			}
			return text;
		}

		public string GetInfoByID(string id)
		{
			return this.GetInfoByID(this.LanguageData.GetCurrentLanguageType, id);
		}

		public string GetInfoByID(string id, params object[] args)
		{
			return string.Format(this.GetInfoByID(this.LanguageData.GetCurrentLanguageType, id), args);
		}

		public string GetInfoByID(LanguageType languageType, string id, params object[] args)
		{
			return string.Format(this.GetInfoByID(languageType, id), args);
		}

		public void CheckLanguage()
		{
			this.LanguageData.CheckLanguage();
		}

		public string GetTime(long time)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			if (timeSpan.Days > 0)
			{
				int num = timeSpan.Days;
				int num2 = timeSpan.Hours;
				if (num2 == 0)
				{
					num--;
					num2 = 23;
				}
				return this.GetInfoByID("32", new object[] { num, num2 });
			}
			if (timeSpan.Hours > 0)
			{
				int num3 = timeSpan.Hours;
				int num4 = timeSpan.Minutes;
				if (num4 == 0)
				{
					num3--;
					num4 = 59;
				}
				return this.GetInfoByID("33", new object[] { num3, num4 });
			}
			if (timeSpan.Minutes > 0)
			{
				return this.GetInfoByID("34", new object[] { timeSpan.Minutes, timeSpan.Seconds });
			}
			return this.GetInfoByID("35", new object[] { timeSpan.Seconds });
		}

		public string GetHours(int value)
		{
			return this.GetInfoByID("45", new object[] { value });
		}

		public string GetGoTime(long time)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			if (timeSpan.Days > 0)
			{
				return this.GetInfoByID("36", new object[] { timeSpan.Days });
			}
			if (timeSpan.Hours > 0)
			{
				return this.GetInfoByID("37", new object[] { timeSpan.Hours });
			}
			if (timeSpan.Minutes > 0)
			{
				return this.GetInfoByID("38", new object[] { timeSpan.Minutes });
			}
			return this.GetInfoByID("39", new object[] { timeSpan.Seconds });
		}

		public string GetAllTime(long time)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double)time).ToLocalTime();
			int year = dateTime.Year;
			int month = dateTime.Month;
			int day = dateTime.Day;
			int hour = dateTime.Hour;
			int minute = dateTime.Minute;
			int second = dateTime.Second;
			return string.Format(this.GetInfoByID("102"), new object[] { year, month, day, hour, minute, second });
		}

		public string GetEndTime(long time)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			if (timeSpan.Days > 0)
			{
				return this.GetInfoByID("29", new object[] { timeSpan.Days });
			}
			if (timeSpan.Hours > 0)
			{
				return this.GetInfoByID("30", new object[] { timeSpan.Hours });
			}
			if (timeSpan.Minutes > 0)
			{
				return this.GetInfoByID("31", new object[] { timeSpan.Minutes });
			}
			return this.GetInfoByID("31", new object[] { 1 });
		}

		public string GetLanguageShortening(LanguageType languageType)
		{
			return this.GetInfoByID(languageType, "113");
		}

		public string GetCurrentLanguageShortening()
		{
			return this.GetInfoByID(this.LanguageData.GetCurrentLanguageType, "113");
		}

		private LanguageDataModule m_data;

		private const string languageShorteningID = "113";
	}
}
