using System;
using Framework.Logic.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	[AddComponentMenu("UI/CustomText", 22)]
	public class CustomText : Text
	{
		public override string text
		{
			get
			{
				return this.m_Text;
			}
			set
			{
				this.m_sourceText = value;
				if (this.m_autoLine)
				{
					this.m_sourceText = this.m_sourceText.Replace("\\n", "\n");
				}
				Func<bool> autoSpaceCheck = CustomText.AutoSpaceCheck;
				if (autoSpaceCheck != null && autoSpaceCheck())
				{
					this.m_sourceText = this.m_sourceText.Replace(" ", "\u00a0");
				}
				if (!string.IsNullOrEmpty(this.m_sourceText))
				{
					if (this.m_Text != this.m_sourceText)
					{
						this.m_Text = this.m_sourceText;
						this.SetVerticesDirty();
						this.SetLayoutDirty();
					}
					return;
				}
				if (string.IsNullOrEmpty(this.m_Text))
				{
					return;
				}
				this.m_Text = "";
				this.SetVerticesDirty();
			}
		}

		public void SetText(int tableID)
		{
			this.SetText(tableID.ToString());
		}

		public void SetText(int tableID, string s1)
		{
			this.SetText(tableID.ToString(), s1);
		}

		public void SetText(int tableID, string s1, string s2)
		{
			this.SetText(tableID.ToString(), s1, s2);
		}

		public void SetText(int tableID, string s1, string s2, string s3)
		{
			this.SetText(tableID.ToString(), s1, s2, s3);
		}

		public void SetText(string tableID)
		{
			if (this.m_languageDataModule == null)
			{
				this.m_languageDataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_languageDataModule.GetCurrentLanguageType, tableID);
			this.text = infoByID;
		}

		public void SetText(string tableID, string s1)
		{
			if (this.m_languageDataModule == null)
			{
				this.m_languageDataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_languageDataModule.GetCurrentLanguageType, tableID);
			this.text = HLog.StringBuilderFormat(infoByID, s1);
		}

		public void SetText(string tableID, string s1, string s2)
		{
			if (this.m_languageDataModule == null)
			{
				this.m_languageDataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_languageDataModule.GetCurrentLanguageType, tableID);
			this.text = HLog.StringBuilderFormat(infoByID, s1, s2);
		}

		public void SetText(string tableID, string s1, string s2, string s3)
		{
			if (this.m_languageDataModule == null)
			{
				this.m_languageDataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_languageDataModule.GetCurrentLanguageType, tableID);
			this.text = HLog.StringBuilderFormat(infoByID, s1, s2, s3);
		}

		private LanguageDataModule m_languageDataModule;

		[Header("Auto Setting")]
		public bool m_autoLine = true;

		private string m_sourceText;

		public static Func<bool> AutoSpaceCheck;
	}
}
