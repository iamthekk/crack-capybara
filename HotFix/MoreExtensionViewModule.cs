using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class MoreExtensionViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			Color color = this.imgMask.color;
			this.toAlpha = this.imgMask.color.a;
			color.a = 0f;
			this.imgMask.color = color;
			this.avatarCtrl.Init();
			this.avatarCtrl.SetEnableButton(true);
			this.avatarCtrl.OnClick = new Action<UIAvatarCtrl>(this.OnBtnAvatarClick);
			this.m_allButtons.Add(this.m_bag);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Bag", this.m_bag.transform);
			this.m_allButtons.Add(this.m_mail);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Mail", this.m_mail.transform);
			this.m_redNodePaths.Add("Main.Mail");
			this.m_allButtons.Add(this.m_mission);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Task", this.m_mission.transform);
			this.m_redNodePaths.Add("Main.Mission");
			this.m_allButtons.Add(this.m_shop);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Task", this.m_shop.transform);
			this.m_redNodePaths.Add("Main.BlackMarket");
			this.m_allButtons.Add(this.m_ranking);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Ranking", this.m_ranking.transform);
			this.m_redNodePaths.Add("Main.Ranking");
			this.m_allButtons.Add(this.m_sign);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Sign", this.m_sign.transform);
			this.m_redNodePaths.Add("Main.Sign");
			this.m_allButtons.Add(this.m_tvReward);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("TVReward", this.m_tvReward.transform);
			this.m_redNodePaths.Add("Main.TVReward");
			this.m_allButtons.Add(this.m_setting);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Setting", this.m_setting.transform);
			this.m_redNodePaths.Add("Main.Setting");
			foreach (BaseUIMainButton baseUIMainButton in this.m_allButtons)
			{
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.Init();
				}
			}
			foreach (string text in this.m_redNodePaths)
			{
				if (!string.IsNullOrEmpty(text))
				{
					RedPointController.Instance.RegRecordChange(text, new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
				}
			}
		}

		public override void OnOpen(object data)
		{
			this.openData = data as MoreExtensionViewModule.OpenData;
			Vector2 anchoredPosition = this.contentRectTransform.anchoredPosition;
			anchoredPosition.x = -this.contentRectTransform.sizeDelta.x - 50f;
			this.contentRectTransform.anchoredPosition = anchoredPosition;
			anchoredPosition.x = 0f;
			TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOAnchorPosX(this.contentRectTransform, 0f, 0.3f, false), 27);
			ShortcutExtensions46.DOFade(this.imgMask, this.toAlpha, 0.3f);
			RedPointController.Instance.RegRecordChange("Main.SelfInfo", new Action<RedNodeListenData>(this.FreshAvatarRed));
			this.UpdateView();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.openData != null)
			{
				Action onCloseCallback = this.openData.onCloseCallback;
				if (onCloseCallback != null)
				{
					onCloseCallback();
				}
				this.openData = null;
			}
			RedPointController.Instance.UnRegRecordChange("Main.SelfInfo", new Action<RedNodeListenData>(this.FreshAvatarRed));
		}

		public override void OnDelete()
		{
			this.avatarCtrl.DeInit();
			for (int i = 0; i < this.m_allButtons.Count; i++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[i];
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.DeInit();
				}
			}
			for (int j = 0; j < this.m_redNodePaths.Count; j++)
			{
				string text = this.m_redNodePaths[j];
				if (!string.IsNullOrEmpty(text))
				{
					RedPointController.Instance.UnRegRecordChange(text, new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
				}
			}
			this.m_allButtons.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnClose.m_onClick = new Action(this.OnBtnCloseClick);
			this.btnMask.m_onClick = new Action(this.OnBtnCloseClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnClose.m_onClick = null;
			this.btnMask.m_onClick = null;
		}

		private void UpdateView()
		{
			this.SetName(this.loginDataModule.NickName);
			this.FreshAvatar();
			int talentStage = GameApp.Data.GetDataModule(DataName.TalentDataModule).talentData.TalentStage;
			TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(talentStage);
			this.txtTalent.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.stepLanguageId);
		}

		private void FreshAvatar()
		{
			if (this.avatarCtrl == null)
			{
				return;
			}
			this.avatarCtrl.RefreshData(this.loginDataModule.Avatar, this.loginDataModule.AvatarFrame);
		}

		private void SetName(string name)
		{
			if (this.txtName == null)
			{
				return;
			}
			this.txtName.text = name;
		}

		private void FreshAvatarRed(RedNodeListenData redData)
		{
			if (this.redNodeAvatar == null)
			{
				return;
			}
			this.redNodeAvatar.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnBtnCloseClick()
		{
			this.imgMask.enabled = false;
			Vector2 anchoredPosition = this.contentRectTransform.anchoredPosition;
			anchoredPosition.x = -this.contentRectTransform.sizeDelta.x - 50f;
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOAnchorPosX(this.contentRectTransform, anchoredPosition.x, 0.1f, false), 1), delegate
			{
				GameApp.View.CloseView(ViewName.MoreExtensionViewModule, null);
			});
		}

		private void OnBtnAvatarClick(UIAvatarCtrl avatarCtrl)
		{
			GameApp.View.OpenView(ViewName.SelfInformationViewModule, null, 1, null, null);
		}

		private void OnRefreshRedPointChange(RedNodeListenData obj)
		{
			for (int i = 0; i < this.m_redNodePaths.Count; i++)
			{
				RedPointDataRecord record = RedPointController.Instance.GetRecord(this.m_redNodePaths[i]);
				if (record != null && record.RedPointCount > 0)
				{
					break;
				}
			}
			for (int j = 0; j < this.m_allButtons.Count; j++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[j];
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.OnRefreshAnimation();
				}
			}
		}

		public static void TryBackOpenView(ViewName viewName)
		{
			if (viewName == ViewName.MoreExtensionViewModule)
			{
				GameApp.View.OpenView(ViewName.MoreExtensionViewModule, null, 1, null, null);
			}
		}

		public CustomButton btnClose;

		public CustomButton btnMask;

		public Image imgMask;

		public RectTransform contentRectTransform;

		public UIAvatarCtrl avatarCtrl;

		public CustomText txtName;

		public GameObject talentGo;

		public CustomText txtTalent;

		public UIMainButton_Bag m_bag;

		public UIMainButton_Mail m_mail;

		public UIMainButton_Mission m_mission;

		public UIMainButton_BlackMarket m_shop;

		public UIMainButton_Ranking m_ranking;

		public UIMainButton_Sociality m_sociality;

		public UIMainButton_Sign m_sign;

		public UIMainButton_TVReward m_tvReward;

		public UIMainButton_Setting m_setting;

		public RedNodeOneCtrl redNodeAvatar;

		private List<string> m_redNodePaths = new List<string>();

		private List<BaseUIMainButton> m_allButtons = new List<BaseUIMainButton>();

		private float toAlpha;

		private MoreExtensionViewModule.OpenData openData;

		private LoginDataModule loginDataModule;

		public class OpenData
		{
			public Action onCloseCallback;
		}
	}
}
