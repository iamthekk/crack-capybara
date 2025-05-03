using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class SocialityInteractiveNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_socialityDataModule = GameApp.Data.GetDataModule(DataName.SocialityDataModule);
			this.m_button.onClick.AddListener(new UnityAction(this.OnClickButton));
		}

		protected override void OnDeInit()
		{
			this.m_button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
			this.m_interactData = null;
			this.m_socialityDataModule = null;
		}

		private void OnClickButton()
		{
			this.m_interactData.m_status = true;
			Action<GameObject> action = delegate(GameObject obj)
			{
				this.OnRefreshStatus();
			};
			int type = GameApp.Table.GetManager().GetSociality_ReportModelInstance().GetElementById(this.m_interactData.m_id)
				.type;
			if (type != 1)
			{
				return;
			}
			ReportConquerViewModule.OpenData openData = new ReportConquerViewModule.OpenData(this.m_interactData);
			GameApp.View.OpenView(ViewName.ReportConquerViewModule, openData, 1, null, action);
		}

		public void RefreshData(SocialityInteractiveData data, int index)
		{
			this.m_interactData = data;
			if (this.m_interactData == null)
			{
				return;
			}
			this.OnRefreshStatus();
			Sociality_Report elementById = GameApp.Table.GetManager().GetSociality_ReportModelInstance().GetElementById(this.m_interactData.m_id);
			if (this.m_bg != null)
			{
				this.m_bg.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.bgName);
			}
			if (this.m_iconBg != null)
			{
				this.m_iconBg.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.iconBgName);
			}
			if (this.m_icon != null)
			{
				this.m_icon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.iconName);
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.titleLanguage);
			}
			if (this.m_contentTxt != null)
			{
				Text contentTxt = this.m_contentTxt;
				LanguageManager instance = Singleton<LanguageManager>.Instance;
				string contentLanguage = elementById.contentLanguage;
				object[] array = DxxTools.GetParameterForSocialityReport(this.m_interactData.m_id, this.m_interactData.m_params).ToArray();
				contentTxt.text = instance.GetInfoByID(contentLanguage, array);
			}
			if (this.m_timeTxt != null)
			{
				this.m_timeTxt.text = Singleton<LanguageManager>.Instance.GetGoTime(this.m_interactData.m_duration);
			}
		}

		private void OnRefreshStatus()
		{
			if (this.m_interactData.m_status)
			{
				if (this.m_newTxt != null)
				{
					this.m_newTxt.SetActive(false);
				}
				if (this.m_grays != null)
				{
					this.m_grays.SetUIGray();
					return;
				}
			}
			else
			{
				if (this.m_newTxt != null)
				{
					this.m_newTxt.SetActive(true);
				}
				if (this.m_grays != null)
				{
					this.m_grays.Recovery();
				}
			}
		}

		public SocialityInteractiveData m_interactData;

		public UIGrays m_grays;

		public CustomButton m_button;

		public CustomImage m_bg;

		public CustomImage m_iconBg;

		public CustomImage m_icon;

		public GameObject m_newTxt;

		public CustomText m_nameTxt;

		public CustomText m_contentTxt;

		public CustomText m_timeTxt;

		private SocialityDataModule m_socialityDataModule;
	}
}
