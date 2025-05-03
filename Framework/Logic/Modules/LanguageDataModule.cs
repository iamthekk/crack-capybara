using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Framework.DataModule;
using Framework.EventSystem;
using UnityEngine;

namespace Framework.Logic.Modules
{
	public class LanguageDataModule : IDataModule
	{
		public LanguageType GetCurrentLanguageType
		{
			get
			{
				return this.m_currentLanguageType;
			}
		}

		public int GetName()
		{
			return 1;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE, new HandlerEvent(this.RefreshLanguageHandle));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE, new HandlerEvent(this.RefreshLanguageHandle));
		}

		public void Reset()
		{
		}

		private void RefreshLanguageHandle(object sender, int type, BaseEventArgs eventObject)
		{
			EventArgLanguageType eventArgLanguageType = eventObject as EventArgLanguageType;
			if (eventArgLanguageType == null)
			{
				return;
			}
			this.m_currentLanguageType = eventArgLanguageType.LanguageType;
			Utility.PlayerPrefs.SetInt("CurrentLanguageType_Key", (int)this.m_currentLanguageType);
			GameApp.Event.DispatchNow(this, 2, null);
		}

		public void CheckLanguage()
		{
			if (Utility.PlayerPrefs.HasKey("CurrentLanguageType_Key"))
			{
				this.m_currentLanguageType = (LanguageType)Utility.PlayerPrefs.GetInt("CurrentLanguageType_Key");
				return;
			}
			SystemLanguage systemLanguage = Application.systemLanguage;
			LanguageType languageType;
			if (systemLanguage <= 23)
			{
				if (systemLanguage != 6)
				{
					if (systemLanguage == 10)
					{
						languageType = LanguageType.English;
						goto IL_00DB;
					}
					switch (systemLanguage)
					{
					case 14:
						languageType = LanguageType.French;
						goto IL_00DB;
					case 15:
						languageType = LanguageType.German;
						goto IL_00DB;
					case 16:
					case 17:
					case 18:
					case 19:
						goto IL_00D9;
					case 20:
						languageType = LanguageType.Indonesian;
						goto IL_00DB;
					case 21:
						languageType = LanguageType.Italian;
						goto IL_00DB;
					case 22:
						languageType = LanguageType.Japanese;
						goto IL_00DB;
					case 23:
						languageType = LanguageType.Korean;
						goto IL_00DB;
					default:
						goto IL_00D9;
					}
				}
			}
			else
			{
				if (systemLanguage == 28)
				{
					languageType = LanguageType.Portuguese;
					goto IL_00DB;
				}
				if (systemLanguage == 30)
				{
					languageType = LanguageType.Russian;
					goto IL_00DB;
				}
				switch (systemLanguage)
				{
				case 34:
					languageType = LanguageType.Spanish;
					goto IL_00DB;
				case 35:
				case 37:
				case 38:
					goto IL_00D9;
				case 36:
					languageType = LanguageType.Thai;
					goto IL_00DB;
				case 39:
					languageType = LanguageType.Vietnamese;
					goto IL_00DB;
				case 40:
					break;
				case 41:
					languageType = LanguageType.ChineseTraditional;
					goto IL_00DB;
				default:
					goto IL_00D9;
				}
			}
			languageType = LanguageType.ChineseSimplified;
			goto IL_00DB;
			IL_00D9:
			languageType = LanguageType.English;
			IL_00DB:
			EventArgLanguageType instance = Singleton<EventArgLanguageType>.Instance;
			instance.SetData(languageType);
			GameApp.Event.DispatchNow(this, 1, instance);
		}

		public void SetLanguageNameAndAbbr(LanguageType type, string name, string abbr)
		{
			LanguageDataModule.languagedic[type] = new ValueTuple<string, string>(abbr, name);
		}

		public string GetLanguageName(LanguageType type)
		{
			ValueTuple<string, string> valueTuple;
			if (LanguageDataModule.languagedic.TryGetValue(type, out valueTuple))
			{
				return valueTuple.Item2;
			}
			return "未知语言";
		}

		public string GetLanguageAbbr(LanguageType type)
		{
			ValueTuple<string, string> valueTuple;
			if (LanguageDataModule.languagedic.TryGetValue(type, out valueTuple))
			{
				return valueTuple.Item1;
			}
			return "未知语言";
		}

		public const string LanguageTableId = "44";

		private const string CURRENT_LANGUAGETYPE_KEY = "CurrentLanguageType";

		private LanguageType m_currentLanguageType;

		[TupleElementNames(new string[] { "abbr", "name" })]
		private static Dictionary<LanguageType, ValueTuple<string, string>> languagedic = new Dictionary<LanguageType, ValueTuple<string, string>>
		{
			{
				LanguageType.English,
				new ValueTuple<string, string>("en", "English")
			},
			{
				LanguageType.ChineseSimplified,
				new ValueTuple<string, string>("zh-CN", "简体中文")
			},
			{
				LanguageType.ChineseTraditional,
				new ValueTuple<string, string>("zh-TW", "繁體中文")
			},
			{
				LanguageType.Korean,
				new ValueTuple<string, string>("ko", "한국어")
			},
			{
				LanguageType.Japanese,
				new ValueTuple<string, string>("ja", "日本語")
			},
			{
				LanguageType.French,
				new ValueTuple<string, string>("fr", "Français")
			},
			{
				LanguageType.German,
				new ValueTuple<string, string>("de", "Deutsch")
			},
			{
				LanguageType.Spanish,
				new ValueTuple<string, string>("es", "Español")
			},
			{
				LanguageType.Portuguese,
				new ValueTuple<string, string>("pt", "Português")
			},
			{
				LanguageType.Indonesian,
				new ValueTuple<string, string>("id", "Bahasa Indonesia")
			},
			{
				LanguageType.Russian,
				new ValueTuple<string, string>("ru", "русский")
			},
			{
				LanguageType.Arabic,
				new ValueTuple<string, string>("ar", "العربية")
			},
			{
				LanguageType.Thai,
				new ValueTuple<string, string>("th", "ไทย")
			},
			{
				LanguageType.Vietnamese,
				new ValueTuple<string, string>("vi", "Tiếng việt")
			},
			{
				LanguageType.Italian,
				new ValueTuple<string, string>("it", "Italiano")
			},
			{
				LanguageType.Dutch,
				new ValueTuple<string, string>("nl", "Nederlandse taal")
			}
		};
	}
}
