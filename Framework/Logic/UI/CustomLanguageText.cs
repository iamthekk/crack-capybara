using System;
using Framework.EventSystem;
using Framework.Logic.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	[AddComponentMenu("UI/CustomLanguageText", 21)]
	public class CustomLanguageText : Text
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			this.OnRefresh();
			if (this.m_autoChangeLanguage && GameApp.Event != null)
			{
				GameApp.Event.RegisterEvent(2, new HandlerEvent(this.OnLanguageChanged));
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (this.m_autoChangeLanguage && GameApp.Event != null)
			{
				GameApp.Event.UnRegisterEvent(2, new HandlerEvent(this.OnLanguageChanged));
			}
		}

		private void OnLanguageChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefresh();
		}

		public void SetContent()
		{
			if (!string.IsNullOrEmpty(this.m_languageId) && GameApp.Data != null)
			{
				this.m_sourceText = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_language, this.m_languageId);
				if (this.m_autoLine)
				{
					this.m_sourceText = this.m_sourceText.Replace("\\n", "\n");
				}
				Func<bool> autoSpaceCheck = CustomLanguageText.AutoSpaceCheck;
				if (autoSpaceCheck != null && autoSpaceCheck())
				{
					this.m_sourceText = this.m_sourceText.Replace(" ", "\u00a0");
				}
				this.text = this.m_sourceText;
			}
		}

		public void OnRefresh()
		{
			if (GameApp.Data != null)
			{
				LanguageDataModule dataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
				this.m_language = dataModule.GetCurrentLanguageType;
			}
			this.SetContent();
		}

		public void ChangeLanguageID(string languageid)
		{
			this.m_languageId = languageid;
			this.OnRefresh();
		}

		[SerializeField]
		[Header("Language Setting")]
		private string m_languageId = "";

		[SerializeField]
		private LanguageType m_language = LanguageType.ChineseSimplified;

		[SerializeField]
		private bool m_autoChangeLanguage = true;

		[Header("Auto Setting")]
		public bool m_autoLine = true;

		private string m_sourceText;

		public static Func<bool> AutoSpaceCheck;
	}
}
