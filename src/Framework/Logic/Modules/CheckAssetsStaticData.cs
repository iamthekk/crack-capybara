using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.Modules
{
	[CreateAssetMenu]
	public class CheckAssetsStaticData : ScriptableObject
	{
		public string GetLanguageListString(LanguageType languageType, int id)
		{
			for (int i = 0; i < this.LanguageList.Count; i++)
			{
				CheckAssetsStaticData.LanguageData languageData = this.LanguageList[i];
				if (id == languageData.id)
				{
					string text;
					try
					{
						switch (languageType)
						{
						case LanguageType.English:
							text = languageData.english;
							break;
						case LanguageType.Spanish:
							text = languageData.spanish;
							break;
						case LanguageType.ChineseSimplified:
							text = languageData.chinesesimplified;
							break;
						case LanguageType.ChineseTraditional:
							text = languageData.chinesetraditional;
							break;
						case LanguageType.Japanese:
							text = languageData.japanese;
							break;
						case LanguageType.French:
							text = languageData.french;
							break;
						case LanguageType.German:
							text = languageData.german;
							break;
						case LanguageType.Italian:
							text = languageData.italian;
							break;
						case LanguageType.Dutch:
							text = languageData.dutch;
							break;
						case LanguageType.Russian:
							text = languageData.russian;
							break;
						case LanguageType.Arabic:
							text = languageData.arabic;
							break;
						case LanguageType.Korean:
							text = languageData.korean;
							break;
						case LanguageType.Vietnamese:
							text = languageData.vetnamese;
							break;
						default:
							text = languageData.english;
							break;
						}
					}
					catch (Exception ex)
					{
						HLog.LogException(ex);
						throw;
					}
					return text;
				}
			}
			return string.Empty;
		}

		public List<CheckAssetsStaticData.LanguageData> LanguageList = new List<CheckAssetsStaticData.LanguageData>();

		[Serializable]
		public class LanguageData
		{
			public int id;

			public string english;

			public string spanish;

			public string chinesesimplified;

			public string chinesetraditional;

			public string japanese;

			public string french;

			public string german;

			public string italian;

			public string dutch;

			public string russian;

			public string arabic;

			public string korean;

			public string vetnamese;
		}
	}
}
